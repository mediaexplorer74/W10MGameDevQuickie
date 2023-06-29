using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FbonizziMonoGame.UI.RateMe
{
    /// <summary>
    /// A dialog to be drawn on the screen to ask for rating the App
    /// </summary>
    public class RateMeDialog
    {
        private const string _titleTextKey = "RateMe|TitleTextKey";
        private const string _messageTextKey = "RateMe|MessageTextKey";
        private const string _rateItButtonTextKey = "RateMe|RateItButtonTextKey";
        private const string _notNowButtonTextKey = "RateMe|NoThanksButtonTextKey";

        private const string _dontShowAgainSettingKey = "RateMe|DontShowAgain";
        private const string _appLaunchCountSettingKey = "RateMe|AppLaunchCount";
        private const string _rateMeLaunchCountSettingKey = "RateMe|RateMeLaunchCount";

        private readonly ISettingsRepository _settingsRepository;
        private readonly ILocalizedStringsRepository _localizedStringsRepository;

        private readonly int _launchesUntilPrompt;
        private readonly int _maxRateShowTimes;
        private readonly Uri _rateAppUri;
        private readonly Dialog _titleButtonButtonDialog;

        /// <summary>
        /// True when the main GUI should show the dialog
        /// </summary>
        public bool ShouldShowDialog { get; private set; }

        /// <summary>
        /// Constructs the dialog
        /// </summary>
        /// <param name="launchesUntilPrompt"></param>
        /// <param name="maxRateShowTimes"></param>
        /// <param name="rateAppUri"></param>
        /// <param name="dialogDefinition"></param>
        /// <param name="font"></param>
        /// <param name="localizedStringsRepository"></param>
        /// <param name="rateMeDialogStrings"></param>
        /// <param name="webPageOpener"></param>
        /// <param name="settingsRepository"></param>
        /// <param name="buttonADefinition"></param>
        /// <param name="buttonBDefinition"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="buttonsBackgroundColor"></param>
        /// <param name="buttonsShadowColor"></param>
        /// <param name="backgroundShadowColor"></param>
        /// <param name="titleColor"></param>
        /// <param name="buttonsTextColor"></param>
        /// <param name="titlePositionOffset"></param>
        /// <param name="buttonTextPadding"></param>
        /// <param name="titlePadding"></param>
        public RateMeDialog(
            int launchesUntilPrompt,
            int maxRateShowTimes,
            Uri rateAppUri,
            Rectangle dialogDefinition,
            SpriteFont font,
            ILocalizedStringsRepository localizedStringsRepository,
            RateMeDialogStrings rateMeDialogStrings,
            IWebPageOpener webPageOpener,
            ISettingsRepository settingsRepository,
            Rectangle buttonADefinition,
            Rectangle buttonBDefinition,
            Color backgroundColor,
            Color buttonsBackgroundColor,
            Color buttonsShadowColor,
            Color backgroundShadowColor,
            Color titleColor,
            Color buttonsTextColor,
            Vector2 titlePositionOffset,
            float buttonTextPadding,
            float titlePadding)
        {
            _launchesUntilPrompt = launchesUntilPrompt;
            _maxRateShowTimes = maxRateShowTimes;
            _rateAppUri = rateAppUri;
            _localizedStringsRepository = localizedStringsRepository;
            _settingsRepository = settingsRepository;

            AddStrings(rateMeDialogStrings);

            var buttonA = new ButtonWithText(
                font: font,
                text: localizedStringsRepository.Get(_rateItButtonTextKey),
                collisionRectangle: buttonADefinition,
                backgroundColor: buttonsBackgroundColor,
                textColor: buttonsTextColor,
                shadowColor: buttonsShadowColor,
                onClick: () =>
                {
                    _settingsRepository.SetBool(_dontShowAgainSettingKey, true);
                    ShouldShowDialog = false;
                    webPageOpener.OpenWebpage(_rateAppUri);
                },
                textPadding: buttonTextPadding);

            var buttonB = new ButtonWithText(
                font: font,
                text: localizedStringsRepository.Get(_notNowButtonTextKey),
                collisionRectangle: buttonBDefinition,
                backgroundColor: buttonsBackgroundColor,
                textColor: buttonsTextColor,
                shadowColor: buttonsShadowColor,
                onClick: () =>
                {
                    // If users says "Not now", I reset the app launch count
                    _settingsRepository.SetInt(_appLaunchCountSettingKey, 0);
                    ShouldShowDialog = false;
                },
                textPadding: buttonTextPadding);

            var minScale = Math.Min(buttonA.TextScale, buttonB.TextScale);
            buttonA.TextScale = minScale;
            buttonB.TextScale = minScale;

            _titleButtonButtonDialog = new Dialog(
                title: localizedStringsRepository.Get(_messageTextKey),
                font: font,
                dialogWindowDefinition: dialogDefinition,
                titlePositionOffset: titlePositionOffset,
                backgroundColor: backgroundColor,
                backgroundShadowColor: backgroundShadowColor,
                titleColor: titleColor,
                titlePadding: titlePadding,
                buttons: new ButtonWithText[] { buttonA, buttonB });

            EvaluateRateMe();
        }

        /// <summary>
        /// It handles input on the dialog buttons
        /// </summary>
        /// <param name="inputPosition"></param>
        public void HandleInput(Vector2 inputPosition)
            => _titleButtonButtonDialog.HandleInput(inputPosition);

        private void EvaluateRateMe()
        {
            //RnD
            //if (_settingsRepository.GetOrSetBool(_dontShowAgainSettingKey, false))
                return;

            int appLaunchCount = _settingsRepository.GetOrSetInt(_appLaunchCountSettingKey, 0) + 1;
            _settingsRepository.SetInt(_appLaunchCountSettingKey, appLaunchCount);

            int rateMeLaunchCount = _settingsRepository.GetOrSetInt(_rateMeLaunchCountSettingKey, 0);

            if (rateMeLaunchCount < _maxRateShowTimes)
            {
                if (appLaunchCount >= _launchesUntilPrompt)
                {
                    ShouldShowDialog = true;
                    _settingsRepository.SetInt(_rateMeLaunchCountSettingKey, rateMeLaunchCount + 1);
                }
            }
            else
            {
                // After n times that the dialog has been shown, It is never shown anymore.
                _settingsRepository.SetBool(_dontShowAgainSettingKey, true);
            }
        }

        /// <summary>
        /// Draws the dialog on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
            => _titleButtonButtonDialog.Draw(spriteBatch);

        private void AddStrings(RateMeDialogStrings rateMeDialogStrings)
        {
            _localizedStringsRepository.AddString(_titleTextKey, rateMeDialogStrings.TitleText);
            _localizedStringsRepository.AddString(_messageTextKey, rateMeDialogStrings.MessageText);
            _localizedStringsRepository.AddString(_rateItButtonTextKey, rateMeDialogStrings.RateItButtonText);
            _localizedStringsRepository.AddString(_notNowButtonTextKey, rateMeDialogStrings.NotNowButtonText);
        }
    }
}
