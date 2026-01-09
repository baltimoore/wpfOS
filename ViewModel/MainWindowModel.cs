using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using wpfOs.Model;
using wpfOs.Service;

namespace wpfOs.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /******************************************
         *******                            *******
         *****  Global Application variables  *****
         *****             START              *****
         ****************         *****************
         ******************************************/

        public AuthService AuthService;
        public DomainService DomainService;

        private User _currentUser;
        public User CurrentUser {
            get { return _currentUser; }
            set { SetProperty(ref _currentUser, value); }
        }

        private Visibility _menuVisibility = Visibility.Collapsed;
        public Visibility GlobalOsMenuVisibility
        {
            get { return _menuVisibility; }
            set { SetProperty(ref _menuVisibility, value); }
        }

        private object _currentViewModel;
        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        /******************************************
         *******                            *******
         *****  Global Application variables  *****
         *****              END               *****
         *****************       ******************
         ******************************************/



        /******************************************
         *******                            *******
         *****  Application view definitions  *****
         *****             START              *****
         ****************         *****************
         ******************************************/

        // Splash screen definitions
        public SplashScreenModel SplashVM { get; }
        public RelayCommand SetSplashScreenViewModel { get; }
        public void NavigateToSplashScreen()
        {
            CurrentViewModel = SplashVM;
        }

        // Login form definitions
        public LoginFormModel LoginVM { get; }
        public RelayCommand SetLoginFormViewModel { get; }
        public void NavigateToLoginForm()
        {
            CurrentViewModel = LoginVM;
        }

        // Dashboard definitions
        public DashboardViewModel DashboardVM { get; }
        public RelayCommand SetDashboardViewModel { get; }
        public void NavigateToDashboard()
        {
            CurrentViewModel = DashboardVM;
        }

        // Settings definition
        public Apps.Settings.GeneralSettingsViewModel SettingsVM { get; }
        public RelayCommand SetSettingsViewModel { get; }
        public void NavigateToSettings()
        {
            CurrentViewModel = SettingsVM;
        }

        // User Management definitions
        public Apps.Settings.UserManagementViewModel UserManagementVM { get; }
        public RelayCommand SetUserManagementViewModel { get; }
        public void NavigateToUserManagement()
        {
            CurrentViewModel = UserManagementVM;
        }

        // Domain Management definitions
        public Apps.Domain.DomainManagementViewModel DomainManagementVM { get; }
        public RelayCommand SetDomainManagementViewModel { get; }
        public void NavigateToDomainManagement()
        {
            CurrentViewModel = DomainManagementVM;
        }



        // File text editor view
        public Apps.TextEditorViewModel TextEditorVM;
        public RelayCommand SetTextEditorViewModel { get; }
        public void NavigateToTextEditor()
        {
            CurrentViewModel = TextEditorVM;
        }

        // File Manager definitions
        public Apps.FileManagerViewModel FileManagerVM { get; }
        public RelayCommand SetFileManagerViewModel { get; }
        public void NavigateToFileManager()
        {
            CurrentViewModel = FileManagerVM;
        }

        // Database Management definitions
        public Apps.DatabaseManagementViewModel DatabaseManagementVM { get; }
        public RelayCommand SetDatabaseManagementViewModel { get; }
        public void NavigateToDatabaseManagement()
        {
            CurrentViewModel = DatabaseManagementVM;
        }

        // Email Management definitions
        public Apps.EmailManagementViewModel EmailManagementVM { get; }
        public RelayCommand SetEmailManagementViewModel { get; }
        public void NavigateToEmailManagement()
        {
            CurrentViewModel = EmailManagementVM;
        }

        // Statistics definitions
        public Apps.StatisticsViewModel StatisticsVM { get; }
        public RelayCommand SetStatisticsViewModel { get; }
        public void NavigateToStatistics()
        {
            CurrentViewModel = StatisticsVM;
        }

        // Backup Management definitions
        public Apps.BackupManagementViewModel BackupManagementVM { get; }
        public RelayCommand SetBackupManagementViewModel { get; }
        public void NavigateToBackupManagement()
        {
            CurrentViewModel = BackupManagementVM;
        }

        /******************************************
         *******                            *******
         *****  Application view definitions  *****
         *****              END               *****
         *****************       ******************
         ******************************************/



        /******************************************
         *******                            *******
         *****     System control methods     *****
         *****             START              *****
         ****************         *****************
         ******************************************/

        public RelayCommand LogoutCommand { get; }
        public void Logout()
        {
            // Clear current user and return to login
            GlobalOsMenuVisibility = Visibility.Collapsed;
            CurrentUser = null;
            NavigateToLoginForm();
        }

        public RelayCommand PoweroffCommand { get; }
        public void Poweroff()
        {
            // Close the application
            Application.Current.Shutdown();
        }

        /******************************************
         *******                            *******
         *****  System control methods      *****
         *****              END               *****
         *****************       ******************
         ******************************************/



        public MainWindowModel()
        {
            // View registration
            SplashVM = new SplashScreenModel();
            SetSplashScreenViewModel = new RelayCommand(_ => NavigateToSplashScreen());

            LoginVM = new LoginFormModel(this);
            SetLoginFormViewModel = new RelayCommand(_ => NavigateToLoginForm());

            DashboardVM = new DashboardViewModel(this);
            SetDashboardViewModel = new RelayCommand(_ => NavigateToDashboard());

            SettingsVM = new Apps.Settings.GeneralSettingsViewModel(this);
            SetSettingsViewModel = new RelayCommand(_ => NavigateToSettings());

            UserManagementVM = new Apps.Settings.UserManagementViewModel(this);
            SetUserManagementViewModel = new RelayCommand(_ => NavigateToUserManagement());

            DomainManagementVM = new Apps.Domain.DomainManagementViewModel(this);
            SetDomainManagementViewModel = new RelayCommand(_ => NavigateToDomainManagement());



            TextEditorVM = new Apps.TextEditorViewModel();
            SetTextEditorViewModel = new RelayCommand(_ => NavigateToTextEditor());

            FileManagerVM = new Apps.FileManagerViewModel();
            SetFileManagerViewModel = new RelayCommand(_ => NavigateToFileManager());

            DatabaseManagementVM = new Apps.DatabaseManagementViewModel();
            SetDatabaseManagementViewModel = new RelayCommand(_ => NavigateToDatabaseManagement());

            EmailManagementVM = new Apps.EmailManagementViewModel();
            SetEmailManagementViewModel = new RelayCommand(_ => NavigateToEmailManagement());

            StatisticsVM = new Apps.StatisticsViewModel();
            SetStatisticsViewModel = new RelayCommand(_ => NavigateToStatistics());

            BackupManagementVM = new Apps.BackupManagementViewModel();
            SetBackupManagementViewModel = new RelayCommand(_ => NavigateToBackupManagement());

            // System control commands
            LogoutCommand = new RelayCommand(_ => Logout());
            PoweroffCommand = new RelayCommand(_ => Poweroff());

            // Event registration
            LoginVM.AuthenticateUserSuccess += LoginVM_AuthenticateUserSuccess;

            // Service registration
            AuthService = new();
            DomainService = new();

            // Initialize the app startup
            this.BootupSequence();
        }

        private void BootupSequence()
        {
            NavigateToSplashScreen();
            //wait for 5 seconds
            Task.Delay(5000).ContinueWith(_ => NavigateToLoginForm() );
        }

        private void LoginVM_AuthenticateUserSuccess(object? sender, EventArgs e)
        {
            // Cast the EventArgs to AuthenticateUserEventArgs to access the AuthenticatedUser
            if (e is AuthenticateUserEventArgs args)
            {
                GlobalOsMenuVisibility = Visibility.Visible;
                CurrentUser = args.AuthenticatedUser; // Access AuthenticatedUser from the casted EventArgs
                NavigateToDashboard();
            }
            else
            {
                MessageBoxHelper.ShowSystemError();
            }
        }
    }
}
