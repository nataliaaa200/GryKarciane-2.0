using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Collections.Generic;
using GryKarciane.Models;
using System;

namespace GryKarciane;

public partial class HistoriaOczko : Window
{
    public HistoriaOczko()
    {
        InitializeComponent();
        LoadResults();
    }

    private void LoadResults()
    {
        ResultsList.Items.Clear();

        List<GameResult> results = GameResultSaver.LoadResults();

        if (results == null || results.Count == 0)
        {
            Console.WriteLine("Brak wyników w historii.");
        }
        else
        {
            Console.WriteLine($"Załadowano {results.Count} wyników.");
        }

        foreach (var result in results)
        {
            var textBlock = new TextBlock();
            var text = $"Gracz: {result.PlayerName} | Wynik: {result.Score} | {(result.IsWin ? "Wygrana" : "Przegrana")}";

            textBlock.Text = text;
            textBlock.Foreground = result.IsWin ? Brushes.Green : Brushes.Red;

            ResultsList.Items.Add(textBlock);
        }
    }
}


