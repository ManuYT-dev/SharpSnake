using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeSpiel
{
    /// <summary>
    /// Interaktionslogik für Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private Snake snake;
        private List<Food> FoodList = [];
        private DispatcherTimer gameTimer;
        private int score;

        public Game()
        {
            SnakeLogger.logger.Information($"Neue Runde wird geladen erstellt");
            InitializeComponent();
            InitializeGame(true);
            SnakeLogger.logger.Information($"Neue Runde wurde erfolgreich erstellt");
        }

        private void InitializeGame(bool isNewGame = true)
        {
            SnakeLogger.logger.Debug($"Runde wird initialisiert");
            double w = this.ActualWidth > 0 ? this.ActualWidth : 800;
            double h = this.ActualHeight > 0 ? this.ActualHeight : 600;

            double availableSize = Math.Min(w - 40, h - 160);
            GameSettings.CellSize = availableSize / GameSettings.FieldSize;

            MainCanvas.Width = GameSettings.FieldSize * GameSettings.CellSize;
            MainCanvas.Height = GameSettings.FieldSize * GameSettings.CellSize;

            // Prompt Start: < Bild vom Spiel> Erstelle mir die UI vom folgenden Bild
            if (GridBrush != null)
            {
                GridBrush.Viewport = new Rect(0, 0, GameSettings.CellSize, GameSettings.CellSize);
                GridBrush.ViewportUnits = BrushMappingMode.Absolute;
                Rectangle gridRect = (Rectangle)GridBrush.Visual;
                gridRect.Width = GameSettings.CellSize;
                gridRect.Height = GameSettings.CellSize;
            }
            // Prompt End

            if (isNewGame)
            {
                MainCanvas.Children.Clear();
                this.FoodList.Clear();
                this.score = 0;
                TextBlock_Score.Text = "0";

                this.snake = new Snake(GameSettings.InitialLength);
                MainCanvas.Children.Add(snake);

                for (int i = 0; i < GameSettings.Food;  i++)
                {
                    this.FoodList.Add(new Food(MainCanvas));
                }
                
                foreach (Food food in this.FoodList)
                {
                    food.Draw_Init();
                    food.Respawn(this.snake.bodySegment, this.FoodList);
                }
            }

            if (gameTimer == null)
            {
                gameTimer = new DispatcherTimer();
                gameTimer.Tick += Update;
            }
            this.gameTimer.Interval = TimeSpan.FromMilliseconds(450 / GameSettings.Speed);

            if (isNewGame) this.gameTimer.Start();
            SnakeLogger.logger.Debug($"Initialisierung abgeschlossen");
        }

        public async void Update(object sender, EventArgs e)
        {
            SnakeLogger.logger.Debug($"Runde wird geupdated");
            this.snake.Move();

            // FirstOrDefault ist tuff und gibt das erste Element zurück das true ist und wenn nichts True ist null
            Food eatenFood = this.FoodList.FirstOrDefault(f => f.GetPosition().Equals(this.snake.head.GetPosition()));
            if (eatenFood != null)
            {
                eatenFood.Respawn(this.snake.bodySegment, this.FoodList);
                this.snake.Grow();
                this.score += 1;
                TextBlock_Score.Text = score.ToString();
                this.snake.Draw();
            }

            if (!this.snake.isAlive)
            {
                this.gameTimer.Stop();
                this.ShowGameOver(false);
                return;
            }

            if (Math.Pow(GameSettings.FieldSize, 2) <= this.snake.bodySegment.Count)
            {
                await Task.Delay(50);
                this.gameTimer.Stop();
                this.ShowGameOver(true);
            }
            SnakeLogger.logger.Debug($"Runde erfolgreich geupdated");
        }

        public void Start()
        {
            SnakeLogger.logger.Debug($"Runde wird fortgesetzt");
            gameTimer.Start();
        }

        public void Pause()
        {
            SnakeLogger.logger.Debug($"Runde wurde pausiert");
            this.gameTimer.Stop();
            MainWindow.GoToPage("pause");
        }

        public void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W: case Key.Up: this.snake.ChangeDirection(Direction.Up); break;
                case Key.D: case Key.Right: this.snake.ChangeDirection(Direction.Right); break;
                case Key.S: case Key.Down: this.snake.ChangeDirection(Direction.Down); break;
                case Key.A: case Key.Left: this.snake.ChangeDirection(Direction.Left); break;
                case Key.Escape: this.Pause(); break;
            }
        }

        public void SaveToJson()
        {
            SnakeLogger.logger.Debug($"Runde wird abgespeichert");
            var saveData = new SnakeData
            {
                IsAlive = this.snake.isAlive,
                CurrentDirection = snake.GetSaveData().CurrentDirection,
                LastMovedDirection = snake.GetSaveData().LastMovedDirection,
                BodyPositions = snake.GetSaveData().BodyPositions,
                Score = this.score,
                FoodPositions = this.FoodList.Select(s => s.GetPosition()).ToList(),
                Speed = GameSettings.Speed,
                FieldSize = GameSettings.FieldSize
            };

            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("savegame.json", json);
            SnakeLogger.logger.Debug($"Runde erfolgreich abgespeichert");
        }

        public void LoadFromJson()
        {
            SnakeLogger.logger.Debug($"Lade alte Runde");
            if (!File.Exists("savegame.json"))
            {
                SnakeLogger.logger.Warning($"Spiel konnte nicht geladen werden File existiert nicht");
                return;
            }

            string json = File.ReadAllText("savegame.json");
            SnakeData data = JsonSerializer.Deserialize<SnakeData>(json);
            GameSettings.Speed = data.Speed;
            GameSettings.FieldSize = data.FieldSize;

            this.InitializeGame(false);

            this.score = data.Score;
            TextBlock_Score.Text = score.ToString();

            MainCanvas.Children.Clear();
            this.FoodList.Clear();

            foreach (Tuple<int, int> pair in data.FoodPositions)
            {
                Food new_food = new Food(MainCanvas);
                new_food.UpdatePosition(pair.Item1, pair.Item2);
                new_food.Draw_Init();
                this.FoodList.Add(new_food);
            }

            this.snake = new Snake(1);
            this.snake.LoadFromData(data);
            MainCanvas.Children.Add(this.snake);

            foreach (Food food in this.FoodList) food.Draw();

            this.gameTimer.Start();
            SnakeLogger.logger.Information($"Runde wurde erfolgreich geladen");
        }
        private async void ShowGameOver(bool won)
        {
            SnakeLogger.logger.Information($"Die Runde ist beendet.");
            if (won)
            {
                TextBlock_Status.Text = "YOU WIN!";
                TextBlock_Status.Foreground = Brushes.LimeGreen;
            }
            else
            {
                TextBlock_Status.Text = "YOU LOSE!";
                TextBlock_Status.Foreground = Brushes.Red;
            }
            ScoreboardManager.AddScore(this.score);
            Overlay.Visibility = Visibility.Visible;
            await Task.Delay(2000);

            MainWindow.GoToPage("homepage");
            MainWindow.pages.Remove("game");
        }
    }
}
