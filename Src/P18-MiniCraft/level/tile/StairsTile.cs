// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.StairsTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.gfx;

#nullable disable
namespace GameManager.level.tile
{
  public class StairsTile : Tile
  {
    private bool leadsUp;
    private int color;

    public StairsTile(int id, bool leadsUp)
      : base(id)
    {
      this.leadsUp = leadsUp;
      this.color = Color.Get(Level.DirtColor, 0, 333, 444);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      int num = 0;
      if (this.leadsUp)
        num = 2;
      screen.Render(x * 16, y * 16, num + 64, this.color, 0);
      screen.Render(x * 16 + 8, y * 16, num + 1 + 64, this.color, 0);
      screen.Render(x * 16, y * 16 + 8, num + 96, this.color, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, num + 1 + 96, this.color, 0);
    }
  }
}
