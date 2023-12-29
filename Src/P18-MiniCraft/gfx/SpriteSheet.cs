// Decompiled with JetBrains decompiler
// Type: GameManager.gfx.SpriteSheet
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameManager.gfx
{
  public class SpriteSheet
  {
    public int[] Pixels;
    private Texture2D texture;

    public int width => this.texture.Width;

    public int height => this.texture.Height;

    public SpriteSheet(ContentManager content, string name)
    {
      this.texture = content.Load<Texture2D>(name);
      this.Pixels = new int[this.texture.Width * this.texture.Height];
      Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[this.Pixels.Length];
      this.texture.GetData<Microsoft.Xna.Framework.Color>(data);
      for (int index = 0; index < this.Pixels.Length; ++index)
        this.Pixels[index] = (int) data[index].B / 64;
    }
  }
}
