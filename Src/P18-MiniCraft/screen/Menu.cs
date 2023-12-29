// Decompiled with JetBrains decompiler
// Type: GameManager.screen.Menu


using GameManager.gfx;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class Menu
  {
    protected Game1 game;
    protected InputHandler input = new InputHandler();

    public void Init(Game1 game, InputHandler input)
    {
      this.input = input;
      this.game = game;
    }

    public virtual void Tick()
    {
    }

    public virtual void Render(Screen screen)
    {
    }

    public void RenderItemList(
      Screen screen,
      int xo,
      int yo,
      int x1,
      int y1,
      List<ListItem> listItems,
      int selected)
    {
      bool flag = true;
      if (selected < 0)
      {
        selected = -selected - 1;
        flag = false;
      }
      int num1 = x1 - xo;
      int num2 = y1 - yo - 1;
      int num3 = 0;
      int num4 = listItems.Count;
      if (num4 > num2)
        num4 = num2;
      int num5 = selected - num2 / 2;
      if (num5 > listItems.Count - num2)
        num5 = listItems.Count - num2;
      if (num5 < 0)
        num5 = 0;
      for (int index = num3; index < num4; ++index)
        listItems[index + num5].RenderInventory(screen, (1 + xo) * 8, (index + 1 + yo) * 8);
      if (!flag)
        return;
      int num6 = selected + 1 - num5 + yo;
      Font.Draw(">", screen, xo * 8, num6 * 8, gfx.Color.Get(5, 555, 555, 555));
      Font.Draw("<", screen, (xo + num1) * 8, num6 * 8, gfx.Color.Get(5, 555, 555, 555));
    }

    public void SaveMenuTypeToWriter(BinaryWriter writer)
    {
      switch (this)
      {
        case AboutMenu _:
          writer.Write((short) 1);
          break;
        case ContainerMenu _:
          writer.Write((short) 2);
          break;
        case CraftingMenu _:
          writer.Write((short) 3);
          break;
        case DeadMenu _:
          writer.Write((short) 4);
          break;
        case InstructionsMenu _:
          writer.Write((short) 5);
          break;
        case InventoryMenu _:
          writer.Write((short) 6);
          break;
        case LevelTransitionMenu _:
          writer.Write((short) 7);
          break;
        case TitleMenu _:
          writer.Write((short) 8);
          break;
        case WonMenu _:
          writer.Write((short) 9);
          break;
        case LoadSaveMenu _:
          writer.Write((short) 10);
          break;
        case OptionsMenu _:
          writer.Write((short) 11);
          break;
        default:
          throw new Exception("unknown MenuType");
      }
    }

    public virtual void SaveToWriter(BinaryWriter writer)
    {
        this.SaveMenuTypeToWriter(writer);
    }

    public virtual void LoadFromReader(Game1 game, BinaryReader reader)
    {
    }

    public static Menu NewMenuFromReader(Game1 game, BinaryReader reader)
    {
      switch (reader.ReadInt16())
      {
        case 1:
          return (Menu) new AboutMenu(game, reader);
        case 2:
          return (Menu) new ContainerMenu(game, reader);
        case 3:
          return (Menu) new CraftingMenu(game, reader);
        case 4:
          return (Menu) new DeadMenu(game, reader);
        case 5:
          return (Menu) new InstructionsMenu(game, reader);
        case 6:
          return (Menu) new InventoryMenu(game, reader);
        case 7:
          return (Menu) new LevelTransitionMenu(game, reader);
        case 8:
          return (Menu) new TitleMenu(game, reader);
        case 9:
          return (Menu) new WonMenu(game, reader);
        case 10:
          return (Menu) new LoadSaveMenu(game, reader);
        case 11:
          return (Menu) new OptionsMenu(game, reader);
        default:
          throw new Exception("unknown Menu");
      }
    }

    public enum MenuType
    {
      UNKNOWN,
      About,
      Container,
      Crafting,
      Dead,
      Instructions,
      Inventory,
      LevelTransition,
      Title,
      Won,
      LoadSave,
      Options,
    }
  }
}
