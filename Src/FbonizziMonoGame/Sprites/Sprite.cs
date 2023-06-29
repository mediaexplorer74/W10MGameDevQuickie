using FbonizziMonoGame.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FbonizziMonoGame.Sprites
{
    /// <summary>
    /// A sprite is an object that identifies a portion of an image
    /// in bigger image
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// A reference to the image in which the sprite is contained
        /// </summary>
        public Texture2D Sheet { get; }

        /// <summary>
        /// The rectangle that identifies this sprite position in the containing image
        /// </summary>
        public Rectangle SourceRectangle { get; }

        /// <summary>
        /// The center coordinates of this sprite
        /// </summary>
        public Vector2 SpriteCenter { get; }

        /// <summary>
        /// Sprite width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Sprite height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The Sprite costructor
        /// </summary>
        /// <param name="spriteDescription"></param>
        /// <param name="sheet">The image in which the Sprite is contained</param>
        public Sprite(
            SpriteDescription spriteDescription,
            Texture2D sheet)
        {
            Sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));

            SourceRectangle = new Rectangle(
                spriteDescription.X, spriteDescription.Y,
                spriteDescription.Width, spriteDescription.Height);

            Width = spriteDescription.Width;
            Height = spriteDescription.Height;

            SpriteCenter = new Vector2(
                SourceRectangle.Width / 2,
                SourceRectangle.Height / 2);
        }

        /// <summary>
        /// The Sprite costructor that creates a Sprite with the same size of the sheet used
        /// </summary>
        /// <param name="sheet">The image in which the Sprite is contained</param>
        public Sprite(Texture2D sheet)
            : this(new SpriteDescription()
            {
                X = 0,
                Y = 0,
                Width = sheet.Width,
                Height = sheet.Height
            },
            sheet)
        {

        }
        
    }
}
