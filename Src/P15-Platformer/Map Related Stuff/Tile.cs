using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager {

    class StartData {
        public int x,y;
        public StartData(int X, int Y) { x = X; y = Y; }
    }

    
    public enum TileType { empty, solid, reflector, spring, platform, spikes }


    // T I L E
    class Tile
    {
        public int index;
        public TileType type;
        public Vector2 scale;
        public Vector2 offset;
        public float rot;
        public bool overlap;
        public bool stand_on;
        public bool is_solid;
        public bool spikes;
        public bool event_active;
        public MonsterType monster_start;

        public Tile(int Index, TileType Type)
        {
            index = Index;
            type = Type;
            scale = Vector2.One;
            monster_start = MonsterType.None;
        }

        public void Clear()
        {
            index = 0;
            type = TileType.empty;
            scale = Vector2.One;
            offset = Vector2.Zero;
            rot = 0;
            overlap = stand_on = spikes = is_solid = false;
            event_active = false; monster_start = MonsterType.None;
        }
    }//Tile


    // PROCESSED TILE 
    class ProcessedTile
    {
        public Vector2 pos;
        public Vector2 scale;
        public float rot;
        public Rectangle rect;

        public void Add(Vector2 position, Rectangle srtRect, float rotation, Vector2 Size)
        {
            pos = position;
            rect = srtRect;
            rot = rotation;
            scale = Size;
        }
    }
}
