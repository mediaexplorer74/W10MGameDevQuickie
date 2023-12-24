using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    class Sound
    {
        public static Song music;
        public static SoundEffect bounce;
        public static SoundEffect jump;
        public static SoundEffect land;
        public static SoundEffect ouch;
        public static SoundEffect attack;
        public static SoundEffect explode;
        public static float max_pan_distance;                            // maximum distance for stereo panning                     
        public static float max_vol_distance;                            // maximum distance for volume adjustment (based on vertical distance)
        // L O A D  (first)
        public static void Load(ContentManager content)
        {
            max_pan_distance = Game1.screenW * 2 - Game1.screen_center.X;
            max_vol_distance = Game1.screenH * 2 - Game1.screen_center.Y;
            bounce  = content.Load<SoundEffect>("Sound/Bounce");
            jump    = content.Load<SoundEffect>("Sound/Jump");
            land    = content.Load<SoundEffect>("Sound/Land");
            ouch    = content.Load<SoundEffect>("Sound/Ouch");
            attack  = content.Load<SoundEffect>("Sound/Attack");
            explode = content.Load<SoundEffect>("Sound/Explode");

            // later could set up a LoadSoundsForLevel(int lev) for new levels 

            music = content.Load<Song>("Music/Music1");

            try
            {
                MediaPlayer.IsRepeating = true;
            }
            catch { }

            try
            {
                MediaPlayer.Volume = 0.4f;
            }
            catch { }
        }

        
        // G E T  P A N
        public static Vector2 GetPan(Vector2 screen_pos)
        {
            Vector2 pan;
            float dist = screen_pos.X - Game1.screen_center.X;
            //dist = MathHelper.Clamp(dist, -Sound.max_pan_distance, Sound.max_pan_distance);
            pan.X = dist / Sound.max_pan_distance;
            if (pan.X > 0) pan.X = 1f - pan.X; else if (pan.X < 0) pan.X = -1f - pan.X;
            dist = screen_pos.Y - Game1.screen_center.Y;
            //dist = MathHelper.Clamp(dist, -Sound.max_vol_distance, Sound.max_vol_distance);
            pan.Y = dist / Sound.max_vol_distance;
            if (pan.Y > 0) pan.Y = 1f - pan.Y; else if (pan.Y < 0) pan.Y = -1f - pan.Y;
            if (pan.Y < 0) pan.Y = -pan.Y;
            
            if (pan.X <= -1f) pan.Y = 0;  // prune sounds that are very far off screen
            if (pan.X >= 1f)  pan.Y = 0;
            
            pan.X = MathHelper.Clamp(pan.X, -1.0f, 1.0f);
            pan.Y = MathHelper.Clamp(pan.Y, 0.0f, 1.0f);                
            return pan; 
        }
    }
}
