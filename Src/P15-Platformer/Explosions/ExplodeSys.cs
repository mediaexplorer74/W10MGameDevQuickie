using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.Explosions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    //  E X P L O D E  S Y S    (CREATE AND RUN EXPLOSIONS)

    class ExplodeSys
    {
        const int WAIT_TIME = 3;                         // effects animation speed (ie: 1=fast, 9=slow)

        SpriteBatch    spr;                              // reference to spriteBatch in Game1
        Rectangle      start_frame;                      // first frame of the animated explosion
        Vector2        origin;                           // rotate and scale around origin
        int            width, height;                    // rectangle size
        int            num_frames;
        Texture2D      tex;                              // sheet containing explosion images                             
        List<Animator> animators;                        // running explosion animations

        
        // CONSTRUCT
        public ExplodeSys(SpriteBatch sbatch)
        {
            spr = sbatch;
            animators = new List<Animator>(); 
        }


        // L O A D                                         (either load here... or pass in a texture that contains the explosion)
        public void Load(ContentManager Content, int frame_total) {
            tex = Content.Load<Texture2D>("explosion1");
            start_frame = new Rectangle(0, 0, 256, 256);  width = height = 256;
            num_frames  = frame_total;
            origin = new Vector2(127, 127);
        }
        public void Load(Texture2D Tex, Rectangle first_rectangle, int frame_total) {
            tex = Tex;
            start_frame = first_rectangle;
            width = start_frame.Width;   height = start_frame.Height;
            num_frames  = frame_total;
            origin = new Vector2(start_frame.Width/2, start_frame.Height/2);
        }



        // A D D  E X P L O S I O N 
        public void Add_Explosion(Vector2 world_position)
        {            
            Animator anim = new Animator(world_position);
            anim.frame    = start_frame;
            animators.Add(anim);             
        }



        // U P D A T E
        public void Update()
        {
            int i = 0;
            while (i < animators.Count) {
                Animator a = animators[i];
                a.timer++;
                if (a.timer > WAIT_TIME) {                              // time to change frames
                    a.timer = 0;
                    a.frame_index++;
                    a.frame.X += width;
                    if (a.frame_index >= num_frames)
                    {
                        a.frame_index = 0; a.timer = 0;
                        a.frame = start_frame;
                        a.done = true;
                    } 
                    else if (a.frame.X >= (tex.Width - 1)) {     // next row
                        a.frame.X = 0;
                        a.frame.Y += height;                        
                    }                    
                }
                i++;
            }
            animators = animators.Where(x => !x.done).ToList();  // remove finished animations
        } // update



        // D R A W 
        public void Draw()
        {
            if (animators.Count <= 0) return;
            int i = 0;
            while (i < animators.Count)
            {
                Animator a = animators[i];
                Rectangle rect = new Rectangle(a.frame.X, a.frame.Y, width, height);
                Vector2 pos = Conv.world_to_screen(a.pos);
                spr.Draw(tex, pos, rect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
                i++;
            }
        }// draw

    }
}
