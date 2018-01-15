namespace Torrentia
{
    partial class DetailsForm
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
            this.listResults = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtContents = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTorrent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblHash = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnCopyHash = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listResults
            // 
            this.listResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listResults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.listResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listResults.ForeColor = System.Drawing.Color.White;
            this.listResults.FullRowSelect = true;
            this.listResults.Location = new System.Drawing.Point(9, 34);
            this.listResults.MultiSelect = false;
            this.listResults.Name = "listResults";
            this.listResults.ShowGroups = false;
            this.listResults.Size = new System.Drawing.Size(856, 328);
            this.listResults.TabIndex = 76;
            this.listResults.UseCompatibleStateImageBehavior = false;
            this.listResults.View = System.Windows.Forms.View.Details;
            this.listResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listResults_MouseDoubleClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tracker";
            this.columnHeader2.Width = 507;
            // 
            // txtContents
            // 
            this.txtContents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.txtContents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContents.ForeColor = System.Drawing.Color.White;
            this.txtContents.Location = new System.Drawing.Point(0, 0);
            this.txtContents.Name = "txtContents";
            this.txtContents.ReadOnly = true;
            this.txtContents.Size = new System.Drawing.Size(854, 158);
            this.txtContents.TabIndex = 77;
            this.txtContents.Text = "";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtContents);
            this.panel1.Location = new System.Drawing.Point(9, 450);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(856, 160);
            this.panel1.TabIndex = 78;
            // 
            // lblTorrent
            // 
            this.lblTorrent.AutoSize = true;
            this.lblTorrent.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTorrent.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblTorrent.Location = new System.Drawing.Point(4, 3);
            this.lblTorrent.Name = "lblTorrent";
            this.lblTorrent.Size = new System.Drawing.Size(0, 28);
            this.lblTorrent.TabIndex = 79;
            this.lblTorrent.Tag = "themeable";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(4, 419);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 28);
            this.label1.TabIndex = 80;
            this.label1.Tag = "themeable";
            this.label1.Text = "Torrent Contents:";
            // 
            // lblHash
            // 
            this.lblHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHash.AutoSize = true;
            this.lblHash.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHash.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblHash.Location = new System.Drawing.Point(4, 365);
            this.lblHash.Name = "lblHash";
            this.lblHash.Size = new System.Drawing.Size(112, 28);
            this.lblHash.TabIndex = 81;
            this.lblHash.Tag = "themeable";
            this.lblHash.Text = "Info Hash: ";
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnDownload.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnDownload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnDownload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.Location = new System.Drawing.Point(744, 368);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(121, 34);
            this.btnDownload.TabIndex = 82;
            this.btnDownload.Tag = "themeable";
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnCopyHash
            // 
            this.btnCopyHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyHash.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCopyHash.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnCopyHash.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCopyHash.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btnCopyHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyHash.ForeColor = System.Drawing.Color.White;
            this.btnCopyHash.Location = new System.Drawing.Point(617, 368);
            this.btnCopyHash.Name = "btnCopyHash";
            this.btnCopyHash.Size = new System.Drawing.Size(121, 34);
            this.btnCopyHash.TabIndex = 83;
            this.btnCopyHash.Tag = "themeable";
            this.btnCopyHash.Text = "Copy Hash";
            this.btnCopyHash.UseVisualStyleBackColor = false;
            this.btnCopyHash.Click += new System.EventHandler(this.btnCopyHash_Click);
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(876, 622);
            this.Controls.Add(this.btnCopyHash);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lblHash);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTorrent);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listResults);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DetailsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Torrent Details";
            this.Load += new System.EventHandler(this.DetailsForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listResults;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.RichTextBox txtContents;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTorrent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblHash;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnCopyHash;
    }
}