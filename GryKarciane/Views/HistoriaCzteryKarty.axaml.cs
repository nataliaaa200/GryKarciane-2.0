using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using GryKarciane.Views;

namespace GryKarciane;

public partial class HistoriaCzteryKarty : Window
{
    public HistoriaCzteryKarty()
    {
        InitializeComponent();
        LoadResults();
    }

    private void LoadResults()
    {
        List<GryKarciane.Views.CzteryKarty.GameResult> results = GryKarciane.Views.CzteryKarty.GameResultSaver.LoadResults();

        foreach (var result in results)
        {
            var text = $"Gracz: {result.PlayerName} | {(result.IsWin ? "Wygrana" : "Przegrana")}";
            ResultsList.Items.Add(text);
        }
    }
}