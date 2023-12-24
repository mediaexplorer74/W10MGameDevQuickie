using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GameManager
{
    class Editor
    {
        Map        mp;                                     // reference to Map in Game1
        Tile[,]    tiles;                                  // reference to tiles in Map
        Sheet[]    sheet;                                  // refer to sheet data in Game1
        Input      inp;                                    // refer to input from Game1
        Player     player;                                 // later we'll want to refer to player made in Game1   
        MonsterSys monsterSys;                             // refer to monsterSys "
        string     time_saved = "-";                       // when was the file saved (to confirm file save worked)
        bool       show_colliders;                         // hit f12 to see collision map

        // C O N S T R U C T
        public Editor(Map map, Input inpt, Player playr, Sheet[] sht, MonsterSys monst)
        {
            mp         = map;
            tiles      = mp.tiles;
            inp        = inpt;
            player     = playr;
            sheet      = sht;
            monsterSys = monst; 
        }


        
        // I N S E R T  M O N S T E R
        void InsertMonster(MonsterType monster, Vector2 world_coord)
        {
            monsterSys.AddMonster(monster, world_coord);  // Immediately adds monster to game at this location
            mp.SetMonster(monster);                       // Stores monster initialization point into the tile map
        }



        //------------
        // U P D A T E   ( editor input )
        //------------
        public void Update()
        {
            mp.scroll_offset = Vector2.Zero;
            bool shift_down = inp.shift_down;
            if (inp.Keypress(Keys.Right) || (shift_down && inp.Keydown(Keys.Right))) { mp.loc.X++; Game1.background_pos.X--; }
            if (inp.Keypress(Keys.Left) || (shift_down && inp.Keydown(Keys.Left))) { mp.loc.X--; Game1.background_pos.X++; }
            if (inp.Keypress(Keys.Down) || (shift_down && inp.Keydown(Keys.Down))) { mp.loc.Y++; }
            if (inp.Keypress(Keys.Up) || (shift_down && inp.Keydown(Keys.Up))) { mp.loc.Y--; }

            // PREVENT OUT OF BOUNDS
            if (mp.loc.X >= Map.TILES_WIDE) { mp.loc.X = Map.TILES_WIDE - 1; Game1.background_pos.X++; }
            if (mp.loc.X < 0) { mp.loc.X = 0; Game1.background_pos.X--; }
            if (mp.loc.Y >= Map.TILES_HIGH) { mp.loc.Y = Map.TILES_HIGH - 1; }
            if (mp.loc.Y < 0) { mp.loc.Y = 0; }

            // DELETE TILE
            if (inp.Keypress(Keys.Delete) || inp.Keypress(Keys.Back)) { mp.DeleteTile(); monsterSys.Delete(); }

            // SET COLLIDER
            if (inp.Keydown(Keys.Insert)) mp.SetType();

            // ADD TILE
            if (inp.Keypress(Keys.Q)) mp.AddTile(1); if (inp.Keypress(Keys.W)) mp.AddTile(2); if (inp.Keypress(Keys.E)) mp.AddTile(3);
            if (inp.Keypress(Keys.R)) mp.AddTile(4); if (inp.Keypress(Keys.T)) mp.AddTile(5); if (inp.Keypress(Keys.Y)) mp.AddTile(6);
            if (inp.Keypress(Keys.U)) mp.AddTile(7); if (inp.Keypress(Keys.I)) mp.AddTile(8); if (inp.Keypress(Keys.O)) mp.AddTile(9);
            if (inp.Keypress(Keys.P)) mp.AddTile(10); if (inp.Keypress(Keys.A)) mp.AddTile(11); if (inp.Keypress(Keys.S)) mp.AddTile(12);
            if (inp.Keypress(Keys.D)) mp.AddTile(13); if (inp.Keypress(Keys.F)) mp.AddTile(14); if (inp.Keypress(Keys.G)) mp.AddTile(15);
            if (inp.Keypress(Keys.H)) mp.AddTile(16);

            // GET WORLD POS OF TILE (to edit)
            Vector2 world_coord = Conv.tile_to_world(mp.loc);

            // ADD CHARACTER / MONSTERS
            if (!tiles[mp.loc.X, mp.loc.Y].is_solid)
            {
                if (inp.Keypress(Keys.D1)) InsertMonster(MonsterType.Mouster, world_coord);
                if (inp.Keypress(Keys.D2)) InsertMonster(MonsterType.Hellcat, world_coord); 
            }

            // PLACE PLAYER
            if (inp.Keypress(Keys.M))
            {
                mp.startData.x = mp.loc.X; mp.startData.y = mp.loc.Y;
                player.pos = world_coord + new Vector2(-32, -32); 
            }
            // SHOW HELPERS (ie: colliders)
            if (inp.Keypress(Keys.F12)) show_colliders = !show_colliders;

            // EDIT A TILE
            if (tiles[mp.loc.X, mp.loc.Y].index > 0)
            {
                if (inp.Keydown(Keys.OemPeriod)) tiles[mp.loc.X, mp.loc.Y].rot += 0.01f;
                if (inp.Keydown(Keys.OemComma)) tiles[mp.loc.X, mp.loc.Y].rot -= 0.01f;
                if (inp.Keydown(Keys.OemPlus)) tiles[mp.loc.X, mp.loc.Y].scale += new Vector2(0.01f, 0.01f);
                if (inp.Keydown(Keys.OemMinus)) tiles[mp.loc.X, mp.loc.Y].scale -= new Vector2(0.01f, 0.01f);
                if (inp.Keydown(Keys.NumPad8)) tiles[mp.loc.X, mp.loc.Y].offset.Y--;
                if (inp.Keydown(Keys.NumPad2)) tiles[mp.loc.X, mp.loc.Y].offset.Y++;
                if (inp.Keydown(Keys.NumPad4)) tiles[mp.loc.X, mp.loc.Y].offset.X--;
                if (inp.Keydown(Keys.NumPad6)) tiles[mp.loc.X, mp.loc.Y].offset.X++;
                if (inp.Keydown(Keys.PageUp)) tiles[mp.loc.X, mp.loc.Y].overlap = true;
                if (inp.Keydown(Keys.PageDown)) tiles[mp.loc.X, mp.loc.Y].overlap = false;
                // reset tile modifications:
                if (inp.Keypress(Keys.Home))
                {
                    tiles[mp.loc.X, mp.loc.Y].offset = sheet[tiles[mp.loc.X, mp.loc.Y].index].offset;
                    tiles[mp.loc.X, mp.loc.Y].rot = 0;
                    tiles[mp.loc.X, mp.loc.Y].scale = Vector2.One;
                }
            }

            // SAVE LEVEL MAP
            if (shift_down && inp.Keypress(Keys.D4))
            {
                SaveLevel(Game1.LEVEL_NAME); 
            }

            // LOAD LEVEL MAP
            if (shift_down && inp.Keypress(Keys.D1)) {
                if (File.Exists(Game1.LEVEL_NAME))
                {
                    LoadLevel(Game1.LEVEL_NAME);
                }
            } 
        } // Update


        // DRAW LOCATORS
        public void DrawLocators(SpriteBatch spriteBatch, Texture2D tiles_image, Vector2 screen_center) 
        {
            // show the tile location: 
            spriteBatch.Draw(tiles_image, screen_center, new Rectangle(960, 0, 63, 63), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            // show player starting location if visible: 
            if ((mp.startData.x > mp.a1) && (mp.startData.x < mp.a2) && (mp.startData.y > mp.b1) && (mp.startData.y < mp.b2))
            {
                float x = mp.sx + 64.0f * (mp.startData.x - mp.a1);
                float y = mp.sy + 64.0f * (mp.startData.y - mp.b1);
                spriteBatch.Draw(tiles_image, new Vector2(x, y), new Rectangle(960, 0, 63, 63), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            // MONSTERS LOCATORS: 
            monsterSys.Draw();                             // shows monsters
            mp.DrawTiles(show_colliders, edit_mode: true); // show monster start locations
        }


        // DRAW INSTRUCTIONS
        public void DrawInstructions(SpriteBatch spriteBatch, Texture2D far_background, SpriteFont font, int screenH)
        {
            spriteBatch.Draw(far_background, new Rectangle(0, 0, 200, screenH), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, 150)); // DARK RECTANGLE (makes text easier to read)
            spriteBatch.DrawString(font, "A-Z = add tile", new Vector2(10, 10), Color.LimeGreen);
            spriteBatch.DrawString(font, "del = delete (set empty)", new Vector2(10, 30), Color.LimeGreen);
            spriteBatch.DrawString(font, "ins = set collider", new Vector2(10, 50), Color.LimeGreen);
            spriteBatch.DrawString(font, "$ = Save to: "+Game1.LEVEL_NAME+"  -- time saved: " + time_saved, new Vector2(10, 70), Color.LimeGreen);
            spriteBatch.DrawString(font, "! = Load from: " + Game1.LEVEL_NAME, new Vector2(10, 90), Color.LimeGreen);
            spriteBatch.DrawString(font, "* = Load from backup ", new Vector2(10, 110), Color.LimeGreen);
            spriteBatch.DrawString(font, "M = player position", new Vector2(10, 130), Color.LimeGreen);
            spriteBatch.DrawString(font, "< > rotate", new Vector2(10, 150), Color.LimeGreen);
            spriteBatch.DrawString(font, "- + scale", new Vector2(10, 170), Color.LimeGreen);
            spriteBatch.DrawString(font, "Page Up/Dn = overlap", new Vector2(10, 190), Color.LimeGreen);
            spriteBatch.DrawString(font, "numpad = offset", new Vector2(10, 210), Color.LimeGreen);
            spriteBatch.DrawString(font, "home = reset tile", new Vector2(10, 230), Color.LimeGreen);
            spriteBatch.DrawString(font, "enter = test level", new Vector2(10, 250), Color.LimeGreen);
            spriteBatch.DrawString(font, "F12 - show colliders", new Vector2(10, 270), Color.LimeGreen);
            spriteBatch.DrawString(font, "1-9 = add characters", new Vector2(10, 290), Color.LimeGreen);
        }


        #region TIPS__FOR_GIANT_LEVELS
        // *** TIP *** (for level files) 
        // For huge levels with massive amounts of data (like a huge 3D game) - you may want more efficiency with your files and data (storage and speed)
        // This is usually a non-issue even for fairly large 2D levels unless the tile data gets out of control - to avoid this: 

        // Tip 1) Use booleans (or flag combos) to have the game generate special tiles with special animation parameters -- these are seperate from the Tile class:
        // ie: tile.type == TilType.Grass; 
        // and you have stuff like: GrassTile grass[], WaterTile water[], ConveyorTile conveyor[], etc... (and these are dynamically allocated at startup after 
        // calling a method to pre-process the tiles based on tile.type and the tile dispatches these other ones when it see's the type flag during loops - fast and saves memory and reduces load time        
        // For example: Water may have various animation attributes that no other tile type has... so it would be a bad idea to put all those parameters into Tile - this fixes the problem

        // TIP 2) Much level data can be be spans of x,y data rather than individual tiles
        // ie: "SPAN", 10, 5
        // So next tile to read should be cloned and span 10 tiles horizontally and 5 down

        // TIP 3) 
        // Only record non-default information for tiles. So for example... if there's no rotation... don't save it to file(and so don't read it)... because it's 0 to begin with anyway... 

        // TIP 4)
        // Procedurally generated elements[like decorative vegitation] (don't need details for these - just a general region and anchor points) ... generation can also be set up to occur in hardware

        // TIP 5) 
        // Later when you're sure you won't need to modify your data layouts;
        // you may want to store files as a binary (ie: .DAT) in which case you'll need to make Serialize and Deserialize methods for objects to be written or read (and may want to do versioning to be safe)
        // This tip goes beyond the scope of this tutorial but you can look it up. 
        // Another thing some ppl do is create/use a tool for the monogame-pipeline to process the level data.
        #endregion


        // L O A D  L E V E L 
        public async void LoadLevel(string level_name)
        {
            Windows.Storage.StorageFolder AppStorageFolder =
            // Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.ApplicationModel.Package.Current.InstalledLocation;

            if (!File.Exists(level_name)) return;

            StorageFile level_file = await AppStorageFolder.GetFileAsync(level_name);

            mp.ClearMap();

            using (FileStream level_fs = new FileStream(level_file.Path, FileMode.OpenOrCreate))
            {

              using (StreamReader reader = new StreamReader(/*level_name*/level_fs))
              {

                string line = reader.ReadLine(); string[] parts = line.Split(',');
                mp.startData.x = Convert.ToInt32(parts[0]);
                mp.startData.y = Convert.ToInt32(parts[1]);
                mp.loc.X = mp.startData.x;
                mp.loc.Y = mp.startData.y;  // position the map at start position of player
                player.pos = Conv.tile_to_world(mp.loc) + new Vector2(-32, -32); // SET PLAYER's POSITION (for player object)
                int x = 0, y = 0;
                while (reader.EndOfStream != true)
                {
                    line = reader.ReadLine(); parts = line.Split(',');
                    switch (parts[0])
                    {
                        case "XY": x = Convert.ToInt32(parts[1]); y = Convert.ToInt32(parts[2]); break;
                        case "INDEX": tiles[x, y].index = Convert.ToInt32(parts[1]); break;
                        case "TYPE": tiles[x, y].type = (TileType)Convert.ToInt32(parts[1]); break;
                        case "OFFSET": tiles[x, y].offset.X = Convert.ToInt32(parts[1]); tiles[x, y].offset.Y = Convert.ToInt32(parts[2]); break;
                        case "ROT": tiles[x, y].rot = Convert.ToSingle(parts[1]); break;
                        case "SCALE": tiles[x, y].scale.X = Convert.ToSingle(parts[1]); tiles[x, y].scale.Y = Convert.ToSingle(parts[2]); break;
                        case "OVERLAP": tiles[x, y].overlap = Convert.ToBoolean(parts[1]); break;
                        case "MONSTER": x = Convert.ToInt32(parts[1]); y = Convert.ToInt32(parts[2]);
                            tiles[x, y].monster_start = (MonsterType)(Convert.ToInt32(parts[3])); break;
                    }
                }
              }

            }


            // PRE-PROCESS LOADED TILES (fix tile data)
            for (int b = 0; b < Map.TILES_HIGH; b++)
            {
                for (int a = 0; a < Map.TILES_WIDE; a++)
                {
                    int i = tiles[a, b].index;
                    if (tiles[a, b].monster_start != MonsterType.None) {
                        monsterSys.AddMonster(tiles[a,b].monster_start, Conv.tile_to_world(new Point(a,b))); // ADD A MONSTER AT THIS LOCATION 
                    }
                    TileType typ = sheet[i].type;
                    if (typ == TileType.empty) continue; 

                    // PROVIDE DATA FOR TILE CLUSTER 
                    if ((typ == TileType.solid)    || (typ == TileType.spring) 
                     || (typ == TileType.platform) || (typ == TileType.spikes))
                    {
                        for (int d = b; d < b + sheet[i].tiles_high; d++)
                        {
                            for (int c = a; c < a + sheet[i].tiles_wide; c++)
                            {
                                tiles[c, d].overlap = true; tiles[c, d].stand_on = true;
                                if (typ == TileType.spikes) { tiles[c, d].spikes = true; tiles[c, d].is_solid = true; }
                                else if (typ == TileType.solid) tiles[c, d].is_solid = true;
                            }
                        }
                    }
                }
            }
        } // Load Level



        // S A V E  L E V E L 
        public async void SaveLevel(string level_name)
        {
            Windows.Storage.StorageFolder AppStorageFolder =
              // Windows.Storage.ApplicationData.Current.LocalFolder;
              Windows.ApplicationModel.Package.Current.InstalledLocation;

            if (File.Exists(level_name))
                File.Copy(level_name, Game1.BACKUP_NAME, true);

            StorageFile level_file = await AppStorageFolder.GetFileAsync(level_name);

            using (FileStream level_fs = new FileStream(level_file.Path, FileMode.OpenOrCreate))
            {

                using (StreamWriter writer = new StreamWriter(/*level_name*/level_fs/*, Encoding.UTF8, 20*/))
                {
                    
                    writer.Write(mp.startData.x.ToString() + ","); 
                    writer.Write(mp.startData.y.ToString() + ","); 
                    writer.WriteLine(); // write start data (player position)

                    for (int y = 0; y < Map.TILES_HIGH; y++)
                    {
                        for (int x = 0; x < Map.TILES_WIDE; x++)
                        {
                            int temp;
                            if ((tiles[x, y].index != 0) || (tiles[x, y].type != TileType.empty))
                            {
                                // store only tiles with used information
                                writer.Write("XY,"); 
                                writer.Write(x.ToString() + ","); 
                                writer.Write(y.ToString() + ","); 
                                writer.WriteLine();

                                writer.Write("INDEX,"); 
                                writer.Write(tiles[x, y].index.ToString() + ","); 
                                writer.WriteLine();
                                temp = (int)tiles[x, y].type;

                                writer.Write("TYPE,"); writer.Write(temp.ToString() + ","); writer.WriteLine();

                                writer.Write("OFFSET,"); 
                                writer.Write(tiles[x, y].offset.X.ToString() + ","); 
                                writer.Write(tiles[x, y].offset.Y.ToString() + ","); writer.WriteLine();
                                
                                writer.Write("ROT,"); 
                                writer.Write(tiles[x, y].rot.ToString() + ","); 
                                writer.WriteLine();
                                
                                writer.Write("SCALE,"); writer.Write(tiles[x, y].scale.X.ToString() + ","); 
                                writer.Write(tiles[x, y].scale.Y.ToString() + ","); writer.WriteLine();
                                
                                writer.Write("OVERLAP,"); writer.Write(tiles[x, y].overlap.ToString() + ","); writer.WriteLine();
                            }
                            if (tiles[x, y].monster_start != MonsterType.None)
                            {
                                temp = (int)tiles[x, y].monster_start;
                                writer.Write("MONSTER,"); writer.Write(x.ToString() + ","); writer.Write(y.ToString() + ",");
                                writer.Write(temp.ToString() + ","); writer.WriteLine();
                            }
                        }
                    }                    
                }
            }
            time_saved = DateTime.Now.ToString(); 
        } // Save Level
        
    }
}
