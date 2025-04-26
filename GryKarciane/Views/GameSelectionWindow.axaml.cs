using Avalonia.Controls;
using System;

namespace GryKarciane.Views
{
    public partial class GameSelectionWindow : Window
    {
        public string PlayerName { get; set; }

        public GameSelectionWindow(string playerName)
        {
            InitializeComponent();
            PlayerName = playerName;
        }

        // Dodajmy logikę zamknięcia gry
        private void EndGame()
        {
            // Przykładowe zakończenie gry
            // Możesz tu dodać zapisywanie punktów, zakończenie gry itd.

            // Zamykamy okno gry
            this.Close();
        }

        // Przykładowa funkcja wywołująca zakończenie gry, np. po kliknięciu przycisku
        private void OnEndGameButtonClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            EndGame();
        }
    }
}

