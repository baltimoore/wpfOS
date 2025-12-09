using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel.Apps
{
    public class BrowserViewModel
    {
        /******************************************
         ***********                    ***********
         *********   Browser properties   *********
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private string _url;
        public string BrowserUrl
        {
            get { return _url; }
            set
            {
                _url = value; OnPropertyChanged(nameof(BrowserUrl));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /******************************************
         ***********                    ***********
         *********   Browser properties   *********
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        public BrowserViewModel()
        {
            // Relay commands
            ChangeWebpageCommand = new RelayCommand(_ => this.ChangeWebpage());
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/
        public RelayCommand ChangeWebpageCommand { get; }

        private void ChangeWebpage()
        {
            throw new NotImplementedException();
        }

    }
}
