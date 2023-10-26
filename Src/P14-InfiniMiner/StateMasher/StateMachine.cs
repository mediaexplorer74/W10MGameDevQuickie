using System;
using System.Collections.Generic;
using System.Runtime.InteropServices; 
using System.Reflection;
using Infiniminer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace StateMasher
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class StateMachine : Microsoft.Xna.Framework.Game
    {
        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow(); 

        public GraphicsDeviceManager graphicsDeviceManager;
        public Infiniminer.PropertyBag propertyBag = null;

        private string currentStateType = "";
        public string CurrentStateType
        {
            get { return currentStateType; }
        }

        private State currentState = null;
        private bool needToRenderOnEnter = false;

        private int frameCount = 0;
        private double frameRate = 0;
        public double FrameRate
        {
            get { return frameRate; }
        }

        private Dictionary<Keys, bool> keysDown = new Dictionary<Keys, bool>();
        private MouseState msOld;

        public StateMachine()
        {
            Content.RootDirectory = "Content";
            graphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected void ChangeState(string newState)
        {
            // Call OnLeave for the old state.
            if (currentState != null)
                currentState.OnLeave(newState);

            // Instantiate and set the new state.
            //RnD
            Assembly a = default;//Assembly.GetExecutingAssembly();
            Type t = a.GetType(newState);
            currentState = Activator.CreateInstance(t) as State;

            // Set up the new state.
            currentState._P = propertyBag;
            currentState._SM = this;
            currentState.OnEnter(currentStateType);
            currentStateType = newState;
            needToRenderOnEnter = true;
        }

        public bool WindowHasFocus()
        {
            return GetForegroundWindow() == (int)Window.Handle;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //RnD
            if (frameCount > 0)
            {
                frameRate = frameCount / 10;//gameTime.TotalRealTime.TotalSeconds;
            }

            if (currentState != null && propertyBag != null)
            {
                // Call OnUpdate.
                string newState = currentState.OnUpdate(gameTime, Keyboard.GetState(), Mouse.GetState());
                if (newState != null)
                    ChangeState(newState);

                // Check for keyboard events.
                KeyboardState keyState = Keyboard.GetState();
                Dictionary<Keys, bool> keysDownNow = new Dictionary<Keys, bool>();
                foreach (Keys k in keyState.GetPressedKeys())
                    keysDownNow.Add(k, true);
                if (WindowHasFocus())
                {
                    foreach (Keys k in keysDownNow.Keys)
                        if (!keysDown.ContainsKey(k))
                            currentState.OnKeyDown(k);
                    foreach (Keys k in keysDown.Keys)
                        if (!keysDownNow.ContainsKey(k))
                            currentState.OnKeyUp(k);
                }
                keysDown = keysDownNow;

                // Check for mouse events.
                MouseState msNew = Mouse.GetState();
                if (WindowHasFocus())
                {
                    if (msOld.LeftButton == ButtonState.Released && msNew.LeftButton == ButtonState.Pressed)
                        currentState.OnMouseDown(MouseButton.LeftButton, msNew.X, msNew.Y);
                    if (msOld.MiddleButton == ButtonState.Released && msNew.MiddleButton == ButtonState.Pressed)
                        currentState.OnMouseDown(MouseButton.MiddleButton, msNew.X, msNew.Y);
                    if (msOld.RightButton == ButtonState.Released && msNew.RightButton == ButtonState.Pressed)
                        currentState.OnMouseDown(MouseButton.RightButton, msNew.X, msNew.Y);
                    if (msOld.LeftButton == ButtonState.Pressed && msNew.LeftButton == ButtonState.Released)
                        currentState.OnMouseUp(MouseButton.LeftButton, msNew.X, msNew.Y);
                    if (msOld.MiddleButton == ButtonState.Pressed && msNew.MiddleButton == ButtonState.Released)
                        currentState.OnMouseUp(MouseButton.MiddleButton, msNew.X, msNew.Y);
                    if (msOld.RightButton == ButtonState.Pressed && msNew.RightButton == ButtonState.Released)
                        currentState.OnMouseUp(MouseButton.RightButton, msNew.X, msNew.Y);
                    if (msOld.ScrollWheelValue != msNew.ScrollWheelValue)
                        currentState.OnMouseScroll(msNew.ScrollWheelValue - msOld.ScrollWheelValue);
                }
                msOld = msNew;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Call OnRenderAtUpdate.
            if (currentState != null && propertyBag != null)
            {
                frameCount += 1;
                currentState.OnRenderAtUpdate(GraphicsDevice, gameTime);
            }

            // If we have one queued, call OnRenderAtEnter.
            if (currentState != null && needToRenderOnEnter && propertyBag != null)
            {
                needToRenderOnEnter = false;
                currentState.OnRenderAtEnter(GraphicsDevice);
            }
            
            base.Draw(gameTime);
        }
    }
}
