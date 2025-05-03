using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System;
using Avalonia.Interactivity;
using System.Linq;

namespace GryKarciane;

public partial class Gwint : Window
{


    public class GwentCard
    {
        public int Power { get; set; }
        // public string Name { get; set; }

        // Losowanie karty o losowej mocy od 1 do 10
        public static GwentCard RandomCard(Random rng)
        {
            return new GwentCard
            {
                Power = rng.Next(1, 11),  // Si³a karty od 1 do 10
                                          //     Name = $"Karta {rng.Next(1, 11)}"
            };
        }

        public override string ToString() => $" ({Power})";
    }
    int playerTotalPower;
    int computerTotalPower;
    private List<GwentCard> playerCards = new();
    private List<GwentCard> computerCards = new();
    private List<GwentCard> playerPlayedCards = new();
    private List<GwentCard> computerPlayedCards = new();
    private int playerScore = 0;
    private int computerScore = 0;
    private int roundsPlayed = 0;
    private Random rng = new();
    private bool isRoundOver = false;
    private StackPanel komputerRowPanel;
    private StackPanel playerRowPanel;
    
    
    
    
    public Gwint(string PlayerName)
    {
        InitializeComponent();
        StartGame();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);


        komputerRowPanel = this.FindControl<StackPanel>("ComputerRowPanel");
        playerRowPanel = this.FindControl<StackPanel>("PlayerRowPanel");
        PlayerCardPanel = this.FindControl<StackPanel>("PlayerCardPanel");
        //ComputerCardPanel = this.FindControl<StackPanel>("ComputerCardPanel");
        GameBoardPanel = this.FindControl<StackPanel>("GameBoardPanel");
        GameResultText = this.FindControl<TextBlock>("GameResultText");
    }
    private void StartGame()
    {
        // Przygotowanie kart testowych


        playerCards = Enumerable.Range(0, 10).Select(_ => GwentCard.RandomCard(rng)).ToList();


        computerCards = Enumerable.Range(0, 10).Select(_ => GwentCard.RandomCard(rng)).ToList();


        // Ustawienie tekstu o stanie gry
        GameResultText.Text = "Rozpocznij grê!";

        // Dodaj testowe karty gracza na ekranie
        PlayerCardPanel.Children.Clear();
        foreach (var card in playerCards)
        {
            var btn = new Button
            {
                Content = card.ToString(),
                Width = 80,
                Height = 50,
                Margin = new Thickness(5)
            };
            btn.Click += (s, args) => PlayCard(card);
            PlayerCardPanel.Children.Add(btn);
        }

        // Dodaj testowe karty komputera na ekranie
        // ComputerCardPanel.Children.Clear();
        foreach (var card in computerCards)
        {
            var btn = new Button
            {
                Content = card.ToString(),
                Width = 80,
                Height = 50,
                Margin = new Thickness(5)
            };
            btn.IsEnabled = false;  // Karty komputera s¹ wy³¹cznie do podgl¹du
                                    // ComputerCardPanel.Children.Add(btn);
        }

        // Dodaj przycisk Pass
        var passButton = new Button
        {
            Content = "Pass",
            Width = 80,
            Height = 40,
            Margin = new Thickness(10)
        };
        passButton.Click += PassRound;
        PlayerCardPanel.Children.Add(passButton);
    }


    private void PlayCard(GwentCard playerCard)
    {
        // Gracz nie mo¿e zagraæ karty, jeœli runda zosta³a zakoñczona
        if (isRoundOver || roundsPlayed >= 3 || playerCards.Count == 0)
        {
            ShowGameResult();
            return;
        }

        // Gracz zagrywa kartê
        playerCards.Remove(playerCard);
        playerPlayedCards.Add(playerCard);

        // Wyœwietl kartê gracza na planszy rozgrywki w rzêdzie
        var playerCardBtn = new Button
        {
            Content = playerCard.ToString(),
            Width = 80,
            Height = 50,
            Margin = new Thickness(5),
            IsEnabled = false  // Zablokuj mo¿liwoœæ klikniêcia po zagrywce
        };

        // Dodaj kartê gracza na planszê
        playerRowPanel.Children.Add(playerCardBtn);

        // Usuwamy przycisk z panelu gracza (po zagraniu)
        var cardButton = PlayerCardPanel.Children.OfType<Button>().FirstOrDefault(b => b.Content.ToString() == playerCard.ToString());
        if (cardButton != null)
        {
            PlayerCardPanel.Children.Remove(cardButton);
        }

        // Komputer zagrywa kartê (sprawdzenie czy s¹ jeszcze karty)
        if (computerCards.Count > 0)
        {
            var computerCard = computerCards[rng.Next(computerCards.Count)];
            computerCards.Remove(computerCard);
            computerPlayedCards.Add(computerCard);

            // Wyœwietl kartê komputera na planszy w rzêdzie
            var computerCardBtn = new Button
            {
                Content = computerCard.ToString(),
                Width = 80,
                Height = 50,
                Margin = new Thickness(5),
                IsEnabled = false  // Zablokuj mo¿liwoœæ klikniêcia po zagrywce
            };

            // Dodaj kartê komputera na planszê
            komputerRowPanel.Children.Add(computerCardBtn);

            // Wyœwietl wynik rundy
            UpdateRoundResult(playerCard, computerCard);
        }
        else
        {
            // Jeœli nie ma kart komputera, zakoñcz rundê
            ShowGameResult();
        }
    }





    private void PassRound(object sender, RoutedEventArgs e)
    {
        // Usuñ wszystkie karty z planszy
        playerRowPanel.Children.Clear();
        komputerRowPanel.Children.Clear();

        // Usuwamy karty gracza i komputera
        playerPlayedCards.Clear();
        computerPlayedCards.Clear();

        // Resetowanie kart w panelu gracza
        PlayerCardPanel.Children.Clear();
        foreach (var card in playerCards)
        {
            var btn = new Button
            {
                Content = card.ToString(),
                Width = 80,
                Height = 50,
                Margin = new Thickness(5)
            };
            btn.Click += (s, args) => PlayCard(card);
            PlayerCardPanel.Children.Add(btn);
        }

        // Resetowanie kart w panelu komputera
        // ComputerCardPanel.Children.Clear();
        foreach (var card in computerCards)
        {
            var btn = new Button
            {
                Content = card.ToString(),
                Width = 80,
                Height = 50,
                Margin = new Thickness(5)
            };
            btn.IsEnabled = false;  // Karty komputera s¹ wy³¹cznie do podgl¹du
                                    // ComputerCardPanel.Children.Add(btn);
        }
        if (computerTotalPower > playerTotalPower)
        {
            computerScore++;
        }
        else if (playerTotalPower > computerTotalPower)
        {
            playerScore++;
        }
        // Zwiêkszenie liczby rund
        roundsPlayed++;


        // Sprawdzenie, czy gra jest zakoñczona
        if (roundsPlayed == 3)
        {
            ShowGameResult();
        }
        else
        {
            // Czekamy na rozpoczêcie nowej rundy
            GameResultText.Text = $"Runda {roundsPlayed} zakoñczona! Kliknij Pass, aby kontynuowaæ!";
        }
        // Dodaj przycisk Pass z powrotem po wyczyszczeniu panelu
        var passButton = new Button
        {
            Content = "Pass",
            Width = 80,
            Height = 40,
            Margin = new Thickness(10)
        };
        passButton.Click += PassRound;
        PlayerCardPanel.Children.Add(passButton);
        if (roundsPlayed == 3)
        {
            if (passButton != null)
                passButton.IsVisible = false;

            ShowGameResult();
        }
        else
        {
            GameResultText.Text = $"Runda {roundsPlayed} zakoñczona! Wybierz dowoln¹ kartê aby graæ dalej";
        }



    }



    private int SumCardPowers(List<GwentCard> playedCards)
    {
        return playedCards.Sum(card => card.Power);
    }

    private void UpdateRoundResult(GwentCard playerCard, GwentCard computerCard)
    {

        // Wyœwietl aktualny wynik po rozegranej turze
        playerTotalPower = SumCardPowers(playerPlayedCards);
        computerTotalPower = SumCardPowers(computerPlayedCards);


        GameResultText.Text = $"\nAktualny wynik: Gracz {playerTotalPower} - Komputer {computerTotalPower}";

    }


    private void ShowGameResult()
    {
        GameResultText.Text = "Gra zakoñczona! ";

        
        if (playerScore > computerScore)
        {
            GameResultText.Text += $"Wygra³eœ! ({playerScore} vs {computerScore})";
        }
        else if (playerScore < computerScore)
        {
            GameResultText.Text += $"Przegra³eœ! ({playerScore} vs {computerScore})";
        }
        else
        {
            GameResultText.Text += $"Remis! ({playerScore} vs {computerScore})";
        }



        // Blokada
        foreach (var child in PlayerCardPanel.Children)
        {
            if (child is Button btn)
            {
                btn.IsEnabled = false;
            }
        }
    }

}