// Decompiled with JetBrains decompiler
// Type: GameEngine.SpriteParent
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class SpriteParent : SceneNode
  {
    public Vector2 NonUniformScale = Vector2.One;
    private Matrix Transform;

    public float Scale
    {
      get => this.Scale;
      set
      {
        this.Scale = value;
        this.NonUniformScale.X = this.NonUniformScale.Y = value;
        this.UpdateLocalTransform();
      }
    }

    public Vector3 Position
    {
      get => this.Position;
      set
      {
        this.Position = value;
        this.UpdateLocalTransform();
      }
    }

    public float Rotation
    {
      get => this.Rotation;
      set
      {
        this.Rotation = value;
        this.UpdateLocalTransform();
      }
    }

    public SpriteParent(string name, Vector3 position, GameScreen screen)
      : base(name, position, screen)
    {
      this.UpdateLocalTransform();
    }

    ~SpriteParent()
    {
    }

    private void UpdateLocalTransform()
    {
      this.Transform = Matrix.CreateScale(this.NonUniformScale.X, this.NonUniformScale.Y, 1f) * Matrix.CreateRotationZ(this.Rotation) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0.0f);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.UpdateLocalTransform();
      base.Update(gameTime, ref this.Transform);
    }
  }
}
