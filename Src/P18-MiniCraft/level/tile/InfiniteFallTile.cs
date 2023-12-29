// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.InfiniteFallTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;

#nullable disable
namespace GameManager.level.tile
{
  public class InfiniteFallTile : Tile
  {
    public InfiniteFallTile(int id)
      : base(id)
    {
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
    }

    public override void Tick(Level level, int xt, int yt)
    {
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => e is AirWizard;
  }
}
