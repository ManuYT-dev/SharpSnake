using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeSpiel
{
    public class BodySegment: Objekt
    {
        public BodySegment(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public BodySegment(Tuple<int, int> values)
        {
            this.X = values.Item1;
            this.Y = values.Item2;
        }
    }
}
