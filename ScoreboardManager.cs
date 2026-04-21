using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SnakeSpiel
{
    public static class ScoreboardManager
    {
        private const string FilePath = "highscore.json";

        public static List<ScoreEntry> LoadScores()
        {
            SnakeLogger.logger.Debug($"Score wird geladen");
            if (!File.Exists(FilePath))
            {
                SnakeLogger.logger.Warning($"Warning beim Laden, da die Datei nicht existiert.");
                return [];
            }
            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<ScoreEntry>>(json) ;
            }
            catch
            {
                return [];
            }
        }

        public static void AddScore(int score)
        {
            SnakeLogger.logger.Debug($"Neuer Score wird hinzugefügt");
            var scores = LoadScores();
            scores.Add(new ScoreEntry { Score = score, Date = DateTime.Now });
            var sortierteListe = scores.OrderByDescending(s => s.Score).ToList();
            var ohneDuplikate = sortierteListe.DistinctBy(s => new { s.Score, s.DateString }).ToList();
            var top100 = ohneDuplikate.Take(100).ToList();

            File.WriteAllText(FilePath, JsonSerializer.Serialize(top100));
            SnakeLogger.logger.Debug($"Neuer Score erfolgreich hinzugefügt und abgespeichert");
        }
    }
}