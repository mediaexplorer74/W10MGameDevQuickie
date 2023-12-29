using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Map1
    {
        public static Point Size { get; } = new(8, 12);
        public Tile1[,] Tiles { get; }
        public Point TileSize { get; }

        public Vector2 MapToScreen(int x, int y) => new(x * TileSize.X, y * TileSize.Y);
        public (int x, int y) ScreenToMap(Vector2 pos) => ((int)pos.X / TileSize.X,
            (int)pos.Y / TileSize.Y);

        public Map1()
        {
            Tiles = new Tile1[Size.X, Size.Y];
            var texture = Glob.Content.Load<Texture2D>("tile");
            TileSize = new(texture.Width, texture.Height);

            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    Tiles[x, y] = new(texture, MapToScreen(x, y) + (TileSize.ToVector2() / 2), x, y);
                }
            }
        }

        public void Update()
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++) Tiles[x, y].Update();
            }
        }

        public void Draw()
        {
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++) Tiles[x, y].Draw();
            }
        }
    }
}
