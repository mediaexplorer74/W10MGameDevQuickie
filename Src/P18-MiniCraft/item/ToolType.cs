// Decompiled with JetBrains decompiler
// Type: GameManager.item.ToolType
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

#nullable disable
namespace GameManager.item
{
  public class ToolType
  {
    public readonly string Name;
    public readonly int ToolSprite;
    public static ToolType Shovel = new ToolType("Shvl", 0);
    public static ToolType Hoe = new ToolType(nameof (Hoe), 1);
    public static ToolType Sword = new ToolType("Swrd", 2);
    public static ToolType Pickaxe = new ToolType("Pick", 3);
    public static ToolType Axe = new ToolType(nameof (Axe), 4);

    private ToolType(string name, int sprite)
    {
      this.Name = name;
      this.ToolSprite = sprite;
    }

    public static ToolType GetToolFromSpriteNum(int sprite)
    {
      switch (sprite)
      {
        case 0:
          return ToolType.Shovel;
        case 1:
          return ToolType.Hoe;
        case 2:
          return ToolType.Sword;
        case 3:
          return ToolType.Pickaxe;
        case 4:
          return ToolType.Axe;
        default:
          return (ToolType) null;
      }
    }
  }
}
