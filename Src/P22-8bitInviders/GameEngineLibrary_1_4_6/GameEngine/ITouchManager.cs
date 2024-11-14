// Decompiled with JetBrains decompiler
// Type: GameEngine.ITouchManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

#nullable disable
namespace GameEngine
{
  public interface ITouchManager
  {
    void OnTap(int ID, string Name);

    void OnPressed(int ID, string Name);

    void OnReleased(int ID);

    void OnChecked(int ID, bool checkedstatus);

    void OnSliderChanged(int ID, float value);

    float MasterOpacity { get; set; }
  }
}
