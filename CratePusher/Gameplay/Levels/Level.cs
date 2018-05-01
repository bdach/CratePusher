using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CratePusher.Gameplay.Levels
{
    public class Level
    {
        public FieldType[,] Fields { get; }
        public bool[,] PaintFloor { get; }
        public int Width => Fields.GetLength(1);
        public int Height => Fields.GetLength(0);

        public Level(IList<string> rows)
        {
            var height = rows.Count;
            var width = rows.Select(r => r.Length).Max();
            Fields = new FieldType[height, width];
            PaintFloor = new bool[height, width];
            Point? playerPosition = null;
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
                            Fields[y, x] = FieldType.Player;
                            playerPosition = new Point(x, y);
                            break;
                    }
                }
            }
            if (!playerPosition.HasValue)
            {
                throw new ArgumentException("No player in level");
            }
            FloodFillFloor(playerPosition.Value);
        }

        private void FloodFillFloor(Point playerPosition)
        {
            var positionStack = new Stack<Point>();
            var neighbors = new List<(int, int)>
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };
            positionStack.Push(playerPosition);
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