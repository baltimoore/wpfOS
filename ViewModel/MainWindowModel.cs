using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get { return this._currentViewModel; }
            set { SetProperty(ref this._currentViewModel, value); }
        }

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

        // Put global variables here

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
            CurrentViewModel = this.SplashVM;
        }

        // Login form definitions
        public LoginFormModel LoginVM { get; }
        public RelayCommand SetLoginFormViewModel { get; }
        public void NavigateToLoginForm()
        {
            CurrentViewModel = this.LoginVM;
        }

        // Desktop definitions
        public DesktopViewModel DesktopVM { get; }
        public RelayCommand SetDesktopViewModel { get; }
        public void NavigateToDesktop()
        {
            CurrentViewModel = DesktopVM;
        }
        
        // File text editor view
        public Apps.TextEditorViewModel TextEditorVM;
        public RelayCommand SetTextEditorViewModel { get; }
        public void NavigateToTextEditor()
        {
            CurrentViewModel = this.TextEditorVM;
        }

        // Browser app definitions
        public Apps.BrowserViewModel WebBrowserVM;
        public RelayCommand SetWebBrowserViewModel { get; }
        public void NavigateToWebBrowser()
        {
            CurrentViewModel = this.WebBrowserVM;
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
            SetSplashScreenViewModel = new RelayCommand(_ => this.NavigateToSplashScreen());

            LoginVM = new LoginFormModel();
            SetLoginFormViewModel = new RelayCommand(_ => this.NavigateToLoginForm());

            DesktopVM = new DesktopViewModel(this);
            SetDesktopViewModel = new RelayCommand(_ => this.NavigateToDesktop());

            TextEditorVM = new Apps.TextEditorViewModel();
            SetTextEditorViewModel = new RelayCommand(_ => this.NavigateToTextEditor());

            WebBrowserVM = new Apps.BrowserViewModel();
            SetWebBrowserViewModel = new RelayCommand(_ => this.NavigateToWebBrowser());

            // Event registration
            LoginVM.AuthenticateUserSuccess += LoginVM_AuthenticateUserSuccess;

            // Initialize the app startup
            this.BootupSequence();
            //this.NavigateToDesktop();
        }

        private void BootupSequence()
        {
            this.NavigateToSplashScreen();
            //wait for 5 seconds
            Task.Delay(5000).ContinueWith(_ => this.NavigateToLoginForm() );
        }

        private void LoginVM_AuthenticateUserSuccess(object? sender, EventArgs e)
        {
            NavigateToDesktop();
        }
    }
}
