// Decompiled with JetBrains decompiler
// Type: GameEngine.Layer3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class Layer3D : SceneNode
  {
    public ICamera Camera;
    public SceneNode FocusTarget;

    public Layer3D(GameScreen screen)
      : this("layer3D", screen)
    {
    }

    public Layer3D(ICamera camera, GameScreen screen)
      : this("layer3D", camera, screen)
    {
    }

    public Layer3D(string name, GameScreen screen)
      : base(name, screen)
    {
      this.FocusTarget = new SceneNode("focusTarget", screen);
      this.Camera = (ICamera) new Camera3D(CameraType.LookAt, new Vector3(0.0f, 10f, 10f), 60f, this.FocusTarget);
    }

    public Layer3D(string name, ICamera cam, GameScreen screen)
      : base(name, screen)
    {
      this.Camera = cam;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.Camera.Update(gameTime);
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw() => this.Draw(this.Camera);
  }
}
