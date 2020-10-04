using System;
using Xunit;
using backend;

namespace test
{
	public class DatabaseConnecterTests
	{
		[Fact]
		public void AddRemoveStudents()
		{
			// Ensure there aren't any test coles out there 
			while(DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca"));

			// Add test student
			DatabaseConnector.Connector.AddStudent(
					"Stephen Cole - Test", new string[]{"SWE4103"}, "scole_test@unb.ca"); 
			
			// Ensure that it can be removed and it returns successful only on first attempt
			bool succeeded1 = DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");
			bool succeeded2 = DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");

			Assert.True(succeeded1);
			Assert.False(succeeded2);
		}

		[Fact]
		public void NewClassAndAbsent()
		{
			// Ensure there aren't any test coles out there 
			while(DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca"));

			// Add test student
			DatabaseConnector.Connector.AddStudent(
					"Stephen Cole - Test", new string[]{"SWE4103"}, "scole_test@unb.ca"); 

			// Add a new class (CS2043) to Stephens courses
			Assert.True(DatabaseConnector.Connector.AddClass("scole_test@unb.ca", "CS2043"));

			// Make Stephen miss a class in CS2043
			Assert.True(DatabaseConnector.Connector.IsAbsent("scole_test@unb.ca", "CS2043"));

			// Clean up database for next tests
			DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");
			
		}

		[Fact]
		public void AddSeatAndRead()
		{
			// Ensure there aren't any test coles out there 
			while(DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca"));

			// Add test student
			DatabaseConnector.Connector.AddStudent(
					"Stephen Cole - Test", new string[]{"SWE4103"}, "scole_test@unb.ca"); 

			int[] seat;

			// If a student doesn't have a seat return -1, -1 with no exception
			try 
			{
				seat = DatabaseConnector.Connector.GetSeat("scole_test@unb.ca", "SWE4103");
			} 
			catch (System.Exception) { Assert.True(false); return; }

			Assert.True(seat[0] == -1 && seat[1] == -1); 

			// Add a seat for a student
			Assert.True(DatabaseConnector.Connector.AddSeat("scole_test@unb.ca", "SWE4103", 5, 4));
			try 
			{
				 seat = DatabaseConnector.Connector.GetSeat("scole_test@unb.ca", "SWE4103");
			} 
			catch (System.Exception) { Assert.True(false); return; }

			// Assert that result is stored
			Assert.True(seat[0] == 5 && seat[1] == 4);

			// Reassign a seat for a student
			Assert.True(DatabaseConnector.Connector.AddSeat("scole_test@unb.ca", "SWE4103", 55, 45));
			try 
			{
				 seat = DatabaseConnector.Connector.GetSeat("scole_test@unb.ca", "SWE4103");
			} 
			catch (System.Exception) { Assert.True(false); return; }

			// Assert that result is stored
			Assert.True(seat[0] == 55 && seat[1] == 45);

			// Ensure that if a class a student isn't in is searched you get an exception
			bool caught = false;
			try 
			{
				 seat = DatabaseConnector.Connector.GetSeat("scole_test@unb.ca", "CLASS_THAT_DOESN\'T EXIST");
			} 
			catch (System.Exception) 
			{
				caught = true;
			}
			Assert.True(caught);
		}
	}
}
