using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel
{
    public class DashboardViewModel
    {
        // Saving MainWindow ViewModel for app-wide data
        public MainWindowModel MainVM { get; }



        public DashboardViewModel(MainWindowModel main)
        {
            this.MainVM = main;
        }
    }
}

