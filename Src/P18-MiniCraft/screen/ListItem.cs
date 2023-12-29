// Decompiled with JetBrains decompiler
// Type: GameManager.screen.ListItem


using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public interface ListItem
  {
    void RenderInventory(Screen screen, int i, int j);

    void SaveToWriter(Game1 game, BinaryWriter writer);
  }
}
