using System.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FbonizziMonoGameGallery
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ParticleGeneratorTimeEffectButton_Click(object sender, RoutedEventArgs e)
        {
            ParticleGeneratorTimedEffect.ParticleGeneratorTimedEffectWindow gallery
                = new ParticleGeneratorTimedEffect.ParticleGeneratorTimedEffectWindow();

            //RnD
            //gallery.ShowDialog();
        }

        private void DynamicScalingMatrixProviderButton_Click(object sender, RoutedEventArgs e)
        {
            Drawing.DynamicScalingMatrixProviderWindow gallery 
                = new Drawing.DynamicScalingMatrixProviderWindow();

            //RnD
            //gallery.ShowDialog();
        }
    }
}

