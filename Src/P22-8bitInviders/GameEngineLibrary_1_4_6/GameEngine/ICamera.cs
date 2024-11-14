// Decompiled with JetBrains decompiler
// Type: GameEngine.ICamera
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public interface ICamera
  {
    Matrix ProjectionMatrix { get; }

    Matrix ViewMatrix { get; }

    Vector3 camPosition { get; }

    Vector3 camUp { get; }

    void Update(GameTime gameTime);

    bool IsInCameraFrustrum(BoundingSphere bsphere);

    bool IsInCameraFrustrum(BoundingBox bbox);

    Vector3? CalculateMouse3DPosition(Vector2 touchPosition);

    Vector3? CalculateMouse3DPosition(Vector2 touchPosition, Plane GroundPlane);
  }
}
