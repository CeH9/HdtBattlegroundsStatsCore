using BgMatchResultRecorder.data;
using BgMatchResultRecorder.network;
using BgMatchResultRecorder.utils;
using System.Windows.Controls;

namespace BgMatchResultRecorder
{
    public partial class SettingsControl : UserControl
    {
        internal SettingsControl()
        {
            InitializeComponent();
            ApplyState(Settings.config);
        }

        internal void ApplyState(Config cfg)
        {
            InputHostAddress.Text= cfg.websocketsServerAddress;
            InputHostAddress.CaretIndex = InputHostAddress.Text.Length;
        }

        private void DebugButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Logger.Info("==== OnDebugButtonClicked ====");

            DebugStuff.LogAppState();


            //GameUtils.GetAvailableRaces();
            //GameUtils.GetBattlegroundsRank();
            //GameUtils.GetRegion();

            //GameUtils.GetPlayerHero();
            //GameUtils.GetOpponentHero();

            //GameUtils.GetBattlegroundsPlace();
            //GameUtils.GetBattlegroundsAllPlaces();
            
            //GameUtils.GetStats();

            //GameUtils.GetPlayerInfo();
            //GameUtils.GetOpponentInfo();
            //GameUtils.GetAllHeroes();

            Logger.Info("======== OnDebug ========");
        }
    }
}