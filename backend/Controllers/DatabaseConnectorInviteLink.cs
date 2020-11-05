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
		public String GenerateInviteKey(string className)
		{
			// Create a filter that will find the student with the given email
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
				keyString += symbols[rand.Next(symbols.Length)];
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
	}
}
