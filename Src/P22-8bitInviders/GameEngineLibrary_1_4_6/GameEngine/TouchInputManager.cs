// Decompiled with JetBrains decompiler
// Type: GameEngine.TouchInputManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

#nullable disable
namespace GameEngine
{
  public class TouchInputManager
  {
    private TouchInput _Touch1;
    private TouchInput _Touch2;
    private TouchInput _Touch3;
    private TouchInput _Touch4;
    private int[] lockedTouchID;

    public TouchInput this[int touchIndex]
    {
      get
      {
        switch (touchIndex)
        {
          case 1:
            return this._Touch1;
          case 2:
            return this._Touch2;
          case 3:
            return this._Touch3;
          case 4:
            return this._Touch4;
          default:
            return this._Touch1;
        }
      }
    }

    public TouchInputManager()
    {
      this._Touch1 = new TouchInput();
      this._Touch2 = new TouchInput();
      this._Touch3 = new TouchInput();
      this._Touch4 = new TouchInput();
      this.lockedTouchID = new int[4];
      for (int index = 0; index < 4; ++index)
        this.lockedTouchID[index] = -1;
    }

    public void Update()
    {
      this._Touch1.Update();
      this._Touch2.Update();
      this._Touch3.Update();
      this._Touch4.Update();
    }
  }
}
