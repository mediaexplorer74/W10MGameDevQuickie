// Decompiled with JetBrains decompiler
// Type: GameEngine.Anim
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public class Anim : IAnim
  {
    public EndBehavior endBehavior = EndBehavior.Nothing;
    public MotionType Motion;
    public float EaseOut = 2f;
    public float EaseIn = 0.7f;
    public Vector3 StartValue;
    public Vector3 TargetValue;
    protected Vector3 current;
    protected Vector3 vector;
    private float _speed;
    protected float lapSpeed;
    protected float Weight;
    public float SpringAmount;
    public bool isStaggered;
    private float StaggeredWeight;
    public int staggerIndex = 1;
    public int staggerItemCount = 1;
    public float staggerOverlap = 0.5f;
    public float BoingPower = 2.2f;
    public float BoingCoeff = 2.5f;
    public float BoingCoeff2 = 1.2f;

    public event Anim.OnCompleteHandler OnComplete;

    public AnimStatus Status { get; set; }

    public string Name { get; set; }

    public float Speed
    {
      get => this._speed;
      set
      {
        this._speed = value / 1000f;
        this.lapSpeed = this.Speed / this.vector.Length();
      }
    }

    public Anim(string name)
    {
      this.Status = AnimStatus.Stopped;
      this.Name = name;
      this.Speed = 240f;
      this.StartValue = this.current = Vector3.Zero;
      this.TargetValue = Vector3.Zero;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      Engine.aniManager.AddAnim(this);
    }

    public Anim(string name, Vector3 startvalue, Vector3 targetvalue)
    {
      this.Status = AnimStatus.Stopped;
      this.Name = name;
      this.Speed = 240f;
      this.StartValue = this.current = startvalue;
      this.TargetValue = targetvalue;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      Engine.aniManager.AddAnim(this);
    }

    public Anim(string name, float startvalue, float targetvalue)
    {
      this.Status = AnimStatus.Stopped;
      this.Name = name;
      this.Speed = 240f;
      this.StartValue = this.current = new Vector3(startvalue, 0.0f, 0.0f);
      this.TargetValue = new Vector3(targetvalue, 0.0f, 0.0f);
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      Engine.aniManager.AddAnim(this);
    }

    public virtual void UpdateAnimation(GameTime gameTime)
    {
      if (this.current == this.TargetValue && this.Status == AnimStatus.Running)
      {
        if (this.endBehavior == EndBehavior.Nothing)
        {
          this.Status = AnimStatus.Stopped;
          if (this.OnComplete == null)
            return;
          this.OnComplete(this);
        }
        else if (this.endBehavior == EndBehavior.Continue)
        {
          this.StartValue = this.current;
          this.TargetValue += this.TargetValue;
          this.Weight = 0.0f;
          this.vector = this.TargetValue - this.StartValue;
          this.lapSpeed = this.Speed / this.vector.Length();
        }
        else if (this.endBehavior == EndBehavior.Loop)
          this.Restart();
        else if (this.endBehavior == EndBehavior.Bounce)
        {
          this.TargetValue = this.StartValue;
          this.StartValue = this.current;
          this.Weight = 0.0f;
          this.vector = this.TargetValue - this.StartValue;
          this.lapSpeed = this.Speed / this.vector.Length();
        }
        else
        {
          if (this.endBehavior != EndBehavior.Remove)
            return;
          if (this.OnComplete != null)
            this.OnComplete(this);
          Engine.aniManager.RemoveAnim(this.Name);
        }
      }
      else
      {
        if (this.Status != AnimStatus.Running)
          return;
        this.Weight += this.lapSpeed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        this.StaggeredWeight = MathHelper.Clamp((this.Weight - (1f - this.staggerOverlap) * (float) this.staggerIndex / (float) this.staggerItemCount) / this.staggerOverlap, 0.0f, 1f);
        float num = !this.isStaggered ? this.Weight : this.StaggeredWeight;
        if (this.Motion == MotionType.Linear)
          this.current = this.StartValue + num * this.vector;
        else if (this.Motion == MotionType.EaseInOut)
        {
          this.current.X = MathHelper.SmoothStep(this.StartValue.X, this.TargetValue.X, num);
          this.current.Y = MathHelper.SmoothStep(this.StartValue.Y, this.TargetValue.Y, num);
          this.current.Z = MathHelper.SmoothStep(this.StartValue.Z, this.TargetValue.Z, num);
        }
        else if (this.Motion == MotionType.EaseOut)
          this.current = this.StartValue + (float) Math.Pow((double) num, (double) this.EaseOut) * this.vector;
        else if (this.Motion == MotionType.EaseIn)
          this.current = this.StartValue + (float) Math.Pow((double) num, (double) this.EaseIn) * this.vector;
        else if (this.Motion == MotionType.Boing)
          this.current = this.StartValue + (this.TargetValue - this.StartValue) * (((float) (Math.Sin((double) num * Math.PI * ((double) this.BoingCoeff * (double) num * (double) num * (double) num)) * Math.Pow(1.0 - (double) num, (double) this.BoingPower)) + num) * (float) (1.0 + (double) this.BoingCoeff2 * (1.0 - (double) num)));
        if ((double) num <= 0.99995)
          return;
        this.current = this.TargetValue;
      }
    }

    public void Start(Vector3 startvalue)
    {
      this.current = this.StartValue = startvalue;
      this.Weight = 0.0f;
      this.vector = this.TargetValue - this.current;
      this.lapSpeed = this.Speed / this.vector.Length();
      this.Status = AnimStatus.Running;
    }

    public virtual void Play()
    { 
        this.Status = AnimStatus.Running; 
    }

    public void Stop()
    {
        this.Status = AnimStatus.Stopped;
    }

    public void Pause() 
    { 
        this.Status = AnimStatus.Paused; 
    }

    public void Restart()
    {
      this.current = this.StartValue;
      this.Weight = 0.0f;
      this.vector = this.TargetValue - this.StartValue;
      this.lapSpeed = this.Speed / this.vector.Length();
      this.Status = AnimStatus.Running;
    }

    public Vector3 CurrentVector3Value => this.current;

    public Vector2 CurrentVector2Value => new Vector2(this.current.X, this.current.Y);

    public float CurrentFloatValue => this.current.X;

    public void Stagger(int index, int count, float overlap)
    {
      this.isStaggered = true;
      this.staggerIndex = index;
      this.staggerItemCount = count;
      this.staggerOverlap = overlap;
    }

    public delegate void OnCompleteHandler(Anim anim);
  }
}
