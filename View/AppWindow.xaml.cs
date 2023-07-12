using System.Windows;
namespace ATM_TestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        public AppWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
            //ContentControl.Content = new ViewModel();
        }
    }
}
