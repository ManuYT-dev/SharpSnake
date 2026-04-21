using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeSpiel
{
    public class SnakeData
    {
        public bool IsAlive { get; set; }
        public List<Tuple<int, int>> BodyPositions { get; set; }
        public Direction CurrentDirection { get; set; }
        public Direction LastMovedDirection { get; set; }
        public int Score { get; set; }
        public List<Tuple<int, int>> FoodPositions { get; set; }
        public int Speed { get; set; }
        public int FieldSize { get; set; }
    }
}
