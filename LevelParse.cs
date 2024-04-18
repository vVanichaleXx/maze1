using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Labirint
{

    internal class LevelParse
    {
        public static string[,] ParseLevelFile(string levelFile)
        {
            string[] lines = File.ReadAllLines(levelFile);
            string FirstLine = lines[0];
            int rows = lines.Length;
            int colums = FirstLine.Length;
            string[,] grid = new string[rows, colums];

            for (int y = 0; y < rows; y++)
            {
                string line = lines[y];
                for (int x = 0; x < colums; x++)
                {
                    char ActualPlayer = line[x];
                    grid[y, x] = ActualPlayer.ToString();

                }
            }

            return grid;
        }

    }
}
