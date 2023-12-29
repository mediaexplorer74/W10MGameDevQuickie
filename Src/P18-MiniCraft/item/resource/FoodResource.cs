// Decompiled with JetBrains decompiler
// Type: GameManager.item.resource.FoodResource
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.level;
using GameManager.level.tile;

#nullable disable
namespace GameManager.item.resource
{
  public class FoodResource : Resource
  {
    private int heal;
    private int staminaCost;

    public FoodResource(string name, int sprite, int color, int heal, int staminaCost)
      : base(name, sprite, color)
    {
      this.heal = heal;
      this.staminaCost = staminaCost;
    }

    public override bool interactOn(
      Tile tile,
      Level level,
      int xt,
      int yt,
      Player player,
      int attackDir)
    {
      if (player.Health >= player.MaxHealth || !player.PayStamina(this.staminaCost))
        return false;
      player.Heal(this.heal);
      return true;
    }
  }
}
