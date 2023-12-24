using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    class Player
    {
        enum motion     { none, walk, jump, idle1, idle2, duck, spell, ouch, }  // HERO PLAYER STATES
        enum duck_state { down, up }
        enum jump_state { start, air, end }
        enum Facing     { left = -1, right = 1 }

        // STATES 
        Hud        hud;                                                         // reference to Game1.hud for display of player status
        Facing     direction   = Facing.right;                                  // which direction is the character facing <-- --> 
        motion     hero_motion = motion.idle1, last_hero_motion = motion.none;
        duck_state duck_mode   = duck_state.down;
        jump_state jump_mode   = jump_state.start;

        // CONSTANTS 
        const int   half_player_width = 32,  player_feet_offset = 26,  player_head_offset = 71;
        const float GRAVITY      = 0.25f,    MAX_FALL_SPEED = 20;               // using same in monster class
        const float MAX_VELOCITY = 7.3f;                                        // max walk/run speed

        // DISPLAY
        QuadBatch quadBatch;

        // MAP
        Map       mp;                         // allow access to map instance
        Tile[,]   tiles;                      // reference to tiles[,] from Game1
        BounceMgr bounceMgr;                  // refer to Map's bounceMgr (for spring platforms)

        // COORDINATES AND CONTROL STATUS: 
        Input          inp;                   // reference to the input object in Game1
        Vector2        rescale;               // refers to Game1's character rescale         
        public Vector4 bbox;                  // bounding box used in other modules to determine special collisions (like monsters)
        public Vector2 pos;                   // player's position in world coordinates
        public Vector2 screen_pos;            // player's position as it appears on the screen
        public Point   loc;                   // player tile-map location           
        Vector2        old_pos;               // previous world position
        Point          old_loc;               // previous map location   (in tiles)
        Vector2        vel;                   // player's velocity        
        float          MAX_JUMP = -14f;       // maximum jump-up speed
        bool           grounded;              // player on ground or not    
        public int     invincible = 0;        // if > 0 invincible = true (counts down to 0)      
        public int     spell_power = 1;       // attack power of spell (damage amount)

        // FOR MEOMOTION ANIMATION
        MeoMotion meo;                        // reference to the MeoMotion class made in Game1
        MeoPlayer meo_hero;                   // controls player's animations
        bool      flip = false;               // flip character horizontally ? 
        bool      must_finish_animation;      // if you want an animation to finish completely before allowing switch to the new one
        int       idle_timer;                 // determines when to change idle1 to idle2
        int       walk_index, ouch_index;     // index of walk and ouch animation (for convenience)

        // SPELLS/FX 
        public List<ParticleSystem> spells;   // magic spells (ie: fire balls)
        Texture2D  tex;                       // Ref to Texture holding spell images (I used tiles_image in Game1)
        ExplodeSys explode;                   // Ref to explodeSys in Game1 (explosion animations)              


        // C O N S T R U C T
         public Player(Vector2 world_pos, QuadBatch Qbatch, Map map, Input Inp, Hud hd, ExplodeSys ex) 
        {            
            pos       = world_pos;                // set some starting position in the world
            quadBatch = Qbatch;                   // refer to Game1 instance of quadBatch
            mp        = map;                      // refer to instance of map
            tiles     = map.tiles;                // refer to map's tiles 
            bounceMgr = map.bounceMgr;            // refer to map's bounceMgr  
            inp       = Inp;                      // refer to Game1's Input instance
            hud       = hd;                       // refer to Game1's hud (player status)
            explode   = ex;                       // refer to Game1's explodeSys (explode animations)
        }


        //--------
        // L O A D 
        //--------
        public void Load(MeoMotion Meo, Texture2D Tex) 
        {
            meo = Meo;                                              // reference the Game1 instance of the MeoMotion class
            tex = Tex;                                              // reference the Game1 tiles_image where spell images are also stored

            // P R E P A R E  C H A R A C T E R             
            rescale = Game1.rescale;                                // copy from Game1 - for character size and adjusted velocity
            meo.Load_TXT("Wizzy", rescale);                         // load characters/animations (.TXT must be with exe file [ie: BIN - Windows - debug/release] )
            meo_hero = new MeoPlayer("Wizzy", pos, meo, quadBatch); // sets the character to use from the sheet    

            // CUSTOMIZE CHARACTER PROPERTIES
            int index;
            index = meo_hero.GetIndex("duck");  //meo.anim[index].speed = 5.0f;    // speed up ducking animation
            index = meo_hero.GetIndex("jump");  //meo.anim[index].speed = 0.7f;    // slow down initial jumping animation
            index = meo_hero.GetIndex("spell"); //meo.anim[index].speed = 2.5f;    // speed up spell/attack animation
            ouch_index = meo_hero.GetIndex("ouch"); 
            //meo.anim[ouch_index].looping = false;                 // ensure it doesn't loop (if forgot to set in MeoMotion)            
            //meo.anim[ouch_index].num_keys--;                      // shorten the animation [don't need last key for this one]
            walk_index = meo_hero.GetIndex("walk");
            //meo.anim[walk_index].offset.Y =  -6f;                 // move it a bit so animation lines up better with ground
            //meo.anim[walk_index].speed    = 0.2f;                 // speed of walk changes so remember the walk index          

            meo_hero.SetAnimation("idle1", flip);                 // set a starting animation      

            // PREPARE SPELLS
            spells      = new List<ParticleSystem>(); 
            spell_power = 1;
        }


        //------------------
        // H E R O   J U M P 
        //------------------
        //float jump_boost = 0f; // this was for a video tutorial example of jump boost
        void HeroJump(bool CheckInput = false, bool play_bounce = false, bool keep_motion = false)
        {            
            if ((hero_motion == motion.jump)&&(jump_mode==jump_state.start)) return;   // don't retrigger jump that's already starting 
            if (CheckInput) {
                if (!inp.Keypress(Keys.Up) && !inp.Keypress(Keys.Space))    return;    //{ jump_boost = 0; return; }    // if jump pressed continue
                if (!grounded && !(meo_hero.animation_index == ouch_index)) return;    //{ jump_boost = 0; return; }    // if on ground (or spikes) continue
            }
            if (!keep_motion) {
                if (hero_motion == last_hero_motion) last_hero_motion = motion.idle1;      // cause SetAnimation to reset jump animation
                hero_motion = motion.jump;
            }
            jump_mode   = jump_state.start;
            grounded    = false;
            // play jump sound
            if (!play_bounce) Sound.jump.Play(); else Sound.bounce.Play();
        }



        //----------------------
        // H E R O   A T T A C K 
        //----------------------
        void HeroAttack()
        {
            if (hero_motion == motion.spell) return;
            if (must_finish_animation) return;        // if hurt, animation must finish first
            hero_motion = motion.spell;
            Sound.attack.Play();
            // currently only 1 spell - adjust code for more spells later:
            Vector2 spell_vel = new Vector2(-6, 0);
            if (vel.X == 0) {    if (direction==Facing.right) spell_vel.X = 6;     }      
            else {               if (vel.X > 0)               spell_vel.X = 6;     }            
            // place a new spell (add vel*2 so that the generating spell doesn't lag behind the player as much) 
            // also adding to velocity of spell the horizontal motion of hero's vel/2 ... this gives part of forward momentum of player to the spell (fire ball)
            ParticleSystem spell = new ParticleSystem(pos + vel * 2, spell_vel + new Vector2(vel.X / 2, 0), 90, ParticleType.fire, tex);
            spell.emit_vel_dampen = 0.99f;           // cause it to slow down a tiny bit as it moves
            spells.Add(spell);         
        }



        // H E R O  D A M A G E D  
        public void HeroDamaged(float life_loss)
        {
            if (invincible > 0) return;
            meo_hero.SetAnimation(ouch_index, flip); must_finish_animation = true;
            if (hero_motion == motion.duck) life_loss /= 2f; // reduced damage if ducking 
            hud.life -= life_loss;
            invincible = 100;
            Sound.ouch.Play();
            if (hud.life <= 0.0f)
            {
                explode.Add_Explosion(pos); 
                Sound.explode.Play(0.7f, 0.7f, 0f);
                hud.life = 1.0f; hud.lives--;
                loc.X = mp.startData.x;                     // reset player position on map
                loc.Y = mp.startData.y;
                pos = Conv.tile_to_world(loc) + new Vector2(-32, -32);
            }
        }



        //------------
        // U P D A T E (player)
        //------------                      
        // UPDATE (Handles input and animation updates)           
        public void Update()
        {
            old_pos = pos;                // previous world position
            old_loc = loc;                // previous map location   (in tiles)

            meo_hero.play_speed = 1f;     // set default speed (if in water or speed boost then it will change) 

            MeoAnimation walk_anim = meo.anim[walk_index]; // reference the walk for easier access

            // HERO STATES --- PLAYER INPUT: 
            switch (hero_motion)
            {
                case motion.idle1: // I D L E ---------------- 
                case motion.idle2:
                    // use timer to select idle mode
                    if ((last_hero_motion != motion.idle1) && (last_hero_motion != motion.idle2)) idle_timer = 0; // not in idle so reset the timer
                    if (idle_timer <= 155) hero_motion = motion.idle1;                                            // idle1 for some time
                    else if (idle_timer > 155)                                                                    // idle2 with blink (until it's done) 
                    {
                        if ((hero_motion == motion.idle1) && (meo_hero.IsDoneAnimation()))   hero_motion = motion.idle2;
                        if ((hero_motion == motion.idle2) && (meo_hero.IsDoneAnimation())) { hero_motion = motion.idle1; idle_timer = 0; }
                    }
                    idle_timer++;

                    if (inp.Keydown(Keys.Right)) { hero_motion = motion.walk; flip = false; vel.X += 0.80f; }
                    if (inp.Keydown(Keys.Left))  { hero_motion = motion.walk; flip = true;  vel.X -= 0.80f; }
                    if (inp.Keydown(Keys.Down)) { hero_motion = motion.duck; duck_mode = duck_state.down; }
                    else HeroJump(CheckInput:true);                     
                    if ((inp.control_press) || (inp.alt_press)) HeroAttack(); 
                    break;
                case motion.duck: // D U C K ----------------- (note: duck logic can be complex)
                    switch (duck_mode)
                    {
                        case duck_state.down: // d u c k i n g  d o w n 
                            meo_hero.reverse = false;                       // initially ensure forward animation
                            if (!inp.Keydown(Keys.Down))
                            {                                               // if down released - start standing up: 
                                duck_mode = duck_state.up;
                                if (!meo_hero.reverse) if (meo_hero.IsDoneAnimation()) meo_hero.StartReverse(); // setup restart of animation for reverse if done
                                meo_hero.reverse = true;                    // set this last (for partial duck) 
                            }                            
                            break;
                        case duck_state.up: // s t a n d i n g  u p
                            meo_hero.reverse = true;                        // initially ensure reverse animation
                            if (inp.Keydown(Keys.Down)) duck_mode = duck_state.down; // start ducking down again                            
                            if (meo_hero.IsDoneAnimation()) { meo_hero.reverse = false; hero_motion = motion.idle1; } // done standing up so go to idle mode initially                            
                            break;
                    }
                    //jump_boost = -2f; 
                    HeroJump(CheckInput: true); 
                    if ((inp.control_press) || (inp.alt_press)) HeroAttack();
                    break;
                case motion.walk: // W A L K ---------------------
                    hero_motion = motion.idle1; idle_timer = 0;        //default to idle if no direction is pressed                           
                    if (inp.Keydown(Keys.Right)) // r i g h t
                    {
                        hero_motion = motion.walk; flip = false;
                        if (vel.X < MAX_VELOCITY) vel.X += 0.80f;      // accelerate right
                    }
                    if (inp.Keydown(Keys.Left))  // l e f t
                    {
                        hero_motion = motion.walk; flip = true;
                        if (vel.X > -MAX_VELOCITY) vel.X -= 0.80f;     // accelerate left
                    }
                    if (inp.Keydown(Keys.Down)) { hero_motion = motion.duck; duck_mode = duck_state.down; } // start duck
                    else HeroJump(CheckInput: true);
                    if ((inp.control_press) || (inp.alt_press)) HeroAttack(); 
                    break;
                case motion.jump: // J U M P -------------------
                    if (last_hero_motion == motion.jump)              // make sure the animation's started already
                    {              
                        if ((jump_mode != jump_state.start) && (last_grounded) && (vel.X != 0))            // if in the air and becomes grounded... prevent sliding
                            hero_motion = motion.walk; else 
                        hero_motion = motion.jump;                        
                        if (meo_hero.key1<2) meo_hero.play_speed=4f; else meo_hero.play_speed = 0.5f;      // customize play speed to match jumping action                        
                        if ((meo_hero.key1 >= 1) && (jump_mode == jump_state.start)) {                     // leaving the ground                        
                            jump_mode  = jump_state.air; 
                            vel.Y = MAX_JUMP; // + jump_boost; jump_boost = 0f; 
                        } 
                        
                        if (inp.Keydown(Keys.Right)) { flip = false; if (vel.X <  MAX_VELOCITY) vel.X += 0.80f; } // accelerate right
                        if (inp.Keydown(Keys.Left))  { flip = true;  if (vel.X > -MAX_VELOCITY) vel.X -= 0.80f; } // accelerate left
                        HeroJump(CheckInput: true);
                        if (meo_hero.IsDoneAnimation()) hero_motion = motion.idle1;
                        if ((inp.control_press) || (inp.alt_press)) HeroAttack();
                    }
                    break;
                case motion.spell: // S P E L L -------------------
                    if (inp.Keydown(Keys.Right)) { flip = false; if (vel.X <  MAX_VELOCITY) vel.X += 0.50f; } // accelerate right
                    if (inp.Keydown(Keys.Left))  { flip = true;  if (vel.X > -MAX_VELOCITY) vel.X -= 0.50f; } // accelerate left
                    HeroJump(CheckInput: true, keep_motion: true);
                    if (meo_hero.IsDoneAnimation()) hero_motion = motion.idle1;
                    break;
            }
            // GRAVITY
            if (vel.Y < MAX_FALL_SPEED) vel.Y += GRAVITY;
            // SLOW DOWN PLAYER (and set animation play speed)
            if (vel.X > 0) { vel.X -= 0.40f; if (vel.X < 0) vel.X = 0; direction = Facing.right; walk_anim.speed = vel.X *  0.42f; }
            if (vel.X < 0) { vel.X += 0.40f; if (vel.X > 0) vel.X = 0; direction = Facing.left;  walk_anim.speed = vel.X * -0.42f; } // animation speeds can only be positive (- x - = +)
            // DETERMINE FLIP
            if (direction == Facing.right) meo_hero.flip = false; else meo_hero.flip = true;
            // MOVE PLAYER
            Vector2 tru_vel = vel * rescale; // factors character resize into velocity
            pos += tru_vel;

            // UPDATE SPELLS
            int i = 0;
            while (i < spells.Count)
            {
                spells[i].Update(true);
                if (spells[i].dead) spells.RemoveAt(i); // remove any dead emitters (ie: fizzled out fire balls)
                i++;
            }

            if (invincible > 0) invincible--; 

        } // ^^ Update ^^



        //-------------------------------------------
        #region C O L L I S I O N S (player vs world)
        //-------------------------------------------
        
        //---------------------
        // T E S T  D O W N  (helper for World Collisions)
        //---------------------
        void Test_Down(int x, int y, int h_offset = 0) {
            const float BOUNCE_VEL = -21f;                 // <-- how fast you'll jump up if you hit a spring platform
            float offy = 0;                                // <--possible offset for collision region (ie: if spikes it is farther down)   
            if (tiles[x, y].stand_on)                      // WILL HIT SOLID (below - can stand-on)
            {
                if (tiles[x, y].spikes) offy = 28; else offy = 0;
                if ((pos.Y + player_feet_offset + 10) > y * 64 + offy)
                {                    
                    if ((pos.Y + player_feet_offset) > y * 64 + offy)
                    {
                        if (h_offset > 0) {                              // allow player to stand at the very edge (hard coded as 84 (64+20) - change for other character sizes) 
                            if (pos.X < x * 64 + h_offset) h_offset = 0;
                        }
                        else if (h_offset < 0) {                         // allow player to stand at the very edge 
                            if (pos.X > x * 64 + h_offset) h_offset = 0;  
                        }
                        if (h_offset == 0) {
                            grounded = true;                             // stop falling and set grounded  
                            pos.Y = y * 64 - player_feet_offset + offy;  // set position so on top of tile 
                            vel.Y = 0;
                            if (offy == 28) on_spikes = true;
                            if (tiles[x, y].type == TileType.spring) { vel.Y = BOUNCE_VEL; bounceMgr.Add(x, y); HeroJump(play_bounce: true); } // spring platform
                        }
                    }
                }
            }
        }

        //-------------
        // T E S T  U P 
        //-------------
        void Test_Up(int x, int y)
        {
            if (y < 0) y = 0;
            if (tiles[x, y].is_solid)                   // WILL HIT SOLID (up)
            {
                if ((pos.Y - player_head_offset) < (y+1) * 64) { vel.Y = 0; pos.Y = old_pos.Y; }
            }            
        }

        //-------------------
        // T E S T  R I G H T
        //-------------------
        void Test_Right(int x, int y) 
        {
            // CAUTION (possible out of bounds test) 
            if (x < 0) x = 0;
            if (x >= Map.TILES_WIDE - 1) x = Map.TILES_WIDE - 1;
            if (tiles[x, y].is_solid)                        // IN SOLID (to right)
            {
                if ((pos.X + half_player_width) > x * 64)
                {
                    if ((!on_spikes) || (!tiles[x, y].spikes)) {
                        pos.X = x * 64 - half_player_width; vel.X = 0f;
                        loc.X = (int)pos.X / 64;
                    }
                }
            }
        }

        //-------------------
        // T E S T  L E F T
        //-------------------
        void Test_Left(int x, int y)
        {
            // CAUTION (possible out of bounds test)                
            if (x < 0) x = 0;
            if (x >= Map.TILES_WIDE - 1) x = Map.TILES_WIDE - 1;
            if (tiles[x, y].is_solid)                   // WILL HIT SOLID (to left)
            {
                if ((pos.X - half_player_width) < (x+1) * 64)
                {
                    if ((!on_spikes) || (!tiles[x, y].spikes)) {
                        pos.X = x * 64 + 64 + half_player_width; vel.X = 0f; 
                        loc.X = (int)pos.X / 64;
                    }
                }
            }
        }

        //-------------------------------
        // W O R L D  C O L L I S I O N S (player vs world)
        //-------------------------------
        bool on_spikes;
        bool last_grounded;
        public void WorldCollisions()
        {
            // GET PLAYER'S SCREEN POSITION
            screen_pos = Conv.world_to_screen(pos);

            // GET CURRENT BBOX COORDS (used in Monster.cs to determine attack collisions)
            bbox = new Vector4(pos.X - 28, pos.Y - 62, pos.X + 28, pos.Y + 16); 

            // get player tile occupation:
            loc.X = (int)pos.X / 64;
            loc.Y = (int)pos.Y / 64;
            int x = loc.X, y = loc.Y;

            // PREVENT ILLEGAL MEMORY ACCESS:
            if ((x + 1 >= Map.TILES_WIDE) || (x - 1 < 0) || (y + 1 >= Map.TILES_HIGH) || (y - 1 < 0)) { pos = old_pos; loc = old_loc; x = loc.X; y = loc.Y; }

            // GET MOVEMENT VECTOR:
            Vector2 move = pos - old_pos;
            // (! Assume velocity never exceeds tile size of 64 which is extremely fast [otherwise you need a sweep test for closest tile interactions] !)     

            //float BOUNCE_VEL = MAX_JUMP - 8f;                   // <-- how fast you'll jump up if you hit a spring platform            
            // MOVING RIGHT 
            if (move.X > 0)
            {
                Test_Right(x + 2, y);
                Test_Right(x + 1, y);
                Test_Right(x + 1, y - 1);                
            }
            // MOVING LEFT
            if (move.X < 0)
            {
                Test_Left(x - 2, y);
                Test_Left(x - 1, y);
                Test_Left(x - 1, y - 1);                
            }
            // MOVING UP 
            if (move.Y < 0)
            {
                Test_Up(x, y - 2);
                Test_Up(x, y - 1);                
                if (tiles[x - 1, y - 1].is_solid)               // WILL HIT SOLID (up-left)
                {
                    if ((pos.Y - player_head_offset) < y * 64) {
                        if (pos.X < (x - 1) * 64 + 84)    {   vel.Y = 0;   pos.Y = old_pos.Y;   }                        
                    }
                }
                if (tiles[x + 1, y - 1].is_solid)               // WILL HIT SOLID (up right)
                {
                    if ((pos.Y - player_head_offset) < y * 64) {
                        if (pos.X > (x + 1) * 64 - 20)    {   vel.Y = 0;   pos.Y = old_pos.Y;   }
                    }
                }
            }            
            // MOVING DOWN 
            on_spikes = false;
            grounded  = false;                                     // reset grounded as false until we know:     
            if (move.Y > 0)
            {                
                Test_Down(x, y);
                Test_Down(x, y + 1);
                Test_Down(x - 1, y, 84);
                Test_Down(x - 1, y + 1, 84);
                Test_Down(x + 1, y + 1, -20);
                Test_Down(x + 1, y, -20);                

                // HIT SPIKES
                if (on_spikes)
                {
                    if (invincible <= 0)
                    {
                        vel.Y = -6f; 
                        HeroDamaged(0.2f);
                    }
                }

                if ((on_spikes == false) && (grounded) && (!last_grounded)) Sound.land.Play();
            }//^^^ move.Y>0 ^^^            
            last_grounded = grounded; 
        } // WorldCollisions
        #endregion



        //-------------------------
        // S E T  A N I M A T I O N  ( player )
        //-------------------------
        public void SetAnimation(GameTime gameTime)
        {
            bool allow_switch = true;
            if (must_finish_animation)
                if (!meo_hero.IsDoneAnimation()) allow_switch = false; else { must_finish_animation = false; hero_motion = motion.idle1; }
            if ((allow_switch) && (last_hero_motion != hero_motion))
            {
                switch (hero_motion)
                {
                    case motion.idle1: meo_hero.SetAnimation("idle1", flip); break;
                    case motion.idle2: meo_hero.SetAnimation("idle2", flip); break;
                    case motion.walk:  meo_hero.SetAnimation("walk",  flip); break;
                    case motion.duck:  meo_hero.SetAnimation("duck",  flip); break;
                    case motion.jump:  meo_hero.SetAnimation("jump",  flip); break;
                    case motion.ouch:  meo_hero.SetAnimation("ouch",  flip); break;
                    case motion.spell: meo_hero.SetAnimation("spell", flip); break; 
                }
                last_hero_motion = hero_motion; 
            }

            meo_hero.position = screen_pos;
            meo_hero.Update(gameTime); 
        }


        //--------
        // D R A W         
        // int cntr = 0; // was using in a jump boost example
        public void Draw()
        {
            BlendState blend = BlendState.AlphaBlend; // if (hero_motion == motion.duck) { cntr++; if (cntr%4<2) blend = BlendState.Additive; } // (example of charging up a jump from duck and making it flash)
            quadBatch.Begin(meo.tex, blend);
            Color hero_color = Color.White;
            if (invincible>0)
            {
                int n = invincible % 10;
                if (n > 5) hero_color = new Color(255, 155, 155, 255); // flashing if invincible 
            }         
            meo_hero.Draw(hero_color);

            // SHOW SPELLS: 
            quadBatch.End(); quadBatch.Begin(tex, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None);
            int i = 0;
            while (i < spells.Count) {
                spells[i].Draw(Vector2.Zero, quadBatch); 
                i++;
            }
            quadBatch.End();            
        }

    }
}
