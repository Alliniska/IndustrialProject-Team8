using Microsoft.EntityFrameworkCore;

namespace QuizAPI.Models
{
	public class QuestionContext : DbContext
	{
		public QuestionContext()
		{
		}

		public QuestionContext(DbContextOptions<QuestionContext> options)
			: base(options)
		{
		}

		public DbSet<Question> QuestionItems { get; set; }
	}
}
