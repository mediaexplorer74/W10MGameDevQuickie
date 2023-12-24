using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChessGame.Classes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace ChessGame
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Chess chess;
        MouseState currentMouseState;
        MouseState previousMouseState;
        Vector2 scale = new Vector2(1f, 1f);
        private Texture2D btnStart;
        private Texture2D btnModeNormal;
        private Texture2D btnModeRapid;
        private Texture2D btnModeBlitz;
        private Texture2D btnModeBullet;
        private Texture2D btnEndMainMenu;
        private Texture2D btnEndRestart;
        private Texture2D btnPaus;
        private Texture2D btnPausGame;
        SpriteFont player1time;
        SpriteFont player2time;
        SpriteFont pausedGame;
        double timeDouble;
        double stopTime;



        Square fromSquare;
        Square toSquare;
        Vector2 previousPiecePos;
        Piece draggingPiece;
        Vector2 imageOffset;

        SpriteFont match;
        String moves;
        String matchSquare;
        String allMoves;

        List<String> arrList = new List<String>();


        float deltaTime;

        // knappar för testing
        bool gameModeNormal = true;
        bool gameModeRapid = false;
        bool gameModeBlitz = false;
        bool gameModeBullet = false;
        bool checkMate = false;
        bool pause = false;
        bool mainMenuButton = false;
        bool resetButton = false;

        public enum GameStates
        {
            Menu,
            GameMode,
            Playing,
            Paused,
            EndGame
        }

        public enum GameModes
        {
            Normal,
            Rapid,
            Blitz,
            Bullet,
            LoopStopper
        }

        public GameStates _gameState = GameStates.Menu;
        public GameModes _gameMode;
        public GameModes _gameModeHolder;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "The Chess Game";
            scale = new Vector2(0.75f, 0.75f);
            chess = new Chess(scale, Content, 7200.0f);
        }

        protected override void Initialize()
        {

            base.Initialize();
            currentMouseState = Mouse.GetState();
            previousMouseState = Mouse.GetState();
        }

   
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            btnStart = Content.Load<Texture2D>("startgamebutton");
            btnModeNormal = Content.Load<Texture2D>("normalbutton");
            btnModeRapid = Content.Load<Texture2D>("rapidbutton");
            btnModeBlitz = Content.Load<Texture2D>("blitzbutton");
            btnModeBullet = Content.Load<Texture2D>("bulletbutton");
            btnEndMainMenu = Content.Load<Texture2D>("mainmenubutton");
            btnEndRestart = Content.Load<Texture2D>("restart");
            btnPaus = Content.Load<Texture2D>("pausedbutton");
            btnPausGame = Content.Load<Texture2D>("pausebutton");

            player1time = Content.Load<SpriteFont>("TextFont");//("player1timer");
            player2time = Content.Load<SpriteFont>("TextFont");//("player2timer");
            pausedGame = Content.Load<SpriteFont>("TextFont");//("paused");
            match = Content.Load<SpriteFont>("TextFont");//("history");

            chess.Board.LoadContent(Content);
            foreach (Square square in chess.Board.Squares)
            {
                square.LoadContent(Content);
            }
            foreach (Piece piece in chess.Board.Pieces)
            {
                piece.LoadContent(Content);
            }
        }


        protected override void UnloadContent()
        {
     
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
          
            switch (_gameState)
            {
                case GameStates.Menu:
                    Window.Title = "The Chess Game - Main Menu";
                   UpdateMainMenu(deltaTime);
                    break;
                case GameStates.GameMode:
                    Window.Title = "The Chess Game - Choose Game Mode";
                    UpdateGameMode(deltaTime);
                    break;
                case GameStates.Playing:
                    Window.Title = "The Chess Game - Playing a match!";
                    UpdatePlaying(deltaTime);
                    break;
                case GameStates.Paused:
                    Window.Title = "The Chess Game - Game Paused";
                    UpdatePause(deltaTime);
                    break;
                case GameStates.EndGame:
                    Window.Title = "The Chess Game - End Screen";
                    UpdateEndGame(deltaTime);
                    break;
            }
            

        }


        public void UpdateMainMenu(float deltaTime)
        {

                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();
                Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                     Vector2 buttonMainMenu = new Vector2(500, 50);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {

                if (mousePos.X >= buttonMainMenu.X && mousePos.X <= buttonMainMenu.X + btnModeNormal.Width
                    && mousePos.Y >= buttonMainMenu.Y && mousePos.Y <= buttonMainMenu.Y + btnModeNormal.Height)
                {
                    _gameState = GameStates.GameMode;
                    }
                }

        }

        public void UpdateGameMode(float deltaTime)
        {
            
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
            Vector2 buttonNormal = new Vector2(500, 50);
            Vector2 buttonBlitz = new Vector2(500, 200);
            Vector2 buttonRapid = new Vector2(500, 350);
            Vector2 buttonBullet = new Vector2(500, 500);
            

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {

                if (mousePos.X >= buttonNormal.X && mousePos.X <= buttonNormal.X + btnModeNormal.Width
                    && mousePos.Y >= buttonNormal.Y && mousePos.Y <= buttonNormal.Y + btnModeNormal.Height)
                {
                    _gameMode = GameModes.Normal;
                    _gameState = GameStates.Playing;
                }
                if (mousePos.X >= buttonBlitz.X && mousePos.X <= buttonBlitz.X + btnModeBlitz.Width
                    && mousePos.Y >= buttonBlitz.Y && mousePos.Y <= buttonBlitz.Y + btnModeBlitz.Height)
                {
                    _gameMode = GameModes.Blitz;
                    _gameState = GameStates.Playing;
                    chess.Timers[0].Time = 600.0f;
                    chess.Timers[1].Time = 600.0f;
                    chess.Timers[1].Text = String.Format("{0}:{1:00}", 600 / 60, 600 % 60);
                }
                if (mousePos.X >= buttonRapid.X && mousePos.X <= buttonRapid.X + btnModeRapid.Width
                    && mousePos.Y >= buttonRapid.Y && mousePos.Y <= buttonRapid.Y + btnModeRapid.Height)
                {
                    _gameMode = GameModes.Rapid;
                    _gameState = GameStates.Playing;
                    chess.Timers[0].Time = 3000.0f;
                    chess.Timers[1].Time = 3000.0f;
                    chess.Timers[1].Text = String.Format("{0}:{1:00}", 3000 / 60, 3000 % 60);
                }
                if (mousePos.X >= buttonBullet.X && mousePos.X <= buttonBullet.X + btnModeBullet.Width
                    && mousePos.Y >= buttonBullet.Y && mousePos.Y <= buttonBullet.Y + btnModeBullet.Height)
                {
                    _gameMode = GameModes.Bullet;
                    _gameState = GameStates.Playing;
                    chess.Timers[0].Time = 12.0f;
                    chess.Timers[1].Time = 12.0f;
                    chess.Timers[1].Text = String.Format("{0}:{1:00}", 12 / 60, 12 % 60);
                }
            }

        }

        public void UpdatePlaying(float deltaTime)
        {
            _gameModeHolder = this._gameMode;
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
            Vector2 buttonPause = new Vector2(750, 650);
            timeDouble++;

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {

                if (mousePos.X >= buttonPause.X && mousePos.X <= buttonPause.X + btnPausGame.Width
                    && mousePos.Y >= buttonPause.Y && mousePos.Y <= buttonPause.Y + btnPausGame.Height)
                {
                    stopTime = timeDouble;
                    _gameState = GameStates.Paused;
                }
            }
               

            // Clicked on piece
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {
                foreach (Square square in chess.Board.Squares)
                {
                    // If within bounds of texture
                    if (square.Piece != null && square.Piece.IsSelectable
                        && mousePos.X >= square.ImagePos.X && mousePos.X <= square.ImagePos.X + square.Image.Width
                        && mousePos.Y >= square.ImagePos.Y && mousePos.Y <= square.ImagePos.Y + square.Image.Height)
                    {
                        fromSquare = square;
                        previousPiecePos = square.Piece.ImagePos;
                        draggingPiece = square.Piece;
                        imageOffset = fromSquare.Piece.ImagePos - mousePos;
                    }
                }
            }
                

            if (draggingPiece != null)
            {
                draggingPiece.ImagePos = mousePos + imageOffset;
            }

            if (draggingPiece != null && currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton != ButtonState.Released)
            {
                bool moveMade = false;
                foreach (Square square in chess.Board.Squares)
                {
                    // If within bounds of texture
                    if (mousePos.X >= square.ImagePos.X && mousePos.X <= square.ImagePos.X + square.Image.Width
                        && mousePos.Y >= square.ImagePos.Y && mousePos.Y <= square.ImagePos.Y + square.Image.Height)
                    {
                        toSquare = square;
                        if (chess.MakeMove(fromSquare, toSquare))
                        {
                            moveMade = true;

                            foreach (var x in chess.MoveHistory)
                            {
                                moves = x.TurnNumber.ToString();
                                matchSquare =  x.ToString();
                            }

                            allMoves = matchSquare + "\n";
                            arrList.Add(allMoves);
                            if (arrList.Count >= 12)
                            {
                                arrList.RemoveAt(0);
                            }

                            allMoves = String.Join("", arrList.ToArray());
                        }
                    }
                }
                if (!moveMade)
                    draggingPiece.ImagePos = previousPiecePos;

                draggingPiece = null;
            }

            chess.Timers[0].Update(deltaTime);
            chess.Timers[1].Update(deltaTime);

            if (chess.Timers[0].Time <= 0)
                chess.CheckStatus = CheckStatus.WhiteCheckmatedBlack;

            if (chess.Timers[1].Time <= 0)
                chess.CheckStatus = CheckStatus.BlackCheckmatedWhite;

            if (chess.CheckStatus == CheckStatus.BlackCheckmatedWhite)
            {
                checkMate = true;
            }
            if (chess.CheckStatus == CheckStatus.WhiteCheckmatedBlack)
            {
                checkMate = true;
            }

            if (checkMate)
            {
                _gameState = GameStates.EndGame;
                chess = new Chess(scale, Content, 7200.0f);
                checkMate = false;
                // Not Done
                //Atm så fungerar Restart så att föredetta gamemode sparas och startas... Brädet nollställs ej vid checkmate bool = true.
            }

            //Beroende på vilken gamemode så ska timer ändras...
            //Needs some tweaking
            /*if (_gameMode == GameModes.Normal)
            {
                _gameMode = GameModes.LoopStopper;
            }
            else if (_gameMode == GameModes.Blitz)
            {

                _gameMode = GameModes.LoopStopper;
            }
            else if (_gameMode == GameModes.Bullet)
            {

                _gameMode = GameModes.LoopStopper;
            }
            else if (_gameMode == GameModes.Rapid)
            {

                _gameMode = GameModes.LoopStopper;
            }*/

        }

        public void UpdatePause(float deltaTime)
        {
            timeDouble = stopTime;
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
            Vector2 buttonResume = new Vector2(350, 400);
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {
                if (mousePos.X >= buttonResume.X && mousePos.X <= buttonResume.X + btnPaus.Width
                && mousePos.Y >= buttonResume.Y && mousePos.Y <= buttonResume.Y + btnPaus.Height)
                {
                    _gameMode = _gameModeHolder;
                    _gameState = GameStates.Playing;
                }
            }
        }

        public void UpdateEndGame(float deltaTime)
        {

                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();
                Vector2 mousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
                Vector2 buttonMainMenu = new Vector2(400, 50);
                Vector2 buttonRestart = new Vector2(400, 150);
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {
                if (mousePos.X >= buttonMainMenu.X && mousePos.X <= buttonMainMenu.X + btnEndMainMenu.Width
                && mousePos.Y >= buttonMainMenu.Y && mousePos.Y <= buttonMainMenu.Y + btnEndMainMenu.Height)
                {
                    _gameMode = GameModes.LoopStopper;
                    _gameState = GameStates.Menu;
                }
                if (mousePos.X >= buttonRestart.X && mousePos.X <= buttonRestart.X + btnEndRestart.Width
                    && mousePos.Y >= buttonRestart.Y && mousePos.Y <= buttonRestart.Y + btnEndRestart.Height)
                {
                    _gameMode = _gameModeHolder;
                    _gameState = GameStates.Playing;
                }
            }
            
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.TransparentBlack);
            base.Draw(gameTime);
            switch (_gameState)
            {
                case GameStates.Menu:
                    DrawMainMenu(deltaTime, gameTime);
                    break;
                case GameStates.Playing:
                    DrawPlaying(deltaTime, gameTime);
                    break;
                case GameStates.GameMode:
                    DrawGameMode(deltaTime, gameTime);
                    break;
                case GameStates.Paused:
                    DrawPause(deltaTime, gameTime);
                    break;
                case GameStates.EndGame:
                    DrawEndGame(deltaTime, gameTime);
                    break;
            }
  
        }


        public void DrawMainMenu(float deltaTime, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(btnStart, new Vector2(400, 50), Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawGameMode(float deltaTime, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(btnModeNormal, new Vector2(400, 50), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(btnModeBlitz, new Vector2(400, 200), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(btnModeRapid, new Vector2(400, 350), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(btnModeBullet, new Vector2(400, 500), Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawPlaying(float deltaTime, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, null);
            spriteBatch.Draw(chess.Board.Image, chess.Board.ImagePos, null, null, null, 0, scale, Microsoft.Xna.Framework.Color.White, SpriteEffects.None, 1);
            foreach (Square square in chess.Board.Squares)
            {
                spriteBatch.Draw(square.Image, square.ImagePos, null, null, null, 0, scale, Microsoft.Xna.Framework.Color.White, SpriteEffects.None, 0.9f);
            }
            if (draggingPiece != null)
            {
                foreach (Square square in draggingPiece.PseudoLegalMoves)
                {
                    spriteBatch.Draw(square.SelectionImage, square.ImagePos, null, null, null, 0, scale, Microsoft.Xna.Framework.Color.White, SpriteEffects.None, 0.8f);
                }
            }
            foreach (Piece piece in chess.Board.Pieces)
            {
                Texture2D image = piece.Image;
                if (piece.Type == PieceType.Pawn)
                {
                    Pawn pawn = (Pawn)piece;
                    if (pawn.IsPromoted)
                        image = pawn.PromotedPiece.Image;
                }
                if (draggingPiece != null && piece == draggingPiece)
                    spriteBatch.Draw(image, piece.ImagePos, null, null, null, 0, scale, Microsoft.Xna.Framework.Color.White, SpriteEffects.None, 0.6f);
                else
                    spriteBatch.Draw(image, piece.ImagePos, null, null, null, 0, scale, Microsoft.Xna.Framework.Color.White, SpriteEffects.None, 0.7f);
            }


            //Beroende på vilken gamemode så ska timer ändras...
            //Needs some tweaking
            if (_gameModeHolder == GameModes.Normal)
            {
                spriteBatch.Draw(btnModeNormal, new Vector2(750, 25), Microsoft.Xna.Framework.Color.White);
             //   _gameMode = GameModes.LoopStopper;
            }
            else if (_gameModeHolder == GameModes.Blitz)
            {
                spriteBatch.Draw(btnModeBlitz, new Vector2(750, 25), Microsoft.Xna.Framework.Color.White);
              //  _gameMode = GameModes.LoopStopper;
            }
            else if (_gameModeHolder == GameModes.Bullet)
            {
                spriteBatch.Draw(btnModeBullet, new Vector2(750, 25), Microsoft.Xna.Framework.Color.White);
              //  _gameMode = GameModes.LoopStopper;
            }
            else if (_gameModeHolder == GameModes.Rapid)
            {
                spriteBatch.Draw(btnModeRapid, new Vector2(750, 25), Microsoft.Xna.Framework.Color.White);
             //   _gameMode = GameModes.LoopStopper;
            }
           
            spriteBatch.DrawString(player1time, "Time: " + chess.Timers[1].Text + ".", new Vector2(880, 150), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(btnPausGame, new Vector2(750, 650), Microsoft.Xna.Framework.Color.White);
            spriteBatch.DrawString(player2time, "Time: " + chess.Timers[0].Text + ".", new Vector2(880, 600), Microsoft.Xna.Framework.Color.White);

            spriteBatch.DrawString(match, "Turn: " + moves, new Vector2(880, 250), Microsoft.Xna.Framework.Color.White);

            spriteBatch.DrawString(match, "Moves:\n" + allMoves, new Vector2(880, 350), Microsoft.Xna.Framework.Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawPause(float deltaTime, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(pausedGame, "Game is paused!", new Vector2(450, 250), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(btnPaus, new Vector2(350, 400), Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawEndGame(float deltaTime, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(btnEndMainMenu, new Vector2(400, 50), Microsoft.Xna.Framework.Color.White);
            spriteBatch.Draw(btnEndRestart, new Vector2(400, 150), Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }


    }
}