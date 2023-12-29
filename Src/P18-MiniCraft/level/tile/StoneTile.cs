// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.StoneTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;

#nullable disable
namespace GameManager.level.tile
{
  public class StoneTile : Tile
  {
    private int col;

    public StoneTile(int id)
      : base(id)
    {
      this.col = Color.Get(111, Level.DirtColor, 333, 555);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      screen.Render(x * 16, y * 16, 32, this.col, 0);
      screen.Render(x * 16 + 8, y * 16, 32, this.col, 0);
      screen.Render(x * 16, y * 16 + 8, 32, this.col, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, 32, this.col, 0);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => false;
  }
}
