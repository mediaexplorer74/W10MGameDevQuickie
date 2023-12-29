// Decompiled with JetBrains decompiler
// Type: GameManager.item.resource.Resource
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.level;
using GameManager.level.tile;

#nullable disable
namespace GameManager.item.resource
{
  public class Resource
  {
    public const string Wood_r = "Wood";
    public const string Stone_r = "Stone";
    public const string Flower_r = "Flower";
    public const string Acorn_r = "Acorn";
    public const string Dirt_r = "Dirt";
    public const string Sand_r = "Sand";
    public const string CactusFlower_r = "Cactus";
    public const string Seeds_r = "Seeds";
    public const string Wheat_r = "Wheat";
    public const string Bread_r = "Bread";
    public const string Apple_r = "Apple";
    public const string Coal_r = "COAL";
    public const string IronOre_r = "I.ORE";
    public const string GoldOre_r = "G.ORE";
    public const string IronIngot_r = "IRON";
    public const string GoldIngot_r = "GOLD";
    public const string Slime_r = "SLIME";
    public const string Glass_r = "glass";
    public const string Cloth_r = "cloth";
    public const string Cloud_r = "cloud";
    public const string Gem_r = "gem";
    public readonly string Name;
    public readonly int Sprite;
    public readonly int ResColor;
    public static Resource Wood = new Resource(nameof (Wood), 129, Color.Get(-1, 200, 531, 430));
    public static Resource Stone = new Resource(nameof (Stone), 130, Color.Get(-1, 111, 333, 555));
    public static Resource Flower = (Resource) new PlantableResource(nameof (Flower), 128, Color.Get(-1, 10, 444, 330), Tile.Flower, new Tile[1]
    {
      Tile.Grass
    });
    public static Resource Acorn = (Resource) new PlantableResource(nameof (Acorn), 131, Color.Get(-1, 100, 531, 320), Tile.TreeSapling, new Tile[1]
    {
      Tile.Grass
    });
    public static Resource Dirt = (Resource) new PlantableResource(nameof (Dirt), 130, Color.Get(-1, 100, 322, 432), Tile.Dirt, new Tile[3]
    {
      Tile.Hole,
      Tile.Water,
      Tile.Lava
    });
    public static Resource Sand = (Resource) new PlantableResource(nameof (Sand), 130, Color.Get(-1, 110, 440, 550), Tile.Sand, new Tile[2]
    {
      Tile.Grass,
      Tile.Dirt
    });
    public static Resource CactusFlower = (Resource) new PlantableResource("Cactus", 132, Color.Get(-1, 10, 40, 50), Tile.CactusSapling, new Tile[1]
    {
      Tile.Sand
    });
    public static Resource Seeds = (Resource) new PlantableResource(nameof (Seeds), 133, Color.Get(-1, 10, 40, 50), Tile.Wheat, new Tile[1]
    {
      Tile.Farmland
    });
    public static Resource Wheat = new Resource(nameof (Wheat), 134, Color.Get(-1, 110, 330, 550));
    public static Resource Bread = (Resource) new FoodResource(nameof (Bread), 136, Color.Get(-1, 110, 330, 550), 2, 5);
    public static Resource Apple = (Resource) new FoodResource(nameof (Apple), 137, Color.Get(-1, 100, 300, 500), 1, 5);
    public static Resource Coal = new Resource("COAL", 138, Color.Get(-1, 0, 111, 111));
    public static Resource IronOre = new Resource("I.ORE", 138, Color.Get(-1, 100, 322, 544));
    public static Resource GoldOre = new Resource("G.ORE", 138, Color.Get(-1, 110, 440, 553));
    public static Resource IronIngot = new Resource("IRON", 139, Color.Get(-1, 100, 322, 544));
    public static Resource GoldIngot = new Resource("GOLD", 139, Color.Get(-1, 110, 330, 553));
    public static Resource Slime = new Resource("SLIME", 138, Color.Get(-1, 10, 30, 50));
    public static Resource Glass = new Resource("glass", 140, Color.Get(-1, 555, 555, 555));
    public static Resource Cloth = new Resource("cloth", 129, Color.Get(-1, 25, 252, 141));
    public static Resource Cloud = (Resource) new PlantableResource("cloud", 130, Color.Get(-1, 222, 555, 444), Tile.Cloud, new Tile[1]
    {
      Tile.InfiniteFall
    });
    public static Resource Gem = new Resource("gem", 141, Color.Get(-1, 101, 404, 545));

    public Resource(string name, int sprite, int color)
    {
      this.Name = name;
      this.Sprite = sprite;
      this.ResColor = color;
    }

    public virtual bool interactOn(
      Tile tile,
      Level level,
      int xt,
      int yt,
      Player player,
      int attackDir)
    {
      return false;
    }

    public static Resource GetResourceByName(string name)
    {
      switch (name)
      {
        case "Wood":
          return Resource.Wood;
        case "Stone":
          return Resource.Stone;
        case "Flower":
          return Resource.Flower;
        case "Acorn":
          return Resource.Acorn;
        case "Dirt":
          return Resource.Dirt;
        case "Sand":
          return Resource.Sand;
        case "Cactus":
          return Resource.CactusFlower;
        case "Seeds":
          return Resource.Seeds;
        case "Wheat":
          return Resource.Wheat;
        case "Bread":
          return Resource.Bread;
        case "Apple":
          return Resource.Apple;
        case "COAL":
          return Resource.Coal;
        case "I.ORE":
          return Resource.IronOre;
        case "G.ORE":
          return Resource.GoldOre;
        case "IRON":
          return Resource.IronIngot;
        case "GOLD":
          return Resource.GoldIngot;
        case "SLIME":
          return Resource.Slime;
        case "glass":
          return Resource.Glass;
        case "cloth":
          return Resource.Cloth;
        case "cloud":
          return Resource.Cloud;
        case "gem":
          return Resource.Gem;
        default:
          return (Resource) null;
      }
    }
  }
}
