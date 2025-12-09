using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using wpfOs.Model;

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

        private bool DisplayErrorsIfAny(List<string> errors)
        {
            if (errors.Count == 0) return false;

            // first newline needed, because join only adds *between* elements
            string msg = ("• " + string.Join("\n• ", errors));
            MessageBox.Show(
                messageBoxText: msg,
                caption: "Input error",
                icon: MessageBoxImage.Exclamation,
                button: MessageBoxButton.OK
            );
            return true;
            
        }

        private void TryAuthenticateUser()
        {
            // first, check if all data is submitted
            List<string> errorList = ValidateLoginForm();

            // If we have any errors so far, display them and fail this function
            if (this.DisplayErrorsIfAny(errorList)) return;

            // Verify user credentials against AuthService
            var answer = MainVM.AuthService.AuthenticateUser(Username, Password);
            switch (answer)
            {
                case null:
                    errorList.Add("Šāds lietotājs nav reģistrēts");
                    break;
                case false:
                    errorList.Add("Parole nav ievadīta pareizi");
                    break;
                case User:
                    // Lietotājs ir atrasts un autentificējies
                    break;

                default:
                    errorList.Add("Sistēmas kļūme!");
                    break;
            }

            // If we have any errors again, display them and fail this function
            if (this.DisplayErrorsIfAny(errorList)) return;

            // No errors? Announce the success
            AuthenticateUserSuccess?.Invoke(null, new AuthenticateUserEventArgs { AuthenticatedUser = answer });
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
