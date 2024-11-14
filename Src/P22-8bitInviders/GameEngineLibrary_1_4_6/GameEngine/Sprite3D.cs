// Decompiled with JetBrains decompiler
// Type: GameEngine.Sprite3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class Sprite3D : SceneNode
  {
    private VertexPositionNormalTexture[] Vertices;
    private short[] Indexes;
    private Vector3 Normal;
    private string imageName;
    private Texture2D tex;
    private Matrix World;
    private Matrix WorldParentTransform;
    private Matrix parentTransform;
    private UVFlip _Facing;
    private Vector2 textureUpperLeft;
    private Vector2 textureUpperRight;
    private Vector2 textureLowerLeft;
    private Vector2 textureLowerRight;
    public Vector3 Rotation3D;
    public BasicEffect basicEffect;
    public DepthStencilState depthStencilState = DepthStencilState.Default;
    public SamplerState samplerState = SamplerState.LinearClamp;
    public BlendState blendState = BlendState.Opaque;
    public RasterizerState rasterState = RasterizerState.CullCounterClockwise;
    public float Opacity = 1f;
    public bool isBillboard;
    public Vector3 AmbientColor;
    public Vector3 DiffuseColor = Vector3.Zero;

    public UVFlip Facing
    {
      get => this._Facing;
      set
      {
        if (value == UVFlip.Normal)
        {
          this.Vertices[0].TextureCoordinate = this.textureLowerLeft;
          this.Vertices[1].TextureCoordinate = this.textureUpperLeft;
          this.Vertices[2].TextureCoordinate = this.textureLowerRight;
          this.Vertices[3].TextureCoordinate = this.textureUpperRight;
        }
        else
        {
          this.Vertices[0].TextureCoordinate = this.textureLowerRight;
          this.Vertices[1].TextureCoordinate = this.textureUpperRight;
          this.Vertices[2].TextureCoordinate = this.textureLowerLeft;
          this.Vertices[3].TextureCoordinate = this.textureUpperLeft;
        }
      }
    }

    public Sprite3D(
      string name,
      Vector3 position,
      float width,
      float height,
      string texName,
      GameScreen screen)
      : this(name, position, Vector3.Backward, Vector3.Up, width, height, texName, screen)
    {
    }

    public Sprite3D(
      string name,
      Vector3 position,
      Vector3 normal,
      Vector3 up,
      float width,
      float height,
      string texName,
      GameScreen screen)
      : base(name, screen)
    {
      this.Position = position;
      this.Width = width;
      this.Height = height;
      this.Normal = normal;
      this.Up = up;
      this.Pivot = Vector3.Zero;
      this.imageName = texName;
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
      this.Vertices = new VertexPositionNormalTexture[4];
      this.Indexes = new short[6];
      Vector3 vector3_1 = Vector3.Cross(this.Normal, this.Up);
      Vector3 vector3_2 = this.Up * this.Height / 2f + this.Pivot;
      Vector3 vector3_3 = vector3_2 + vector3_1 * this.Width / 2f;
      Vector3 vector3_4 = vector3_2 - vector3_1 * this.Width / 2f;
      Vector3 vector3_5 = vector3_3 - this.Up * this.Height;
      Vector3 vector3_6 = vector3_4 - this.Up * this.Height;
      this.textureUpperLeft = new Vector2(0.0f, 0.0f);
      this.textureUpperRight = new Vector2(1f, 0.0f);
      this.textureLowerLeft = new Vector2(0.0f, 1f);
      this.textureLowerRight = new Vector2(1f, 1f);
      for (int index = 0; index < this.Vertices.Length; ++index)
        this.Vertices[index].Normal = this.Normal;
      this.Vertices[0].Position = vector3_5;
      this.Vertices[0].TextureCoordinate = this.textureLowerLeft;
      this.Vertices[1].Position = vector3_3;
      this.Vertices[1].TextureCoordinate = this.textureUpperLeft;
      this.Vertices[2].Position = vector3_6;
      this.Vertices[2].TextureCoordinate = this.textureLowerRight;
      this.Vertices[3].Position = vector3_4;
      this.Vertices[3].TextureCoordinate = this.textureUpperRight;
      this.Indexes[0] = (short) 0;
      this.Indexes[1] = (short) 1;
      this.Indexes[2] = (short) 2;
      this.Indexes[3] = (short) 2;
      this.Indexes[4] = (short) 1;
      this.Indexes[5] = (short) 3;
    }

    public void SetUVRepeat(float uScale, float vScale)
    {
      this.Vertices[0].TextureCoordinate.X *= uScale;
      this.Vertices[0].TextureCoordinate.Y *= vScale;
      this.Vertices[1].TextureCoordinate.X *= uScale;
      this.Vertices[1].TextureCoordinate.Y *= vScale;
      this.Vertices[2].TextureCoordinate.X *= uScale;
      this.Vertices[2].TextureCoordinate.Y *= vScale;
      this.Vertices[3].TextureCoordinate.X *= uScale;
      this.Vertices[3].TextureCoordinate.Y *= vScale;
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
      Vector3 vector3_1 = Vector3.Cross(this.Normal, this.Up);
      Vector3 vector3_2 = this.Up * this.Height / 2f;
      Vector3 vector3_3 = vector3_2 + vector3_1 * this.Width / 2f;
      Vector3 vector3_4 = vector3_2 - vector3_1 * this.Width / 2f;
      Vector3 vector3_5 = vector3_3 - this.Up * this.Height;
      Vector3 vector3_6 = vector3_4 - this.Up * this.Height;
      Vector3 zero = Vector3.Zero;
      if (this.Up == Vector3.Forward)
      {
        zero.X = 0.0f;
        zero.Y = 0.0f;
        zero.Z = (float) (-(double) this.Height / 2.0);
      }
      else if (this.Up == Vector3.Up)
      {
        zero.X = 0.0f;
        zero.Y = this.Height / 2f;
        zero.Z = 0.0f;
      }
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
          pivot = new Vector3(0.0f, zero.Y, zero.Z);
          break;
        case PivotLocation.BottomRight:
          pivot = new Vector3((float) (-(double) this.Width / 2.0), this.Height / 2f, 0.0f);
          break;
      }
      this.SetPivot(pivot);
    }

    public void TranslateUV(float uAmount, float vAmount)
    {
      this.Vertices[0].TextureCoordinate.X += uAmount;
      this.Vertices[0].TextureCoordinate.Y += vAmount;
      this.Vertices[1].TextureCoordinate.X += uAmount;
      this.Vertices[1].TextureCoordinate.Y += vAmount;
      this.Vertices[2].TextureCoordinate.X += uAmount;
      this.Vertices[2].TextureCoordinate.Y += vAmount;
      this.Vertices[3].TextureCoordinate.X += uAmount;
      this.Vertices[3].TextureCoordinate.Y += vAmount;
    }

    public void SetUVCoords(Vector2 TL, Vector2 TR, Vector2 BL, Vector2 BR)
    {
      this.Vertices[0].TextureCoordinate.X = BL.X;
      this.Vertices[0].TextureCoordinate.Y = BL.Y;
      this.Vertices[1].TextureCoordinate.X = TL.X;
      this.Vertices[1].TextureCoordinate.Y = TL.Y;
      this.Vertices[2].TextureCoordinate.X = BR.X;
      this.Vertices[2].TextureCoordinate.Y = BR.Y;
      this.Vertices[3].TextureCoordinate.X = TR.X;
      this.Vertices[3].TextureCoordinate.Y = TR.Y;
    }

    public void RestoreTexture() => this.SetTexture(this.imageName);

    public void SetTexture(string image)
    {
      this.imageName = image;
      if (image.Contains("JPG"))
        this.basicEffect.Texture = this.tex = this.Screen.textureManager.LoadJPG(this.imageName);
      else
        this.basicEffect.Texture = this.tex = this.Screen.textureManager.Load(this.imageName);
    }

    public override void SetSourceRect()
    {
      Rectangle spriteSource = this.Screen.GetSpriteSource(this.Name);
      if (spriteSource != Rectangle.Empty)
      {
        float num1 = (float) spriteSource.Height / (float) spriteSource.Width;
        float num2 = 1f / ((float) this.tex.Width / (float) spriteSource.Width);
        float num3 = num2 * num1 * (float) this.tex.Width / (float) this.tex.Height;
        float x1 = (float) spriteSource.X;
        float y1 = (float) spriteSource.Y;
        float x2 = Tools.RemapValue(x1, 0.0f, (float) this.tex.Width, 0.0f, 1f);
        float y2 = Tools.RemapValue(y1, 0.0f, (float) this.tex.Height, 0.0f, 1f);
        this.SetUVCoords(new Vector2(x2, y2), new Vector2(x2 + num2, y2), new Vector2(x2, y2 + num3), new Vector2(x2 + num2, y2 + num3));
      }
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
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
      base.Update(gameTime, ref this.WorldParentTransform);
    }

    public override void Draw(ICamera camera)
    {
      if (!this.Visible || this.Delete)
        return;
      base.Draw(camera);
      Engine.gdm.GraphicsDevice.DepthStencilState = this.depthStencilState;
      Engine.gdm.GraphicsDevice.BlendState = this.blendState;
      Engine.gdm.GraphicsDevice.SamplerStates[0] = this.samplerState;
      Engine.gdm.GraphicsDevice.RasterizerState = this.rasterState;
      this.basicEffect.Alpha = this.Opacity;
      this.basicEffect.AmbientLightColor = this.AmbientColor;
      this.basicEffect.DiffuseColor = this.DiffuseColor;
      this.basicEffect.View = camera.ViewMatrix;
      this.basicEffect.Projection = camera.ProjectionMatrix;
      this.basicEffect.World = this.WorldParentTransform;
      this.basicEffect.CurrentTechnique.Passes[0].Apply();
      Engine.gdm.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, this.Vertices, 0, 4, this.Indexes, 0, 2);
    }
  }
}
