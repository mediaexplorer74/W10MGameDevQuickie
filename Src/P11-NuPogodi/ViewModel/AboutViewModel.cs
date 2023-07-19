using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Win8.Core.Helpers;
using Win8.Core.Services;
using Windows.UI.Xaml;

namespace GameManager.ViewModel
{
    public class AboutViewModel
    {
        public AboutResources Loc
        {
            get { return loc ?? (loc = new AboutResources()); }
        }
        private AboutResources loc;


        public string AppName
        {
            get { return infoService.AppName; }
        }

        public string AppDescription
        {
            get { return infoService.AppDescription; }
        }

        public string AppVersion
        {
            get { return infoService.AppVersion.ToString(2); }
        }

        public string AuthorName
        {
            get { return infoService.AuthorName; }
        }

        public string AuthorEmail
        {
            get { return infoService.AuthorEmail; }
        }

        public string AuthorTwitter
        {
            get { return infoService.AuthorTwitter.ToString(); }
        }

        public static Visibility IsTrial
        {
            get { return AppHelper.IsTrial ? Visibility.Visible : Visibility.Collapsed; }
        }

 
        private readonly IInfoService infoService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public AboutViewModel(IInfoService infoService)
        {
            this.infoService = infoService;

            InitializeCommands();
        }

       // Commands

        private void InitializeCommands()
        {
            // button commands
            BuyCmd = new RelayCommand(MarketplaceHelper.ShowApplicationOnMarketplace);
            RateCmd = new RelayCommand(MarketplaceHelper.ShowApplicationRating);
            MoreAppsCmd = new RelayCommand(() => MarketplaceHelper.ShowAuthorOnMarketplace(infoService.AuthorName));
            FeedbackCmd = new RelayCommand(() => EmailHelper.Send(AuthorEmail,
                AppName, AboutResources.FeedbackMessage));
            ShowTwitterCmd = new RelayCommand(() => WebHelper.NavigateTo(infoService.AuthorTwitter));
            ShareCmd = new RelayCommand(() => ShareHelper.ShareLink(infoService.AppMarketplaceLink, infoService.AppName, 
                AboutResources.ShareMessage));
        }

        // button commands
        public ICommand BuyCmd { get; private set; }
        public ICommand RateCmd { get; private set; }
        public ICommand ShareCmd { get; private set; }
        public ICommand MoreAppsCmd { get; private set; }
        public ICommand ShowTwitterCmd { get; private set; }
        public ICommand FeedbackCmd { get; private set; }

    }
}