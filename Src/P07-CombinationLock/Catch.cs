using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CombinationLock
{
    public class Catch
    {
        public Sprite sprite;
        public int Position { get; private set; }
        private int? PreviousRotation { get; set; }
        private bool TouchingLeftSide { get; set; }
        private bool TouchingRightSide { get; set; }

        public Catch(Texture2D tex)
        {
            sprite = new Sprite(tex);
            Position = 0;
            PreviousRotation = null;
            TouchingLeftSide = true;
            TouchingRightSide = false;
        }

        public void UpdatePosition(int rotation)
        {
            Debug.WriteLine("Before code");
            Debug.WriteLine($"Position: {Position}");
            Debug.WriteLine($"Previous Rotation: {PreviousRotation}");
            Debug.WriteLine($"Touching Left Side: {TouchingLeftSide}");
            Debug.WriteLine($"Touching Right Side: {TouchingRightSide}");

            if (TouchingLeftSide)
            {
                if ((rotation > Position && (Position != 0 || rotation != 179)) ||
                    (rotation == 0 && Position == 179))
                {
                    Position = rotation;
                }
                else
                {
                    TouchingLeftSide = false;
                }
            }
            else if (TouchingRightSide)
            {
                if ((rotation < Position && (Position != 0 || rotation != 1)) ||
                    (rotation == 179 && Position == 0))
                {
                    Position = rotation;
                }
                else
                {
                    TouchingRightSide = false;
                }
            }

            if (PreviousRotation != null)
            {
                if (PreviousRotation < rotation ||
                (PreviousRotation == 179 && rotation == 0))
                {
                    if (rotation == Position)
                    {
                        TouchingLeftSide = true;
                    }
                }
                else if (PreviousRotation > rotation ||
                    (PreviousRotation == 0 && rotation == 179))
                {
                    if (rotation == Position)
                    {
                        TouchingRightSide = true;
                    }
                }
            }
            //sprite.rotation = Position * 2;

            PreviousRotation = rotation;

            Debug.WriteLine("After code");
            Debug.WriteLine($"Position: {Position}");
            Debug.WriteLine($"Previous Rotation: {PreviousRotation}");
            Debug.WriteLine($"Touching Left Side: {TouchingLeftSide}");
            Debug.WriteLine($"Touching Right Side: {TouchingRightSide}");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
