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
		}
    
		[Fact]
		public void MakeClassAndDisableSeat()
		{
			DatabaseConnector.Connector.RemoveClass("CS2043");
			Assert.True(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));
			Assert.False(DatabaseConnector.Connector.MakeClass("CS2043", 5, 5));

			Assert.True(DatabaseConnector.Connector.DisableSeat("CS2043", 1, 1));
			Assert.False(DatabaseConnector.Connector.DisableSeat("CS2043", 1, 1));
		}

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
		public void GetProfsClasses()
		{
			
			DatabaseConnector.Connector.RemoveProf("dawn@unb.ca");
			DatabaseConnector.Connector.AddProf("Dawn", "dawn@unb.ca", "pass");
			DatabaseConnector.Connector.AddClassProf("dawn@unb.ca", "CS2043");
			DatabaseConnector.Connector.AddClassProf("dawn@unb.ca", "SWE4103");

			List<String> classes = DatabaseConnector.Connector.GetProfClassNames("dawn@unb.ca");

			Assert.True(classes[0] == "CS2043");
			Assert.True(classes[1] == "SWE4103");

			try
			{
				DatabaseConnector.Connector.GetProfClassNames("NONAME");

				// SHOULDN"T GET HERE
				Assert.False(true);
			}
			catch(System.Exception)
			{
				// SHOULD GET HERE
				Assert.True(true);
			}
		}
		[Fact]
		public void GetClassTest()
		{
			DatabaseConnector.Connector.RemoveClass("CS2043");
			DatabaseConnector.Connector.MakeClass("CS2043", 5, 5);

			DatabaseConnector.Connector.DisableSeat("CS2043", 1, 1);
			DatabaseConnector.Connector.DisableSeat("CS2043", 1, 2);
			DatabaseConnector.Connector.ReserveSeat("CS2043", 1, 2);
			DatabaseConnector.Connector.ReserveSeat("CS2043", 1, 1);

			ClassDTO MrClassy = DatabaseConnector.Connector.GetClass("CS2043");
			Assert.True(MrClassy.height == 5);
			Assert.True(MrClassy.width == 5);
			Assert.True(MrClassy.className == "CS2043");

			Assert.True(MrClassy.ReservedSeats[0].x == 1 && MrClassy.ReservedSeats[0].y == 2);
			Assert.True(MrClassy.ReservedSeats[1].x == 1 && MrClassy.ReservedSeats[1].y == 1);
			Assert.True(MrClassy.DisabledSeats[0].x == 1 && MrClassy.DisabledSeats[0].y == 1);
			Assert.True(MrClassy.DisabledSeats[1].x == 1 && MrClassy.DisabledSeats[1].y == 2);
			
			DatabaseConnector.Connector.RemoveClass("CS2043");
			try
			{
				// Should throw exception
				DatabaseConnector.Connector.GetClass("CS2043");
				Assert.True(false);
			}
			catch(System.Exception)
			{

			}
		}
	}
}
