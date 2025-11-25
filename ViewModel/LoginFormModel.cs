using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;

namespace wpfOs.ViewModel
{
    public class LoginFormModel
    {
        // Storing Application properties from MainWindow
        public MainWindowModel MainVM { get; }

        // Authentification event
        public event EventHandler AuthenticateUserSuccess;


        /******************************************
         ***********                    ***********
         *********    Form  properties    *********
         ***********       START        ***********
         ****************         *****************
         ******************************************/

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


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
            UserAuthenticateCommand = new RelayCommand(_ => this.AuthenticateUser());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/
        public RelayCommand UserAuthenticateCommand { get; }

        private List<string> ValidateForm()
        {
            List<string> msg = new();

            if (this.Username == null ||
                this.Username.Length == 0)
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

        private void AuthenticateUser()
        {
            // first, check if all data is submitted
            List<string> validationMessage = ValidateForm();
            if (validationMessage.Count > 0)
            {
                // first newline needed, because join only adds *between* elements
                string msg = ("• " + string.Join("\n• ", validationMessage));
                MessageBox.Show(
                    messageBoxText: msg,
                    caption: "Input error",
                    icon: MessageBoxImage.Exclamation,
                    button: MessageBoxButton.OK
                );
                return;
            }

            // TODO: Do a check against a user file

            AuthenticateUserSuccess?.Invoke(null, EventArgs.Empty);
            return;
        }
    }
}
