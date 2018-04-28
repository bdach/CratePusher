using System;
using CratePusher.Gameplay.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CratePusher.Graphics
{
    public class LevelRenderer
    {
        private readonly TileSheet tileSheet;
        private readonly int screenWidth;
        private readonly int screenHeight;
        private readonly int maxLevelWidth;
        private readonly int maxLevelHeight;
        private const double MarginProportion = 0.9;

        public LevelRenderer(GraphicsDeviceManager deviceManager, TileSheet tileSheet)
        {
            screenWidth = deviceManager.PreferredBackBufferWidth;
            screenHeight = deviceManager.PreferredBackBufferHeight;
            maxLevelWidth = (int) (screenWidth * MarginProportion);
            maxLevelHeight = (int) (screenHeight * MarginProportion);
            this.tileSheet = tileSheet;
        }

        public void Render(Level level, SpriteBatch spriteBatch)
        {
            var targetWidth = level.Width * TileSheet.TileSize;
            var targetHeight = level.Height * TileSheet.TileSize;
            var targetTileSize = GetTileSize(targetWidth, targetHeight);
            if (targetTileSize != TileSheet.TileSize)
            {
                targetWidth = level.Width * targetTileSize;
                targetHeight = level.Height * targetTileSize;
            }
            var offset = new Point((screenWidth - targetWidth) / 2, (screenHeight - targetHeight) / 2);
            DrawLevelGrid(level, spriteBatch, offset, targetTileSize);
        }

        private void DrawLevelGrid(Level level, SpriteBatch spriteBatch, Point offset, int targetTileSize)
        {
            for (int y = 0; y < level.Height; ++y)
            {
                for (int x = 0; x < level.Width; ++x)
                {
                    var location = new Point(x * targetTileSize, y * targetTileSize) + offset;
                    if (level.PaintFloor[y, x])
                    {
                        tileSheet.DrawTile(spriteBatch, TileType.GrayFloor, location, targetTileSize);
                    }

                    switch (level.Fields[y, x])
                    {
                        case FieldType.Player:
                            tileSheet.DrawTile(spriteBatch, TileType.PlayerFacingDown, location, targetTileSize);
                            break;
                        case FieldType.Slot:
                            tileSheet.DrawTile(spriteBatch, TileType.TargetField, location, targetTileSize);
                            break;
                        case FieldType.Stone:
                            tileSheet.DrawTile(spriteBatch, TileType.YellowCrate, location, targetTileSize);
                            break;
                        case FieldType.Wall:
                            tileSheet.DrawTile(spriteBatch, TileType.RedWall, location, targetTileSize);
                            break;
                    }
                }
            }
        }

        private int GetTileSize(int fullScaleWidth, int fullScaleHeight)
        {
            var desiredTileSize = TileSheet.TileSize;
            // Make sure we actually can draw at full size
            if (fullScaleHeight > maxLevelHeight || fullScaleWidth > maxLevelWidth)
            {
                // Calculate the scaling factor
                var scalingFactor = Math.Min(
                    (double)maxLevelWidth / fullScaleWidth,
                    (double)maxLevelHeight / fullScaleHeight
                );
                desiredTileSize = (int)(scalingFactor * desiredTileSize);
            }
            return desiredTileSize;
        }
    }
}