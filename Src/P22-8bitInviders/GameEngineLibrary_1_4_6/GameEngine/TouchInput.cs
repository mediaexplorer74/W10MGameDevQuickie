// Decompiled with JetBrains decompiler
// Type: GameEngine.TouchInput
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

#nullable disable
namespace GameEngine
{
  public class TouchInput
  {
    public int tapToleranceTimer;
    public int tapDeadZone;
    public int dragDeadZone;
    public ulong doubleTapThreshold;
    public int touchID = -1;
    public bool InitialPress;
    public Vector2 startPosition;
    public Vector2 lastPosition;
    public Vector2 touchPosition;
    public Vector2 touchDelta;
    public Vector2 cumulativeDelta;
    public float VelocityX;
    public float VelocityY;
    public bool isDragging;
    public int pressedTimer;
    public ulong previousTapTicks;
    public GestureState gestureState;
    private TimeSpan flickStart;
    private TimeSpan flickEnd;

    public bool Pressed(out Vector2 where)
    {
      if (this.touchID != -1 && this.InitialPress)
      {
        where = this.touchPosition;
        return true;
      }
      where = Vector2.Zero;
      return false;
    }

    public bool HeldInRectangle(Rectangle rect)
    {
      return this.touchID != -1 && rect.Contains(this.touchPosition);
    }

    public bool PressedInRectangle(Rectangle rect)
    {
      return this.touchID != -1 && this.InitialPress && rect.Contains(this.touchPosition);
    }

    public bool PressedInRectangle(Rectangle rect, out Vector2 where)
    {
      where = Vector2.Zero;
      if (this.touchID == -1 || !this.InitialPress || !rect.Contains(this.touchPosition))
        return false;
      where = this.touchPosition;
      return true;
    }

    public bool DragInRectangle(Rectangle rect, out Vector2 where)
    {
      if (this.touchID != -1 && this.isDragging && rect.Contains(this.touchPosition))
      {
        where = this.touchPosition;
        return true;
      }
      where = Vector2.Zero;
      return false;
    }

    public void Update()
    {
      ++this.previousTapTicks;
      if (this.gestureState == GestureState.Released)
        this.gestureState = GestureState.Idle;
      TouchLocation? nullable = new TouchLocation?();
      foreach (TouchLocation touchLocation in TouchPanel.GetState())
      {
        if (touchLocation.Id == this.touchID)
        {
          nullable = new TouchLocation?(touchLocation);
          this.InitialPress = false;
          ++this.pressedTimer;
          this.touchPosition = nullable.Value.Position;
          if (touchLocation.State == TouchLocationState.Moved)
          {
            if (touchLocation.Position != this.startPosition)
            {
              this.touchDelta = this.touchPosition - this.lastPosition;
              this.lastPosition = this.touchPosition;
              this.cumulativeDelta += this.touchDelta;
              if ((double) Math.Abs(this.cumulativeDelta.X) <= (double) this.dragDeadZone)
              {
                if ((double) Math.Abs(this.cumulativeDelta.Y) <= (double) this.dragDeadZone)
                  break;
              }
              this.isDragging = true;
              break;
            }
            break;
          }
          break;
        }
        TouchLocation previousLocation;
        if (!touchLocation.TryGetPreviousLocation(out previousLocation))
          previousLocation = touchLocation;
        if (this.touchID == -1)
        {
          nullable = new TouchLocation?(previousLocation);
          if (!TouchManager.IsTracking(nullable.Value.Id))
            break;
        }
      }
      if (nullable.HasValue)
      {
        if (!TouchManager.AddID(nullable.Value.Id))
          return;
        this.flickStart = TimeSpan.FromTicks(DateTime.Now.Ticks);
        this.touchID = nullable.Value.Id;
        this.InitialPress = true;
        this.touchPosition = this.startPosition = this.lastPosition = nullable.Value.Position;
        this.touchDelta = Vector2.Zero;
        this.cumulativeDelta = Vector2.Zero;
        this.pressedTimer = 0;
        this.isDragging = false;
        this.gestureState = GestureState.Pressed;
      }
      else
      {
        if (this.touchID == -1)
          return;
        this.flickEnd = TimeSpan.FromTicks(DateTime.Now.Ticks);
        TimeSpan timeSpan = this.flickEnd - this.flickStart;
        Vector2 zero = Vector2.Zero with
        {
          X = this.touchDelta.X * (float) timeSpan.TotalSeconds
        };
        this.VelocityX = zero.X / (float) timeSpan.TotalSeconds;
        zero.Y = this.touchDelta.Y * (float) timeSpan.TotalSeconds;
        this.VelocityY = zero.Y / (float) timeSpan.TotalSeconds;
        TouchManager.RemoveID(this.touchID);
        this.touchID = -1;
        this.isDragging = false;
        this.gestureState = GestureState.Released;
      }
    }
  }
}
