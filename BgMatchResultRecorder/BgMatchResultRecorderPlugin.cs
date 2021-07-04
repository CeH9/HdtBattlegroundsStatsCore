using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;

namespace BgMatchResultRecorder
{
    public class BgMatchResultRecorderPlugin : IPlugin
    {
        public void OnLoad()
        {
            Logger.Info("BgMatchResultRecorderPlugin OnLoad");

            Settings.IsPluginEnabled = true;

            // TODO Init websockets
            Thread.CurrentThread.Name = "MainThread";
            MultiThreadingUtil.dispatcherMain = Dispatcher.CurrentDispatcher;
            WebSockets.Open();

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
            Logger.Info("BgMatchResultRecorderPlugin OnUnload");

            Settings.IsPluginEnabled = true;

            WebSockets.Close();
        }

        public void OnButtonPress()
        {
            Logger.Info("OnButtonPress");

            //Entity[] list = Helper.DeepClone<Dictionary<int, Entity>>(Hearthstone_Deck_Tracker.API.Core.Game.Entities).Values.ToArray<Entity>();

            //var card = Database.GetCardFromDbfId(x.Key, false);
            //var card = Database.GetCardFromId("TB_BaconUps_093");
            //Logger.Info($"Card Id: {card.Id} Name: {card.Name}");

            WebSockets.ws.SendAsync("Hello Dude!", ((completed) => {}));
        }

        public void OnUpdate()
        {
            // called every ~100ms
        }

        public string Name => "Battlegrounds Stats Core";

        public string Description => "Stats provider for stream overlay, e.g. match history";

        public string ButtonText => "Test Button";

        public string Author => "github.com/CeH9";

        public Version Version => new Version(0, 0, 1);

        public MenuItem MenuItem => null;
    }
}
