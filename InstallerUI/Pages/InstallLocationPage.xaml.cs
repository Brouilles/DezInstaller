using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace InstallerUI.Pages
{
    /// <summary>
    /// Interaction logic for InstallLocationPage.xaml
    /// </summary>
    public partial class InstallLocationPage : Page
    {
        public InstallLocationPage()
        {
            InitializeComponent();
        }

        static string ProgramFiles()
        {
            return Environment.ExpandEnvironmentVariables("%ProgramW6432%");
        }

        private void Page_Loaded(object sender, EventArgs e)
        {
            installPathTextBox.Text = Path.Combine(ProgramFiles(), Properties.Resource.APP_NAME);
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowser = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = ProgramFiles();
                System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    installPathTextBox.Text = Path.Combine(folderBrowser.SelectedPath, Properties.Resource.APP_NAME);
                }
            }
        }

        public string GetInstallPath()
        {
            return installPathTextBox.Text;
        }
    }
}
