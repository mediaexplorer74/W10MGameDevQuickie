//#define SHOW_PLAYER_COLLIDER
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace GameManager
{
    public class Game1 : Game
    {
        // CHANGE THIS FOR MAKING A NEW LEVEL                      (use backup to retrieve a copy of old one if you accidentally save over it)
        static public string LEVEL_NAME  = @"Content\\lev1.txt";   // this is stored in the BIN/execute version of Content
        static public string BACKUP_NAME = @"Content\\backup.txt"; 

        // GAME STATES:
        enum GameState { edit, play, game_over, menu, load_level }
        GameState gameState = GameState.play;
        
        // DISPLAY
        const int  SCREENWIDTH = 1024, SCREENHEIGHT = 768;     // TARGET FORMAT
        const bool FULLSCREEN  = false;                        // not fullscreen because using windowed fill-screen mode 
        GraphicsDeviceManager  graphics;
        PresentationParameters pp; 
        SpriteBatch            spriteBatch;
        QuadBatch              quadBatch;
        SpriteFont             font;
        static public int      screenW, screenH;
        static public Vector2  screen_center;

        //INPUT
        Input     inp;

        //HUD
        Hud                   hud;                                  // heads up display (shows life, player status, etc)

        //TEXTURES
        Texture2D             far_background, mid_background;
        Texture2D             tiles_image;        

        //SOURCE RECTANGLES
        Rectangle             screenRect, desktopRect;              // render target size, desktop screen size

        //POSITIONS
        static public Vector2 background_pos;  

        // UTILS
        static public Random  rnd; 

        //RENDERTARGETS
        RenderTarget2D        MainTarget;                           // render to a standard target and fit it to the desktop resolution
        RenderTarget2D        sprite_target;                        // any sprites reflected onto crystals are drawn on here also for crystal effect 

        //FX
        Effect                crystal_fx;                           // reflective crystal effect

        //MAP DATA       
        const int             MAX_SHEET_PARTS = 300;                // maximum allowed sprite-sheet parts for tiles image        
        Sheet[]               sheet;                                // sprite sheet data for tiles (could use a list)
        SheetManager          sheet_mgr;                            // where a level's sheet definitions can be edited  
        Map                   map;                                  // holds all the tiles map stuff
        Editor                editor;                               // map editor                     

        // PLAYER / MEOMOTION / CAMERA        
        static public Vector2 rescale = Vector2.One; // adjusts character scale to screen resolution (static so if altered, all creatures can be scaled in all classes)
        static public Vector2 cam_pos;               // world position of camera
        Player                player;                // hero-player control and collision detection/response
        MeoMotion             meo;                   // meo = MeoMotion manager which loads and stores vector-based blended keyframe animations

        // MONSTERS / NPC's 
        MonsterSys            monsterSys;            // holds and controls monsters or npc's

        // FRAME ANIMATION STUFF / EXPLOSIONS        
        ExplodeSys            explodeSys;            // tracks explosion frames for all explosions within it 


        
        //------------------
        // C O N S T R U C T
        //------------------
        public Game1()
        {
            // Set a display mode that is windowed but is the same as the desktop's current resolution (don't show a border)... 
            // This is done instead of using true fullscreen mode since some firewalls will crash the computer in true fullscreen mode
            int initial_screen_width = SCREENWIDTH;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 10; //(-10 if dubugging) 
            int initial_screen_height = SCREENHEIGHT;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height- 10; //(-10 if debugging) [makes taskbar visible]
            graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth  = initial_screen_width,
                PreferredBackBufferHeight = initial_screen_height,
                IsFullScreen = FULLSCREEN,  PreferredDepthStencilFormat = DepthFormat.Depth16
            };
            //Window.IsBorderless = true;
            Content.RootDirectory = "Content";
        }


        //--------
        // I N I T
        //--------
        protected override void Initialize()
        {
            // SETUP SPRITEBATCH AND GET TRUE DISPLAY
            spriteBatch   = new SpriteBatch(GraphicsDevice);
            MainTarget    = new RenderTarget2D(GraphicsDevice, SCREENWIDTH, SCREENHEIGHT);
            pp            = GraphicsDevice.PresentationParameters;
            SurfaceFormat format = pp.BackBufferFormat;
            screenW       = MainTarget.Width;
            screenH       = MainTarget.Height;
            desktopRect   = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            screenRect    = new Rectangle(0, 0, screenW, screenH);
            screen_center = new Vector2(screenW / 2.0f, screenH / 2.0f) - new Vector2(32f, 32f);               // subtracting half of the size of a tile since this is for tile calculations
            quadBatch = new QuadBatch(Content, GraphicsDevice, "QuadEffect.fx", "FontTexture", screenW, screenH); // setup distortable quad class (like spriteBatch)
            quadBatch.PIXEL = new Rectangle(897, 1, 2, 2);
            
            // INIT RENDER TARGETS
            sprite_target = new RenderTarget2D(GraphicsDevice, screenW, screenH);

            rnd = new Random(); // INIT UTILS
            inp = new Input();  // INIT INPUT            

            // INIT MAP
            sheet = new Sheet[MAX_SHEET_PARTS];
            sheet_mgr = new SheetManager();
            map = new Map(sheet, spriteBatch);

            // INIT MEOMOTION
            meo = new MeoMotion(Content, quadBatch); 

            // INIT EXPLOSIONS
            explodeSys = new ExplodeSys(spriteBatch);

            base.Initialize();
        }


        //--------
        // L O A D
        //--------
        protected override void LoadContent()
        {
            // LOAD GRAPHICS: 
            far_background = Content.Load<Texture2D>("background_stars");
            mid_background = Content.Load<Texture2D>("mid_background");
            tiles_image = Content.Load<Texture2D>("tiles1");
            font = Content.Load<SpriteFont>("TextFont");//("Font"); // .spritefont not supported; .xnb used.
            // LOAD HUD
            hud = new Hud(spriteBatch, font);
            hud.Load(Content);

            // LOAD EXPLOSIONS
            explodeSys.Load(Content, 16); 

            // CREATE PLAYER
            player = new Player(screen_center, quadBatch, map, inp, hud, explodeSys); // add explosions, etc.. 

            // CREATE MONSTER SYSTEM
            monsterSys = new MonsterSys(Content, quadBatch, map, player, explodeSys);
            monsterSys.Load(1, Content); 

            // INIT EDITOR
            editor = new Editor(map, inp, player, sheet, monsterSys);    // do this here because editor needs player and player needs hud and hud needs font (and font cannot be null)

            // SET IMAGE FOR TILES
            map.SetTilesImage(tiles_image);                              // this might be used in multiple places so send a reference to map but store tiles here
            
            // FX
            crystal_fx = Content.Load<Effect>("Crystal.fx");

            if (crystal_fx != null)
            {
                crystal_fx.Parameters["screen"].SetValue(new Vector2(screenW, screenH)); // screen(target) resolution 
                crystal_fx.Parameters["BackMap"].SetValue(far_background);              // image to make shimmer from                       
            }

            // later we will want update to check if loading a new area is needed... and then have it call a method to load based on the level or area (game-state in update)
            sheet_mgr.Setup_Sheet_Level_1(ref sheet);

            map.AddBorder(6);

            // LOAD AND SETUP PLAYER ANIMATIONS: 
            float resize = 0.60f; rescale = new Vector2(resize, resize);           // 60% size - scale characters overall
            meo.Adjust_Scale_For_ScreenResolution(ref rescale, screenW, screenH);  // adjust final scale for target resolution
            player.Load(meo, tiles_image);               

            editor.LoadLevel(LEVEL_NAME); 

            // LOAD SOUND AND MUSIC
            Sound.Load(Content);

            //RnD
            //MediaPlayer.Play(Sound.music);
        }//LoadContent


        protected override void UnloadContent() 
        {  
        }


        //------------
        // U P D A T E
        //------------   
        protected override void Update(GameTime gameTime)
        {
            inp.Update();
            if (inp.Keypress(Keys.Escape)) Exit();     // <-- CHANGE LATER - (to go to menu state - then provide exit option in menu)

            map.UpdateVars();
            explodeSys.Update();

            if (inp.Keydown(Keys.Left)) background_pos.X++;
            if (inp.Keydown(Keys.Right)) background_pos.X--;
            if (inp.Keydown(Keys.Up)) background_pos.Y++;
            if (inp.Keydown(Keys.Down)) background_pos.Y--;

            switch (gameState)
            {
                case GameState.play: 
                    // P L A Y  M O D E

                    // CHECK FOR GAMESTATE CHANGES
                    if (inp.Keypress(Keys.E)) { gameState = GameState.edit; }
                    if (hud.lives < 0)          gameState = GameState.game_over;

                    // PLAYER INPUT AND ANIMATION UPDATES:
                    player.Update(); 

                    // MOVE CAMERA
                    cam_pos += (player.pos - cam_pos) * 0.1f;               // smooth pan camera toward player's world position (ease-in) 

                    // MATCH WORLD VIEW TO CAMERA
                    map.world_to_camera(cam_pos, ref background_pos);       // (what you see in the world based on where camera is)

                    // PLAYER COLLISION DETECTION / RESPONSE: 
                    player.WorldCollisions(); 

                    // setting all animations
                    // UPDATE BOUNCERS 
                    map.bounceMgr.Update();

                    // INIT ANY NEW ANIMATION:
                    player.SetAnimation(gameTime); 

                    // MONSTER AI AND ANIMATIONS:
                    monsterSys.Update(gameTime); 

                    break;
                case GameState.edit:
                    //----------------------
                    // E D I T O R   M O D E  (input/updates)
                    cam_pos = map.loc.ToVector2() * 64;
                    monsterSys.Update(gameTime, false); 
                    editor.Update();

                    if (inp.Keypress(Keys.Enter)) gameState = GameState.play;
                    break;
                case GameState.game_over:
                    // GAME OVER
                    if (inp.Keypress(Keys.Escape) || inp.Keypress(Keys.Enter)) Exit(); // change to go to menu state later
                    break;
            }

            base.Update(gameTime);
        }


        //--------
        // D R A W
        //--------
        protected override void Draw(GameTime gameTime)
        {
            // DRAW SPRITES TO A SPRITE TARGET (so we can use it in crystal fx):
            if (gameState == GameState.play)
            {
                GraphicsDevice.SetRenderTarget(sprite_target);
                GraphicsDevice.Clear(Color.TransparentBlack);
                
                // DRAW MONSTERS
                monsterSys.Draw();
                
                // DRAW PLAYERS
                player.Draw();
            }
            //-------------------
            
            GraphicsDevice.SetRenderTarget(MainTarget);            

            if ((gameState != GameState.edit) && (gameState != GameState.play)) goto DRAW_OTHER_STATES;
            
            // DRAW OPAQUE FAR BACKGROUND
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap);
            spriteBatch.Draw(far_background, screenRect, new Rectangle((int)(-background_pos.X * 0.5f), 0, far_background.Width, far_background.Height), Color.White);
            spriteBatch.End();
            // DRAW MID BACKGROUND(S) 
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearWrap);
            spriteBatch.Draw(mid_background, screenRect, new Rectangle((int)(-background_pos.X), (int)(-background_pos.Y), far_background.Width, far_background.Height), Color.White);
            spriteBatch.End();

            //-----------------
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            map.DrawTiles();    // D R A W  T I L E S
            spriteBatch.End();            
            //-----------------

            // DRAW CRYSTALS 
            if (crystal_fx != null)
              crystal_fx.Parameters["ReflectMap"].SetValue(sprite_target);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                SamplerState.LinearClamp, DepthStencilState.None, 
                RasterizerState.CullNone, crystal_fx);
            map.DrawCrystals();
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

            // SHOW SPRITES
            spriteBatch.Draw(sprite_target, Vector2.Zero, Color.White);
            
            // DRAW OVERLAPS
            map.DrawOverlaps();
            spriteBatch.End();

            // DRAW ADDITIVE EFFECTS
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp);
            explodeSys.Draw(); 
            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            
            // E D I T  M O D E  only - - - - - - - - - - 
            if (gameState == GameState.edit)
            {
                editor.DrawLocators(spriteBatch, tiles_image, screen_center); 
            } // - - - - - - - - - - - - - - - - - - - - 
            #if SHOW_PLAYER_COLLIDER
            if ((player.loc.X > map.a1) && (player.loc.X < map.a2) && (player.loc.Y > map.b1) && (player.loc.Y < map.b2)) {       // if in drawn tiles
                Vector2 CDpos = new Vector2(map.sx + 64.0f * (player.loc.X - map.a1), map.sy + 64.0f * (player.loc.Y - map.b1));
                CDpos -= map.scroll_offset;
                spriteBatch.Draw(tiles_image, CDpos, new Rectangle(960, 0, 63, 63), Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                monsterSys.DrawColliders(spriteBatch, tiles_image);
            }
            #endif

            // TEXT / HUD
            if (gameState == GameState.play) 
            {
                spriteBatch.DrawString(font, "Press E to go to map editor", 
                    new Vector2(1, screenH-50), Color.DarkGreen); 
                spriteBatch.DrawString(font, "Press E to go to map editor", 
                    new Vector2(1, screenH - 49), Color.GreenYellow);
                hud.Draw();
            }
            else
            {   // EDITOR INSTRUCTIONS
                editor.DrawInstructions(spriteBatch, far_background, font, screenH);
            }
            spriteBatch.End();

            DRAW_OTHER_STATES:                                        // (label for goto to avoid superfluous programming [use sparingly and cautiously] )
            //----------------
            switch (gameState)
            {
                case GameState.game_over:                             // maybe add fade transitions later
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
                    GraphicsDevice.Clear(Color.Black); 
                    spriteBatch.DrawString(font, "G A M E  O V E R",  screen_center + new Vector2(-80 -20), Color.LimeGreen);
                    spriteBatch.DrawString(font, "  (press enter)  ", screen_center + new Vector2(-62, 20), Color.LimeGreen);
                    spriteBatch.End();
                    break;
            }

            // DRAW MAINTARGET TO BACKBUFFER
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            spriteBatch.Draw(MainTarget, desktopRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
