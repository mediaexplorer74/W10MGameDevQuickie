using FbonizziMonoGame.Assets;
using FbonizziMonoGame.PlatformAbstractions;
using System;
using System.Diagnostics;
using System.IO;

namespace FbonizziMonoGame.Implementations
{
    /// <summary>
    /// A <see cref="CTextFileLoader"/> that loads a file using System.IO.File windows API
    /// </summary>
    public class WindowsTextFileImporter : CTextFileLoader
    {
        /// <summary>
        /// It loades the text content of a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        
        public string LoadFile(string filePath)
        {
            string res = "";

            try
            {
                res = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] TextFileLoader.LoadFile ex.: " + ex.Message);
            }

            return res;
        }
    }
}
