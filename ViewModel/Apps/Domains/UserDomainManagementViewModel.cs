using System.ComponentModel;
using System.Runtime.CompilerServices;
using wpfOs.Model;

namespace wpfOs.ViewModel.Apps.Domains
{
    public class UserDomainManagementViewModel : INotifyPropertyChanged
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

        private string _newDomain;
        public string NewDomainName
        {
            get {  return _newDomain; }
            set {
                _newDomain = value;
                OnPropertyChanged(NewDomainName);
            }
        }

        public Array DomainStatuses
        {
            get { return Enum.GetValues(typeof(DomainStatus)); }
        }

        public List<Domain> DomainList
        {
            get
            {
                List<Domain> domains;
                try
                {
                    // throws error if current user isn't admin
                    domains = MainVM.DomainService.GetAllDomains(MainVM.AuthService, MainVM.CurrentUser);
                }
                catch (Exception ex)
                {
                    // user isn't admin; just grab their domains
                    domains = MainVM.DomainService.GetUserDomains(MainVM.CurrentUser);
                }
                return domains;
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



        public UserDomainManagementViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            NavigateBackCommand = new RelayCommand(_ => MainVM.NavigateToDashboard());
            ClearSelectedDomainCommand = new RelayCommand(_ => ClearSelectedDomain());
            RequestDomainCommand = new RelayCommand(_ => TryRequestDomainManagement());
            CancelDomainCommand = new RelayCommand(_ => TryCancelDomainManagement());
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

        public RelayCommand RequestDomainCommand { get; }
        public void TryRequestDomainManagement()
        {
            try
            {
                if (NewDomainName == null || NewDomainName.Trim().Length == 0)
                    throw new ArgumentException("Ievadiet pārvaldāmā domēna nosaukumu!");

                // throws error if domain already exists
                MainVM.DomainService.RequestDomain(MainVM.CurrentUser, NewDomainName);

                // notify UI and user that userlist has changed
                MessageBoxHelper.ShowSuccess("Domēna pārvaldīšanas pieteikums izveidots veiksmīgi!");
                OnPropertyChanged(nameof(DomainList));
            }
            catch (ArgumentException ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
            }
        }

        public RelayCommand CancelDomainCommand { get; }
        public void TryCancelDomainManagement()
        {
            try
            {
                if (SelectedDomain == null)
                    throw new ArgumentException ("Atlasiet domēnu, kuram pārtraukt pārvaldību!");

                // throws error if user is not owner
                MainVM.DomainService.CancelDomain(MainVM.CurrentUser, SelectedDomain);
            }
            catch (ArgumentException ex)
            {
                MessageBoxHelper.ShowError(ex.Message);
            }
        }
    }
}

