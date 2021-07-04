using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using MahApps.Metro.Controls;

namespace BgMatchResultRecorder
{
    public class BgMatchResultRecorderPlugin : IPlugin
    {

        private Flyout settingsFlyout;
        private SettingsControl settingsControl;

        public void OnLoad()
        {
            Logger.Info("Plugin OnLoad");

            // Init Settings
            Settings.IsPluginEnabled = true;
            Settings.LoadConfig();

            // Setup MultiThreading' stuff
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "MainThread";
            }
            MultiThreadingUtil.dispatcherMain = Dispatcher.CurrentDispatcher;

            WebSockets.Open();

            // Setup Settings UI
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

            // Register GameEvents' Callbacks
            GameEvents.OnInMenu.Add(CoreEventsHandler.OnInMenu);
            GameEvents.OnModeChanged.Add(CoreEventsHandler.OnModeChanged);

            GameEvents.OnGameStart.Add(CoreEventsHandler.GameStart);
            GameEvents.OnGameEnd.Add(CoreEventsHandler.OnGameEnd);
            GameEvents.OnGameWon.Add(CoreEventsHandler.OnGameWon);
            GameEvents.OnGameLost.Add(CoreEventsHandler.OnGameLost);
            GameEvents.OnGameTied.Add(CoreEventsHandler.OnGameTied);

            GameEvents.OnTurnStart.Add(CoreEventsHandler.TurnStart);
        }

        public void OnUnload()
        {
            // Triggered when the user unticks the plugin
            Logger.Info("Plugin OnUnload");

            WebSockets.Close();

            Settings.IsPluginEnabled = false;
            Settings.UnloadConfig();
        }

        public void OnButtonPress()
        {
            Logger.Info("Plugin OnButtonPress");

            //Entity[] list = Helper.DeepClone<Dictionary<int, Entity>>(Hearthstone_Deck_Tracker.API.Core.Game.Entities).Values.ToArray<Entity>();

            //var card = Database.GetCardFromDbfId(x.Key, false);
            //var card = Database.GetCardFromId("TB_BaconUps_093");
            //Logger.Info($"Card Id: {card.Id} Name: {card.Name}");

            //WebSockets.ws.SendAsync("Hello Dude!", (completed) => { });

            settingsFlyout.IsOpen = true;
        }

        public void OnUpdate()
        {
            // called every ~100ms
        }

        public string Name => "Battlegrounds Stats Core";

        public string Description => "Stats provider for stream overlay, e.g. match history";

        public string ButtonText => "Settings";

        public string Author => "github.com/CeH9";

        public Version Version => new Version(1, 0, 0);

        public MenuItem MenuItem => null;
    }
}
