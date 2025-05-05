using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace GryKarciane;

public partial class GwintHistoria : Window
{
    public GwintHistoria()
    {
        InitializeComponent();
        LoadResults();
    }
    private void LoadResults()
    {
        List<Gwint.GameResult> results = Gwint.GameResultSaver.LoadResults();

        foreach (var result in results)
        {
            var text = $"Gracz: {result.PlayerName} | Czas: {result.Wynik:mm\\:ss} | {(result.IsWin ? "Wygrana" : "Przegrana")}";
            ResultsList.Items.Add(text);
        }
    }
}