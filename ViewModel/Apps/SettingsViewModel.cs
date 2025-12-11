using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

namespace wpfOs.ViewModel.Apps
{
    public class SettingsViewModel
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

        public static ObservableCollection<String> UserPfpArray { get; } = new()
        {
            @"C:\Path\To\Image1.jpg",
            @"C:\Path\To\Image2.jpg",
            @"C:\Path\To\Image3.jpg"
        };

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

        /******************************************
        ***********                    ***********
        ******   Settings view properties   ******
        ***********         END        ***********
        ****************         *****************
        ******************************************/



        public SettingsViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            SetNewUsername = new RelayCommand(_ => this.ChangeUserUsername());
            SetNewPassword = new RelayCommand(_ => this.ChangeUserPassword());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        public RelayCommand SetNewUsername { get; }
        public void ChangeUserUsername()
        {
            List<string> errorList = new();
            try
            {
                if (string.IsNullOrWhiteSpace(NewUsername))
                    throw new ArgumentException("Ievadiet lietotājvārdu!");

                MainVM.AuthService.ChangeUsername(
                    this.MainVM.CurrentUser,
                    NewUsername.Trim()
                );
            }
            catch (ArgumentException ex)
            {
                errorList.Add( ex.Message );
                DisplayErrorsIfAny(errorList);
                return;
            }

            // If we're here, username was changed successfully
            DisplaySuccess("Lietotājvārds veiksmīgi nomainīts!");
        }

        public RelayCommand SetNewPassword { get; }
        public void ChangeUserPassword()
        {

            List<string> errorList = new();
            try
            {
                // Check if passwords are identical
                if (!PasswordsAreEqual(NewPass1, NewPass2))
                    throw new ArgumentException("Paroles nav vienādas!");

                // since they're equal here, doesn't matter what we pass
                MainVM.AuthService.ChangePassword(
                    this.MainVM.CurrentUser,
                    NewPass1
                );
            }
            catch (ArgumentException ex)
            {
                errorList.Add(ex.Message);
                DisplayErrorsIfAny(errorList);
                return;
            }

            // If we're here, username was changed successfully
            DisplaySuccess("Parole veiksmīgi nomainīta!");
        }

        // https://stackoverflow.com/a/23183092
        private bool PasswordsAreEqual(SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

        private static void DisplayErrorsIfAny(List<string> errors)
        {
            // If no errors exist, don't show error box
            if (errors.Count == 0)
                return;

            // first newline needed, because join only adds *between* elements
            string msg = ("• " + string.Join("\n• ", errors));
            MessageBox.Show(
                messageBoxText: msg,
                caption: "Input error",
                icon: MessageBoxImage.Exclamation,
                button: MessageBoxButton.OK
            );
            return;
        }

        private static void DisplaySuccess(string message)
        {
            // first newline needed, because join only adds *between* elements
            MessageBox.Show(
                messageBoxText: message,
                caption: "Success!",
                icon: MessageBoxImage.Information,
                button: MessageBoxButton.OK
            );
            return;
        }
    }
}
