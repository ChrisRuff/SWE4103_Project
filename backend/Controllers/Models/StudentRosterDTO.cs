using System;                                                                                                                            
using System.Collections; 
using System.Collections.Generic; 

namespace backend.Controllers.Models
{
	public class StudentRosterDTO
	{
		public string[] studentNames { get; set; }
		public string studentName { get; set; } // Don't question it for different things
		public string date { get; set; }
		public string[] daysMissed { get; set; }
		public string className { get; set; }
		public bool response { get; set; }
	}
} 