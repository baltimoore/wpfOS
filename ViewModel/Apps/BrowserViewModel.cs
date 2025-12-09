using Microsoft.Web.WebView2.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel.Apps
{
    public class BrowserViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /******************************************
         ***********                    ***********
         *********   Browser properties   *********
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private string _currentBrowserUrl;
        public string BrowserUrl
        {
            get { return _currentBrowserUrl; }
            set
            {
                _currentBrowserUrl = value; OnPropertyChanged(nameof(BrowserUrl));

                PageNavigationHandling();
            }
        }

        const string homepage = "https://www.lbtu.lv/lv";
        private Stack<String> _historyBack;
        private Stack<String> _historyForward;

        /******************************************
         ***********                    ***********
         *********   Browser properties   *********
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        public BrowserViewModel()
        {
            _historyBack = new();
            _historyForward = new();
            BrowserUrl = homepage;

            // Relay commands
            GoHistoryBackCommand = new RelayCommand(_ => this.BrowserHistoryBackwards());
            GoHistoryForwardCommand = new RelayCommand(_ => this.BrowserHistoryForwards());
            GoHomepageCommand = new RelayCommand(_ => this.BrowserUrl = homepage);
        }



        /******************************************
         **********                      **********
         ********    View functionality    ********
         **********        START         **********
         ****************         *****************
         ******************************************/

        public RelayCommand GoHomepageCommand { get; }

        public RelayCommand GoHistoryBackCommand { get; }
        private void BrowserHistoryBackwards()
        {
            // Can't go back if there's nothing registered in the past
            if (_historyBack.Count == 1)
                return;

            this._historyForward.Push(this._historyBack.Pop());
            BrowserUrl = _historyBack.Peek();
        }

        public RelayCommand GoHistoryForwardCommand { get; }
        private void BrowserHistoryForwards()
        {
            // Can't go into the future if there's nothing registered
            if (_historyForward.Count == 0)
                return;

            this._historyBack.Push(this._historyForward.Pop());
            BrowserUrl = _historyForward.Peek();
        }

        private void PageNavigationHandling()
        {
            if (this._historyBack.Count == 0
             || this._historyBack.Peek() != BrowserUrl)
            {
                this._historyBack.Push(BrowserUrl);
            }

            // Clear the forward history if the new page is out of the future prediction
            if (this._historyForward.Count > 0
             && this._historyForward.Peek() != BrowserUrl)
            {
                this._historyForward.Clear();
            }
        }
    }
}
