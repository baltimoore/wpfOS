namespace wpfOs.ViewModel.Apps
{
    public class SettingsViewModel
    {
        // Saving MainWindow ViewModel for app-wide data
        private MainWindowModel MainVM { get; }

        public SettingsViewModel(MainWindowModel main)
        {
            this.MainVM = main;
        }
    }
}
