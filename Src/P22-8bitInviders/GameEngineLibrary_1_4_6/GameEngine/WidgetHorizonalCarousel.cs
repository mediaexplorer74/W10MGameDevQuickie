// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetHorizonalCarousel
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class WidgetHorizonalCarousel : Widget
  {
    public Color pressedColor = Color.Cyan;
    public Color disabledColor = Color.Gray;
    public Color tintColor = Color.White;
    public bool SnapToItem = true;
    public float Decay = 0.93f;
    public float BounceSpring = 0.4f;
    public float SpeedSpring = 0.4f;
    public bool isScrolling;
    private bool startedInControl;
    private int leftBounds;
    private Rectangle itemBoundsBox;
    private Rectangle widgetSize;
    private List<SceneNode> childList;
    private int tapOffset;
    private float velocity;
    private bool isTouching;
    private float MOUSE_DOWN_DECAY = 0.5f;
    private Vector3 lastMouseDownPoint;
    private int currentItemView;
    private int currentItemLeftBound;
    private int snapLeft;

    public event WidgetHorizonalCarousel.OnTapHandler OnTap;

    public event WidgetHorizonalCarousel.OnPressedHandler OnPressed;

    public event WidgetHorizonalCarousel.OnReleasedHandler OnReleased;

    public WidgetHorizonalCarousel(
      string name,
      Rectangle itembounds,
      Rectangle widgetsize,
      GameScreen screen)
      : base(name, new Vector2((float) itembounds.Left, (float) (itembounds.Top + itembounds.Height / 2)), screen)
    {
      this.handleInput = true;
      this.widgetSize = widgetsize;
      this.itemBoundsBox = new Rectangle(this.widgetSize.X, this.widgetSize.Y, itembounds.Width, itembounds.Height);
      this.childList = new List<SceneNode>();
      this.Position = new Vector3((float) this.itemBoundsBox.Left, (float) this.itemBoundsBox.Top, 0.0f);
    }

    public void AddCarouselItem(SceneNode node)
    {
      node.Pivot = Vector3.Zero;
      this.leftBounds = (this.childList.Count + 1) * this.itemBoundsBox.Width <= this.widgetSize.Width ? -this.childList.Count * this.itemBoundsBox.Width : -(this.childList.Count + 1) * this.itemBoundsBox.Width + this.widgetSize.Width;
      node.Position.X = (float) (this.itemBoundsBox.Left + this.childList.Count * this.itemBoundsBox.Width * this.childList.Count);
      node.Position.Y = (float) this.itemBoundsBox.Top + this.Position.Y;
      this.childList.Add(node);
    }

    public override bool HandleTouch(GestureSample gesture, TouchInput touchInput)
    {
      if (this.buttonState == GestureState.Disabled || !this.handleInput)
        return false;
      if (gesture.GestureType == GestureType.Tap && touchInput.HeldInRectangle(this.itemBoundsBox))
      {
        this.Position.X = touchInput.touchPosition.X - (float) this.tapOffset;
        this.velocity = 0.0f;
        this.isTouching = false;
        this.buttonState = GestureState.Tap;
        if (this.OnTap != null)
          this.OnTap();
        this.uiManager.OnTap(this.ID, this.Name);
        return true;
      }
      Vector2 zero = Vector2.Zero;
      if (touchInput.PressedInRectangle(this.itemBoundsBox))
      {
        this.isTouching = true;
        this.tapOffset = (int) touchInput.touchPosition.X - (int) this.Position.X;
        this.velocity = 0.0f;
        this.startedInControl = true;
        this.lastMouseDownPoint.X = touchInput.touchPosition.X - (float) this.tapOffset;
        this.buttonState = GestureState.Pressed;
        if (this.OnPressed != null)
          this.OnPressed();
        this.uiManager.OnPressed(this.ID, this.Name);
        return true;
      }
      if (touchInput.gestureState == GestureState.Released && this.startedInControl)
      {
        this.isTouching = false;
        this.velocity = touchInput.VelocityX;
        this.startedInControl = false;
        this.buttonState = GestureState.Released;
        if (this.OnReleased != null)
          this.OnReleased();
        this.uiManager.OnReleased(this.ID);
        return true;
      }
      if (touchInput.HeldInRectangle(this.itemBoundsBox) && this.buttonState == GestureState.Pressed)
      {
        this.Position.X = touchInput.touchPosition.X - (float) this.tapOffset;
        this.buttonState = GestureState.Dragging;
        return true;
      }
      if (touchInput.HeldInRectangle(this.itemBoundsBox) && this.startedInControl)
      {
        this.velocity += touchInput.touchPosition.X - this.lastMouseDownPoint.X;
        this.lastMouseDownPoint.X = touchInput.touchPosition.X;
        this.Position.X = touchInput.touchPosition.X - (float) this.tapOffset;
        return true;
      }
      if (touchInput.isDragging && this.startedInControl)
      {
        this.velocity += touchInput.touchPosition.X - this.lastMouseDownPoint.X;
        this.lastMouseDownPoint.X = touchInput.touchPosition.X;
        this.Position.X = touchInput.touchPosition.X - (float) this.tapOffset;
        this.buttonState = GestureState.Dragging;
        return true;
      }
      this.buttonState = GestureState.Idle;
      this.startedInControl = false;
      return false;
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      for (int index = 0; index < this.childList.Count; ++index)
      {
        this.childList[index].Position.X = this.Position.X + (float) (index * this.itemBoundsBox.Width);
        this.childList[index].Position.Y = this.Position.Y;
        this.childList[index].Update(gameTime, ref worldTransform);
      }
      if (this.isTouching)
        this.velocity *= this.MOUSE_DOWN_DECAY;
      else
        this.velocity *= this.Decay;
      if (!this.isTouching)
      {
        float num = 0.0f;
        if ((double) this.Position.X > (double) this.widgetSize.Left)
        {
          this.velocity = 0.0f;
          num = (float) -((double) this.Position.X - (double) this.widgetSize.Left) * this.BounceSpring;
        }
        else if ((double) this.Position.X < (double) (this.leftBounds + this.widgetSize.Left))
        {
          this.velocity = 0.0f;
          num = (float) -((double) (this.leftBounds * -1) + (double) this.Position.X - (double) this.widgetSize.Left) * this.BounceSpring;
        }
        else if (this.SnapToItem && (double) Math.Abs(this.velocity) < 2.0)
        {
          this.velocity = 0.0f;
          num = (float) -((double) this.snapLeft + (double) this.Position.X - (double) this.widgetSize.Left) * this.BounceSpring;
        }
        this.Position.X = this.Position.X + this.velocity + num;
        if (((double) this.Position.X > (double) this.itemBoundsBox.Left || (double) this.Position.X < (double) this.leftBounds) && Tools.WithinTolerance(this.velocity, 0.0f, 25f))
        {
          this.velocity = 0.0f;
          if (Tools.WithinTolerance(this.Position.X, 0.0f, 1f))
            this.Position.X = 0.0f;
          else if (Tools.WithinTolerance(this.Position.X, (float) this.leftBounds, 1f))
            this.Position.X = (float) this.leftBounds;
        }
      }
      this.isScrolling = (double) this.velocity != 0.0;
      this.currentItemView = (int) Math.Abs((this.Position.X - (float) this.itemBoundsBox.Left - (float) (this.itemBoundsBox.Width / 2)) / (float) this.itemBoundsBox.Width);
      this.currentItemLeftBound = this.currentItemView * this.itemBoundsBox.Width;
      this.currentItemLeftBound = (int) MathHelper.Clamp((float) this.currentItemLeftBound, 0.0f, (float) ((this.childList.Count - 1) * this.itemBoundsBox.Width));
      this.snapLeft = this.currentItemLeftBound;
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      foreach (SceneNode child in this.childList)
        child.Draw();
    }

    public delegate void OnTapHandler();

    public delegate void OnPressedHandler();

    public delegate void OnReleasedHandler();
  }
}
