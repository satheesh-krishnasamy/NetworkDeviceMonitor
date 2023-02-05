using Microsoft.Extensions.Configuration;
using NetworkDeviceMonitor.Lib.Config;
using NetworkDeviceMonitor.Lib.Logic;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NetworkDeviceMonitor
{
    public partial class NetworkDevice : Form
    {
        private readonly JioDeviceMonitorLogic deviceMonitor;
        private bool userForcedClose = false;

        private delegate void NotificationHandler(IReadOnlyDictionary<string, string> notifications);

        public NetworkDevice(IConfiguration config)
        {
            InitializeComponent();

            this.deviceMonitor = new JioDeviceMonitorLogic(new HttpClient(), new NotificationConfig(config));
            this.deviceMonitor.NotificationEvent += OnDeviceNotifications;
            this.deviceMonitor.StarDeviceMonitor();
        }

        private void OnDeviceNotifications(IReadOnlyDictionary<string, string> notifications)
        {
            if (notifications == null || notifications.Count < 1)
                return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new NotificationHandler(ShowAppInNormalWindowSize), notifications);
            }
            else
            {
                this.ShowAppInNormalWindowSize(notifications);
            }
        }

        private void ShowAppInNormalWindowSize(IReadOnlyDictionary<string, string> notifications)
        {
            string notificationText = FormNotificationText(notifications);


            txtNotificationTextBox.Text = notificationText.ToString();

            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
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
            this.MinimizeWindow();
        }

        private void systemTrayIcon_MouseClick(object sender, MouseEventArgs clickEvent)
        {
            if (clickEvent == null || clickEvent.Button == MouseButtons.Left)
                ShowAppInNormalWindowSize(null);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowAppInNormalWindowSize(null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.userForcedClose = true;
            this.Close();
        }
    }
}