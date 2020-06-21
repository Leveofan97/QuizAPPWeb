using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QUIZ.Models;

namespace QUIZ.Controllers
{
	public class ThemesController : ApiController
	{
		ThemesController()
		{
			Model = new Models.ThemesModel();
		}

		public Models.ThemesModel Model { get; set; }

		// GET api/themes
		public IEnumerable<Theme> Get()
		{
			return Model.GetThemes();
		}

		// GET api/themes/5
		public Theme Get(int id)
		{
			return Model.GetThemeById(id);
		}

		[Route ("api/Themes/Add/{name}")]
		public void Get(string name)
		{
			Model.AddTheme(name);
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
			Model.DellThemeById(id);
		}
	}
}
