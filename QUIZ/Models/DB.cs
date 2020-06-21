using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ClientQUIZ;

namespace QUIZ.Models
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext() : base("QUIZTWO")
		{
		}
		
		public DbSet<Theme> Themes { get; set; }
		public DbSet<Question> Question { get; set; }
		public DbSet<User> Users { get; set; }
	}
}