using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Xml.Serialization;
using GryKarciane.Views;


namespace GryKarciane;

public partial class Historia : Window
{
    public Historia()
    {
        InitializeComponent();
    }

    private void Gapa_Click(object? sender, RoutedEventArgs e)
    {
        var historiaGapa = new HistoriaGapa();
        historiaGapa.Show();
    }

    private void Oczko_Click(object? sender, RoutedEventArgs e)
    {
        var historiaOczko = new HistoriaOczko();
        historiaOczko.Show();
    }

    private void Kinga_Click(object? sender, RoutedEventArgs e)
    {

    }

    private void Gwint_Click(object? sender, RoutedEventArgs e)
    {
        var gwintHistoria = new GwintHistoria();
        gwintHistoria.Show();
    }
}