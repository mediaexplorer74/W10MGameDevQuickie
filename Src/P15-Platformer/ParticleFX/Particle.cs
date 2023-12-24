using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    class Particle
    {
        public Vector2 pos, vel;
        public float rot, scale, rot_vel;
        public Color col;
        public float life, lifespan;

        public Particle()
        {
            col = Color.White; scale = 1.0f;
        }
    }
}
