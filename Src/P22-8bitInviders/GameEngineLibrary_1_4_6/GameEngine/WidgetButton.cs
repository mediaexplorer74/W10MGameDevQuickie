// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetButton
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameEngine
{
  public class WidgetButton : Widget
  {
    public Color pressedColor = Color.DarkGray;
    public Color disabledColor = Color.Gray;
    public Color tintColor = Color.White;
    private bool Hovering;
    private bool startedInControl;
    private string pressedName;
    private int touchID = -1;

    public event WidgetButton.OnTapHandler OnTap;

    public event WidgetButton.OnPressedHandler OnPressed;

    public event WidgetButton.OnReleasedHandler OnReleased;

    public WidgetButton(string name, Vector2 position, string contentname, GameScreen screen)
      : base(name, position, contentname, screen)
    {
      this.handleInput = true;
      this.pressedName = name;
    }

    public WidgetButton(
      string name,
      string pressedname,
      Vector2 position,
      string contentname,
      GameScreen screen)
      : base(name, position, contentname, screen)
    {
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

    public void RenameSourceRect(string name) => this.RenameSourceRect(name, true);

    public void RenameSourceRect(string name, bool renamePressedName)
    {
      if (renamePressedName)
        this.pressedName = name;
      this.Name = name;
      this.sourceName = name;
      this.SetSourceRect();
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      this.SetTextureSource(this.Name);
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
            this.SetTextureSource(this.pressedName);
            this.DrawWidget(this.pressedColor);
            break;
          }
          this.SetTextureSource(this.Name);
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
  }
}
