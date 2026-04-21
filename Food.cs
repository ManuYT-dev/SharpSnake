using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeSpiel
{
    public class Food: Objekt
    {
        private Random random;

        private Canvas canvas;
        private Ellipse foodVisual;
        private double offsetX;
        private double offsetY;

        public Food(Canvas canvas = null)
        {
            this.random = new();

            if (canvas != null)
            {
                this.canvas = canvas;
                this.Draw_Init();
            }
            SnakeLogger.logger.Debug("Neues Food erstellt");
        }

        public void Respawn(List<BodySegment> snakeSegments, List<Food> allFoods)
        {
            List<Tuple<int, int>> freeFields = new List<Tuple<int, int>>();

            for (int x = 0; x < GameSettings.FieldSize; x++)
            {
                for (int y = 0; y < GameSettings.FieldSize; y++)
                {
                    bool isSnake = snakeSegments.Any(s => s.X == x && s.Y == y);
                    bool isOtherFood = allFoods.Any(f => f != this && f.X == x && f.Y == y);

                    if (!isSnake && !isOtherFood)
                    {
                        freeFields.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            if (freeFields.Count > 0)
            {
                var chosen = freeFields[this.random.Next(freeFields.Count)];
                this.X = chosen.Item1;
                this.Y = chosen.Item2;
                this.Draw();
            }
            else
            {
                this.X = -1;
                this.Y = -1;
            }
            SnakeLogger.logger.Debug($"Food ist gerespawnt auf der Position {this.X}, {this.Y}");
        }

        public void Draw_Init()
        {
            this.foodVisual = new Ellipse
            {
                Fill = Brushes.Red,
                Width = GameSettings.CellSize * 0.8,
                Height = GameSettings.CellSize * 0.8
            };

            this.offsetX = (GameSettings.CellSize - foodVisual.Width) / 2;
            this.offsetY = (GameSettings.CellSize - foodVisual.Height) / 2;
            if (this.canvas == null) throw new ArgumentNullException(nameof(this.canvas));
            Canvas.SetLeft(this.foodVisual, -100000);
            Canvas.SetTop(this.foodVisual, -100000);
            this.canvas.Children.Add(this.foodVisual);
        }
        public void Draw()
        {
            double pixelX = this.X * GameSettings.CellSize;
            double pixelY = this.Y * GameSettings.CellSize;

            Canvas.SetLeft(this.foodVisual, pixelX + this.offsetX);
            Canvas.SetTop(this.foodVisual, pixelY + this.offsetY);
            SnakeLogger.logger.Debug("Food wurde gezeichnet");
        }
    }
}
