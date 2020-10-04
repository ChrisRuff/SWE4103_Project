using System;                                                                                                                            
 
namespace backend.Controllers.Models
{
	public class AddStudentDTO
	{
		public string studentName { get; set; }
		public string[] classNames { get; set; }
		public string email { get; set; }
		public bool response { get; set; }
  }
} 

