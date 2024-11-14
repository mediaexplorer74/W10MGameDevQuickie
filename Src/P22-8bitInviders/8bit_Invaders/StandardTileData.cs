// Decompiled with JetBrains decompiler
// Type: GameManager.SaveGameManager
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using System;

namespace GameManager
{
    public class StandardTileData : ShellTileData
    {
        internal Uri BackgroundImage;

        public int? Count { get; set; }
        public string BackTitle { get; set; }
        public Uri BackBackgroundImage { get; set; }
        public string BackContent { get; set; }
    }
}