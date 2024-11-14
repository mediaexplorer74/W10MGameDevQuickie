// Decompiled with JetBrains decompiler
// Type: GameEngine.QuickStopwatchTimer
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class QuickStopwatchTimer : SceneNode
  {
    private readonly Timer _timer1 = new Timer("t1");
    private readonly Timer _timer2 = new Timer("t2");

    public event QuickStopwatchExpiredHandler ExpiredEvent;

    public bool Expired => this._timer2.Expired;

    public bool Started => this._timer1.Expired;

    public QuickStopwatchTimer(string name)
      : base((GameScreen) null)
    {
      this.Name = name;
      this.Reset();
    }

    public QuickStopwatchTimer()
      : base((GameScreen) null)
    {
      this.Name = "Unnamed";
      this.Reset();
    }

    public void Reset()
    {
      this._timer1.Reset();
      this._timer2.Reset();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (this._timer1.Expired && !this._timer2.Running && !this._timer2.Expired)
        this._timer2.Start();
      if (!this._timer2.Expired || this.ExpiredEvent == null)
        return;
      this.ExpiredEvent(this);
    }

    public float Delta
    {
      get
      {
        if (this._timer2.Running)
          return this._timer2.Delta;
        return !this.Started || !this.Expired ? 0.0f : 1f;
      }
    }

    public float FloatSmoothStep(float start, float end)
    {
      float delta = this.Delta;
      return MathHelper.SmoothStep(start, end, delta);
    }

    public Vector2 Vector2SmoothStep(Vector2 start, Vector2 end)
    {
      float delta = this.Delta;
      return Vector2.SmoothStep(start, end, delta);
    }

    public Vector3 Vector3SmoothStep(Vector3 start, Vector3 end)
    {
      float delta = this.Delta;
      return Vector3.SmoothStep(start, end, delta);
    }

    public Vector2 Vector2Lerp(Vector2 start, Vector2 end)
    {
      float delta = this.Delta;
      return Vector2.Lerp(start, end, delta);
    }

    public Vector3 Vector3Lerp(Vector3 start, Vector3 end)
    {
      float delta = this.Delta;
      return Vector3.Lerp(start, end, delta);
    }

    public Vector4 Vector4Lerp(Vector4 start, Vector4 end)
    {
      float delta = this.Delta;
      return Vector4.Lerp(start, end, delta);
    }

    public float SecondsUntilExpire
    {
      set => this._timer2.SecondsUntilExpire = value;
      get => this._timer2.SecondsUntilExpire;
    }

    public float SecondsUntilStart
    {
      get => this._timer1.SecondsUntilExpire;
      set => this._timer1.SecondsUntilExpire = value;
    }

    public float MillisecondsUntilExpire
    {
      set => this._timer2.MillisecondsUntilExpire = value;
      get => this._timer2.MillisecondsUntilExpire;
    }

    public float MillisecondsUntilStart
    {
      get => this._timer1.MillisecondsUntilExpire;
      set => this._timer1.MillisecondsUntilExpire = value;
    }

    public void Start()
    {
      this._timer2.Reset();
      this._timer1.Start();
    }
  }
}
