using Microsoft.Xna.Framework.Graphics;
using System;

namespace Infiniminer
{
    internal class ResolveTexture2D
    {
        private GraphicsDevice graphicsDevice;
        private int width;
        private int height;
        private int v;
        private SurfaceFormat format;

        public ResolveTexture2D(GraphicsDevice graphicsDevice, int width, int height, int v, SurfaceFormat format)
        {
            this.graphicsDevice = graphicsDevice;
            this.width = width;
            this.height = height;
            this.v = v;
            this.format = format;
        }

        internal void Dispose()
        {
            //TODO
        }
    }
}