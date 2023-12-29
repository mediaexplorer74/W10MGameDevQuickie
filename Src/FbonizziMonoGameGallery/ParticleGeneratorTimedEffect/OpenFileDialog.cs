using System;

namespace FbonizziMonoGameGallery.ParticleGeneratorTimedEffect
{
    internal class OpenFileDialog
    {
        internal string FileName;

        public OpenFileDialog()
        {
        }

        public string Filter { get; set; }
        public bool Multiselect { get; set; }

        internal bool ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}