using ReactiveUI;
using System;
using System.Reactive;
using GryKarciane.Views;

namespace GryKarciane.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private string _login;
        public string Login
        {
            get => _login;
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> GuestCommand { get; }

        // EVENT - informuje, że trzeba otworzyć nowe okno
        public event Action<string> OnLoginCompleted;

        public MainViewModel()
        {
            LoginCommand = ReactiveCommand.Create(DoLogin);
            GuestCommand = ReactiveCommand.Create(DoGuestLogin);
        }

        private void DoLogin()
        {
            if (string.IsNullOrWhiteSpace(Login))
                return;

            OnLoginCompleted?.Invoke(Login);
        }

        private void DoGuestLogin()
        {
            Login = "Gość";
            OnLoginCompleted?.Invoke(Login);
        }
    }
}





