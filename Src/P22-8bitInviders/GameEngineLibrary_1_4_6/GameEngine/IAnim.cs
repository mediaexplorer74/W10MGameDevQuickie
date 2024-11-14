// Decompiled with JetBrains decompiler
// Type: GameEngine.IAnim
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public interface IAnim
  {
    AnimStatus Status { get; set; }

    Vector3 CurrentVector3Value { get; }

    Vector2 CurrentVector2Value { get; }

    float CurrentFloatValue { get; }

    string Name { get; set; }

    void UpdateAnimation(GameTime gameTime);

    void Start(Vector3 startvalue);

    void Play();

    void Stop();

    void Pause();

    void Restart();
  }
}
