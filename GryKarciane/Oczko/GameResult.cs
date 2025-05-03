using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace GryKarciane.Models
{
    public class GameResult
    {
        public string PlayerName { get; set; }
        public TimeSpan GameTime { get; set; }
        public int Score { get; set; }
        public bool IsWin { get; set; }
    }

    public static class GameResultSaver
    {
        public static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wyniki_oczko.json");

        private static List<GameResult> _results;

        static GameResultSaver()
        {
            _results = LoadResults(); 
        }

        public static void SaveResult(GameResult result)
        {
            try
            {
                
                _results.Add(result);

                const int maxResults = 10;
                if (_results.Count > maxResults)
                {
                    _results = _results.Skip(_results.Count - maxResults).ToList();
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_results, options);

                Console.WriteLine($"Zapisuję dane do pliku: {FilePath}");
                Console.WriteLine($"Dane do zapisania: {json}");

                File.WriteAllText(FilePath, json);

                Console.WriteLine("Wynik zapisany do pliku."); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisywania wyniku: {ex.Message}");
            }
        }

        public static List<GameResult> LoadResults()
        {
            try
            {

                if (!File.Exists(FilePath))
                {
                    Console.WriteLine("Plik wyników nie istnieje, zwracam pustą listę.");
                    return new List<GameResult>();
                }

                string json = File.ReadAllText(FilePath);
                var results = JsonSerializer.Deserialize<List<GameResult>>(json);

                if (results == null)
                {
                    Console.WriteLine("Deserializacja zwróciła null, zwracam pustą listę.");
                    return new List<GameResult>();
                }

                Console.WriteLine($"Załadowano {results.Count} wyników z pliku.");
                return results;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Błąd podczas ładowania wyników: {ex.Message}");
                return new List<GameResult>(); 
            }
        }
    }

}


