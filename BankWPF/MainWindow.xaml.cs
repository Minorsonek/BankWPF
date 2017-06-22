using BankWPF.Core;
using System.Windows;

namespace BankWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Connect with ViewModel
            this.DataContext = new WindowViewModel(this);
        }
    }
}
