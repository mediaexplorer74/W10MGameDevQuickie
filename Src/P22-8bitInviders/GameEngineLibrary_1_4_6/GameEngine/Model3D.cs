// Decompiled with JetBrains decompiler
// Type: GameEngine.Model3D
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
  public class Model3D : SceneNode
  {
    private string assetName;
    protected Model Model;
    protected Matrix World;
    public Matrix WorldParentTransform;
    public Matrix parentTransform;
    public BoundingBox boundingBox;
    protected Matrix[] boneTransforms;
    private Dictionary<string, int> PartsIndex;
    protected Matrix[] originalTransforms;
    protected Matrix[] partTransforms;
    protected ModelBone[] modelBones;
    public Vector3 Rotation3D;
    public Vector3 Scale3D = Vector3.One;
    public Light Light0;
    public Light Light1;
    public Light Light2;
    public Vector3 AmbientColor;
    public Vector3 DiffuseColor;
    public bool LightingEnabled;
    public bool PerPixelLighting;
    public float specPower = 20f;
    public Vector3 specColor = Vector3.One;
    public DepthStencilState depthStencilState = DepthStencilState.Default;
    public SamplerState samplerState = SamplerState.LinearClamp;
    public BlendState blendState = BlendState.Opaque;
    public RasterizerState rasterizerState = RasterizerState.CullCounterClockwise;
    public float Alpha = 1f;

    public Model meshModel => this.Model;

    public void CalculateBoundingBox()
    {
      Matrix matrix = Matrix.Identity * Matrix.CreateScale(this.Scale3D) * Matrix.CreateFromYawPitchRoll(this.Rotation3D.Y, this.Rotation3D.X, this.Rotation3D.Z) * Matrix.CreateTranslation(this.Position);
      Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (ModelMeshPart meshPart in mesh.MeshParts)
        {
          int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
          int num = meshPart.NumVertices * vertexStride;
          float[] data = new float[num / 4];
          meshPart.VertexBuffer.GetData<float>(data);
          for (int index = 0; index < num / 4; index += vertexStride / 4)
          {
            Vector3 vector3 = Vector3.Transform(new Vector3(data[index], data[index + 1], data[index + 2]), matrix);
            min = Vector3.Min(min, vector3);
            max = Vector3.Max(max, vector3);
          }
        }
      }
      this.boundingBox = new BoundingBox(min, max);
    }

    public Matrix this[string key]
    {
      get
      {
        return this.PartsIndex.ContainsKey(key) ? this.partTransforms[this.PartsIndex[key]] : Matrix.Identity;
      }
      set
      {
        if (!this.PartsIndex.ContainsKey(key))
          return;
        this.partTransforms[this.PartsIndex[key]] = value;
      }
    }

    public float Scale
    {
      get => this.Scale3D.X;
      set => this.Scale3D = new Vector3(value);
    }

    public Model3D(string name, Vector3 position, string asset, GameScreen screen)
      : base(name, position, screen)
    {
      this.assetName = asset;
      this.LightingEnabled = true;
      this.PerPixelLighting = false;
      this.AmbientColor = new Vector3(0.3f, 0.3f, 0.3f);
      this.DiffuseColor = new Vector3(0.5f);
      this.Light0.Enabled = true;
      this.Light1.Enabled = true;
      this.Light2.Enabled = true;
      this.Light0.DiffuseColor = this.Light1.DiffuseColor = this.Light2.DiffuseColor = Vector3.One;
      this.Light0.SpecColor = this.Light1.SpecColor = this.Light2.SpecColor = Vector3.Zero;
      this.Light0.Direction = this.Light1.Direction = this.Light2.Direction = Vector3.Down;
      this.Light0.Position = this.Light1.Position = this.Light2.Position = new Vector3(this.Position.X, position.Y + 30f, position.Z);
      this.Light0.Direction = new Vector3(-0.52f, -0.57f, -0.62f);
      this.Light1.Direction = new Vector3(0.71f, 0.34f, 0.6f);
      this.Light2.Direction = new Vector3(0.45f, -0.76f, 0.45f);
      this.Initialize();
    }

    public override void Initialize()
    {
      this.Model = this.Screen.content.Load<Model>(this.assetName);
      this.PartsIndex = new Dictionary<string, int>();
      this.partTransforms = new Matrix[this.Model.Bones.Count];
      this.originalTransforms = new Matrix[this.Model.Bones.Count];
      this.modelBones = new ModelBone[this.Model.Bones.Count];
      this.boneTransforms = new Matrix[this.Model.Bones.Count];
      for (int index = 0; index < this.Model.Bones.Count; ++index)
      {
        this.modelBones[index] = this.Model.Bones[index];
        this.originalTransforms[index] = this.Model.Bones[index].Transform;
        this.partTransforms[index] = Matrix.Identity;
        if (!this.PartsIndex.ContainsKey(this.Model.Bones[index].Name))
          this.PartsIndex.Add(this.Model.Bones[index].Name, index);
        else
          Tools.Trace("Model3D: PartsIndex already contains: " + this.Model.Bones[index].Name);
      }
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      this.parentTransform = worldTransform;
      this.World = Matrix.Identity * Matrix.CreateScale(this.Scale3D) * Matrix.CreateFromYawPitchRoll(this.Rotation3D.Y, this.Rotation3D.X, this.Rotation3D.Z) * Matrix.CreateTranslation(this.Position);
      this.WorldParentTransform = this.World * this.parentTransform;
      base.Update(gameTime, ref this.WorldParentTransform);
    }

    protected void SetLighting(BasicEffect effect)
    {
      effect.DirectionalLight0.Enabled = this.Light0.Enabled;
      effect.DirectionalLight0.DiffuseColor = this.Light0.DiffuseColor;
      effect.DirectionalLight0.SpecularColor = this.Light0.SpecColor;
      effect.DirectionalLight0.Direction = this.Light0.Direction;
      effect.DirectionalLight1.Enabled = this.Light1.Enabled;
      effect.DirectionalLight1.DiffuseColor = this.Light1.DiffuseColor;
      effect.DirectionalLight1.SpecularColor = this.Light1.SpecColor;
      effect.DirectionalLight1.Direction = this.Light1.Direction;
      effect.DirectionalLight2.Enabled = this.Light2.Enabled;
      effect.DirectionalLight2.DiffuseColor = this.Light2.DiffuseColor;
      effect.DirectionalLight2.SpecularColor = this.Light2.SpecColor;
      effect.DirectionalLight2.Direction = this.Light2.Direction;
    }

    public void MakeConstantLighting()
    {
      this.DiffuseColor = Vector3.One;
      this.AmbientColor = Vector3.One;
      this.LightingEnabled = false;
    }

    public void SetLightValues(Vector3 value)
    {
      this.Light0.DiffuseColor = value;
      this.Light1.DiffuseColor = value;
      this.Light2.DiffuseColor = value;
    }

    public bool IntersectsObject(BoundingSphere collider)
    {
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        this.Model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
        Matrix boneTransform = this.boneTransforms[mesh.ParentBone.Index];
        if (this.TransformBoundingSphere(mesh.BoundingSphere, boneTransform).Intersects(collider))
          return true;
      }
      return false;
    }

    public bool IntersectsRay(Ray r)
    {
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        this.Model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
        Matrix boneTransform = this.boneTransforms[mesh.ParentBone.Index];
        if (this.TransformBoundingSphere(mesh.BoundingSphere, boneTransform).Intersects(r).HasValue)
          return true;
      }
      return false;
    }

    private BoundingSphere TransformBoundingSphere(BoundingSphere sphere, Matrix transform)
    {
      Vector3 vector3 = Vector3.TransformNormal(new Vector3(sphere.Radius, sphere.Radius, sphere.Radius), transform);
      BoundingSphere boundingSphere;
      boundingSphere.Radius = Math.Max(vector3.X, Math.Max(vector3.Y, vector3.Z));
      boundingSphere.Center = Vector3.Transform(sphere.Center, transform);
      return boundingSphere;
    }

    public void SetTexture(string assetName)
    {
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
          effect.Texture = this.Screen.textureManager.Load(assetName);
      }
    }

    public override void Draw(ICamera Camera)
    {
      if (!this.Visible || this.Delete)
        return;
      for (int index = 0; index < this.partTransforms.Length; ++index)
        this.modelBones[index].Transform = this.partTransforms[index] * this.originalTransforms[index];
      this.Model.Root.Transform = this.WorldParentTransform;
      this.Model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
      Engine.gdm.GraphicsDevice.DepthStencilState = this.depthStencilState;
      Engine.gdm.GraphicsDevice.BlendState = this.blendState;
      Engine.gdm.GraphicsDevice.SamplerStates[0] = this.samplerState;
      Engine.gdm.GraphicsDevice.RasterizerState = this.rasterizerState;
      foreach (ModelMesh mesh in this.Model.Meshes)
      {
        foreach (BasicEffect effect in mesh.Effects)
        {
          effect.PreferPerPixelLighting = this.PerPixelLighting;
          effect.LightingEnabled = this.LightingEnabled;
          effect.AmbientLightColor = this.AmbientColor;
          effect.DiffuseColor = this.DiffuseColor;
          effect.SpecularPower = this.specPower;
          effect.SpecularColor = this.specColor;
          effect.Alpha = this.Alpha;
          this.SetLighting(effect);
          effect.World = this.boneTransforms[mesh.ParentBone.Index];
          effect.View = Camera.ViewMatrix;
          effect.Projection = Camera.ProjectionMatrix;
        }
        mesh.Draw();
      }
      base.Draw(Camera);
    }
  }
}
