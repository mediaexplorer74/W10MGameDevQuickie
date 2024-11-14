// Decompiled with JetBrains decompiler
// Type: GameEngine.AniManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public sealed class AniManager : GameComponent
  {
    private Dictionary<string, Anim> Animations = new Dictionary<string, Anim>();
    private List<IAnim> copy = new List<IAnim>();

    public AniManager(Game game)
      : base(game)
    {
    }

    public void AddAnim(Anim anim)
    {
      if (!this.Animations.ContainsKey(anim.Name))
      {
        this.Animations.Add(anim.Name, anim);
      }
      else
      {
        this.Animations.Remove(anim.Name);
        this.Animations.Add(anim.Name, anim);
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (Engine.isObscured || Engine.isPaused)
        return;
      this.copy.Clear();
      foreach (string key in this.Animations.Keys)
        this.copy.Add((IAnim) this.Animations[key]);
      foreach (IAnim anim in this.copy)
        anim.UpdateAnimation(gameTime);
    }

    public void RemoveAll() => this.Animations.Clear();

    public void RemoveAnim(string key) => this.Animations.Remove(key);

    public AnimStatus GetAnimationStatus(string key)
    {
      return this.Animations.ContainsKey(key) ? this.Animations[key].Status : AnimStatus.Stopped;
    }

    public void Start(string key)
    {
      if (!this.Animations.ContainsKey(key))
        return;
      if (this.Animations[key].Status == AnimStatus.Stopped)
      {
        this.Animations[key].Restart();
      }
      else
      {
        if (this.Animations[key].Status != AnimStatus.Paused)
          return;
        this.Animations[key].Play();
      }
    }

    public void Start(string key, Vector3 startvalue)
    {
      if (!this.Animations.ContainsKey(key))
        return;
      if (this.Animations[key].Status == AnimStatus.Stopped)
      {
        this.Animations[key].Restart();
      }
      else
      {
        if (this.Animations[key].Status != AnimStatus.Paused)
          return;
        this.Animations[key].Start(startvalue);
      }
    }

    public void Stop(string key)
    {
      if (!this.Animations.ContainsKey(key))
        return;
      this.Animations[key].Stop();
    }

    public void StopAndRemove(string key)
    {
      if (!this.Animations.ContainsKey(key))
        return;
      this.Animations[key].Stop();
      this.RemoveAnim(key);
    }

    public void Restart(string key)
    {
      if (!this.Animations.ContainsKey(key))
        return;
      this.Animations[key].Restart();
    }

    public void Pause(string key)
    {
      if (!this.Animations.ContainsKey(key))
        return;
      this.Animations[key].Pause();
    }

    public Vector2 GetCurrentV2Value(string key)
    {
      return this.Animations.ContainsKey(key) ? this.Animations[key].CurrentVector2Value : Vector2.Zero;
    }

    public float GetCurrentFloatValue(string key)
    {
      return this.Animations.ContainsKey(key) ? this.Animations[key].CurrentFloatValue : 0.0f;
    }
  }
}
