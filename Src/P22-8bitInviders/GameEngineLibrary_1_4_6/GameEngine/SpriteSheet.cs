// Decompiled with JetBrains decompiler
// Type: GameEngine.SpriteSheet
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class SpriteSheet
  {
    public Texture2D tex;
    private Dictionary<string, Rectangle> spriteDefinitions;

    public Rectangle this[string i] => this.spriteDefinitions[i];

    public Rectangle this[int index]
    {
      get
      {
        int num = 0;
        foreach (string key in this.spriteDefinitions.Keys)
        {
          if (num == index)
            return this.spriteDefinitions[key];
          ++num;
        }
        return new Rectangle();
      }
    }

    public SpriteSheet(string contentName, GameScreen screen)
    {
      this.tex = screen.textureManager.Load(contentName);
      this.spriteDefinitions = new Dictionary<string, Rectangle>();
    }

    public void AddSpriteSource(string key, Rectangle rect)
    {
      this.spriteDefinitions.Add(key, rect);
    }

    public void GetRectangle(ref string i, out Rectangle rect) => rect = this.spriteDefinitions[i];

    public string GetName(int index)
    {
      int num = 0;
      foreach (string key in this.spriteDefinitions.Keys)
      {
        if (num == index)
          return key;
        ++num;
      }
      return string.Empty;
    }
  }
}
