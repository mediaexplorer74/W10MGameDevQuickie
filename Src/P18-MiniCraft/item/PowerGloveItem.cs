// Decompiled with JetBrains decompiler
// Type: GameManager.item.PowerGloveItem
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.item
{
  public class PowerGloveItem : Item
  {
    public PowerGloveItem()
    {
    }

    public PowerGloveItem(Game1 game, BinaryReader reader)
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

    public override int GetColor()
    {
        return gfx.Color.Get(-1, 100, 320, 430);
    }

    public override int GetSprite()
    {
        return 135;
    }

    public override void RenderIcon(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.GetSprite(), this.GetColor(), 0);
    }

    public override void RenderInventory(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.GetSprite(), this.GetColor(), 0);
      Font.Draw(this.GetName(), screen, x + 8, y, gfx.Color.Get(-1, 555, 555, 555));
    }

    public override string GetName() => "Pow glove";

    public override bool Interact(Player player, Entity entity, int attackDir)
    {
      if (!(entity is Furniture))
        return false;
      ((Furniture) entity).Take(player);
      return true;
    }
  }
}
