

namespace wpfOs.ViewModel
{
    public class DesktopViewModel
    {
        // Storing Application properties from MainWindow
        public MainWindowModel MainVM { get; }

        public DesktopViewModel(MainWindowModel main)
        {
            this.MainVM = main;
        }
    }
}
