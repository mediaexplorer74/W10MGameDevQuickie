using System;
using System.ComponentModel;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FbonizziMonoGameGallery.Drawing
{
    public partial class DynamicScalingMatrixProviderWindow : Page
    {
        private readonly DynamicScalingMatrixProviderGame _game;

        public DynamicScalingMatrixProviderWindow()
        {
            InitializeComponent();

            _game = new DynamicScalingMatrixProviderGame(
                new WpfScreenSizeChangedNotifier(this),
                (float)Width,
                (float)Height);

            //RnD
            //GameGrid.Children.Add(_game);
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    _game.Dispose();
        //    base.OnClosing(e);
        //}

        private void ChbMantainProportions_Click(object sender, RoutedEventArgs e)
        {
            _game.MantainProportionsOnScalingMatrix = chbMantainProportions.IsChecked ?? false;
        }
    }
}
