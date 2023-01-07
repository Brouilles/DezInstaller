using InstallerUI.Bootstrapper;
using InstallerUI.Interfaces;
using InstallerUI.Pages;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace InstallerUI.View
{
    /// <summary>
    /// Interaction logic for InstallerUIWindow.xaml
    /// </summary>
    [Export("InstallerUIWindow", typeof(Window))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainWindow : Window
    {
        // Installer
        public BootstrapperApplication bootstrapper;
        public Engine engine;
        public readonly BootstrapperBundleData bootstrapperBundleData;

        [Import]
        private IInteractionService interactionService = null;

        public InstallationStatus Status;
        public bool IsCancelled;
        public bool Installing;
        public string CurrentAction;
        public bool Downgrade;
        public string CurrentPackage;
        public string Progress;
        public int LocalProgress;
        public int GlobalProgress;

        // Pages
        private WelcomePage _welcomePage = new WelcomePage();
        private LicenseAgreementPage _licenseAgreementPage = new LicenseAgreementPage();
        private InstallLocationPage _installLocationPage = new InstallLocationPage();
        private InstallationProgressPage _installationProgressPage = new InstallationProgressPage();
        private InstalledPage _installedPage = new InstalledPage();
        private UninstallPage _uninstallPage = new UninstallPage();

        [ImportingConstructor]
        public MainWindow(BootstrapperApplication bootstrapper, Engine engine)
        {
            bootstrapperBundleData = new BootstrapperBundleData();
            this.bootstrapper = bootstrapper;
            this.engine = engine;

            Loaded += (sender, e) => engine.CloseSplashScreen();
            Closed += (sender, e) => Dispatcher.InvokeShutdown(); // shutdown dispatcher when the window is closed.

            bootstrapper.DetectBegin += (_, ea) =>
            {
                LogEvent("DetectBegin", ea);
                CurrentAction = ea.Installed ? "Preparing for software uninstall" : "Preparing for software install";

                interactionService.RunOnUIThread(() =>
                {
                    Status = ea.Installed ? InstallationStatus.DetectedPresent : InstallationStatus.DetectedAbsent;
                    if (Status == InstallationStatus.DetectedPresent)
                    {
                        MainFrame.Navigate(_uninstallPage);
                    }
                });
            };

            bootstrapper.DetectRelatedBundle += (_, ea) =>
            {
                LogEvent("DetectRelatedBundle", ea);


                interactionService.RunOnUIThread(() => Downgrade |= ea.Operation == RelatedOperation.Downgrade);
            };

            bootstrapper.PlanComplete += (_, ea) =>
            {
                LogEvent("PlanComplete", ea);


                if (ea.Status >= 0)
                {
                    engine.Apply(interactionService.GetMainWindowHandle());
                }
            };

            bootstrapper.ApplyBegin += (_, ea) =>
            {
                LogEvent("ApplyBegin");


                interactionService.RunOnUIThread(() => Installing = true);
            };

            bootstrapper.ExecutePackageBegin += (_, ea) =>
            {
                LogEvent("ExecutePackageBegin", ea);
                CurrentAction = this.Status == InstallationStatus.DetectedAbsent ? "We are installing software" : "We are uninstalling software";

                interactionService.RunOnUIThread(() =>
                CurrentPackage = String.Format("Current package: {0}",
                bootstrapperBundleData.Data.Packages.Where(p => p.Id == ea.PackageId).FirstOrDefault().DisplayName));
            };

            bootstrapper.ExecutePackageComplete += (_, ea) =>
            {
                LogEvent("ExecutePackageComplete", ea);

                interactionService.RunOnUIThread(() => CurrentPackage = string.Empty);
            };

            bootstrapper.ExecuteProgress += (_, ea) =>
            {
                LogEvent("ExecuteProgress", ea);
                if (IsCancelled == true)
                {
                    ea.Result = Result.Abort;
                }
                Progress = String.Format("Progress: {0}{1}", ea.OverallPercentage.ToString(), "%");


                interactionService.RunOnUIThread(() =>
                {
                    LocalProgress = ea.ProgressPercentage;
                    GlobalProgress = ea.OverallPercentage;
                    _installationProgressPage.SetProgressBar(GlobalProgress);
                });
            };

            bootstrapper.ApplyComplete += (_, ea) =>
            {
                LogEvent("ApplyComplete", ea);
                interactionService.RunOnUIThread(() =>
                {
                    switch (MainFrame.Content.GetType().Name.ToString())
                    {
                        case nameof(InstallationProgressPage):
                            MainFrame.Navigate(_installedPage);
                            return;
                        default:
                            Close();
                            break;
                    }
                });
            };

            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _welcomePage.DataContext = DataContext;
            _installationProgressPage.DataContext = DataContext;
            MainFrame.Navigate(_welcomePage);
        }

        private void UninstallBtn_Click(object sender, RoutedEventArgs e)
        {
            engine.Plan(LaunchAction.Uninstall);
        }

        private void FinishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_installedPage.GetLaunchOnExitCheckbox())
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        WorkingDirectory = _installLocationPage.GetInstallPath(),
                        FileName = Properties.Resource.APP_EXE
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            Close();
        }
            
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (MainFrame.Content.GetType().Name.ToString())
            {
                case nameof(WelcomePage):
                    MainFrame.Navigate(_licenseAgreementPage);
                    break;
                case nameof(LicenseAgreementPage):
                    MainFrame.Navigate(_installLocationPage);
                    break;
                case nameof(InstallLocationPage):
                    MainFrame.Navigate(_installationProgressPage);
                    // Update InstallFolder path
                    engine.StringVariables["InstallFolder"] = _installLocationPage.GetInstallPath();
                    break;
                case nameof(InstallationProgressPage):
                    MainFrame.Navigate(_installedPage);
                    //VerifyForInstall();
                    break;
                default:
                    break;
            }
        }
        
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (MainFrame.Content.GetType().Name.ToString())
            {
                case nameof(LicenseAgreementPage):
                    MainFrame.Navigate(_welcomePage);
                    break;
                case nameof(InstallLocationPage):
                    MainFrame.Navigate(_licenseAgreementPage);
                    break;
                default:
                    break;
            }
        }

        private void LogEvent(string eventName, EventArgs arguments = null)
        {
            engine.Log(
                LogLevel.Verbose,
                arguments == null ? string.Format("EVENT: {0}", eventName)
                                    :
                                    string.Format("EVENT: {0} ({1})",
                                                  eventName,
                                                  JsonConvert.SerializeObject(arguments))
            );
        }
    }
}
