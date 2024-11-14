// Decompiled with JetBrains decompiler
// Type: GameEngine.PhysicsModel
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class PhysicsModel : Model3D
  {
    public Matrix physicsTransform;

    public PhysicsModel(string name, Vector3 position, string asset, GameScreen screen)
      : base(name, position, asset, screen)
    {
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.parentTransform = worldTransform;
      this.WorldParentTransform = Matrix.Identity * Matrix.CreateScale(this.Scale3D) * Matrix.CreateFromYawPitchRoll(this.Rotation3D.Y, this.Rotation3D.X, this.Rotation3D.Z) * Matrix.CreateTranslation(this.Position) * this.physicsTransform;
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Update(gameTime, ref this.WorldParentTransform);
    }
  }
}
