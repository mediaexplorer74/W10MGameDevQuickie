// Decompiled with JetBrains decompiler
// Type: dpLogo.dpLogoDisplayer
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

//using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace dpLogo
{
  public class dpLogoDisplayer : DrawableGameComponent
  {
    private const int startTime = 2;
    private const int logoTime = 2;
    private const int speed = 14;
    public SpriteBatch SpriteBatch;
    public bool Finished;
    private Game myGame;
    private GraphicsDeviceManager graphics;
    private SpriteFont myFont;
    private Texture2D whiteTexture;
    private Texture2D blackTexture;
    private Texture2D dpLeft;
    private Texture2D dpRight;
    private Rectangle topLeftRec;
    private Rectangle topRightRec;
    private Rectangle bottomLeftRec;
    private Rectangle bottomRightRec;
    private Rectangle bottomRightRec2;
    private dpLogoDisplayer.Status myStatus;
    private int topTime;
    private int myLogoTime;
    private int width;
    private int widthDiv2;
    private int height;
    private int heightDiv2;
    private Vector2 v1;
    private Vector2 v2;
    private Vector2 v3;
    private Vector2 v4;
    private Vector2 v5;
    private Vector2 v6;
    private int ticks;

    public dpLogoDisplayer(Game aGame, GraphicsDeviceManager aGraphics)
      : base(aGame)
    {
      this.myGame = aGame;
      this.graphics = aGraphics;
      this.myStatus = dpLogoDisplayer.Status.Black;
    }

    public override void Initialize()
    {
      this.myGame.IsFixedTimeStep = true;
      this.whiteTexture = new Texture2D(this.Game.GraphicsDevice, 1, 1);
      this.whiteTexture.SetData<Color>(new Color[1]
      {
        Color.White
      });
      this.blackTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
      this.blackTexture.SetData<Color>(new Color[1]
      {
        Color.Black
      });
      this.topLeftRec = new Rectangle();
      this.topRightRec = new Rectangle();
      this.bottomLeftRec = new Rectangle();
      this.bottomRightRec = new Rectangle();
      this.bottomRightRec2 = new Rectangle();
      this.width = this.graphics.PreferredBackBufferWidth;
      this.widthDiv2 = this.width / 2;
      this.height = this.graphics.PreferredBackBufferHeight;
      this.heightDiv2 = this.height / 2;
      this.topTime = this.graphics.PreferredBackBufferHeight;
      this.v1 = new Vector2();
      this.v2 = new Vector2();
      this.v3 = new Vector2();
      this.v4 = new Vector2();
      this.v5 = new Vector2();
      this.v6 = new Vector2();
      base.Initialize();
    }

    public void LoadTheContent()
    {
        this.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
      this.ticks += 14;
      switch (this.myStatus)
      {
        case dpLogoDisplayer.Status.Black:
          this.calcBlack();
          break;
        case dpLogoDisplayer.Status.Top:
          this.calcTop();
          break;
        case dpLogoDisplayer.Status.Left1:
          this.calcLeft1();
          break;
        case dpLogoDisplayer.Status.Left2:
          this.calcLeft2();
          break;
        case dpLogoDisplayer.Status.Logo:
          --this.myLogoTime;
          if (this.myLogoTime <= 0)
          {
            this.Finished = true;
            break;
          }
          break;
      }
      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      this.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      switch (this.myStatus)
      {
        case dpLogoDisplayer.Status.Black:
          this.SpriteBatch.Draw(this.blackTexture, this.topLeftRec, Color.White);
          break;
        case dpLogoDisplayer.Status.Top:
          this.SpriteBatch.Draw(this.blackTexture, this.topLeftRec, Color.White);
          this.SpriteBatch.Draw(this.whiteTexture, this.topRightRec, Color.White);
          this.SpriteBatch.Draw(this.blackTexture, this.bottomRightRec, Color.White);
          break;
        case dpLogoDisplayer.Status.Left1:
        case dpLogoDisplayer.Status.Left2:
          this.SpriteBatch.Draw(this.blackTexture, this.topLeftRec, Color.White);
          this.SpriteBatch.Draw(this.whiteTexture, this.topRightRec, Color.White);
          this.SpriteBatch.Draw(this.whiteTexture, this.bottomLeftRec, Color.White);
          this.SpriteBatch.Draw(this.blackTexture, this.bottomRightRec, Color.White);
          this.SpriteBatch.Draw(this.whiteTexture, this.bottomRightRec2, Color.White);
          break;
        case dpLogoDisplayer.Status.Logo:
          this.SpriteBatch.Draw(this.blackTexture, this.topLeftRec, Color.White);
          this.SpriteBatch.Draw(this.whiteTexture, this.topRightRec, Color.White);
          this.SpriteBatch.Draw(this.whiteTexture, this.bottomLeftRec, Color.White);
          this.SpriteBatch.Draw(this.blackTexture, this.bottomRightRec, Color.White);
          this.v1.X = (float) (this.widthDiv2 - this.dpLeft.Width);
          this.v1.Y = (float) (this.heightDiv2 - this.dpLeft.Height);
          this.SpriteBatch.Draw(this.dpLeft, this.v1, Color.White);
          this.v2.X = (float) this.widthDiv2;
          this.v2.Y = (float) (this.heightDiv2 - this.dpRight.Height);
          this.SpriteBatch.Draw(this.dpRight, this.v2, Color.White);
          this.v3.X = (float) ((double) this.widthDiv2 - (double) this.myFont.MeasureString("d").X - 3.0);
          this.v3.Y = (float) (this.heightDiv2 - this.dpLeft.Height + 10);
          this.SpriteBatch.DrawString(this.myFont, "d", this.v3, Color.Black);
          this.v4.X = (float) (this.widthDiv2 + 4);
          this.v4.Y = (float) (this.heightDiv2 - this.dpRight.Height + 10);
          this.SpriteBatch.DrawString(this.myFont, "p", this.v4, Color.White);
          this.v5.X = (float) ((double) this.widthDiv2 - 
                        (double) this.myFont.MeasureString("diamond").X - 8.0);
          this.v5.Y = (float) this.heightDiv2;
          this.SpriteBatch.DrawString(this.myFont, "diamond", this.v5, Color.Black);
          this.v6.X = (float) (this.widthDiv2 + 10);
          this.v6.Y = (float) this.heightDiv2;
          this.SpriteBatch.DrawString(this.myFont, "productions", this.v6, Color.White);
          break;
      }

      //RnD
      if (false)//(PhoneApplicationService.Current.StartupMode == null)
      {
        this.v1.X = 30f;
        this.v1.Y = (float) (this.height - 60);
        this.SpriteBatch.DrawString(this.myFont, "resuming...", this.v1, Color.Black);
      }
      
            this.SpriteBatch.End();
      base.Draw(gameTime);
    }

    protected override void LoadContent()
    {
      this.myFont = this.Game.Content.Load<SpriteFont>("dpLogo\\BauhausFont");
      this.dpLeft = this.Game.Content.Load<Texture2D>("dpLogo\\dp_left_tr");
      this.dpRight = this.Game.Content.Load<Texture2D>("dpLogo\\dp_right_tr");
      base.LoadContent();
    }

    private void calcBlack()
    {
      this.topLeftRec.X = 0;
      this.topLeftRec.Y = 0;
      this.topLeftRec.Width = this.width;
      this.topLeftRec.Height = this.height;
      if (this.ticks < 2)
        return;
      this.myStatus = dpLogoDisplayer.Status.Top;
      this.calcTop();
    }

    private void calcTop()
    {
      this.topLeftRec.X = 0;
      this.topLeftRec.Y = 0;
      this.topLeftRec.Width = this.widthDiv2;
      this.topLeftRec.Height = this.height;
      this.topRightRec.X = this.topLeftRec.Width;
      this.topRightRec.Y = 0;
      this.topRightRec.Width = this.widthDiv2;
      this.topRightRec.Height = Math.Max(1, this.ticks - 2);
      this.bottomRightRec.X = this.topRightRec.X;
      this.bottomRightRec.Y = this.topRightRec.Height;
      this.bottomRightRec.Width = this.topRightRec.Width;
      this.bottomRightRec.Height = this.height - this.topRightRec.Height;
      if (this.ticks < 2 + this.topTime)
        return;
      this.myStatus = dpLogoDisplayer.Status.Left1;
      this.calcLeft1();
    }

    private void calcLeft1()
    {
      this.topLeftRec.X = 0;
      this.topLeftRec.Y = 0;
      this.topLeftRec.Width = this.widthDiv2;
      this.topLeftRec.Height = this.heightDiv2;
      this.topRightRec.X = this.topLeftRec.Width;
      this.topRightRec.Y = 0;
      this.topRightRec.Width = this.widthDiv2;
      this.topRightRec.Height = this.heightDiv2;
      this.bottomLeftRec.X = 0;
      this.bottomLeftRec.Y = this.topLeftRec.Height;
      this.bottomLeftRec.Height = this.heightDiv2;
      this.bottomLeftRec.Width = 
                (int) MathHelper.Clamp((float) (this.ticks - (2 + this.topTime)), 
                1f, (float) this.widthDiv2);
      this.bottomRightRec.X = this.bottomLeftRec.Width;
      this.bottomRightRec.Y = this.bottomLeftRec.Y;
      this.bottomRightRec.Height = this.bottomLeftRec.Height;
      this.bottomRightRec.Width = Math.Max(1, this.widthDiv2 - this.bottomLeftRec.Width);
      this.bottomRightRec2.X = this.topRightRec.X;
      this.bottomRightRec2.Y = this.bottomLeftRec.Y;
      this.bottomRightRec2.Width = this.topRightRec.Width;
      this.bottomRightRec2.Height = this.bottomLeftRec.Height;
      if (this.bottomLeftRec.Width != this.widthDiv2)
        return;
      this.myStatus = dpLogoDisplayer.Status.Left2;
      this.calcLeft2();
    }

    private void calcLeft2()
    {
      this.topLeftRec.X = 0;
      this.topLeftRec.Y = 0;
      this.topLeftRec.Width = this.widthDiv2;
      this.topLeftRec.Height = this.heightDiv2;
      this.topRightRec.X = this.topLeftRec.Width;
      this.topRightRec.Y = 0;
      this.topRightRec.Width = this.widthDiv2;
      this.topRightRec.Height = this.heightDiv2;
      this.bottomLeftRec.X = 0;
      this.bottomLeftRec.Y = this.topLeftRec.Height;
      this.bottomLeftRec.Height = this.heightDiv2;
      this.bottomLeftRec.Width = this.topLeftRec.Width;
      this.bottomRightRec.X = this.bottomLeftRec.Width;
      this.bottomRightRec.Y = this.bottomLeftRec.Y;
      this.bottomRightRec.Height = this.bottomLeftRec.Height;
      this.bottomRightRec.Width = Math.Max(1, this.ticks - (2 + this.topTime) - this.widthDiv2);
      this.bottomRightRec2.X = this.bottomRightRec.X + this.bottomRightRec.Width;
      this.bottomRightRec2.Y = this.bottomLeftRec.Y;
      this.bottomRightRec2.Width = this.topRightRec.Width - this.bottomRightRec.Width;
      this.bottomRightRec2.Height = this.bottomLeftRec.Height;
      if (this.bottomRightRec.Width < this.widthDiv2)
        return;
      this.myStatus = dpLogoDisplayer.Status.Logo;
      this.myLogoTime = 2;
    }

    public enum Status
    {
      Black,
      Top,
      Left1,
      Left2,
      Logo,
    }
  }
}
