using System.Windows;
using TPUM.Client.Presentation.ViewModel;

namespace TPUM.Presentation.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.CreateViewModel();
        }
    }
}
