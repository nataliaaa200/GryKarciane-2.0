using System.ComponentModel;

namespace GryKarciane.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _login;

        // Właściwość do powiązania loginu w UI
        public string Login
        {
            get => _login;
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Metoda powiadamiająca o zmianach właściwości
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
