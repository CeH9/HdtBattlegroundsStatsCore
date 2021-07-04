using System.Windows.Controls;

namespace BgMatchResultRecorder
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            ApplyState(Settings.config);
        }

        public void ApplyState(Config cfg)
        {
            InputHostAddress.Text= cfg.websocketsServerAddress;
            InputHostAddress.CaretIndex = InputHostAddress.Text.Length;
        }

        private void DebugButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CoreEventsHandler.OnDebugButtonClicked();
        }
    }
}