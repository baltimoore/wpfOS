using System.ComponentModel;
using System.Runtime.CompilerServices;
using wpfOs.Model;

namespace wpfOs.ViewModel.Apps.Domains
{
    public class AdminDomainManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /******************************************
         ********                          ********
         ********  Domain management view  ********
         ******          properties          ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private MainWindowModel MainVM { get; }

        public List<Domain> DomainList
        {
            get
            {
                try
                {
                    // throws error if current user isn't admin
                    return MainVM.DomainService.GetAllDomains(MainVM.AuthService, MainVM.CurrentUser);
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.ShowError(ex.Message);
                }
                List<Domain> empty = new();
                return empty;
            }
        }

        public Array DomainStatuses
        {
            get
            {
                return Enum.GetValues(typeof(DomainStatus));
            }
        }

        private Domain? _domain;
        public Domain? SelectedDomain
        {
            get { return _domain; }
            set
            {
                _domain = value;
                OnPropertyChanged(nameof(SelectedDomain));
            }
        }

        /******************************************
         ********                          ********
         ********  Domain management view  ********
         ******          properties          ******
         ***********         END        ***********
         ******************      ******************
         ******************************************/



        public AdminDomainManagementViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            NavigateBackCommand = new RelayCommand(_ => MainVM.NavigateToDashboard());
            ClearSelectedDomainCommand = new RelayCommand(_ => ClearSelectedDomain());
            DeleteDomainRecordCommand = new RelayCommand(_ => TryDeleteDomainRecord());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        // System control commands, inherited from MainVM
        public RelayCommand NavigateBackCommand { get; }

        /*
         * Actual view functionality
         */
        public RelayCommand ClearSelectedDomainCommand { get; }
        public void ClearSelectedDomain()
        {
            SelectedDomain = null;
        }

        public RelayCommand DeleteDomainRecordCommand { get; }
        public void TryDeleteDomainRecord()
        {
            try
            {
                MainVM.DomainService.DeleteDomainRecord(SelectedDomain, MainVM.AuthService, MainVM.CurrentUser);
            }
            catch (ArgumentException ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
            }
        }
    }
}
