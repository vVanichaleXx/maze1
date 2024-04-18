using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Labirint
{
    class Field
    {
        private string[,] Grid;
        public int Rows;
        private int Colums;
        private List<Key> keys = new List<Key>();
        private List<Door> doors = new List<Door>();
        private List<Coin> coins = new List<Coin>();
        public int CoinCount { get; private set; }

        public Field(string[,] grid)
        {
            Grid = grid;
            Rows = Grid.GetLength(0);
            Colums = Grid.GetLength(1);
        }

        public void Draw()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Colums; x++)
                {
                    string element = Grid[y, x];
                    SetCursorPosition(x, y);

                    switch (element)
                    {
                        case "X":
                            ForegroundColor = ConsoleColor.Green;
                            break;
                        case "@":
                            ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case "D":
                            ForegroundColor = ConsoleColor.Red;
                            break;
                        case "K":
                            ForegroundColor = ConsoleColor.Blue;
                            break;
                        default:
                            ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    Write(element);
                }
            }
        }

        public string Target(int x, int y)
        {
            return Grid[y, x];
        }

        public void KeyAdding(Key key)
        {
            keys.Add(key);
        }
        public void DoorAdding(Door door)
        {
            doors.Add(door);
        }
        public void CoinAdding(Coin coin)
        {
            coins.Add(coin);
        }
        public void DrawCoins()
        {
            foreach (var coin in coins)
            {
                SetCursorPosition(coin.X, coin.Y);
                ForegroundColor = ConsoleColor.Yellow;
                Write("@");
            }
        }

        public bool HasCoin(int x, int y)
        {
            return coins.Exists(coin => coin.X == x && coin.Y == y);
        }




        public void CollectCoin(int x, int y)
        {
            Coin coinToRemove = coins.Find(coin => coin.X == x && coin.Y == y);
            foreach (var coin in coins)
            {
                if (coinToRemove != null)
                {
                    coins.Remove(coinToRemove);
                    CoinCount++;
                    WriteLine("Coin collected!");
                    DrawCoins();

                }
            }
        }

        public bool AvailablePosition(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Colums || y >= Rows)
                return false;

            bool hasKey = keys.Exists(k => k.X == x && k.Y == y);

            foreach (var door in doors)
            {
                if (door.X == x && door.Y == y)
                {
                    if (Grid[y, x] == "D" && !hasKey)
                        return false;
                }
            }

            return Grid[y, x] == " " || Grid[y, x] == "X" || Grid[y, x] == "K" || (Grid[y, x] == "D" && hasKey) || Grid[y, x] == "@";
        }

        public void RemoveKey(int x, int y)
        {
            Grid[y, x] = " ";
        }

        public void RemoveDoor(int x, int y)
        {
            Grid[y, x] = " ";
        }
    }
}