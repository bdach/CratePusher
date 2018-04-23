using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CratePusher.Graphics
{
    public class TileSheet
    {
        private const int Width = 13;
        public const int TileSize = 128;

        private readonly Texture2D tileSheetTexture;

        public TileSheet(Texture2D tileSheetTexture)
        {
            this.tileSheetTexture = tileSheetTexture;
        }

        public void DrawTile(SpriteBatch spriteBatch, TileType type, Point point, int tileSize)
        {
            var idx = (int) type;
            var x = idx % Width;
            var y = idx / Width;
            var sourceRectangle = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
            spriteBatch.Draw(tileSheetTexture, new Rectangle(point, new Point(tileSize)), sourceRectangle, Color.White);
        }
    }
}