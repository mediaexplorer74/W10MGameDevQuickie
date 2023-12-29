// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Player


using GameManager.entity.particle;
using GameManager.gfx;
using GameManager.item;
using GameManager.level;
using GameManager.level.tile;
using GameManager.screen;
using GameManager.sound;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Player : Mob
  {
    public Game1 MyGame;
    public Inventory PlayerInventory = new Inventory();
    public Item AttackItem;
    public Item ActiveItem;
    public int Stamina;
    public int StaminaRecharge;
    public int StaminaRechargeDelay;
    public int Score;
    public int MaxStamina = 10;
    public int InvulnerableTime;
    private InputHandler input;
    private int attackTime;
    private int attackDir;
    private int onStairDelay;

    public Player(Game1 game, InputHandler input)
    {
      this.MyGame = game;
      this.input = input;
      this.X = 24;
      this.Y = 24;
      this.Stamina = this.MaxStamina;
      this.PlayerInventory.Add((ListItem) new FurnitureItem((Furniture) new Workbench()));
      this.PlayerInventory.Add((ListItem) new PowerGloveItem());
    }

    public Player(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.attackTime);
      writer.Write(this.attackDir);
      writer.Write(this.Stamina);
      writer.Write(this.StaminaRecharge);
      writer.Write(this.StaminaRechargeDelay);
      writer.Write(this.Score);
      writer.Write(this.MaxStamina);
      writer.Write(this.onStairDelay);
      writer.Write(this.InvulnerableTime);

      this.PlayerInventory.SaveToWriter(game, writer);
      this.PlayerInventory.SaveItemIndexToWriter((ListItem) this.AttackItem, writer);
      if (this.ActiveItem == null)
      {
        writer.Write(false);
      }
      else
      {
        writer.Write(true);
        this.ActiveItem.SaveToWriter(game, writer);
      }
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      this.MyGame = game;
      this.input = game.Input;

      base.LoadFromReader(game, reader);
      this.attackTime = reader.ReadInt32();
      this.attackDir = reader.ReadInt32();
      this.Stamina = reader.ReadInt32();
      this.StaminaRecharge = reader.ReadInt32();
      this.StaminaRechargeDelay = reader.ReadInt32();
      this.Score = reader.ReadInt32();
      this.MaxStamina = reader.ReadInt32();
      this.onStairDelay = reader.ReadInt32();
      this.InvulnerableTime = reader.ReadInt32();
      this.PlayerInventory = new Inventory(game, reader);
      this.AttackItem = (Item) this.PlayerInventory.GetItemByIndex(reader.ReadInt32());

      if (reader.ReadBoolean())
        this.ActiveItem = Item.NewItemFromReader(game, reader);
      else
        this.ActiveItem = (Item) null;
    }

    public override void Tick()
    {
      base.Tick();
      if (this.InvulnerableTime > 0)
        --this.InvulnerableTime;
      Tile tile = this.MyLevel.GetTile(this.X >> 4, this.Y >> 4);
      if (tile == Tile.StairsDown || tile == Tile.StairsUp)
      {
        if (this.onStairDelay == 0)
        {
          this.ChangeLevel(tile == Tile.StairsUp ? 1 : -1);
          this.onStairDelay = 10;
          return;
        }
        this.onStairDelay = 10;
      }
      else if (this.onStairDelay > 0)
        --this.onStairDelay;
      if (this.Stamina <= 0 && this.StaminaRechargeDelay == 0 && this.StaminaRecharge == 0)
        this.StaminaRechargeDelay = 40;
      if (this.StaminaRechargeDelay > 0)
        --this.StaminaRechargeDelay;
      if (this.StaminaRechargeDelay == 0)
      {
        ++this.StaminaRecharge;
        if (this.IsSwimming())
          this.StaminaRecharge = 0;
        while (this.StaminaRecharge > 10)
        {
          this.StaminaRecharge -= 10;
          if (this.Stamina < this.MaxStamina)
            ++this.Stamina;
        }
      }
      int xa = 0;
      int ya = 0;
      
      //RnD
      //if (this.input.Up.Down)
      //  --ya;
      //if (this.input.Down.Down)
      //  ++ya;
      //if (this.input.Left.Down)
      //  --xa;
      //if (this.input.Right.Down)
      //  ++xa;

      if (this.IsSwimming() && this.TickTime % 60 == 0)
      {
        if (this.Stamina > 0)
          --this.Stamina;
        else
          this.Hurt((Mob) this, 1, this.Dir ^ 1);
      }
      if (this.StaminaRechargeDelay % 2 == 0)
        this.Move(xa, ya);

      //RnD
      if (/*this.input.Attack.Clicked &&*/ this.Stamina != 0)
      {
        --this.Stamina;
        this.StaminaRecharge = 0;
        this.Attack();
      }

      if (/*this.input.Menu.Clicked && */!this.Use())
        this.MyGame.SetMenu((Menu) new InventoryMenu(this));

      if (this.attackTime <= 0)
        return;
      --this.attackTime;
    }

    public bool Use()
    {
      int num1 = -2;
      if (this.Dir == 0 && this.Use(this.X - 8, this.Y + 4 + num1, this.X + 8, this.Y + 12 + num1) 
                || this.Dir == 1 && this.Use(this.X - 8, this.Y - 12 + num1,
                this.X + 8, this.Y - 4 + num1) 
                || this.Dir == 3 && this.Use(this.X + 4, this.Y - 8 + num1, this.X + 12,
                this.Y + 8 + num1) || this.Dir == 2 && this.Use(this.X - 12, this.Y - 8 + num1,
                this.X - 4, this.Y + 8 + num1))
        return true;

      int num2 = this.X >> 4;
      int num3 = this.Y + num1 >> 4;
      int num4 = 12;
      if (this.attackDir == 0)
        num3 = this.Y + num4 + num1 >> 4;
      if (this.attackDir == 1)
        num3 = this.Y - num4 + num1 >> 4;
      if (this.attackDir == 2)
        num2 = this.X - num4 >> 4;
      if (this.attackDir == 3)
        num2 = this.X + num4 >> 4;
      return num2 >= 0 && num3 >= 0 && num2 < this.MyLevel.W && num3 < this.MyLevel.H && this.MyLevel.GetTile(num2, num3).Use(this.MyLevel, num2, num3, this, this.attackDir);
    }

    public bool CheckUse()
    {
      if (this.MyLevel != null)
      {
        int num1 = -2;
        if (this.Dir == 0 && this.CheckUse(this.X - 8, this.Y + 4 + num1, this.X + 8, this.Y + 12 + num1) || this.Dir == 1 && this.CheckUse(this.X - 8, this.Y - 12 + num1, this.X + 8, this.Y - 4 + num1) || this.Dir == 3 && this.CheckUse(this.X + 4, this.Y - 8 + num1, this.X + 12, this.Y + 8 + num1) || this.Dir == 2 && this.CheckUse(this.X - 12, this.Y - 8 + num1, this.X - 4, this.Y + 8 + num1))
          return true;
        int num2 = this.X >> 4;
        int num3 = this.Y + num1 >> 4;
        int num4 = 12;
        if (this.attackDir == 0)
          num3 = this.Y + num4 + num1 >> 4;
        if (this.attackDir == 1)
          num3 = this.Y - num4 + num1 >> 4;
        if (this.attackDir == 2)
          num2 = this.X - num4 >> 4;
        if (this.attackDir == 3)
          num2 = this.X + num4 >> 4;
        if (num2 >= 0 && num3 >= 0 && num2 < this.MyLevel.W && num3 < this.MyLevel.H && this.MyLevel.GetTile(num2, num3).CheckUse(this.MyLevel, num2, num3, this, this.attackDir))
          return true;
      }
      return false;
    }

    public override void Render(Screen screen)
    {
      int num1 = 0;
      int num2 = 14;
      int bits1 = this.walkDist >> 3 & 1;
      int bits2 = this.walkDist >> 3 & 1;
      if (this.Dir == 1)
        num1 += 2;
      if (this.Dir > 1)
      {
        bits1 = 0;
        bits2 = this.walkDist >> 4 & 1;
        if (this.Dir == 2)
          bits1 = 1;
        num1 += 4 + (this.walkDist >> 3 & 1) * 2;
      }

      int xp = this.X - 8;
      int yp = this.Y - 11;

      if (this.IsSwimming())
      {
        yp += 4;
        int colors = gfx.Color.Get(-1, -1, 115, 335);

        if (this.TickTime / 8 % 2 == 0)
          colors = gfx.Color.Get(-1, 335, 5, 115);

        screen.Render(xp, yp + 3, 421, colors, 0);
        screen.Render(xp + 8, yp + 3, 421, colors, 1);
      }

      if (this.attackTime > 0 && this.attackDir == 1)
      {
        screen.Render(xp, yp - 4, 422, gfx.Color.Get(-1, 555, 555, 555), 0);
        screen.Render(xp + 8, yp - 4, 422, gfx.Color.Get(-1, 555, 555, 555), 1);
        if (this.AttackItem != null)
          this.AttackItem.RenderIcon(screen, xp + 4, yp - 4);
      }

      int colors1 = gfx.Color.Get(-1, 100, 220, 532);

      if (this.HurtTime > 0)
        colors1 = gfx.Color.Get(-1, 555, 555, 555);

      if (this.ActiveItem is FurnitureItem)
        num2 += 2;
      screen.Render(xp + 8 * bits1, yp, num1 + num2 * 32, colors1, bits1);
      screen.Render(xp + 8 - 8 * bits1, yp, num1 + 1 + num2 * 32, colors1, bits1);
      if (!this.IsSwimming())
      {
        screen.Render(xp + 8 * bits2, yp + 8, num1 + (num2 + 1) * 32, colors1, bits2);
        screen.Render(xp + 8 - 8 * bits2, yp + 8, num1 + 1 + (num2 + 1) * 32, colors1, bits2);
      }
      if (this.attackTime > 0 && this.attackDir == 2)
      {
        screen.Render(xp - 4, yp, 423, gfx.Color.Get(-1, 555, 555, 555), 1);
        screen.Render(xp - 4, yp + 8, 423, gfx.Color.Get(-1, 555, 555, 555), 3);
        if (this.AttackItem != null)
          this.AttackItem.RenderIcon(screen, xp - 4, yp + 4);
      }
      if (this.attackTime > 0 && this.attackDir == 3)
      {
        screen.Render(xp + 8 + 4, yp, 423, gfx.Color.Get(-1, 555, 555, 555), 0);
        screen.Render(xp + 8 + 4, yp + 8, 423, gfx.Color.Get(-1, 555, 555, 555), 2);
        if (this.AttackItem != null)
          this.AttackItem.RenderIcon(screen, xp + 8 + 4, yp + 4);
      }
      if (this.attackTime > 0 && this.attackDir == 0)
      {
        screen.Render(xp, yp + 8 + 4, 422, gfx.Color.Get(-1, 555, 555, 555), 2);
        screen.Render(xp + 8, yp + 8 + 4, 422, gfx.Color.Get(-1, 555, 555, 555), 3);
        if (this.AttackItem != null)
          this.AttackItem.RenderIcon(screen, xp + 4, yp + 8 + 4);
      }
      if (!(this.ActiveItem is FurnitureItem))
        return;
      Furniture furniture = ((FurnitureItem) this.ActiveItem).MyFurniture;
      furniture.X = this.X;
      furniture.Y = yp;
      furniture.Render(screen);
    }

    public override void TouchItem(ItemEntity itemEntity)
    {
      itemEntity.Take(this);
      this.PlayerInventory.Add((ListItem) itemEntity.MyItem);
    }

    public override bool CanSwim() => true;

    public override bool FindStartPos(Level level)
    {
      int x;
      int y;
      do
      {
        x = this.random.Next(level.W);
        y = this.random.Next(level.H);
      }
      while (level.GetTile(x, y) != Tile.Grass);
      this.X = x * 16 + 8;
      this.Y = y * 16 + 8;
      return true;
    }

    public bool PayStamina(int cost)
    {
      if (cost > this.Stamina)
        return false;
      this.Stamina -= cost;
      return true;
    }

    public void ChangeLevel(int dir) => this.MyGame.ScheduleLevelChange(dir);

    public override int GetLightRadius()
    {
      int lightRadius1 = 2;
      if (this.ActiveItem != null && this.ActiveItem is FurnitureItem)
      {
        int lightRadius2 = ((FurnitureItem) this.ActiveItem).MyFurniture.GetLightRadius();
        if (lightRadius2 > lightRadius1)
          lightRadius1 = lightRadius2;
      }
      return lightRadius1;
    }

    public void GameWon()
    {
      this.MyLevel.MyPlayer.InvulnerableTime = 300;
      this.MyGame.Won();
    }

    public override void TouchedBy(Entity entity)
    {
      if (entity is Player)
        return;
      entity.TouchedBy((Entity) this);
    }

    protected override void DoHurt(int damage, int attackDir)
    {
      if (this.HurtTime > 0 || this.InvulnerableTime > 0)
        return;
      Sound.PlayerHurt.Play();
     
            this.MyLevel.Add((Entity) new TextParticle(string.Empty + (object) damage, 
          this.X, this.Y, gfx.Color.Get(-1, 504, 504, 504)));

      this.Health -= damage;
      if (attackDir == 0)
        this.yKnockback = 6;
      if (attackDir == 1)
        this.yKnockback = -6;
      if (attackDir == 2)
        this.xKnockback = -6;
      if (attackDir == 3)
        this.xKnockback = 6;
      this.HurtTime = 10;
      this.InvulnerableTime = 30;
    }

    protected override void Die()
    {
      base.Die();
      Sound.PlayerDeath.Play();
    }

    private void Attack()
    {
      this.walkDist += 8;
      this.attackDir = this.Dir;
      this.AttackItem = this.ActiveItem;
      bool flag = false;
      if (this.ActiveItem != null)
      {
        this.attackTime = 10;
        int num1 = -2;
        int num2 = 12;
        if (this.Dir == 0 && this.Interact(this.X - 8, this.Y + 4 + num1, this.X + 8, this.Y + num2 + num1))
          flag = true;
        if (this.Dir == 1 && this.Interact(this.X - 8, this.Y - num2 + num1, this.X + 8, this.Y - 4 + num1))
          flag = true;
        if (this.Dir == 3 && this.Interact(this.X + 4, this.Y - 8 + num1, this.X + num2, this.Y + 8 + num1))
          flag = true;
        if (this.Dir == 2 && this.Interact(this.X - num2, this.Y - 8 + num1, this.X - 4, this.Y + 8 + num1))
          flag = true;
        if (flag)
          return;
        int num3 = this.X >> 4;
        int num4 = this.Y + num1 >> 4;
        int num5 = 12;
        if (this.attackDir == 0)
          num4 = this.Y + num5 + num1 >> 4;
        if (this.attackDir == 1)
          num4 = this.Y - num5 + num1 >> 4;
        if (this.attackDir == 2)
          num3 = this.X - num5 >> 4;
        if (this.attackDir == 3)
          num3 = this.X + num5 >> 4;
        if (num3 >= 0 && num4 >= 0 && num3 < this.MyLevel.W && num4 < this.MyLevel.H)
        {
          if (this.ActiveItem.InteractOn(this.MyLevel.GetTile(num3, num4), this.MyLevel, num3, num4, this, this.attackDir))
            flag = true;
          else if (this.MyLevel.GetTile(num3, num4).Interact(this.MyLevel, num3, num4, this, this.ActiveItem, this.attackDir))
            flag = true;
          if (this.ActiveItem.IsDepleted())
            this.ActiveItem = (Item) null;
        }
      }
      if (flag || this.ActiveItem != null && !this.ActiveItem.CanAttack())
        return;
      this.attackTime = 5;
      int num6 = -2;
      int num7 = 20;
      if (this.Dir == 0)
        this.Hurt(this.X - 8, this.Y + 4 + num6, this.X + 8, this.Y + num7 + num6);
      if (this.Dir == 1)
        this.Hurt(this.X - 8, this.Y - num7 + num6, this.X + 8, this.Y - 4 + num6);
      if (this.Dir == 3)
        this.Hurt(this.X + 4, this.Y - 8 + num6, this.X + num7, this.Y + 8 + num6);
      if (this.Dir == 2)
        this.Hurt(this.X - num7, this.Y - 8 + num6, this.X - 4, this.Y + 8 + num6);
      int x = this.X >> 4;
      int y = this.Y + num6 >> 4;
      int num8 = 12;
      if (this.attackDir == 0)
        y = this.Y + num8 + num6 >> 4;
      if (this.attackDir == 1)
        y = this.Y - num8 + num6 >> 4;
      if (this.attackDir == 2)
        x = this.X - num8 >> 4;
      if (this.attackDir == 3)
        x = this.X + num8 >> 4;
      if (x < 0 || y < 0 || x >= this.MyLevel.W || y >= this.MyLevel.H)
        return;
      this.MyLevel.GetTile(x, y).Hurt(this.MyLevel, x, y, (Mob) this, this.random.Next(3) + 1, this.attackDir);
    }

    private bool Use(int x0, int y0, int x1, int y1)
    {
      List<Entity> entities = this.MyLevel.GetEntities(x0, y0, x1, y1);
      for (int index = 0; index < entities.Count; ++index)
      {
        Entity entity = entities[index];
        if (entity != this && entity.Use(this, this.attackDir))
          return true;
      }
      return false;
    }

    private bool CheckUse(int x0, int y0, int x1, int y1)
    {
      if (this.MyLevel != null)
      {
        List<Entity> entities = this.MyLevel.GetEntities(x0, y0, x1, y1);
        for (int index = 0; index < entities.Count; ++index)
        {
          Entity entity = entities[index];
          if (entity != this && entity.CheckUse(this, this.attackDir))
            return true;
        }
      }
      return false;
    }

    private bool Interact(int x0, int y0, int x1, int y1)
    {
      List<Entity> entities = this.MyLevel.GetEntities(x0, y0, x1, y1);
      for (int index = 0; index < entities.Count; ++index)
      {
        Entity entity = entities[index];
        if (entity != this && entity.Interact(this, this.ActiveItem, this.attackDir))
          return true;
      }
      return false;
    }

    private void Hurt(int x0, int y0, int x1, int y1)
    {
      List<Entity> entities = this.MyLevel.GetEntities(x0, y0, x1, y1);
      for (int index = 0; index < entities.Count; ++index)
      {
        Entity e = entities[index];
        if (e != this)
          e.Hurt((Mob) this, this.GetAttackDamage(e), this.attackDir);
      }
    }

    private int GetAttackDamage(Entity e)
    {
      int attackDamage = this.random.Next(3) + 1;
      if (this.AttackItem != null)
        attackDamage += this.AttackItem.GetAttackDamageBonus(e);
      return attackDamage;
    }
  }
}
