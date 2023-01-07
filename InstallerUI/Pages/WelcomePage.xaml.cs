using InstallerUI.View;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Windows;
using System.Windows.Controls;

namespace InstallerUI.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.BackBtn.IsEnabled = false;
                parentWindow.NextBtn.IsEnabled = true;
            }
        }
    }
}
