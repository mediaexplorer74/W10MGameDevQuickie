// Decompiled with JetBrains decompiler
// Type: GameEngine.UIManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class UIManager : SceneNode, ITouchManager
  {
    public UIManager.OnSliderValueChangedHandler FireSliderValueChanged;
    public bool isDisabled;
    private float Opacity = 1f;
    private List<Widget> widgetList;
    private List<Widget> widgetListTemp;
    private List<Widget> originalOrder;

    public event UIManager.OnPressedWithNameHandler FirePressedWithName;

    public event UIManager.OnTapBWithNameHandler FireTapWithName;

    public event UIManager.OnTapHandler FireTap;

    public event UIManager.OnPressedHandler FirePressed;

    public event UIManager.OnReleasedHandler FireReleased;

    public event UIManager.OnCheckedHandler FireChecked;

    public float MasterOpacity
    {
      get => this.Opacity;
      set => this.Opacity = value;
    }

    public Widget this[int idx]
    {
      get
      {
        foreach (Widget widget in this.widgetList)
        {
          if (widget.WidgetID == idx)
            return widget;
        }
        return (Widget) null;
      }
    }

    public Widget this[string name]
    {
      get
      {
        foreach (Widget widget in this.widgetList)
        {
          if (widget.Name == name)
            return widget;
        }
        return (Widget) null;
      }
    }

    public UIManager(GameScreen screen)
      : base("UIMANAGER", screen)
    {
      this.handleInput = true;
      this.blockInput = false;
      this.widgetList = new List<Widget>();
      this.widgetListTemp = new List<Widget>();
      this.originalOrder = new List<Widget>();
    }

    public void RegisterWidget(Widget widget)
    {
      widget.WidgetID = this.widgetList.Count;
      this.widgetList.Add(widget);
      this.originalOrder.Add(widget);
      widget.uiManager = (ITouchManager) this;
    }

    public void RegisterWidget(Widget widget, int id)
    {
      widget.WidgetID = id;
      this.widgetList.Add(widget);
      this.originalOrder.Add(widget);
      widget.uiManager = (ITouchManager) this;
    }

    public bool HandleTouch(GestureSample gesture)
    {
      if (this.isDisabled)
        return false;
      this.widgetListTemp.Clear();
      foreach (Widget widget in this.widgetList)
        this.widgetListTemp.Add(widget);
      for (int index = this.widgetListTemp.Count - 1; index >= 0; --index)
      {
        if (this.widgetListTemp[index].HandleTouch(gesture, Engine.touchInputManager[1]))
        {
          this.widgetList.Remove(this.widgetListTemp[index]);
          this.widgetList.Add(this.widgetListTemp[index]);
          return true;
        }
      }
      this.widgetList.Clear();
      foreach (Widget widget in this.originalOrder)
        this.widgetList.Add(widget);
      return this.HandleInput(gesture);
    }

    public void Enable(bool enable)
    {
      foreach (Widget widget in this.widgetList)
        widget.Enable(enable);
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      if (!this.isDisabled)
      {
        foreach (SceneNode sceneNode in this.originalOrder)
          sceneNode.Update(gameTime, ref worldTransform);
      }
      base.Update(gameTime, ref worldTransform);
    }

    public override void Draw()
    {
      if (!this.isDisabled)
      {
        foreach (SceneNode sceneNode in this.originalOrder)
          sceneNode.Draw();
      }
      base.Draw();
    }

    public override void SetSourceRect()
    {
      foreach (SceneNode widget in this.widgetList)
        widget.SetSourceRect();
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.SetSourceRect();
    }

    public void OnTap(int id, string name)
    {
      if (this.FireTap != null)
        this.FireTap(id);
      if (this.FireTapWithName == null)
        return;
      this.FireTapWithName(id, name);
    }

    public void OnPressed(int id, string name)
    {
      if (this.FirePressed != null)
        this.FirePressed(id);
      if (this.FirePressedWithName == null)
        return;
      this.FirePressedWithName(id, name);
    }

    public void OnReleased(int id)
    {
      if (this.FireReleased == null)
        return;
      this.FireReleased(id);
    }

    public void OnChecked(int id, bool checkedstatus)
    {
      if (this.FireChecked == null)
        return;
      this.FireChecked(id, checkedstatus);
    }

    public void OnSliderChanged(int id, float value)
    {
      if (this.FireSliderValueChanged == null)
        return;
      this.FireSliderValueChanged(id, value);
    }

    public delegate void OnPressedWithNameHandler(int ID, string btnName);

    public delegate void OnTapBWithNameHandler(int ID, string btnName);

    public delegate void OnTapHandler(int ID);

    public delegate void OnPressedHandler(int ID);

    public delegate void OnReleasedHandler(int ID);

    public delegate void OnCheckedHandler(int ID, bool checkedStatus);

    public delegate void OnSliderValueChangedHandler(int ID, float newValue);
  }
}
