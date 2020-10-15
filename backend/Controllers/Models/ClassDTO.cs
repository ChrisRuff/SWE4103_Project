using System;
 
namespace backend.Controllers.Models
{
	public class ClassDTO
	{
		public string className { get; set; }
		public SeatDTO seat { get; set; }
		public int width { get; set; }
		public int height { get; set; }
  }
}
