using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ClientQUIZ;

namespace QUIZ.Models
{
	public class UserModel
	{
		public UserModel()
		{
			db = new ApplicationContext();
			db.Users.Load();
			Users = db.Users.Local;
		}

		ApplicationContext db;
		public ObservableCollection<User> Users { get; private set; }
		
		public void AddUser(string name)
		{
			User U = new User() {Name = name, Rating = 0};
			db.Users.Add(U);
			db.SaveChanges();
		}

		public User GetUserByName(string name)
		{
			IEnumerable<User> U = db.Users.Where(Users => Users.Name == name);
			if (U.Count()<1) return null;
			return U.First();
		}

		public void UpData(string name, int rating)
		{
			IEnumerable<User> U = db.Users.Where(Users => Users.Name == name);
			if (U.Count() < 1) return;
			DellUserById(U.First().Id);
			User nU = new User() {Name = name, Rating = rating};
			db.Users.Add(nU);
			db.SaveChanges();
		}

		public void DellUserById(int id)
		{
			User U = db.Users.Find(id);
			if (U == null)
			{
				return;
			}
			db.Users.Remove(U);
			db.SaveChanges();
		}

	}
}