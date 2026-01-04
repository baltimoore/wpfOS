using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel
{
    public class DashboardViewModel
    {
        // Saving MainWindow ViewModel for app-wide data
        public MainWindowModel MainVM { get; }

        // Navigation commands
        public RelayCommand NavigateToFileManagerCommand { get; }
        public RelayCommand NavigateToDomainManagementCommand { get; }
        public RelayCommand NavigateToDatabaseManagementCommand { get; }
        public RelayCommand NavigateToEmailManagementCommand { get; }
        public RelayCommand NavigateToStatisticsCommand { get; }
        public RelayCommand NavigateToBackupManagementCommand { get; }
        public RelayCommand NavigateToSettingsCommand { get; }

        public DashboardViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Initialize navigation commands
            NavigateToFileManagerCommand = new RelayCommand(_ => MainVM.NavigateToFileManager());
            NavigateToDomainManagementCommand = new RelayCommand(_ => MainVM.NavigateToDomainManagement());
            NavigateToDatabaseManagementCommand = new RelayCommand(_ => MainVM.NavigateToDatabaseManagement());
            NavigateToEmailManagementCommand = new RelayCommand(_ => MainVM.NavigateToEmailManagement());
            NavigateToStatisticsCommand = new RelayCommand(_ => MainVM.NavigateToStatistics());
            NavigateToBackupManagementCommand = new RelayCommand(_ => MainVM.NavigateToBackupManagement());
            NavigateToSettingsCommand = new RelayCommand(_ => MainVM.NavigateToSettings());
        }
    }
}

