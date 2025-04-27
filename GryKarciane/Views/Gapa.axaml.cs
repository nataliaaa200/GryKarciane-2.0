using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Avalonia.Interactivity;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Threading;
using Avalonia.Media;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace GryKarciane;

public partial class Gapa : Window
{
    public class GameResult

    {
        public string PlayerName { get; set; }
        public TimeSpan GameTime { get; set; }
        public bool IsWin { get; set; }
    }

    public static class GameResultSaver
    {
        private static readonly string FilePath = "wyniki_gapa.json";

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
        public enum Suit { Pik, Kier, Karo, Trefl }

    public class Card
    {
        public Suit Suit { get; set; }

        public static Card RandomCard(Random rng)
        {
            var suit = (Suit)rng.Next(0, 4);
            return new Card { Suit = suit };
        }

        public string Symbol =>
            Suit switch
            {
                Suit.Pik => "♠",
                Suit.Kier => "♥",
                Suit.Karo => "♦",
                Suit.Trefl => "♣",
                _ => "?"
            };

        public string Color =>
            (Suit == Suit.Kier || Suit == Suit.Karo) ? "Red" : "Black";

        public override string ToString() => Symbol;
    }

    private Stopwatch stopwatch = new();
    private Timer timer = new();
    private List<Card> playerCards = new();
    private Card topCard;
    private Random rng = new();
    private readonly string playerName;
    public Gapa(string playerName)
    {
        InitializeComponent();
        this.playerName = playerName;
        StartGame();

    }

    private void StartGame()
    {
        playerCards = Enumerable.Range(0, 6).Select(_ => Card.RandomCard(rng)).ToList();
        topCard = Card.RandomCard(rng);
        RefreshUI();
        stopwatch.Restart();
        timer = new Timer(1000);
        timer.Elapsed += (_, _) =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Title = $"Rozgrywka - czas: {stopwatch.Elapsed:mm\\:ss}";
            });
        };
        timer.Start();

    }

    private void RefreshUI()
    {
        // Pokaż kartę na stosie
        TopCardView.Text = topCard.Symbol;
        TopCardView.FontSize = 48;
        TopCardView.Foreground = new SolidColorBrush(
            topCard.Color == "Red" ? Colors.Red : Colors.Black);
        TopCardView.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
        TopCardView.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;

        // Wyczyść panel z kartami gracza
        PlayerCardsPanel.Children.Clear();

        // Dodaj każdą kartę jako przycisk
        foreach (var card in playerCards)
        {
            var btn = new Button
            {
                Width = 60,
                Height = 80,
                Margin = new Thickness(5),
                Content = new TextBlock
                {
                    Text = card.Symbol,
                    FontSize = 32,
                    Foreground = new SolidColorBrush(
                        card.Color == "Red" ? Colors.Red : Colors.Black),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    TextAlignment = Avalonia.Media.TextAlignment.Center
                }
            };

            btn.Click += (_, _) => PlayCard(card);
            PlayerCardsPanel.Children.Add(btn);
        }
    }


    private void PlayCard(Card card)
    {
        if (card.Suit == topCard.Suit)
        {
            playerCards.Remove(card);
            topCard = Card.RandomCard(rng); // komputer dokłada
            RefreshUI();
            if (playerCards.Count == 0)
                ShowWinMessage();
        }
        else
        {
            EndGame("Gapa!");
        }
    }

    private async void EndGame(string message)
    {
        timer.Stop();
        stopwatch.Stop();

        var result = new GameResult
        {
            PlayerName = playerName, // musisz mieć gdzieś zapisane `playerName`
            GameTime = stopwatch.Elapsed,
            IsWin = (message == "Wygrałeś!")
        };

        GameResultSaver.SaveResult(result);

        await MessageBox.Show(this, message, "Koniec gry");
        Close();
    }



    private void DrawCard_Click(object? sender, RoutedEventArgs e)
    {
        var drawnCard = Card.RandomCard(rng);
        playerCards.Add(drawnCard);
        RefreshUI();
    }

    private void ShowWinMessage() => EndGame("Wygrałeś!");

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
