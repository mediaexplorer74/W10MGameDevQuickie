using FbonizziMonoGame.PlatformAbstractions;
using System;

namespace FbonizziMonoGame.Implementations
{
    /// <summary>
    /// Opens an URI with System.Diagnostics.Process.Start
    /// </summary>
    public class WindowsWebSiteOpener : IWebPageOpener
    {
        /// <summary>
        /// Opens an URI with System.Diagnostics.Process.Start
        /// </summary>
        /// <param name="uri"></param>
        public void OpenWebpage(Uri uri)
        { 
            //RnD; TODO
            //System.Diagnostics.Process.Start(uri.ToString());
        }
    }
}
