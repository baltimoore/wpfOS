using System.Windows;
using System.Windows.Controls;
using wpfOs.ViewModel.Apps;

namespace wpfOs.View.Apps
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        // https://stackoverflow.com/a/25001115
        private void OnNewPass1Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((SettingsViewModel)this.DataContext).NewPass1 = ((PasswordBox)sender).SecurePassword;
        }

        private void OnNewPass2Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((SettingsViewModel)this.DataContext).NewPass2 = ((PasswordBox)sender).SecurePassword;
        }
    }
}
