// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetSlider
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameEngine
{
  public class WidgetSlider : Widget
  {
    public Color pressedColor = Color.Cyan;
    public Color disabledColor = Color.Gray;
    public Color tintColor = Color.White;
    private float _Value;
    private bool Hovering;
    private bool startedInControl;
    private int leftBounds;
    private int rightBounds;
    private float valueMin;
    private float valueMax;
    private Rectangle sliderBox;

    public event WidgetSlider.OnTapHandler OnTap;

    public event WidgetSlider.OnPressedHandler OnPressed;

    public event WidgetSlider.OnReleasedHandler OnReleased;

    public event WidgetSlider.ValueChangedHandler ValueChanged;

    public float Value
    {
      get => this._Value;
      set
      {
        double num = (double) MathHelper.Clamp(value, this.valueMin, this.valueMax);
        this._Value = value;
        this.Position.X = Tools.RemapValue(this._Value, this.valueMin, this.valueMax, (float) this.leftBounds, (float) this.rightBounds);
      }
    }

    public WidgetSlider(
      string nubName,
      Rectangle bounds,
      float min,
      float max,
      string contentname,
      GameScreen screen)
      : base(nubName, new Vector2((float) bounds.Left, (float) (bounds.Top + bounds.Height / 2)), contentname, screen)
    {
      this.handleInput = true;
      this.valueMin = min;
      this.valueMax = max;
      this.leftBounds = bounds.Left;
      this.rightBounds = bounds.Right;
      this.sliderBox = bounds;
      this.SetSourceRect();
    }

    public override bool HandleTouch(GestureSample gesture, TouchInput touchInput)
    {
      this.Hovering = false;
      if (this.buttonState == GestureState.Disabled || !this.handleInput)
        return false;
      if (gesture.GestureType == GestureType.Tap && touchInput.HeldInRectangle(this.sliderBox))
      {
        this._Value = Tools.RemapValue(gesture.Position.X, (float) this.leftBounds, (float) this.rightBounds, this.valueMin, this.valueMax);
        this.Position.X = gesture.Position.X;
        this.buttonState = GestureState.Tap;
        if (this.OnTap != null)
          this.OnTap();
        this.uiManager.OnTap(this.ID, this.Name);
        if (this.ValueChanged != null)
          this.ValueChanged(this._Value);
        this.uiManager.OnSliderChanged(this.ID, this._Value);
        return true;
      }
      Vector2 zero = Vector2.Zero;
      if (touchInput.PressedInRectangle(this.sliderBox))
      {
        this._Value = Tools.RemapValue(touchInput.touchPosition.X, (float) this.leftBounds, (float) this.rightBounds, this.valueMin, this.valueMax);
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
      if (touchInput.HeldInRectangle(this.sliderBox) && this.buttonState == GestureState.Pressed)
      {
        this.Position.X = touchInput.touchPosition.X;
        this._Value = Tools.RemapValue(touchInput.touchPosition.X, (float) this.leftBounds, (float) this.rightBounds, this.valueMin, this.valueMax);
        this.Hovering = true;
        this.buttonState = GestureState.Dragging;
        if (this.ValueChanged != null)
          this.ValueChanged(this._Value);
        this.uiManager.OnSliderChanged(this.ID, this._Value);
        return true;
      }
      if (touchInput.HeldInRectangle(this.sliderBox))
      {
        this.Position.X = touchInput.touchPosition.X;
        this._Value = Tools.RemapValue(touchInput.touchPosition.X, (float) this.leftBounds, (float) this.rightBounds, this.valueMin, this.valueMax);
        this.Hovering = true;
        if (this.ValueChanged != null)
          this.ValueChanged(this._Value);
        this.uiManager.OnSliderChanged(this.ID, this._Value);
        return true;
      }
      if (touchInput.isDragging && this.startedInControl)
      {
        this.Position.X = touchInput.touchPosition.X;
        this._Value = Tools.RemapValue(touchInput.touchPosition.X, (float) this.leftBounds, (float) this.rightBounds, this.valueMin, this.valueMax);
        this.buttonState = GestureState.Dragging;
        if (this.ValueChanged != null)
          this.ValueChanged(this._Value);
        this.uiManager.OnSliderChanged(this.ID, this._Value);
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
          this.DrawWidget(this.tintColor);
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
          this.DrawWidget(this.tintColor);
          break;
        default:
          this.DrawWidget(this.tintColor);
          break;
      }
    }

    private void DrawWidget(Color color)
    {
      this.Screen.spriteBatch.Draw(this.tex, this.Position.ToVector2(), new Rectangle?(this.SourceRect), color * this.Opacity * this.uiManager.MasterOpacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Scale, this.Facing, 0.0f);
    }

    public delegate void OnTapHandler();

    public delegate void OnPressedHandler();

    public delegate void OnReleasedHandler();

    public delegate void ValueChangedHandler(float newValue);
  }
}
