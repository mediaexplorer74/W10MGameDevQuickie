// Decompiled with JetBrains decompiler
// Type: GameEngine.ParticleEmitterTemplate
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class ParticleEmitterTemplate
  {
    public float rotateMin;
    public float rotateMax = -1f;
    public float scaleMin = 1f;
    public float scaleMax = 1f;
    public float Spread = 45f;
    public int velocityMin = 50;
    public int velocityMax = 150;
    public Vector2 lifeMinMax = new Vector2(1f, 2f);
    public bool doDeathReset;
    public bool Directional;
    public float Direction;
    public Vector2 Origin;
    public Vector2 Position;
    public Vector2 Velocity;
    public float Scale = 1f;
    public float Rotation;
    public float Lifetime;
    public bool doFade = true;
    public float RotationSpeed;
    public float scaleRate;
    public bool OrientToDirection;
    public float Gravity;
    public float decayFactor = 0.02f;
    public bool Follow;
  }
}
