using System;
using System.Collections.Generic;
using System.Threading;
using static System.Console;

namespace Labirint
{
    public class Game
    {
        private Field MyField;
        private Player ActualPlayer;
        private List<Key> KeyCollection = new List<Key>();

        public void Start()
        {
            Title = "Welcome to the Maze!";
            CursorVisible = false;
            Intro();
            LevelSelection();
            GameLoop();
        }

        private void BordersOfDraw()
        {
            Clear();
            MyField.Draw();
            MyField.DrawCoins();
            ActualPlayer.Draw();

            // Поместим курсор внизу поля игры
            SetCursorPosition(0, MyField.Rows + 2);
            WriteLine($"Keys collected: {KeyCollection.Count}");
        }

        private void Intro()
        {
            WriteLine("Welcome to the maze");
            WriteLine();
            WriteLine("Instructions:");
            WriteLine("> Use the arrow keys to move.");
            WriteLine("> Try to reach the goal, which looks like this: ");
            ForegroundColor = ConsoleColor.Green;
            WriteLine("X");
            ResetColor();
            WriteLine("> Press any key to start.");
            ReadKey(true);
        }

        public void LevelSelection()
        {
            WriteLine("Select a level:");
            WriteLine("1. Level 1");
            WriteLine("2. Level 2");
            WriteLine("3. Level 3");
            Write("Enter your choice: ");

            int choice;
            while (!int.TryParse(ReadLine(), out choice)  choice < 1  choice > 3)
            {
                WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                Write("Enter your choice: ");
            }

            string levelFile = $"Level{choice}.txt";
            string[,] grid = LevelParse.ParseLevelFile(levelFile);
            MyField = new Field(grid);
            ActualPlayer = new Player(2, 5);
        }

        private void Outro()
        {
            Clear();
            WriteLine("You escaped!");
            WriteLine("Press any key to exit...");
            ReadKey(true);
        }

        private void ActualPlayerInput()
        {
            ConsoleKey key;

            do
            {
                ConsoleKeyInfo infoKey = ReadKey(true);
                key = infoKey.Key;

            } while (KeyAvailable); switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (MyField.AvailablePosition(ActualPlayer.X, ActualPlayer.Y - 1))
                        ActualPlayer.Y -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (MyField.AvailablePosition(ActualPlayer.X, ActualPlayer.Y + 1))
                        ActualPlayer.Y += 1;
                    break;
                case ConsoleKey.LeftArrow:
                    if (MyField.AvailablePosition(ActualPlayer.X - 1, ActualPlayer.Y))
                        ActualPlayer.X -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    if (MyField.AvailablePosition(ActualPlayer.X + 1, ActualPlayer.Y))
                        ActualPlayer.X += 1;
                    break;
                case ConsoleKey.Spacebar:
                    string elementAtPosition = MyField.Target(ActualPlayer.X, ActualPlayer.Y);
                    if (elementAtPosition == "K")
                    {
                        MyField.RemoveKey(ActualPlayer.X, ActualPlayer.Y);
                        KeyCollection.Add(new Key(ActualPlayer.X, ActualPlayer.Y));
                        BordersOfDraw();
                    }
                    else if (elementAtPosition == "D")
                    {
                        OpenDoor();
                        BordersOfDraw();
                    }
                    else if (elementAtPosition == "@")
                    {
                        MyField.CollectCoin(ActualPlayer.X, ActualPlayer.Y);
                        BordersOfDraw();
                    }
                    break;
            }
        }

        private void OpenDoor()
        {
            // Проверяем, есть ли хотя бы один ключ у игрока
            if (KeyCollection.Count > 0)
            {
                // Проверяем, находится ли игрок рядом с дверью и есть ли у него ключ
                if (IsNextToDoorWithKey())
                {
                    MyField.RemoveDoor(ActualPlayer.X, ActualPlayer.Y); // Правильно удаляем дверь, используя текущие координаты игрока
                }
                else
                {
                    WriteLine("You need to be next to the door with a key to open it!");
                    Thread.Sleep(15);
                }
            }
            else
            {
                WriteLine("You need a key to open this door!");
                Thread.Sleep(15);
            }
        }

        private bool IsNextToDoorWithKey()
        {
            // Проверяем клетки вокруг игрока
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int x = ActualPlayer.X + dx;
                    int y = ActualPlayer.Y + dy;

                    // Проверяем, не выходит ли позиция за границы поля
                    if (x >= 0 && x < MyField.Cols && y >= 0 && y < MyField.Rows)
                    {
                        if (MyField.Target(x, y) == "D" && KeyCollection.Exists(k => k.X == x && k.Y == y))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void GameLoop()
        {
            while (true)
            {
                BordersOfDraw();
                ActualPlayerInput();
                string elementAtPosition = MyField.Target(ActualPlayer.X, ActualPlayer.Y);
                if (elementAtPosition == "X")
                {
                    break;
                }
                Thread.Sleep(15);
            }

            Outro();
        }
    }
}
