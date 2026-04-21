using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeSpiel
{
    public class ScoreEntry
    {
        public int Score { get; set; }
        public DateTime Date { get; set; }
        public string DateString => Date.ToString("dd.MM.yyyy HH:mm");
    }
}
