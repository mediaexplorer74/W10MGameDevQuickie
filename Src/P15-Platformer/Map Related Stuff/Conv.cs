using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    static public class Conv
    {
        static public Point GetTileCoord(Vector2 position, ref Vector2 offset)
        {
            Point tile;
            tile.X = (int)position.X / 64;
            tile.Y = (int)position.Y / 64;
            offset.X = (int)(position.X - tile.X * 64);
            offset.Y = (int)(position.Y - tile.Y * 64);
            return tile; 
        }
        static public Point GetTileCoord(Vector2 position)
        {
            Point tile;
            tile.X = (int)position.X / 64; tile.Y = (int)position.Y / 64;
            return tile; 
        }

        static public Vector2 world_to_screen(Vector2 world_position)
        {
            return world_position - Game1.cam_pos + Game1.screen_center;
        }

        // BBOX WORLD TO SCREEN 
        static public Rectangle bbox_world_to_screen(Vector4 bbox)
        {
            bbox.X = bbox.X - Game1.cam_pos.X + Game1.screen_center.X; bbox.Y = bbox.Y - Game1.cam_pos.Y + Game1.screen_center.Y;
            bbox.Z = bbox.Z - Game1.cam_pos.X + Game1.screen_center.X; bbox.W = bbox.W - Game1.cam_pos.Y + Game1.screen_center.Y;
            Rectangle s_box = new Rectangle((int)bbox.X, (int)bbox.Y, (int)(bbox.Z - bbox.X), (int)(bbox.W - bbox.Y));
            return s_box;
        }

        static public Vector2 tile_to_world(Point tile_loc)
        {
            return new Vector2(tile_loc.X * 64, tile_loc.Y * 64);
        }
    }
}
