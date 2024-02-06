using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FbonizziMonoGameGallery.ParticleGeneratorTimedEffect
{
    public partial class ParticleGeneratorTimedEffectWindow : Page
    {
        private readonly ParticleGeneratorTimedEffectGame _game;

        public ParticleGeneratorTimedEffectWindow()
        {
            InitializeComponent();

            _game = new ParticleGeneratorTimedEffectGame();

            //RnD
            //GameGrid.Children.Add(_game);
            
            _game.GameInitialized += _game_GameInitialized;
        }

        private void _game_GameInitialized(object sender, System.EventArgs e)
        {
            _game.NewParticleGenerator(Path.GetFullPath("SampleImages/particle.png"));

            GenerationIntervalMillisecondsSlider.Value = _game.ParticleGeneratorTimedEffect.GenerationInterval.TotalMilliseconds;
            DensitySlider.Value = _game.ParticleGenerator.Density;
            MinNumParticlesSlider.Value = _game.ParticleGenerator.MinNumParticles;
            MaxNumParticlesSlider.Value = _game.ParticleGenerator.MaxNumParticles;
            MinInitialSpeedSlider.Value = _game.ParticleGenerator.MinInitialSpeed;
            MaxInitialSpeedSlider.Value = _game.ParticleGenerator.MaxInitialSpeed;
            MinAccelerationSlider.Value = _game.ParticleGenerator.MinAcceleration;
            MaxAccelerationSlider.Value = _game.ParticleGenerator.MaxAcceleration;
            MinRotationSpeedSlider.Value = _game.ParticleGenerator.MinRotationSpeed;
            MinLifetimeMillisecondsSlider.Value = _game.ParticleGenerator.MinLifetime.TotalMilliseconds;
            MaxLifetimeMillisecondsSlider.Value = _game.ParticleGenerator.MaxLifetime.TotalMilliseconds;
            MinScaleSlider.Value = _game.ParticleGenerator.MinScale;
            MaxScaleSlider.Value = _game.ParticleGenerator.MaxScale;
            MinSpawnAngleSlider.Value = _game.ParticleGenerator.MinSpawnAngle;
            MaxSpawnAngleSlider.Value = _game.ParticleGenerator.MaxSpawnAngle;

            GenerationIntervalMillisecondsSlider.ValueChanged += (obj, args) => _game.ParticleGeneratorTimedEffect.GenerationInterval = TimeSpan.FromMilliseconds((int)args.NewValue);
            DensitySlider.ValueChanged += (obj, args) => _game.ParticleGenerator.Density = (int)args.NewValue;
            MinNumParticlesSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinNumParticles = (int)args.NewValue;
            MaxNumParticlesSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxNumParticles = (int)args.NewValue;
            MinInitialSpeedSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinInitialSpeed = (int)args.NewValue;
            MaxInitialSpeedSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxInitialSpeed = (int)args.NewValue;
            MinAccelerationSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinAcceleration = (float)args.NewValue;
            MaxAccelerationSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxAcceleration = (float)args.NewValue;
            MinRotationSpeedSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinRotationSpeed = (float)args.NewValue;
            MaxRotationSpeedSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxRotationSpeed = (float)args.NewValue;
            MinLifetimeMillisecondsSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinLifetime = TimeSpan.FromMilliseconds((int)args.NewValue);
            MaxLifetimeMillisecondsSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxLifetime = TimeSpan.FromMilliseconds((int)args.NewValue);
            MinScaleSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinScale = (float)args.NewValue;
            MaxScaleSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxScale = (float)args.NewValue;
            MinSpawnAngleSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MinSpawnAngle = (float)args.NewValue;
            MaxSpawnAngleSlider.ValueChanged += (obj, args) => _game.ParticleGenerator.MaxSpawnAngle = (float)args.NewValue;
        }

        //RnD
        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    _game.GameInitialized -= _game_GameInitialized;
        //    _game.Dispose();
        //    base.OnClosing(e);
        //}

        private void LoadParticleButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "PNG image|*.png",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = Path.GetFullPath(openFileDialog.FileName);
                _game.NewParticleGenerator(filePath);
            }
        }
    }
}
