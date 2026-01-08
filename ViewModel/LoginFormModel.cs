using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using wpfOs.Model;
using wpfOs;

namespace wpfOs.ViewModel
{
    public class LoginFormModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /******************************************
         ***********                    ***********
         *********    Form  properties    *********
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        // Saving MainWindow ViewModel for app-wide data
        public MainWindowModel MainVM { get; }

        // Authentification event
        public event EventHandler AuthenticateUserSuccess;

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value; OnPropertyChanged(nameof(Username));
            }
        }

        private SecureString _password;
        public SecureString Password
        {
            get { return _password; }
            set { _password = value;}
        }


        /******************************************
         ***********                    ***********
         *********    Form  properties    *********
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        public LoginFormModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            UserAuthenticateCommand = new RelayCommand(_ => this.TryAuthenticateUser());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/
        public RelayCommand UserAuthenticateCommand { get; }

        private List<string> ValidateLoginForm()
        {
            List<string> msg = new();

            if (string.IsNullOrEmpty(Username))
            {
                msg.Add("Ievadiet lietotājvārdu!");
            }

            if (this.Password == null ||
                this.Password.Length == 0)
            {
                msg.Add("Ievadiet paroli!");
            }

            return msg;
        }

        private void TryAuthenticateUser()
        {
            // first, check if all data is submitted
            List<string> errorList = ValidateLoginForm();

            // If we have any errors so far, display them and fail this function
            if (MessageBoxHelper.ShowError(errorList)) return;

            // Verify user credentials against AuthService
            User authUser;
            try
            {
                authUser = MainVM.AuthService.AuthenticateUser(Username, Password);
            }
            catch (ArgumentException ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
                return;
            }

            // If we have any errors again, display them and fail this function

            // No errors? Announce the success
            AuthenticateUserSuccess?.Invoke(
                null,
                new AuthenticateUserEventArgs { AuthenticatedUser = authUser }
            );
            return;
        }
    }
}



/// <summary>
/// A custom EventArg class, that stores the authenticated user's username as a custom property
/// </summary>
internal class AuthenticateUserEventArgs : EventArgs
{
    public User AuthenticatedUser { get; set; }
}
