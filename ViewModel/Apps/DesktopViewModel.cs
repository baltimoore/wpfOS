using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace wpfOs.ViewModel
{
    public class DesktopViewModel
    {
        // Saving MainWindow ViewModel for app-wide data
        public MainWindowModel MainVM { get; }

        private object _currentAppModel;
        public object CurrentAppModel
        {
            get { return this._currentAppModel; }
            set { SetProperty(ref this._currentAppModel, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }



        public DesktopViewModel(MainWindowModel main)
        {
            this.MainVM = main;
        }
    }
}
