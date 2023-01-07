using InstallerUI.View;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InstallerUI.Pages
{
    /// <summary>
    /// Interaction logic for InstallationProgressPage.xaml
    /// </summary>
    public partial class InstallationProgressPage : Page
    {
        public InstallationProgressPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, EventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.engine.Plan(LaunchAction.Install);

                // UI
                parentWindow.BackBtn.IsEnabled = false;
                parentWindow.NextBtn.IsEnabled = false;
            }
        }

        public void SetProgressBar(int progress)
        {
            progressBar.Value = progress;
        }
    }
}
