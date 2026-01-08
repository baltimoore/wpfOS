using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using wpfOs.Model;

namespace wpfOs.ViewModel.Apps.Settings
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /******************************************
         *********  User management view  *********
         ******          properties          ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/
        private MainWindowModel MainVM { get; }

        private string _username;
        public string SelectedUsername
        {
            get { return _username; }
            set
            {
                _username = value; OnPropertyChanged(nameof(SelectedUsername));
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

        public Array UserRoles
        {
            get { return Enum.GetValues(typeof(UserRole)); }
        }

        private UserRole? _role = null;
        public UserRole? SelectedUserRole
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged(nameof(SelectedUserRole));
            }
        }

        public List<User> RegisteredUsers
        {
            get { return MainVM.AuthService.GetAllUsers(MainVM.CurrentUser); }
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;

                AutofillFormFields();

                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        /******************************************
         *********  User management view  *********|
         ******          properties          ******
         ***********         END        ***********
         ****************          ****************
         ******************************************/



        public UserManagementViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            NavigateToSettingsCommand = new RelayCommand(_ => MainVM.NavigateToSettings());
            ClearSelectedUserCommand = new RelayCommand(_ => ClearSelectedUser());
            CreateOrUpdateCommand = new RelayCommand(_ => CreateOrUpdateUser());
            DeleteCommand = new RelayCommand(_ => TryDeleteUser());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        // System control commands, inherited from MainVM
        public RelayCommand NavigateToSettingsCommand { get; }

        // Actual ViewModel logic methods
        public RelayCommand ClearSelectedUserCommand { get; }
        public void ClearSelectedUser()
        {
            SelectedUser = null;
        }

        public RelayCommand CreateOrUpdateCommand { get; }
        public void CreateOrUpdateUser()
        {
            // cred check
            if (! MainVM.AuthService.AuthorizeUser(MainVM.CurrentUser, UserRole.ADMIN))
                throw new ArgumentException("Jums nav tiesību veikt šo darbību.");

            // check if user is selected
            if (SelectedUser == null)
            {
                // doesn't exist, registering a new one
                TryRegisterUser();
            }

            // a user is selected
            // editing their user details
            else
            {
                TryEditUser();
            }
        }

        public RelayCommand DeleteCommand { get; }
        public void TryDeleteUser()
        {
            try
            {
                if (SelectedUser == null)
                    throw new ArgumentException("Atlasiet lietotāju ko dzēst!");

                MainVM.AuthService.DeleteUser(MainVM.CurrentUser, SelectedUser);

                // notify UI and user that userlist has changed
                SelectedUser = null;
                MessageBoxHelper.ShowSuccess("Lietotājs dzēsts veiksmīgi!");
                OnPropertyChanged(nameof(RegisteredUsers));
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
            }
        }

        private void TryRegisterUser()
        {
            try
            {
                if (NewPass1 == null || NewPass1.Length == 0 ||
                    NewPass2 == null || NewPass2.Length == 0)
                    throw new ArgumentException("Lai veidotu jaunu lietotāju, jāievada parole!");

                if (!Service.AuthService.ArePasswordsEqual(NewPass1, NewPass2))
                    throw new ArgumentException("Paroles nav vienādas!");

                // CreateUser throws errors for duplicate usernames
                if (SelectedUserRole != null)
                {
                    MainVM.AuthService.CreateUser(MainVM.CurrentUser,
                        SelectedUsername,
                        NewPass1,
                        (UserRole) SelectedUserRole
                    );
                }
                else
                {
                    MainVM.AuthService.CreateUser(MainVM.CurrentUser,
                        SelectedUsername,
                        NewPass1
                    );
                }

                // notify UI and user that userlist has changed
                MessageBoxHelper.ShowSuccess("Lietotājs izveidots veiksmīgi!");
                OnPropertyChanged(nameof(RegisteredUsers));
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
            }
        }

        private void TryEditUser()
        {
            try
            {
                // change username only if the field is not empty
                // ChangeUsername throws errors for duplicate usernames
                if (SelectedUsername != null &&
                    SelectedUsername.Length != 0 &&
                    SelectedUsername != SelectedUser.Username)
                    MainVM.AuthService.ChangeUsername(SelectedUser, SelectedUsername);

                // If password fields are not filled, assume they're not being changed
                if (NewPass1 != null && NewPass1.Length != 0 &&
                    NewPass2 != null && NewPass2.Length != 0)
                    if (Service.AuthService.ArePasswordsEqual(NewPass1, NewPass2))
                        MainVM.AuthService.ChangePassword(SelectedUser, NewPass1);

                // Only add the selected role, if user does not have it
                if (SelectedUser != null &&
                  ! SelectedUser.HasRole((UserRole) SelectedUserRole))
                    MainVM.AuthService.ChangeUserRole(MainVM.CurrentUser, SelectedUser, (UserRole) SelectedUserRole);

                // notify UI and user that userlist has changed
                MessageBoxHelper.ShowSuccess("Lietotāja dati mainīti veiksmīgi!");
                OnPropertyChanged(nameof(RegisteredUsers));
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
            }
        }

        private void AutofillFormFields()
        {
            if (SelectedUser == null)
                return;

            SelectedUsername = SelectedUser.Username;
            SelectedUserRole = SelectedUser.Roles[0];

            // no clue how to clear the passwords, so they aren't accidentally cloned
            // tried the following, and the passwordboxes still had text in them
            //NewPass1?.Clear();
            //NewPass2?.Clear();
            //NewPass1 = new();
            //NewPass2 = new();
        }
    }
}
