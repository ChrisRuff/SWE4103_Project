using System;

namespace backend.Controllers.Models
{
	public class ClassDTO
	{
		public string className { get; set; }
		public SeatDTO seat { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int notificationFreq {get; set;}
		public bool mandatory {get; set;}
		public SeatDTO[] DisabledSeats { get; set; }
		public SeatDTO[] ReservedSeats { get; set; }
		public SeatDTO[] AccessibleSeats { get; set; }
		public SeatDTO[] OpenSeats { get; set; }
		public string classCode { get; set; }
		public StudentRosterDTO[] roster { get; set; }
		public bool response { get; set; }
	}
}	
