// Decompiled with JetBrains decompiler
// Type: GameEngine.Engine
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

//using Microsoft.Phone.Shell;
using GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

#nullable disable
namespace GameEngine
{
  public static class Engine
  {
    public static int gameWidth;
    public static int gameHeight;
    public static GameOrientation Orientation;
    public static bool isObscured;
    public static bool isPaused;
    public static bool PreMultiply;
    public static GraphicsDeviceManager gdm;
    public static Game game;
    public static ScreenManager screenManager;
    public static AniManager aniManager;
    public static Random random;
    public static BlendState BlendColorBlendState;
    public static BlendState BlendAlphaBlendState;

    public static TouchInputManager touchInputManager
    {
        get
        {
            return Engine.screenManager.touchInputManager;
        }
    }

    public static event LaunchingHandler OnLaunching;

    public static event ActivatedHandler OnActivated;

    public static event DeactivatedHandler OnDeactivated;

    public static event ClosingHandler OnClosing;

    public static event ObscuredHandler OnObscured;

    public static event UnobscuredHandler OnUnobscured;

    public static ContentManager GlobalContent => Engine.game.Content;

    public static void Initialize(
      Game g,
      GraphicsDeviceManager graphics,
      GameOrientation orientation
      )
    {            
      Engine.random = new Random();
      Engine.game = g;
      Engine.gdm = graphics;
      Engine.gdm.IsFullScreen = false;//true;
      Engine.isObscured = false;
      Engine.isPaused = false;
      Engine.Orientation = orientation;
      Engine.PreMultiply = false;


      TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag;
      
      if (orientation == GameOrientation.Portrait)
      {
        Engine.gdm.PreferredBackBufferWidth = Engine.gameWidth = 480;
        Engine.gdm.PreferredBackBufferHeight = Engine.gameHeight = 800;
      }
      else
      {
        Engine.gdm.PreferredBackBufferWidth = Engine.gameWidth = 800;
        Engine.gdm.PreferredBackBufferHeight = Engine.gameHeight = 480;
      }


      Engine.aniManager = new AniManager(Engine.game);

      Engine.game.Components.Add((IGameComponent) Engine.aniManager);

      Engine.screenManager = new ScreenManager(Engine.game);

      Engine.game.Components.Add((IGameComponent) Engine.screenManager);

      Engine.BlendColorBlendState = new BlendState()
      {
        ColorDestinationBlend = Blend.Zero,
        ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue,
        AlphaDestinationBlend = Blend.Zero,
        AlphaSourceBlend = Blend.SourceAlpha,
        ColorSourceBlend = Blend.SourceAlpha
      };
      Engine.BlendAlphaBlendState = new BlendState()
      {
        ColorWriteChannels = ColorWriteChannels.Alpha,
        AlphaDestinationBlend = Blend.Zero,
        ColorDestinationBlend = Blend.Zero,
        AlphaSourceBlend = Blend.One,
        ColorSourceBlend = Blend.One
      };

            /*
            PhoneApplicationService.Current.Launching 
                      += new EventHandler<LaunchingEventArgs>(Engine.Current_Launching);
            PhoneApplicationService.Current.Deactivated 
                      += new EventHandler<DeactivatedEventArgs>(Engine.Current_Deactivated);
            PhoneApplicationService.Current.Activated 
                      += new EventHandler<ActivatedEventArgs>(Engine.Current_Activated);
            PhoneApplicationService.Current.Closing 
                      += new EventHandler<ClosingEventArgs>(Engine.Current_Closing);
            */

            Engine.game.Activated += new EventHandler<EventArgs>(Engine.game_UnObscured);
            Engine.game.Deactivated += new EventHandler<EventArgs>(Engine.game_Obscured);

            //Experimental
            //Engine.game.Activated += new EventHandler<EventArgs>(Engine.Current_Activated);
            //Engine.game.Deactivated += new EventHandler<EventArgs>(Engine.Current_Deactivated);
        }

    private static void game_UnObscured(object sender, EventArgs e)
    {
      Engine.isObscured = false;
      if (Engine.OnUnobscured == null)
        return;
      Engine.OnUnobscured();
    }

    private static void game_Obscured(object sender, EventArgs e)
    {
      Engine.isObscured = true;
      if (Engine.OnObscured == null)
        return;
      Engine.OnObscured();
    }

    private static void Current_Closing(object sender, /*Closing*/EventArgs e)
    {
      Engine.screenManager.OnClosing();
      if (Engine.OnClosing == null)
        return;
      Engine.OnClosing();
    }

    private static void Current_Launching(object sender, /*Launching*/EventArgs e)
    {
      if (Engine.OnLaunching == null)
        return;
      Engine.OnLaunching();
    }

    private static void Current_Activated(object sender, /*Activated*/EventArgs e)
    {

      // ------------------------
      //Engine.isObscured = false;
      //if (Engine.OnUnobscured == null)
      //    return;
      //Engine.OnUnobscured();
      //-------------------------

      if (/*e.IsApplicationInstancePreserved && */!MusicManager.MusicCheck())
      {
        MusicManager.DisallowDisableMusic();
        MusicManager.shouldConfirmMusic = true;
      }
      Engine.screenManager.OnActivating(/*e.IsApplicationInstancePreserved*/default);
      if (Engine.OnActivated == null)
        return;

      Engine.OnActivated(/*e.IsApplicationInstancePreserved*/default);
    }

    private static void Current_Deactivated(object sender, /*Deactivated*/EventArgs e)
    {
      // ------------------------
      //Engine.isObscured = true;
      //if (Engine.OnObscured == null)
      //    return;
      //Engine.OnObscured();
      //-------------------------

      Engine.screenManager.OnDeactivating();

      if (Engine.OnDeactivated == null)
        return;

      Engine.OnDeactivated();
    }

    public static void EnableLockScreen(bool tf)
    {
        //Guide.IsScreenSaverEnabled = tf;
    }

    public static void SimulateTrialTestMode()
    {
    }

    public static void ForceSimulateTrialTestMode()
    {
        //Guide.SimulateTrialMode = true;
    }

    public static bool IsTrialMode() => false;//Guide.IsTrialMode;
  }
}
