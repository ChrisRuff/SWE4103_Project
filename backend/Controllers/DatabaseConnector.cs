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

		public bool AddStudent(string name, string[] classes, string email)
		{
			// Create an array of all the recorded absents the student has
			// (this is also a list of all the classes the student is in)
			BsonArray absents = new BsonArray(classes.Length); 
			for(int i = 0; i < classes.Length; ++i)
			{
				absents.Add(new BsonDocument{ {"name", classes[i]}, {"num", 0} });
			}

			// Create the student document
			BsonDocument newStudent = new BsonDocument
			{
				{ "name", name },
				{ "absents", absents },
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

			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("absents", new BsonDocument{{"name", className}, {"num", 0}});

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

		public bool IsAbsent(string studentEmail, string className)
		{
			// Create a filter that finds the student and selects the 
			// correct course within the absents array
			var filter = Builders<BsonDocument>.Filter;
			FilterDefinition<BsonDocument> query = 
				filter.And(filter.Eq("email", studentEmail), 
				filter.Eq("absents.name", className));

			// Increment the value found by the query by one
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Inc("absents.$.num", 1);

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
