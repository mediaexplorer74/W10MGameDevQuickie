using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    enum MonsterType { None, Mouster, Hellcat }
    
    class Monster
    {
        const float GRAVITY  = 0.25f, MAX_FALL_SPEED = 20;       // using same in player class
        const float MAX_JUMP = -12;

        public enum Act      { idle, walk, run, attack, jump, ouch }         // later add things like jump, duck, guard, or whatever
        public enum Mode     { wander, run_away, patrol }                    // what mode of behavior the monster AI is using 

        int screenW, screenH; 

        // MEO / ANIMATION
        MeoMotion         meo;            // reference to MonsterSys's meoMotion player        
        MeoPlayer         meo_play;       // for controlling enemy character animations (unique instance)
        bool              flip;           // flip horizontally                
        Vector2           rescale;        // adjusts meo player image scale for resolution (and movement speed)
        Act               motion, last_motion; // if motion changes, it will play the corresponding animation
        Mode              mode;           // wander, chase, escape, patrol 
        int               walk_index;     // remember index of walk/run to adjust animation speed        
        int               jump_index;     // remember index of "jump" if it exists
        int               ouch_index;
        int               time, total_time;           // current time doing an action, total time to continue that action
        bool              must_finish_animation;      // if you want an animation to finish completely before allowing switch to the new one      

        ExplodeSys        explode;        // <-- frame based animations (referenced from Game1) 

       // MAP / PLAYER
        Tile[,]           tiles;          // reference to tiles[,] from Game1
        BounceMgr         bounceMgr;      // refer to Game1's bounceMgr (for spring platforms)
        Player            player;         // refer to player 

        // MONSTER        
        MonsterType       type;           // which type of monster                        
        public bool       dead;           // if it's dead, skip processing it
        public int        life;           // life status 
        public Vector2    pos, vel;       // world position, velocity
        public Vector2    target_vel;     // velocity goal (either random or deliberate)
        public Vector2    old_pos;        // previous position of monster
        public Vector2    screen_pos;     // where monster would appear on screen  
        public Point      loc, old_loc;   // approximate map tile coordinate of monster
        public Rectangle  bbox;           // bounding box for collision detection
        public Rectangle  attack_box;     // for collision detection of attack from monster  
        bool              grounded;       // monster on ground or not 
        int               attack_key; 
        int               invincible;     

        // SOUND        
        Vector2           pan;            // stereo pan (left-right speaker volumes based on monster position)


        // CONSTRUCT
        public Monster(Vector2 Pos, MeoMotion Meo, QuadBatch QBatch, MonsterType monster_type, Map map, Player playr, ExplodeSys ex)
        {
            screenW = Game1.screenW;  screenH = Game1.screenH;  rescale = Game1.rescale; 
            meo        = Meo;                        
            tiles      = map.tiles;
            bounceMgr  = map.bounceMgr;
            player     = playr;
            explode    = ex;

            type       = monster_type;            
            string sheet_name = get_monster_properties(type);
            meo_play   = new MeoPlayer(sheet_name, Pos, Meo, QBatch);
            pos        = Pos;       old_pos = pos;
            screen_pos = Conv.world_to_screen(pos);
            loc        = Conv.GetTileCoord(pos);
            dead       = false;
            customize_monster();            
            mode       = Mode.wander;
        }


       // C H A N G E  M O N S T E R
        public void ChangeMonster(Vector2 Pos, MonsterType monster_type) {
            pos        = Pos;        old_pos = pos;
            screen_pos = Conv.world_to_screen(pos);
            loc        = Conv.GetTileCoord(pos);            
            type = monster_type;
            meo_play.sheet_name = get_monster_properties(type);             
            dead = false;
            customize_monster();            
        }


        //------------------------
        // M O N S T E R   J U M P 
        //------------------------
        // play_bounce [plays bounce sound], keep_motion [don't change animations]
        void MonsterJump(bool play_bounce = false, bool keep_motion = false)
        {
            if (!grounded) return;   // don't retrigger jump that's already starting             
            if ((!keep_motion) && (jump_index > 0)) motion = Act.jump;
            grounded = false;
            // play jump sound
            if (pan.Y > 0f) { if (!play_bounce) Sound.jump.Play(pan.Y, 0.9f, pan.X); else Sound.bounce.Play(pan.Y, 0.9f, pan.X); }
            if (must_finish_animation)
            {
                vel.Y = MAX_JUMP; // need to trigger from here instead of state because waiting for animation to finish                
            }
        }


        //-----------------------------
        // M O N S T E R  D A M A G E D 
        //-----------------------------
        public void MonsterDamaged(int life_loss)
        {
            if (invincible > 0) return;
            vel.Y = -6f;
            meo_play.SetAnimation(ouch_index, flip); must_finish_animation = true; motion = Act.ouch;
            life -= life_loss;            
            invincible = 100;

            // (sounds could be referenced with indexing[] too if you have lots of monster types)
            if (pan.Y > 0)
            {
                switch (type)
                {
                    case MonsterType.Mouster: MonsterSys.monster_ouch1.Play(pan.Y, 0.9f, pan.X); break;
                    case MonsterType.Hellcat: MonsterSys.monster_ouch2.Play(pan.Y, 0.9f, pan.X); break;
                }
            }

            if (life <= 0)
            {
                dead = true;
                // ADD EXPLOSION:
                explode.Add_Explosion(pos); 
                if (pan.Y>0) Sound.explode.Play(pan.Y, 0.9f, pan.X);                
            }
        }


        
        // GET RANDOM SPEED
        float GetRandomSpeed()
        {
            float x = Game1.rnd.Next(12) - 6;
            if (x > 0) if (x < 4)  x =  4;
            if (x < 0) if (x > -4) x = -4;
            return x;
        }


        
        // SOLID (collision helper)
        bool Solid(int x, int y)
        {
            if ((x < 0) || (x >= Map.TILES_WIDE) || (y < 0) || (y >= Map.TILES_HIGH)) return true;
            if (tiles[x, y].is_solid) return true;
            return false; 
        }
        // SPIKE (collision helper)
        bool Spike(int x, int y)
        {
            if ((x < 0) || (x >= Map.TILES_WIDE) || (y < 0) || (y >= Map.TILES_HIGH)) return true;
            if (tiles[x, y].spikes) return true;
            return false;
        }
        // CONTAINS EMPTY 
        bool ContainsEmpty(int x, int y1, int y2)
        {
            if (y2 < y1) { int tmp = y1; y1 = y2; y2 = tmp; } // swap 
            y1 = MathHelper.Clamp(y1, 0, Map.TILES_HIGH - 1); 
            y2 = MathHelper.Clamp(y2, 0, Map.TILES_HIGH - 1);
            x  = MathHelper.Clamp(x,  0, Map.TILES_WIDE - 1);
            int y = y1;
            do {
                if (!tiles[x, y].is_solid) return true;
                y++;
            } while (y < y2);
            return false; 
        }
        // CONTAINS SOLID (test a vertical strip of tiles to see if a solid tile exists to land on)
        int ContainsSolid(int x, int y1, int y2)
        {
            if (y2 < y1) { int tmp = y1; y1 = y2; y2 = tmp; } // swap 
            y1 = MathHelper.Clamp(y1, 0, Map.TILES_HIGH - 1);
            y2 = MathHelper.Clamp(y2, 0, Map.TILES_HIGH - 1);
            x  = MathHelper.Clamp(x,  0, Map.TILES_WIDE - 1);
            int y = y1;
            do {
                if (tiles[x, y].spikes) return 2;   // don't jump down onto spikes (monster's not that dumb)
                if (tiles[x, y].stand_on) return 1;
                if (tiles[x, y].is_solid) return 1;                
                y++;
            } while (y < y2);
            return 0;
        }




        // ----------------
        // U P D A T E  A I 
        // ----------------
        void UpdateAI()
        {
            int x = loc.X, y = loc.Y;

            #region BASIC MOTION CHANGES
            switch (motion) {
                case Act.idle:   // I D L E -------------------------  
                    if (last_motion != motion) {
                        time = 0; total_time = Game1.rnd.Next(50, 140); 
                    }
                    time++;
                    if (time > total_time) {
                        target_vel.X = GetRandomSpeed(); 
                    }
                    break;
                case Act.walk:   // W A L K / R U N ---------------------
                case Act.run:
                    if (last_motion != motion) {
                        time = 0; total_time = Game1.rnd.Next(80, 160);
                    }
                    if (mode == Mode.wander) {
                        time++;
                        if (time > total_time) {
                            time = 0; total_time = Game1.rnd.Next(90, 160);
                            target_vel.X = GetRandomSpeed();
                            if (total_time > 150) target_vel.X = 0f;              // force an idle in wander mode sometimes
                            if (total_time < 110) mode = Mode.patrol;             // switch between patrol and wander sometimes
                        }
                    }
                    else if (mode == Mode.patrol) {
                        time++;
                        if (time > total_time) {
                            time = 0; total_time = Game1.rnd.Next(90, 160);
                            if (total_time < 110) mode = Mode.wander;             // switch between patrol and wander sometimes
                        }
                    }
                    break;
                case Act.jump:   // J U M P -----------------------
                    if (motion != last_motion) {
                        vel.Y = (float)(-Game1.rnd.Next(6) - 6); 
                    }
                    if (mode == Mode.wander) {
                        time++;
                        if (time > total_time) {
                            time = 0; total_time = Game1.rnd.Next(80, 160);
                            target_vel.X = GetRandomSpeed();
                            if (total_time > 150) target_vel.X = 0f;              // force an idle in wander mode sometimes                            
                        }
                    }
                    break;
                case Act.attack: // A T T A C K -------------------
                    if (meo_play.IsDoneAnimation()) motion = Act.idle;
                    break;
                case Act.ouch:   // O U C H -----------------------
                    if (meo_play.IsDoneAnimation()) motion = Act.idle;
                    break;
            } // ^switch(motion)^    
            #endregion

            #region DECISIONS (on ground): 
            if (grounded)
            {
                switch (mode)
                {
                    // --- W A N D E R ---
                    case Mode.run_away:
                    case Mode.wander:                        
                        if (target_vel.X < 0)                           // ___ GOING LEFT ___
                        {
                            // SOLID WALL TO LEFT? (OR SPIKES) 
                            bool isSpike = Spike(x-1,y);       
                            if (Solid(x-1,y)&& !isSpike) {              // if obstacle (that's not spike)
                                if (ContainsEmpty(x-1, y-4, y-1)) {     // see if can jump over it (or onto it)
                                    vel.Y = -12; MonsterJump(); 
                                }
                                else target_vel.X = 4;                  // solid wall to left so go right
                            } 
                            else if (isSpike) target_vel.X = 4;         // change directions if spikes are directly beside monster (ie: x-1, y)

                            // SOMETHING DO JUMP DOWN TO? 
                            int isSolid = ContainsSolid(x-1, y+1, y+6); 
                            if (isSolid==0) target_vel.X = 4;           // nothing to jump down onto so change directions
                            else if (isSolid == 2) { target_vel.X = -8; vel.Y = -12; MonsterJump(); }   // try to jump over the spikes (x-1, y+(1 to 5))
                        } 
                        else if (target_vel.X > 0)                      // ___ GOING RIGHT ___
                        {
                            // SOLID WALL TO RIGHT? (OR SPIKES) 
                            bool isSpike = Spike(x + 1, y);       
                            if (Solid(x + 1, y)&& !isSpike) {              // if obstacle (that's not spike)
                                if (ContainsEmpty(x+1, y-4, y-1)) {     // see if can jump over it (or onto it)
                                    vel.Y = -12; MonsterJump(); 
                                }
                                else target_vel.X = -4;                  // solid wall to right so go left
                            } 
                            else if (isSpike) target_vel.X = -4;         // change directions if spikes are directly beside monster (ie: x+1, y)

                            // SOMETHING DO JUMP DOWN TO? 
                            int isSolid = ContainsSolid(x+1, y+1, y+6); 
                            if (isSolid==0) target_vel.X = -4;           // nothing to jump down onto so change directions
                            else if (isSolid == 2) { target_vel.X = 8; vel.Y = -12; MonsterJump(); }   // try to jump over the spikes (x+1, y+(1 to 5))
                        }
                        break;

                    // --- P A T R O L ---
                    case Mode.patrol:
                        if (target_vel.X < 0) {                                 // __ GOING LEFT __                  
                            if (Solid(x - 1, y) || !tiles[x - 1, y + 1].stand_on || Spike(x-1,y+1)) // if obstacle or drop-off or spikes
                            {
                                target_vel.X = Game1.rnd.Next(3) * + 4;        // go the other way (randomize speed a bit)
                            }                            
                        }
                        else if (target_vel.X > 0) {                           // __ GOING RIGHT __
                            if (Solid(x + 1, y) || !tiles[x + 1, y + 1].stand_on || Spike(x + 1, y + 1)) // if obstacle or drop-off or spikes
                            {
                                target_vel.X = -Game1.rnd.Next(3) * - 4;       // go the other way (randomize speed a bit)
                            }                            
                        }                   
                        break;
                }//^switch^
            }//^grounded^
            #endregion

            #region --- DETERMINE  C H A S E --- (ATTACK)
            if ((mode == Mode.wander) || (mode == Mode.patrol)) {
                if ((pos.X > player.pos.X - 220) && (pos.X < player.pos.X + 220))
                {
                    if ((pos.Y > player.pos.Y - 150) && (pos.Y < player.pos.Y + 150)) // within range so give chase
                    {                        
                        if (pos.X < player.pos.X) target_vel.X =  6;
                        if (pos.X > player.pos.X) target_vel.X = -6;
                        float left  = pos.X+bbox.X,     player_left  = player.bbox.X;
                        float right = left+bbox.Width,  player_right = player.bbox.Z;
                        float top   = pos.Y+bbox.Y,     player_top   = player.bbox.Y;
                        float bot   = top+bbox.Height,  player_bot   = player.bbox.W;
                        const float xrange = 100, yrange = 70;
                        // IN RANGE TO TRY ATTACK?
                        if ((right > player_left - xrange) && (left < player_right + xrange) &&
                            (bot   > player_top - yrange)  && (top  < player_bot - yrange))
                        {                            
                            // BEGIN ATTACK
                            if (last_motion != Act.attack)
                            {                                
                                motion = Act.attack;
                                if (pan.Y > 0)
                                {
                                    switch (type)           // (sounds could be referenced with indexing[] too if you have lots of monster types)
                                    {
                                        case MonsterType.Mouster: MonsterSys.monster_attack1.Play(pan.Y, 0.8f, pan.X); break;
                                        case MonsterType.Hellcat: MonsterSys.monster_attack2.Play(pan.Y, 0.8f, pan.X); break;
                                    }
                                }
                            }
                            // DOES ATTACK HIT PLAYER?  // (only check if the animation's played past the first set of keys [****this may depend on how you made the attack keys****] 
                            else if ((motion == Act.attack) && (meo_play.key1 > attack_key) && (player.invincible <= 0))
                            {
                                top = pos.Y + attack_box.Y; bot = top + attack_box.Height;
                                if (!flip) { left = pos.X + attack_box.X; right = left + attack_box.Width; }
                                else { right = pos.X - attack_box.X; left = right - attack_box.Width; }
                                if ((right > player_left)&&(left<player_right) &&
                                    (bot > player_top) && (top < player_bot))
                                {
                                    player.HeroDamaged(0.2f); 
                                }
                            }
                        }
                    }
                }
            }
            else if (mode == Mode.run_away) // R U N  A W A Y  (simple for now [maybe later add logic to escape spells too])
            {
                if (pos.X < player.pos.X) target_vel.X = -6;
                if (pos.X > player.pos.X) target_vel.X = 6;
            }
            #endregion

            // ADJUST VELOCITY / MOTION
            if (vel.X < target_vel.X) vel.X += 0.5f;
            if (vel.X > target_vel.X) vel.X -= 0.5f;
            if (vel.X == 0) { if (motion != Act.jump) motion = Act.idle; }
            else if (motion == Act.idle)
            {
                motion = Act.walk;
                if (type == MonsterType.Mouster) motion = Act.run;
            }
        } // ^^^ UpdateAI ^^^



        //-------------
        // U P D A T E  ( monster motions / AI)
        //-------------
        public void Update(GameTime gameTime, bool update_AI = true)
        {
            old_pos = pos;
            old_loc = loc;
            
            if (update_AI)
            {
                UpdateAI();              // eventually you may want to change this to dispatch a unique AI for different types of monsters
            }

            // GRAVITY
            if (vel.Y < MAX_FALL_SPEED) vel.Y += GRAVITY;
            // SLOW DOWN MONSTER:
            MeoAnimation walk_anim = meo.anim[walk_index]; // reference the walk for easier access
            if (vel.X > 0)
            {
                vel.X -= 0.40f; 

                if (vel.X < 0) 
                    vel.X = 0; 

                walk_anim.speed = vel.X *  0.42f; 
            }

            if (vel.X < 0) 
            { 
                vel.X += 0.40f; 
                if (vel.X > 0) vel.X = 0; 
                walk_anim.speed = vel.X * -0.42f; 
            } // animation speeds can only be positive (- x - = +)
            
            // DETERMINE FLIP (assumes monsters initially facing left)
            if (vel.X < -1)
                flip = true; 
            else if (vel.X > 1) 
                flip = false;
            // MOVE MONSTER
            Vector2 tru_vel = vel * rescale; // factors character resize into velocity
            pos += tru_vel;
            // STEREO PAN
            pan = Sound.GetPan(screen_pos); 

            // ANIMATE: 
            bool allow_switch = true;
            if (must_finish_animation)
            {
                if (!meo_play.IsDoneAnimation()) allow_switch = false;
                else
                {
                    must_finish_animation = false;
                    motion = Act.idle;
                }
            }

            if ((allow_switch) && (motion != last_motion))
            {
                switch (motion)
                {
                    case Act.idle:   meo_play.SetAnimation("idle",   flip); 
                        break;
                    case Act.walk:   meo_play.SetAnimation("walk",   flip); 
                        break;
                    case Act.run:    meo_play.SetAnimation("run",    flip); 
                        break;
                    case Act.attack: meo_play.SetAnimation("attack", flip); 
                        break;
                    case Act.ouch:   meo_play.SetAnimation("ouch",   flip); 
                        break;
                    case Act.jump:   if (jump_index>0) meo_play.SetAnimation("jump",   flip); 
                        break;   // if jump animation exists (none for hellcat)
                }
            }
            meo_play.flip = flip;
            last_motion = motion;

            if ((screen_pos.X > bbox.X) && (screen_pos.X + bbox.X < screenW) 
                && (screen_pos.Y > bbox.Y) && (screen_pos.Y + bbox.Y < screenH))
                meo_play.Update(gameTime);

            // TEST IF PLAYER ATTACK HITS THIS MONSTER
            // P L A Y E R   A T T A C K E D   M O N S T E R   T E S T --------------------------
            if (invincible <= 0)
            {
                if (mode == Mode.run_away) mode = Mode.patrol;
                const int radius = 32, rad2 = 64; // radius of spell, radius * 2
                Rectangle bound_rec = new Rectangle((int)(pos.X + bbox.X),
                    (int)(pos.Y + bbox.Y), (int)bbox.Width, (int)bbox.Height);
                int i = 0;
                while (i < player.spells.Count)
                {
                    Vector2 spos = player.spells[i].emit_world_pos;
                    Rectangle spell_rec = new Rectangle((int)(spos.X - radius),
                        (int)(spos.Y - radius), rad2, rad2);

                    if (spell_rec.Intersects(bound_rec)) 
                        MonsterDamaged(player.spell_power); 
                    i++;
                }
            }
            else { invincible--; mode = Mode.run_away; }
            // ^^^ player attacked monster test ^^^ ----------------------------------------------

        } // ^^^ update ^^^



        //----------------------------------------------------------
        #region // C O L L I S I O N S (monster vs world)
        //----------------------------------------------------------
        
        //---------------------
        // T E S T  D O W N     (helper for World Collisions)
        //---------------------        
        void Test_Down(int x, int y, int h_offset = 0)
        {
            const float BOUNCE_VEL = -20f;                 // <-- how fast monster jumps up if it hits a spring platform
            float offy = 0;                                // <-- possible offset for collision region (ie: if spikes it is farther down)   
            if (tiles[x, y].stand_on)                      // WILL HIT SOLID (below - can stand-on)
            {
                if (tiles[x, y].spikes) offy = 28; else offy = 0;                               
                if ((pos.Y + bottom) > y * 64 + offy)    
                {
                    if (h_offset > 0) {
                        if (pos.X < x * 64 + h_offset) {  h_offset = 0; } // allow monster to stand at the very edge (hard coded as 84 (64+20) - change for other character sizes)     
                    }
                    else if (h_offset < 0) {
                        if (pos.X > x * 64 + h_offset) {  h_offset = 0; }  // allow monster to stand at the very edge     
                    }
                    if (h_offset == 0)    
                    {    
                        pos.Y = y * 64 - bottom + offy;    // set position so on top of tile     
                        vel.Y = 0;
                        grounded = true;                   // stop falling and set grounded (allows jump)    
                        if (offy == 28) on_spikes = true;    
                        if (tiles[x, y].type == TileType.spring) { vel.Y = BOUNCE_VEL; bounceMgr.Add(x, y); MonsterJump(play_bounce: true); } // spring platform    
                    }    
                }                    
            }
        }

        //-------------------
        // T E S T  R I G H T
        //-------------------
        void Test_Right(int x, int y)
        {
            // CAUTION (possible out of bounds test) 
            if ((x < 0) || (x >= Map.TILES_WIDE - 1)) {
                pos.X = old_pos.X; x = old_loc.X; return; 
            }
            int half_width = bbox.Width / 2 - 10;
            
            if (tiles[x, y].is_solid)                        // IN SOLID (to right)
            {
                if ((pos.X + half_width) > x * 64)
                {
                    if ((!on_spikes) || (!tiles[x, y].spikes)) {
                        pos.X = x * 64 - half_width - 1;  vel.X = 0f;
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
            if ((x < 0) || (x >= Map.TILES_WIDE - 1)) {
                pos.X = old_pos.X; x = old_loc.X; return;
            }
            int half_width = bbox.Width / 2 - 10;

            if (tiles[x, y].is_solid)                   // WILL HIT SOLID (to left)
            {
                if ((pos.X - half_width) < (x + 1) * 64)
                {
                    if ((!on_spikes) || (!tiles[x, y].spikes)) {
                        pos.X = x * 64 + 64 + half_width;  vel.X = 0f;
                        loc.X = (int)pos.X / 64;                        
                    }
                }
            }
        }

        //-------------------------------
        // W O R L D  C O L L I S I O N S
        //-------------------------------
        bool on_spikes;
        bool last_grounded;
        float bottom; 
        public void WorldCollisions()
        {
            // GET MONSTER'S SCREEN POSITION
            meo_play.position = screen_pos = Conv.world_to_screen(pos);

            // get monster tile occupation:
            loc.X = (int)pos.X / 64;
            loc.Y = (int)pos.Y / 64;
            int x = loc.X, y = loc.Y;

            // PREVENT ILLEGAL MEMORY ACCESS:
            if ((x + 1 >= Map.TILES_WIDE) || (x - 1 < 0) || (y + 1 >= Map.TILES_HIGH) || (y - 1 < 0)) { pos = old_pos; loc = old_loc; x = loc.X; y = loc.Y; }

            // GET MOVEMENT VECTOR:
            Vector2 move = pos - old_pos;
            // (! Assume velocity never exceeds tile size of 64 which is extremely fast [otherwise you need a sweep test for closest tile interactions] !)     
                      
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
                if (tiles[x, y - 1].is_solid)
                {                  // WILL HIT SOLID (up)                
                    if ((pos.Y + bbox.Y) < y * 64) { vel.Y = 0; pos.Y = old_pos.Y; }
                }
                if (tiles[x - 1, y - 1].is_solid)
                {              // WILL HIT SOLID (up-left)                
                    if ((pos.Y + bbox.Y) < y * 64)
                    {
                        if (pos.X < (x - 1) * 64 + 84) { vel.Y = 0; pos.Y = old_pos.Y; }
                    }
                }
                if (tiles[x + 1, y - 1].is_solid)
                {              // WILL HIT SOLID (up right)                
                    if ((pos.Y + bbox.Y) < y * 64)
                    {
                        if (pos.X > (x + 1) * 64 - 20) { vel.Y = 0; pos.Y = old_pos.Y; }
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
                    if ((invincible <= 0)&&(vel.Y>-1))
                    {                        
                        vel.Y = -6f;
                        MonsterDamaged(1);                        
                    }
                }

                if ((on_spikes == false) && (vel.Y>2f) && (grounded) && (!last_grounded)) 
                    if (pan.Y>0) Sound.land.Play(pan.Y, 0.8f, pan.X);
            }//^^^ move.Y>0 ^^^            
            last_grounded = grounded; 
        } // ^^^ world collisions ^^^
        #endregion



        //--------
        // D R A W 
        //--------
        public void Draw()
        {
            // IS VISIBLE? (if no return) 
            if ((screen_pos.X < bbox.X) || (screen_pos.X + bbox.X > screenW) || (screen_pos.Y < bbox.Y) || (screen_pos.Y + bbox.Y > screenH)) return;

            // DRAW MONSTER
            Color col = Color.White;
            if (invincible > 0)
            {
                int n = invincible % 10;
                if (n > 5) col = new Color(255, 155, 155, 255); // flashing if invincible 
            }
            meo_play.Draw(col); 
        }

        

        //-------------------------
        // D R A W  C O L L I D E R (s)  (for debugging collision detections)
        //-------------------------
        public void DrawCollider(SpriteBatch spriteBatch, Texture2D tex)
        {
            // IS VISIBLE? (if no return) 
            if ((screen_pos.X < bbox.X) || (screen_pos.X + bbox.X > screenW) || (screen_pos.Y < bbox.Y) || (screen_pos.Y + bbox.Y > screenH)) return;

            float left = screen_pos.X + bbox.X;
            float top  = screen_pos.Y + bbox.Y;
            Rectangle pixel = new Rectangle(896, 0, 1, 1);

            // SHOW MONSTER'S VULNERABLE BBOX: 
            spriteBatch.DrawRectLines(tex, pixel, new Rectangle((int)left, (int)top, bbox.Width, bbox.Height), Color.White); 
            // SHOW MONSTER'S ATTACKING BBOX:
            top = screen_pos.Y + attack_box.Y;
            if (!flip) { left = screen_pos.X + attack_box.X; } else { left = screen_pos.X - attack_box.X - attack_box.Width; }
            //Console.WriteLine("coord: " + left + "," + top + "," + attack_box.Width + "," + attack_box.Height);
            spriteBatch.DrawRectLines(tex, pixel, new Rectangle((int)left, (int)top, attack_box.Width, attack_box.Height), Color.Red); 
        }



        // RESCALE RECT (helper for Get Monster Properties) 
        Rectangle Rescale_Rect(int X, int Y, int Width, int Height)
        {
            Vector2 corner    = new Vector2(X,Y) * rescale;
            Vector2 dimension = new Vector2(Width, Height) * rescale;
            return new Rectangle((int)corner.X, (int)corner.Y, (int)dimension.X, (int)dimension.Y);
        }


        // G E T  M O N S T E R  P R O P E R T I E S  (and assign a bounding box) 
        string get_monster_properties(MonsterType mType)
        {
            string sheet_name = "mouster";    
            switch (mType) {
                case MonsterType.Mouster:
                    sheet_name = "mouster";                    
                    bbox       = Rescale_Rect(-150,-115,300,205);      // top-left from center, width, height (box around monster for collision detection [sizes using info in original image before dissection])
                    attack_box = Rescale_Rect(30, -130, 120, 180);     // top_left from center, width, height (for attack region that can hit player)                                                                                
                    bottom     = bbox.Y + bbox.Height - 12 *rescale.Y; // bottom = custom value (-14) to make sure it looks right when standing on something
                    attack_key = 1;                                    // which animation key does the attack begin to be dangerous
                    life       = 3;
                    break;
                case MonsterType.Hellcat:
                    sheet_name = "hellcat";                    
                    bbox       = Rescale_Rect( -80, -135, 160, 170);
                    attack_box = Rescale_Rect(-190, -100, 380, 110);
                    bottom     = bbox.Y + bbox.Height-11*rescale.Y;    // bottom = custom value (-11) so it looks right when standing on something
                    attack_key = 11;
                    life       = 2;
                    break;
            }
            return sheet_name; 
        }


        // C U S T O M I Z E  M O N S T E R 
        void customize_monster()
        {
            int index;
            jump_index = meo_play.GetIndex("jump", false); // FALSE = don't show errors for this one since if no jump is available that will be ok
            ouch_index = meo_play.GetIndex("ouch");
            meo_play.SetAnimation("idle", flip); motion = Act.idle;
            index = meo_play.GetIndex("idle");   meo.anim[index].speed = 0.8f;  // slow down the idle a bit    
            switch (type)
            {
                case MonsterType.Mouster:                    
                    walk_index = meo_play.GetIndex("run");
                    break; 
                case MonsterType.Hellcat:                    
                    walk_index = meo_play.GetIndex("walk");
                    break; 
            }
        }
    }
}
