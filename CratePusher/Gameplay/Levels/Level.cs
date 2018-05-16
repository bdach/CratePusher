using System;
using System.Collections.Generic;
using System.Linq;
using CratePusher.Input;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Levels
{
    public class Level
    {
        public FieldType[,] Fields { get; }
        public bool[,] PaintFloor { get; }
        public Point PlayerPosition { get; set; }
        public int Width => Fields.GetLength(1);
        public int Height => Fields.GetLength(0);

        public Level(IList<string> rows)
        {
            var height = rows.Count;
            var width = rows.Select(r => r.Length).Max();
            Fields = new FieldType[height, width];
            PaintFloor = new bool[height, width];
            Point? initialPosition = null;
            for (int y = 0; y < height; ++y)
            {
                var line = rows[y];
                for (int x = 0; x < line.Length; ++x)
                {
                    switch (line[x])
                    {
                        case '#':
                            Fields[y, x] = FieldType.Wall;
                            PaintFloor[y, x] = true;
                            break;
                        case '$':
                            Fields[y, x] = FieldType.Stone;
                            break;
                        case '.':
                            Fields[y, x] = FieldType.Slot;
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
                if (PaintFloor[y, x])
                {
                    continue;
                }
                PaintFloor[y, x] = true;
                foreach ((var dx, var dy) in neighbors)
                {
                    var newX = x + dx;
                    var newY = y + dy;
                    var point = new Point(newX, newY);
                    if (newX >= 0 && newX < Width && newY >= 0 && newY < Height && !PaintFloor[newY, newX])
                    {
                        positionStack.Push(point);
                    }
                }
            }
        }
    }
}