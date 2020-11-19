using System;                                                                                                                            
 
namespace backend.Controllers.Models
{
	public class StudentDTO
	{
		public string name { get; set; }
		public ClassDTO[] classes { get; set; }
		public string email { get; set; }
		public string pass { get; set; }
		public bool response { get; set; }
  }
} 

