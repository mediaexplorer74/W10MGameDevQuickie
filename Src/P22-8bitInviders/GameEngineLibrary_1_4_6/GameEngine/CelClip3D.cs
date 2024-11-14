// Decompiled with JetBrains decompiler
// Type: GameEngine.CelClip3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public class CelClip3D
  {
    public string Name;
    public ClipStatus clipStatus = ClipStatus.Stopped;
    public CelAnimType AnimationType;
    public TimeSpan frameInterval;
    public UVRect[] Frames;
    public TimeSpan nextFrame;
    public int currentFrame;
    public int FrameCount;
    public float celWidth;
    public float celHeight;
    public Vector2 Pivot;
    public int frameIncrement = 1;
    public int playDirection;
  }
}
