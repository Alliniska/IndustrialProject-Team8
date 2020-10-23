using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Models
{
	public class Lobby
	{
		//Model for records in the Lobby table
		public int id { get; set; }
		public string requestURL { get; set; }
		public string date { get; set; } //format: YYYY-MM-DDTHH:mm:ss.sssZ
	}
}