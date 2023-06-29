using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Sprites;

namespace FbonizziMonoGame.UI.RandomImageShower
{
    /// <summary>
    /// An image with text
    /// </summary>
    public class ImageWithText
    {
        /// <summary>
        /// The text to draw near the image
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The image
        /// </summary>
        public Sprite Image { get; set; }

        /// <summary>
        /// The text drawing infos
        /// </summary>
        public DrawingInfos TextDrawingInfos { get; set; }
    }
}
