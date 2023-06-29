using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Snake
{
    public sealed partial class MainPage : Page
    {
        Engine engine;

        public MainPage()
        {
            this.InitializeComponent();
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = true;
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            StartGame();
        }

        private void StartGame()
        {
            engine = MonoGame.Framework.XamlGame<Engine>.Create(
                string.Empty, Window.Current.CoreWindow, monogameRenderTarget);
        }
    }
}
