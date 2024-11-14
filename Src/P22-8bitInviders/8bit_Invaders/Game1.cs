// Decompiled with JetBrains decompiler
// Type: GameManager.Main
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input; //
using System;

#nullable disable
namespace GameManager
{
  public class Game1 : Game
  {

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    Vector2 baseScreenSize = new Vector2(800, 480);
    public static Matrix globalTransformation; // static is bad idea?
    int backbufferWidth, backbufferHeight;



    public SaveGameManager saveManager;


    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);



        //DEBUG: IsFullScreen = *false*
        graphics.IsFullScreen = false;//set it *true* for W10M

        //RnD
        graphics.PreferredBackBufferWidth = 800;
        graphics.PreferredBackBufferHeight = 480;

        graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft
            | DisplayOrientation.LandscapeRight;// | DisplayOrientation.Portrait;

      // force it!
      //ScalePresentationArea();

      Accelerometer.Initialize();


      this.Content.RootDirectory = "Content";
      
      Engine.Initialize((Game) this, this.graphics, GameOrientation.Portrait);

      Engine.OnActivated += new ActivatedHandler(this.Engine_OnActivated);
      Engine.OnClosing += new ClosingHandler(this.Engine_OnClosing);
      Engine.OnDeactivated += new DeactivatedHandler(this.Engine_OnDeactivated);
      Engine.OnLaunching += new LaunchingHandler(this.Engine_OnLaunching);
      
      this.Exiting += new EventHandler<EventArgs>(this.Main_Exiting);
      

      Tools.StartMemoryMonitor();
       
      this.TargetElapsedTime = TimeSpan.FromTicks(333333L);
      this.InactiveSleepTime = TimeSpan.FromSeconds(1.0);
    }

    private void StartGame()
    {
      MusicManager.LoadMusicSettings();
      this.saveManager = new SaveGameManager();
      this.saveManager.LoadGame();
      this.saveManager.UpdateTile();

      LoadingScreen.LoadLogo(0.7f, (GameScreen) new TitleScreen());
    }

    private void Engine_OnLaunching()
    {
       this.StartGame();
    }

    private void Engine_OnActivated(bool IsApplicationInstancePreserved)
    {
      //if (IsApplicationInstancePreserved)
      //  return;
      //
      this.StartGame();
    }

    private void Engine_OnDeactivated()
    {
        MusicManager.SaveMusicSettings();
    }

    private void Engine_OnClosing()
    { 
        MusicManager.SaveMusicSettings(); 
    }

    protected override void Initialize()
    {
 
        base.Initialize();

        this.StartGame();
   }

    protected override void LoadContent()
    {
        // ?
        this.Content.RootDirectory = "Content";


        this.spriteBatch = new SpriteBatch(this.GraphicsDevice);


        ScalePresentationArea();

            // right place ?
            //Engine_OnActivated(false);

    }



    public void ScalePresentationArea()
    {
        //Work out how much we need to scale our graphics to fill the screen
        backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth - 0; // 40 - dirty hack for Astoria!
        backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        float horScaling = backbufferWidth / baseScreenSize.X;
        float verScaling = backbufferHeight / baseScreenSize.Y;

        Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);

        globalTransformation = Matrix.CreateScale(screenScalingFactor);

        System.Diagnostics.Debug.WriteLine("Screen Size - Width["
            + GraphicsDevice.PresentationParameters.BackBufferWidth + "] " +
            "Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");
   
    }//Scale...



    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {

      
        //Confirm the screen has not been resized by the user
        if (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight ||
            backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
        {
            ScalePresentationArea();
        }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
           || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            KeyboardState keyState = Keyboard.GetState();
           
            //RnD
            if (keyState.IsKeyDown(Keys.F2))
            {
                this.graphics.ToggleFullScreen();
            }


            base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(Color.Black);

      //TODO
      //this.spriteBatch.Begin(SpriteSortMode.Deferred, null,
      //       null, null, null, null, globalTransformation);
      // ...
      //this.spriteBatch.End();

      base.Draw(gameTime);
    }

    private void Main_Exiting(object sender, EventArgs e)
    {
      //
    }

    private void ClearGestures()
    {
      while (TouchPanel.IsGestureAvailable)
        TouchPanel.ReadGesture();
    }

  }
}
