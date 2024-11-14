// Decompiled with JetBrains decompiler
// Type: GameEngine.BitmapNumbers
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class BitmapNumbers : SceneNode
  {
    private int number;
    private int[] numarray;
    private int MinLength;
    private Dictionary<int, string> numlookup;
    private string sheetName;
    public Color color = Color.White;
    public float Opacity = 1f;
    public int HorizontalSqueaze;
    private Alignment numAlignment;

    public Alignment numberAlignment
    {
      get
      {
        this.Pivot = Vector3.Zero;
        return this.numAlignment;
      }
      set => this.numAlignment = value;
    }

    public BitmapNumbers(Vector2 where, GameScreen screen, string sheet)
      : this(string.Empty, where, screen, sheet)
    {
    }

    public BitmapNumbers(string setID, Vector2 where, GameScreen screen, string sheet)
      : this(setID, where, 0, screen, sheet)
    {
    }

    public BitmapNumbers(
      string setID,
      Vector2 where,
      int minLength,
      GameScreen screen,
      string sheet)
      : base("numnode", where, screen)
    {
      this.sheetName = sheet;
      this.numlookup = new Dictionary<int, string>();
      this.numlookup.Add(0, setID + "0");
      this.numlookup.Add(1, setID + "1");
      this.numlookup.Add(2, setID + "2");
      this.numlookup.Add(3, setID + "3");
      this.numlookup.Add(4, setID + "4");
      this.numlookup.Add(5, setID + "5");
      this.numlookup.Add(6, setID + "6");
      this.numlookup.Add(7, setID + "7");
      this.numlookup.Add(8, setID + "8");
      this.numlookup.Add(9, setID + "9");
      this.MinLength = minLength;
      this.numarray = new int[1];
    }

    public BitmapNumbers(Vector2 where, int minLength, GameScreen screen, string sheet)
      : base("numnode", where, screen)
    {
      this.sheetName = sheet;
      this.numlookup = new Dictionary<int, string>();
      this.numlookup.Add(0, "0");
      this.numlookup.Add(1, "1");
      this.numlookup.Add(2, "2");
      this.numlookup.Add(3, "3");
      this.numlookup.Add(4, "4");
      this.numlookup.Add(5, "5");
      this.numlookup.Add(6, "6");
      this.numlookup.Add(7, "7");
      this.numlookup.Add(8, "8");
      this.numlookup.Add(9, "9");
      this.MinLength = minLength;
      this.numarray = new int[minLength];
    }

    public void IncrementValue()
    {
      if (this.number >= 2147483645)
        return;
      ++this.number;
      this.numarray = Tools.NumberArrayFromInt(this.number);
      if (this.numarray.Length >= this.MinLength)
        return;
      int[] numArray = new int[this.MinLength];
      for (int index = 0; index < this.MinLength - this.numarray.Length; ++index)
        numArray[index] = 0;
      int num = 0;
      for (int index = this.MinLength - this.numarray.Length; index < this.MinLength; ++index)
        numArray[index] = this.numarray[num++];
      this.numarray = numArray;
    }

    public int Number
    {
      get => this.number;
      set => this.SetValue(value);
    }

    public void SetValue(int count)
    {
      if (count < 0)
        count = 0;
      this.number = count;
      this.numarray = Tools.NumberArrayFromInt(this.number);
      if (this.numarray.Length >= this.MinLength)
        return;
      int[] numArray = new int[this.MinLength];
      for (int index = 0; index < this.MinLength - this.numarray.Length; ++index)
        numArray[index] = 0;
      int num = 0;
      for (int index = this.MinLength - this.numarray.Length; index < this.MinLength; ++index)
        numArray[index] = this.numarray[num++];
      this.numarray = numArray;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
            
      //!!!      
      this.Screen.spriteBatch.Begin(SpriteSortMode.Deferred/*.BackToFront*/, null,
        null, null, null, null, Game1.globalTransformation);

      int num1 = 0;
      if (this.numAlignment == Alignment.Center)
      {
        int num2 = 0;
        for (int index = 0; index < Math.Min(10, this.numarray.Length); ++index)
        {
          num2 += (int) ((double) this.Screen.GetSpriteSource(
              this.numlookup[this.numarray[index]]).Width * (double) this.Scale);
          if (this.numarray.Length > 1)
            num2 += this.HorizontalSqueaze;
        }
        num1 = -num2 / 2;
        if (this.numarray.Length > 1)
          num1 += this.HorizontalSqueaze * (this.numarray.Length - 1) / 2;
      }
      int num3 = 0;
      for (int index = 0; index < Math.Min(10, this.numarray.Length); ++index)
      {
        Vector2 zero = Vector2.Zero;
        if (this.numAlignment == Alignment.Center)
          zero.Y = this.Screen.GetSpriteSource(
              this.numlookup[this.numarray[index]]).CenterPivot().Y;
        this.Screen.spriteBatch.Draw(this.Screen.textureManager.Load(this.sheetName), 
            this.Position.ToVector2() + new Vector2((float) (num1 + num3), 0.0f), 
            new Rectangle?(this.Screen.GetSpriteSource(this.numlookup[this.numarray[index]])), 
            this.color * this.Opacity, 0.0f, zero, this.Scale, SpriteEffects.None, 0.0f);
        num3 += (int) ((double) this.Screen.GetSpriteSource(
            this.numlookup[this.numarray[index]]).Width * (double) this.Scale) 
            + this.HorizontalSqueaze;
      }

        this.Screen.spriteBatch.End();
    }
  }
}
