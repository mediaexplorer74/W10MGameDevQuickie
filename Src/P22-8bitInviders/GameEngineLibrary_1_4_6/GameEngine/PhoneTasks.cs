// Decompiled with JetBrains decompiler
// Type: GameEngine.PhoneTasks
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

//using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace GameEngine
{
  public static class PhoneTasks
  {
    public static void OpenThisAppInMarket()
    {
      try
      {
        //new MarketplaceDetailTask()
        //{
        //  ContentIdentifier = ((string) null)
        //}.Show();
      }
      catch
      {
      }
    }

    public static void Email(string appName_version)
    {
      try
      {
        //new EmailComposeTask()
        //{
        //  Subject = ("Support Request: " + appName_version),
        //  To = "mail@mail.com"
        //}.Show();
      }
      catch
      {
      }
    }

    public static void WebBrowser(string url)
    {
      try
      {
        //new WebBrowserTask() { Uri = new Uri(url) }.Show();
      }
      catch
      {
      }
    }

    public static void RateReviewApp()
    {
      try
      {
        //new MarketplaceReviewTask().Show();
      }
      catch
      {
      }
    }

    public static void OpenAppInMarket(string appID)
    {
      try
      {
        //new MarketplaceDetailTask() { ContentIdentifier = appID }.Show();
      }
      catch
      {
      }
    }

    public static void ShowMyGamesInMarketplace()
    {
      try
      {
        //new MarketplaceSearchTask()
        //{
        //  ContentType = ((MarketplaceContentType) 1),
        //  SearchTerms = "GlowPuff"
        //}.Show();
      }
      catch
      {
      }
    }

    public static void ShowKeyboard(string title, string description, AsyncCallback result)
    {
      PhoneTasks.ShowKeyboard(title, description, "", result);
    }

    public static void ShowKeyboard(string title, string description, Action<string> callback)
    {
      PhoneTasks.ShowKeyboard(title, description, "", (AsyncCallback) (result =>
      {
        string str1 = string.Empty;
        string str2 = string.Empty;//Guide.EndShowKeyboardInput(result);
          if (!string.IsNullOrEmpty(str2))
        {
          string upperInvariant = str2.ToUpperInvariant();
          Regex regex = new Regex("[A-Z0-9]+");
          string empty = string.Empty;
          foreach (Match match in regex.Matches(upperInvariant))
            empty += match.Value;
          str1 = empty.Substring(0, Math.Min(empty.Length, 13));
        }
        if (string.IsNullOrEmpty(str1))
          return;
        callback(str1);
      }));
    }

    public static void ShowKeyboard(
      string title,
      string description,
      string defaultText,
      AsyncCallback result)
    {
      //Guide.BeginShowKeyboardInput(PlayerIndex.One, title, description, 
      //    defaultText, new AsyncCallback(result.Invoke), (object) null);
    }
  }
}
