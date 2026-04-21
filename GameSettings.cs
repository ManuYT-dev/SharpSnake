using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeSpiel
{
    public static class GameSettings
    {
        public static int Speed { get; set; }
        public static int FieldSize { get; set; }
        public static int InitialLength { get; set; }
        public static int Food { get; set; }
        public static double CellSize { get; set; }

        public static void Init(int speed, int fieldSize, int initialLength, int food, double cellsize)
        {
            GameSettings.Speed = speed;
            GameSettings.FieldSize = fieldSize;
            GameSettings.InitialLength = initialLength;
            GameSettings.Food = food;
            GameSettings.CellSize = cellsize;
        }
    }
}
