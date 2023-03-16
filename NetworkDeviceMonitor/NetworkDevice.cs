using Microsoft.Extensions.Configuration;
using NetworkDeviceMonitor.Lib.Config;
using NetworkDeviceMonitor.Lib.Logic;
using NetworkDeviceMonitor.Lib.Model;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NetworkDeviceMonitor
{
    public partial class NetworkDevice : Form
    {
        private readonly JioDeviceMonitorLogic deviceMonitor;
        private bool userForcedClose = false;
        private bool isShownAutomatically = false;
        private string lastMessageShown = string.Empty;
        private DateTime lastRefreshedAt = DateTime.MinValue;


        private delegate void NotificationHandler(NotificationModel notifications);

        public NetworkDevice(IConfiguration config)
        {
            InitializeComponent();

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

                    lastRefreshedAt = notificationsResult.LastStatusCheckOn;
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
            if (lastRefreshedAt > DateTime.Now.AddSeconds(-30))
            {
                MessageBox.Show("Wait 30 seconds before refresh.", "Not allowed");
                return;
            }

            lastRefreshedAt = DateTime.Now;
            refreshButton.Text = "Refreshing...";
            try
            {
                var notifications = await this.deviceMonitor.GetNotificationsAsync();
                if (notifications != null)
                {
                    this.HandleNetworkDeviceNotifications(notifications);
                }
            }
            catch (Exception exp)
            {
                /*Error*/
                MessageBox.Show($"Unable to refresh. [{exp.Message}", "Error");
            }
            finally
            {
                refreshButton.Text = "Refresh";
            }
        }
    }
}