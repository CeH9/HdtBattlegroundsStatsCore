using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using BgMatchResultRecorder.data;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using MahApps.Metro.Controls;

namespace BgMatchResultRecorder
{
    public class BgMatchResultRecorderPlugin : IPlugin
    {

        private Flyout settingsFlyout;
        private SettingsControl settingsControl;

        public string Name => "Battlegrounds Stats Core";
        public string Description => "Stats provider for stream overlay, e.g. match history";
        public string ButtonText => "Settings";
        public string Author => "github.com/CeH9";
        public Version Version => new Version(1, 0, 0);
        public MenuItem MenuItem => null;

        public void OnLoad()
        {
            Logger.Info("Plugin OnLoad");

            // Init Settings
            Settings.IsPluginEnabled = true;
            Settings.LoadConfig();

            AppState.matchState = AppState.DefaultMatchState();

            InitMultiThreadingStuff();
            WebSockets.Open();
            InitSettingsUi();
            InitGameEvents();
        }

        public void OnUnload()
        {
            // Triggered when the user unticks the plugin
            Logger.Info("Plugin OnUnload");

            WebSockets.Close();
            GameEventsHandler.resetState();
            AppState.matchState = null;

            Settings.IsPluginEnabled = false;
            Settings.UnloadConfig();
        }

        public void OnButtonPress()
        {
            Logger.Info("Plugin OnButtonPress");

            settingsFlyout.IsOpen = true;
        }

        public void OnUpdate()
        {
            // called every ~100ms
        }

        private void InitMultiThreadingStuff()
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "MainThread";
            }
            MultiThreadingUtil.dispatcherMain = Dispatcher.CurrentDispatcher;
        }

        private void InitGameEvents()
        {
            GameEvents.OnInMenu.Add(GameEventsHandler.OnInMenu);
            GameEvents.OnModeChanged.Add(GameEventsHandler.OnModeChanged);

            GameEvents.OnGameStart.Add(GameEventsHandler.GameStart);
            GameEvents.OnGameEnd.Add(GameEventsHandler.OnGameEnd);
            GameEvents.OnGameWon.Add(GameEventsHandler.OnGameWon);
            GameEvents.OnGameLost.Add(GameEventsHandler.OnGameLost);
            GameEvents.OnGameTied.Add(GameEventsHandler.OnGameTied);
            GameEvents.OnOpponentCreateInPlay.Add(GameEventsHandler.OnOpponentCreateInPlay);

            GameEvents.OnTurnStart.Add(GameEventsHandler.TurnStart);
        }

        private void InitSettingsUi()
        {
            settingsFlyout = new Flyout();
            settingsFlyout.Name = "BgSettingsFlyout";
            settingsFlyout.Position = Position.Left;
            Panel.SetZIndex(settingsFlyout, 100);
            settingsFlyout.Header = "Battlegrounds Match Data Settings";
            settingsControl = new SettingsControl();
            settingsFlyout.Content = settingsControl;
            settingsFlyout.ClosingFinished += (sender, args) =>
            {
                Settings.config.websocketsServerAddress = settingsControl.InputHostAddress.Text;
                Settings.config.save();
            };
            Core.MainWindow.Flyouts.Items.Add(settingsFlyout);
        }
    }
}
