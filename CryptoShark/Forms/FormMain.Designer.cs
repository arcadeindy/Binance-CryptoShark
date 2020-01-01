namespace CryptoShark.Forms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.applicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageHunting = new System.Windows.Forms.TabPage();
            this.buttonClear = new System.Windows.Forms.Button();
            this.checkBoxLogActive = new System.Windows.Forms.CheckBox();
            this.comboBoxHuntingTypes = new System.Windows.Forms.ComboBox();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPageOpenOrders = new System.Windows.Forms.TabPage();
            this.dataGridViewOpenOrders = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelWallet = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPageAllOrders = new System.Windows.Forms.TabPage();
            this.dataGridViewAllOrders = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageHunting.SuspendLayout();
            this.tabPageOpenOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOpenOrders)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tabPageAllOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAllOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(504, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // applicationToolStripMenuItem
            // 
            this.applicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.applicationToolStripMenuItem.Name = "applicationToolStripMenuItem";
            this.applicationToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.applicationToolStripMenuItem.Text = "Application";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageHunting);
            this.tabControl.Controls.Add(this.tabPageOpenOrders);
            this.tabControl.Controls.Add(this.tabPageAllOrders);
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(504, 498);
            this.tabControl.TabIndex = 6;
            // 
            // tabPageHunting
            // 
            this.tabPageHunting.Controls.Add(this.buttonClear);
            this.tabPageHunting.Controls.Add(this.checkBoxLogActive);
            this.tabPageHunting.Controls.Add(this.comboBoxHuntingTypes);
            this.tabPageHunting.Controls.Add(this.buttonStartStop);
            this.tabPageHunting.Controls.Add(this.richTextBox);
            this.tabPageHunting.Location = new System.Drawing.Point(4, 22);
            this.tabPageHunting.Name = "tabPageHunting";
            this.tabPageHunting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHunting.Size = new System.Drawing.Size(496, 472);
            this.tabPageHunting.TabIndex = 0;
            this.tabPageHunting.Text = "Hunting";
            this.tabPageHunting.UseVisualStyleBackColor = true;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(8, 12);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(69, 23);
            this.buttonClear.TabIndex = 9;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // checkBoxLogActive
            // 
            this.checkBoxLogActive.AutoSize = true;
            this.checkBoxLogActive.Checked = true;
            this.checkBoxLogActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogActive.Location = new System.Drawing.Point(83, 16);
            this.checkBoxLogActive.Name = "checkBoxLogActive";
            this.checkBoxLogActive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxLogActive.TabIndex = 8;
            this.checkBoxLogActive.Text = "Active";
            this.checkBoxLogActive.UseVisualStyleBackColor = true;
            // 
            // comboBoxHuntingTypes
            // 
            this.comboBoxHuntingTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxHuntingTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHuntingTypes.FormattingEnabled = true;
            this.comboBoxHuntingTypes.Location = new System.Drawing.Point(260, 12);
            this.comboBoxHuntingTypes.Name = "comboBoxHuntingTypes";
            this.comboBoxHuntingTypes.Size = new System.Drawing.Size(121, 21);
            this.comboBoxHuntingTypes.TabIndex = 7;
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStartStop.Location = new System.Drawing.Point(387, 12);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(101, 23);
            this.buttonStartStop.TabIndex = 6;
            this.buttonStartStop.Text = "Start Hunting";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(8, 41);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(480, 429);
            this.richTextBox.TabIndex = 5;
            this.richTextBox.Text = "";
            // 
            // tabPageOpenOrders
            // 
            this.tabPageOpenOrders.Controls.Add(this.dataGridViewOpenOrders);
            this.tabPageOpenOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageOpenOrders.Name = "tabPageOpenOrders";
            this.tabPageOpenOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOpenOrders.Size = new System.Drawing.Size(496, 472);
            this.tabPageOpenOrders.TabIndex = 1;
            this.tabPageOpenOrders.Text = "Open Orders";
            this.tabPageOpenOrders.UseVisualStyleBackColor = true;
            // 
            // dataGridViewOpenOrders
            // 
            this.dataGridViewOpenOrders.AllowUserToAddRows = false;
            this.dataGridViewOpenOrders.AllowUserToDeleteRows = false;
            this.dataGridViewOpenOrders.AllowUserToResizeRows = false;
            this.dataGridViewOpenOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOpenOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOpenOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOpenOrders.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewOpenOrders.Name = "dataGridViewOpenOrders";
            this.dataGridViewOpenOrders.ReadOnly = true;
            this.dataGridViewOpenOrders.RowHeadersVisible = false;
            this.dataGridViewOpenOrders.Size = new System.Drawing.Size(490, 466);
            this.dataGridViewOpenOrders.TabIndex = 0;
            this.dataGridViewOpenOrders.DataSourceChanged += new System.EventHandler(this.dataGridViewOrders_DataSourceChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelWallet});
            this.statusStrip1.Location = new System.Drawing.Point(0, 525);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(504, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelWallet
            // 
            this.toolStripStatusLabelWallet.Name = "toolStripStatusLabelWallet";
            this.toolStripStatusLabelWallet.Size = new System.Drawing.Size(489, 17);
            this.toolStripStatusLabelWallet.Spring = true;
            this.toolStripStatusLabelWallet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPageAllOrders
            // 
            this.tabPageAllOrders.Controls.Add(this.dataGridViewAllOrders);
            this.tabPageAllOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageAllOrders.Name = "tabPageAllOrders";
            this.tabPageAllOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAllOrders.Size = new System.Drawing.Size(496, 472);
            this.tabPageAllOrders.TabIndex = 2;
            this.tabPageAllOrders.Text = "All Orders";
            this.tabPageAllOrders.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAllOrders
            // 
            this.dataGridViewAllOrders.AllowUserToAddRows = false;
            this.dataGridViewAllOrders.AllowUserToDeleteRows = false;
            this.dataGridViewAllOrders.AllowUserToResizeRows = false;
            this.dataGridViewAllOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAllOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAllOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAllOrders.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewAllOrders.Name = "dataGridViewAllOrders";
            this.dataGridViewAllOrders.ReadOnly = true;
            this.dataGridViewAllOrders.RowHeadersVisible = false;
            this.dataGridViewAllOrders.Size = new System.Drawing.Size(490, 466);
            this.dataGridViewAllOrders.TabIndex = 1;
            this.dataGridViewAllOrders.DataSourceChanged += new System.EventHandler(this.dataGridViewAllOrders_DataSourceChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 547);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "CryptoShark";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageHunting.ResumeLayout(false);
            this.tabPageHunting.PerformLayout();
            this.tabPageOpenOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOpenOrders)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabPageAllOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAllOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageHunting;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.CheckBox checkBoxLogActive;
        private System.Windows.Forms.ComboBox comboBoxHuntingTypes;
        private System.Windows.Forms.Button buttonStartStop;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.TabPage tabPageOpenOrders;
        private System.Windows.Forms.DataGridView dataGridViewOpenOrders;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelWallet;
        private System.Windows.Forms.TabPage tabPageAllOrders;
        private System.Windows.Forms.DataGridView dataGridViewAllOrders;
    }
}

