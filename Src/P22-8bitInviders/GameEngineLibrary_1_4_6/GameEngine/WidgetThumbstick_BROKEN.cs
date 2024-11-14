// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetThumbstick_BROKEN
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameEngine
{
  public class WidgetThumbstick_BROKEN : Widget
  {
    public Color pressedColor = Color.White;
    public Color disabledColor = Color.Gray;
    public Color tintColor = Color.White;
    private bool startedInControl;
    private string pressedName;
    private int touchID = -1;
    public float maxThumbstickDistance;
    public Vector2? ThumbstickCenter = new Vector2?();
    private Rectangle BoundingBox;
    private Vector3 NubNeutralPosition;
    private float maxDistance;
    private Vector2 Ringcenter;
    private Vector3 nubPosition;
    private int maxTouchDiameter;
    public float InactiveOpacity = 1f;
    public float ActiveOpacity = 0.5f;

    public event WidgetThumbstick_BROKEN.OnPressedHandler OnPressed;

    public event WidgetThumbstick_BROKEN.OnReleasedHandler OnReleased;

    public Vector2 ThumbstickValue
    {
      get
      {
        if (!this.ThumbstickCenter.HasValue || this.buttonState == GestureState.Disabled)
          return Vector2.Zero;
        Vector2 thumbstickValue = (this.Position.ToVector2() - this.Ringcenter) / this.maxThumbstickDistance;
        if ((double) thumbstickValue.LengthSquared() > 1.0)
          thumbstickValue.Normalize();
        return thumbstickValue;
      }
    }

    public float? ThumbstickAngle
    {
      get
      {
        Vector2 vector = (this.Position.ToVector2() - this.Ringcenter) / this.maxThumbstickDistance;
        if (!(vector != Vector2.Zero) || this.touchID == -1 || this.buttonState == GestureState.Disabled)
          return new float?();
        vector.Normalize();
        return new float?(Tools.VectorToAngle(vector));
      }
    }

    public WidgetThumbstick_BROKEN(
      string name,
      Vector2 position,
      string contentname,
      int maxdistance,
      int maxtouchdiameter,
      GameScreen screen)
      : base(name, position, contentname, screen)
    {
      this.handleInput = true;
      this.pressedName = name;
      this.maxTouchDiameter = maxtouchdiameter;
      int num = maxtouchdiameter;
      this.maxThumbstickDistance = (float) maxdistance;
      this.BoundingBox = new Rectangle((int) position.X - num / 2, (int) position.Y - num / 2, num, num);
      this.Position = this.nubPosition = this.NubNeutralPosition = position.ToVector3();
      this.Ringcenter = new Vector2((float) (this.BoundingBox.X + this.BoundingBox.Width / 2), (float) (this.BoundingBox.Y + this.BoundingBox.Height / 2));
      this.maxDistance = (float) maxdistance;
    }

    public WidgetThumbstick_BROKEN(
      string name,
      Vector2 position,
      string contentname,
      int maxdistance,
      GameScreen screen)
      : this(name, position, contentname, maxdistance, maxdistance * 2, screen)
    {
    }

    public override bool HandleTouch(GestureSample gesture, TouchInput touchInput)
    {
      if (this.buttonState == GestureState.Disabled || !this.handleInput)
        return false;
      Vector2 zero = Vector2.Zero;
      if (touchInput.PressedInRectangle(this.BoundingBox))
      {
        if (this.touchID == -1)
          this.touchID = touchInput.touchID;
        else if (touchInput.touchID != this.touchID)
          return false;
        if (!this.ThumbstickCenter.HasValue)
          this.ThumbstickCenter = new Vector2?(touchInput.touchPosition);
        this.nubPosition = this.Position = touchInput.touchPosition.ToVector3();
        if ((double) Vector2.Distance(this.Position.ToVector2(), this.Ringcenter) > (double) this.maxDistance)
        {
          Vector2 vector2 = this.Position.ToVector2() - this.Ringcenter;
          vector2.Normalize();
          this.nubPosition = (this.Ringcenter + vector2 * this.maxDistance).ToVector3();
        }
        this.startedInControl = true;
        this.buttonState = GestureState.Pressed;
        if (this.OnPressed != null)
          this.OnPressed();
        this.uiManager.OnPressed(this.ID, this.Name);
        return true;
      }
      if (touchInput.gestureState == GestureState.Released && this.startedInControl)
      {
        this.ThumbstickCenter = new Vector2?();
        this.nubPosition = this.NubNeutralPosition;
        if (this.touchID != -1 && touchInput.touchID != this.touchID)
          return false;
        this.startedInControl = false;
        this.buttonState = GestureState.Released;
        if (this.OnReleased != null)
          this.OnReleased();
        this.uiManager.OnReleased(this.ID);
        return true;
      }
      if (touchInput.isDragging && this.startedInControl)
      {
        if (this.touchID != -1 && touchInput.touchID != this.touchID)
          return false;
        this.nubPosition = this.Position = touchInput.touchPosition.ToVector3();
        if ((double) Vector2.Distance(this.nubPosition.ToVector2(), this.Ringcenter) > (double) this.maxDistance)
        {
          Vector2 vector2 = this.nubPosition.ToVector2() - this.Ringcenter;
          vector2.Normalize();
          this.nubPosition = (this.Ringcenter + vector2 * this.maxDistance).ToVector3();
        }
        this.buttonState = GestureState.Dragging;
        return true;
      }
      this.ThumbstickCenter = new Vector2?();
      this.nubPosition = this.NubNeutralPosition;
      this.buttonState = GestureState.Idle;
      this.startedInControl = false;
      this.touchID = -1;
      return false;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      float num = this.ThumbstickCenter.HasValue ? this.ActiveOpacity : this.InactiveOpacity;
      this.SetTextureSource(this.Name);
      switch (this.buttonState)
      {
        case GestureState.Idle:
          this.DrawWidget(this.tintColor * num);
          break;
        case GestureState.Pressed:
          this.DrawWidget(this.pressedColor * num);
          break;
        case GestureState.Disabled:
          this.DrawWidget(this.disabledColor * num);
          break;
        case GestureState.Dragging:
          this.SetTextureSource(this.pressedName);
          this.DrawWidget(this.pressedColor * num);
          break;
        default:
          this.DrawWidget(this.tintColor);
          break;
      }
    }

    private void DrawWidget(Color color)
    {
      this.Screen.spriteBatch.Draw(this.tex, this.nubPosition.ToVector2(), new Rectangle?(this.SourceRect), color * this.Opacity * this.uiManager.MasterOpacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Scale, this.Facing, 0.0f);
    }

    public delegate void OnPressedHandler();

    public delegate void OnReleasedHandler();
  }
}
