using System.Collections.Generic;
using System.Linq;

namespace CratePusher.Gameplay.Levels
{
    public class Level
    {
        public FieldType[,] Fields { get; }
        public int Width => Fields.GetLength(1);
        public int Height => Fields.GetLength(0);

        public Level(IList<string> rows)
        {
            var height = rows.Count;
            var width = rows.Select(r => r.Length).Max();
            Fields = new FieldType[height, width];
            for (int y = 0; y < height; ++y)
            {
                var line = rows[y];
                for (int x = 0; x < line.Length; ++x)
                {
                    switch (line[x])
                    {
                        case '#':
                            Fields[y, x] = FieldType.Wall;
                            break;
                        case '$':
                            Fields[y, x] = FieldType.Stone;
                            break;
                        case '.':
                            Fields[y, x] = FieldType.Slot;
                            break;
                        case '@':
                            Fields[y, x] = FieldType.Player;
                            break;
                    }
                }
            }
        }
    }
}