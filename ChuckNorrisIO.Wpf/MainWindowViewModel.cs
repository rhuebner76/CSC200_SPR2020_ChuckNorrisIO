using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ChuckNorrisIO.Wpf
{
    public class DelegateCommand : System.Windows.Input.ICommand
    {
        private readonly Action<object> _execute;

        public DelegateCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    public class MainWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private Services.ChuckNorrisJokeClient Client { get; }

        private IEnumerable<string> _categories;
        private string _joke;
        private string _category;
        private string _query;

        public IEnumerable<string> Categories
        {
            get { return _categories; }
        }

        public string CurrentCategory
        {
            get { return _category; }
            set
            {
                _category = value;
                SetRandomJoke();
            }
        }


        public string CurrentQuery
        {
            get { return _query; }
            set
            {
                _query = value;
                SearchJoke();
            }
        }

        public string CurrentJoke
        {
            get { return _joke; }
        }

        public System.Windows.Input.ICommand NextJoke { get; private set; }


        public MainWindowViewModel()
        {
            Client = new Services.ChuckNorrisJokeClient();
            _categories = Client.Categories();

            NextJoke = new DelegateCommand(_ => SetRandomJoke());

            SetRandomJoke();
        }

        private void SetRandomJoke()
        {
            if (string.IsNullOrEmpty(CurrentCategory))
            {
                IEnumerable<string> jokes = Client.Random();
                _joke = jokes.First();

            }
            else
            {
                IEnumerable<string> jokes = Client.Random(CurrentCategory);
                _joke = jokes.First();
            }

            OnPropertyChanged("CurrentJoke");
        }

        private void SearchJoke()
        {
            IEnumerable<string> jokes = Client.Search(CurrentQuery);
            _joke = jokes.First();

            OnPropertyChanged("CurrentJoke");
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));  
        }
    }
}