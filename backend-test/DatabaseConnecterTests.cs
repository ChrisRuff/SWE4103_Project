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
	}
}
