using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GryKarciane.Models;

namespace GryKarciane
{
    public partial class Oczko : Window
    {
        private List<Card> playerCards = new List<Card>();

        
        public enum Suit { Kier } 

        public class Card
        {
            public string Face { get; set; } 
            public Suit Suit { get; set; } = Suit.Kier;

            public string ImagePath { get; set; }

            public int Value => Face switch
            {
                "A" => 11,
                "K" => 4,
                "Q" => 3,
                "J" => 2,
                _ => int.Parse(Face)
            };
        }


        private List<Card> deck = new();
        private int score = 0;
        private bool gameEnded = false;
        private readonly string playerName;
        private Random rng = new();


        public Oczko(string playerName)
        {
            InitializeComponent();

            try
            {
                var testImage = new Bitmap("avares://GryKarciane/Aski/Cards/2k.png");
                Console.WriteLine("Obrazek 2k.png załadowany poprawnie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd testowego ładowania obrazka: {ex.Message}");
            }

            this.playerName = playerName;
            StartGame(); 
        }

        private void StartGame()
        {
            InitDeck();
            playerCards.Clear();
            score = 0;
            gameEnded = false;
            RefreshUI(); 
        }

        private void InitDeck()
        {
            deck.Clear();

            string basePath = "avares://GryKarciane/Aski/Cards/";

            string[] faces = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            foreach (var face in faces)
            {
                string imagePath = $"{basePath}{face.ToLower()}k.png";  
                if (face == "J" || face == "Q" || face == "K" || face == "A")
                {
                    imagePath = $"{basePath}{face.ToUpper()}k.png";  
                }

                deck.Add(new Card { Face = face, ImagePath = imagePath });
            }

            Shuffle(deck); 
        }

        private void Shuffle(List<Card> deck)
        {
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

        private void RefreshUI()
        {
            CardsPanel.Children.Clear(); 

            foreach (var card in playerCards)
            {
                var imagePath = card.ImagePath;  

                var bitmap = new Bitmap(AssetLoader.Open(new Uri(imagePath)));  

                var image = new Image
                {
                    Source = bitmap,  
                    Width = 100,  
                    Height = 140  
                };

                CardsPanel.Children.Add(image);  
            }

            ScoreLabel.Text = $"Wynik: {score}";


        }

        private void DrawCard_Click(object? sender, RoutedEventArgs e)
        {
            if (gameEnded || deck.Count == 0) return;

            var card = deck[0];  
            deck.RemoveAt(0);  
            playerCards.Add(card);  
            score += card.Value;  
            RefreshUI();  
            CheckWinCondition(card);  
        }

        private void CheckWinCondition(Card lastCard)
        {
            if (score == 22 && lastCard.Face == "A")
            {
                EndGame("Perskie oczko! Wygrałeś!", true);
            }
            else if (score == 21)
            {
                EndGame("Masz oczko! Wygrałeś!", true);
            }
            else if (score > 21)
            {
                EndGame("Przegrałeś! Masz więcej niż 21.", false);
            }
        }

        private void Pass_Click(object? sender, RoutedEventArgs e)
        {
            if (gameEnded) return;
            EndGame($"Zakończyłeś z wynikiem {score}.", false);
        }

        private async void EndGame(string message, bool isWin)
        {
            gameEnded = true;

            var result = new GryKarciane.Models.GameResult
            {
                PlayerName = playerName,
                Score = score,
                IsWin = isWin,
                GameTime = TimeSpan.Zero 
            };

            GryKarciane.Models.GameResultSaver.SaveResult(result);

            await MessageBox.Show(this, message, "Koniec gry");
            Close();
        }

        public static class MessageBox
        {
            public static async Task Show(Window parent, string text, string title)
            {
                var dialog = new Window
                {
                    Title = title,
                    Width = 300,
                    Height = 150,
                    Content = new StackPanel
                    {
                        Margin = new Thickness(10),
                        Children =
                        {
                            new TextBlock { Text = text, Margin = new Thickness(0, 0, 0, 20) },
                            new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center }
                        }
                    }
                };

                var button = ((dialog.Content as StackPanel)?.Children[1] as Button)!;
                button.Click += (_, _) => dialog.Close();

                await dialog.ShowDialog(parent);
            }
        }
    }
}







