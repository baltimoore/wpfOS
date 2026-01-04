using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;

namespace wpfOs.ViewModel.Apps
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
         *********  User management view  *********
         ******          properties          ******
         ***********         END        ***********
         ****************          ****************
         ******************************************/



        public UserManagementViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            NavigateToSettingsCommand = new RelayCommand(_ => MainVM.NavigateToSettings());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        // System control commands, inherited from MainVM
        public RelayCommand NavigateToSettingsCommand { get; }
    }
}
