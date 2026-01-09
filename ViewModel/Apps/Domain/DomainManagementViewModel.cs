using System.ComponentModel;
using System.Runtime.CompilerServices;
using wpfOs.Model;

namespace wpfOs.ViewModel.Apps.Domain
{
    public class DomainManagementViewModel : INotifyPropertyChanged
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

        public List<Model.Domain> RegisteredDomains
        {
            get
            {
                List<Model.Domain>
                MainVM.AuthService.GetAllUsers(MainVM.CurrentUser);
            }
        }

        /******************************************
         ********                          ********
         ********  Domain management view  ********
         ******          properties          ******
         ***********         END        ***********
         ******************      ******************
         ******************************************/



        public DomainManagementViewModel(MainWindowModel main)
        {
            this.MainVM = main;

            // Relay commands
            NavigateBackCommand = new RelayCommand(_ => MainVM.NavigateToDashboard());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        // System control commands, inherited from MainVM
        public RelayCommand NavigateBackCommand { get; }

    }
}

