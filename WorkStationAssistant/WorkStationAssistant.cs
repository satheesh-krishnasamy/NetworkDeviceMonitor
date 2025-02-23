using DeviceMonitor.Lib.Shared.Logic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkStationAssistant.CustomControls;
using WorkStationAssistant.Lib.Config;
using WorkStationAssistant.Lib.Logic;
using WorkStationAssistant.Lib.Model;
using System.Diagnostics;
using System.Text;
using WorkStationAssistant.Model;

namespace WorkStationAssistant
{

    public partial class WorkStationAssistant : Form
    {
        private const int RefreshIntervalOnSuccessfulRefresh = 30;
        private const int RefreshIntervalOnFailureRefresh = 10;
        private const FormWindowState DefaultWindowStatusUponAlerts = FormWindowState.Normal;
        private readonly DeviceMonitorDriver deviceMonitor;
        private bool userForcedClose = false;
        private bool isShownAutomatically = true;
        private readonly BasicProgressBar batteryIndicatorNw = new BasicProgressBar();
        private readonly BasicProgressBar batteryIndicatorLaptop = new BasicProgressBar();
        private readonly LastMessageTracking lastMessageTrackingNw = new LastMessageTracking();
        private readonly LastMessageTracking lastMessageTrackingLaptop = new LastMessageTracking();



        private delegate void NotificationHandler(IEnumerable<NotificationModel> notifications);

        public WorkStationAssistant(IConfiguration config)
        {
            InitializeComponent();
            batteryIndicatorNw = GetNewBatteryIndicator(pnlNwBatteryInfo);
            batteryIndicatorLaptop = GetNewBatteryIndicator(pnlLaptopBatteryInfo);

#if DEBUG
            this.Text = this.Text + " - Debug mode";
#endif

            var notificationConfig = new NotificationConfig(config);
            var deviceMonitorsList = new List<IDeviceMonitorLogic>();
            deviceMonitorsList.Add(new JioDeviceMonitorLogic(new HttpClient(), notificationConfig));
            deviceMonitorsList.Add(new LaptopDeviceMonitorLogic(notificationConfig));

            deviceMonitor = new DeviceMonitorDriver(deviceMonitorsList, notificationConfig);
            deviceMonitor.NotificationEvent += OnDeviceNotifications;
            deviceMonitor.StarDeviceMonitor();
        }

        private static BasicProgressBar GetNewBatteryIndicator(Panel containerPanel)
        {
            var bi = new BasicProgressBar();
            bi.Minimum = 0;
            bi.Maximum = 100;
            bi.Value = 0;
            bi.Text = "-";
            bi.Size = new Size(containerPanel.Width, containerPanel.Height);
            bi.Orientation = containerPanel.Width < containerPanel.Height ?
                Orientation.Vertical : Orientation.Horizontal;
            // Set the TextStyle to show the symbol % in the control.
            bi.TextStyle = BasicProgressBar.TextStyleType.Percentage;

            containerPanel.Controls.Add(bi);
            return bi;
        }

