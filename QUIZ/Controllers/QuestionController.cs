using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QUIZ.Models;

namespace QUIZ.Controllers
{
	public class QuestionController : ApiController
	{
		QuestionController()
		{
			Model = new QuestionModel();
		}

		public Models.QuestionModel Model { get; set; }

		[Route("api/Questions/")]
		public IEnumerable<Question> Get()
		{
			return Model.Questions;
		}

		[Route("api/Questions/{id}")]
		public Question GetById(int id)
		{
			return Model.GetQuestionById(id);
		}

		[Route("api/Questions/Add/{name}/{theme}/{answer}")]
		public void Get(string name, int theme, string answer)
		{
			Model.AddQuestion(name, theme, answer);
		}

		[Route("api/Questions/{id}")]
		public void Delete(int id)
		{
			Model.DellQuestionById(id);
		}

		[Route("api/Questions/Theme/{theme}")]
		public IEnumerable<Question> GetByTheme(int theme)
		{
			return Model.GetByTheme(theme);
		}
	}
}