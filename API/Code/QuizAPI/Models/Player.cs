using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Models
{
	public class Player
	{
		//Model for records in the Player table
		public int id { get; set; }
		public string name { get; set; }
		public int score { get; set; }
		public int lobbyId { get; set; } //foreign key from the Lobby table
		public int questionIndex { get; set; }
		public bool lifeline5050 { get; set; }
		public bool lifelineSkip { get; set; }
	}
}