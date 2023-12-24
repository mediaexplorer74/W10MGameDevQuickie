using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    class Input
    {
        public KeyboardState kb, okb;
        public bool shift_down, control_down, alt_down;
        public bool shift_press, control_press, alt_press;
        public bool old_shift_down, old_control_down, old_alt_down;

        public Input() {
            // May want to disable Windows Key in the future (or not) 
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Keypress(Keys k) { if (kb.IsKeyDown(k) && okb.IsKeyUp(k)) return true; else return false; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Keydown(Keys k) { if (kb.IsKeyDown(k)) return true; else return false; }



        public void Update()
        {
            old_alt_down = alt_down; old_shift_down = shift_down; old_control_down = control_down;
            okb = kb;
            kb = Keyboard.GetState();
            shift_down = false; shift_press = false;
            control_down = false; control_press = false;
            alt_down = false; alt_press = false;
            if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) shift_down = true;
            if (kb.IsKeyDown(Keys.LeftControl) || kb.IsKeyDown(Keys.RightControl)) control_down = true;
            if (kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) alt_down = true;
            if ((shift_down) && (!old_shift_down)) shift_press = true;
            if ((control_down) && (!old_control_down)) control_press = true;
            if ((alt_down) && (!old_alt_down)) alt_press = true;
        }
    }
}
