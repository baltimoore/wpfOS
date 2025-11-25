

namespace wpfOs.ViewModel
{
    public class SplashScreenModel
    {
        // Storing Application properties from MainWindow
        public MainWindowModel MainVM { get; }

        public SplashScreenModel(MainWindowModel main)
        {
            this.MainVM = main;
        }
    }
}
