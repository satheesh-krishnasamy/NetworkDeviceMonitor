namespace WorkStationAssistant
{
    partial class WorkStationAssistant
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkStationAssistant));
            txtNwInfo = new TextBox();
            tabControl1 = new TabControl();
            tabNotificationsPage = new TabPage();
            grpLaptopInfo = new GroupBox();
            txtLaptopInfo = new TextBox();
            btnLaptopShowTrend = new Button();
            pnlLaptopBatteryInfo = new Panel();
            grpNetworkInfo = new GroupBox();
            btnNwShowTrend = new Button();
            pnlNwBatteryInfo = new Panel();
            tabSettingsPage = new TabPage();
            chkBoxClosePreference = new CheckBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlMessageBox = new Panel();
            lblLastStatusCheckDateTime = new Label();
            refreshButton = new Button();
            lblNwLastStatusChecked = new Label();
            btnOk = new Button();
            systemTrayIcon = new NotifyIcon(components);
            systemTrayIconClickMenu = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabNotificationsPage.SuspendLayout();
            grpLaptopInfo.SuspendLayout();
            grpNetworkInfo.SuspendLayout();
            tabSettingsPage.SuspendLayout();
            pnlMessageBox.SuspendLayout();
            systemTrayIconClickMenu.SuspendLayout();
            SuspendLayout();
            // 
            // txtNwInfo
            // 
            txtNwInfo.Location = new Point(6, 22);
            txtNwInfo.Multiline = true;
            txtNwInfo.Name = "txtNwInfo";
            txtNwInfo.ReadOnly = true;
            txtNwInfo.Size = new Size(215, 112);
            txtNwInfo.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabNotificationsPage);
            tabControl1.Controls.Add(tabSettingsPage);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(629, 234);
            tabControl1.TabIndex = 2;
            // 
            // tabNotificationsPage
            // 
            tabNotificationsPage.Controls.Add(grpLaptopInfo);
            tabNotificationsPage.Controls.Add(grpNetworkInfo);
            tabNotificationsPage.Location = new Point(4, 24);
            tabNotificationsPage.Name = "tabNotificationsPage";
            tabNotificationsPage.Padding = new Padding(3);
            tabNotificationsPage.Size = new Size(621, 206);
            tabNotificationsPage.TabIndex = 0;
            tabNotificationsPage.Text = "Notification";
            tabNotificationsPage.UseVisualStyleBackColor = true;
            // 
            // grpLaptopInfo
            // 
            grpLaptopInfo.Controls.Add(txtLaptopInfo);
            grpLaptopInfo.Controls.Add(btnLaptopShowTrend);
            grpLaptopInfo.Controls.Add(pnlLaptopBatteryInfo);
            grpLaptopInfo.Location = new Point(321, 6);
            grpLaptopInfo.Name = "grpLaptopInfo";
            grpLaptopInfo.Size = new Size(287, 172);
            grpLaptopInfo.TabIndex = 10;
            grpLaptopInfo.TabStop = false;
            grpLaptopInfo.Text = "Laptop";
            // 
            // txtLaptopInfo
            // 
            txtLaptopInfo.Location = new Point(6, 22);
            txtLaptopInfo.Multiline = true;
            txtLaptopInfo.Name = "txtLaptopInfo";
            txtLaptopInfo.ReadOnly = true;
            txtLaptopInfo.Size = new Size(215, 112);
            txtLaptopInfo.TabIndex = 0;
            // 
            // btnLaptopShowTrend
            // 
            btnLaptopShowTrend.Location = new Point(6, 140);
            btnLaptopShowTrend.Name = "btnLaptopShowTrend";
            btnLaptopShowTrend.Size = new Size(93, 23);
            btnLaptopShowTrend.TabIndex = 7;
            btnLaptopShowTrend.Text = "Show Trend";
            btnLaptopShowTrend.UseVisualStyleBackColor = true;
            btnLaptopShowTrend.Click += btnLaptopShowTrend_Click;
            // 
            // pnlLaptopBatteryInfo
            // 
            pnlLaptopBatteryInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlLaptopBatteryInfo.BackColor = Color.Transparent;
            pnlLaptopBatteryInfo.Location = new Point(228, 22);
            pnlLaptopBatteryInfo.Name = "pnlLaptopBatteryInfo";
            pnlLaptopBatteryInfo.Size = new Size(47, 112);
            pnlLaptopBatteryInfo.TabIndex = 6;
            // 
            // grpNetworkInfo
            // 
            grpNetworkInfo.Controls.Add(txtNwInfo);
            grpNetworkInfo.Controls.Add(btnNwShowTrend);
            grpNetworkInfo.Controls.Add(pnlNwBatteryInfo);
            grpNetworkInfo.Location = new Point(6, 6);
            grpNetworkInfo.Name = "grpNetworkInfo";
            grpNetworkInfo.Size = new Size(287, 178);
            grpNetworkInfo.TabIndex = 9;
            grpNetworkInfo.TabStop = false;
            grpNetworkInfo.Text = "Network";
            // 
            // btnNwShowTrend
            // 
            btnNwShowTrend.Location = new Point(6, 140);
            btnNwShowTrend.Name = "btnNwShowTrend";
            btnNwShowTrend.Size = new Size(93, 23);
            btnNwShowTrend.TabIndex = 7;
            btnNwShowTrend.Text = "Show Trend";
            btnNwShowTrend.UseVisualStyleBackColor = true;
            btnNwShowTrend.Click += btnShowBatteryTrend_Click;
            // 
            // pnlNwBatteryInfo
            // 
            pnlNwBatteryInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlNwBatteryInfo.BackColor = Color.Transparent;
            pnlNwBatteryInfo.Location = new Point(228, 22);
            pnlNwBatteryInfo.Name = "pnlNwBatteryInfo";
            pnlNwBatteryInfo.Size = new Size(47, 112);
            pnlNwBatteryInfo.TabIndex = 6;
            // 
            // tabSettingsPage
            // 
            tabSettingsPage.Controls.Add(chkBoxClosePreference);
            tabSettingsPage.Controls.Add(tableLayoutPanel1);
            tabSettingsPage.Location = new Point(4, 24);
            tabSettingsPage.Name = "tabSettingsPage";
            tabSettingsPage.Padding = new Padding(3);
            tabSettingsPage.Size = new Size(621, 206);
            tabSettingsPage.TabIndex = 1;
            tabSettingsPage.Text = "Settings";
            tabSettingsPage.UseVisualStyleBackColor = true;
            // 
            // chkBoxClosePreference
            // 
            chkBoxClosePreference.AutoSize = true;
            chkBoxClosePreference.Checked = true;
            chkBoxClosePreference.CheckState = CheckState.Checked;
            chkBoxClosePreference.Location = new Point(10, 15);
            chkBoxClosePreference.Name = "chkBoxClosePreference";
            chkBoxClosePreference.Size = new Size(181, 19);
            chkBoxClosePreference.TabIndex = 1;
            chkBoxClosePreference.Text = "Minimize window upon close";
            chkBoxClosePreference.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(17, 61);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(200, 100);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlMessageBox
            // 
            pnlMessageBox.AutoScroll = true;
            pnlMessageBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlMessageBox.Controls.Add(lblLastStatusCheckDateTime);
            pnlMessageBox.Location = new Point(135, 249);
            pnlMessageBox.Name = "pnlMessageBox";
            pnlMessageBox.Size = new Size(215, 40);
            pnlMessageBox.TabIndex = 8;
            // 
            // lblLastStatusCheckDateTime
            // 
            lblLastStatusCheckDateTime.AutoSize = true;
            lblLastStatusCheckDateTime.Location = new Point(3, 0);
            lblLastStatusCheckDateTime.Name = "lblLastStatusCheckDateTime";
            lblLastStatusCheckDateTime.Size = new Size(107, 15);
            lblLastStatusCheckDateTime.TabIndex = 3;
            lblLastStatusCheckDateTime.Text = "<not checked yet>";
            lblLastStatusCheckDateTime.Click += lblLastStatusCheckDateTime_Click;
            // 
            // refreshButton
            // 
            refreshButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            refreshButton.Location = new Point(479, 252);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(75, 23);
            refreshButton.TabIndex = 5;
            refreshButton.Text = "&Refresh";
            refreshButton.UseVisualStyleBackColor = true;
            refreshButton.Click += refreshButton_Click;
            // 
            // lblNwLastStatusChecked
            // 
            lblNwLastStatusChecked.AutoSize = true;
            lblNwLastStatusChecked.Location = new Point(16, 249);
            lblNwLastStatusChecked.Name = "lblNwLastStatusChecked";
            lblNwLastStatusChecked.Size = new Size(113, 15);
            lblNwLastStatusChecked.TabIndex = 4;
            lblNwLastStatusChecked.Text = "Last status check on";
            // 
            // btnOk
            // 
            btnOk.Location = new Point(560, 252);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 2;
            btnOk.Text = "&Ok";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click_1;
            // 
            // systemTrayIcon
            // 
            systemTrayIcon.BalloonTipIcon = ToolTipIcon.Warning;
            systemTrayIcon.ContextMenuStrip = systemTrayIconClickMenu;
            systemTrayIcon.Icon = (Icon)resources.GetObject("systemTrayIcon.Icon");
            systemTrayIcon.Text = "Network device status";
            systemTrayIcon.Visible = true;
            systemTrayIcon.MouseClick += systemTrayIcon_MouseClick;
            // 
            // systemTrayIconClickMenu
            // 
            systemTrayIconClickMenu.ImageScalingSize = new Size(20, 20);
            systemTrayIconClickMenu.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, exitToolStripMenuItem });
            systemTrayIconClickMenu.Name = "systemTrayIconClickMenu";
            systemTrayIconClickMenu.Size = new Size(104, 48);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(103, 22);
            toolStripMenuItem1.Text = "&Show";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(103, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // WorkStationAssistant
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(654, 300);
            ContextMenuStrip = systemTrayIconClickMenu;
            Controls.Add(pnlMessageBox);
            Controls.Add(tabControl1);
            Controls.Add(btnOk);
            Controls.Add(refreshButton);
            Controls.Add(lblNwLastStatusChecked);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "WorkStationAssistant";
            Text = "Workstation Assistant";
            FormClosing += NetworkDevice_FormClosing;
            Load += NetworkDevice_Load;
            tabControl1.ResumeLayout(false);
            tabNotificationsPage.ResumeLayout(false);
            grpLaptopInfo.ResumeLayout(false);
            grpLaptopInfo.PerformLayout();
            grpNetworkInfo.ResumeLayout(false);
            grpNetworkInfo.PerformLayout();
            tabSettingsPage.ResumeLayout(false);
            tabSettingsPage.PerformLayout();
            pnlMessageBox.ResumeLayout(false);
            pnlMessageBox.PerformLayout();
            systemTrayIconClickMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNwInfo;
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
        private Button btnNwShowTrend;
        private Label lblNwLastStatusChecked;
        private Button refreshButton;
        private Panel pnlNwBatteryInfo;
        private Button btnShowBatteryTrend;
        private Panel pnlMessageBox;
        private GroupBox grpLaptopInfo;
        private TextBox txtLaptopInfo;
        private Panel pnlLaptopLastCheckedStatusContainer;
        private Label lblLaptopLastCheckedResult;
        private Button btnLaptopShowTrend;
        private Panel pnlLaptopBatteryInfo;
        private Label lblLaptopLastStatusChecked;
        private GroupBox grpNetworkInfo;
    }
}