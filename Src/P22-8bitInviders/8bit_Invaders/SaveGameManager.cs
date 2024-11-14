// Decompiled with JetBrains decompiler
// Type: GameManager.SaveGameManager
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Mangopollo;
using Mangopollo.Tiles;
//using Microsoft.Phone.Shell;
using System;
using System.Globalization;
using System.Linq;

#nullable disable
namespace GameManager
{
  public class SaveGameManager
  {
    public SaveState saveState;
    public PlayerScores playerScores;
    public OptionsState optionsState;

    public SaveGameManager()
    {
      this.saveState = new SaveState();
      this.playerScores = new PlayerScores();
      this.optionsState = new OptionsState();
    }

    public void ResetGame(bool isnewgame)
    {
      this.saveState = new SaveState();
      this.SaveGame(isnewgame);
    }

    public void ResetScores() => this.playerScores.Reset();

    public void UpdateTile()
    {
      ShellTile shellTile = null;//ShellTile.ActiveTiles.First<ShellTile>();
      if (shellTile != null && !Utils.CanUseLiveTiles)
      {
        shellTile.Update((ShellTileData) new StandardTileData()
        {
          Count = new int?(this.saveState.Level),
          BackTitle = ("SCORE: " 
          + this.saveState.Score.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
          BackBackgroundImage = new Uri("backtile.png", UriKind.Relative),
          BackContent = "Twitter: @GlowPuff"
        });
      }
      else
      {
        if (shellTile == null)
          return;
        if (!Utils.CanUseLiveTiles)
          return;
        try
        {
          FlipTileData flipTileData = new FlipTileData()
          {
            Title = "8bit Invaders",
            BackTitle = "SCORE: " + 
            this.saveState.Score.ToString((IFormatProvider) CultureInfo.InvariantCulture),
            BackContent = "Twitter: @GlowPuff",
            WideBackContent = "Twitter: @GlowPuff",
            Count = new int?(this.saveState.Level),
            SmallBackgroundImage = new Uri("/LiveTiles/Tile159.png", UriKind.Relative),
            BackgroundImage = new Uri("/LiveTiles/Tile336.png", UriKind.Relative),
            BackBackgroundImage = new Uri("/LiveTiles/TileBack336.png", UriKind.Relative),
            WideBackgroundImage = new Uri("/LiveTiles/wideTile.png", UriKind.Relative),
            WideBackBackgroundImage = new Uri("/LiveTiles/wideTileBack.png", UriKind.Relative)
          };

          //RnD
          //shellTile.Update((ShellTileData) flipTileData);
        }
        catch
        {
        }
      }
    }

    public void LoadGame()
    {
      bool flag = false;
      if (!DataStore.Deserialize<SaveState>("save2.dat", out this.saveState))
      {
        Tools.Trace("LoadGame() - NEW SAVEGAME");
        this.saveState = new SaveState();
      }
      else
      {
        Tools.Trace("LoadGame() - LOADED SAVEGAME");
        Tools.Trace("SCORE: " + this.saveState.Score.ToString());
        Tools.Trace("LIVES: " + this.saveState.Lives.ToString());
        Tools.Trace("LEVEL: " + this.saveState.Level.ToString());
      }
      flag = false;
      if (!DataStore.Deserialize<PlayerScores>("hiscores.dat", out this.playerScores))
      {
        Tools.Trace("LoadGame() - NEW HISCORES");
        this.playerScores = new PlayerScores();
      }
      else
        Tools.Trace("LoadGame() - LOADED HISCORES");
      flag = false;
      if (!DataStore.Deserialize<OptionsState>("options.dat", out this.optionsState))
      {
        Tools.Trace("LoadGame() - NEW OPTIONS");
        this.optionsState = new OptionsState();
      }
      else
        Tools.Trace("LoadGame() - LOADED OPTIONS");
    }

    public void SaveGame() => this.SaveGame(false);

    public void SaveGame(bool isnewgame)
    {
      Tools.Trace("SaveGame() - SAVED GAME");
      this.saveState.isNewGame = isnewgame;
      DataStore.Serialize<SaveState>("save2.dat", this.saveState);
      this.UpdateTile();
    }

    public void SaveOptionsHiscores()
    {
      Tools.Trace("SaveOptionsHiscores() - SAVED GAME");
      DataStore.Serialize<PlayerScores>("hiscores.dat", this.playerScores);
      DataStore.Serialize<OptionsState>("options.dat", this.optionsState);
    }
  }
}
