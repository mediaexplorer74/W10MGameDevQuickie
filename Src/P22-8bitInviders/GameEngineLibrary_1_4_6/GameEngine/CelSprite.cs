// Decompiled with JetBrains decompiler
// Type: GameEngine.CelSprite
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class CelSprite : SceneNode
  {
    private Dictionary<string, CelClip> celClips;
    private string contentName;
    private string currentClipName;
    protected Texture2D tex;
    protected int celWidth;
    protected int celHeight;
    protected CelClip currentClip;
    public float Opacity = 1f;
    public Color color;
    public SpriteEffects Facing;
    public float layerDepth;

    public CelClip CurrentClip => this.currentClip;

    public Rectangle Bounds
    {
      get
      {
        return this.Pivot == Vector3.Zero ? new Rectangle((int) this.Position.X, (int) this.Position.Y, (int) this.Width, (int) this.Height) : new Rectangle((int) ((double) this.Position.X - (double) this.currentClip.Pivot.X * (double) this.Scale), (int) ((double) this.Position.Y - (double) this.currentClip.Pivot.Y * (double) this.Scale), (int) this.Width, (int) this.Height);
      }
    }

    public override float Width
    {
      get
      {
        return this.currentClip != null ? (float) this.currentClip.celWidth * this.Scale : (float) this.celWidth * this.Scale;
      }
      set
      {
      }
    }

    public override float Height
    {
      get
      {
        return this.currentClip != null ? (float) this.currentClip.celHeight * this.Scale : (float) this.celHeight * this.Scale;
      }
      set
      {
      }
    }

    public CelSprite(
      string name,
      Vector2 position,
      Point celDimensions,
      string imageName,
      GameScreen screen)
      : base(name, position, screen)
    {
      this.Facing = SpriteEffects.None;
      this.celClips = new Dictionary<string, CelClip>();
      this.contentName = imageName;
      this.color = Color.White;
      this.celWidth = celDimensions.X;
      this.celHeight = celDimensions.Y;
      this.Initialize();
    }

    public override void Initialize() => this.SetTexture(this.contentName);

    public void SetTexture(string imageName)
    {
      this.tex = this.Screen.textureManager.Load(imageName);
      this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
    }

    public Texture2D GetTexture() => this.tex;

    public void AddClip(
      string name,
      int speed,
      int startX,
      int startY,
      int framecount,
      CelAnimType animtype)
    {
      this.AddClip(name, speed, startX, startY, framecount, animtype, this.tex.Width);
    }

    public void AddClip(
      string name,
      int speed,
      int startX,
      int startY,
      Point celDimensions,
      int framecount,
      CelAnimType animtype)
    {
      this.AddClip(name, speed, startX, startY, celDimensions, framecount, animtype, this.tex.Width);
    }

    public void AddClip(
      string name,
      int speed,
      int startX,
      int startY,
      int framecount,
      CelAnimType animtype,
      int clipwidth)
    {
      CelClip celClip = new CelClip()
      {
        Name = name,
        frameInterval = TimeSpan.FromSeconds(1.0 / (double) speed),
        FrameCount = framecount,
        currentFrame = 0,
        AnimationType = animtype,
        celWidth = this.celWidth,
        celHeight = this.celHeight,
        Pivot = new Vector2((float) (this.celWidth / 2), (float) (this.celHeight / 2)),
        playDirection = 1
      };
      celClip.Frames = new Rectangle[framecount];
      int x = startX;
      int y = startY;
      for (int index = 0; index < framecount; ++index)
      {
        celClip.Frames[index] = new Rectangle(x, y, this.celWidth, this.celHeight);
        x += this.celWidth;
        if (x >= startX + clipwidth)
        {
          x = startX;
          y += this.celHeight;
        }
      }
      if (string.IsNullOrEmpty(this.currentClipName))
      {
        this.currentClipName = name;
        this.currentClip = celClip;
      }
      this.celClips.Add(name, celClip);
    }

    public void AddClip(
      string name,
      int speed,
      int startX,
      int startY,
      Point celDimensions,
      int framecount,
      CelAnimType animtype,
      int clipwidth)
    {
      CelClip celClip = new CelClip()
      {
        Name = name,
        frameInterval = TimeSpan.FromSeconds(1.0 / (double) speed),
        FrameCount = framecount,
        currentFrame = 0,
        AnimationType = animtype,
        celWidth = celDimensions.X,
        celHeight = celDimensions.Y,
        Pivot = new Vector2((float) (this.celWidth / 2), (float) (this.celHeight / 2)),
        playDirection = 1
      };
      celClip.Frames = new Rectangle[framecount];
      int x = startX;
      int y = startY;
      for (int index = 0; index < framecount; ++index)
      {
        celClip.Frames[index] = new Rectangle(x, y, celClip.celWidth, celClip.celHeight);
        x += celClip.celWidth;
        if (x >= startX + clipwidth)
        {
          x = startX;
          y += celClip.celHeight;
        }
      }
      if (string.IsNullOrEmpty(this.currentClipName))
      {
        this.currentClipName = name;
        this.currentClip = celClip;
      }
      this.celClips.Add(name, celClip);
    }

    public void Play(string name)
    {
      if (!this.celClips.ContainsKey(name))
        return;
      this.celClips[this.currentClipName].clipStatus = ClipStatus.Stopped;
      this.currentClipName = name;
      this.celClips[name].clipStatus = ClipStatus.Playing;
      this.currentClip = this.celClips[name];
    }

    public void Pause(string name)
    {
      if (!this.celClips.ContainsKey(name))
        return;
      this.currentClipName = name;
      if (this.celClips[name].clipStatus == ClipStatus.Playing)
      {
        this.celClips[name].clipStatus = ClipStatus.Paused;
      }
      else
      {
        if (this.celClips[name].clipStatus != ClipStatus.Paused)
          return;
        this.celClips[name].clipStatus = ClipStatus.Playing;
      }
    }

    public void Stop(string name)
    {
      if (!this.celClips.ContainsKey(name))
        return;
      this.currentClipName = name;
      this.celClips[name].clipStatus = ClipStatus.Stopped;
      this.celClips[name].nextFrame = TimeSpan.Zero;
      this.celClips[name].currentFrame = 0;
    }

    public void Play() => this.Play(this.currentClipName);

    public void Pause() => this.Pause(this.currentClipName);

    public void Stop() => this.Stop(this.currentClipName);

    public void SetFrame(string clipname, int frame)
    {
      if (!this.celClips.ContainsKey(clipname))
        return;
      this.celClips[this.currentClipName].clipStatus = ClipStatus.Stopped;
      this.currentClipName = clipname;
      this.celClips[clipname].clipStatus = ClipStatus.Paused;
      this.celClips[clipname].currentFrame = frame > this.celClips[clipname].Frames.Length ? 0 : frame;
      this.currentClip = this.celClips[clipname];
    }

    public void SetPivot(string clipname, Vector2 pivot)
    {
      if (!this.celClips.ContainsKey(clipname))
        return;
      this.celClips[clipname].Pivot = pivot;
    }

    public void SetSpeed(string clipname, int speed)
    {
      if (!this.celClips.ContainsKey(clipname))
        return;
      this.celClips[clipname].frameInterval = TimeSpan.FromSeconds(1.0 / (double) speed);
    }

    public void SetPlayDirection(string clipname, PlayDirection dir)
    {
      if (!this.celClips.ContainsKey(clipname))
        return;
      this.celClips[clipname].playDirection = dir == PlayDirection.Forward ? 1 : -1;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (this.currentClip.clipStatus != ClipStatus.Playing)
        return;
      if (this.currentClip.nextFrame >= this.currentClip.frameInterval)
      {
        this.currentClip.currentFrame += this.currentClip.frameIncrement * this.currentClip.playDirection;
        if (this.currentClip.AnimationType == CelAnimType.Loop)
        {
          if (this.currentClip.currentFrame >= this.currentClip.FrameCount)
            this.currentClip.currentFrame = 0;
        }
        else if (this.currentClip.AnimationType == CelAnimType.Bounce)
        {
          if (this.currentClip.currentFrame >= this.currentClip.FrameCount)
          {
            this.currentClip.frameIncrement = -1;
            this.currentClip.currentFrame = this.currentClip.FrameCount - 2;
          }
          else if (this.currentClip.currentFrame <= 0)
          {
            this.currentClip.frameIncrement = 1;
            this.currentClip.currentFrame = 0;
          }
        }
        else if (this.currentClip.AnimationType == CelAnimType.PlayOnce)
        {
          if (this.currentClip.currentFrame >= this.currentClip.FrameCount)
            this.currentClip.currentFrame = this.currentClip.FrameCount - 1;
          else if (this.currentClip.currentFrame < 0)
            this.currentClip.currentFrame = 0;
        }
        this.currentClip.nextFrame = TimeSpan.Zero;
      }
      else
        this.currentClip.nextFrame += gameTime.ElapsedGameTime;
      this.currentClip.currentFrame = (int) MathHelper.Clamp((float) this.currentClip.currentFrame, 0.0f, (float) this.currentClip.Frames.Length);
    }

    public override void Draw()
    {
      if (!this.Visible || this.Delete)
        return;

      this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred/*.BackToFront*/, null,
        null, null, null, null, Game1.globalTransformation);

      this.Screen.spriteBatch.Draw(this.tex, this.Position.ToVector2(), 
          new Rectangle?(this.currentClip.Frames[this.currentClip.currentFrame]),
          this.color * this.Opacity, MathHelper.ToRadians(this.Rotation),
          this.currentClip.Pivot, this.Scale, this.Facing, this.layerDepth);

            this.Screen.spriteBatch.End();
    }
  }
}
