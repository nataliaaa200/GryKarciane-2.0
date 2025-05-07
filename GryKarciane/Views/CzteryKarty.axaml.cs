using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Generic;
using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using GryKarciane.CzteryKarty;
using System.Linq;
using GryKarciane.ViewModels;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Timers;
using System.Diagnostics;
using Avalonia.Threading;

namespace GryKarciane.Views;

public partial class CzteryKarty : Window
{
    public class GameResult
    {
        public string PlayerName { get; set; }
        public bool IsWin { get; set; }
    }

    public static class GameResultSaver
    {
        private static readonly string FilePath = "wyniki_czteryKarty.json";

        public static void SaveResult(GameResult result)
        {
            List<GameResult> results = LoadResults();

            results.Add(result);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(results, options);
            File.WriteAllText(FilePath, json);
        }

        public static List<GameResult> LoadResults()
        {
            if (!File.Exists(FilePath))
                return new List<GameResult>();

            string json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<List<GameResult>>(json) ?? new List<GameResult>();
        }
    }

    private string _playerName;
    private int selectedCardIndex = -1;
    private Deck deck;
    private Player player;
    private Player computer;
    private Game game;

    public CzteryKarty(string playerName)
    {
        InitializeComponent();
        InitializeGame();
        _playerName = playerName;
    }

    private void InitializeGame()
    {
        player = new Player("Gracz 1");
        computer = new Player("Komputer");
        deck = new Deck();
        game = new Game();
        game.AddPlayer(player);
        game.AddPlayer(computer);

        game.DealCards(deck);

        Card1Image.Source = LoadImage(player.Hand[0].ImagePath);
        Card2Image.Source = LoadImage(player.Hand[1].ImagePath);
        Card3Image.Source = LoadImage(player.Hand[2].ImagePath);
        Card4Image.Source = LoadImage(player.Hand[3].ImagePath);
    }

    private Bitmap LoadImage(string uri)
    {
        var assetUri = new Uri(uri);
        var stream = AssetLoader.Open(assetUri);
        return new Bitmap(stream);
    }

    private void Card_Click(object sender, RoutedEventArgs e)
    {
        var cardButton = sender as Button;
        if (cardButton != null)
        {
            int cardIndex = int.Parse(cardButton.Name.Substring(4)) - 1;

            selectedCardIndex = cardIndex;

            if (selectedCardIndex >= 0 && selectedCardIndex < player.Hand.Count)
            {
                string discardedCard = player.Hand[selectedCardIndex].ImagePath;
                player.Hand.RemoveAt(selectedCardIndex);

                Card newCard = deck.DrawCard(); 

                if (newCard != null)
                {
                    player.Hand.Add(newCard); 
                    UpdateCardImages();
                    if (player.HasFourOfAKind())
                    {
                        var winner = player.Name;
                        MessageBox.Show(this, $"{winner} wygral!", "Wygrana");
                    }
                    else
                    {
                        computer.MakeMove(deck);
                        if (computer.HasFourOfAKind())
                        {
                            var winner = computer.Name;
                            MessageBox.Show(this, $"{winner} wygral!", "Wygrana");
                        }
                    }
                    if (player.HasFourOfAKind() || computer.HasFourOfAKind())
                    {
                        var result = new GameResult
                        {
                            PlayerName = _playerName,
                            IsWin = player.HasFourOfAKind()
                        };

                        GameResultSaver.SaveResult(result);
                    }
                }
            }
        }
        UpdateDeckStatus();

        void UpdateCardImages()
        {
            Card1Image.Source = LoadImage(player.Hand[0].ImagePath);
            Card2Image.Source = LoadImage(player.Hand[1].ImagePath);
            Card3Image.Source = LoadImage(player.Hand[2].ImagePath);
            Card4Image.Source = LoadImage(player.Hand[3].ImagePath);
        }
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
            button.Click += (_, _) =>
            {
                dialog.Close();
                parent.Close();
            };

            await dialog.ShowDialog(parent);
        }
    }

    private void UpdateDeckStatus()
    {
        int remaining = deck.RemainingCards; 
        DeckInfo.Text = $"Stos: {remaining} kart";

        DeckImage.IsVisible = remaining > 0;
        ShuffleButton.IsVisible = remaining == 0;
    }

    private void ShuffleButton_Click(object? sender, RoutedEventArgs e)
    {
        deck = new Deck();

        foreach (var player in game.players) 
        {
            foreach (var card in player.Hand)
            {
                deck.RemoveCard(card);
            }
        }
        UpdateDeckStatus();
        deck.Shuffle();
        
    }
}
