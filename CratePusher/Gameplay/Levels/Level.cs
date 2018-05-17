using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Levels
{
    public class Level
    {
        public bool[,] Floor { get; }
        public HashSet<Point> Walls { get; }
        public HashSet<Point> Goals { get; }
        public HashSet<Point> Crates { get; }
        public Point PlayerPosition { get; set; }

        public int Width => Floor.GetLength(1);
        public int Height => Floor.GetLength(0);

        public Level(IList<string> rows)
        {
            var height = rows.Count;
            var width = rows.Select(r => r.Length).Max();

            Floor = new bool[height, width];
            Walls = new HashSet<Point>();
            Goals = new HashSet<Point>();
            Crates = new HashSet<Point>();

            Point? initialPosition = null;
            for (int y = 0; y < height; ++y)
            {
                var line = rows[y];
                for (int x = 0; x < line.Length; ++x)
                {
                    switch (line[x])
                    {
                        case '#':
                            Walls.Add(new Point(x, y));
                            Floor[y, x] = true;
                            break;
                        case '$':
                            Crates.Add(new Point(x, y));
                            break;
                        case '.':
                            Goals.Add(new Point(x, y));
                            break;
                        case '@':
                            initialPosition = new Point(x, y);
                            break;
                    }
                }
            }
            if (!initialPosition.HasValue)
            {
                throw new ArgumentException("No player in level");
            }
            PlayerPosition = initialPosition.Value;
            FloodFillFloor(initialPosition.Value);
        }

        public bool InBounds(Point point)
        {
            return point.X >= 0 && point.X < Width &&
                   point.Y >= 0 && point.Y < Height;
        }

        private void FloodFillFloor(Point initialPosition)
        {
            var positionStack = new Stack<Point>();
            var neighbors = new List<(int, int)>
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };
            positionStack.Push(initialPosition);
            while (positionStack.Count > 0)
            {
                var nextPoint = positionStack.Pop();
                var x = nextPoint.X;
                var y = nextPoint.Y;
                if (Floor[y, x])
                {
                    continue;
                }
                Floor[y, x] = true;
                foreach ((var dx, var dy) in neighbors)
                {
                    var newX = x + dx;
                    var newY = y + dy;
                    var point = new Point(newX, newY);
                    if (newX >= 0 && newX < Width && newY >= 0 && newY < Height && !Floor[newY, newX])
                    {
                        positionStack.Push(point);
                    }
                }
            }
        }

        public bool LevelDone()
        {
            return Goals.SetEquals(Crates);
        }
    }
}