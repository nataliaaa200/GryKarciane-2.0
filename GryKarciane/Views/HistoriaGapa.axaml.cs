using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace GryKarciane;

public partial class HistoriaGapa : Window
{
    public HistoriaGapa()
    {
        InitializeComponent();
        LoadResults();

    }

    private void LoadResults()
    {
        List<Gapa.GameResult> results = Gapa.GameResultSaver.LoadResults();

        foreach (var result in results)
        {
            var text = $"Gracz: {result.PlayerName} | Czas: {result.GameTime:mm\\:ss} | {(result.IsWin ? "Wygrana" : "Przegrana")}";
            ResultsList.Items.Add(text); 
        }
    }
}
