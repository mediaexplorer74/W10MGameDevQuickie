using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// Extensions to SpriteBatch to draw <see cref="Sprite"/>
    /// </summary>
    public static class SpriteBatchExtensions
    {
        private static readonly Color _hitBoxColor = Color.Red.WithAlpha(0.5f);
        private static Texture2D _rectangleTexture;

        /// <summary>
        /// It draws a rectangle shape filled with a color
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="rectangleDefinition"></param>
        /// <param name="fillColor"></param>
        public static void DrawRectangle(
           this SpriteBatch spriteBatch,
           Rectangle rectangleDefinition,
           Color fillColor)
        {
            if (_rectangleTexture == null)
            {
                _rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _rectangleTexture.SetData(new Color[] { Color.White });
            }

            spriteBatch.Draw(
                _rectangleTexture,
                rectangleDefinition,
                fillColor);
        }

        /// <summary>
        /// It draws a rectangle shape filled with a color and a layer depth
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="rectangleDefinition"></param>
        /// <param name="fillColor"></param>
        /// <param name="origin"></param>
        /// <param name="layerDepth"></param>
        public static void DrawRectangle(
           this SpriteBatch spriteBatch,
           Rectangle rectangleDefinition,
           Color fillColor,
           Vector2 origin,
           float layerDepth)
        {
            if (_rectangleTexture == null)
            {
                _rectangleTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _rectangleTexture.SetData(new Color[] { Color.White });
            }

            spriteBatch.Draw(
                texture: _rectangleTexture,
                destinationRectangle: rectangleDefinition,
                sourceRectangle: null,
                color: fillColor,
                rotation: 0f,
                origin: origin,
                effects: SpriteEffects.None,
                layerDepth: layerDepth);
        }

        /// <summary>
        /// It draws a <see cref="Sprite"/> in a given position
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="sprite"></param>
        /// <param name="spatialObject"></param>
        /// <param name="isDebugModeEnabled">It draws a visible boxing rectangle</param>
        public static void Draw(
            this SpriteBatch spriteBatch,
            Sprite sprite,
            DrawingInfos spatialObject,
            bool isDebugModeEnabled = false)
        {
            spriteBatch.Draw(
                sprite.Sheet,
                spatialObject.Position,
                sprite.SourceRectangle,
                spatialObject.OverlayColor,
                spatialObject.Rotation,
                spatialObject.Origin,
                spatialObject.Scale,
                spatialObject.IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                spatialObject.LayerDepth);

            if (isDebugModeEnabled)
            {
                DrawRectangle(
                    spriteBatch,
                    spatialObject.HitBox(sprite.Width, sprite.Height),
                    _hitBoxColor);
            }
        }

        /// <summary>
        /// It draws a <see cref="Sprite"/> in Vector2.Zero position
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        public static void Draw(
            this SpriteBatch spriteBatch,
            Sprite sprite,
            Color? color = null)
        {
            spriteBatch.Draw(
                sprite.Sheet,
                Vector2.Zero,
                sprite.SourceRectangle,
                color ?? Color.White);
        }

        /// <summary>
        /// It draws a <see cref="Texture2D"/> in a given position
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="spatialObject"></param>
        /// <param name="isDebugModeEnabled">It draws a visible boxing rectangle</param>
        public static void Draw(
            this SpriteBatch spriteBatch,
            Texture2D texture,
            DrawingInfos spatialObject,
            bool isDebugModeEnabled = false)
        {
            spriteBatch.Draw(
                texture,
                spatialObject.Position,
                null,
                spatialObject.OverlayColor,
                spatialObject.Rotation,
                spatialObject.Origin,
                spatialObject.Scale,
                spatialObject.IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                spatialObject.LayerDepth);

            if (isDebugModeEnabled)
            {
                DrawRectangle(
                    spriteBatch,
                    spatialObject.HitBox(texture.Width, texture.Height),
                    _hitBoxColor);
            }
        }

        /// <summary>
        /// It draws a <see cref="Texture2D"/> in Vector2.Zero position
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        public static void Draw(
            this SpriteBatch spriteBatch,
            Texture2D texture,
            Color? color = null)
        {
            spriteBatch.Draw(
                texture,
                Vector2.Zero,
                null,
                color ?? Color.White);
        }

        /// <summary>
        /// It draws a string in a given position
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="spatialInfos"></param>
        public static void DrawString(
            this SpriteBatch spriteBatch,
            SpriteFont font,
            string text,
            DrawingInfos spatialInfos)
        {
            spriteBatch.DrawString(
                font,
                text,
                spatialInfos.Position,
                spatialInfos.OverlayColor,
                spatialInfos.Rotation,
                spatialInfos.Origin,
                spatialInfos.Scale,
                spatialInfos.IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                spatialInfos.LayerDepth);
        }
    }
}
