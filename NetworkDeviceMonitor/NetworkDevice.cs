using Microsoft.Extensions.Configuration;
using NetworkDeviceMonitor.CustomControls;
using NetworkDeviceMonitor.Lib.Config;
using NetworkDeviceMonitor.Lib.Logic;
using NetworkDeviceMonitor.Lib.Model;
using System.Diagnostics;
using System.Text;

namespace NetworkDeviceMonitor
{
    public partial class NetworkDevice : Form
    {
        private const int RefreshIntervalOnSuccessfulRefresh = 30;
        private const int RefreshIntervalOnFailureRefresh = 10;
        private readonly JioDeviceMonitorLogic deviceMonitor;
        private bool userForcedClose = false;
        private bool isShownAutomatically = false;
        private string lastMessageShown = string.Empty;
        private DateTime nextRefreshAt = DateTime.MinValue;
        private readonly BasicProgressBar batteryIndicator = new BasicProgressBar();


        private delegate void NotificationHandler(NotificationModel notifications);

        public NetworkDevice(IConfiguration config)
        {
            InitializeComponent();

            batteryIndicator.Minimum = 0;
            batteryIndicator.Maximum = 100;
            batteryIndicator.Value = 0;
            batteryIndicator.Text = "-";
            batteryIndicator.Size = new Size(batteryIndicatorSection.Width, batteryIndicatorSection.Height);
            batteryIndicator.Orientation = batteryIndicatorSection.Width < batteryIndicatorSection.Height ?
                Orientation.Vertical : Orientation.Horizontal;
            // Set the TextStyle to show the symbol % in the control.
            batteryIndicator.TextStyle = BasicProgressBar.TextStyleType.Percentage;
            batteryIndicatorSection.Controls.Add(batteryIndicator);

            this.deviceMonitor = new JioDeviceMonitorLogic(new HttpClient(), new NotificationConfig(config));
            this.deviceMonitor.NotificationEvent += OnDeviceNotifications;
            this.deviceMonitor.StarDeviceMonitor();



        }

        private void OnDeviceNotifications(NotificationModel notifications)
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


        private void HandleNetworkDeviceNotifications(NotificationModel notificationsResult)
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

            // lblLastStatusCheck.Text = $"Last checked on: {notificationsResult.LastStatusCheckOn}";

            DisplayBatteryStatus(notificationsResult);

            if (notificationsResult.Notifications == null ||
                notificationsResult.Notifications.Count < 1 ||
                notificationsResult.IsCharging)
            {
                // If there is no notifications and the device
                // is getting charged then minimize the window automatically.
                if (isShownAutomatically)
                    MinimizeWindow();

                txtNotificationTextBox.ForeColor = Color.Black;

                var infoAlone = FormNotificationText(notificationsResult.Information);

                if (!string.IsNullOrWhiteSpace(infoAlone))
                {
                    lastMessageShown = infoAlone;
                    txtNotificationTextBox.Text = infoAlone;

                    nextRefreshAt = notificationsResult.LastStatusCheckOn.AddSeconds(RefreshIntervalOnSuccessfulRefresh);
                    lblLastStatusCheckDateTime.Text = notificationsResult.LastStatusCheckOn.ToString();
                }
                isShownAutomatically = false;
                return;
            }

            string displayText = FormNotificationText(notificationsResult.Notifications) +
                Environment.NewLine +
                FormNotificationText(notificationsResult.Information);

            if (lastMessageShown != null
                && lastMessageShown.Equals(displayText))
            {
                if (!string.IsNullOrWhiteSpace(displayText))
                {
                    lblLastStatusCheckDateTime.Text = notificationsResult.LastStatusCheckOn.ToString();
                }
                return;
            }

            txtNotificationTextBox.ForeColor = notificationsResult.Notifications.Count > 0 ? Color.Red : Color.Black;

            if (!string.IsNullOrWhiteSpace(displayText))
            {
                lastMessageShown = displayText;
                txtNotificationTextBox.Text = displayText;

                lblLastStatusCheckDateTime.Text = notificationsResult.LastStatusCheckOn.ToString();
            }

            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            isShownAutomatically = true;
            // Let the system tray icon visible as the menu options are needed to close the window.
            // systemTrayIcon.Visible = false;
        }

        private void DisplayBatteryStatus(NotificationModel notificationsResult)
        {
            if (notificationsResult.BatteryPercentage > -1)
            {
                this.batteryIndicator.ForeColor = notificationsResult.IsOnLowBattery ? Color.OrangeRed :
                    notificationsResult.BatteryPercentage >= 80 ? Color.Green : Color.LightGreen;
                this.batteryIndicator.Value = notificationsResult.BatteryPercentage;
                batteryIndicator.Text = notificationsResult.BatteryPercentage.ToString();
            }
            else
            {
                this.batteryIndicator.Value = 0;
            }
        }

        private string FormNotificationText(IReadOnlyDictionary<string, string> notifications)
        {
            if (notifications == null)
                return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var notice in notifications)
            {
                stringBuilder.Append(notice.Key).Append(" : ").AppendLine(notice.Value);
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
                this.deviceMonitor.Dispose();
            }
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
            if (DateTime.Now < nextRefreshAt)
            {
                return;
            }


            refreshButton.Text = "Refreshing...";
            try
            {
                var notifications = await this.deviceMonitor.GetNotificationsAsync();
                if (notifications != null)
                {
                    nextRefreshAt = DateTime.Now.AddSeconds(RefreshIntervalOnSuccessfulRefresh);
                    this.HandleNetworkDeviceNotifications(notifications);
                }
                else
                {
                    nextRefreshAt = DateTime.Now.AddSeconds(RefreshIntervalOnFailureRefresh);
                }
            }
            catch (Exception exp)
            {
                nextRefreshAt = DateTime.Now.AddSeconds(RefreshIntervalOnFailureRefresh);
                /*Error*/
                MessageBox.Show($"Unable to refresh. [{exp.Message}", "Error");
            }
            finally
            {
                refreshButton.Text = "Refresh";
            }
        }

        private async void btnShowBatteryTrend_Click(object sender, EventArgs e)
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var dataPath = Path.Combine(basePath, "app_data", "data.js");
                var chartHtml = Path.Combine(basePath, "app_data", "main.html");
                var chartTemplateHtml = Path.Combine(basePath, "app_data", "main_template.html");

                var mainHtmlFileContent = await File.ReadAllTextAsync(chartTemplateHtml);
                await File.WriteAllTextAsync(chartHtml, mainHtmlFileContent.Replace("data.js", "data.js?v=" + DateTime.UnixEpoch));

                var chartData = await this.deviceMonitor.GetChartDataAsync();
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
    }
}