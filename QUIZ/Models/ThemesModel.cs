using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace QUIZ.Models
{
	public class ThemesModel
	{
		public ThemesModel()
		{
			db = new ApplicationContext();
			db.Themes.Load();
			Themes = db.Themes.Local;
		}


		ApplicationContext db;

		public ObservableCollection<Theme> Themes { get; private set; }

		public ObservableCollection<Theme> GetThemes()
		{
			return Themes;
		}

		public Theme GetThemeById(int id)
		{
			return db.Themes.Find(id);
		}

		public void DellThemeById(int id)
		{
			Theme T = db.Themes.Find(id);
			if (T==null)
			{
				return;
			}
			db.Themes.Remove(T);
			db.SaveChanges();
		}

		public void AddTheme(string name)
		{
			Theme T = new Theme() {Name = name};
			db.Themes.Add(T);
			db.SaveChanges();
		}
	}
}