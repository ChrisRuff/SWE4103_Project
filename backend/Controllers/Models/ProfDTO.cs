
using System;

namespace backend.Controllers.Models
{
	public class ProfDTO
	{
		public string name { get; set; }
		public string email { get; set; }
		public string pass { get; set; }
		public ClassDTO[] classes { get; set; }
		public bool response { get; set; }
	}
}
