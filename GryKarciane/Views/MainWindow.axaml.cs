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
            var gameSelectionWindow = new GameSelectionWindow(playerName);

            
            gameSelectionWindow.Closed += (sender, e) =>
            {
                
                this.Show();
            };

            gameSelectionWindow.Show();
            this.Hide();
        }

        private void Historia_Click(object? sender, RoutedEventArgs e)
        {
            var gameWindow = new Historia();
            gameWindow.Show();
        }
    }
}




