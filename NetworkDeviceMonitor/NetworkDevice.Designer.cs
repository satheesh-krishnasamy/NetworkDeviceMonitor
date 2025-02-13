namespace NetworkDeviceMonitor
{
    partial class NetworkDevice
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetworkDevice));
            this.txtNotificationTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabNotificationsPage = new System.Windows.Forms.TabPage();
            this.pnlMessageBox = new System.Windows.Forms.Panel();
            this.lblLastStatusCheckDateTime = new System.Windows.Forms.Label();
            this.btnShowBatteryTrend = new System.Windows.Forms.Button();
            this.batteryIndicatorSection = new System.Windows.Forms.Panel();
            this.refreshButton = new System.Windows.Forms.Button();
            this.LstCheckedOnLabel = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.tabSettingsPage = new System.Windows.Forms.TabPage();
            this.chkBoxClosePreference = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.systemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.systemTrayIconClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabNotificationsPage.SuspendLayout();
            this.pnlMessageBox.SuspendLayout();
            this.tabSettingsPage.SuspendLayout();
            this.systemTrayIconClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNotificationTextBox
            // 
            this.txtNotificationTextBox.Location = new System.Drawing.Point(6, 6);
            this.txtNotificationTextBox.Multiline = true;
            this.txtNotificationTextBox.Name = "txtNotificationTextBox";
            this.txtNotificationTextBox.ReadOnly = true;
            this.txtNotificationTextBox.Size = new System.Drawing.Size(485, 112);
            this.txtNotificationTextBox.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabNotificationsPage);
            this.tabControl1.Controls.Add(this.tabSettingsPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(558, 221);
            this.tabControl1.TabIndex = 2;
            // 
            // tabNotificationsPage
            // 
            this.tabNotificationsPage.Controls.Add(this.pnlMessageBox);
            this.tabNotificationsPage.Controls.Add(this.btnShowBatteryTrend);
            this.tabNotificationsPage.Controls.Add(this.batteryIndicatorSection);
            this.tabNotificationsPage.Controls.Add(this.refreshButton);
            this.tabNotificationsPage.Controls.Add(this.LstCheckedOnLabel);
            this.tabNotificationsPage.Controls.Add(this.btnOk);
            this.tabNotificationsPage.Controls.Add(this.txtNotificationTextBox);
            this.tabNotificationsPage.Location = new System.Drawing.Point(4, 24);
            this.tabNotificationsPage.Name = "tabNotificationsPage";
            this.tabNotificationsPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotificationsPage.Size = new System.Drawing.Size(550, 193);
            this.tabNotificationsPage.TabIndex = 0;
            this.tabNotificationsPage.Text = "Notification";
            this.tabNotificationsPage.UseVisualStyleBackColor = true;
            // 
            // pnlMessageBox
            // 
            this.pnlMessageBox.AutoScroll = true;
            this.pnlMessageBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMessageBox.Controls.Add(this.lblLastStatusCheckDateTime);
            this.pnlMessageBox.Location = new System.Drawing.Point(133, 128);
            this.pnlMessageBox.Name = "pnlMessageBox";
            this.pnlMessageBox.Size = new System.Drawing.Size(142, 40);
            this.pnlMessageBox.TabIndex = 8;
            // 
            // lblLastStatusCheckDateTime
            // 
            this.lblLastStatusCheckDateTime.AutoSize = true;
            this.lblLastStatusCheckDateTime.Location = new System.Drawing.Point(3, 0);
            this.lblLastStatusCheckDateTime.Name = "lblLastStatusCheckDateTime";
            this.lblLastStatusCheckDateTime.Size = new System.Drawing.Size(107, 15);
            this.lblLastStatusCheckDateTime.TabIndex = 3;
            this.lblLastStatusCheckDateTime.Text = "<not checked yet>";
            this.lblLastStatusCheckDateTime.Click += new System.EventHandler(this.lblLastStatusCheckDateTime_Click);
            // 
            // btnShowBatteryTrend
            // 
            this.btnShowBatteryTrend.Location = new System.Drawing.Point(289, 128);
            this.btnShowBatteryTrend.Name = "btnShowBatteryTrend";
            this.btnShowBatteryTrend.Size = new System.Drawing.Size(93, 23);
            this.btnShowBatteryTrend.TabIndex = 7;
            this.btnShowBatteryTrend.Text = "Show Trend";
            this.btnShowBatteryTrend.UseVisualStyleBackColor = true;
            this.btnShowBatteryTrend.Click += new System.EventHandler(this.btnShowBatteryTrend_Click);
            // 
            // batteryIndicatorSection
            // 
            this.batteryIndicatorSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.batteryIndicatorSection.BackColor = System.Drawing.Color.Transparent;
            this.batteryIndicatorSection.Location = new System.Drawing.Point(497, 6);
            this.batteryIndicatorSection.Name = "batteryIndicatorSection";
            this.batteryIndicatorSection.Size = new System.Drawing.Size(47, 112);
            this.batteryIndicatorSection.TabIndex = 6;
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.refreshButton.Location = new System.Drawing.Point(388, 128);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "&Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // LstCheckedOnLabel
            // 
            this.LstCheckedOnLabel.AutoSize = true;
            this.LstCheckedOnLabel.Location = new System.Drawing.Point(14, 128);
            this.LstCheckedOnLabel.Name = "LstCheckedOnLabel";
            this.LstCheckedOnLabel.Size = new System.Drawing.Size(113, 15);
            this.LstCheckedOnLabel.TabIndex = 4;
            this.LstCheckedOnLabel.Text = "Last status check on";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(469, 128);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click_1);
            // 
            // tabSettingsPage
            // 
            this.tabSettingsPage.Controls.Add(this.chkBoxClosePreference);
            this.tabSettingsPage.Controls.Add(this.tableLayoutPanel1);
            this.tabSettingsPage.Location = new System.Drawing.Point(4, 24);
            this.tabSettingsPage.Name = "tabSettingsPage";
            this.tabSettingsPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingsPage.Size = new System.Drawing.Size(550, 193);
            this.tabSettingsPage.TabIndex = 1;
            this.tabSettingsPage.Text = "Settings";
            this.tabSettingsPage.UseVisualStyleBackColor = true;
            // 
            // chkBoxClosePreference
            // 
            this.chkBoxClosePreference.AutoSize = true;
            this.chkBoxClosePreference.Checked = true;
            this.chkBoxClosePreference.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxClosePreference.Location = new System.Drawing.Point(10, 15);
            this.chkBoxClosePreference.Name = "chkBoxClosePreference";
            this.chkBoxClosePreference.Size = new System.Drawing.Size(181, 19);
            this.chkBoxClosePreference.TabIndex = 1;
            this.chkBoxClosePreference.Text = "Minimize window upon close";
            this.chkBoxClosePreference.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 61);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // systemTrayIcon
            // 
            this.systemTrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.systemTrayIcon.ContextMenuStrip = this.systemTrayIconClickMenu;
            this.systemTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("systemTrayIcon.Icon")));
            this.systemTrayIcon.Text = "Network device status";
            this.systemTrayIcon.Visible = true;
            this.systemTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.systemTrayIcon_MouseClick);
            // 
            // systemTrayIconClickMenu
            // 
            this.systemTrayIconClickMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.systemTrayIconClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.systemTrayIconClickMenu.Name = "systemTrayIconClickMenu";
            this.systemTrayIconClickMenu.Size = new System.Drawing.Size(104, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem1.Text = "&Show";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // NetworkDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(579, 245);
            this.ContextMenuStrip = this.systemTrayIconClickMenu;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NetworkDevice";
            this.Text = "Network device";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkDevice_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabNotificationsPage.ResumeLayout(false);
            this.tabNotificationsPage.PerformLayout();
            this.pnlMessageBox.ResumeLayout(false);
            this.pnlMessageBox.PerformLayout();
            this.tabSettingsPage.ResumeLayout(false);
            this.tabSettingsPage.PerformLayout();
            this.systemTrayIconClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox txtNotificationTextBox;
        private TabControl tabControl1;
        private TabPage tabNotificationsPage;
        private Button btnOk;
        private TabPage tabSettingsPage;
        private TableLayoutPanel tableLayoutPanel1;
        private CheckBox chkBoxClosePreference;
        private NotifyIcon systemTrayIcon;
        private ContextMenuStrip systemTrayIconClickMenu;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Label lblLastStatusCheckDateTime;
        private Label LstCheckedOnLabel;
        private Button refreshButton;
        private Panel batteryIndicatorSection;
        private Button btnShowBatteryTrend;
        private Panel pnlMessageBox;
    }
}