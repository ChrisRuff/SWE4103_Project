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
		public string GenerateInviteKey(string className)
		{
			// Create a filter that will find the class and verify classname
			FilterDefinition<BsonDocument> query 
				= Builders<BsonDocument>.Filter.Eq("name", className);

			var foundClasses = classes.Find(query);
			if(foundClasses.CountDocuments() <= 0)
			{
				throw new System.Exception("Could not find class");
			}

			// Generate an alphanumeric invite key
			Random rand = new Random();
			string symbols = "abcdefghijklmnopqrstuvwxyz0123456789";
			string keyString = "";

			for(int i = 0; i < 8; i++)
            {
				string randomIndex = rand.Next(symbols.Length);
				keyString += symbols[randomIndex];
            }

			// Create the student document
			BsonDocument newLink = new BsonDocument
			{
				{ "className", className },
				{ "inviteLinkKey", keyString }
			};

			// Insert it into the database
			classCodes.InsertOne(newLink);

			return keyString;
		}

		public string GetInviteKey(string classCode)
		{
			// Create a filter that will find class with a given code
			FilterDefinition<BsonDocument> query
				= Builders<BsonDocument>.Filter.Eq("inviteLinkKey", classCode);

			var foundClasses = classCodes.Find(query);
			if (foundClasses.CountDocuments() <= 0)
			{
				throw new System.Exception("Invalid key.");
			}

			var foundClass = foundClasses.First();
			return foundClass["className"].ToString();
		}
	}
}
