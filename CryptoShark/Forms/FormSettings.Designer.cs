namespace CryptoShark.Forms
{
    partial class FormSettings
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
            Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.Data.GenericPropertyListOptions genericPropertyListOptions1 = new Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.Data.GenericPropertyListOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.genericPropertyListControl = new Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.GenericPropertyListControl();
            this.tabControl.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(356, 408);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(275, 408);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(419, 390);
            this.tabControl.TabIndex = 2;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.genericPropertyListControl);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(411, 364);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // genericPropertyListControl
            // 
            this.genericPropertyListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.genericPropertyListControl.AutoScroll = true;
            this.genericPropertyListControl.Location = new System.Drawing.Point(3, 3);
            this.genericPropertyListControl.Margin = new System.Windows.Forms.Padding(0);
            this.genericPropertyListControl.Name = "genericPropertyListControl";
            genericPropertyListOptions1.AutoSize = false;
            genericPropertyListOptions1.CategoryBottomMargin = 7;
            genericPropertyListOptions1.CategoryLeftMargin = 5;
            genericPropertyListOptions1.CategoryRightMargin = 5;
            genericPropertyListOptions1.CategorySeperatorColor = System.Drawing.Color.Silver;
            genericPropertyListOptions1.CategorySeperatorHeight = 1;
            genericPropertyListOptions1.CategoryTitleFont = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            genericPropertyListOptions1.CategoryTitleForeColor = System.Drawing.Color.Silver;
            genericPropertyListOptions1.CategoryTitleHeight = 21;
            genericPropertyListOptions1.CategoryTopMargin = 5;
            genericPropertyListOptions1.ItemGap = 3;
            genericPropertyListOptions1.ItemHeight = 22;
            genericPropertyListOptions1.ItemLabelForeColor = System.Drawing.Color.Empty;
            genericPropertyListOptions1.ItemLeftMargin = 5;
            genericPropertyListOptions1.ItemRightMargin = 5;
            genericPropertyListOptions1.ScrollBarPadding = 3;
            genericPropertyListOptions1.SeperatorOffset = 40;
            genericPropertyListOptions1.SeperatorOffsetType = Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.Enum.SeperatorOffsetType.Percent;
            genericPropertyListOptions1.SeperatorPadding = 5;
            genericPropertyListOptions1.ViewMode = Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.Enum.ViewMode.List;
            this.genericPropertyListControl.Options = genericPropertyListOptions1;
            this.genericPropertyListControl.Properties = null;
            this.genericPropertyListControl.Size = new System.Drawing.Size(405, 358);
            this.genericPropertyListControl.TabIndex = 0;
            this.genericPropertyListControl.Values = null;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(443, 443);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private Quantum.Framework.GenericProperties.Controls.GenericPropertyListControl.GenericPropertyListControl genericPropertyListControl;
    }
}