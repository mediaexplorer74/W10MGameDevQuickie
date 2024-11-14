// Decompiled with JetBrains decompiler
// Type: GameEngine.VirtualStick
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameEngine
{
  public class VirtualStick : SceneNode
  {
    public float maxThumbstickDistance;
    public Vector2? ThumbstickCenter = new Vector2?();
    public Color Color = Color.White;
    public float Opacity = 1f;
    public float InactiveOpacity = 0.5f;
    public float ActiveOpacity = 1f;
    private Vector3 nubPosition;
    private int touchID = -1;
    private string nubname;
    public Rectangle SourceRectNUB;
    private Rectangle BoundingBox;
    private Texture2D NubTex;
    private Vector3 NubNeutralPosition;
    private float maxDistance;
    private Vector2 Ringcenter;
    private StickDock stickDock;
    private string contentName;
    private bool enabled = true;

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

    public VirtualStick(
      StickDock location,
      string NUBtexName,
      string imageName,
      int maxdistance,
      GameScreen screen)
      : base("VSTICK", screen)
    {
      this.contentName = imageName;
      this.stickDock = location;
      this.nubname = NUBtexName;
      this.NubTex = screen.textureManager.Load(this.contentName);
      this.SourceRectNUB = new Rectangle(0, 0, this.NubTex.Width, this.NubTex.Height);
      this.Pivot = new Vector3((float) (this.NubTex.Width / 2), (float) (this.NubTex.Height / 2), 0.0f);
      int width = this.SourceRectNUB.Width;
      this.maxThumbstickDistance = (float) maxdistance;
      switch (location)
      {
        case StickDock.LowerLeft:
          this.BoundingBox = new Rectangle(0, Engine.gameHeight - width, width, width);
          this.Position = this.nubPosition = this.NubNeutralPosition = new Vector3((float) (width / 2), (float) (Engine.gameHeight - width / 2), 0.0f);
          break;
        case StickDock.LowerRight:
          this.BoundingBox = new Rectangle(Engine.gameWidth - width, Engine.gameHeight - width, width, width);
          this.Position = this.nubPosition = this.NubNeutralPosition = new Vector3((float) (Engine.gameWidth - width / 2), (float) (Engine.gameHeight - width / 2), 0.0f);
          break;
      }
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
      int width = this.SourceRectNUB.Width;
      this.maxThumbstickDistance = this.maxDistance;
      if (this.stickDock == StickDock.LowerLeft)
      {
        this.BoundingBox = new Rectangle(0, Engine.gameHeight - width, width, width);
        this.Position = this.nubPosition = this.NubNeutralPosition = new Vector3((float) (width / 2), (float) (Engine.gameHeight - width / 2), 0.0f);
      }
      else if (this.stickDock == StickDock.LowerRight)
      {
        this.BoundingBox = new Rectangle(Engine.gameWidth - width, Engine.gameHeight - width, width, width);
        this.Position = this.nubPosition = this.NubNeutralPosition = new Vector3((float) (Engine.gameWidth - width / 2), (float) (Engine.gameHeight - width / 2), 0.0f);
      }
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
        if (this.touchID == -1 && this.BoundingBox.Contains(previousLocation.Position))
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
      }
      else
      {
        this.ThumbstickCenter = new Vector2?();
        this.touchID = -1;
        this.nubPosition = this.NubNeutralPosition;
      }
    }

    public void Enable(bool toggle) => this.enabled = toggle;

    public override void Draw()
    {
      if (!this.Visible)
        return;
      float num = this.touchID == -1 || !this.enabled ? this.InactiveOpacity : this.ActiveOpacity;
      this.Screen.spriteBatch.Draw(this.NubTex, this.nubPosition.ToVector2(), new Rectangle?(this.SourceRectNUB), this.Color * this.Opacity * num, 0.0f, this.Pivot.ToVector2(), 1f, SpriteEffects.None, 0.0f);
    }
  }
}
