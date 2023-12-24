using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Explosions
{
    class Animator
    {
        // ANIMATOR (helper - tracks explosions)        
        public int frame_index;      // frame number
        public int timer;            // time between frames  
        public Vector2 pos;          // world position
        public Rectangle frame;      // frame rectangle position    (where on the sprite sheet to get the source rect from) 
        public bool done;            // animation done

        // CONSTRUCT
        public Animator(Vector2 world_pos)
        {
            pos = world_pos;
        }        
    }
}
