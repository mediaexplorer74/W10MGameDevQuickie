// Decompiled with JetBrains decompiler
// Type: GameEngine.Timer
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class Timer
  {
    public string Name;
    public bool Running;
    public bool Expired;
    private float _timeExpired;
    private float _timeTarget;

    public event TimerExpiredHandler ExpiredEvent;

    public Timer(string name)
    {
      this.Name = name;
      this.Reset();
    }

    ~Timer() => TimerManager.Instance.TimerList.Remove(this);

    public void Reset()
    {
      this.Expired = false;
      this.Running = false;
      this._timeExpired = 0.0f;
    }

    public float Delta
    {
      get => (double) this._timeTarget != -1.0 ? this._timeExpired / this._timeTarget : 0.0f;
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

    public void Start()
    {
      if (!this.Running && !TimerManager.Instance.TimerList.Contains(this))
        TimerManager.Instance.Add(this);
      this._timeExpired = 0.0f;
      this.Running = true;
    }

    public void Stop()
    {
      this.Running = false;
      TimerManager.Instance.Remove(this);
    }

    public void Update(GameTime time)
    {
      if (!this.Running)
        return;
      this._timeExpired += (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this._timeExpired < (double) this._timeTarget || (double) this._timeTarget == -1.0)
        return;
      this.Expired = true;
      if (this.ExpiredEvent != null)
        this.ExpiredEvent(this);
      this.Stop();
    }

    public float SecondsUntilExpire
    {
      set => this._timeTarget = (float) (int) ((double) value * 1000.0);
      get => this._timeTarget / 1000f;
    }

    public float MillisecondsUntilExpire
    {
      set => this._timeTarget = (float) (int) value;
      get => this._timeTarget;
    }
  }
}
