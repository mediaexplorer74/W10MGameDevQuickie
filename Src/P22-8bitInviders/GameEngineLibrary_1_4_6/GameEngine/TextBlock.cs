// Decompiled with JetBrains decompiler
// Type: GameEngine.TextBlock
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

#nullable disable
namespace GameEngine
{
  public class TextBlock : SceneNode
  {
    private SpriteFont font;
    private string fontContent;
    private StringBuilder _Text = new StringBuilder((int) byte.MaxValue);
    public Color color = Color.White;
    public int maxWidth = -1;
    public Alignment ObjectAlignment = Alignment.Center;
    public float Opacity = 1f;

    public string Text
    {
      get => this._Text.ToString();
      set
      {
        this._Text.Remove(0, this._Text.Length);
        this._Text.Append(value);
      }
    }

    public override float Width => this.font.MeasureString(this.Text).X;

    public override float Height => this.font.MeasureString(this.Text).Y;

    public TextBlock(
      string name,
      Vector2 position,
      string text,
      string fontcontent,
      GameScreen screen)
      : base(name, position, screen)
    {
      this.Text = text;
      this.fontContent = fontcontent;
      this.Initialize();
      this.SetAlignment(this.ObjectAlignment);
    }

    public TextBlock(string name, Vector2 position, string text, GameScreen screen)
      : base(name, position, screen)
    {
      this.Text = text;
      this.fontContent = "gameengine/defaultfont";
      this.Initialize();
      this.SetAlignment(this.ObjectAlignment);
    }

    public void SetAlignment(Alignment alignment)
    {
      if (alignment == Alignment.Left)
      {
        this.Pivot = Vector3.Zero;
      }
      else
      {
        if (alignment != Alignment.Center)
          return;
        this.Pivot = new Vector3(this.Width / 2f, this.Height / 2f, 0.0f);
      }
    }

    public void AppendNumber(int number)
    {
      this._Text = this._Text.AppendNumber(number);
      this.SetAlignment(this.ObjectAlignment);
    }

    public void SetTextAsNumber(int number, int digits)
    {
      this._Text.Remove(0, this._Text.Length);
      this._Text = this._Text.AppendNumber(number, digits);
      this.SetAlignment(this.ObjectAlignment);
    }

    public override void Initialize()
    {
      this.font = Engine.GlobalContent.Load<SpriteFont>(this.fontContent);
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      if (this.maxWidth == -1)
      {
        this.Screen.spriteBatch.DrawString(this.font, this._Text, this.Position.ToVector2(), this.color * this.Opacity, 0.0f, this.Pivot.ToVector2(), 1f, SpriteEffects.None, 0.0f);
      }
      else
      {
        string[] strArray = this.Text.Split(' ');
        string text1 = string.Empty;
        int num1 = 0;
        float val1 = 0.0f;
        StringBuilder text2 = new StringBuilder();
        int num2 = 0;
        string str1 = string.Empty;
        while (num1 < strArray.Length)
        {
          string str2 = text1 + str1;
          str1 = strArray[num1++] + " ";
          if ((double) this.font.MeasureString(str2 + str1).X > (double) this.maxWidth)
          {
            text2.AppendLine(str2.TrimStart(' '));
            text1 = string.Empty;
            ++num2;
            if (num1 == strArray.Length)
              text2.Append(str1);
          }
          else
          {
            text1 = str2 + str1;
            str1 = string.Empty;
          }
          val1 = Math.Max(val1, this.font.MeasureString(text1).X);
        }
        text2.AppendLine(text1.TrimStart(' '));
        this.Screen.spriteBatch.DrawString(this.font, text2, this.Position.ToVector2(), this.color * this.Opacity, 0.0f, this.Pivot.ToVector2(), 1f, SpriteEffects.None, 0.0f);
      }
    }
  }
}
