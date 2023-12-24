using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    // H E A D S  U P  D I S P L A Y  (lifebar, lives, score, etc)
    class Hud
    {
        SpriteBatch  spriteBatch;
        SpriteFont   font;
        Texture2D    hud_tex;
        Rectangle    lifebar_border, lifebar, life_word, mini_hero, life_rect;
        Vector2      lifebar_pos, lives_pos, life_word_pos, count_pos;
        public float life;        // amount of life in lifebar
        public int   lives;       // number of retries

        // CONSTRUCT
        public Hud(SpriteBatch spr, SpriteFont fnt)
        {
            spriteBatch    = spr;
            font           = fnt;
            lifebar_border = new Rectangle(0, 64, 284, 21);
            lifebar        = new Rectangle(0,0,284,21);
            life_word      = new Rectangle(0, 22, 60, 27);
            mini_hero      = new Rectangle(326, 14, 48, 37);
            life_word_pos  = new Vector2(10, 6);
            lifebar_pos    = new Vector2(70, 10);
            lives_pos      = new Vector2(360, 3);
            count_pos      = new Vector2(410, 10);
        }


        // L O A D (and INIT)
        public void Load(ContentManager Content)
        {
            hud_tex = Content.Load<Texture2D>("hud");
            life    = 1.0f;      // ranges 0-1 (like 0% to 100%)
            lives   = 2;
            life_rect = lifebar; // source of image changes length depending on life
        }


        // D R A W
        public void Draw()
        {
            spriteBatch.Draw(hud_tex, life_word_pos, life_word, Color.White);
            spriteBatch.Draw(hud_tex, lives_pos, mini_hero, Color.White);
            spriteBatch.DrawString(font, "X "+lives, count_pos, Color.LimeGreen);
            life_rect.Width = (int)((float)lifebar.Width * life);                  // calculate lifebar's source width based on life
            spriteBatch.Draw(hud_tex, lifebar_pos, life_rect, Color.White);
            spriteBatch.Draw(hud_tex, lifebar_pos, lifebar_border, Color.White);
        }
    }
}
