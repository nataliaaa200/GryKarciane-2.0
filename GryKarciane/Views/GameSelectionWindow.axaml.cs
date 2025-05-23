﻿using Avalonia.Controls;
using Avalonia.Interactivity;
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

        private void CzteryKarty_Click(object sender, RoutedEventArgs e)
        {
            var czteryKartyOkno = new CzteryKarty(PlayerName); // zakładamy że masz klasę `CzteryKartyWindow`

            czteryKartyOkno.Closed += (s, args) =>
            {
                this.Show(); // Po zamknięciu gry pokaż z powrotem menu
            };

            czteryKartyOkno.Show();
            this.Hide();
        }

        private void Gapa_Click(object sender, RoutedEventArgs e)
        {
            var gapaOkno = new Gapa(PlayerName);

            gapaOkno.Closed += (s, args) =>
            {
                this.Show(); // Po zamknięciu Gapy pokaż z powrotem to okno
            };

            gapaOkno.Show();
            this.Hide();
        }

        private void Oczko_Click(object sender, RoutedEventArgs e)
        {
            var oczkoOkno = new Oczko(PlayerName);

            oczkoOkno.Closed += (s, args) =>
            {
                this.Show();
            };

            oczkoOkno.Show();
            this.Hide();
        }

        private void Gwint_Click(object sender, RoutedEventArgs e)
        {
            var gwintOkno = new Gwint(PlayerName);

            gwintOkno.Closed += (s, args) =>
            {
                this.Show(); // Po zamknięciu Gapy pokaż z powrotem to okno
            };

            gwintOkno.Show();
            this.Hide();
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

