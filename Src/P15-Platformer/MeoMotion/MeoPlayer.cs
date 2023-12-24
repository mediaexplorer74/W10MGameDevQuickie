using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    class MeoPlayer
    {
        public bool PREMULTIPLY_ALPHA = true;
        const float HIDE_ALPHA_UNDER = 0.02f; // minimum alpha (anything lower is clipped from calculations)        
        //--------------------------------------------------------------------------------------------------
        public Vector2 position;    // character's position         
        public string sheet_name;   // name of the original sheet used in the animator (without the .png) [this is just for identification purposes]
        public int animation_index; // index into list of animations (ie: anim[animation_index]) 
        public float play_speed;    // animation speed    
        public bool stopped, flip, active, reverse, last_reverse;
        public int key1;            // current key
        public int key2;            // next key                
        private float timer;        // for interpolating between keys        
        private bool done_anim;     // used in IsDoneAnimation() to tell if at least one animation cycle has finished
        private int part_count;
        private Final[] final;      // holds final animation data        

        public QuadBatch batch;
        MeoMotion meo;              // meo = original instance of MeoMotion manager which loads and contains all the animations


        // C O N S T R U C T O R 
        /// <summary>
        /// Creates a new character -- note: character can be changed by changing sheet_name and then setting an animation
        /// </summary>
        /// <param name="SheetName">used to determine which character on the spritesheet to use (uses the old original sheet name to refer to it)</param>
        /// <param name="Position">default position of character</param>
        /// <param name="Meo">instance of meo to use</param>
        /// <param name="spriteBatchDistort">instance of spriteBatchDistort to use for drawing</param>
        public MeoPlayer(string SheetName, Vector2 Position, MeoMotion Meo, QuadBatch quadBatch)
        {
            sheet_name = SheetName;
            position   = Position;
            play_speed = 1.0f; part_count = 0; flip = false; reverse = false;
            meo = Meo;
            final = new Final[(meo.max_parts + 1)]; // create a memory pool big enough to hold any character's render data
            int a = 0;
            do { final[a] = new Final(); a++; } while (a < (meo.max_parts + 1)); // allocate
            batch = quadBatch;
        }


        // S E T  A N I M A T I O N 
        /// <summary>
        /// Set a character animation by name with option to set flip horizontally
        /// </summary> 
        public void SetAnimation(string AnimationName, bool Flip = false, bool play_backward = false) // note: normally, if changing character direction - just change public variable flip = !flip
        {
            if (AnimationName == "none") 
            { 
                active = false; 
                return; 
            }    // usually better to just change public var active to false for this
            
            animation_index = meo.GetIndex(sheet_name, AnimationName);

            part_count = 0;// meo.anim[animation_index].end_part - meo.anim[animation_index].start_part; 
            // make sure we are using the correct part_count
            
            flip = Flip; 
            active = true; 
            key1 = 0; 
            key2 = 1;
            
            timer = 0; // meo.anim[animation_index].times[key1]; 
            
            done_anim = false; 
            stopped = false; 
            reverse = false; 
            last_reverse = false;
            
            if (play_backward) 
                StartReverse(); 
        }
        public void SetAnimation(int AnimationIndex, bool Flip = false,
            bool Active=true, bool play_backward = false) // (if we already know the index)
        {
            if (!Active)
            { 
                active = false; 
                return; 
            }
            animation_index = AnimationIndex;
            
            part_count = 0;// meo.anim[animation_index].end_part - meo.anim[animation_index].start_part; 
            // make sure we are using the correct part_count
            
            flip = Flip; 
            active = true; 
            key1 = 0; 
            key2 = 1;
            
            timer = 0;// meo.anim[animation_index].times[key1]; 
            done_anim = false; stopped = false; reverse = false; last_reverse = false;
            if (play_backward) StartReverse();
        }


        // see if at the end of an animation cycle
        public bool IsDoneAnimation() 
        {
            if (last_reverse != reverse) 
                return false;  // just changed playback direction so not done
           
            if (done_anim) 
            {
                done_anim = false; 
                return true; 
            }
            return false; 
        }



        public void StartReverse()
        {
            key2 = 0;// meo.anim[animation_index].num_keys - 1; 
            key1 = key2 - 1; 
            done_anim = false; 
            stopped = false; 
            reverse = true;

            timer = 0;// meo.anim[animation_index].times[key2]; 
        }



        // U P D A T E 
        // Customizable update for character animation. Could have other updates with other behaviors too, or pass in vars to control behavior decisions.. (like point weapon or something)
        public void Update(GameTime gameTime)
        {
            if (!active) return;
            int a = animation_index;
            int k1 = key1, k2 = key2;
            int t1 = 0;// meo.anim[a].times[k1]; 
            int t2 = 0;// meo.anim[a].times[k2];
            float time_dif, percent;

            if (1==1)//(meo.anim[a].looping) 
                done_anim = false;

            if (reverse)
            {
                timer -= 16.666667f * (play_speed);// * meo.anim[a].speed); //timer -= (gameTime.ElapsedGameTime.Milliseconds * (play_speed * meo.anim[a].speed)); // <-- (could use this also) track milliseconds that passed since start of first key                       
                if (timer < t1) // ready to switch keys to interpolate between
                {
                    key2 = key1;
                    key1--;
                    if (key1 < 0)
                    {
                        if (meo.anim[a].looping == false)
                        {   // STOP ANIMATION
                            key1 = 0; 
                            key2 = 0; 
                            stopped = true;
                            timer = 0;// meo.anim[a].times[key1]; 
                            done_anim = true; last_reverse = reverse; return;
                        }
                        else
                        {   // LOOP ANIMATION
                            key1 = 0;// meo.anim[a].num_keys-2; 
                            key2 = key1+1;
                            timer = 0;// meo.anim[a].times[key2]; 
                            done_anim = true;
                        }
                    }
                    k1 = key1; 
                    k2 = key2;
                    t1 = 0;// meo.anim[a].times[k1]; 
                    t2 = 0;// meo.anim[a].times[k2];
                }
            }
            else
            {
                timer += 16.666667f * (play_speed/* * meo.anim[a].speed*/); //timer += (gameTime.ElapsedGameTime.Milliseconds * (play_speed * meo.anim[a].speed)); // <-- (could use this also) track milliseconds that passed since start of first key                       
                if (timer > t2) // ready to switch keys to interpolate between
                {
                    key1 = key2;
                    key2++;
                    if (key2 >= 0)//meo.anim[a].num_keys)
                    {
                        if (true)//(meo.anim[a].looping == false)
                        {   // STOP ANIMATION
                            key2 = 0;//meo.anim[a].num_keys - 1; 
                            stopped = true; timer = 0; 
                            done_anim = true; 
                            last_reverse = reverse; 
                            return;
                        }
                        else
                        {   // LOOP ANIMATION
                            key1 = 0; 
                            key2 = 1; 
                            timer = 0; 
                            done_anim = true; 
                        }
                    }
                    k1 = key1; 
                    k2 = key2;
                    t1 = 0;// meo.anim[a].times[k1];
                    t2 = 0;// meo.anim[a].times[k2];
                }
            }

            time_dif = t2 - t1;                     // total time between both keys
            if (time_dif <= 0) 
                time_dif = 0.0001f;  // prevent unlikely possibility of division by zero

            percent = (timer - t1) / time_dif;      // what is the percentage (0-1) [for interpolation]

            Vector2 pos, scale, o1, o2, o3, o4;
            float rot;
            float x1, x2, x3, x4;
            float y1, y2, y3, y4;
            int i = 0, p;
            do
            {
                final[i].order = 0;// meo.anim[a].keys[i, k1].order;
                if (true)//(meo.anim[a].keys[i, k1].active == false)
                { 
                    final[i].hide = true; 
                    i++; 
                    continue; 
                } // (part not shown - skip - go to do)
                
                final[i].hide = false;
                final[i].part = 0;// meo.anim[a].keys[i, k1].part;
                p = final[i].part;

                // interpolate the data between the 2 keyframes
                final[i].alpha = 0;// MathHelper.Lerp(meo.anim[a].keys[i, k1].alpha, meo.anim[a].keys[i, k2].alpha, percent); // blend alpha transparency
                if (final[i].alpha < HIDE_ALPHA_UNDER) { final[i].hide = true; i++; continue; }
                if (final[i].alpha > 1.0f) final[i].alpha = 1.0f;                                                        // precaution
                pos = Vector2.Lerp(meo.anim[a].keys[i, k1].pos, meo.anim[a].keys[i, k2].pos, percent);                   // blend position                
                rot = MathHelper.Lerp(meo.anim[a].keys[i, k1].rot, meo.anim[a].keys[i, k2].rot, percent);                // blend rotation
                scale = Vector2.Lerp(meo.anim[a].keys[i, k1].scale, meo.anim[a].keys[i, k2].scale, percent);             // blend scale                
                o1 = Vector2.Lerp(meo.anim[a].keys[i, k1].o1, meo.anim[a].keys[i, k2].o1, percent);                      // blend distortion offsets
                o2 = Vector2.Lerp(meo.anim[a].keys[i, k1].o2, meo.anim[a].keys[i, k2].o2, percent);
                o3 = Vector2.Lerp(meo.anim[a].keys[i, k1].o3, meo.anim[a].keys[i, k2].o3, percent);
                o4 = Vector2.Lerp(meo.anim[a].keys[i, k1].o4, meo.anim[a].keys[i, k2].o4, percent);
                // calculate the transformed vertices from the above data                
                x1 = meo.parts[p].m1.X * scale.X; y1 = meo.parts[p].m1.Y * scale.Y; // scale part points at origin(0,0)
                x2 = meo.parts[p].m2.X * scale.X; y2 = meo.parts[p].m2.Y * scale.Y;
                x3 = meo.parts[p].m3.X * scale.X; y3 = meo.parts[p].m3.Y * scale.Y;
                x4 = meo.parts[p].m4.X * scale.X; y4 = meo.parts[p].m4.Y * scale.Y;
                pos += meo.anim[a].offset; // adjust postions by default offset property
                // HERE YOU COULD ADD EXTRA PROGRAMMED ROTATION RESPONSES (like trailing hair, tail, etc - or vector based weapon pointing)
                // Note: Use: meo.parts[p].name to identify and if has children add child to end-point of parent-limb (and add parent rotation to child rotation also)  
                if (rot != 0f)
                {
                    float cos = (float)Math.Cos(rot), sin = (float)Math.Sin(rot);   // rotate points around origin and then add the position where they belong
                    final[i].v1.X = pos.X + x1 * cos - y1 * sin; final[i].v1.Y = pos.Y + x1 * sin + y1 * cos;
                    final[i].v2.X = pos.X + x2 * cos - y2 * sin; final[i].v2.Y = pos.Y + x2 * sin + y2 * cos;
                    final[i].v3.X = pos.X + x3 * cos - y3 * sin; final[i].v3.Y = pos.Y + x3 * sin + y3 * cos;
                    final[i].v4.X = pos.X + x4 * cos - y4 * sin; final[i].v4.Y = pos.Y + x4 * sin + y4 * cos;
                }
                else
                {
                    final[i].v1.X = pos.X + x1; final[i].v1.Y = pos.Y + y1;         // no rotation, so just put the points in the correct position
                    final[i].v2.X = pos.X + x2; final[i].v2.Y = pos.Y + y2;
                    final[i].v3.X = pos.X + x3; final[i].v3.Y = pos.Y + y3;
                    final[i].v4.X = pos.X + x4; final[i].v4.Y = pos.Y + y4;    
                }
                final[i].v1 += o1; // add the distortion offsets of the points
                final[i].v2 += o2;
                final[i].v3 += o3;
                final[i].v4 += o4;
                if (flip) // flip horizontally: 
                {
                    final[i].v1.X = -final[i].v1.X;
                    final[i].v2.X = -final[i].v2.X;
                    final[i].v3.X = -final[i].v3.X;
                    final[i].v4.X = -final[i].v4.X;
                }
                i++;
            }
            while (i < part_count);

            last_reverse = reverse; 
        }//Update



        // D R A W
        // Customizable draw for character. Could make other draw overloads or pass in vars to control drawing behavior for specific characters. 
        public void Draw(Color color)
        {
            if (!active) return;
            int i = 0, n = 0, p;
            Color col;
            n = 0;
            do
            {
                i = final[n].order;
                if (final[i].hide)
                {
                    n++;
                    continue;
                }

                p = final[i].part;                                  // may switch parts during animation
                color.A = (byte)(final[i].alpha * 255.0f);          // alpha transparency from 0-255                
                if (PREMULTIPLY_ALPHA) col = Color.FromNonPremultiplied(color.ToVector4()); // make sure the properties of the image in content is set to premultiply alpha true
                else col = color;

                try
                {
                    if (meo.parts[p] != null)
                    {
                        batch.DrawTransformedVertices(meo.parts[p].rect,
                        position, final[i].v1, final[i].v2, final[i].v3, final[i].v4, col);
                        // draw transformed sprite parts at character's position
                    }
                }
                catch { }
                n++;
            } while (n < part_count);
        }


        // GET INDEX
        public int GetIndex(string AnimationName, bool show_errors = true)
        {
            if (AnimationName == "none") return 0;
            return meo.GetIndex(sheet_name, AnimationName, show_errors); 
        }

    }
}
