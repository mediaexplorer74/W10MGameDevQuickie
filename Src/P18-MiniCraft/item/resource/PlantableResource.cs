// Decompiled with JetBrains decompiler
// Type: GameManager.item.resource.PlantableResource
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.level;
using GameManager.level.tile;
using System.Collections.Generic;

#nullable disable
namespace GameManager.item.resource
{
  public class PlantableResource : Resource
  {
    private List<Tile> sourceTiles;
    private Tile targetTile;

    public PlantableResource(
      string name,
      int sprite,
      int color,
      Tile targetTile,
      Tile[] sourceTiles1)
      : this(name, sprite, color, targetTile, new List<Tile>((IEnumerable<Tile>) sourceTiles1))
    {
    }

    public PlantableResource(
      string name,
      int sprite,
      int color,
      Tile targetTile,
      List<Tile> sourceTiles)
      : base(name, sprite, color)
    {
      this.sourceTiles = sourceTiles;
      this.targetTile = targetTile;
    }

    public override bool interactOn(
      Tile tile,
      Level level,
      int xt,
      int yt,
      Player player,
      int attackDir)
    {
      if (!this.sourceTiles.Contains(tile))
        return false;
      level.SetTile(xt, yt, this.targetTile, 0);
      return true;
    }
  }
}
