using InstallerUI.View;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InstallerUI.Pages
{
    /// <summary>
    /// Interaction logic for InstalledPage.xaml
    /// </summary>
    public partial class InstalledPage : Page
    {
        public InstalledPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, EventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.BackBtn.IsEnabled = false;
                parentWindow.NextBtn.IsEnabled = false;
                parentWindow.CancelBtn.Visibility = Visibility.Visible;
                parentWindow.FinishBtn.Visibility = Visibility.Visible;
            }
        }

        public bool GetLaunchOnExitCheckbox()
        {
            return (bool)launchOnExitCheckbox.IsChecked;
        }
    }
}
