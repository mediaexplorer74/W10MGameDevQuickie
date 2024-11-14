// Decompiled with JetBrains decompiler
// Type: GameEngine.AccelerometerInput
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Windows.Devices.Sensors;
using System;

#nullable disable
namespace GameEngine
{
  public class AccelerometerInput
  {
    private const double LowPassFilterCoef = 0.1;
    private const double NoiseMaxAmplitude = 0.05;
    private Accelerometer accelerometer_sensor;
    private double lastX;
    private double lastY;
    private double lastZ;
    private double _X;
    private double _Y;
    private double _Z;
    private bool _useLowPassFilter;
    private bool _useOptimallyFilteredAcceleration = true;
    public double deltaX;
    public double deltaY;
    public double deltaZ;

    public double X => this._X;

    public double Y => this._Y;

    public double Z => this._Z;

    public double Magnitude => Math.Sqrt(this._X * this._X + this._Y * this._Y + this._Z * this._Z);

    public bool useLowPassFilter
    {
      get => this._useLowPassFilter;
      set
      {
        this._useLowPassFilter = value;
        if (!value)
          return;
        this._useOptimallyFilteredAcceleration = false;
      }
    }

    public bool useOptimalyFilteredAcceleration
    {
      get => this._useOptimallyFilteredAcceleration;
      set
      {
        this._useOptimallyFilteredAcceleration = value;
        if (!value)
          return;
        this._useLowPassFilter = false;
      }
    }

    public AccelerometerInput()
    {
      //this.accelerometer_sensor = new Accelerometer();
      //((SensorBase<AccelerometerReading>) this.accelerometer_sensor).CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(this.accelerometer_sensor_CurrentValueChanged);
    }

    private void accelerometer_sensor_CurrentValueChanged(
      object sender, AccelerometerReading e)
    {
      AccelerometerReading sensorReading1 = default;//e.SensorReading;

      this._X = 0;  // (double)  sensorReading1.Acceleration.X;

      AccelerometerReading sensorReading2 = default;//e.SensorReading;

      this._Y = 0;  // (double) sensorReading2.Acceleration.Y;

      AccelerometerReading sensorReading3 = default;//e.SensorReading;

     this._Z = 0;  // (double) sensorReading3.Acceleration.Z;

      if (this.useLowPassFilter)
      {
        this._X = this.LowPassFilter(this.X, this.lastX);
        this._Y = this.LowPassFilter(this.Y, this.lastY);
        this._Z = this.LowPassFilter(this.Z, this.lastZ);
      }
      else if (this.useOptimalyFilteredAcceleration)
      {
        this._X = this.FastLowAmplitudeNoiseFilter(this.X, this.lastX);
        this._Y = this.FastLowAmplitudeNoiseFilter(this.Y, this.lastY);
        this._Z = this.FastLowAmplitudeNoiseFilter(this.Z, this.lastZ);
      }
      this.deltaX = this.X - this.lastX;
      this.deltaY = this.Y - this.lastY;
      this.deltaZ = this.Z - this.lastZ;
      this.lastX = this.X;
      this.lastY = this.Y;
      this.lastZ = this.Z;
    }

    public void Start()
    {
      try
      {
        //this.accelerometer_sensor.Start();
      }
      catch
      {
      }
    }

    public void Stop()
    {
      try
      {
        //this.accelerometer_sensor.Stop();
      }
      catch
      {
      }
    }

    private double LowPassFilter(double newInputValue, double priorOutputValue)
    {
      return priorOutputValue + 0.1 * (newInputValue - priorOutputValue);
    }

    private double FastLowAmplitudeNoiseFilter(double newInputValue, double priorOutputValue)
    {
      double num = newInputValue;
      if (Math.Abs(newInputValue - priorOutputValue) <= 0.05)
        num = priorOutputValue + 0.1 * (newInputValue - priorOutputValue);
      return num;
    }
  }
}
