using System.Windows;

using y.Extends.WPF.Extends;

namespace WpfApp1
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //
            Loaded += (s, e) =>
            {
            };
        }

        private void showwindow(object sender, RoutedEventArgs e)
        {
            var w = new Window();
            w.Title = "{510E8C69-F19F-45F3-835A-CE0D09A6EBD2}";
            w.Loaded += (s, e1) =>
            {
                ImmersiveWindiw.Register(w, "{510E8C69-F19F-45F3-835A-CE0D09A6EBD2}");
            };
            w.Show();
        }
    }
}