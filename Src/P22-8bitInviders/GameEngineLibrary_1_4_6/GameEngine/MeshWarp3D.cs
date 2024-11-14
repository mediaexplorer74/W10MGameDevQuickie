// Decompiled with JetBrains decompiler
// Type: GameEngine.MeshWarp3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace GameEngine
{
  public class MeshWarp3D : SceneNode
  {
    protected string imageName;
    protected VertexPositionNormalTexture[] Vertices;
    protected BasicEffect basicEffect;
    protected Texture2D tex;
    public DepthStencilState depthStencilState = DepthStencilState.Default;
    public SamplerState samplerState = SamplerState.LinearClamp;
    public BlendState blendState = BlendState.Opaque;
    public Vector3 AmbientColor;

    protected short[] Indices { get; set; }

    protected int gridWidth { get; set; }

    protected int gridHeight { get; set; }

    protected Vector2 CellSize { get; set; }

    public MeshWarp3D(
      string name,
      int columns,
      int rows,
      Vector2 cellSize,
      Vector3 position,
      string imgName,
      GameScreen screen)
      : base(name, position, screen)
    {
      this.BuildGeometry(columns, rows, cellSize);
      this.basicEffect = new BasicEffect(Engine.gdm.GraphicsDevice);
      this.basicEffect.TextureEnabled = true;
      this.imageName = imgName;
      this.SetTexture(imgName);
      this.basicEffect.LightingEnabled = true;
      this.basicEffect.DirectionalLight0.Enabled = false;
      this.AmbientColor = Vector3.One;
    }

    public void SetTexture(string imageName)
    {
      this.tex = this.Screen.textureManager.Load(imageName);
      this.basicEffect.Texture = this.tex;
    }

    public void BuildGeometry(int colums, int rows, Vector2 cellSize)
    {
      this.gridWidth = colums;
      this.gridHeight = rows;
      this.CellSize = cellSize;
      this.Vertices = new VertexPositionNormalTexture[(this.gridWidth + 1) * (this.gridHeight + 1)];
      this.Indices = new short[this.gridWidth * this.gridHeight * 6];
      float num1 = (float) this.gridHeight / 2f;
      float x = (float) -this.gridWidth / 2f;
      for (int index1 = 0; index1 < this.gridWidth + 1; ++index1)
      {
        float y = (float) this.gridHeight / 2f;
        for (int index2 = 0; index2 < this.gridHeight + 1; ++index2)
        {
          int index3 = index2 * (this.gridWidth + 1) + index1;
          VertexPositionNormalTexture positionNormalTexture = new VertexPositionNormalTexture()
          {
            Position = new Vector3(new Vector2(x, y) * this.CellSize, 0.0f),
            Normal = Vector3.Backward,
            TextureCoordinate = this.GetDefaultUV(index3)
          };
          --y;
          this.Vertices[index3] = positionNormalTexture;
        }
        ++x;
      }
      int index4 = 0;
      for (int index5 = 0; index5 < this.gridWidth; ++index5)
      {
        for (int index6 = 0; index6 < this.gridHeight; ++index6)
        {
          int num2 = index6 * (this.gridWidth + 1) + index5;
          int num3 = index6 * (this.gridWidth + 1) + index5 + 1;
          int num4 = (index6 + 1) * (this.gridWidth + 1) + index5;
          int num5 = (index6 + 1) * (this.gridWidth + 1) + index5 + 1;
          this.Indices[index4] = (short) num2;
          this.Indices[index4 + 1] = (short) num3;
          this.Indices[index4 + 2] = (short) num4;
          this.Indices[index4 + 3] = (short) num4;
          this.Indices[index4 + 4] = (short) num3;
          this.Indices[index4 + 5] = (short) num5;
          index4 += 6;
        }
      }
    }

    public Vector2 GetUV0(int index) => this.Vertices[index].TextureCoordinate;

    public void SetUV0(int index, Vector2 value) => this.Vertices[index].TextureCoordinate = value;

    public Vector2 GetDefaultUV(int index)
    {
      return new Vector2((float) (index % (this.gridWidth + 1)) / (float) this.gridWidth, (float) (index / (this.gridWidth + 1)) / (float) this.gridHeight);
    }

    public void ResetUVs()
    {
      for (int index = 0; index < this.Vertices.Length; ++index)
      {
        VertexPositionNormalTexture vertex = this.Vertices[index] with
        {
          TextureCoordinate = this.GetDefaultUV(index)
        };
        this.Vertices[index] = vertex;
      }
    }

    public void TintTexture(Color tint, int strength, MixMethod method)
    {
      this.basicEffect.Texture = Tools.TintColor(this.tex, tint, strength, method);
    }

    public void RestoreTexture() => this.SetTexture(this.imageName);

    public void ApplyPinch(Vector2 center, float intensity)
    {
      Vector2 vector2_1 = new Vector2((float) this.gridWidth, (float) this.gridHeight) * this.CellSize;
      for (int index = 0; index < this.Vertices.Length; ++index)
      {
        Vector2 vector2_2 = this.GetUV0(index) * vector2_1;
        Vector2 vector2_3 = vector2_2 - center;
        float val2 = -vector2_3.Length();
        if ((double) vector2_3.Length() != 0.0)
          vector2_3.Normalize();
        Vector2 vector2_4 = vector2_3 * Math.Max(intensity, val2);
        Vector2 vector2_5 = vector2_2 + vector2_4;
        this.SetUV0(index, vector2_5 / vector2_1);
      }
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.ResetUVs();
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw(ICamera camera)
    {
      base.Draw(camera);
      if (!this.Visible || this.Delete)
        return;
      Engine.gdm.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
      Engine.gdm.GraphicsDevice.DepthStencilState = this.depthStencilState;
      Engine.gdm.GraphicsDevice.BlendState = this.blendState;
      Engine.gdm.GraphicsDevice.SamplerStates[0] = this.samplerState;
      this.basicEffect.AmbientLightColor = this.AmbientColor;
      this.basicEffect.View = camera.ViewMatrix;
      this.basicEffect.Projection = camera.ProjectionMatrix;
      this.basicEffect.World = Matrix.Identity * Matrix.CreateScale(this.Scale) * Matrix.CreateRotationZ(MathHelper.ToRadians(this.Rotation)) * Matrix.CreateTranslation(this.Position);
      this.basicEffect.CurrentTechnique.Passes[0].Apply();
      Engine.gdm.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, this.Vertices, 0, this.Vertices.Length, this.Indices, 0, this.Indices.Length / 3);
    }
  }
}
