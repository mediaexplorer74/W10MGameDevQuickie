// Decompiled with JetBrains decompiler
// Type: GameEngine.TextureManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameEngine
{
  public class TextureManager
  {
    private GameScreen Screen;
    private Dictionary<string, Texture2D> Assets;
    public bool PreMultply;

    public TextureManager()
    {
      this.Screen = (GameScreen) null;
      this.Assets = new Dictionary<string, Texture2D>();
    }

    public TextureManager(GameScreen screen)
    {
      this.Screen = screen;
      this.Assets = new Dictionary<string, Texture2D>();
    }

    public Texture2D Load<T>(string loc) => this.Load(loc);

    public Texture2D LoadJPG(string loc)
    {
      if (this.Screen != null && this.Screen.useContentManager)
        return this.Screen.content.Load<Texture2D>(loc);
      if (this.Assets.ContainsKey(loc))
        return this.Assets[loc];
      Texture2D texture2D = (Texture2D) null;
      using (Stream stream = TitleContainer.OpenStream("Content\\" + loc + ".jpg"))
        texture2D = Texture2D.FromStream(Engine.gdm.GraphicsDevice, stream);
      this.Assets.Add(loc, texture2D);
      return texture2D;
    }

    public Texture2D Load(string loc)
    {
      if (this.Screen != null && this.Screen.useContentManager)
        return this.Screen.content.Load<Texture2D>(loc);
      if (this.Assets.ContainsKey(loc))
        return this.Assets[loc];
      Texture2D texture = (Texture2D) null;
      using (Stream stream = TitleContainer.OpenStream("Content\\" + loc + ".png"))
        texture = Texture2D.FromStream(Engine.gdm.GraphicsDevice, stream);
      if (this.PreMultply)
      {
        using (RenderTarget2D renderTarget = new RenderTarget2D(Engine.game.GraphicsDevice, texture.Width, texture.Height))
        {
          Viewport viewport = Engine.gdm.GraphicsDevice.Viewport;
          Engine.game.GraphicsDevice.SetRenderTarget(renderTarget);
          Engine.game.GraphicsDevice.Clear(Color.Black);
          SpriteBatch spriteBatch = new SpriteBatch(Engine.gdm.GraphicsDevice);
          spriteBatch.Begin(SpriteSortMode.Immediate, Engine.BlendColorBlendState);
          spriteBatch.Draw(texture, texture.Bounds, Color.White);
          spriteBatch.End();
          spriteBatch.Begin(SpriteSortMode.Immediate, Engine.BlendAlphaBlendState);
          spriteBatch.Draw(texture, texture.Bounds, Color.White);
          spriteBatch.End();
          Engine.game.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
          Engine.game.GraphicsDevice.Viewport = viewport;
          Color[] data = new Color[texture.Width * texture.Height];
          renderTarget.GetData<Color>(data);
          Engine.game.GraphicsDevice.Textures[0] = (Texture) null;
          texture.SetData<Color>(data);
          this.Assets.Add(loc, texture);
          return texture;
        }
      }
      else
      {
        this.Assets.Add(loc, texture);
        return texture;
      }
    }

    public void Unload()
    {
      foreach (string key in this.Assets.Keys)
        this.Assets[key].Dispose();
    }
  }
}
