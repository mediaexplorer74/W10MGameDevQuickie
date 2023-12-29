using FbonizziMonoGame.Drawing.Abstractions;
using System;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FbonizziMonoGameGallery.Drawing
{
    public class WpfScreenSizeChangedNotifier : IScreenSizeChangedNotifier
    {
        public event EventHandler OnScreenSizeChanged;

        public WpfScreenSizeChangedNotifier(Page gameWindow)
        {
            if (gameWindow == null)
                throw new ArgumentNullException(nameof(gameWindow));

            //RnD
            gameWindow.SizeChanged += gameWindow_SizeChanged;
        }

        private void gameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //RnD
            OnScreenSizeChanged?.Invoke(sender, default); // (EventArgs)e
        }
    }
}
