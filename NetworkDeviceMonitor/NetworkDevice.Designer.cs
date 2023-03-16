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
            this.LstCheckedOnLabel = new System.Windows.Forms.Label();
            this.lblLastStatusCheckDateTime = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.tabSettingsPage = new System.Windows.Forms.TabPage();
            this.chkBoxClosePreference = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.systemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.systemTrayIconClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabNotificationsPage.SuspendLayout();
            this.tabSettingsPage.SuspendLayout();
            this.systemTrayIconClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNotificationTextBox
            // 
            this.txtNotificationTextBox.Location = new System.Drawing.Point(7, 8);
            this.txtNotificationTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNotificationTextBox.Multiline = true;
            this.txtNotificationTextBox.Name = "txtNotificationTextBox";
            this.txtNotificationTextBox.ReadOnly = true;
            this.txtNotificationTextBox.Size = new System.Drawing.Size(614, 148);
            this.txtNotificationTextBox.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabNotificationsPage);
            this.tabControl1.Controls.Add(this.tabSettingsPage);
            this.tabControl1.Location = new System.Drawing.Point(14, 16);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(638, 260);
            this.tabControl1.TabIndex = 2;
            // 
            // tabNotificationsPage
            // 
            this.tabNotificationsPage.Controls.Add(this.refreshButton);
            this.tabNotificationsPage.Controls.Add(this.LstCheckedOnLabel);
            this.tabNotificationsPage.Controls.Add(this.lblLastStatusCheckDateTime);
            this.tabNotificationsPage.Controls.Add(this.btnOk);
            this.tabNotificationsPage.Controls.Add(this.txtNotificationTextBox);
            this.tabNotificationsPage.Location = new System.Drawing.Point(4, 29);
            this.tabNotificationsPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabNotificationsPage.Name = "tabNotificationsPage";
            this.tabNotificationsPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabNotificationsPage.Size = new System.Drawing.Size(630, 227);
            this.tabNotificationsPage.TabIndex = 0;
            this.tabNotificationsPage.Text = "Notification";
            this.tabNotificationsPage.UseVisualStyleBackColor = true;
            // 
            // LstCheckedOnLabel
            // 
            this.LstCheckedOnLabel.AutoSize = true;
            this.LstCheckedOnLabel.Location = new System.Drawing.Point(16, 170);
            this.LstCheckedOnLabel.Name = "LstCheckedOnLabel";
            this.LstCheckedOnLabel.Size = new System.Drawing.Size(139, 20);
            this.LstCheckedOnLabel.TabIndex = 4;
            this.LstCheckedOnLabel.Text = "Last status check on";
            // 
            // lblLastStatusCheckDateTime
            // 
            this.lblLastStatusCheckDateTime.AutoSize = true;
            this.lblLastStatusCheckDateTime.Location = new System.Drawing.Point(161, 170);
            this.lblLastStatusCheckDateTime.Name = "lblLastStatusCheckDateTime";
            this.lblLastStatusCheckDateTime.Size = new System.Drawing.Size(133, 20);
            this.lblLastStatusCheckDateTime.TabIndex = 3;
            this.lblLastStatusCheckDateTime.Text = "<not checked yet>";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(535, 170);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 31);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click_1);
            // 
            // tabSettingsPage
            // 
            this.tabSettingsPage.Controls.Add(this.chkBoxClosePreference);
            this.tabSettingsPage.Controls.Add(this.tableLayoutPanel1);
            this.tabSettingsPage.Location = new System.Drawing.Point(4, 29);
            this.tabSettingsPage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSettingsPage.Name = "tabSettingsPage";
            this.tabSettingsPage.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSettingsPage.Size = new System.Drawing.Size(630, 227);
            this.tabSettingsPage.TabIndex = 1;
            this.tabSettingsPage.Text = "Settings";
            this.tabSettingsPage.UseVisualStyleBackColor = true;
            // 
            // chkBoxClosePreference
            // 
            this.chkBoxClosePreference.AutoSize = true;
            this.chkBoxClosePreference.Checked = true;
            this.chkBoxClosePreference.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxClosePreference.Location = new System.Drawing.Point(11, 20);
            this.chkBoxClosePreference.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkBoxClosePreference.Name = "chkBoxClosePreference";
            this.chkBoxClosePreference.Size = new System.Drawing.Size(224, 24);
            this.chkBoxClosePreference.TabIndex = 1;
            this.chkBoxClosePreference.Text = "Minimize window upon close";
            this.chkBoxClosePreference.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(19, 81);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(229, 133);
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
            this.systemTrayIconClickMenu.Size = new System.Drawing.Size(115, 52);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(114, 24);
            this.toolStripMenuItem1.Text = "&Show";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(421, 170);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(86, 31);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "&Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // NetworkDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(665, 279);
            this.ContextMenuStrip = this.systemTrayIconClickMenu;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "NetworkDevice";
            this.Text = "Network device";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkDevice_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabNotificationsPage.ResumeLayout(false);
            this.tabNotificationsPage.PerformLayout();
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
    }
}