using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace QUIZ.Models
{
	public class QuestionModel
	{
		public QuestionModel()
		{
			db = new ApplicationContext();
			db.Question.Load();
			Questions = db.Question.Local;
		}


		ApplicationContext db;

		public ObservableCollection<Question> Questions { get; private set; }

		public ObservableCollection<Question> GetQuestions()
		{
			return Questions;
		}

		public Question GetQuestionById(int id)
		{
			return db.Question.Find(id);
		}

		public void DellQuestionById(int id)
		{
			Question T = db.Question.Find(id);
			if (T == null)
			{
				return;
			}
			db.Question.Remove(T);
			db.SaveChanges();
		}

		public void AddQuestion(string name, int theme, string answer)
		{
			Question Q = new Question() { Name = name, Theme = theme, Answer = answer};
			db.Question.Add(Q);
			db.SaveChanges();
		}

		public string GetAnswerById(int id)
		{
			return db.Question.Find(id).Answer;
		}

		public IEnumerable<Question> GetByTheme(int theme)
		{
			IEnumerable<Question> Q = db.Question.Where(Questions => Questions.Theme == theme);
			return Q;
		}
	}
}