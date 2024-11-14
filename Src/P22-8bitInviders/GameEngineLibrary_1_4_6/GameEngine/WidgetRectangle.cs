// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetRectangle
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameEngine
{
  public class WidgetRectangle : Widget
  {
    public Color pressedColor = Color.DarkGray;
    public Color disabledColor = Color.Gray;
    public Color tintColor = Color.White;
    private bool Hovering;
    private bool startedInControl;
    private string pressedName;
    private int touchID = -1;
    private Rectangle regionBounds;
    public bool ShowChrome = true;
    public bool ShowWhenInteracted = true;

    public event WidgetRectangle.OnTapHandler OnTap;

    public event WidgetRectangle.OnPressedHandler OnPressed;

    public event WidgetRectangle.OnReleasedHandler OnReleased;

    public override Rectangle Bounds => this.regionBounds;

    public WidgetRectangle(string name, Rectangle bounds, GameScreen screen)
      : base(name, bounds.CenterPoint(), string.Empty, screen)
    {
      this.regionBounds = bounds;
      this.handleInput = true;
      this.pressedName = name;
    }

    public WidgetRectangle(string name, Vector2 where, int width, int height, GameScreen screen)
      : base(name, where, string.Empty, screen)
    {
      this.regionBounds = new Rectangle((int) where.X - width / 2, (int) where.Y - height / 2, width, height);
      this.handleInput = true;
      this.pressedName = name;
    }

    public WidgetRectangle(string name, string pressedname, Rectangle bounds, GameScreen screen)
      : base(name, bounds.CenterPoint(), string.Empty, screen)
    {
      this.regionBounds = bounds;
      this.handleInput = true;
      this.pressedName = pressedname;
    }

    public WidgetRectangle(
      string name,
      string pressedname,
      Vector2 where,
      int width,
      int height,
      GameScreen screen)
      : base(name, where, string.Empty, screen)
    {
      this.regionBounds = new Rectangle((int) where.X - width / 2, (int) where.Y - height / 2, width, height);
      this.handleInput = true;
      this.pressedName = pressedname;
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
        if (this.touchID == -1)
          this.touchID = touchInput.touchID;
        else if (touchInput.touchID != this.touchID)
          return false;
        this.startedInControl = true;
        this.buttonState = GestureState.Pressed;
        if (this.OnPressed != null)
          this.OnPressed();
        this.uiManager.OnPressed(this.ID, this.Name);
        return true;
      }
      if (touchInput.gestureState == GestureState.Released && this.startedInControl)
      {
        if (this.touchID != -1 && touchInput.touchID != this.touchID)
          return false;
        this.startedInControl = false;
        this.buttonState = GestureState.Released;
        if (this.OnReleased != null)
          this.OnReleased();
        this.uiManager.OnReleased(this.ID);
        return true;
      }
      if (touchInput.HeldInRectangle(this.Bounds) && this.buttonState == GestureState.Pressed)
      {
        if (this.touchID == -1)
          this.touchID = touchInput.touchID;
        else if (touchInput.touchID != this.touchID)
          return false;
        this.Hovering = true;
        this.buttonState = GestureState.Dragging;
        return true;
      }
      if (touchInput.HeldInRectangle(this.Bounds))
      {
        if (this.touchID == -1)
          this.touchID = touchInput.touchID;
        else if (touchInput.touchID != this.touchID)
          return false;
        this.Hovering = true;
        return true;
      }
      if (touchInput.isDragging && this.startedInControl)
      {
        if (this.touchID != -1 && touchInput.touchID != this.touchID)
          return false;
        this.buttonState = GestureState.Dragging;
        return true;
      }
      this.buttonState = GestureState.Idle;
      this.startedInControl = false;
      this.touchID = -1;
      return false;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      switch (this.buttonState)
      {
        case GestureState.Idle:
          if (!this.ShowChrome)
            break;
          this.DrawWidget(this.tintColor);
          break;
        case GestureState.Pressed:
          if (!this.ShowWhenInteracted)
            break;
          this.DrawWidget(this.pressedColor);
          break;
        case GestureState.Disabled:
          if (!this.ShowChrome)
            break;
          this.DrawWidget(this.disabledColor);
          break;
        case GestureState.Dragging:
          if (this.Hovering)
          {
            if (!this.ShowWhenInteracted)
              break;
            this.DrawWidget(this.pressedColor);
            break;
          }
          if (!this.ShowWhenInteracted)
            break;
          this.DrawWidget(this.tintColor);
          break;
        default:
          if (!this.ShowChrome)
            break;
          this.DrawWidget(this.tintColor);
          break;
      }
    }

    private void DrawWidget(Color color)
    {
      this.Screen.spriteBatch.Draw(this.tex, this.regionBounds, color * this.Opacity * this.uiManager.MasterOpacity);
    }

    public delegate void OnTapHandler();

    public delegate void OnPressedHandler();

    public delegate void OnReleasedHandler();
  }
}
