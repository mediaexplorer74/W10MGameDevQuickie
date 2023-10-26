using System;
using System.Diagnostics;
using System.IO;

namespace FbonizziMonoGame.Assets
{
    public class CTextFileLoader
    {
        internal string LoadFile(string spriteSheetDescriptionFilePath)
        {
            string res = "";

            try
            {
                res = File.ReadAllText(spriteSheetDescriptionFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] CTextFileLoader.LoadFile ex.: " + ex.Message);
            }

            return res;
        }
    }
}