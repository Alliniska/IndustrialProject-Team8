using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Models
{
	public class Question
	{
		//Model for records in the Question table
		public int id { get; set; }
		public string category { get; set; }
		public string type { get; set; }
		public string difficulty { get; set; }
		public string question { get; set; }
		public string correctAnswer { get; set; }
		public string incorrectAnswer1 { get; set; }
		public string incorrectAnswer2 { get; set; }
		public string incorrectAnswer3 { get; set; }
		public int questionIndex { get; set; }
		public int lobbyId { get; set; } //foreign key from the Lobby table
	}
}