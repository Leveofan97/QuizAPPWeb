using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ClientQUIZ;
using QUIZ.Models;

namespace QUIZ.Controllers
{
	//api/Users/Add/{UserName}
	//api/Users/{UserName}
	//api/Users/UpData/{UserName}/{Rating}
	public class UserController : ApiController
	{
		UserController()
		{
			Model = new UserModel();
		}

		public Models.UserModel Model { get; set; }

		[Route("api/Users/")]
		public IEnumerable<User> Get()
		{
			return Model.Users;
		}


		[Route("api/Users/Add/{Name}")]
		public void Get(string Name)
		{
			Model.AddUser(Name);
		}

		[Route("api/Users/{name}")]
		public User GetByName(string name)
		{
			return Model.GetUserByName(name);
		}

		[Route("api/Users/UpData/{Name}/{Rating}")]
		public void GetUpData(string Name, int Rating)
		{
			Model.UpData(Name, Rating);
		}

		[Route("api/Users/{id}")]
		public void Delete(int id)
		{
			Model.DellUserById(id);
		}

	}
}