using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    public enum ParticleType { fire, }         // later add other particle types

    class ParticleSystem
    {
        Particle[]    particles;
        public int    capacity, count;        // capacity = total amount possible,  count = current amount
        Texture2D     tex;                    // texture sheet to extract particle image from 
        Rectangle     source_rect;            // rectangle of image on tex sheet        
        Vector2       center;                 // center of source_rect
        ParticleType  particle_type;          // fire, etc...

        // REGULAR PARAMS
        public Color   start_col;
        public Vector2 emit_world_pos;                     // emitter's world
        public Vector2 emit_vel;                           // emitter's velocity (if moving)
        public float   emit_vel_dampen;                    // amount to slow down ( 0.1 = slow down now!!!, 0.9999 slow down gradually, 1=never slow down, 0 = stop )  
        public int     emitter_width,    emitter_height;   // girth at emitter location
        public Vector2 spray_direction;                    // adjust this in update if needed
        public float   start_scale;                        // use custom update with multiplier to do non-uniform scale updates
        public float   rot_speed;
        public float   velocity_range;                     // max randomization of velocity
        public int     min_life, max_life;                 // shortest and longest life span
        public float   gravity, scale_rate;                // gravity, scale speed                
        public bool    spawn;                              // whether to spawn new particles (if emitter is too slow it dies and stops producing new particles)
        public bool    dead;                               // if particles can no longer spawn and all life for each is too low then this emitter can be declared dead and thus removed

        // CONSTRUCT
        public ParticleSystem(Vector2 location, Vector2 vel, int max_particles, ParticleType type, Texture2D Tex)
        {
            tex = Tex; spawn = true;

            particles        = new Particle[max_particles];
            capacity         = particles.Length;                              
            for (int i = 0; i < capacity; i++) particles[i] = new Particle(); //allocate all particles (will be reused) 

            emit_world_pos   = location;    emit_vel = vel;          // set the world position and speed of the emitter
            particle_type    = type;         

            // COMMON SETTINGS:            
            emit_vel_dampen = 1.0f;                                  // default = no slow-down of emitter motion
            rot_speed       = 0.2f;            
            emitter_width   = 0;     emitter_height = 0;
            min_life        = 20;      
        
            // CUSTOM SETTINGS:
            switch (particle_type)
            {
                case ParticleType.fire:
                    spray_direction = new Vector2(0, -2.12f);       // sprays upward a bit
                    velocity_range = 1f;  
                    gravity = 0f;                                   // no gravity on flames
                    start_scale = 1.67f;  scale_rate = -0.025f;     // start size, how fast it changes size (shrinks in this case)
                    max_life    = 51;                               // how long particle stays alive
                    start_col   = new Color(125, 51, 26);           
                    source_rect = new Rectangle(128, 384, 64, 64);  // where image of particle is located                       
                    break;
            }
            center = new Vector2(source_rect.Width / 2, source_rect.Height / 2); 
        } // c o n s t r u c t



        // RESET 
        // (causes a restart of all particles at the emitter)
        public void reset() {
            for (int p = 0; p < particles.Length; p++) {
                particles[p].life = 0;
            }
            count = 0; spawn = true; 
        }



        // INIT PARTICLE
        void InitParticle(int i)
        {
            float r1 = (float)(Game1.rnd.NextDouble() * 2 - 1), r2 = (float)(Game1.rnd.NextDouble() * 2 - 1); // make 2 random numbers between -1 to +1
             Particle p = particles[i];
            p.col      = start_col;                                             // starting color (for more control you can use color.lerp and 2 colors [start,end]) 
            p.pos      = emit_world_pos;                                        // where the particle starts
            //p.pos.X += (float)Game1.rnd.Next(emitter_width);                  // later may want to use emitter_width or height for expanding emitter (ie: width for splash or flame effects)
            p.scale    = start_scale * (float)(Game1.rnd.NextDouble()+0.5f);    // randomize scale and resize it based on start_scale
            p.vel.X    = velocity_range * r1;                                   // scale the initial random velocity based on range
            p.vel.Y    = velocity_range * r2;
            p.vel      += spray_direction;                                      // add the spray direction into the velocity
            p.rot_vel  = rot_speed * r2;                                        // set a random rotation scaled by desired rotation speed
            p.rot      = p.rot_vel*10;                                          // give it some initial rotation                            
            p.life = p.lifespan = Game1.rnd.Next(min_life, max_life);           // randomly set life between min and max 
        }



        //------------
        // U P D A T E
        //------------       
        public void Update(bool is_projectile)
        {
            if (is_projectile)                 // ie: if a standard projectile
            {
                if (dead) return;
                emit_world_pos += emit_vel;
                emit_vel *= emit_vel_dampen;
                if ((emit_vel.X < 0.2f) && (emit_vel.Y < 0.2f) && (emit_vel.X > -0.2f) && (emit_vel.Y > -0.2f)) spawn = false; // slowed down a lot - stop making particles               
            }

            if (count < capacity)
            {
                if (spawn) InitParticle(count);    // ADD PARTICLE
                count++;
            }

            Particle p; 
            int i=0, c=0;                          // index, dead_particle_counter
            while (i < count)
            {
                p = particles[i];
                p.life--;
                if (p.life <= 0)
                {
                    if (spawn) InitParticle(i); else if (p.col.A > 8) p.col.A -= 8;
                    i++; c++; continue; 
                }
                p.vel.Y += gravity;
                p.pos += p.vel;
                p.rot += p.rot_vel;
                p.scale += scale_rate;
                switch (particle_type)              // custom particle behaviors:  
                {
                    case ParticleType.fire:
                        if (p.col.G > 2) p.col.G--; if (p.col.B > 2) p.col.B--; if (p.col.A > 2) p.col.A--;
                        if (p.scale < 0.02f) p.scale = 0.02f;
                        break;
                }
                i++; 
            }
            if (c >= count) dead = true;           // no particles have life left... particle sys is dead
        }



        //--------
        // D R A W  (spriteBatch version)
        //--------
        public void Draw(Vector2 offset, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < count; i++)
            {
                var p = particles[i];
                Vector2 poz = Conv.world_to_screen(p.pos + offset);
                spriteBatch.Draw(tex, poz, source_rect, p.col, p.rot, center, p.scale, SpriteEffects.None, 0f);                
            }
        } // d r a w


        //--------
        // D R A W  (quadBatch version)
        //--------
        public void Draw(Vector2 offset, QuadBatch quadBatch)
        {
            for (int i = 0; i < count; i++)
            {
                var p = particles[i];
                Vector2 poz = Conv.world_to_screen(p.pos + offset - center * p.scale);
                quadBatch.Draw(source_rect, poz, center, p.scale, p.rot, p.col, SpriteEffects.None);                
            }
        } // d r a w
    }
}
