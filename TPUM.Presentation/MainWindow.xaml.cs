using System.Windows;
using TPUM.Presentation.ViewModel;

namespace TPUM.Presentation.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