        private void OnDeviceNotifications(IEnumerable<NotificationModel> notifications)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new NotificationHandler(HandleNetworkDeviceNotifications), notifications);
            }
            else
            {
                this.HandleNetworkDeviceNotifications(notifications);
            }
        }


        private void HandleNetworkDeviceNotifications(IEnumerable<NotificationModel> notificationsResult)
        {
            if (notificationsResult == null)
            {
                this.Show();
                this.Focus();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.isShownAutomatically = false;
                return;
            }


            foreach (var notification in notificationsResult)
            {
                SetUIControlsForNotifications(notification);
            }

            FormWindowState formState = notificationsResult.Any(n =>
                n.Messages != null &&
                n.Messages.Any(m => m.RequiresAttention)) ?
                DefaultWindowStatusUponAlerts : FormWindowState.Minimized;

            if (formState == FormWindowState.Normal || formState == FormWindowState.Maximized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                isShownAutomatically = true;
            }
            else if (isShownAutomatically)
            {
                this.WindowState = formState;
                this.ShowInTaskbar = false;
                isShownAutomatically = false;
                this.Hide();
            }
            // Let the system tray icon visible as the menu options are needed to close the window.
            // systemTrayIcon.Visible = false;
        }

        private void SetUIControlsForNotifications(NotificationModel notification)
        {
            TextBox pInfoTextBox;
            BasicProgressBar pBatteryIndicator;
            LastMessageTracking lastMsgTracker;

            if (notification.DeviceType == DeviceType.NetworkDevice)
            {
                pInfoTextBox = txtNwInfo;
                pBatteryIndicator = batteryIndicatorNw;
                lastMsgTracker = lastMessageTrackingNw;
            }
            else
            {
                pInfoTextBox = txtLaptopInfo;
                pBatteryIndicator = batteryIndicatorLaptop;
                lastMsgTracker = lastMessageTrackingLaptop;
            }

            ProcessAndShowAllNotificationInfo(
                notification,
                pBatteryIndicator,
                pInfoTextBox,
                lblLastStatusCheckDateTime,
                isShownAutomatically,
                lastMsgTracker,
                systemTrayIcon);
        }

        private static void ProcessAndShowAllNotificationInfo(
            NotificationModel notificationsResult,
            BasicProgressBar pBatteryIndicator,
            TextBox pTextInfo,
            Label pLabellastStatusOn,
            bool isShownAutomatically,
            LastMessageTracking lastMsgForDevice,
            NotifyIcon trayIcon)
        {
            DisplayBatteryStatus(notificationsResult, pBatteryIndicator);
            SetTextboxColors(notificationsResult, pTextInfo);

            if (notificationsResult.Messages == null ||
                !notificationsResult.Messages.Any() ||
                notificationsResult.IsCharging)
            {
                pTextInfo.ForeColor = Color.Black;

                var infoAlone = FormNotificationText(
                    notificationsResult,
                    LogLevel.Trace,
                    LogLevel.Information);

                if (!string.IsNullOrWhiteSpace(infoAlone))
                {
                    lastMsgForDevice.lastMessageShown = infoAlone;
                    pTextInfo.Text = infoAlone;

                    lastMsgForDevice.nextRefreshAt = notificationsResult.LastStatusCheckOn.AddSeconds(RefreshIntervalOnSuccessfulRefresh);
                    pLabellastStatusOn.Text = notificationsResult.LastStatusCheckOn.ToString();
                }
                isShownAutomatically = false;
                return;
            }

            string messageRequireAttention = FormNotificationText(notificationsResult, LogLevel.Warning, LogLevel.Critical);

            string displayText =
                messageRequireAttention +
                Environment.NewLine +
                FormNotificationText(notificationsResult, LogLevel.Trace, LogLevel.Information);

            trayIcon.Text = messageRequireAttention;

            if (lastMsgForDevice.lastMessageShown.Equals(displayText))
            {
                if (!string.IsNullOrWhiteSpace(displayText))
                {
                    pLabellastStatusOn.Text = notificationsResult.LastStatusCheckOn.ToString();
                }
                return;
            }


            if (!string.IsNullOrWhiteSpace(displayText))
            {
                lastMsgForDevice.lastMessageShown = displayText;
                pTextInfo.Text = displayText;

                pLabellastStatusOn.Text = notificationsResult.LastStatusCheckOn.ToString();
            }
        }

        private static void SetTextboxColors(NotificationModel notificationsResult, TextBox pTextInfo)
        {
            pTextInfo.ForeColor = Color.Black;
            if (notificationsResult.Messages.Any(m => m.Severity >= LogLevel.Critical))
            {
                pTextInfo.ForeColor = Color.Red;
            }
            else if (notificationsResult.Messages.Any(m => m.Severity >= LogLevel.Warning))
            {
                pTextInfo.ForeColor = Color.Orange;
            }
        }

        private static void DisplayBatteryStatus(
            NotificationModel notificationsResult,
            BasicProgressBar pbatteryIndicator)
        {
            if (notificationsResult.BatteryPercentage > -1)
            {
                pbatteryIndicator.ForeColor = notificationsResult.IsOnLowBattery ? Color.OrangeRed :
                    notificationsResult.BatteryPercentage >= 80 ? Color.Green : Color.LightGreen;
                pbatteryIndicator.Value = notificationsResult.BatteryPercentage;
                pbatteryIndicator.Text = notificationsResult.BatteryPercentage.ToString();
            }
            else
            {
                pbatteryIndicator.Value = 0;
            }
        }

        private static string FormNotificationText(
            NotificationModel notificationsResult,
            LogLevel minLogLevel,
            LogLevel maxLogLevel)
        {
            if (notificationsResult?.Messages == null || !notificationsResult.Messages.Any())
                return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var m in notificationsResult.Messages.Where(m => m.Severity >= minLogLevel &&
            m.Severity <= maxLogLevel))
            {
                stringBuilder
                    .Append(m.Title)
                    .Append(" : ")
                    .AppendLine(m.Message);
            }


            return stringBuilder.ToString();
        }

        private void NetworkDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkBoxClosePreference.Checked && !this.userForcedClose)
            {
                MinimizeWindow();
                e.Cancel = true;
            }
            else
            {
                DisposeAllComponents();
            }
        }

        private void DisposeAllComponents()
        {
            try
            {
                deviceMonitor.Dispose();
            }
            catch { }
        }

        private void MinimizeWindow()
        {
            this.WindowState = FormWindowState.Minimized;
            // this.notifyIcon1.ShowBalloonTip(20, "Alert", "Network device", ToolTipIcon.Warning);
            this.Hide();
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            this.isShownAutomatically = false;
            this.MinimizeWindow();
        }

        private void systemTrayIcon_MouseClick(object sender, MouseEventArgs clickEvent)
        {
            if (clickEvent == null || clickEvent.Button == MouseButtons.Left)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    HandleNetworkDeviceNotifications(null);
                }
                else
                {
                    this.MinimizeWindow();
                    this.isShownAutomatically = false;
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            isShownAutomatically = false;
            HandleNetworkDeviceNotifications(null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.userForcedClose = true;
            this.Close();
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            await RefreshDeviceStatusAsync(
                lastMessageTrackingNw,
                refreshButton,
                this.deviceMonitor,
                HandleNetworkDeviceNotifications);
        }


        private static async Task RefreshDeviceStatusAsync(
            LastMessageTracking lastMessageTrackingNw,
            Button pRefreshButton,
            DeviceMonitorDriver deviceMonitor,
            NotificationHandler notificationHandler)
        {
            if (DateTime.Now < lastMessageTrackingNw.nextRefreshAt)
            {
                return;
            }


            pRefreshButton.Text = "Refreshing...";
            try
            {
                var notifications = await deviceMonitor.GetNotificationsAsync();
                if (notifications != null)
                {
                    lastMessageTrackingNw.nextRefreshAt = DateTime.Now.AddSeconds(RefreshIntervalOnSuccessfulRefresh);
                    notificationHandler(notifications);
                }
                else
                {
                    lastMessageTrackingNw.nextRefreshAt = DateTime.Now.AddSeconds(RefreshIntervalOnFailureRefresh);
                }
            }
            catch (Exception exp)
            {
                lastMessageTrackingNw.nextRefreshAt = DateTime.Now.AddSeconds(RefreshIntervalOnFailureRefresh);
                /*Error*/
                MessageBox.Show($"Unable to refresh. [{exp.Message}", "Error");
            }
            finally
            {
                pRefreshButton.Text = "Refresh";
            }
        }

        private async void btnShowBatteryTrend_Click(object sender, EventArgs e)
        {
            await ShowBatteryStatusTrendChart(DeviceType.NetworkDevice);
        }

        private async Task ShowBatteryStatusTrendChart(
            DeviceType deviceType)
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var dataPath = Path.Combine(basePath, "app_data", "data.js");
                var chartHtml = Path.Combine(basePath, "app_data", "main.html");
                var chartTemplateHtml = Path.Combine(basePath, "app_data", "main_template.html");

                var mainHtmlFileContent = await File.ReadAllTextAsync(chartTemplateHtml);
                await File.WriteAllTextAsync(chartHtml, mainHtmlFileContent.Replace("data.js", "data.js?v=" + DateTime.UnixEpoch));

                var chartData = await deviceMonitor.GetChartDataAsync(deviceType);
                if (!string.IsNullOrWhiteSpace(chartData))
                {
                    await File.WriteAllTextAsync(dataPath, chartData);
                    try
                    {
                        var p = new Process();
                        p.StartInfo = new ProcessStartInfo(chartHtml)
                        {
                            UseShellExecute = true
                        };
                        p.Start();
                    }
                    catch (Exception errorInOpeningHtmlFile)
                    {
                        lblLastStatusCheckDateTime.Text = errorInOpeningHtmlFile.Message;
                    }
                }
            }
            catch (Exception ErrorInDataPopulation)
            {
                lblLastStatusCheckDateTime.Text = ErrorInDataPopulation.Message;
            }
        }

        private void lblLastStatusCheckDateTime_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(lblLastStatusCheckDateTime.Text);
            }
            catch { }
        }

        private void NetworkDevice_Load(object sender, EventArgs e)
        {
        }

        private async void btnLaptopShowTrend_Click(object sender, EventArgs e)
        {
            await ShowBatteryStatusTrendChart(DeviceType.Laptop);
        }
    }
}