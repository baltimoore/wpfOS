using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel
{
    public class DesktopViewModel
    {
        // Saving MainWindow ViewModel for app-wide data
        public MainWindowModel MainVM { get; }



        public DesktopViewModel(MainWindowModel main)
        {
            this.MainVM = main;
        }
    }
}
