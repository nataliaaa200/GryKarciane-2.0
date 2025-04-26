using Avalonia.Controls;
using Avalonia.Interactivity;
using GryKarciane.ViewModels;

namespace GryKarciane.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (DataContext is MainViewModel vm)
            {
                vm.OnLoginCompleted += Vm_OnLoginCompleted;
            }
        }

        private void Vm_OnLoginCompleted(string playerName)
        {
            // Otwórz okno wyboru gry i przekazanie loginu gracza
            var gameSelectionWindow = new GameSelectionWindow(playerName);

            // Zarejestruj zamknięcie okna gry, aby nie zamykało całego programu
            gameSelectionWindow.Closed += (sender, e) =>
            {
                // Po zamknięciu okna gry, wróć do okna logowania
                this.Show();
            };

            gameSelectionWindow.Show();
            this.Hide(); // Ukrywamy główne okno logowania, gdy otwieramy okno wyboru gry
        }

        private void Historia_Click(object? sender, RoutedEventArgs e)
        {
            var gameWindow = new Historia();
            gameWindow.Show();
        }
    }
}




