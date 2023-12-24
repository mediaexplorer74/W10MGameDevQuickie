using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    // Loads monster sets, add / delete monsters / calls to draw/update monsters
    // In this version, Monsters are added using the map editor to indicate where monsters start... but you could make a off-screen spawn method too
    class MonsterSys
    {
        const int   MAX_MONSTERS = 30;                    // most number of monsters allowed in a level                
        
        // FOR MEOMOTION ANIMATION
        MeoMotion           meo;                        // MeoMotion class to contain monsters 
        public Monster[]    monsters;                   // unique monsters (which have their own MeoPlayers)         
        int                 num_monst;        
        Vector2             rescale;                    // adjusts character size and motion based on resolution

        // OTHER
        QuadBatch           quadBatch;                  // reference to quadBatch (provided to player in Monster class)
        Map                 mp;                         // reference to map   
        Player              player;                     // reference to player
        ExplodeSys          explode;                    // reference to explodeSys (explosion animations)

        // SOUND
        // if you end up with lots of monster sounds you could make arrays and use MonsterType enums to index them 
        static public SoundEffect monster_attack1;  // Mouseter attack sound   
        static public SoundEffect monster_attack2;  // HellCat " "
        static public SoundEffect monster_ouch1;    // Mouster ouch
        static public SoundEffect monster_ouch2;    // HellCat ouch


        // C O N S T R U C T
        public MonsterSys(ContentManager Content, QuadBatch QBatch, Map map, Player playr, ExplodeSys ex)
        {
            meo       = new MeoMotion(Content, QBatch);
            monsters  = new Monster[MAX_MONSTERS];
            num_monst = 0;
            
            quadBatch = QBatch;
            mp        = map;
            player    = playr; 
            explode   = ex;
        }


        //--------
        // L O A D 
        //--------
        public void Load(int lev, ContentManager Content)
        {
            rescale = Game1.rescale * 0.5f; // make 50% smaller than original monster sizes
            switch (lev) {
                case 1:                     
                    meo.Load_TXT("Monsters1", rescale);                             // load characters/animations (.TXT must be with exe file [ie: BIN - Windows - debug/release] )
                    monster_attack1 = Content.Load<SoundEffect>("Sound/MousterHit");
                    monster_ouch1   = Content.Load<SoundEffect>("Sound/MousterOuch");
                    monster_attack2 = Content.Load<SoundEffect>("Sound/ChainsawHit");
                    monster_ouch2   = Content.Load<SoundEffect>("Sound/CatOuch");
                    break;
            }                                    
        }


        //---------------------
        // A D D  M O N S T E R 
        //---------------------
        public void AddMonster(MonsterType type, Vector2 Pos)
        {
            // Check existing monsters (if any) and try to change any dead ones to the new one (if any)             
            int i = 0;
            while (i < num_monst) {
                if (monsters[i].dead) {
                    monsters[i].ChangeMonster(Pos, type);                           // make alive and change to desired monster
                    return;                                                         // monster is added so return
                }
                i++;
            }
            // No existing monsters that are dead to replace - see if we can add a new one:
            if (num_monst >= MAX_MONSTERS) return;                                    // maximum - can't add more to this level - return 
            monsters[num_monst] = new Monster(Pos, meo, quadBatch, type, mp, player, explode); // CREATE MONSTER
            num_monst++;
        }


        //------------
        // D E L E T E ( editor delete of monster by map-tile coordinate [ in the game mode you simply set it to dead=true ] ) 
        //------------
        public void Delete()
        {
            int i = 0;
            while (i < num_monst) {
                if (monsters[i].loc == mp.loc) {        // if a monster occupies the editor tile location: 
                    int a = i;
                    while (a < num_monst-1) {           // shift the list back 1 and delete the last entry since it becomes a duplicate of the 2nd-last    
                        if (monsters[a + 1] != null) monsters[a] = monsters[a + 1]; else break;
                        a++;
                    }
                    monsters[a] = null; num_monst--;    // delete last entry since they were copy-shifted back 1
                }
                i++;
            }
        }


        //------------
        // U P D A T E  ( AI )
        //------------
        public void Update(GameTime gameTime, bool updateAI = true)
        {
            int i = 0;            
            while (i < num_monst)
            {
                if (monsters[i].dead) { i++; continue; }
                monsters[i].Update(gameTime, updateAI);                     // update all monster motions in world
                monsters[i].WorldCollisions();                              // check for world collisions                
                i++;
            }
        }


        //--------
        // D R A W  
        //--------
        public void Draw()
        {
            quadBatch.Begin(meo.tex, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None);
            int i = 0;
            while (i < num_monst)
            {
                if (!monsters[i].dead) monsters[i].Draw();                   //  ( draw visible monsters )
                i++;
            }
            quadBatch.End();
        }



        public void DrawColliders(SpriteBatch spriteBatch, Texture2D tx) 
        {
            // get the bounding box in screen coordinates:
            // top left & bottom right coords - used to visualize if enemy attack hits you (monsters.cs):  
            Rectangle sbox = Conv.bbox_world_to_screen(player.bbox);
            Rectangle pixel = new Rectangle(896, 0, 1, 1); 

            // SHOW PLAYER'S VULNERABLE BBOX:
            spriteBatch.DrawRectLines(tx, pixel, sbox, Color.White);

            int i = 0;
            while (i < num_monst)
            {
                if (!monsters[i].dead) monsters[i].DrawCollider(spriteBatch, tx); 
                i++;
            }

        }
    }
}
