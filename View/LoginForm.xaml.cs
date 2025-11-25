using System.Windows;
using System.Windows.Controls;
using wpfOs.ViewModel;

namespace wpfOs.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : UserControl
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        // https://stackoverflow.com/a/25001115
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((LoginFormModel)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }
    }
}
