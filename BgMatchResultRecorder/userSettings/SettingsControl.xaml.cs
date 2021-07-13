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
            InputHostAddress.Text = cfg.websocketsServerAddress;
            InputHostAddress.CaretIndex = InputHostAddress.Text.Length;

            DebugButton.Content = "Toggle Logging Opponent";
            DebugButton2.Content = "Log AppState";
            DebugButton3.Content = "Toggle Logging Races";
        }

        private void DebugButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Logger.Info("==== OnDebugButtonClicked ====");

            DebugStuff.ToggleLoggingOpponent();
        }

        private void DebugButton_Click2(object sender, System.Windows.RoutedEventArgs e)
        {
            Logger.Info("==== OnDebugButton2 Clicked ====");

            DebugStuff.LogAppState();

            Logger.Info("======== OnDebug ========");
        }    

        private void DebugButton_Click3(object sender, System.Windows.RoutedEventArgs e)
        {
            //Logger.Info("==== OnDebugButton3 Clicked ====");

            DebugStuff.ToggleLoggingRaces();
        }
    }
}