using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    class Map
    {
        public const int TILES_WIDE = 100, TILES_HIGH  = 50;     // map dimensions
        public const int TILE_SPAN_X = 14, TILE_SPAN_Y = 10;     // number of tiles processed on the screen (14 * 2= 28 horizontal, 10 * 2 = 20 verticle) 

        // MAP DATA
        public Tile[,]   tiles;                                  // map tiles         
        public StartData startData;                              // info about starting state of map (like player start position) 
        public BounceMgr bounceMgr;                              // manages bounce events (triggered by players or monsters)        
        public Point     loc;                                    // tiles map location (used to track what section of tiles to draw)                
        Sheet[]          sheet;                                  // refer to the sheet data in Game1
        ProcessedTile[]  overlapTiles;                           // tracks tiles to overlap into scene
        ProcessedTile[]  crystals;                               // tracks crystals to be rendered differently                
        Color            crystal_color;                          // color/tint of crytals (animated)
        int              overlap_count, crystal_count;           // number of layer2 tiles in the scene,  number of crystals to be rendered with crystal shader                
        public Vector2   scroll_offset;                          // smooth scroll between tile section updates using this (0-64, 0-64)
        public int       a1, b1, a2, b2;                         // start and end of visible tiles 
        public float     sx, sy;                                 // start coords (top-left) of tiles to draw on screen

        Texture2D        tiles_image;                            // refers to tiles image from Game1
        Vector2          screen_center;                          // center of screen for a tile
        float            timer;                                  // using this to get a fluctuating wave from sin or cos    
        SpriteBatch      spriteBatch;                            // refer to spriteBatch in Game1   


        // C O N S T R U C T
        public Map(Sheet[] sht, SpriteBatch sprBatch)
        {
            sheet       = sht;
            spriteBatch = sprBatch;
            tiles       = new Tile[TILES_WIDE, TILES_HIGH];                      // allocate memory
            // fill it in with default values                                    (prefer to avoid null values - totally empty tiles not recorded to file anyway)
            for (int b = 0; b < TILES_HIGH; b++)
            {
                for (int a = 0; a < TILES_WIDE; a++)
                {
                    tiles[a, b] = new Tile(0, TileType.empty);
                }
            }            
            overlapTiles = new ProcessedTile[250];                                // up to 250 overlap tiles on screen at once   
            crystals     = new ProcessedTile[250];                                // up to 250 crystals on screen at once   
            for (int a = 0; a < 250; a++) overlapTiles[a] = new ProcessedTile();
            for (int a = 0; a < 250; a++) crystals[a]     = new ProcessedTile();

            // EDITOR START LOCATION:
            loc = new Point(5, 5);
            startData = new StartData(loc.X, loc.Y);                              // init player start position        
            screen_center = Game1.screen_center;
            
            bounceMgr = new BounceMgr(tiles);                                     // init bounce manager (for spring tiles)
        }


        //----------------------------
        // S E T  T I L E S  I M A G E (texture sprite-sheet for tiles)
        //----------------------------
        public void SetTilesImage(Texture2D tilesPic)
        {
            tiles_image = tilesPic;                                              // keep original image in Game1 so can use it in multiple circumstances/places
        }


        //---------------
        // A D D  T I L E
        //---------------
        public void AddTile(int i) {
            DeleteTile();                                                       // Delete any old tile occupations that might overlap
            tiles[loc.X, loc.Y].index = i;
            tiles[loc.X, loc.Y].offset = sheet[i].offset;
            for (int b = loc.Y; b < loc.Y + sheet[i].tiles_high; b++) {
                if (b>=(TILES_HIGH-1)) break;
                for (int a = loc.X; a < loc.X + sheet[i].tiles_wide; a++)
                {
                    if (a >= (TILES_WIDE - 1)) break;
                    TileType type = sheet[i].type;
                    tiles[a, b].type = type; 
                    if ((type == TileType.solid)       || (type == TileType.spring)
                        || (type == TileType.platform) || (type == TileType.spikes))
                    {
                        tiles[a, b].overlap = true; tiles[a, b].stand_on = true;
                        if (type == TileType.spikes) { tiles[a, b].spikes = true; tiles[a, b].is_solid = true; }
                        else if (type == TileType.solid) tiles[a, b].is_solid = true;
                    }
                }
            }
        }


        //-----------------
        // S E T  T Y P E
        //-----------------
        public void SetType(TileType type = TileType.solid)
        {
            int a = loc.X, b = loc.Y;
            tiles[a, b].type = type;
            if ((type == TileType.solid) || (type == TileType.spring) || (type == TileType.platform) || (type == TileType.spikes)) {
                tiles[a, b].overlap = true; tiles[a, b].stand_on = true;
                if (type == TileType.spikes) { tiles[a, b].spikes = true; tiles[a, b].is_solid = true; }
                else if (type == TileType.solid) tiles[a, b].is_solid = true;
            }

        }


        // SET MONSTER
        public void SetMonster(MonsterType monster)
        {
            tiles[loc.X, loc.Y].monster_start = monster;
        }


        //---------------------
        // D E L E T E  T I L E
        //---------------------
        public void DeleteTile()
        {
            int i = tiles[loc.X, loc.Y].index;
            for (int b = loc.Y; b < loc.Y + sheet[i].tiles_high; b++) {
                if (b>=(TILES_HIGH-1)) break;
                for (int a = loc.X; a < loc.X + sheet[i].tiles_wide; a++)
                {
                    if (a >= (TILES_WIDE - 1)) break;
                    tiles[a, b].Clear(); 
                }
            }
        }
        // CLEAR MAP
        public void ClearMap() { for (int b=0; b<TILES_HIGH; b++) {for(int a=0; a<TILES_WIDE;a++) {tiles[a,b].Clear();}}}


        //-------------------
        // A D D  B O R D E R 
        //-------------------
        public void AddBorder(int i)
        {
            for (int a = 0; a < TILES_WIDE; a++) { loc.X = a; loc.Y = 0; AddTile(i); loc.Y = TILES_HIGH - 1; AddTile(i); }
            for (int a = 0; a < TILES_HIGH; a++) { loc.X = 0; loc.Y = a; AddTile(i); loc.X = TILES_WIDE - 1; AddTile(i); }
        }


        //----------------------
        // U P D A T E   V A R S  (for updates that apply to both editor and play modes) 
        //----------------------
        public void UpdateVars()
        {
            // UPDATE CRYSTAL COLOR
            timer += 0.05f;
            double t = (double)timer;
            // make color channels wave between 0.8 to 1.0 (slightly different wave for each channel)
            crystal_color = new Color((float)Math.Sin(t) * 0.2f + 0.8f, (float)Math.Sin(t + 0.5) * 0.2f + 0.8f, (float)Math.Sin(t + 1.0) * 0.2f + 0.8f, 255);
        }


        //----------------------------
        // W O R L D  T O  C A M E R A -- MATCH WORLD VIEW TO CAMERA
        //----------------------------
        public void world_to_camera(Vector2 cam_pos, ref Vector2 background_pos)
        {
            scroll_offset = Vector2.Zero;
            loc = Conv.GetTileCoord(cam_pos, ref scroll_offset);       // get location of center of screen as a tile coordinate in the map (and map's scroll offset [0-64, 0-64])
            background_pos.X = (loc.X * 64 + scroll_offset.X) * -0.5f; // reminder: using wrap mode to draw background (so values will wrap)
            background_pos.Y = (loc.Y * 64 + scroll_offset.Y) * -0.5f; // so set scroll of background to half of total world scroll at this location (half since it scrolls slower)
        }


        //-------------------
        // D R A W  T I L E S 
        //-------------------
        public void DrawTiles(bool draw_colliders = false, bool edit_mode = false)
        {
            // get the region of tiles to draw(top to bottom, left to right) on the screen based on map position:
            b1 = loc.Y - TILE_SPAN_Y;
            b2 = loc.Y + TILE_SPAN_Y;
            a1 = loc.X - TILE_SPAN_X;
            a2 = loc.X + TILE_SPAN_X;

            // prevent out-of-bounds:
            if (b1 < 0) { b1 = 0; if (b2 < 0) b2 = 0; }
            if (b2 >= TILES_HIGH) { b2 = TILES_HIGH - 1; if (b1 >= TILES_HIGH) b1 = TILES_HIGH - 1; }
            if (a1 < 0) { a1 = 0; if (a2 < 0) a2 = 0; }
            if (a2 >= TILES_WIDE) { a2 = TILES_WIDE - 1; if (a1 >= TILES_WIDE) a1 = TILES_WIDE - 1; }

            // calculate start coordinates for drawing tiles:
            int bdif = loc.Y - b1; // how many tiles up from middle of screen
            int adif = loc.X - a1; // how many tiles left from middle of screen

            sx = screen_center.X - adif * 64.0f; // calculate starting x coordinate of tiles shown on screen
            sy = screen_center.Y - bdif * 64.0f; // calculate starting y coordinate of tiles shown on screen

            // draw section of tiles that should be seen:
            float x, y;        // screen coord of tile
            int a, b, i;       // current tile indices and sheet index
            Sheet sh;          // reference to a sheet
            overlap_count = 0; // count the overlapping tiles (store info for them to draw later) 
            crystal_count = 0; // 
            Vector2 tile_pos;  // final tile position on screen (with offsets)

            b = b1; y = sy;
            while (b < b2)
            {
                a = a1; x = sx;
                while (a < a2)
                {
                    i = tiles[a, b].index;
                    tile_pos = new Vector2(x, y) - scroll_offset; // x,y location - scrolling amount (0-64) [ ie: scroll 0-64 then new tile loc ]

                    // E D I T O R   L O C A T I O N  H E L P E R S  ( EDIT MODES ONLY ) ----------------------------------------------------------------------
                    // if draw_colliders is true then all we do is draw the colliders and continue (skips the other drawing since it was already drawn)
                    if (draw_colliders) {
                        if ((tiles[a, b].is_solid) || (tiles[a, b].stand_on)) {
                            Vector2 siz = Vector2.One;
                            if (!tiles[a, b].is_solid) siz = new Vector2(1, 0.4f);
                            spriteBatch.Draw(tiles_image, tile_pos, new Rectangle(960, 0, 63, 63), Color.Purple, 0f, Vector2.Zero, siz, SpriteEffects.None, 0f);
                            a++; x += 64.0f; continue; 
                        }
                    }
                    if (edit_mode) {
                        if (tiles[a, b].monster_start != MonsterType.None) {
                            spriteBatch.Draw(tiles_image, tile_pos, new Rectangle(960, 0, 63, 63), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        a++; x += 64.0f; continue;
                    }//----------------------------------------------------------------------------------------------------------------------------------------


                    // A C T U A L  T I L E  I M A G E S  A R E   D R A W N -----------------------------------------------------------------------------------
                    if (i == 0) { a++; x += 64.0f; continue; }                       // empty so skip to next one
                    sh = sheet[i];
                    tile_pos += tiles[a, b].offset;                                  // make sure to add the tile's offset (editor-customized placement of image)
                    // STORE OVERLAPS ... ELSE DRAW IT
                    if (tiles[a, b].overlap) { overlapTiles[overlap_count].Add(tile_pos, sh.rect, tiles[a, b].rot, tiles[a, b].scale); overlap_count++; }
                    else if (tiles[a, b].type == TileType.reflector) { crystals[crystal_count].Add(tile_pos, sh.rect, tiles[a, b].rot, tiles[a, b].scale); crystal_count++; }
                    else spriteBatch.Draw(tiles_image, tile_pos, sh.rect, Color.White, tiles[a, b].rot, Vector2.Zero, tiles[a, b].scale, SpriteEffects.None, 0f);

                    a++; x += 64.0f;
                }
                b++; y += 64.0f;
            }
        } // ^^^ DrawTiles ^^^



        //-------------------------
        // D R A W  C R Y S T A L S
        //-------------------------  
        public void DrawCrystals()
        {
            int i = 0;
            while (i < crystal_count)
            {
                spriteBatch.Draw(tiles_image, crystals[i].pos, crystals[i].rect, crystal_color, crystals[i].rot, Vector2.Zero, crystals[i].scale, SpriteEffects.None, 0f);
                i++;
            }
        }



        //-------------------------
        // D R A W  O V E R L A P S ( probably more than half the tiles are overlaps [drawn on top of character layer] )  
        //-------------------------  
        public void DrawOverlaps()
        {
            int i = 0;
            while (i < overlap_count)
            {
                spriteBatch.Draw(tiles_image, overlapTiles[i].pos, overlapTiles[i].rect, Color.White, overlapTiles[i].rot, Vector2.Zero, overlapTiles[i].scale, SpriteEffects.None, 0f);
                i++;
            }
        }
    }
}
