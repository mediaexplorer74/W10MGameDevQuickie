// Decompiled with JetBrains decompiler
// Type: GameEngine.VirtualStickV2
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameEngine
{
  public class VirtualStickV2 : SceneNode
  {
    public float maxThumbstickDistance;
    public Vector2? ThumbstickCenter = new Vector2?();
    public Color Color = Color.White;
    public float Opacity = 1f;
    public float InactiveOpacity = 1f;
    public float ActiveOpacity = 0.5f;
    private Vector3 nubPosition;
    private int touchID = -1;
    private string nubname;
    public Rectangle SourceRectNUB;
    private Rectangle BoundingBox;
    private Texture2D NubTex;
    private Vector3 NubNeutralPosition;
    private float maxDistance;
    private Vector2 Ringcenter;
    private string contentName;
    private bool enabled = true;
    private int maxTouchDiameter;
    private static int[] lockedTouchID;

    public Vector2 ThumbstickValue
    {
      get
      {
        if (!this.ThumbstickCenter.HasValue || !this.enabled)
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
        if (!(vector != Vector2.Zero) || this.touchID == -1 || !this.enabled)
          return new float?();
        vector.Normalize();
        return new float?(Tools.VectorToAngle(vector));
      }
    }

    public VirtualStickV2(
      Vector2 position,
      string NUBtexName,
      string imageName,
      int maxdistance,
      int maxtouchdiameter,
      GameScreen screen)
      : base("VSTICK", screen)
    {
      VirtualStickV2.lockedTouchID = new int[4];
      this.ReleaseAllTouchID();
      this.contentName = imageName;
      this.nubname = NUBtexName;
      this.NubTex = screen.textureManager.Load(this.contentName);
      this.SourceRectNUB = new Rectangle(0, 0, this.NubTex.Width, this.NubTex.Height);
      this.Pivot = new Vector3((float) (this.NubTex.Width / 2), (float) (this.NubTex.Height / 2), 0.0f);
      this.maxTouchDiameter = maxtouchdiameter;
      int num = maxtouchdiameter;
      this.maxThumbstickDistance = (float) maxdistance;
      this.BoundingBox = new Rectangle((int) position.X - num / 2, (int) position.Y - num / 2, num, num);
      this.Position = this.nubPosition = this.NubNeutralPosition = position.ToVector3();
      this.Ringcenter = new Vector2((float) (this.BoundingBox.X + this.BoundingBox.Width / 2), (float) (this.BoundingBox.Y + this.BoundingBox.Height / 2));
      this.maxDistance = (float) maxdistance;
      this.SetSourceRect();
    }

    public override void SetSourceRect()
    {
      this.SourceRectNUB = this.Screen.GetSpriteSource(this.nubname);
      if (this.SourceRectNUB == Rectangle.Empty)
        this.SourceRectNUB = new Rectangle(0, 0, this.NubTex.Width, this.NubTex.Height);
      this.Pivot = new Vector3((float) this.SourceRectNUB.Width / 2f, (float) this.SourceRectNUB.Height / 2f, 0.0f);
      int maxTouchDiameter = this.maxTouchDiameter;
      this.BoundingBox = new Rectangle((int) this.Position.X - maxTouchDiameter / 2, (int) this.Position.Y - maxTouchDiameter / 2, maxTouchDiameter, maxTouchDiameter);
      this.nubPosition = this.NubNeutralPosition = this.Position;
      this.maxThumbstickDistance = this.maxDistance;
      this.Ringcenter = new Vector2((float) (this.BoundingBox.X + this.BoundingBox.Width / 2), (float) (this.BoundingBox.Y + this.BoundingBox.Height / 2));
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      TouchLocation? nullable = new TouchLocation?();
      foreach (TouchLocation touchLocation in TouchPanel.GetState())
      {
        if (touchLocation.Id == this.touchID)
        {
          nullable = new TouchLocation?(touchLocation);
          break;
        }
        TouchLocation previousLocation;
        if (!touchLocation.TryGetPreviousLocation(out previousLocation))
          previousLocation = touchLocation;
        if (this.touchID == -1 && this.BoundingBox.Contains(previousLocation.Position) && !this.TouchIDinUse(previousLocation.Id))
        {
          nullable = new TouchLocation?(previousLocation);
          break;
        }
      }
      if (nullable.HasValue)
      {
        if (!this.ThumbstickCenter.HasValue)
          this.ThumbstickCenter = new Vector2?(nullable.Value.Position);
        this.nubPosition = this.Position = nullable.Value.Position.ToVector3();
        if ((double) Vector2.Distance(this.Position.ToVector2(), this.Ringcenter) > (double) this.maxDistance)
        {
          Vector2 vector2 = this.Position.ToVector2() - this.Ringcenter;
          vector2.Normalize();
          this.nubPosition = (this.Ringcenter + vector2 * this.maxDistance).ToVector3();
        }
        this.touchID = nullable.Value.Id;
        this.LockTouchID(this.touchID);
      }
      else
      {
        this.ReleaseTouchID(this.touchID);
        this.ThumbstickCenter = new Vector2?();
        this.touchID = -1;
        this.nubPosition = this.NubNeutralPosition;
      }
    }

    public void Enable(bool toggle) => this.enabled = toggle;

    private void LockTouchID(int id)
    {
      for (int index = 0; index < 4; ++index)
      {
        if (VirtualStickV2.lockedTouchID[index] == id)
          return;
      }
      for (int index = 0; index < 4; ++index)
      {
        if (VirtualStickV2.lockedTouchID[index] == -1)
        {
          VirtualStickV2.lockedTouchID[index] = id;
          break;
        }
      }
    }

    private void ReleaseTouchID(int id)
    {
      for (int index = 0; index < 4; ++index)
      {
        if (VirtualStickV2.lockedTouchID[index] == id)
        {
          VirtualStickV2.lockedTouchID[index] = -1;
          break;
        }
      }
    }

    private void ReleaseAllTouchID()
    {
      for (int index = 0; index < 4; ++index)
        VirtualStickV2.lockedTouchID[index] = -1;
    }

    private bool TouchIDinUse(int id)
    {
      bool flag = false;
      for (int index = 0; index < 4; ++index)
      {
        if (VirtualStickV2.lockedTouchID[index] == id)
          flag = true;
      }
      return flag;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      float num = this.touchID == -1 || !this.enabled ? this.InactiveOpacity : this.ActiveOpacity;
      this.Screen.spriteBatch.Draw(this.NubTex, this.nubPosition.ToVector2(), new Rectangle?(this.SourceRectNUB), this.Color * this.Opacity * num, 0.0f, this.Pivot.ToVector2(), 1f, SpriteEffects.None, 0.0f);
    }
  }
}
