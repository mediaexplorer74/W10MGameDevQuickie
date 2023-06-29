using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Snake.Scenes
{
    internal class TitleScreen : Scene
    {
        public TitleScreen(IScenes scenes)
        {
            Texture2D glowingTile = null;
            PixelFont pixelFont = null;
            int selectionIndex = 0;
            int selectionCount = 3;
            BackgroundParticles backgroundParticles = new BackgroundParticles();

            LoadContent = content =>
            {
                glowingTile = content.Load<Texture2D>("glowing Tile");
                pixelFont = new PixelFont();
            };

            Update = (input, delta) =>
            {
                backgroundParticles.Update(TimeSpan.FromTicks(delta.Ticks / 2));

                var keyboard = input.Keyboard;
                var gamePad = input.GamePad;
                var touchPanel = input.TouchPanel;

                var select = false;

                if (keyboard.WhenDown(Keys.Down) || gamePad.WhenButtonDown(Buttons.DPadDown))
                {
                    selectionIndex = Math.Min(selectionIndex + 1, selectionCount - 1);
                }
                else if (keyboard.WhenDown(Keys.Up) || gamePad.WhenButtonDown(Buttons.DPadUp))
                {
                    selectionIndex = Math.Max(selectionIndex - 1, 0);
                }
                else if (keyboard.WhenDown(Keys.Enter) || gamePad.WhenButtonDown(Buttons.X))
                {
                    select = true;
                }

                if (touchPanel.WhenTouching)
                {
                    if (touchPanel.RelativePosition.Y > 0.5f + 0.5f * (1f / 4f))
                    {
                        selectionIndex = 0;
                        select = true;
                    }
                    if (touchPanel.RelativePosition.Y > 0.5f + 0.5f * (2f / 4f))
                    {
                        selectionIndex = 1;
                    }
                    if(touchPanel.RelativePosition.Y > 0.5f + 0.5f * (3f / 4f))
                    {
                        selectionIndex = 2;
                    }
                }

                if(select)
                {
                    switch (selectionIndex)
                    {
                        case 0:
                            scenes.Level.BeginScene(false);
                            this.PauseScene();
                            break;

                        case 1:
                            scenes.Level.BeginScene(true);
                            this.PauseScene();
                            break;

                        case 2:
                            this.EndScene();
                            break;
                    }
                }
            };

            Draw = engine =>
            {
                engine.ConfigureEffect(e =>
                {
                    e.Texture = glowingTile;
                });

                backgroundParticles.Each(p =>
                {
                    engine.DrawSquare(p.position.X, p.position.Y, p.age * .1f, p.color * .1f, 0.2f, Runtime);
                });

                var seconds = (float)Runtime.TotalSeconds;
                var heights = new float[]
                {
                    MathHelper.SmoothStep(0, -.5f, (seconds - 2.5f) * 1f),
                    MathHelper.SmoothStep(0, -.5f, (seconds - 2.5f) * .8f),
                    MathHelper.SmoothStep(0, -.5f, (seconds - 2.5f) * .6f),
                    MathHelper.SmoothStep(0, -.5f, (seconds - 2.5f) * .4f),
                };

                string getSelectionText(int index)
                {
                    var selections = new string[]
                    {
                        "1 PLAYER",
                        "2 PLAYERS",
                        "EXIT"
                    };
                    if (index == selectionIndex)
                    {
                        return $">> {selections[index]}";
                    }
                    else
                    {
                        return selections[index];
                    }
                }

                engine.DrawAnimatedMessage(Runtime, pixelFont, "SNAKE", seconds, heights[3] - .2f, 1f / 12f, Alignment.TopLeft);

                engine.DrawAnimatedMessage(Runtime, pixelFont, getSelectionText(0), seconds - 1.5f, heights[2] + .9f, 1f / 48f, Alignment.BottomRight);
                engine.DrawAnimatedMessage(Runtime, pixelFont, getSelectionText(1), seconds - 1.7f, heights[1] + 1.15f, 1f / 48f, Alignment.BottomRight);
                engine.DrawAnimatedMessage(Runtime, pixelFont, getSelectionText(2), seconds - 1.9f, heights[0] + 1.4f, 1f / 48f, Alignment.BottomRight);
            };
        }
    }
}