using System;
using System.Collections.Generic;
using System.Collections;
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
				{ "mandatory", false },
				{ "dSeats", new BsonArray{}},
				{ "rSeats", new BsonArray{}},
				{ "aSeats", new BsonArray{}},
				{ "oSeats", new BsonArray{}},
				{ "roster", new BsonArray{}}
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
		public bool OpenSeat(string className, int x, int y)
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
			foreach(var i in foundClass["oSeats"].AsBsonArray)
			{
				if(i["x"] == x && i["y"] == y)
					return false;
			}
			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("oSeats", new BsonDocument{{"x", x}, {"y", y}});
			classes.UpdateOne(foundClass, update);

			return true;
		}
		public bool ReserveSeat(string className, int x, int y, string studentEmail)
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
			var studentName = Connector.GetStudent(studentEmail).name;
			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("rSeats", new BsonDocument{{"x", x}, 
						{"y", y}, {"name", studentName}, {"email", studentEmail}});
			classes.UpdateOne(foundClass, update);

			return true;
		}
		public bool UnReserveSeat(string className, int x, int y)
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
			var seats = new BsonArray();
			var worked = false;
			foreach(var i in foundClass["rSeats"].AsBsonArray)
			{
				if(i["x"] != x && i["y"] != y)
				{
					seats.Add(i);
				}
				else
				{
					worked = true;
				}
			}
			if(!worked)
			{
				return false;
			}

			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Set("rSeats", seats);
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

			FilterDefinition<BsonDocument> classQuery 
				= Builders<BsonDocument>.Filter.Eq("classes.name", className);
			
			// Actually delete the student if query finds result
			if(classes.Find(query).CountDocuments() > 0)
			{
				classes.DeleteOne(query);
				if(profs.Find(classQuery).CountDocuments() > 0)
				{
					UpdateDefinition<BsonDocument> update = 
						Builders<BsonDocument>.Update.Pull("classes", new BsonDocument{{"name", className}});

					profs.UpdateOne(classQuery, update);
				}
				if(students.Find(classQuery).CountDocuments() > 0)
				{
					UpdateDefinition<BsonDocument> update = 
						Builders<BsonDocument>.Update.Pull("classes", new BsonDocument{{"name", className}});

					students.UpdateOne(classQuery, update);
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool setMandatory(string className, bool mandatory)
		{
			// Create a filter that will find the class with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);
			
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Set("mandatory", mandatory);

			if(classes.Find(query).CountDocuments() <= 0)
			{
				return false;
			}
			else
			{
				classes.UpdateOne(query, update);
				return true;
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
				throw new System.Exception("Could not find class");
			}

			var foundClass = foundClasses.First();

			// Create a ClassDTO to hand back to caller
			ClassDTO MrClassy = new ClassDTO();
			MrClassy.className = className;
			MrClassy.height = foundClass["height"].ToInt32();
			MrClassy.width = foundClass["width"].ToInt32();
			MrClassy.notificationFreq = foundClass["notificationFreq"].ToInt32();
			
			var roster = foundClass["roster"].AsBsonArray;
			if(roster.Count != 0)
			{
				MrClassy.roster = new StudentRosterDTO[roster.Count];
				for(int i = 0; i < roster.Count; ++i)
				{
					StudentRosterDTO rStudent = new StudentRosterDTO();
					rStudent.studentName = (string)roster[i]["name"];
					var daysMissed = roster[i]["daysMissed"].AsBsonArray;
					rStudent.daysMissed = new string[daysMissed.Count];
					for(int j = 0; j < daysMissed.Count; j++)
						rStudent.daysMissed[j] = (string)daysMissed[j];
					
					MrClassy.roster[i] = rStudent;
				}

			}
			
			try { MrClassy.mandatory = foundClass["mandatory"].ToBoolean(); } catch(Exception e) {}

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
				seat.email = s["email"].ToString();
				seat.name = s["name"].ToString();
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

			// Get open seats
			var oseats = foundClass["oSeats"].AsBsonArray;
			MrClassy.OpenSeats = new SeatDTO[oseats.Count];
			for(int i = 0; i < oseats.Count; ++i)
			{
				var s = oseats[i];
				SeatDTO seat = new SeatDTO();
				seat.x = s["x"].ToInt32();
				seat.y = s["y"].ToInt32();
				MrClassy.OpenSeats[i] = seat;
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

			update = 
				Builders<BsonDocument>.Update.Set("oSeats", new BsonArray{});
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
		public bool AddAttendance(string className, string date, string[] students)
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

			var roster = foundClass["roster"].AsBsonArray;

			// if there are no students in the roster create a new roster
			if( roster.Count == 0)
			{
				BsonArray newRoster = new BsonArray();
				for(int i = 0; i < students.Length; i++)
				{
					BsonArray  daysMissed = new BsonArray { BsonValue.Create(date) };
					BsonDocument bStudent = new BsonDocument{{"name", students[i]}, {"daysMissed", daysMissed}};
					newRoster.Add(bStudent);
				}

				UpdateDefinition<BsonDocument> update =
					Builders<BsonDocument>.Update.Set("roster", newRoster);

				classes.UpdateOne(foundClass, update);


			}
			else
			{

				//Deal with students who have missed a class before
				for(int i = 0; i < students.Length; i++)
				{
					for(int j = 0; j < roster.Count; j++)
					{

						if(students[i] == roster[j]["name"])
						{	
							if(roster[j]["daysMissed"].AsBsonArray.Contains(date) == false)
							{
								roster[j]["daysMissed"].AsBsonArray.Add(date);
							}
							students[i] = "included";
						}
						
					}
				}

				//Deal with new delinquents
				for(int i = 0; i < students.Length; i++)
				{
					if(students[i] != "included")
					{
						BsonArray  daysMissed = new BsonArray { BsonValue.Create(date) };
						BsonDocument bStudent = new BsonDocument{{"name", students[i]}, {"daysMissed", daysMissed}};
						roster.Add(bStudent);
					}
				}

				// Replace current roster with new roster
				foundClass["roster"] = roster;
				classes.FindOneAndReplace(query, foundClass);
			}
			return true;
		}
    
		public bool EditClass(string className, int width, int height)
		{
			// Create a filter that will find the class with the given name
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);


			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				return false;
			}

			// Create a update routine that will update a class 
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.Set("height", height);
			classes.UpdateOne(query, update);

			update = 
				Builders<BsonDocument>.Update.Set("width", width);
			classes.UpdateOne(query, update);

			return WipeSeats(className);
		}
	}
}
