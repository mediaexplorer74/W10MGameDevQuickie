// Decompiled with JetBrains decompiler
// Type: GameEngine.ParticleQuad
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameEngine
{
  public class ParticleQuad
  {
    public Vector3 Origin;
    public VertexPositionNormalTexture[] Vertices;
    public short[] Indexes;
    public BasicEffect quadEffect;

    public ParticleQuad(
      Vector3 normal,
      Vector3 up,
      float width,
      float height,
      string texName,
      GameScreen screen)
    {
      this.Initialize(normal, up, width, height, texName, screen);
    }

    private void Initialize(
      Vector3 normal,
      Vector3 up,
      float width,
      float height,
      string texName,
      GameScreen screen)
    {
      this.Vertices = new VertexPositionNormalTexture[4];
      this.Indexes = new short[6];
      this.Origin = Vector3.Zero;
      Vector3 vector3_1 = normal;
      Vector3 vector2_1 = up;
      Vector3 vector3_2 = Vector3.Cross(normal, vector2_1);
      Vector3 vector3_3 = vector2_1 * height / 2f + this.Origin;
      Vector3 vector3_4 = vector3_3 + vector3_2 * width / 2f;
      Vector3 vector3_5 = vector3_3 - vector3_2 * width / 2f;
      Vector3 vector3_6 = vector3_4 - vector2_1 * height;
      Vector3 vector3_7 = vector3_5 - vector2_1 * height;
      this.quadEffect = new BasicEffect(Engine.gdm.GraphicsDevice);
      this.quadEffect.World = Matrix.CreateTranslation(Vector3.Zero);
      this.quadEffect.TextureEnabled = true;
      this.quadEffect.Texture = screen.textureManager.Load(texName);
      this.quadEffect.LightingEnabled = true;
      this.quadEffect.DirectionalLight0.Enabled = false;
      this.quadEffect.AmbientLightColor = Vector3.One;
      Vector2 vector2_2 = new Vector2(0.0f, 0.0f);
      Vector2 vector2_3 = new Vector2(1f, 0.0f);
      Vector2 vector2_4 = new Vector2(0.0f, 1f);
      Vector2 vector2_5 = new Vector2(1f, 1f);
      for (int index = 0; index < this.Vertices.Length; ++index)
        this.Vertices[index].Normal = vector3_1;
      this.Vertices[0].Position = vector3_6;
      this.Vertices[0].TextureCoordinate = vector2_4;
      this.Vertices[1].Position = vector3_4;
      this.Vertices[1].TextureCoordinate = vector2_2;
      this.Vertices[2].Position = vector3_7;
      this.Vertices[2].TextureCoordinate = vector2_5;
      this.Vertices[3].Position = vector3_5;
      this.Vertices[3].TextureCoordinate = vector2_3;
      this.Indexes[0] = (short) 0;
      this.Indexes[1] = (short) 1;
      this.Indexes[2] = (short) 2;
      this.Indexes[3] = (short) 2;
      this.Indexes[4] = (short) 1;
      this.Indexes[5] = (short) 3;
    }

    public void SetTexture(Texture2D t) => this.quadEffect.Texture = t;

    public void Draw(ICamera camera)
    {
      this.quadEffect.View = camera.ViewMatrix;
      this.quadEffect.Projection = camera.ProjectionMatrix;
      foreach (EffectPass pass in this.quadEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        Engine.gdm.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, this.Vertices, 0, 4, this.Indexes, 0, 2);
      }
    }
  }
}
