using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    // BOUNCER (spring platform)
    class Bouncer
    {
        public int x, y;            // tile to run bounce animation on
        public Vector2 original_offset;
        double distance, frequency; // controls wobble motion
        double f;

        public Bouncer(int tile_x, int tile_y, Vector2 original_Offset, double start_wobble_distance, double wobble_frequency)
        {
            x = tile_x;
            y = tile_y;
            original_offset = original_Offset;
            distance = start_wobble_distance;
            frequency = wobble_frequency;
            f = 0;
        }

        // UPDATE 
        public bool Update(ref Vector2 offset)
        {
            offset.Y = original_offset.Y + (float)(distance * Math.Sin(f));
            f += frequency;
            distance -= 0.6;
            if (distance < 1.0) return false;
            return true;
        }
    } //-----------------------------------



    //--------------------------------------------
    // B O U N C E  M G R  (manages bounce events)
    //--------------------------------------------
    class BounceMgr
    {
        Tile[,] tiles; //refers to Game1's tiles
        Bouncer[] bouncers; // tracks active bouncers animating
        int num_bouncers;   // number of active bouncers

        // CONSTRUCT
        public BounceMgr(Tile[,] Tiles)
        {
            tiles = Tiles;
            bouncers = new Bouncer[100];
        }


        // A D D  (tile event for bouncey stuff)
        public void Add(int x, int y)
        {
            if (tiles[x, y].event_active) return;
            bouncers[num_bouncers] = new Bouncer(x, y, tiles[x, y].offset, 25, 0.4);
            tiles[x, y].event_active = true;
            num_bouncers++;
        }

        // UPDATE
        public void Update()
        {
            if (num_bouncers < 1) return;
            int i = 0, bx, by;
            while (i < num_bouncers)
            {
                bx = bouncers[i].x; by = bouncers[i].y; // get tile of bouncer
                if (!bouncers[i].Update(ref tiles[bx, by].offset))
                {
                    tiles[bx, by].offset = bouncers[i].original_offset;
                    tiles[bx, by].event_active = false;
                    bouncers[i] = bouncers[num_bouncers - 1];
                    if (num_bouncers > 0) num_bouncers--;
                }
                i++;
            }
        }
    }

}
