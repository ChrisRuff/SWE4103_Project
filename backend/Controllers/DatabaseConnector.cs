using System;
using System.Collections.Generic;
using backend.Controllers.Models;

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

		private MongoClient client;
		private IMongoDatabase database;
		private IMongoCollection<BsonDocument> students;
		private IMongoCollection<BsonDocument> classes;
		private IMongoCollection<BsonDocument> profs;

		private DatabaseConnector()
		{
			// mongo "mongodb+srv://cluster0.hzsao.mongodb.net/SWE4103_Project" --username admin
			client = new MongoClient("mongodb+srv://admin:admin@cluster0.hzsao.mongodb.net/SWE4103_Project?retryWrites=true&w=majority");

			// Create connections to the various tables we'll need
			database = client.GetDatabase("attendance");
			students = database.GetCollection<BsonDocument>("students");
			profs = database.GetCollection<BsonDocument>("profs");
			classes = database.GetCollection<BsonDocument>("classes");
		}

		public bool CheckPass(string name, string pass)
		{
			return true;
		}

		public bool AddStudent(string name, string email, string pass)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("email", email);
			if(students.Find(query).CountDocuments() > 0)
			{
				return false;
			}


			// Create the student document
			BsonDocument newStudent = new BsonDocument
			{
				{ "name", name },
				{ "email", email },
				{ "pass", pass },
				{ "classes", new BsonArray{}}
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
		public StudentDTO GetStudent(string email)
		{
			// Create a filter that finds the student
			FilterDefinition<BsonDocument> query = 
				Builders<BsonDocument>.Filter.Eq("email", email);

			var studentsFound = students.Find(query);
			if(studentsFound.CountDocuments() <= 0)
			{
				return null;
			}
			var student = studentsFound.First();
			StudentDTO data = new StudentDTO();
			data.studentName = student["name"].ToString();
			data.email = student["email"].ToString();
			data.pass = student["pass"].ToString();
			ClassDTO[] classes = new ClassDTO[student["classes"].AsBsonArray.Count];
			int idx = 0;
			foreach(var i in student["classes"].AsBsonArray)
			{
				var seat = new SeatDTO();
				seat.x = i[1]["x"].ToInt32();
				seat.y = i[1]["y"].ToInt32();
				classes[idx].className = i[0].ToString();
				classes[idx].seat = seat;
				idx++;
			}
			return data;
		}

		public void Wipe()
		{
			client.DropDatabase("attendance");
			students = database.GetCollection<BsonDocument>("students");
			classes = database.GetCollection<BsonDocument>("classes");
		}

		public bool MakeClass(string name, int width, int height)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", name);
			if(classes.Find(query).CountDocuments() > 0)
			{
				return false;
			}

			// Create the student document
			BsonDocument newClass = new BsonDocument
			{
				{ "name", name },
				{ "width", width },
				{ "height", height },
				{ "dSeats", new BsonArray{}},
				{ "rSeats", new BsonArray{}},
			};

			// Insert it into the database
			classes.InsertOne(newClass);
			return true;
		}

		public bool DisableSeat(string className, int x, int y)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);


			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				return false;
			}
			var foundClass = foundClasses.First();
			if(foundClass["width"] < x || foundClass["height"] < y)
			{
				return false;
			}
			foreach(var i in foundClass["dSeats"].AsBsonArray)
			{
				if(i["x"] == x && i["y"] == y)
					return false;
			}
			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("dSeats", new BsonDocument{{"x", x}, {"y", y}});
			classes.UpdateOne(foundClass, update);

			return true;
		}
		public bool ReserveSeat(string className, int x, int y)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);


			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				return false;
			}
			var foundClass = foundClasses.First();
			if(foundClass["width"] < x || foundClass["height"] < y)
			{
				return false;
			}
			foreach(var i in foundClass["rSeats"].AsBsonArray)
			{
				if(i["x"] == x && i["y"] == y)
					return false;
			}
			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("rSeats", new BsonDocument{{"x", x}, {"y", y}});
			classes.UpdateOne(foundClass, update);

			return true;
		}
		public bool RemoveClass(string className)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);
			
			// Actually delete the student if query finds result
			if(classes.Find(query).CountDocuments() > 0)
			{
				classes.DeleteOne(query);
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
