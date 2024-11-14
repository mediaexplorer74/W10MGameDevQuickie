// Decompiled with JetBrains decompiler
// Type: GameEngine.TimerManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public sealed class TimerManager
  {
    private readonly List<Timer> _timerList = new List<Timer>();
    private readonly List<Timer> _removeList = new List<Timer>();
    private readonly List<StopwatchTimer> _stopwatchRemoveList = new List<StopwatchTimer>();
    private readonly List<StopwatchTimer> _stopwatchTimerList = new List<StopwatchTimer>();
    private static readonly TimerManager _instance = new TimerManager();
    private List<Timer> timerCopy = new List<Timer>();
    private List<Timer> tRemoveCopy = new List<Timer>();
    private List<StopwatchTimer> stopwatchCopy = new List<StopwatchTimer>();
    private List<StopwatchTimer> swRemoveCopy = new List<StopwatchTimer>();

    public static TimerManager Instance => TimerManager._instance;

    internal List<StopwatchTimer> StopwatchTimerList => this._stopwatchTimerList;

    internal List<Timer> TimerList => this._timerList;

    public void Remove(StopwatchTimer timer) => this._stopwatchRemoveList.Add(timer);

    public void Add(StopwatchTimer timer) => this._stopwatchTimerList.Add(timer);

    public void Remove(Timer timer) => this._removeList.Add(timer);

    public void Add(Timer timer) => this._timerList.Add(timer);

    public void Update(GameTime time)
    {
      if (Engine.isObscured || Engine.isPaused)
        return;
      this.timerCopy.Clear();
      this.tRemoveCopy.Clear();
      this.stopwatchCopy.Clear();
      this.swRemoveCopy.Clear();
      foreach (Timer timer in this._timerList)
        this.timerCopy.Add(timer);
      foreach (Timer remove in this._removeList)
        this.tRemoveCopy.Add(remove);
      foreach (StopwatchTimer stopwatchTimer in this._stopwatchTimerList)
        this.stopwatchCopy.Add(stopwatchTimer);
      foreach (StopwatchTimer stopwatchRemove in this._stopwatchRemoveList)
        this.swRemoveCopy.Add(stopwatchRemove);
      foreach (Timer timer in this.timerCopy)
        timer.Update(time);
      foreach (StopwatchTimer stopwatchTimer in this.stopwatchCopy)
        stopwatchTimer.Update(time);
      foreach (Timer timer in this.tRemoveCopy)
        this._timerList.Remove(timer);
      foreach (StopwatchTimer stopwatchTimer in this.swRemoveCopy)
        this._stopwatchTimerList.Remove(stopwatchTimer);
      this._removeList.Clear();
      this._stopwatchRemoveList.Clear();
    }
  }
}
