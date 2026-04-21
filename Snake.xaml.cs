using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeSpiel
{
    public partial class Snake : UserControl
    {
        public bool isAlive { get; private set; }
        public BodySegment head { get; private set; }
        public List<BodySegment> bodySegment { get; private set; }
        private Direction direction;
        private Direction lastMovedDirection;
        private Tuple<int, int> lastPos;
        private int startLength;

        public Snake(int startLength)
        {
            InitializeComponent();
            this.startLength = startLength;
            this.Reset();
        }

        public void Move()
        {
            SnakeLogger.logger.Debug("Schlange bewegt sich.");
            if (!isAlive) return;

            this.lastMovedDirection = this.direction;

            var headPos = bodySegment[0].GetPosition();
            int nextX = headPos.Item1;
            int nextY = headPos.Item2;

            switch (this.direction)
            {
                case Direction.Up: nextY -= 1; break;
                case Direction.Down: nextY += 1; break;
                case Direction.Left: nextX -= 1; break;
                case Direction.Right: nextX += 1; break;
            }

            if (CheckWallCollsion(nextX, nextY) || CheckCollision(nextX, nextY))
            {
                this.isAlive = false;
                this.Draw();
                return;
            }

            this.lastPos = bodySegment.Last().GetPosition();

            for (int i = bodySegment.Count - 1; i > 0; i--)
            {
                var prevPos = bodySegment[i - 1].GetPosition();
                bodySegment[i].UpdatePosition(prevPos.Item1, prevPos.Item2);
            }

            bodySegment[0].UpdatePosition(nextX, nextY);
            this.head = bodySegment[0];

            this.Draw();
        }

        private bool CheckWallCollsion(int x, int y)
        {
            return x < 0 || x >= GameSettings.FieldSize ||
                   y < 0 || y >= GameSettings.FieldSize;
        }

        public void ChangeDirection(Direction newDir)
        {
            SnakeLogger.logger.Debug($"Richtung der Schlange wird geändert.");
            if (newDir == Direction.Up && lastMovedDirection == Direction.Down) return;
            if (newDir == Direction.Down && lastMovedDirection == Direction.Up) return;
            if (newDir == Direction.Left && lastMovedDirection == Direction.Right) return;
            if (newDir == Direction.Right && lastMovedDirection == Direction.Left) return;

            this.direction = newDir;
        }

        public void Grow()
        {
            SnakeLogger.logger.Debug("Schlange wächst");
            this.bodySegment.Add(new BodySegment(this.lastPos));
        }

        public bool CheckCollision(int x, int y) => 
            this.bodySegment.Skip(1).Any(s => s.X == x && s.Y == y);

        public void Reset()
        {
            SnakeLogger.logger.Debug($"Schlange wird reseted");
            this.bodySegment = [];
            int startX = GameSettings.FieldSize / 2;
            int startY = (GameSettings.FieldSize - this.startLength) / 2;

            for (int i = 0; i < this.startLength; i++)
                this.bodySegment.Add(new BodySegment(startX, startY + i));
            this.direction = Direction.Up;
            this.lastMovedDirection = Direction.Up;
            this.isAlive = true;
            this.head = this.bodySegment.First();
            Draw();
        }

        public void Draw()
        {
            SnakeLogger.logger.Debug("Schlange wird gezeichnet");
            GameCanvas.Children.Clear();

            for (int i = 0; i < bodySegment.Count; i++)
            {
                var seg = bodySegment[i];
                bool isHead = (i == 0);

                Rectangle rect = new Rectangle
                {
                    Width = GameSettings.CellSize - 1,
                    Height = GameSettings.CellSize - 1,
                    Fill = Brushes.Green
                };

                Canvas.SetLeft(rect, seg.X * GameSettings.CellSize);
                Canvas.SetTop(rect, seg.Y * GameSettings.CellSize);
                GameCanvas.Children.Add(rect);

                if (isHead)
                {
                    DrawEyes(seg.X, seg.Y);
                }
            }
        }

        private void DrawEyes(int headX, int headY)
        {
            double eyeSize = GameSettings.CellSize * 0.2;
            double offset = GameSettings.CellSize * 0.2;

            for (int i = 0; i < 2; i++)
            {
                Ellipse eye = new()
                {
                    Width = eyeSize,
                    Height = eyeSize,
                    Fill = Brushes.White
                };

                double xPos = headX * GameSettings.CellSize;
                double yPos = headY * GameSettings.CellSize;

                if (this.direction == Direction.Up || this.direction == Direction.Down)
                {
                    if (i == 0) xPos += offset;
                    else xPos += (GameSettings.CellSize - offset - eyeSize);
                    if (this.direction == Direction.Up) yPos += offset;
                    else yPos += (GameSettings.CellSize - offset - eyeSize);
                }
                else
                {
                    if (this.direction == Direction.Left) xPos += offset;
                    else xPos += (GameSettings.CellSize - offset - eyeSize);
                    if (i == 0) yPos += offset;
                    else yPos += (GameSettings.CellSize - offset - eyeSize);
                }

                Canvas.SetLeft(eye, xPos);
                Canvas.SetTop(eye, yPos);
                GameCanvas.Children.Add(eye);

                Ellipse pupil = new() { Width = eyeSize / 2, Height = eyeSize / 2, Fill = Brushes.Black };
                Canvas.SetLeft(pupil, xPos + eyeSize / 4);
                Canvas.SetTop(pupil, yPos + eyeSize / 4);
                GameCanvas.Children.Add(pupil);
            }
        }

        public SnakeData GetSaveData()
        {
            return new SnakeData
            {
                IsAlive = this.isAlive,
                CurrentDirection = this.direction,
                LastMovedDirection = this.lastMovedDirection,
                BodyPositions = this.bodySegment.Select(s => s.GetPosition()).ToList(),
            };
        }

        public void LoadFromData(SnakeData data)
        {
            SnakeLogger.logger.Debug("Schlange wird geladen von JSON File");
            this.isAlive = data.IsAlive;
            this.direction = data.CurrentDirection;
            this.lastMovedDirection = data.LastMovedDirection;

            this.bodySegment.Clear();
            foreach (var pos in data.BodyPositions)
            {
                this.bodySegment.Add(new BodySegment(pos.Item1, pos.Item2));
            }
            this.head = this.bodySegment.First();
            this.Draw();
        }
    }
}