// Decompiled with JetBrains decompiler
// Type: GameManager.level.Level
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.level.levelgen;
using GameManager.level.tile;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.level
{
  public class Level
  {
    public const int Version_Seen = 1;
    public static int GrassColor = 141;
    public static int DirtColor = 322;
    public static int SandColor = 550;
    public int MonsterDensity = 8;
    public int W;
    public int H;
    public byte[] Tiles;
    public byte[] Data;
    public bool[] Seen;
    public List<Entity>[] EntitiesInTiles;
    public List<Entity> Entities = new List<Entity>();
    public Player MyPlayer;
    private Random random = new Random();
    private int depth;
    private List<Entity> rowSprites = new List<Entity>();

    public static int CompareEntity(Entity e0, Entity e1)
    {
      if (e1.Y < e0.Y)
        return 1;
      return e1.Y > e0.Y ? -1 : 0;
    }

    public Level(int w, int h, int level, Level parentLevel, Game1 game)
    {
      if (level < 0)
        Level.DirtColor = 222;
      this.depth = level;
      this.W = w;
      this.H = h;
      if (level == 1)
        Level.DirtColor = 444;
      byte[][] numArray;
      if (level == 0)
        numArray = LevelGen.CreateAndValidateTopMap(w, h);
      else if (level < 0)
      {
        numArray = LevelGen.CreateAndValidateUndergroundMap(w, h, -level);
        this.MonsterDensity = 4;
      }
      else
      {
        numArray = LevelGen.CreateAndValidateSkyMap(w, h);
        this.MonsterDensity = 4;
      }
      this.Tiles = numArray[0];
      this.Data = numArray[1];
      this.Seen = new bool[this.Tiles.Length];
      for (int index = 0; index < this.Seen.Length; ++index)
        this.Seen[index] = false;
      if (parentLevel != null)
      {
        for (int y = 0; y < h; ++y)
        {
          for (int x = 0; x < w; ++x)
          {
            if (parentLevel.GetTile(x, y) == Tile.StairsDown)
            {
              this.SetTile(x, y, Tile.StairsUp, 0);
              if (level == 0)
              {
                this.SetTile(x - 1, y, Tile.HardRock, 0);
                this.SetTile(x + 1, y, Tile.HardRock, 0);
                this.SetTile(x, y - 1, Tile.HardRock, 0);
                this.SetTile(x, y + 1, Tile.HardRock, 0);
                this.SetTile(x - 1, y - 1, Tile.HardRock, 0);
                this.SetTile(x - 1, y + 1, Tile.HardRock, 0);
                this.SetTile(x + 1, y - 1, Tile.HardRock, 0);
                this.SetTile(x + 1, y + 1, Tile.HardRock, 0);
              }
              else
              {
                this.SetTile(x - 1, y, Tile.Dirt, 0);
                this.SetTile(x + 1, y, Tile.Dirt, 0);
                this.SetTile(x, y - 1, Tile.Dirt, 0);
                this.SetTile(x, y + 1, Tile.Dirt, 0);
                this.SetTile(x - 1, y - 1, Tile.Dirt, 0);
                this.SetTile(x - 1, y + 1, Tile.Dirt, 0);
                this.SetTile(x + 1, y - 1, Tile.Dirt, 0);
                this.SetTile(x + 1, y + 1, Tile.Dirt, 0);
              }
            }
          }
        }
      }
      this.EntitiesInTiles = new List<Entity>[w * h];
      for (int index = 0; index < w * h; ++index)
        this.EntitiesInTiles[index] = new List<Entity>();
      if (level != 1)
        return;
      game.ActiveAirWizard = new AirWizard();
      game.ActiveAirWizard.X = w * 8;
      game.ActiveAirWizard.Y = h * 8;
      this.Add((Entity) game.ActiveAirWizard);
    }

    public Level(Game1 game, BinaryReader reader, int version)
    {
      this.LoadFromReader(game, reader, version);
    }

    public void LoadFromReader(Game1 game, BinaryReader reader, int version)
    {
      this.MyPlayer = !reader.ReadBoolean() ? (Player) null : game.ActivePlayer;
      this.W = reader.ReadInt32();
      this.H = reader.ReadInt32();
      Level.GrassColor = reader.ReadInt32();
      Level.DirtColor = reader.ReadInt32();
      Level.SandColor = reader.ReadInt32();
      this.depth = reader.ReadInt32();
      this.MonsterDensity = reader.ReadInt32();
      int count1 = reader.ReadInt32();
      this.Tiles = new byte[count1];
      this.Tiles = reader.ReadBytes(count1);
      int count2 = reader.ReadInt32();
      this.Data = new byte[count2];
      this.Data = reader.ReadBytes(count2);
      this.Entities = Entity.LoadListFromReader(game, reader);
      for (int index = 0; index < this.Entities.Count; ++index)
      {
        if (this.Entities[index] is Player)
          this.Entities[index] = (Entity) this.MyPlayer;
        this.Entities[index].MyLevel = this;
      }
      this.EntitiesInTiles = new List<Entity>[reader.ReadInt32()];
      for (int index = 0; index < this.EntitiesInTiles.Length; ++index)
        this.EntitiesInTiles[index] = Entity.LoadListIndicesFromReader(reader, this.Entities);
      this.rowSprites = Entity.LoadListIndicesFromReader(reader, this.Entities);
      if (version >= 1)
      {
        this.Seen = new bool[reader.ReadInt32()];
        for (int index = 0; index < this.Seen.Length; ++index)
          this.Seen[index] = reader.ReadBoolean();
      }
      else
        this.Seen = new bool[this.Tiles.Length];
    }

    public void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      writer.Write(this.MyPlayer != null);
      writer.Write(this.W);
      writer.Write(this.H);
      writer.Write(Level.GrassColor);
      writer.Write(Level.DirtColor);
      writer.Write(Level.SandColor);
      writer.Write(this.depth);
      writer.Write(this.MonsterDensity);
      writer.Write(this.Tiles.Length);
      writer.Write(this.Tiles);
      writer.Write(this.Data.Length);
      for (int index = 0; index < this.Data.Length; ++index)
        writer.Write(this.Data[index]);
      Entity.SaveListToWriter(game, writer, this.Entities);
      writer.Write(this.EntitiesInTiles.Length);
      for (int index = 0; index < this.EntitiesInTiles.Length; ++index)
        Entity.SaveListIndicesToWriter(writer, this.EntitiesInTiles[index], this.Entities);
      Entity.SaveListIndicesToWriter(writer, this.rowSprites, this.Entities);
      for (int index = 0; index < this.Entities.Count; ++index)
      {
        if (this.Entities[index] is AirWizard)
        {
          game.ActiveAirWizard = (AirWizard) this.Entities[index];
          break;
        }
      }
      writer.Write(this.Seen.Length);
      for (int index = 0; index < this.Seen.Length; ++index)
        writer.Write(this.Seen[index]);
    }

    public void RenderBackground(Screen screen, int xScroll, int yScroll)
    {
      int num1 = xScroll >> 4;
      int num2 = yScroll >> 4;
      int num3 = screen.W + 15 >> 4;
      int num4 = screen.H + 15 >> 4;
      screen.SetOffset(xScroll, yScroll);
      for (int y = num2; y <= num4 + num2; ++y)
      {
        for (int x = num1; x <= num3 + num1; ++x)
        {
          if (y < num4 + num2 || x > num1 + 9)
          {
            this.GetTile(x, y).Render(screen, this, x, y);
            this.Seen[x + y * this.W] = true;
          }
        }
      }
      screen.SetOffset(0, 0);
    }

    public void RenderSprites(Screen screen, int xScroll, int yScroll)
    {
      int num1 = xScroll >> 4;
      int num2 = yScroll >> 4;
      int num3 = screen.W + 15 >> 4;
      int num4 = screen.H + 15 >> 4;
      screen.SetOffset(xScroll, yScroll);
      for (int index1 = num2; index1 <= num4 + num2; ++index1)
      {
        for (int index2 = num1; index2 <= num3 + num1; ++index2)
        {
          if (index2 >= 0 && index1 >= 0 && index2 < this.W && index1 < this.H)
            this.rowSprites.AddRange((IEnumerable<Entity>) this.EntitiesInTiles[index2 + index1 * this.W]);
        }
        if (this.rowSprites.Count > 0)
          this.SortAndRender(screen, this.rowSprites);
        this.rowSprites.Clear();
      }
      screen.SetOffset(0, 0);
    }

    public void RenderLight(Screen screen, int xScroll, int yScroll)
    {
      int num1 = xScroll >> 4;
      int num2 = yScroll >> 4;
      int num3 = screen.W + 15 >> 4;
      int num4 = screen.H + 15 >> 4;
      screen.SetOffset(xScroll, yScroll);
      int num5 = 4;
      for (int y = num2 - num5; y <= num4 + num2 + num5; ++y)
      {
        for (int x = num1 - num5; x <= num3 + num1 + num5; ++x)
        {
          if (x >= 0 && y >= 0 && x < this.W && y < this.H)
          {
            List<Entity> entitiesInTile = this.EntitiesInTiles[x + y * this.W];
            for (int index = 0; index < entitiesInTile.Count; ++index)
            {
              Entity entity = entitiesInTile[index];
              int lightRadius = entity.GetLightRadius();
              if (lightRadius > 0)
                screen.RenderLight(entity.X - 1, entity.Y - 4, lightRadius * 8);
            }
            int lightRadius1 = this.GetTile(x, y).GetLightRadius(this, x, y);
            if (lightRadius1 > 0)
              screen.RenderLight(x * 16 + 8, y * 16 + 8, lightRadius1 * 8);
          }
        }
      }
      screen.SetOffset(0, 0);
    }

    public Tile GetTile(int x, int y)
    {
      return x < 0 || y < 0 || x >= this.W || y >= this.H ? Tile.Rock : Tile.Tiles[(int) this.Tiles[x + y * this.W]];
    }

    public void SetTile(int x, int y, Tile t, int dataVal)
    {
      if (x < 0 || y < 0 || x >= this.W || y >= this.H)
        return;
      this.Tiles[x + y * this.W] = t.Id;
      this.Data[x + y * this.W] = (byte) dataVal;
    }

    public int GetData(int x, int y)
    {
      return x < 0 || y < 0 || x >= this.W || y >= this.H ? 0 : (int) this.Data[x + y * this.W] & (int) byte.MaxValue;
    }

    public void SetData(int x, int y, int val)
    {
      if (x < 0 || y < 0 || x >= this.W || y >= this.H)
        return;
      this.Data[x + y * this.W] = (byte) val;
    }

    public void Add(Entity entity)
    {
      if (entity is Player)
        this.MyPlayer = (Player) entity;
      entity.Removed = false;
      this.Entities.Add(entity);
      entity.Init(this);
      this.insertEntity(entity.X >> 4, entity.Y >> 4, entity);
    }

    public void Remove(Entity e)
    {
      this.Entities.Remove(e);
      this.removeEntity(e.X >> 4, e.Y >> 4, e);
    }

    public void TrySpawn(int count)
    {
      for (int index = 0; index < count; ++index)
      {
        int num1 = 1;
        int num2 = 1;
        if (this.depth < 0)
          num2 = -this.depth + 1;
        if (this.depth > 0)
          num1 = num2 = 4;
        int lvl = this.random.Next(num2 - num1 + 1) + num1;
        Mob mob = this.random.Next(2) != 0 ? (Mob) new Zombie(lvl) : (Mob) new Slime(lvl);
        if (mob.FindStartPos(this))
          this.Add((Entity) mob);
      }
    }

    public void Tick()
    {
      this.TrySpawn(1);
      for (int index = 0; index < this.W * this.H / 50; ++index)
      {
        int num1 = this.random.Next(this.W);
        int num2 = this.random.Next(this.W);
        this.GetTile(num1, num2).Tick(this, num1, num2);
      }
      for (int index = 0; index < this.Entities.Count; ++index)
      {
        Entity entity = this.Entities[index];
        int x1 = entity.X >> 4;
        int y1 = entity.Y >> 4;
        entity.Tick();
        if (entity.Removed)
        {
          this.Entities.RemoveAt(index--);
          this.removeEntity(x1, y1, entity);
        }
        else
        {
          int x2 = entity.X >> 4;
          int y2 = entity.Y >> 4;
          if (x1 != x2 || y1 != y2)
          {
            this.removeEntity(x1, y1, entity);
            this.insertEntity(x2, y2, entity);
          }
        }
      }
    }

    public List<Entity> GetEntities(int x0, int y0, int x1, int y1)
    {
      List<Entity> entities = new List<Entity>();
      int num1 = (x0 >> 4) - 1;
      int num2 = (y0 >> 4) - 1;
      int num3 = (x1 >> 4) + 1;
      int num4 = (y1 >> 4) + 1;
      for (int index1 = num2; index1 <= num4; ++index1)
      {
        for (int index2 = num1; index2 <= num3; ++index2)
        {
          if (index2 >= 0 && index1 >= 0 && index2 < this.W && index1 < this.H)
          {
            List<Entity> entitiesInTile = this.EntitiesInTiles[index2 + index1 * this.W];
            for (int index3 = 0; index3 < entitiesInTile.Count; ++index3)
            {
              Entity entity = entitiesInTile[index3];
              if (entity.Intersects(x0, y0, x1, y1))
                entities.Add(entity);
            }
          }
        }
      }
      return entities;
    }

    private void SortAndRender(Screen screen, List<Entity> list)
    {
      list.Sort(new Comparison<Entity>(Level.CompareEntity));
      for (int index = 0; index < list.Count; ++index)
        list[index].Render(screen);
    }

    private void insertEntity(int x, int y, Entity e)
    {
      if (x < 0 || y < 0 || x >= this.W || y >= this.H)
        return;
      this.EntitiesInTiles[x + y * this.W].Add(e);
    }

    private void removeEntity(int x, int y, Entity e)
    {
      if (x < 0 || y < 0 || x >= this.W || y >= this.H)
        return;
      this.EntitiesInTiles[x + y * this.W].Remove(e);
    }
  }
}
