// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Lantern


using GameManager.gfx;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Lantern : Furniture
  {
    public Lantern()
      : base(nameof (Lantern))
    {
      this.Col = gfx.Color.Get(-1, 0, 111, 555);
      this.Sprite = 5;
      this.Xr = 3;
      this.Yr = 2;
    }

    public Lantern(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
    }

    public override int GetLightRadius() => 8;
  }
}
