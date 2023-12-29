// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Entity


using GameManager.entity.particle;
using GameManager.gfx;
using GameManager.item;
using GameManager.level;
using GameManager.level.tile;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Entity
  {
    public int X;
    public int Y;
    public int Xr = 6;
    public int Yr = 6;
    public bool Removed;
    public Level MyLevel;
    protected Random random = new Random();

    public Entity()
    {
    }

    public Entity(Game1 game, BinaryReader reader)
    {
        this.LoadFromReader(game, reader);
    }

    public static Entity NewEntityFromReader(Game1 game, BinaryReader reader)
    {
      Entity.EntityType entityType = (Entity.EntityType) reader.ReadInt16();
      switch (entityType)
      {
        case Entity.EntityType.AirWizard:
          return (Entity) new AirWizard(game, reader);
        case Entity.EntityType.Anvil:
          return (Entity) new Anvil(game, reader);
        case Entity.EntityType.Chest:
          return (Entity) new Chest(game, reader);
        case Entity.EntityType.Furnace:
          return (Entity) new Furnace(game, reader);
        case Entity.EntityType.Lantern:
          return (Entity) new Lantern(game, reader);
        case Entity.EntityType.Oven:
          return (Entity) new Oven(game, reader);
        case Entity.EntityType.Slime:
          return (Entity) new Slime(game, reader);
        case Entity.EntityType.Spark:
          return (Entity) new Spark(game, reader);
        case Entity.EntityType.Workbench:
          return (Entity) new Workbench(game, reader);
        case Entity.EntityType.Zombie:
          return (Entity) new Zombie(game, reader);
        case Entity.EntityType.Particle:
          return (Entity) new Particle(game, reader);
        case Entity.EntityType.SmashParticle:
          return (Entity) new SmashParticle(game, reader);
        case Entity.EntityType.TextParticle:
          return (Entity) new TextParticle(game, reader);
        case Entity.EntityType.ItemEntity:
          return (Entity) new ItemEntity(game, reader);
        case Entity.EntityType.Player:
          return (Entity) new Player(game, reader);
        default:
          throw new Exception("unknown Entity " + entityType.ToString("F"));
      }
    }

    public static void SaveListToWriter(Game1 game, BinaryWriter writer, List<Entity> list)
    {
      if (list == null)
      {
        writer.Write(false);
      }
      else
      {
        writer.Write(true);
        writer.Write(list.Count);
        for (int index = 0; index < list.Count; ++index)
          list[index].SaveToWriter(game, writer);
      }
    }

    public static List<Entity> LoadListFromReader(Game1 game, BinaryReader reader)
    {
      if (!reader.ReadBoolean())
        return (List<Entity>) null;
      int num = reader.ReadInt32();
      List<Entity> entityList = new List<Entity>();
      for (int index = 0; index < num; ++index)
        entityList.Add(Entity.NewEntityFromReader(game, reader));
      return entityList;
    }

    public static void SaveListIndicesToWriter(
      BinaryWriter writer,
      List<Entity> list,
      List<Entity> sourceList)
    {
      if (list == null)
      {
        writer.Write(false);
      }
      else
      {
        writer.Write(true);
        writer.Write(list.Count);
        for (int index = 0; index < list.Count; ++index)
        {
          int num = sourceList.IndexOf(list[index]);
          if (num == -1)
            throw new Exception("unknown entity -1");
          writer.Write(num);
        }
      }
    }

    public static List<Entity> LoadListIndicesFromReader(
      BinaryReader reader,
      List<Entity> sourceList)
    {
      if (!reader.ReadBoolean())
        return (List<Entity>) null;
      int num = reader.ReadInt32();
      List<Entity> entityList = new List<Entity>();
      for (int index1 = 0; index1 < num; ++index1)
      {
        int index2 = reader.ReadInt32();
        if (index2 < 0 || index2 >= sourceList.Count)
          throw new Exception("unknown entity " + index2.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        entityList.Add(sourceList[index2]);
      }
      return entityList;
    }

    public virtual void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      this.SaveEntityTypeToWriter(writer);
      writer.Write(this.X);
      writer.Write(this.Y);
      writer.Write(this.Xr);
      writer.Write(this.Yr);
      writer.Write(this.Removed);
    }

    public virtual void LoadFromReader(Game1 game, BinaryReader reader)
    {
      this.X = reader.ReadInt32();
      this.Y = reader.ReadInt32();
      this.Xr = reader.ReadInt32();
      this.Yr = reader.ReadInt32();
      this.Removed = reader.ReadBoolean();
    }

    public void SaveEntityTypeToWriter(BinaryWriter writer)
    {
      switch (this)
      {
        case AirWizard _:
          writer.Write((short) 1);
          break;
        case Anvil _:
          writer.Write((short) 2);
          break;
        case Chest _:
          writer.Write((short) 3);
          break;
        case Furnace _:
          writer.Write((short) 4);
          break;
        case Lantern _:
          writer.Write((short) 5);
          break;
        case Oven _:
          writer.Write((short) 6);
          break;
        case Slime _:
          writer.Write((short) 7);
          break;
        case Spark _:
          writer.Write((short) 8);
          break;
        case Workbench _:
          writer.Write((short) 9);
          break;
        case Zombie _:
          writer.Write((short) 10);
          break;
        case ItemEntity _:
          writer.Write((short) 14);
          break;
        case TextParticle _:
          writer.Write((short) 13);
          break;
        case SmashParticle _:
          writer.Write((short) 12);
          break;
        case Particle _:
          writer.Write((short) 11);
          break;
        case Player _:
          writer.Write((short) 15);
          break;
        default:
          throw new Exception("unknown EntityType " + this.GetType().ToString());
      }
    }

    public virtual void Render(Screen screen)
    {
    }

    public virtual void Tick()
    {
    }

    public void Remove() => this.Removed = true;

    public void Init(Level level) => this.MyLevel = level;

    public bool Intersects(int x0, int y0, int x1, int y1)
    {
      return this.X + this.Xr >= x0 && this.Y + this.Yr >= y0 
                && this.X - this.Xr <= x1 && this.Y - this.Yr <= y1;
    }

    public virtual bool Blocks(Entity e) => false;

    public virtual void Hurt(Mob mob, int dmg, int attackDir)
    {
    }

    public virtual void Hurt(Tile tile, int x, int y, int dmg)
    {
    }

    public virtual bool Move(int xa, int ya)
    {
      if (xa == 0 && ya == 0)
        return true;
      bool flag = true;
      if (xa != 0 && this.Move2(xa, 0))
        flag = false;
      if (ya != 0 && this.Move2(0, ya))
        flag = false;
      if (!flag)
      {
        int num1 = this.X >> 4;
        int num2 = this.Y >> 4;
        this.MyLevel.GetTile(num1, num2).SteppedOn(this.MyLevel, num1, num2, this);
      }
      return !flag;
    }

    public virtual void TouchedBy(Entity entity)
    {
    }

    public virtual bool IsBlockableBy(Mob mob) => true;

    public virtual void TouchItem(ItemEntity itemEntity)
    {
    }

    public virtual bool CanSwim() => false;

    public bool Interact(Player player, Item item, int attackDir)
    {
      return item.Interact(player, this, attackDir);
    }

    public virtual bool Use(Player player, int attackDir) => false;

    public virtual bool CheckUse(Player player, int attackDir) => false;

    public virtual int GetLightRadius() => 0;

    protected bool Move2(int xa, int ya)
    {
      int num1 = this.X - this.Xr >> 4;
      int num2 = this.Y - this.Yr >> 4;
      int num3 = this.X + this.Xr >> 4;
      int num4 = this.Y + this.Yr >> 4;
      int num5 = this.X + xa - this.Xr >> 4;
      int num6 = this.Y + ya - this.Yr >> 4;
      int num7 = this.X + xa + this.Xr >> 4;
      int num8 = this.Y + ya + this.Yr >> 4;
      bool flag = false;
      for (int index1 = num6; index1 <= num8; ++index1)
      {
        for (int index2 = num5; index2 <= num7; ++index2)
        {
          if (index2 < num1 || index2 > num3 || index1 < num2 || index1 > num4)
          {
            this.MyLevel.GetTile(index2, index1).BumpedInto(this.MyLevel, index2, index1, this);
            if (!this.MyLevel.GetTile(index2, index1).MayPass(this.MyLevel, index2, index1, this))
              return false;
          }
        }
      }
      if (flag)
        return false;
      List<Entity> entities1 = this.MyLevel.GetEntities(this.X - this.Xr, this.Y - this.Yr, this.X + this.Xr, this.Y + this.Yr);
      List<Entity> entities2 = this.MyLevel.GetEntities(this.X + xa - this.Xr, this.Y + ya - this.Yr, this.X + xa + this.Xr, this.Y + ya + this.Yr);
      for (int index = 0; index < entities2.Count; ++index)
      {
        Entity entity = entities2[index];
        if (entity != this)
          entity.TouchedBy(this);
      }
      for (int index = entities2.Count - 1; index >= 0; --index)
      {
        if (entities1.Contains(entities2[index]))
          entities2.RemoveAt(index);
      }
      for (int index = 0; index < entities2.Count; ++index)
      {
        Entity entity = entities2[index];
        if (entity != this && entity.Blocks(this))
          return false;
      }
      this.X += xa;
      this.Y += ya;
      return true;
    }

    public enum EntityType
    {
      UNKNOWN,
      AirWizard,
      Anvil,
      Chest,
      Furnace,
      Lantern,
      Oven,
      Slime,
      Spark,
      Workbench,
      Zombie,
      Particle,
      SmashParticle,
      TextParticle,
      ItemEntity,
      Player,
    }
  }
}
