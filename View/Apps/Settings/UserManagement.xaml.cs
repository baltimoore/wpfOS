using System.Windows;
using System.Windows.Controls;
using wpfOs.ViewModel.Apps.Settings;

namespace wpfOs.View.Apps.Settings
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : UserControl
    {
        public UserManagement()
        {
            InitializeComponent();
        }

        // https://stackoverflow.com/a/25001115
        private void OnNewPass1Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((UserManagementViewModel)this.DataContext).NewPass1 = ((PasswordBox)sender).SecurePassword;
        }

        private void OnNewPass2Changed(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((UserManagementViewModel)this.DataContext).NewPass2 = ((PasswordBox)sender).SecurePassword;
        }
    }
}
