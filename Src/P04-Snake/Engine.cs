using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Snake
{
    interface IScenes
    {
        Scenes.Scene SplashScreen { get; }
        Scenes.Scene TitleScreen { get; }
        Scenes.Scene Level { get; }
    }

    internal class Engine: Microsoft.Xna.Framework.Game, IScenes
    {
        GraphicsDeviceManager graphics;
        BasicEffect basicEffect;
        BufferedInput input;

        public Scenes.Scene SplashScreen { get; }
        public Scenes.Scene TitleScreen { get; }
        public Scenes.Scene Level { get; }

        public Engine()
        {
            SplashScreen = new Scenes.SplashScreen(this);
            TitleScreen = new Scenes.TitleScreen(this);
            Level = new Scenes.Level.LevelScene(this);
            input = new BufferedInput();

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Window.ClientBounds.Width,
                PreferredBackBufferHeight = Window.ClientBounds.Height,
                IsFullScreen = true
            };
            IsMouseVisible = false;
            Content.RootDirectory = "Content";
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            base.Initialize();
            basicEffect = new BasicEffect(graphics.GraphicsDevice);

            SplashScreen.BeginScene();
        }

        private Scenes.Scene[] allScenes;
        private IEnumerable<Scenes.Scene> AllScenes
        {
            get
            {
                if (allScenes == null)
                {
                    var properties = typeof(IScenes).GetTypeInfo().DeclaredProperties;
                    var values = properties.Select(p => p.GetValue(this));
                    allScenes = values.Cast<Scenes.Scene>().ToArray();
                }
                return allScenes;
            }
        }
        private IEnumerable<Scenes.Scene> ActiveScenes => AllScenes.Where(s => s.Active);

        protected override void LoadContent()
        {
            base.LoadContent();

            foreach(var scene in AllScenes)
            {
                scene.LoadContentForScene(Content);
            }
        }

        public Action<State> OnDraw { get; internal set; }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            if (graphics.PreferredBackBufferWidth != Window.ClientBounds.Width || graphics.PreferredBackBufferHeight != Window.ClientBounds.Height)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            input.Update();
            if(ActiveScenes.Count() == 0)
            {
                Exit();
            }
            foreach(var scenes in ActiveScenes)
            {
                scenes.UpdateScene(input, gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            var (width, height) = (graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            if (height < width)
            {
                basicEffect.Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateScale(new Vector3(1f * height / width, 1, 1));
            }
            else
            {
                basicEffect.Projection = Matrix.CreateScale(1, -1, 1) * Matrix.CreateScale(new Vector3(1, 1f * width / height, 1));
            }

            graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            basicEffect.CurrentTechnique.Passes.First().Apply();

            basicEffect.World = Matrix.Identity;
            basicEffect.CurrentTechnique.Passes.First().Apply();

            foreach (var scene in ActiveScenes)
            {
                scene.DrawScene(this);
            }
        }

        public void ClearScreen(Color color)
        {
            graphics.GraphicsDevice.Clear(color);
        }

        public void ConfigureEffect(Action<BasicEffect> configurationCallback)
        {
            configurationCallback(basicEffect);
            basicEffect.CurrentTechnique.Passes.First().Apply();
        }

        public void DrawSquare(float x, float y, float scale, Color color, float factor, TimeSpan runtime)
        {
            scale *= 1 + factor * (float)Math.Sin(runtime.TotalSeconds * 2.4f + x + y);
            var positions = new Vector3[]
            {
                new Vector3(x - scale, y - scale, 0),
                new Vector3(x - scale, y + scale, 0),
                new Vector3(x + scale, y - scale, 0),
                new Vector3(x + scale, y + scale, 0)
            }
            .ToArray();
            var vertices = new VertexPositionColorTexture[]
            {
                new VertexPositionColorTexture(positions[0], color, Vector2.Zero),
                new VertexPositionColorTexture(positions[1], color, Vector2.UnitX),
                new VertexPositionColorTexture(positions[2], color, Vector2.UnitY),
                new VertexPositionColorTexture(positions[3], color, Vector2.One)
            };
            graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
        }



        public void DrawAnimatedMessage(TimeSpan runtime, PixelFont pixelFont, string message, float seconds, float verticalShift, float scale, Alignment alignment)
        {
            var i = 0;
            foreach(var line in message.Split('\n'))
            {
                DrawAnimatedMessageLine(runtime, pixelFont, line, seconds, verticalShift + scale + (i++) * scale * (pixelFont.Height + 1), scale, alignment);
            }
        }

        private void DrawAnimatedMessageLine(TimeSpan runtime, PixelFont pixelFont, string message, float seconds, float verticalShift, float scale, Alignment alignment)
        {
            // zoom out a tiny bit to have some border...
            var _paddingScale = 0.975f;
            var aspectRatio = ((float)Window.ClientBounds.Width) / Window.ClientBounds.Height;
            var screenshift = Math.Max(aspectRatio, 1);
            var textshift = -(float)((pixelFont.Width + 1) * message.Length - 1);
            var yshift = 0f;

            // horizontal
            if (alignment == Alignment.TopLeft)
            {
                screenshift = -screenshift;
                textshift = 0;
            }
            if (alignment == Alignment.TopRight || alignment == Alignment.BottomRight)
            {
                // no changes needed
            }
            if (alignment == Alignment.Centered)
            {
                screenshift = 0;
                textshift = textshift / 2;
            }

            // vertical
            if (alignment == Alignment.TopLeft || alignment == Alignment.TopRight)
            {
                yshift = aspectRatio < 1 ? 1 / aspectRatio - 1 : 0;
            }
            if(alignment == Alignment.Centered)
            {
                // no changes needed
            }
            if(alignment == Alignment.BottomRight)
            {
                yshift = aspectRatio < 1 ? 1 - 1 / aspectRatio : 0;
            }

            ConfigureEffect(e =>
            {
                e.View = Matrix.CreateScale(_paddingScale);
                e.World =
                    Matrix.CreateTranslation(textshift + .5f, -pixelFont.Height / 2f + .5f, 0)
                    * Matrix.CreateScale(scale)
                    * Matrix.CreateTranslation(screenshift, verticalShift - yshift, 0);
            });

            pixelFont.DrawString(message, (x, y) =>
            {
                var dx = x / (float)message.Length;
                var fade = Math.Max(0, Math.Min(1, (seconds - .5f) - dx / 3));
                var color = Grey((float)Math.Sqrt(fade));
                DrawSquare(x, y, 1f, color, 0.1f + (float)Math.Pow(1.1f * Math.Abs(1 - fade), 20), runtime);
            });
        }

        private Color Grey(float brightness) => new Color(brightness, brightness, brightness, brightness);
    }
}