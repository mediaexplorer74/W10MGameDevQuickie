// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.Tile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;
using System;

#nullable disable
namespace GameManager.level.tile
{
  public class Tile
  {
    public readonly byte Id;
    public static int TickCount = 0;
    public static Tile[] Tiles = new Tile[256];
    public static Tile Grass = (Tile) new GrassTile(0);
    public static Tile Rock = (Tile) new RockTile(1);
    public static Tile Water = (Tile) new WaterTile(2);
    public static Tile Flower = (Tile) new FlowerTile(3);
    public static Tile Tree = (Tile) new TreeTile(4);
    public static Tile Dirt = (Tile) new DirtTile(5);
    public static Tile Sand = (Tile) new SandTile(6);
    public static Tile Cactus = (Tile) new CactusTile(7);
    public static Tile Hole = (Tile) new HoleTile(8);
    public static Tile TreeSapling = (Tile) new SaplingTile(9, Tile.Grass, Tile.Tree);
    public static Tile CactusSapling = (Tile) new SaplingTile(10, Tile.Sand, Tile.Cactus);
    public static Tile Farmland = (Tile) new FarmTile(11);
    public static Tile Wheat = (Tile) new WheatTile(12);
    public static Tile Lava = (Tile) new LavaTile(13);
    public static Tile StairsDown = (Tile) new StairsTile(14, false);
    public static Tile StairsUp = (Tile) new StairsTile(15, true);
    public static Tile InfiniteFall = (Tile) new InfiniteFallTile(16);
    public static Tile Cloud = (Tile) new CloudTile(17);
    public static Tile HardRock = (Tile) new HardRockTile(18);
    public static Tile IronOre = (Tile) new OreTile(19, Resource.IronOre);
    public static Tile GoldOre = (Tile) new OreTile(20, Resource.GoldOre);
    public static Tile GemOre = (Tile) new OreTile(21, Resource.Gem);
    public static Tile CloudCactus = (Tile) new CloudCactusTile(22);
    public bool ConnectsToGrass;
    public bool ConnectsToSand;
    public bool ConnectsToLava;
    public bool ConnectsToWater;
    protected Random random = new Random();

    public Tile(int id)
    {
      this.Id = (byte) id;
      Tile.Tiles[id] = this;
    }

    public virtual void Render(Screen screen, Level level, int x, int y)
    {
    }

    public virtual bool MayPass(Level level, int x, int y, Entity e) => true;

    public virtual int GetLightRadius(Level level, int x, int y) => 0;

    public virtual void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
    }

    public virtual void BumpedInto(Level level, int xt, int yt, Entity entity)
    {
    }

    public virtual void Tick(Level level, int xt, int yt)
    {
    }

    public virtual void SteppedOn(Level level, int xt, int yt, Entity entity)
    {
    }

    public virtual bool Interact(
      Level level,
      int xt,
      int yt,
      Player player,
      Item item,
      int attackDir)
    {
      return false;
    }

    public bool Use(Level level, int xt, int yt, Player player, int attackDir) => false;

    public bool CheckUse(Level level, int xt, int yt, Player player, int attackDir) => false;

    public bool ConnectsToLiquid() => this.ConnectsToWater || this.ConnectsToLava;
  }
}
