using System;
using System.Collections.Generic;

using MongoDB.Driver;
using MongoDB.Bson;


namespace backend
{
	// Check it <3 
	// https://mongodb.github.io/mongo-csharp-driver/2.10/getting_started/quick_tour/
	public class DatabaseConnector
	{
		// Connector is a singleton because we only want one 
		// instance of the database connection at a time
		private static DatabaseConnector connector = null;

		private IMongoDatabase database;
		private IMongoCollection<BsonDocument> classes;
		private IMongoCollection<BsonDocument> profs;
		private IMongoCollection<BsonDocument> students;

		private DatabaseConnector()
		{
			// TODO: Hook up to a atlas mongo db
			var mongo = new MongoClient();

			// Create connections to the various tables we'll need
			database = mongo.GetDatabase("attendance");
			classes = database.GetCollection<BsonDocument>("classes");
			profs = database.GetCollection<BsonDocument>("profs");
			students = database.GetCollection<BsonDocument>("students");
		}

		public bool AddStudent(string name, string[] classNames, string email)
		{
			// Create an array of all the classes the student has
			BsonArray classes = new BsonArray(classNames.Length); 
			for(int i = 0; i < classNames.Length; ++i)
			{
				classes.Add(new BsonDocument{ {"name", classNames[i]}, {"absents", 0}, 
						{ "seat", new BsonDocument{{"x", -1}, {"y", -1}}}});
			}

			// Create the student document
			BsonDocument newStudent = new BsonDocument
			{
				{ "name", name },
				{ "classes", classes },
				{ "email", email }
			};

			// Insert it into the database
			students.InsertOne(newStudent);
			return true;
		}

		public bool RemoveStudent(string email)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("email", email);

			// Actually delete the student if query finds result
			if(students.Find(query).CountDocuments() > 0)
			{
				students.DeleteOne(query);
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool AddClass(string studentEmail, string className)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query = 
				Builders<BsonDocument>.Filter.Eq("email", studentEmail);

			var classDoc = new BsonDocument{{"name", className}, {"absents", 0}};
			classDoc.Add("seat", new BsonDocument{{"x", -1}, {"y", -1}});

			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("classes", classDoc);

			new BsonDocument{{"a", 2}};
			// Actually update the database if query finds result
			if(students.Find(query).CountDocuments() > 0)
			{
				students.UpdateOne(query, update);
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool AddSeat(string studentEmail, string className, int x, int y)
		{
			// Create a filter that finds the student and selects the correct class
			var filter = Builders<BsonDocument>.Filter;
			FilterDefinition<BsonDocument> query = 
				filter.And(filter.Eq("email", studentEmail), 
				filter.Eq("classes.name", className));

			// Set the value of the seat to x and y
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Set("classes.$.seat", new BsonDocument{{"x", x}, {"y", y}});

			// Actually update the database if query finds result
			if(students.Find(query).CountDocuments() > 0)
			{
				students.UpdateOne(query, update);
				return true;
			}
			else
			{
				return false;
			}
		}

		public int[] GetSeat(string studentEmail, string className)
		{
			// Create a filter that finds the student
			FilterDefinition<BsonDocument> query = 
				Builders<BsonDocument>.Filter.Eq("email", studentEmail);

			// Find the student and search for the right class
			var found = students.Find(query).First()["classes"].AsBsonArray;
			foreach(var i in found)
			{
				if(i["name"] == className)
				{
					// If you find the right class return the seat
					return new int[]{(int)i["seat"]["x"], (int)i["seat"]["y"]};
				}
			}

			// If you can't find the class throw an error
			// TODO: Custom exception
			throw new System.Exception("Could not find seat");
		}

		public bool IsAbsent(string studentEmail, string className)
		{
			// Create a filter that finds the student and selects the 
			// correct course within the classes array
			var filter = Builders<BsonDocument>.Filter;
			FilterDefinition<BsonDocument> query = 
				filter.And(filter.Eq("email", studentEmail), 
				filter.Eq("classes.name", className));

			// Increment the value found by the query by one
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Inc("classes.$.absents", 1);

			// Actually do the work if it exists
			if(students.Find(query).CountDocuments() > 0)
			{
				students.UpdateOne(query, update);
				return true;
			}
			else
			{
				return false;
			}
		}


		public static DatabaseConnector Connector
		{
			// If there exists an active communication then use it
			// otherwise make a connection
			get
			{
				if(connector == null)
				{
					connector = new DatabaseConnector();
				}
				return connector;
			}
		}
	}
}
