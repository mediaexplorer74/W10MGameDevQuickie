using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Diagnostics;
using System.Linq;

namespace NeonShooter
{
    static class Input
    {
        // track the current and previous state of the inputs
        // previous is needed to know when buttons were pressed
        private static KeyboardState keyboardState;
        private static KeyboardState lastKeyboardState;
        
        private static GamePadState gamepadState;
        private static GamePadState lastGamepadState;

        private static /*MouseState*/TouchCollection mouseState;
        private static /*MouseState*/TouchCollection lastMouseState;

        private static bool isMouseStateOk = true;
        private static bool isLastMouseStateOk = true;


        // track whether or not the user is using the mouse or not
        // since we want the mouse icon to disappear when its not being used
        // not moved since the last update
        public static bool isAimingWithMouse = false;
        private static bool isAimingWithKeyboard = false;
        private static bool isAimingWithGamepad = false;

        // these are the keyboard controls for aiming, we need this list to track whether
        // or not the user is using them to aim or not
        private static Keys[] keyboardAimKeys = new Keys[4] { Keys.Up, Keys.Down, Keys.Left, Keys.Right };

        // some storage for directions of movement and aiming
        private static Vector2 moveDirection;
        private static Vector2 aimDirection;

        /// <summary>
        /// property for getting the current mouse position
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                float x = 0f;
                float y = 0f;

                try
                {
                    if (isMouseStateOk)//if (mouseState.IsConnected)
                    {
                        x = mouseState[0].Position.X;
                        y = mouseState[0].Position.Y;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ex] isMouseStateOk? Result: " + ex.Message);
                    isMouseStateOk = false;
                }                

                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// function to run on each update cycle in GameRoot to retrieve the input from 
        /// the user and move the current state info to the laststate property 
        /// finally we also decide whether or not the user is using the mouse or not
        /// so that we know whether or not to display the mouse icon
        /// </summary>
        public static void Update()
        {
            lastKeyboardState = keyboardState;
            lastGamepadState = gamepadState;
            lastMouseState = mouseState;

            keyboardState = Keyboard.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
            mouseState = TouchPanel.GetState();//Mouse.GetState();

            // if the right thumbstick has a value other than zero then it means
            // the player is pushing it. So set IsAimingWithGamepad to true
            if (gamepadState.ThumbSticks.Right == Vector2.Zero)
            {
                isAimingWithGamepad = false;
            }
            else
            {
                isAimingWithGamepad = true;
            }

            // if any of the keyboardAimKeys are down set isAimingWithKeyboard to true
            if (keyboardAimKeys.Any(key => keyboardState.IsKeyDown(key)))
            {
                isAimingWithKeyboard = true;
            }
            else
            {
                isAimingWithKeyboard = false;
            }

            // if player has touched any of the keyboard or gamepad aim controls then
            // he is not aiming with the mouse to set aiming with mouse to false
            if (isAimingWithKeyboard == true || isAimingWithGamepad == true)
            {
                isAimingWithMouse = false;
            }
            // if the player hasn't touched the keyboard or gamepad aim controls
            // and the mouse has moved since the last update then he must be using the 
            // the mouse to aim to set isAimingWithMouse to true
            else
            {
                float x = 0f;
                float y = 0f;

                try
                {
                    if(isLastMouseStateOk) //if (lastMouseState.IsConnected)
                    {
                        x = lastMouseState[0].Position.X;
                        y = lastMouseState[0].Position.Y;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ex] isLastMouseStateOk? Result: " + ex.Message);
                    isLastMouseStateOk = false;
                }


                if (MousePosition != new Vector2(x, y))
                {
                    isAimingWithMouse = true;
                }
            }
        }

        /// <summary>
        /// a method for checking if a key was pressed since the last update a keypress
        /// is when a key was pressed and then let go
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool WasKeyPressed(Keys key)
        {
            return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// a method for checking if a button was pressed and then let go since the last
        /// update cycle.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool WasButtonPressed(Buttons button)
        {
            return gamepadState.IsButtonUp(button) && lastGamepadState.IsButtonDown(button);
        }

        public static Vector2 GetMovementDirection()
        {
            // the xbox thumbstick gives a positive y value on the vector it returns when the
            // player pushes it up. So we need to invert it because the monogame 0 coordinate
            // is the top left corner of the monogame screen
            moveDirection = gamepadState.ThumbSticks.Left; // returns a vector 2 of -1 to 1 zero being the centre and -1, -1 being down/left 
            moveDirection.Y *= -1; // the xbox thumbstick gives a 

            // next we redefine the direction if the player is using the keyboard instead of 
            // a controller
            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveDirection.X = -1;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveDirection.X = 1;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                moveDirection.Y = -1;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                moveDirection.Y = 1;
            }

            // the direction will be used later to update the position of the playership
            // in order to keep the speed consistent between all 8 directions we need to
            // normalize the direction value (bring it to a value of 1 if its more than 1 or less than -1 (thats why we square the length)
            // if the direction value is 1.4 say then it means the ship would move in diagonals
            // faster than moving left right and up and down (x = 1, y = 0) 
            if (moveDirection.LengthSquared() > 1)
            {
                moveDirection.Normalize();
            }

            return moveDirection;
        }

        public static Vector2 GetAimDirection()
        {
            if (isAimingWithMouse)
            {
                return GetMouseAimDirection();
            }

            // invert the input of the right thumbstick so that up is up in the game.
            // thumbsticks return positive values when pushed upward but the zero 
            // coordinate is the top left.
            aimDirection = gamepadState.ThumbSticks.Right;
            aimDirection.Y *= -1;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                aimDirection.X -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                aimDirection.X += 1;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                aimDirection.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                aimDirection.Y += 1;
            }

            // if no aim input is given then just return vector 0 otherwise normalize it
            // the speed of the bullets are based on this number so it also needs to be normalized
            if (aimDirection == Vector2.Zero)
            {
                return Vector2.Zero;
            }
            else
            {
                aimDirection.Normalize();
                return aimDirection;
            }
        }

        private static Vector2 GetMouseAimDirection()
        {
            aimDirection = MousePosition - PlayerShip.Instance.Position;

            // if the delta between the mouse position and the player ships position is
            // the same then the direction is nothing thus vector2.zero otherwise normalize
            // the direction and return the value
            if (aimDirection == Vector2.Zero)
            {
                return Vector2.Zero;
            }
            else
            {
                // normalize the direction if it is more than 0
                aimDirection.Normalize();
                return aimDirection;
            }
        }

        public static bool WasBombButtonPressed()
        {
            return WasButtonPressed(Buttons.LeftTrigger) 
                || WasButtonPressed(Buttons.RightTrigger) 
                || WasKeyPressed(Keys.Space) || (mouseState.Count > 0);
        }
    }
}