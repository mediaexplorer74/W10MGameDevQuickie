namespace FbonizziMonoGame.Assets
{
    /// <summary>
    /// DTO object to represent a sprite when parsed from a SpriteSheet description
    /// </summary>
    public class SpriteDescription
    {
        /// <summary>
        /// The sprite name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sprite X position in the spritesheet
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The sprite Y position in the spritesheet
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// The sprite width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The sprite height
        /// </summary>
        public int Height { get; set; }
    }
}
