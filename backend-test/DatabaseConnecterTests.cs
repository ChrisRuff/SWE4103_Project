using System;
using System.Collections.Generic;
using Xunit;
using backend;
using backend.Controllers.Models;

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
			Assert.True(DatabaseConnector.Connector.AddStudent(
					"Stephen Cole - Test", "scole_test@unb.ca", "pass")); 
			
			// Ensure that it can be removed and it returns successful only on first attempt
			bool succeeded1 = DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");
			bool succeeded2 = DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");

			Assert.True(succeeded1);
			Assert.False(succeeded2);
		}
		[Fact]
		public void AddRemoveProfs()
		{
			// Ensure there aren't any test coles out there 
			while(DatabaseConnector.Connector.RemoveProf("dawn@unb.ca"));

			// Add test student
			Assert.True(DatabaseConnector.Connector.AddProf(
					"Dawn", "dawn@unb.ca", "pass")); 
			
			// Ensure that it can be removed and it returns successful only on first attempt
			bool succeeded1 = DatabaseConnector.Connector.RemoveProf("dawn@unb.ca");
			bool succeeded2 = DatabaseConnector.Connector.RemoveProf("dawn@unb.ca");

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
					"Stephen Cole - Test", "scole_test@unb.ca", "pass"); 

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
					"Stephen Cole - Test", "scole_test@unb.ca", "pass"); 

			// Ensure double adding a student returns false
			Assert.False(DatabaseConnector.Connector.AddStudent(
					"Stephen Cole - Test", "scole_test@unb.ca", "pass")); 
			Assert.True(DatabaseConnector.Connector.AddClass("scole_test@unb.ca", "SWE4103"));

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
			DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");
		}
    

		[Fact]
		public void MakeClassAndReserve()
		{
			DatabaseConnector.Connector.RemoveClass("CS2043");

			Assert.True(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));
			Assert.False(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));

			Assert.True(DatabaseConnector.Connector.ReserveSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.ReserveSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.ReserveSeat("CS2043", 10, 10));
			Assert.False(DatabaseConnector.Connector.ReserveSeat("CS243", 10, 10));

			Assert.True(DatabaseConnector.Connector.RemoveClass("CS2043"));

		}
		[Fact]
		public void MakeClassAndDisable()
		{
			DatabaseConnector.Connector.RemoveClass("CS2043");
			DatabaseConnector.Connector.RemoveClass("CS243");

			Assert.True(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));
			Assert.False(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));

			Assert.True(DatabaseConnector.Connector.DisableSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.DisableSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.DisableSeat("CS2043", 10, 10));
			Assert.False(DatabaseConnector.Connector.DisableSeat("CS243", 10, 10));

			Assert.True(DatabaseConnector.Connector.RemoveClass("CS2043"));
		}
		[Fact]
		public void MakeClassAndAccessible()
		{
			DatabaseConnector.Connector.RemoveClass("CS2043");

			Assert.True(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));
			Assert.False(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));

			Assert.True(DatabaseConnector.Connector.AccessibleSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.AccessibleSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.AccessibleSeat("CS2043", 10, 10));
			Assert.False(DatabaseConnector.Connector.AccessibleSeat("CS243", 10, 10));

			Assert.True(DatabaseConnector.Connector.RemoveClass("CS2043"));

		}

		[Fact]
		public void RemoveClass()
		{
			DatabaseConnector.Connector.MakeClass("CS2042", 5, 5);
			DatabaseConnector.Connector.AddProf("TESTPROF123", "TESTPROF123@123.TEST", "PASS");
			DatabaseConnector.Connector.AddClassProf("TESTPROF123@123.TEST", "CS2042");

			Assert.True(DatabaseConnector.Connector.GetProfClassNames("TESTPROF123@123.TEST")[0] == "CS2042");
			Assert.True(DatabaseConnector.Connector.RemoveClass("CS2042"));
			DatabaseConnector.Connector.GetProfClassNames("TESTPROF123@123.TEST").ForEach(Console.WriteLine);
			Assert.True(DatabaseConnector.Connector.GetProfClassNames("TESTPROF123@123.TEST").Count == 0);
		}

		public void SetMandatory()
		{
			DatabaseConnector.Connector.RemoveClass("CS2043");
			Assert.False(DatabaseConnector.Connector.setMandatory("CS2043", true));

			DatabaseConnector.Connector.MakeClass("CS2043", 5, 5);
			Assert.True(DatabaseConnector.Connector.GetClass("CS2043").mandatory == false);
			Assert.True(DatabaseConnector.Connector.setMandatory("CS2043", true));
			Assert.True(DatabaseConnector.Connector.GetClass("CS2043").mandatory == true);
			Assert.True(DatabaseConnector.Connector.setMandatory("CS2043", false));
			Assert.True(DatabaseConnector.Connector.GetClass("CS2043").mandatory == false);
			DatabaseConnector.Connector.RemoveClass("CS2043");
		}

		[Fact]
		public void ProfLogin()
		{
			DatabaseConnector.Connector.AddProf("Tester", "test@gmail.com", "password");
			DatabaseConnector.Connector.RemoveProf("wrong@gmail.com");
			Assert.True(DatabaseConnector.Connector.CheckPassProf("test@gmail.com", "password"));
			Assert.True(DatabaseConnector.Connector.CheckPassProf("test@gmail.com", "password"));
			Assert.False(DatabaseConnector.Connector.CheckPassProf("test@gmail.com", "passwor"));
			Assert.False(DatabaseConnector.Connector.CheckPassProf("wrong@gmail.com", "passwor"));
		}

		[Fact]
		public void StudentLogin()
		{
			DatabaseConnector.Connector.AddStudent("Tester", "test@gmail.com", "password");
			DatabaseConnector.Connector.RemoveStudent("wrong@gmail.com");
			Assert.True(DatabaseConnector.Connector.CheckPassStudent("test@gmail.com", "password"));
			Assert.True(DatabaseConnector.Connector.CheckPassStudent("test@gmail.com", "password"));
			Assert.False(DatabaseConnector.Connector.CheckPassStudent("test@gmail.com", "passwor"));
			Assert.False(DatabaseConnector.Connector.CheckPassStudent("wrong@gmail.com", "passwor"));
			DatabaseConnector.Connector.RemoveStudent("test@gmail.com");
		}

		[Fact]
		public void GetStudent()
		{
			// Ensure there aren't any test coles out there 
			while(DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca"));

			// Add test student
			DatabaseConnector.Connector.AddStudent(
					"Stephen Cole - Test", "scole_test@unb.ca", "pass"); 
			DatabaseConnector.Connector.AddClass("scole_test@unb.ca", "CS2043");
			DatabaseConnector.Connector.AddClass("scole_test@unb.ca", "SWE4103");
			DatabaseConnector.Connector.AddSeat("scole_test@unb.ca", "CS2043", 1, 1);
			DatabaseConnector.Connector.AddSeat("scole_test@unb.ca", "SWE4103", 2, 5);

			var stephen = DatabaseConnector.Connector.GetStudent("scole_test@unb.ca");

			Assert.True(stephen.studentName == "Stephen Cole - Test");
			Assert.True(stephen.email == "scole_test@unb.ca");
			Assert.True(stephen.pass == "pass");

			Assert.True(stephen.classes[0].className == "CS2043");
			Assert.True(stephen.classes[0].seat.x == 1);
			Assert.True(stephen.classes[0].seat.y == 1);
			Assert.True(stephen.classes[1].className == "SWE4103");
			Assert.True(stephen.classes[1].seat.x == 2);
			Assert.True(stephen.classes[1].seat.y == 5);
			
			DatabaseConnector.Connector.RemoveStudent("scole_test@unb.ca");
		}
	}
}
