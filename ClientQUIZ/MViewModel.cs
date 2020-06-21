using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Input;
using EasyHttp.Http;


namespace ClientQUIZ
{
	class MViewModel : INotifyPropertyChanged
	{
        public MViewModel()
        {
            _answerText = "";
            _question = "";
            _info = "<--- Введите свое имя, чтобы начать";
            _questionsItems = new ObservableCollection<Question>();
            _themesItems = new ObservableCollection<Theme>();
            _selectThemes = -1;
            _selectQuestions = -1;
            _userName = "";
            _rating = 0;
            LoadThemes();
            IsAuthorize = false;
        }
        //---------------------------------------------
        private string _answerText;
        private int _rating;
        private string _userName;
        private string _question;
        private string _info;
        private int _selectThemes;
        private int _selectQuestions;
        private ObservableCollection<Theme> _themesItems;
        private ObservableCollection<Question> _questionsItems;
        //---------------------------------------------
        public bool IsAuthorize { set; get; }
        public int Rating { get { return _rating; } set { _rating = value; OnPropertyChanged(nameof(Rating)); } }
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(nameof(UserName)); } }
        public string AnswerText { get { return _answerText; } set { _answerText = value; OnPropertyChanged(nameof(AnswerText)); } }
        public string Question { get { return _question; } set { _question = value; OnPropertyChanged(nameof(Question)); } }
        public string Info { get { return _info; } set { _info = value; OnPropertyChanged(nameof(Info)); } }
        public int SelectThemes { get { return _selectThemes; } set { _selectThemes = value; OnPropertyChanged(nameof(SelectThemes)); } }
        public int SelectQuestions { get { return _selectQuestions; } set { _selectQuestions = value; OnPropertyChanged(nameof(SelectQuestions)); } }
        public ObservableCollection<Question> QuestionsItems { get { return _questionsItems; } set { _questionsItems = value; OnPropertyChanged(nameof(QuestionsItems)); } }
        public ObservableCollection<Theme> ThemesItems { get { return _themesItems; } set { _themesItems = value; OnPropertyChanged(nameof(ThemesItems)); } }

        //========================================================================================================================

        private ICommand _loadQuestionsCommand;
        private ICommand _loadQuestionTextCommand;
        private ICommand _loadAnswerCommand;
        private ICommand _answerCommand;
        private ICommand _randomQuestionCommand;
        private ICommand _authCommand;

        //---------------------------------------------------------------------------------------------------------------------------
        public ICommand LoadQuestionsCommand { get {return _loadQuestionsCommand ?? (_loadQuestionsCommand = new RelayCommand(LoadQuestionsCommandExe, () => SelectThemes>-1 && IsAuthorize)); } }
        public ICommand LoadQuestionTextCommand { get {return _loadQuestionTextCommand ?? (_loadQuestionTextCommand = new RelayCommand(LoadQuestionTextCommandExe, () => SelectQuestions>-1)); } }
        public ICommand LoadAnswerCommand { get {return _loadAnswerCommand ?? (_loadAnswerCommand = new RelayCommand(LoadAnswerCommandExe, () => SelectQuestions>-1)); } }
        public ICommand AnswerCommand { get {return _answerCommand ?? (_answerCommand = new RelayCommand(AnswerCommandExe, () => SelectQuestions>-1)); } }
        public ICommand RandomQuestionCommand { get {return _randomQuestionCommand ?? (_randomQuestionCommand = new RelayCommand(RandomQuestionCommandExe, () => SelectThemes>-1)); } }
        public ICommand AuthCommand { get {return _authCommand ?? (_authCommand = new RelayCommand(AuthCommandExe)); } }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void LoadThemes()
        {
			var client = new HttpClient();
			var response = client.Get("https://localhost:44341/api/Themes");
			var themes = response.StaticBody<ObservableCollection<Theme>>();
			ThemesItems = themes;
        }
        private void LoadQuestionsCommandExe()
        {
            var client = new HttpClient();
	        var response = client.Get($"https://localhost:44341/api/Questions/Theme/{SelectThemes+1}");
	        var questions = response.StaticBody<ObservableCollection<Question>>();
	        QuestionsItems = questions;
	        Info = "Выберите вопрос~";
        }
        private void LoadQuestionTextCommandExe()
        {
	        Question = QuestionsItems[SelectQuestions].Name;
	        Info = "Дайте ответ на вопрос!";
        }
        private void LoadAnswerCommandExe()
        {
	        Info = QuestionsItems[SelectQuestions].Answer;
        }

        private void AnswerCommandExe()
        {
	        if (AnswerText == QuestionsItems[SelectQuestions].Answer)
	        {
		        Info = "ВЕРНО";
		        Rating++;
		        QuestionsItems.RemoveAt(SelectQuestions);
            }
	        else
	        {
		        Info = "НЕ ВЕРНО";
		        Rating--;
	        }
	        var client = new HttpClient();
	        client.Get($"https://localhost:44341/api/Users/UpData/{UserName}/{Rating}");
	        AnswerText = "";
        }

        private void RandomQuestionCommandExe()
        {
	        Random rnd = new Random();
            if(QuestionsItems.Count < 1)return;
            int RIndex = rnd.Next(QuestionsItems.Count);
	        SelectQuestions = QuestionsItems.IndexOf(QuestionsItems[RIndex]);
	        Question = QuestionsItems[RIndex].Name;
        }

        private void AuthCommandExe()
        {
	        if (UserName.Length < 1)return;
	        var client = new HttpClient();
	        var response = client.Get($"https://localhost:44341/api/Users/{UserName}");
            var user = response.StaticBody<User>();
	        if (user == null)
	        {
		        client.Get($"https://localhost:44341/api/Users/Add/{UserName}");
            }
	        else
	        {
		        Rating = user.Rating;
	        }
	        IsAuthorize = true;
	        Info = "Выберите тему из списка!";
        }


        //==========================================================================================================================
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public void Execute(object parameter)
        {
            if (parameter is T arg)
            {
                _execute.Invoke(arg);
            }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is T arg)
            {
                return _canExecute?.Invoke(arg) ?? true;
            }
            return false;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
