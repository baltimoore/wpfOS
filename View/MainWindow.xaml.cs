using System.Windows;

namespace wpfOs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel.MainWindowModel _context = new ViewModel.MainWindowModel();
            this.DataContext = _context;
        }
    }
}