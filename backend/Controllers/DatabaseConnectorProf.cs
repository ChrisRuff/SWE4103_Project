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
		public bool CheckPassProf(string email, string pass)
		{
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("email", email);
			var qResults = profs.Find(query);
			if(qResults.CountDocuments() <= 0)
			{
				return false;
			}
			var prof = qResults.First();
			if(prof["pass"] == pass)
			{
				return true;
			}
			else 
			{
				return false;
			}
		}
		public bool AddProf(string name, string email, string pass)
		{
			// Create a filter that will find the prof with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("email", email);
			if(profs.Find(query).CountDocuments() > 0)
			{
				return false;
			}


			// Create the Prof document
			BsonDocument newProf = new BsonDocument
			{
				{ "name", name },
				{ "email", email },
				{ "pass", pass },
				{ "classes", new BsonArray{}}
			};

			// Insert it into the database
			profs.InsertOne(newProf);
			return true;
		}
		public bool RemoveProf(string email)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("email", email);

			// Actually delete the student if query finds result
			if(profs.Find(query).CountDocuments() > 0)
			{
				profs.DeleteOne(query);
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool AddClassProf(string email, string className)
		{
			// Create a filter that will find the student with the given email
			FilterDefinition<BsonDocument> query = 
				Builders<BsonDocument>.Filter.Eq("email", email);

			var classDoc = new BsonDocument{{"name", className}};

			// Create a update routine that will add a class 
			// (with absents field to the student document)
			UpdateDefinition<BsonDocument> update = 
				Builders<BsonDocument>.Update.AddToSet("classes", classDoc);

			// Actually update the database if query finds result
			if(profs.Find(query).CountDocuments() > 0)
			{
				profs.UpdateOne(query, update);
				return true;
			}
			else
			{
				return false;
			}
		}
		public List<String> GetProfClassNames(string email)
		{
			List<String> classes = new List<String>();
			FilterDefinition<BsonDocument> query = 
				Builders<BsonDocument>.Filter.Eq("email", email);

			var prof = profs.Find(query);
			if(prof.CountDocuments() <= 0)
			{
				throw new System.Exception("Could not find prof");
			}
			var profClasses = prof.First()["classes"].AsBsonArray;
			foreach(var x in profClasses)
			{
				classes.Add(x["name"].ToString());
			}
			return classes;
		}
	}
}
