// Decompiled with JetBrains decompiler
// Type: GameManager.SputnikGenerator
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;

#nullable disable
namespace GameManager
{
  public class SputnikGenerator : SceneNode
  {
    private AdvancedObjectPool<ASTSputnik> sputnikPool;

    public SputnikGenerator(GameScreen screen)
      : base(screen)
    {
      this.Initialize();
    }

    public override void Initialize()
    {
      this.sputnikPool = new AdvancedObjectPool<ASTSputnik>(4);
      for (int index = 0; index < 4; ++index)
        this.sputnikPool.AddToPool(new ASTSputnik(this.Screen));
      this.sputnikPool.Get().Item.InitFromPool(new Vector2(600f, 600f));
      this.sputnikPool.Get().Item.InitFromPool(new Vector2(600f, 4400f));
      this.sputnikPool.Get().Item.InitFromPool(new Vector2(4400f, 600f));
      this.sputnikPool.Get().Item.InitFromPool(new Vector2(4400f, 4400f));
    }

    public float GetClosestSputnik(Vector3 where)
    {
      float closestSputnik = 10000f;
      foreach (ASTSputnik astSputnik in this.sputnikPool)
      {
        float num = Vector3.Distance(where, astSputnik.sputnik.Position);
        if ((double) num < (double) closestSputnik)
          closestSputnik = num;
      }
      return closestSputnik;
    }

    public Vector3 ActivateTractor(Vector3 where)
    {
      Vector3 vector3 = Vector3.Zero;
      ASTSputnik astSputnik1 = (ASTSputnik) null;
      float num1 = 10000f;
      foreach (ASTSputnik astSputnik2 in this.sputnikPool)
      {
        float num2 = Vector3.Distance(where, astSputnik2.sputnik.Position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          astSputnik1 = astSputnik2;
        }
      }
      if (astSputnik1 != null)
      {
        astSputnik1.ActivateTractor();
        vector3 = astSputnik1.sputnik.Position;
      }
      return vector3;
    }

    public void DisableTractor()
    {
      foreach (ASTSputnik astSputnik in this.sputnikPool)
        astSputnik.DisableTractor();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      foreach (AdvancedObjectPool<ASTSputnik>.Node activeNode in this.sputnikPool.ActiveNodes)
      {
        activeNode.Item.Update(gameTime, ref worldTransform);
        if (activeNode.Item.returntoPool)
          this.sputnikPool.Return(activeNode);
      }
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw()
    {
      foreach (SceneNode sceneNode in this.sputnikPool)
        sceneNode.Draw();
    }
  }
}
