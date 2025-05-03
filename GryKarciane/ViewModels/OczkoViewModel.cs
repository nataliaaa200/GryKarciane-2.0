using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using GryKarciane.Models;

namespace GryKarciane.ViewModels
{
    public class OczkoViewModel : ReactiveObject
    {
        private readonly Window _window;

        public string PlayerName { get; }

        private List<string> _deck;
        private int _score;
        private bool _passed;
        private bool _gameEnded;

        public string CurrentScore => $"Twój wynik: {_score}";
        public string LastCardDrawn { get; set; }

        public ReactiveCommand<Unit, Unit> DrawCardCommand { get; }
        public ReactiveCommand<Unit, Unit> PassCommand { get; }
        public ReactiveCommand<Unit, Unit> EndGameCommand { get; }

        public OczkoViewModel(string playerName, Window window)
        {
            PlayerName = $"Gracz: {playerName}";
            _window = window;

            InitDeck();
            _score = 0;
            _passed = false;

            DrawCardCommand = ReactiveCommand.Create(DrawCard);
            PassCommand = ReactiveCommand.Create(Pass);
            EndGameCommand = ReactiveCommand.Create(() => _window.Close());
        }

        private void InitDeck()
        {
            _deck = new List<string>();
            string[] faces = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            foreach (var face in faces)
            {
                for (int i = 0; i < 4; i++) // 4 kolory
                    _deck.Add(face);
            }

            Shuffle(_deck);
        }

        private void Shuffle(List<string> deck)
        {
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var temp = deck[k];
                deck[k] = deck[n];
                deck[n] = temp;
            }
        }

        private int CardValue(string card)
        {
            return card switch
            {
                "J" => 2,
                "Q" => 3,
                "K" => 4,
                "A" => 11,
                _ => int.Parse(card),
            };
        }

        private void DrawCard()
        {
            if (_deck.Count == 0 || _gameEnded) return;

            string card = _deck[0];
            _deck.RemoveAt(0);

            LastCardDrawn = $"Wylosowano kartę: {card}";
            _score += CardValue(card);

            this.RaisePropertyChanged(nameof(LastCardDrawn));
            this.RaisePropertyChanged(nameof(CurrentScore));

            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (_score == 22 && LastCardDrawn == "A")
            {
                ShowResult("Perskie oczko! Wygrałeś!");
                _gameEnded = true;
            }
            else if (_score == 21)
            {
                ShowResult("Masz oczko! Wygrałeś!");
                _gameEnded = true;
            }
            else if (_score > 21)
            {
                ShowResult("Przegrałeś! Masz więcej niż 21.");
                _gameEnded = true;
            }
        }

        private void Pass()
        {
            if (_gameEnded) return;
            ShowResult($"Zakończyłeś z wynikiem {_score}.");
            _gameEnded = true;
        }

        private async void ShowResult(string message)
        {
            await MessageBox.Show(_window, message, "Wynik");
        }
    }

    public static class MessageBox
    {
        public static async System.Threading.Tasks.Task Show(Window owner, string message, string title)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 300,
                Height = 150,
                Content = new TextBlock
                {
                    Text = message,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap
                }
            };

            await dialog.ShowDialog(owner);
        }
    }
}

