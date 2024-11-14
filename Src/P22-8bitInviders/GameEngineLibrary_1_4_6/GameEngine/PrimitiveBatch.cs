// Decompiled with JetBrains decompiler
// Type: GameEngine.PrimitiveBatch
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace GameEngine
{
  public class PrimitiveBatch : IDisposable
  {
    private const int DefaultBufferSize = 500;
    private VertexPositionColor[] vertices = new VertexPositionColor[500];
    private int positionInBuffer;
    private VertexDeclaration vertexDeclaration;
    private BasicEffect basicEffect;
    private PrimitiveType primitiveType;
    private int numVertsPerPrimitive;
    private bool hasBegun;
    private bool isDisposed;

    public PrimitiveBatch()
    {
      if (Engine.game.GraphicsDevice == null)
        throw new ArgumentNullException("graphicsDevice");
      this.vertexDeclaration = VertexPositionColor.VertexDeclaration;
      this.basicEffect = new BasicEffect(Engine.game.GraphicsDevice);
      this.basicEffect.VertexColorEnabled = true;
      this.basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0.0f, (float) Engine.game.GraphicsDevice.Viewport.Width, (float) Engine.game.GraphicsDevice.Viewport.Height, 0.0f, 0.0f, 1f);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.isDisposed)
        return;
      Tools.Trace("PRIMITIVEBATCH DISPOSED");
      if (this.vertexDeclaration != null)
        this.vertexDeclaration.Dispose();
      if (this.basicEffect != null)
        this.basicEffect.Dispose();
      this.isDisposed = true;
    }

    public void Begin(PrimType primType)
    {
      if (this.hasBegun)
        throw new InvalidOperationException("End must be called before Begin can be called again.");
      PrimitiveType primitive = (PrimitiveType) primType;
      switch (primitive)
      {
        case PrimitiveType.TriangleStrip:
        case PrimitiveType.LineStrip:
          throw new NotSupportedException("The specified primitiveType is not supported by PrimitiveBatch.");
        default:
          this.primitiveType = primitive;
          this.numVertsPerPrimitive = PrimitiveBatch.NumVertsPerPrimitive(primitive);
          this.basicEffect.CurrentTechnique.Passes[0].Apply();
          this.hasBegun = true;
          break;
      }
    }

    public void AddVertex(Vector2 vertex, Color color)
    {
      if (!this.hasBegun)
        throw new InvalidOperationException("Begin must be called before AddVertex can be called.");
      if (this.positionInBuffer % this.numVertsPerPrimitive == 0 && this.positionInBuffer + this.numVertsPerPrimitive >= this.vertices.Length)
        this.Flush();
      this.vertices[this.positionInBuffer].Position = new Vector3(vertex, 0.0f);
      this.vertices[this.positionInBuffer].Color = color;
      ++this.positionInBuffer;
    }

    public void End()
    {
      if (!this.hasBegun)
        throw new InvalidOperationException("Begin must be called before End can be called.");
      this.Flush();
      this.hasBegun = false;
    }

    private void Flush()
    {
      if (!this.hasBegun)
        throw new InvalidOperationException("Begin must be called before Flush can be called.");
      int primitiveCount = this.positionInBuffer / this.numVertsPerPrimitive;
      if (primitiveCount == 0)
        return;
      Engine.game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(this.primitiveType, this.vertices, 0, primitiveCount);
      this.positionInBuffer = 0;
    }

    private static int NumVertsPerPrimitive(PrimitiveType primitive)
    {
      switch (primitive)
      {
        case PrimitiveType.TriangleList:
          return 3;
        case PrimitiveType.LineList:
          return 2;
        default:
          throw new InvalidOperationException("primitive is not valid");
      }
    }
  }
}
