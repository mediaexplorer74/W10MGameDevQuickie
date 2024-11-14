// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetText
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Text;

#nullable disable
namespace GameEngine
{
  public class WidgetText : Widget
  {
    private SpriteFont font;
    private string fontContent;
    private StringBuilder _Text = new StringBuilder((int) byte.MaxValue);
    public Color pressedColor = Color.Cyan;
    public Color disabledColor = Color.Gray;
    private bool Hovering;
    private bool startedInControl;
    public int maxWidth = -1;

    public event WidgetText.OnTapHandler OnTap;

    public event WidgetText.OnPressedHandler OnPressed;

    public event WidgetText.OnReleasedHandler OnReleased;

    public string Text
    {
      get => this._Text.ToString();
      set
      {
        Vector2 vector2 = Vector2.Zero;
        if (this.font != null)
          vector2 = new Vector2(this.font.MeasureString(value).X, this.font.MeasureString(value).Y);
        this._Text.Remove(0, this._Text.Length);
        this._Text.Append(value);
        this.SourceRect = new Rectangle(0, 0, (int) vector2.X, (int) vector2.Y);
      }
    }

    public override float Width => this.font.MeasureString(this.Text).X;

    public override float Height => this.font.MeasureString(this.Text).Y;

    public WidgetText(
      string name,
      Vector2 position,
      string text,
      string fontcontent,
      GameScreen screen)
      : base(name, position, screen)
    {
      this.handleInput = false;
      this.Text = text;
      this.fontContent = fontcontent;
      this.font = Engine.GlobalContent.Load<SpriteFont>(this.fontContent);
      this.SetAlignment(Alignment.Center);
      this.SourceRect = new Rectangle(0, 0, (int) this.font.MeasureString(text).X, (int) this.font.MeasureString(text).Y);
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

    public override bool HandleTouch(GestureSample gesture, TouchInput touchInput)
    {
      this.Hovering = false;
      if (this.buttonState == GestureState.Disabled || !this.handleInput)
        return false;
      if (gesture.GestureType == GestureType.Tap && touchInput.HeldInRectangle(this.Bounds))
      {
        this.buttonState = GestureState.Tap;
        if (this.OnTap != null)
          this.OnTap();
        this.uiManager.OnTap(this.ID, this.Name);
        return true;
      }
      Vector2 zero = Vector2.Zero;
      if (touchInput.PressedInRectangle(this.Bounds))
      {
        this.startedInControl = true;
        this.buttonState = GestureState.Pressed;
        if (this.OnPressed != null)
          this.OnPressed();
        this.uiManager.OnPressed(this.ID, this.Name);
        return true;
      }
      if (touchInput.gestureState == GestureState.Released && this.startedInControl)
      {
        this.startedInControl = false;
        this.buttonState = GestureState.Released;
        if (this.OnReleased != null)
          this.OnReleased();
        this.uiManager.OnReleased(this.ID);
        return true;
      }
      if (touchInput.HeldInRectangle(this.Bounds) && this.buttonState == GestureState.Pressed)
      {
        this.Hovering = true;
        this.buttonState = GestureState.Dragging;
        return true;
      }
      if (touchInput.HeldInRectangle(this.Bounds))
      {
        this.Hovering = true;
        return true;
      }
      if (touchInput.isDragging && this.startedInControl)
      {
        this.buttonState = GestureState.Dragging;
        return true;
      }
      this.buttonState = GestureState.Idle;
      this.startedInControl = false;
      return false;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      switch (this.buttonState)
      {
        case GestureState.Idle:
          this.DrawWidget(this.color);
          break;
        case GestureState.Pressed:
          this.DrawWidget(this.pressedColor);
          break;
        case GestureState.Disabled:
          this.DrawWidget(this.disabledColor);
          break;
        case GestureState.Dragging:
          if (this.Hovering)
          {
            this.DrawWidget(this.pressedColor);
            break;
          }
          this.DrawWidget(this.color);
          break;
        default:
          this.DrawWidget(this.color);
          break;
      }
    }

    private void DrawWidget(Color color)
    {
      if (this.maxWidth == -1)
      {
        this.Screen.spriteBatch.DrawString(this.font, this._Text, this.Position.ToVector2(), color * this.Opacity * this.uiManager.MasterOpacity, 0.0f, this.Pivot.ToVector2(), 1f, SpriteEffects.None, 0.0f);
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
        this.Screen.spriteBatch.DrawString(this.font, text2, this.Position.ToVector2(), color * this.Opacity * this.uiManager.MasterOpacity, 0.0f, this.Pivot.ToVector2(), 1f, SpriteEffects.None, 0.0f);
      }
    }

    public delegate void OnTapHandler();

    public delegate void OnPressedHandler();

    public delegate void OnReleasedHandler();
  }
}
