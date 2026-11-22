using System.Windows;
using WindowsOptimizer.Gui.ViewModels;

namespace WindowsOptimizer.Gui.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Create_Click(object sender, RoutedEventArgs e) => ViewModel.CreateProfile();
        private void Save_Click(object sender, RoutedEventArgs e) => ViewModel.SaveProfile();
        private void Delete_Click(object sender, RoutedEventArgs e) => ViewModel.DeleteProfile();
        private void Run_Click(object sender, RoutedEventArgs e) => ViewModel.RunOptimization();
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            new HelpWindow { Owner = this }.ShowDialog();
        }
    }
}