using System;                                                                                                                            
using System.Collections; 
using System.Collections.Generic; 

namespace backend.Controllers.Models
{
	public class StudentRosterDTO
	{
		public string name { get; set; }
		public ArrayList daysMissed { get; set; }
	}
} 