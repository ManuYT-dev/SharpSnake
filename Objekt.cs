using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeSpiel
{
    public class Objekt
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public Tuple<int, int> GetPosition() => new(this.X, this.Y);

        public void UpdatePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
            SnakeLogger.logger.Debug("Update Objekt Positon.");
        }

        public void UpdatePosition(Tuple<int, int> positions)
        {
            this.X = positions.Item1;
            this.Y = positions.Item2;
            SnakeLogger.logger.Debug("Update Objekt Positon.");
        }
    }
}
