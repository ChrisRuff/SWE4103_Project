using System;
using System.Collections.Generic;
using backend.Controllers.Models;

using MongoDB.Driver;
using MongoDB.Bson;


namespace backend
{
	// Check it <3 
	// https://mongodb.github.io/mongo-csharp-driver/2.10/getting_started/quick_tour/
	public partial class DatabaseConnector
	{
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
				{ "notificationFreq", 3 },
				{ "dSeats", new BsonArray{}},
				{ "rSeats", new BsonArray{}},
				{ "aSeats", new BsonArray{}}
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
		public List<String> GetAllClasses()
		{
			List<String> cls = new List<String>();
			var allClasses = classes.AsQueryable();
			foreach(var x in allClasses)
			{
				cls.Add(x["name"].ToString());
			}
			return cls;
		}
		public bool AccessibleSeat(string className, int x, int y)
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
			foreach(var i in foundClass["aSeats"].AsBsonArray)
			{
				if(i["x"] == x && i["y"] == y)
					return false;
			}
			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("aSeats", new BsonDocument{{"x", x}, {"y", y}});
			classes.UpdateOne(foundClass, update);

			return true;
		}
		public bool RemoveClass(string className)
		{
			// Create a filter that will find the class with the given email
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
		public ClassDTO GetClass(string className)
		{
			// Create a filter that will find the class with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);

			// If no classes were found throw exception
			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				Console.WriteLine("Could not find class");
				throw new System.Exception("Could not find class");
			}

			var foundClass = foundClasses.First();

			// Create a ClassDTO to hand back to caller
			ClassDTO MrClassy = new ClassDTO();
			MrClassy.className = className;
			MrClassy.height = foundClass["height"].ToInt32();
			MrClassy.width = foundClass["width"].ToInt32();
			MrClassy.notificationFreq = foundClass["notificationFreq"].ToInt32();

			// Get disabled seats
			var dseats = foundClass["dSeats"].AsBsonArray;
			MrClassy.DisabledSeats = new SeatDTO[dseats.Count];
			for(int i = 0; i < dseats.Count; ++i)
			{
				var s = dseats[i];
				SeatDTO seat = new SeatDTO();
				seat.x = s["x"].ToInt32();
				seat.y = s["y"].ToInt32();
				MrClassy.DisabledSeats[i] = seat;
			}

			// Get reserved seats
			var rseats = foundClass["rSeats"].AsBsonArray;
			MrClassy.ReservedSeats = new SeatDTO[rseats.Count];
			for(int i = 0; i < rseats.Count; ++i)
			{
				var s = rseats[i];
				SeatDTO seat = new SeatDTO();
				seat.x = s["x"].ToInt32();
				seat.y = s["y"].ToInt32();
				MrClassy.ReservedSeats[i] = seat;
			}

			// Get accessible seats
			var aseats = foundClass["aSeats"].AsBsonArray;
			MrClassy.AccessibleSeats = new SeatDTO[aseats.Count];
			for(int i = 0; i < aseats.Count; ++i)
			{
				var s = aseats[i];
				SeatDTO seat = new SeatDTO();
				seat.x = s["x"].ToInt32();
				seat.y = s["y"].ToInt32();
				MrClassy.AccessibleSeats[i] = seat;
			}
			return MrClassy;
		}
		public bool WipeSeats(string name)
		{
			// Create a filter that will find the class with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", name);
			
			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				return false;
			}

			var foundClass = foundClasses.First();

			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Set("aSeats", new BsonArray{});
			classes.UpdateOne(query, update);

			update = 
				Builders<BsonDocument>.Update.Set("rSeats", new BsonArray{});
			classes.UpdateOne(query, update);

			update = 
				Builders<BsonDocument>.Update.Set("dSeats", new BsonArray{});
			classes.UpdateOne(query, update);
			return true;
		}
		public bool ChangeFreq(string className, int newF)
		{
			// Create a filter that will find the class with the given name
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);


			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				return false;
			}
			var foundClass = foundClasses.First();

			// Create a update routine that will update a class 
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Set("notificationFreq", newF);

			classes.UpdateOne(foundClass, update);

			return true;
		}
	}
}
