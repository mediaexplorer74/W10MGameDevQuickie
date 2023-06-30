using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

using TiledSharp;

namespace GameManager.JRPG
{    
    
    public class Zone
    {
        // Terrain information
        public List<uint[,]> map;              // Map layers
        public List<Texture2D> tileset;        // Tilesets
        public int layer_width, layer_height;  // Layer dimensions
        public int tile_width, tile_height;    // Pixel shape of tiles

        public float tile_scale;

        public List<Tile> map_tiles;

        // Navigation
        public Vector2 position;
        public Vector2 last_position;
        public Vector2 direction;
        public Vector2 last_direction;
        public float walking_speed = 30.0f; // Use 60Hz ratios
        public bool is_walking;

        public static Dictionary<ButtonState, Vector2> pad_dir;
        
        // Incorporate a Tiled object's contents into a Zone object
        public /*static*/ Zone Load(TmxMap map, ContentManager content)
        {
            Zone zone = new Zone();

            // Get dimensions
            zone.tile_width = map.TileWidth;//.tilewidth;
            zone.tile_height = map.TileHeight;//.tileheight;

            // Copy the layer tile maps...
            zone.map = new List<uint[,]>();

            try
            {
                foreach (TmxLayer layer in map.Layers)//.layer(s)
                {
                    uint[,] Data = new uint[map.Width, map.Height];

                    try
                    {
                        foreach (TmxLayerTile t in layer.Tiles)
                        {
                            Data[t.X, t.Y] = (uint)t.Gid;
                        }
                    }
                    catch (Exception ex2)
                    {
                        Debug.WriteLine("[ex] foreach (TmxLayerTile... bug: " + ex2.Message);

                        continue;
                    }
                    zone.map.Add(Data);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] zone.map.Add bug: " + ex.Message);
            }

            // Process the tile images
            zone.tileset = new List<Texture2D>();
            foreach (var ts in map.Tilesets)//.tileset
            {
                var tileset_path = Path.GetFileNameWithoutExtension(ts.Image.Source);//ts.image.source
                zone.tileset.Add(content.Load<Texture2D>(tileset_path));
            }
            
            // Process individual tiles as a list
            zone.map_tiles = new List<Tile>();


            // Initial position
            zone.position = Vector2.Zero;
            zone.tile_scale = 3.0f;

            return zone;
        }//Load


        // Draw
        public void Draw(SpriteBatch batch)
        {
            // Kind of a mess... need to consolidate here!

            // Draw the tile map (slow?)
            foreach (var m in map)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    for (int i = 0; i < m.GetLength(0); i++)
                    {
                        if (m[i, j] != 0) // Need a more elegant mask control
                        {
                            var tile_index = m[i, j] - 1;
                            var pos = new Vector2((float)(tile_width * tile_scale * i),
                                                  (float)(tile_height * tile_scale * j));
                            // Need to reduce rect to the game window
                            var rect = new Rectangle(tile_width * (int)tile_index, 0, tile_width, tile_height);

                            batch.Draw(tileset[0], pos, rect, Color.White,
                                 0.0f, this.position, tile_scale, SpriteEffects.None, 0);
                        }//if
                    }//for                
                }//for
            }//foreach
        }//Draw end


        //DrawTiles
        public void DrawTiles(Zone zone)
        {
            // Test method to draw a list of tiles
            // ...
        }

        // UpdatePosition
        public void UpdatePosition(KeyboardState keyboardState, GameTime game_time)
        {
            // Note: We must update before checking, and cannot use an 'else'
            // (Not 100% on the logic here; consider refactoring)

            if (is_walking)
            {
                // Increment the position
                position += direction * walking_speed *
                    (float)game_time.ElapsedGameTime.TotalSeconds;

                var x = (int)(position.X - last_position.X);
                var y = (int)(position.Y - last_position.Y);
                // Flip to not walking if displacement has reached a tile width/height
                if (Math.Abs(x) >= tile_width || Math.Abs(y) >= tile_height)
                {
                    is_walking = false;
                    last_direction = direction;
                }
            }

            // Check if gamepad is pressed, reinstate if necessary
            if (!is_walking)
            {
                direction = new Vector2(0, 0);

                if (keyboardState.IsKeyDown(Keys.Left))// == ButtonState.Pressed)
                    direction -= Vector2.UnitX;

                if (keyboardState.IsKeyDown(Keys.Right))//(pad_state.DPad.Right == ButtonState.Pressed)
                    direction += Vector2.UnitX;

                if (keyboardState.IsKeyDown(Keys.Up))//(pad_state.DPad.Up == ButtonState.Pressed)
                    direction -= Vector2.UnitY;

                if (keyboardState.IsKeyDown(Keys.Down)) //(pad_state.DPad.Down == ButtonState.Pressed)
                    direction += Vector2.UnitY;

                if (direction != Vector2.Zero)
                {
                    last_position = position;
                    is_walking = true;
                }
            }

            //Debug.WriteLine("[i] is_walking = " + is_walking.ToString());
            //Debug.WriteLine("[i] position = " + position.ToString());
        }
    } // end Zone
    
    public class Tile
    {
        // Abstraction
        public int x, y;        // Tile coordinate (x,y)
        public bool visible;
        // + properties(wall, water, etc)

        // Rendering
        public Texture2D set;           // Source tileset
        public Rectangle rect;   // Tileset bounding box
        public Rectangle window_rect;   // Window bounding box

        public Tile(int x_in, int y_in, Texture2D tileset, Rectangle set_rect)
        {
            set = tileset;
            rect = set_rect;
            x = x_in;
            y = y_in;
        }
    }
    
}
