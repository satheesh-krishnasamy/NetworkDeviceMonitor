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
            //if (notifications == null || (notifications.Notifications.Count < 1 && !notifications.IsCharging))
            //{
            //    return;
            //}

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new NotificationHandler(ShowAppInNormalWindowSize), notifications);
            }
            else
            {
                this.ShowAppInNormalWindowSize(notifications);
            }
        }

        private void ShowAppInNormalWindowSize(NotificationModel notificationsResult)
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

            if (notificationsResult.Notifications == null ||
                notificationsResult.Notifications.Count < 1 ||
                notificationsResult.IsCharging)
            {
                // If there is no notifications and the device
                // is getting charged then minimize the window automatically.
                if (isShownAutomatically)
                    MinimizeWindow();

                txtNotificationTextBox.ForeColor = Color.Black;
                txtNotificationTextBox.Text = FormNotificationText(notificationsResult.Information);

                isShownAutomatically = false;
                return;
            }

            string displayText = FormNotificationText(notificationsResult.Notifications) +
                Environment.NewLine +
                FormNotificationText(notificationsResult.Information);

            if (txtNotificationTextBox.Text != null
                && txtNotificationTextBox.Text.Equals(displayText))
            {
                return;
            }

            txtNotificationTextBox.ForeColor = notificationsResult.Notifications.Count > 0 ? Color.Red : Color.Black;
            txtNotificationTextBox.Text = displayText;

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
                    ShowAppInNormalWindowSize(null);
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
            ShowAppInNormalWindowSize(null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.userForcedClose = true;
            this.Close();
        }
    }
}