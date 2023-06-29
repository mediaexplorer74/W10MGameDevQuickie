using Microsoft.Xna.Framework;

namespace Snake.Scenes.Level
{
    interface Theme
    {
        Color Background { get; }
        (Color Head, Color Tail) Snake { get; }
        Color Apple { get; }
        Color Particle(int index);
    }
}
