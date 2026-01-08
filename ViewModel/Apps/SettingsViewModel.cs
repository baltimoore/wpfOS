using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using wpfOs.Model;
using wpfOs;

namespace wpfOs.ViewModel.Apps
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /******************************************
         ***********                    ***********
         ******   Settings view properties   ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private MainWindowModel MainVM { get; }

        public string CurrentUsername
        {
            get { return MainVM.CurrentUser.Username ?? ""; }
        }

        public string CurrentUserRoles
        {
            get
            {
                // should never trigger, but again, just in case
                if (MainVM.CurrentUser.Roles == null || MainVM.CurrentUser.Roles.Count == 0)
                    return "Nav lomu";
                
                return string.Join(", ", MainVM.CurrentUser.Roles.Select(r => r.ToString()));
            }
        }

        private string _newUsername;
        public string NewUsername
        {
            get { return _newUsername; }
            set
            {
                _newUsername = value; OnPropertyChanged(nameof(NewUsername));
            }
        }

        private SecureString _pass1;
        public SecureString NewPass1
        {
            get { return _pass1; }
            set { _pass1 = value; }
        }

        private SecureString _pass2;
        public SecureString NewPass2
        {
            get { return _pass2; }
            set { _pass2 = value; }
        }

        public Visibility UserManagementButtonVisibility
        {
            get
            {
                // bypass check
                User? currentUser = MainVM?.CurrentUser;
                if (currentUser == null)
                    return Visibility.Collapsed;
                
                // display button only to admins
                return MainVM.AuthService.AuthorizeUser(currentUser, UserRole.ADMIN)
                    ? Visibility.Visible 
                    : Visibility.Collapsed;
            }
        }

        /******************************************
        ***********                    ***********
        ******   Settings view properties   ******
        ***********         END        ***********
        ****************         *****************
        ******************************************/



        public SettingsViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Initialize new username with current username
            // current user should already be set, but just in case
            if (MainVM?.CurrentUser != null)
            {
                _newUsername = MainVM.CurrentUser.Username;
            }

            // Relay commands
            SetNewUsername   = new RelayCommand(_ => this.ChangeUserUsername());
            SetNewPassword   = new RelayCommand(_ => this.ChangeUserPassword());
            NavigateToUserManagementCommand = new RelayCommand(_ => MainVM.NavigateToUserManagement());
            
            // System control commands (exposed from MainVM)
            LogoutCommand = new RelayCommand(_ => MainVM.Logout());
            PoweroffCommand = new RelayCommand(_ => MainVM.Poweroff());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        // System control commands, inherited from MainVM
        public RelayCommand LogoutCommand { get; }
        public RelayCommand PoweroffCommand { get; }
        public RelayCommand NavigateToUserManagementCommand { get; }

        public RelayCommand SetNewUsername { get; }
        public void ChangeUserUsername()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewUsername))
                    throw new ArgumentException("Ievadiet lietotājvārdu!");

                // function throws error if username is already taken
                MainVM.AuthService.ChangeUsername(
                    this.MainVM.CurrentUser,
                    NewUsername.Trim()
                );
            }
            catch (ArgumentException ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
                return;
            }

            // If we're here, username was changed successfully
            // notify UI and user that username has changed
            OnPropertyChanged(nameof(CurrentUsername));
            MessageBoxHelper.ShowSuccess("Lietotājvārds veiksmīgi nomainīts!");
        }

        public RelayCommand SetNewPassword { get; }
        public void ChangeUserPassword()
        {
            List<string> errorList = new();
            try
            {
                // Check if passwords are set
                if (NewPass1 == null || NewPass1.Length == 0 ||
                    NewPass2 == null || NewPass2.Length == 0)
                    throw new ArgumentException("Lai mainītu paroli, jāaizpilda abi lauki!");

                // Check if passwords are identical
                if (Service.AuthService.PasswordsAreEqual(NewPass1, NewPass2))
                    throw new ArgumentException("Paroles nav vienādas!");

                // since they're equal here, doesn't matter what we pass
                MainVM.AuthService.ChangePassword(
                    this.MainVM.CurrentUser,
                    NewPass1
                );
            }
            catch (ArgumentException ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
                return;
            }

            // If we're here, username was changed successfully
            MessageBoxHelper.ShowSuccess("Parole veiksmīgi nomainīta!");
        }
    }
}
