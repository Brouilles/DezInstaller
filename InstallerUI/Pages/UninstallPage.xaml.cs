using InstallerUI.View;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Windows;
using System.Windows.Controls;

namespace InstallerUI.Pages
{
    /// <summary>
    /// Interaction logic for UninstallPage.xaml
    /// </summary>
    public partial class UninstallPage : Page
    {
        public UninstallPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.BackBtn.Visibility = Visibility.Hidden;
                parentWindow.NextBtn.Visibility = Visibility.Hidden;
                parentWindow.CancelBtn.Visibility = Visibility.Hidden;
                parentWindow.FinishBtn.Visibility = Visibility.Hidden;
                parentWindow.UninstallBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
