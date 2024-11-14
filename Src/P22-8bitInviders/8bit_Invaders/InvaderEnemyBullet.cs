// Decompiled with JetBrains decompiler
// Type: GameManager.InvaderEnemyBullet
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class InvaderEnemyBullet
  {
    public Vector2 Position;
    public Vector2 Direction;
    public float Rotation;

    public void InitFromPool(Vector2 where)
    {
      this.Direction = new Vector2(0.0f, -1f);
      this.Position = where;
      this.Rotation = 0.0f;
    }

    public void InitFromPool(Vector2 where, Vector2 direction)
    {
      this.Direction = direction;
      this.Position = where;
      this.Rotation = Tools.VectorToAngle(direction);
    }
  }
}
