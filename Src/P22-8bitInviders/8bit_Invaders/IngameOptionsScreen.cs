// Decompiled with JetBrains decompiler
// Type: GameManager.IngameOptionsScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class IngameOptionsScreen : GameScreen
  {
    private UIManager uiNode;
    private WidgetCheckBox musicCB;
    private WidgetCheckBox soundCB;
    private WidgetCheckBox shootCB;
    private WidgetCheckBox inputCB;

    public IngameOptionsScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
      this.AddSpriteSheet("sheets/ingameopts.xml");
    }

    public override void Initialize()
    {
      string str = "sheets/ingameopts";
      this.musicCB = new WidgetCheckBox("unchecked", "checked", new Vector2(330f, 420f), str, (GameScreen) this);
      this.musicCB.pressedColor = Color.Red;
      this.musicCB.isChecked = MusicManager.AppEnableMusic;
      this.musicCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.musicCB_OnChecked);
      this.soundCB = new WidgetCheckBox("unchecked", "checked", new Vector2(330f, 490f), str, (GameScreen) this);
      this.soundCB.pressedColor = Color.Red;
      this.soundCB.isChecked = MusicManager.SoundEnabled;
      this.soundCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.soundCB_OnChecked);
      this.shootCB = new WidgetCheckBox("unchecked", "checked", new Vector2(360f, 640f), str, (GameScreen) this);
      this.shootCB.pressedColor = Color.Red;
      this.shootCB.isChecked = Globals.saveManager.optionsState.autoShoot;
      this.shootCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.shootCB_OnChecked);
      this.inputCB = new WidgetCheckBox("tuc", "tc", new Vector2(240f, 565f), str, (GameScreen) this);
      this.inputCB.isChecked = Globals.saveManager.optionsState.usingTouch;
      this.inputCB.OnChecked += new WidgetCheckBox.OnCheckedHandler(this.inputCB_OnChecked);
      if (this.inputCB.isChecked)
      {
        this.shootCB.isChecked = true;
        this.shootCB.Enable(false);
        Globals.saveManager.optionsState.autoShoot = true;
      }
      Layer2D childItem = new Layer2D((GameScreen) this);
      childItem.Add((SceneNode) new Sprite("optionsscreen", new Vector2(240f, 500f), str, (GameScreen) this));
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.FireTapWithName += new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      this.uiNode.RegisterWidget((Widget) this.musicCB);
      this.uiNode.RegisterWidget((Widget) this.soundCB);
      this.uiNode.RegisterWidget((Widget) this.shootCB);
      this.uiNode.RegisterWidget((Widget) this.inputCB);
      childItem.Add((SceneNode) this.uiNode);
      childItem.SetSourceRect();
      this.sceneGraph.Add((SceneNode) childItem);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.audioManager.AddSound("click", "sounds/buttonpress");
    }

    private void uiNode_FireTapWithName(int ID, string btnName)
    {
      this.audioManager.Play("click");
      switch (btnName)
      {
        case "options":
          MusicManager.AllowEnableMusic();
          MusicManager.shouldConfirmMusic = false;
          MusicManager.Play("music/title2", true);
          this.GetScreenByType<OptionsScreen>().CheckMusic(true);
          this.ExitScreen();
          break;
        case "no":
          this.GetScreenByType<OptionsScreen>().CheckMusic(false);
          this.ExitScreen();
          break;
      }
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
        this.ExitScreen();
      base.HandleInput(gesture);
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
        this.screenManager.AddScreen((GameScreen) new IngameConfirmMusic());
      else if (checkedStatus)
      {
        MusicManager.AllowEnableMusic();
        MusicManager.Play("music/gameplay", true);
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
  }
}
