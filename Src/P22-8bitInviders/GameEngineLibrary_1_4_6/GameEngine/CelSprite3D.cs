// Decompiled with JetBrains decompiler
// Type: GameEngine.CelSprite3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class CelSprite3D : SceneNode
  {
    private Dictionary<string, CelClip3D> celClips;
    private string currentClipName;
    private BasicEffect basicEffect;
    private VertexPositionNormalTexture[] Vertices;
    private short[] Indexes;
    private Vector3 Normal;
    private string imageName;
    private Texture2D tex;
    private Matrix World;
    private Matrix WorldParentTransform;
    private Matrix parentTransform;
    protected float celWidth;
    protected float celHeight;
    protected CelClip3D currentClip;
    public Vector3 Rotation3D;
    public DepthStencilState depthStencilState = DepthStencilState.Default;
    public SamplerState samplerState = SamplerState.LinearClamp;
    public BlendState blendState = BlendState.Opaque;
    public bool isBillboard;
    public Vector3 AmbientColor;
    public UVFlip Facing;
    public float Opacity = 1f;

    public CelSprite3D(
      string name,
      Vector3 position,
      float unitwidth,
      Point celDimensions,
      string texName,
      GameScreen screen)
      : this(name, position, Vector3.Backward, Vector3.Up, unitwidth, celDimensions, texName, screen)
    {
    }

    public CelSprite3D(
      string name,
      Vector3 position,
      Vector3 normal,
      Vector3 up,
      float unitwidth,
      Point celDimensions,
      string texName,
      GameScreen screen)
      : base(name, screen)
    {
      this.celClips = new Dictionary<string, CelClip3D>();
      this.Position = position;
      this.Width = unitwidth;
      this.Normal = normal;
      this.Up = up;
      this.Pivot = Vector3.Zero;
      this.imageName = texName;
      this.celWidth = (float) celDimensions.X;
      this.celHeight = (float) celDimensions.Y;
      this.Initialize();
    }

    public override void Initialize()
    {
      this.basicEffect = new BasicEffect(Engine.gdm.GraphicsDevice);
      this.basicEffect.World = Matrix.CreateTranslation(Vector3.Zero);
      this.basicEffect.TextureEnabled = true;
      this.SetTexture(this.imageName);
      this.basicEffect.LightingEnabled = true;
      this.basicEffect.DirectionalLight0.Enabled = false;
      this.AmbientColor = Vector3.One;
      this.Height = this.celHeight / this.celWidth * this.Width;
      this.Vertices = new VertexPositionNormalTexture[4];
      this.Indexes = new short[6];
      Vector3 vector3_1 = Vector3.Cross(this.Normal, this.Up);
      Vector3 vector3_2 = this.Up * this.Height / 2f + this.Pivot;
      Vector3 vector3_3 = vector3_2 + vector3_1 * this.Width / 2f;
      Vector3 vector3_4 = vector3_2 - vector3_1 * this.Width / 2f;
      Vector3 vector3_5 = vector3_3 - this.Up * this.Height;
      Vector3 vector3_6 = vector3_4 - this.Up * this.Height;
      for (int index = 0; index < this.Vertices.Length; ++index)
        this.Vertices[index].Normal = this.Normal;
      this.Vertices[0].Position = vector3_5;
      this.Vertices[1].Position = vector3_3;
      this.Vertices[2].Position = vector3_6;
      this.Vertices[3].Position = vector3_4;
      this.Indexes[0] = (short) 0;
      this.Indexes[1] = (short) 1;
      this.Indexes[2] = (short) 2;
      this.Indexes[3] = (short) 2;
      this.Indexes[4] = (short) 1;
      this.Indexes[5] = (short) 3;
    }

    private void SetTexture(string image)
    {
      this.imageName = image;
      this.basicEffect.Texture = this.tex = this.Screen.textureManager.Load(this.imageName);
    }

    public void SetPivot(Vector3 pivot)
    {
      this.Pivot = pivot;
      Vector3 vector3_1 = Vector3.Cross(this.Normal, this.Up);
      Vector3 vector3_2 = this.Up * this.Height / 2f + this.Pivot;
      Vector3 vector3_3 = vector3_2 + vector3_1 * this.Width / 2f;
      Vector3 vector3_4 = vector3_2 - vector3_1 * this.Width / 2f;
      Vector3 vector3_5 = vector3_3 - this.Up * this.Height;
      Vector3 vector3_6 = vector3_4 - this.Up * this.Height;
      for (int index = 0; index < this.Vertices.Length; ++index)
        this.Vertices[index].Normal = this.Normal;
      this.Vertices[0].Position = vector3_5;
      this.Vertices[1].Position = vector3_3;
      this.Vertices[2].Position = vector3_6;
      this.Vertices[3].Position = vector3_4;
    }

    public void SetPivot(PivotLocation location)
    {
      Vector3 pivot = Vector3.Zero;
      switch (location)
      {
        case PivotLocation.TopLeft:
          pivot = new Vector3(this.Width / 2f, (float) (-(double) this.Height / 2.0), 0.0f);
          break;
        case PivotLocation.TopCenter:
          pivot = new Vector3(0.0f, (float) (-(double) this.Height / 2.0), 0.0f);
          break;
        case PivotLocation.TopRight:
          pivot = new Vector3((float) (-(double) this.Width / 2.0), (float) (-(double) this.Height / 2.0), 0.0f);
          break;
        case PivotLocation.LeftCenter:
          pivot = new Vector3(this.Width / 2f, 0.0f, 0.0f);
          break;
        case PivotLocation.RightCenter:
          pivot = new Vector3((float) (-(double) this.Width / 2.0), 0.0f, 0.0f);
          break;
        case PivotLocation.BottomLeft:
          pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
          break;
        case PivotLocation.BottomCenter:
          pivot = new Vector3(0.0f, this.Height / 2f, 0.0f);
          break;
        case PivotLocation.BottomRight:
          pivot = new Vector3((float) (-(double) this.Width / 2.0), this.Height / 2f, 0.0f);
          break;
      }
      this.SetPivot(pivot);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (this.isBillboard)
      {
        Vector3 translation;
        this.WorldParentTransform.Decompose(out Vector3 _, out Quaternion _, out translation);
        this.WorldParentTransform = Matrix.CreateScale(this.Scale) * Matrix.CreateWorld(translation, translation - ((Layer3D) this.Root).Camera.camPosition, this.Up);
      }
      else
      {
        this.parentTransform = worldTransform;
        this.World = Matrix.Identity * Matrix.CreateScale(this.Scale) * Matrix.CreateFromYawPitchRoll(this.Rotation3D.Y, this.Rotation3D.X, this.Rotation3D.Z) * Matrix.CreateTranslation(this.Position);
        this.WorldParentTransform = this.World * this.parentTransform;
      }
      this.UpdateAnimation(gameTime);
      base.Update(gameTime, ref this.WorldParentTransform);
    }

    public void ManualSetWorldMatrix(Matrix world) => this.WorldParentTransform = world;

    public void ManualUpdateAnimation(GameTime gt) => this.UpdateAnimation(gt);

    protected void UpdateAnimation(GameTime gameTime)
    {
      if (this.currentClip.clipStatus != ClipStatus.Playing)
        return;
      if (this.Facing == UVFlip.Normal)
      {
        this.Vertices[0].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv3;
        this.Vertices[1].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv1;
        this.Vertices[2].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv4;
        this.Vertices[3].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv2;
      }
      else
      {
        this.Vertices[0].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv4;
        this.Vertices[1].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv2;
        this.Vertices[2].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv3;
        this.Vertices[3].TextureCoordinate = this.currentClip.Frames[this.currentClip.currentFrame].uv1;
      }
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

    public void ManualDraw(ICamera camera)
    {
      this.basicEffect.Alpha = this.Opacity;
      this.basicEffect.AmbientLightColor = this.AmbientColor;
      this.basicEffect.View = camera.ViewMatrix;
      this.basicEffect.Projection = camera.ProjectionMatrix;
      this.basicEffect.World = this.WorldParentTransform;
      this.basicEffect.CurrentTechnique.Passes[0].Apply();
      Engine.gdm.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, this.Vertices, 0, 4, this.Indexes, 0, 2);
    }

    public override void Draw(ICamera camera)
    {
      if (!this.Visible || this.Delete)
        return;
      base.Draw(camera);
      Engine.gdm.GraphicsDevice.DepthStencilState = this.depthStencilState;
      Engine.gdm.GraphicsDevice.BlendState = this.blendState;
      Engine.gdm.GraphicsDevice.SamplerStates[0] = this.samplerState;
      this.basicEffect.Alpha = this.Opacity;
      this.basicEffect.AmbientLightColor = this.AmbientColor;
      this.basicEffect.View = camera.ViewMatrix;
      this.basicEffect.Projection = camera.ProjectionMatrix;
      this.basicEffect.World = this.WorldParentTransform;
      this.basicEffect.CurrentTechnique.Passes[0].Apply();
      Engine.gdm.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, this.Vertices, 0, 4, this.Indexes, 0, 2);
    }

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
      float num1 = this.celHeight / this.celWidth;
      float num2 = 1f / ((float) clipwidth / this.celWidth);
      float num3 = num2 * num1 * (float) this.tex.Width / (float) this.tex.Height;
      CelClip3D celClip3D = new CelClip3D()
      {
        Name = name,
        frameInterval = TimeSpan.FromSeconds(1.0 / (double) speed),
        FrameCount = framecount,
        currentFrame = 0,
        AnimationType = animtype,
        celWidth = num2,
        celHeight = num3,
        playDirection = 1
      };
      celClip3D.Frames = new UVRect[framecount];
      float num4 = (float) startX;
      float num5 = (float) startY;
      float x = Tools.RemapValue(num4, 0.0f, (float) this.tex.Width, 0.0f, 1f);
      float y = Tools.RemapValue(num5, 0.0f, (float) this.tex.Height, 0.0f, 1f);
      float num6 = x;
      for (int index = 0; index < framecount; ++index)
      {
        celClip3D.Frames[index] = new UVRect(new Vector2(x, y), new Vector2(x + num2, y), new Vector2(x, y + num3), new Vector2(x + num2, y + num3));
        num4 += this.celWidth;
        x += num2;
        if ((double) num4 >= (double) (startX + clipwidth))
        {
          num4 = (float) startX;
          num5 += this.celHeight;
          x = num6;
          y += num3;
        }
      }
      if (string.IsNullOrEmpty(this.currentClipName))
      {
        this.currentClipName = name;
        this.currentClip = celClip3D;
      }
      this.celClips.Add(name, celClip3D);
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
      float num1 = (float) celDimensions.Y / (float) celDimensions.X;
      float num2 = 1f / ((float) clipwidth / this.celWidth);
      float num3 = num2 * num1 * (float) this.tex.Width / (float) this.tex.Height;
      CelClip3D celClip3D = new CelClip3D()
      {
        Name = name,
        frameInterval = TimeSpan.FromSeconds(1.0 / (double) speed),
        FrameCount = framecount,
        currentFrame = 0,
        AnimationType = animtype,
        celWidth = num2,
        celHeight = num3,
        playDirection = 1
      };
      celClip3D.Frames = new UVRect[framecount];
      float num4 = (float) startX;
      float num5 = (float) startY;
      float x = Tools.RemapValue(num4, 0.0f, (float) this.tex.Width, 0.0f, 1f);
      float y = Tools.RemapValue(num5, 0.0f, (float) this.tex.Height, 0.0f, 1f);
      float num6 = x;
      for (int index = 0; index < framecount; ++index)
      {
        celClip3D.Frames[index] = new UVRect(new Vector2(x, y), new Vector2(x + num2, y), new Vector2(x, y + num3), new Vector2(x + num2, y + num3));
        num4 += (float) celDimensions.X;
        x += num2;
        if ((double) num4 >= (double) (startX + clipwidth))
        {
          num4 = (float) startX;
          num5 += (float) celDimensions.Y;
          x = num6;
          y += num3;
        }
      }
      if (string.IsNullOrEmpty(this.currentClipName))
      {
        this.currentClipName = name;
        this.currentClip = celClip3D;
      }
      this.celClips.Add(name, celClip3D);
    }

    public void Play(string name, int fromFrame)
    {
      if (!this.celClips.ContainsKey(name))
        return;
      this.currentClipName = name;
      this.celClips[this.currentClipName].clipStatus = ClipStatus.Stopped;
      this.celClips[name].clipStatus = ClipStatus.Playing;
      this.currentClip = this.celClips[name];
      if (fromFrame < this.currentClip.FrameCount)
        this.currentClip.currentFrame = fromFrame;
      else
        this.currentClip.currentFrame = 0;
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

    public void Play(int fromFrame) => this.Play(this.currentClipName, fromFrame);

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

    public int CurrentFrame() => this.currentClip.currentFrame;
  }
}
