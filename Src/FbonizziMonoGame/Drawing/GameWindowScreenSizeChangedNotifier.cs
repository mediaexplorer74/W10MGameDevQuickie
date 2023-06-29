using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using System;

namespace FbonizziMonoGame.Drawing
{
    /// <summary>
    /// MonoGame GameWindow implementation
    /// </summary>
    public class GameWindowScreenSizeChangedNotifier : IScreenSizeChangedNotifier
    {
        /// <summary>
        /// Raises when screen size changes
        /// </summary>
        public event EventHandler OnScreenSizeChanged;

        /// <summary>
        /// MonoGame GameWindow implementation of <see cref="IScreenSizeChangedNotifier"/>
        /// </summary>
        /// <param name="gameWindow"></param>
        public GameWindowScreenSizeChangedNotifier(GameWindow gameWindow)
        {
            if (gameWindow == null)
                throw new ArgumentNullException(nameof(gameWindow));

            gameWindow.ClientSizeChanged += gameWindow_ClientSizeChanged;
        }

        private void gameWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            OnScreenSizeChanged?.Invoke(sender, e);
        }
    }
}
