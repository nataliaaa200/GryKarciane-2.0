using ReactiveUI;

namespace GryKarciane.ViewModels
{
    public class GameSelectionViewModel : ReactiveObject
    {
        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => this.RaiseAndSetIfChanged(ref _welcomeMessage, value);
        }

        public GameSelectionViewModel(string playerName)
        {
            WelcomeMessage = $"Witaj, {playerName}! Wybierz grę:";
        }

        public GameSelectionViewModel() { } // konstruktor pusty dla Avalonia designer
    }
}


