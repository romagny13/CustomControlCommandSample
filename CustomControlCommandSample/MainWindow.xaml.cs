using System.Windows;

namespace CustomControlCommandSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine($"SplitButton Click event {e.OriginalSource}");
        }
    }


}
