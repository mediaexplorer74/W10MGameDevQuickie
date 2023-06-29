using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Snake.Scenes.Level
{
    internal class LevelScene : Scene
    {
        double shake;
        private Random random = new Random();

        public LevelScene(IScenes scenes)
        {
            ForegroundParticles foregroundParticles = default;
            BackgroundParticles backgroundParticles = default;
            Game game = default;
            State state = default;
            Themes themes = default;
            Theme last = default;
            float themefade = 0;

            Texture2D glowingTile = default;
            PixelFont pixelFont = default;
            SoundEffect eatingApple = default;
            SoundEffect dyingSnake = default;
            SoundEffect newHighscore = default;

            TimeSpan bufferToNextTick = default;
            TimeSpan tickRate = default;

            Input bufferedInput = default;

            bool twoPlayerMode = false;
            bool playerTwo = false;

            var field = new (Color color, float random)[16, 16];

            Begin = () =>
            {
                if((bool)this.Parameters == true)
                {
                    twoPlayerMode = true;
                }
                foregroundParticles = new ForegroundParticles();
                backgroundParticles = new BackgroundParticles();

                game = new Game(16);
                state = game.Init(PersistentState.Highscore);
                themes = new Themes(levelsPerTheme: 10);

                for (var y = 0; y < 16; y++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var color = Color.Black;
                        color.G = (byte)(255 * (float)random.NextDouble() * .03f);
                        color.R = (byte)(color.G * (float)random.NextDouble());
                        color.B = (byte)(color.R * (float)random.NextDouble());

                        field[x, y] = (color, (float)random.NextDouble());
                    }
                }

                bufferedInput = Input.None;
                bufferToNextTick = TimeSpan.Zero;
                tickRate = TimeSpan.FromMilliseconds(1000 / 5/*15*/);

                game.OnMoment = moment =>
                {
                    if(moment == Moment.EatingApple)
                    {
                        if (twoPlayerMode == true)
                        {
                            playerTwo = !playerTwo;
                        }
                        eatingApple.Play();
                        foregroundParticles.AddEatenApple(new Vector2(state.apple.position.X, 
                            state.apple.position.Y));

                        last = themes[state.score - 1];
                    }
                    if(moment == Moment.Dying)
                    {
                        if(twoPlayerMode == true)
                        {
                            playerTwo = !playerTwo;
                        }
                        dyingSnake.Play();
                        shake = 1f;
                        foregroundParticles.AddDyingSnake(state.snake.links.Select(l => new Vector2(l.X, l.Y)));
                    }
                    if(moment == Moment.NewHighscore)
                    {
                        newHighscore.Play();
                        foregroundParticles.AddSparklesToSnake(state.snake.links.Select(l => new Vector2(l.X, l.Y)));
                    }
                    if (moment == Moment.Moving)
                    {
                        foregroundParticles.AddSparklesToMovement(state.snake.links.Select(l => new Vector2(l.X, l.Y)));
                    }
                };
            };

            LoadContent = content =>
            {
                glowingTile = content.Load<Texture2D>("glowing Tile.png");
                eatingApple = content.Load<SoundEffect>("eating Apple.wav");
                dyingSnake = content.Load<SoundEffect>("dying Snake.wav");
                newHighscore = content.Load<SoundEffect>("new Highscore.wav");

                pixelFont = new PixelFont();
            };

            Update = (input, delta) =>
            {
                var keyboard = input.Keyboard;
                var touchPanel = input.TouchPanel;
                var gamePad = input.GamePad;

                if (last != null)
                {
                    themefade += (float)delta.TotalSeconds;
                    if(themefade >= 1f)
                    {
                        last = null;
                        themefade = 0f;
                    }
                }

                if (keyboard.WhenDown(Keys.Escape))
                {
                    this.EndScene();
                    scenes.TitleScreen.ContinueScene();
                }
                var gameInput = bufferedInput;
                if(twoPlayerMode == false || !playerTwo)
                {
                    gameInput = keyboard.WhenDown(Keys.Left) ? Input.Left : gameInput;
                    gameInput = keyboard.WhenDown(Keys.Right) ? Input.Right : gameInput;
                    gameInput = keyboard.WhenDown(Keys.Up) ? Input.Up : gameInput;
                    gameInput = keyboard.WhenDown(Keys.Down) ? Input.Down : gameInput;

                    gameInput = keyboard.WhenDown(Keys.LeftControl) ? Input.TurnLeft : gameInput;
                    gameInput = keyboard.WhenDown(Keys.RightControl) ? Input.TurnRight : gameInput;
                }
                if (twoPlayerMode == false || playerTwo)
                {
                    gameInput = keyboard.WhenDown(Keys.A) ? Input.Left : gameInput;
                    gameInput = keyboard.WhenDown(Keys.D) ? Input.Right : gameInput;
                    gameInput = keyboard.WhenDown(Keys.W) ? Input.Up : gameInput;
                    gameInput = keyboard.WhenDown(Keys.S) ? Input.Down : gameInput;

                    gameInput = gamePad.WhenButtonDown(Buttons.DPadLeft) ? Input.Left : gameInput;
                    gameInput = gamePad.WhenButtonDown(Buttons.DPadRight) ? Input.Right : gameInput;
                    gameInput = gamePad.WhenButtonDown(Buttons.DPadUp) ? Input.Up : gameInput;
                    gameInput = gamePad.WhenButtonDown(Buttons.DPadDown) ? Input.Down : gameInput;
                }

                if(touchPanel.WhenTouching)
                {
                    gameInput = touchPanel.RelativePosition.X < 0.5f ? Input.TurnLeft : Input.TurnRight;
                }

                bufferToNextTick += delta;
                if (bufferToNextTick > tickRate)
                {
                    bufferToNextTick -= tickRate;
                    state = game.Update(state, gameInput);
                    gameInput = Input.None;

                    PersistentState.Highscore = state.highscore;
                }

                bufferedInput = gameInput;

                foregroundParticles.Update(delta);
                backgroundParticles.Update(delta);

                shake /= 2;
                if (shake < 0.001f)
                {
                    shake = 0;
                }
            };

            Draw = engine =>
            {
                var theme = themes[state.score];

                var backgroundcolor = last != null ? Color.Lerp(last.Background, theme.Background, themefade) : theme.Background;

                engine.ClearScreen(backgroundcolor);

                var _shake = Math.Pow(shake, 0.125f);

                engine.ConfigureEffect(e =>
                {
                    e.View = Matrix.CreateTranslation(
                        0.02f * (float)(_shake * Math.Sin(Runtime.TotalSeconds * 74.1)),
                        0.02f * (float)(_shake * Math.Sin((Runtime.TotalSeconds + 23532) * 47.2)),
                        0
                    );
                });

                engine.ConfigureEffect(e =>
                {
                    e.Texture = glowingTile;
                    e.World = Matrix.Identity;
                });

                backgroundParticles.Each(p =>
                {
                    var color = theme.Particle(p.id);
                    engine.DrawSquare(p.position.X, p.position.Y, p.age * .1f, color, 0.2f, Runtime);
                });
                //engine.DrawSquare(graphics.GraphicsDevice, 0, 0, 2, Color.Black, 0, Runtime);

                engine.ConfigureEffect(e =>
                {
                    e.World = Matrix.CreateTranslation(-7.5f, -7.5f, 0) * Matrix.CreateScale(1f / 8);
                });

                for (var y = 0; y < 16; y++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var scale = (1 - 4 * Math.Max(0, (float)shake - .3f * field[x, y].random)) * Math.Min(1.01f, Math.Max(0f, (float)(Runtime.TotalSeconds - field[x, y].random)));
                        engine.DrawSquare(x, y, scale, field[x, y].color, 0.01f, Runtime);
                    }
                }

                var players = new[]{
                    (head: theme.Snake.Head, tail: theme.Snake.Tail),
                    (head: Color.LightYellow, tail: Color.DarkKhaki)
                };
                var player = playerTwo ? players[1] : players[0];

                for (var y = 0; y < 16; y++)
                {
                    for (var x = 0; x < 16; x++)
                    {
                        var position = new System.Numerics.Vector2(x, y);
                        var tail = state.snake.links?.Any(link => link == position) ?? false;
                        var head = state.snake.position == position;
                        var apple = state.apple.position == position;
                        if (apple)
                        {
                            engine.DrawSquare(x, y, 15f / 16f, theme.Apple, 0.15f, Runtime);
                        }
                        else if (head)
                        {
                            var color = player.head;
                            engine.DrawSquare(x, y, 15f / 16f, color, 0.1f, Runtime);
                        }
                        else if (tail)
                        {
                            var color = player.tail;
                            foreach (var (link, index) in state.snake.links.Select((link, index) => ( link,  index)).Where(value => value.link == position))
                            {
                                var scale = 1f - 1f * index / state.snake.length;
                                engine.DrawSquare(x, y, 1f - 1f / 4f * scale, color, 0.03f, Runtime);
                            }
                        }
                    }
                }

                foregroundParticles.Each(p =>
                {
                    var color = theme.Particle(p.id);
                    engine.DrawSquare(p.position.X, p.position.Y, p.age * .3f, color, 0.1f, Runtime);
                });

                engine.DrawAnimatedMessage(Runtime, pixelFont, $"Highscore\n{state.score}\n{state.highscore}", (float)Runtime.TotalSeconds - 1, -1, 1f / 96f, Alignment.TopRight);
            };
        }
    }
}