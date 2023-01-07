using InstallerUI.View;
using System.Windows;
using System.Windows.Controls;

namespace InstallerUI.Pages
{
    /// <summary>
    /// Interaction logic for LicenseAgreementPage.xaml
    /// </summary>
    public partial class LicenseAgreementPage : Page
    {
        public LicenseAgreementPage()
        {
            InitializeComponent();
            this.LicenseText.Text = Properties.Resource.LICENSE;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.NextBtn.IsEnabled = true;
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.NextBtn.IsEnabled = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            if (parentWindow != null)
            {
                parentWindow.BackBtn.IsEnabled = true;
                if (acceptRadioBtn.IsChecked == false)
                {
                    parentWindow.NextBtn.IsEnabled = false;
                }
            }
        }
    }
}
