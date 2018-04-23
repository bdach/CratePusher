using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CratePusher.Gameplay.Levels
{
    public class LevelImporter : IDisposable
    {
        private readonly StreamReader streamReader;

        public LevelImporter(string filePath)
        {
            streamReader = new StreamReader(filePath);
        }

        public LevelCollection LoadLevels()
        {
            var collection = new LevelCollection();;
            var contents = streamReader.ReadToEnd();
            var lines = contents.Split('\n');
            var level = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.StartsWith(";"))
                {
                    collection.Levels.Add(new Level(level));
                    level.Clear();
                    continue;
                }
                level.Add(line);
            }
            return collection;
        }

        public void Dispose()
        {
            streamReader?.Dispose();
        }
    }
}