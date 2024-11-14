// Decompiled with JetBrains decompiler
// Type: GameManager.OptionsScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class OptionsScreen : GameScreen
  {
    private UIManager uiNode;
    private WidgetCheckBox musicCB;
    private WidgetCheckBox soundCB;
    private WidgetCheckBox shootCB;
    private WidgetCheckBox inputCB;

    public OptionsScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
      this.AddSpriteSheet("sheets/title.xml");
    }

    public override void Initialize()
    {
      this.musicCB = new WidgetCheckBox("unchecked", "checked", new Vector2(390f, 330f), "sheets/title", (GameScreen) this);
      this.musicCB.pressedColor = Color.Red;
      this.musicCB.isChecked = MusicManager.AppEnableMusic;
      this.musicCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.musicCB_OnChecked);
      this.soundCB = new WidgetCheckBox("unchecked", "checked", new Vector2(390f, 430f), "sheets/title", (GameScreen) this);
      this.soundCB.pressedColor = Color.Red;
      this.soundCB.isChecked = MusicManager.SoundEnabled;
      this.soundCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.soundCB_OnChecked);
      this.shootCB = new WidgetCheckBox("unchecked", "checked", new Vector2(430f, 690f), "sheets/title", (GameScreen) this);
      this.shootCB.pressedColor = Color.Red;
      this.shootCB.isChecked = Globals.saveManager.optionsState.autoShoot;
      this.shootCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.shootCB_OnChecked);
      this.inputCB = new WidgetCheckBox("touchunchecked", "touchchecked", new Vector2(240f, 586f), "sheets/title", (GameScreen) this);
      this.inputCB.isChecked = Globals.saveManager.optionsState.usingTouch;
      this.inputCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.inputCB_OnChecked);
      if (this.inputCB.isChecked)
      {
        this.shootCB.isChecked = true;
        this.shootCB.Enable(false);
        Globals.saveManager.optionsState.autoShoot = true;
      }
      Layer2D childItem = new Layer2D((GameScreen) this);
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.RegisterWidget((Widget) this.musicCB);
      this.uiNode.RegisterWidget((Widget) this.soundCB);
      this.uiNode.RegisterWidget((Widget) this.shootCB);
      this.uiNode.RegisterWidget((Widget) this.inputCB);
      this.uiNode.FireTapWithName += new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      childItem.Add((SceneNode) this.uiNode);
      childItem.Add((SceneNode) new Sprite("audio", new Vector2(211f, 540f), "sheets/title", (GameScreen) this));
      childItem.SetSourceRect();
      this.sceneGraph.Add((SceneNode) childItem);
    }

    private void inputCB_OnChecked(int ID, bool checkedStatus)
    {
      Globals.saveManager.optionsState.usingTouch = checkedStatus;
      if (checkedStatus)
      {
        this.shootCB.isChecked = true;
        this.shootCB.Enable(false);
        Globals.saveManager.optionsState.autoShoot = true;
      }
      else
        this.shootCB.Enable(true);
      Globals.saveManager.SaveOptionsHiscores();
    }

    private void shootCB_OnChecked(int ID, bool checkedStatus)
    {
      Globals.saveManager.optionsState.autoShoot = checkedStatus;
      Globals.saveManager.SaveOptionsHiscores();
    }

    public void CheckMusic(bool en) => this.musicCB.isChecked = en;

    private void musicCB_OnChecked(int ID, bool checkedStatus)
    {
      if (MusicManager.shouldConfirmMusic && !MusicManager.IsMusicAllowed)
        this.screenManager.AddScreen((GameScreen) new ConfirmMusic());
      else if (checkedStatus)
      {
        MusicManager.AllowEnableMusic();
        MusicManager.Play("music/title2", true);
      }
      else
      {
        MusicManager.AppEnableMusic = false;
        MusicManager.Stop();
      }
    }

    private void soundCB_OnChecked(int ID, bool checkedStatus)
    {
      MusicManager.SoundEnabled = checkedStatus;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.audioManager.AddSound("click", "sounds/buttonpress");
    }

    private void uiNode_FireTapWithName(int ID, string btnName)
    {
      this.audioManager.Play("click");
      if (!(btnName == "resetbutton"))
        return;
      Globals.saveManager.ResetScores();
      this.uiNode[0].Enable(false);
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
      {
        this.GetScreenByType<TitleScreen>().EnableUI();
        this.ExitScreen();
      }
      base.HandleInput(gesture);
    }
  }
}
