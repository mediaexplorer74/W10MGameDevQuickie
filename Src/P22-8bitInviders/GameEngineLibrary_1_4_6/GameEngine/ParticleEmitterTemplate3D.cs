// Decompiled with JetBrains decompiler
// Type: GameEngine.ParticleEmitterTemplate3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class ParticleEmitterTemplate3D
  {
    public float rotateMin;
    public float rotateMax = -1f;
    public float scaleMin = 0.05f;
    public float scaleMax = 0.25f;
    public float Spread = 45f;
    public float velocityMin = 3f;
    public float velocityMax = 8f;
    public Vector2 lifeMinMax = new Vector2(1f, 2f);
    public bool doDeathReset;
    public bool Directional;
    public Vector3 Direction = Vector3.Zero;
    public Vector3 Origin;
    public Vector3 Position;
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
