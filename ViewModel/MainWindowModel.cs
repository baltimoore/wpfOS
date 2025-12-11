using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using wpfOs.Model;
using wpfOs.Service;
using wpfOs.ViewModel.Apps;

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

        // Desktop definitions
        public DesktopViewModel DesktopVM { get; }
        public RelayCommand SetDesktopViewModel { get; }
        public void NavigateToDesktop()
        {
            CurrentViewModel = DesktopVM;
        }

        // Settings definition
        public SettingsViewModel SettingsVM { get; }
        public RelayCommand SetSettingsViewModel { get; }
        public void NavigateToSettings()
        {
            CurrentViewModel = SettingsVM;
        }
        
        // File text editor view
        public TextEditorViewModel TextEditorVM;
        public RelayCommand SetTextEditorViewModel { get; }
        public void NavigateToTextEditor()
        {
            CurrentViewModel = TextEditorVM;
        }

        // Browser app definitions
        public BrowserViewModel WebBrowserVM;
        public RelayCommand SetWebBrowserViewModel { get; }
        public void NavigateToWebBrowser()
        {
            CurrentViewModel = WebBrowserVM;
        }

        /******************************************
         *******                            *******
         *****  Application view definitions  *****
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

            DesktopVM = new DesktopViewModel(this);
            SetDesktopViewModel = new RelayCommand(_ => NavigateToDesktop());

            SettingsVM = new SettingsViewModel(this);
            SetSettingsViewModel = new RelayCommand(_ => NavigateToSettings());

            TextEditorVM = new TextEditorViewModel();
            SetTextEditorViewModel = new RelayCommand(_ => NavigateToTextEditor());

            WebBrowserVM = new BrowserViewModel();
            SetWebBrowserViewModel = new RelayCommand(_ => NavigateToWebBrowser());

            // Event registration
            LoginVM.AuthenticateUserSuccess += LoginVM_AuthenticateUserSuccess;

            // Service registration
            AuthService = new();

            CreateDefaultUser();

            // Initialize the app startup
            //this.BootupSequence();
            LoginVM_AuthenticateUserSuccess(null, EventArgs.Empty);
        }

        private void BootupSequence()
        {
            NavigateToSplashScreen();
            //wait for 5 seconds
            Task.Delay(5000).ContinueWith(_ => NavigateToLoginForm() );
        }

        private void CreateDefaultUser()
        {
            try
            {
                SecureString pass = new();
                pass.AppendChar('A');
                AuthService.CreateUser("a", pass);
            }
            catch (ArgumentException ex)
            {
                // user already exists; do nothin
            }
        }

        private void LoginVM_AuthenticateUserSuccess(object? sender, EventArgs e)
        {
            GlobalOsMenuVisibility = Visibility.Visible;
            NavigateToDesktop();
        }
    }
}
