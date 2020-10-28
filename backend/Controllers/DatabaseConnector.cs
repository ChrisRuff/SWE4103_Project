using System;
using System.Collections.Generic;

using backend.Controllers.Models;


using MongoDB.Driver;
using MongoDB.Bson;


// Heroku login:
// pass: SWETeam2!
namespace backend
{
	// Check it <3 
	// https://mongodb.github.io/mongo-csharp-driver/2.10/getting_started/quick_tour/
	public partial class DatabaseConnector
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

		public void Wipe()
		{
			client.DropDatabase("attendance");
			students = database.GetCollection<BsonDocument>("students");
			classes = database.GetCollection<BsonDocument>("classes");
			profs = database.GetCollection<BsonDocument>("profs");
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
