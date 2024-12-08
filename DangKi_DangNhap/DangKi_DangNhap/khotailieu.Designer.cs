using System;

namespace DangKi_DangNhap
{
    partial class khotailieu
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(khotailieu));
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.btnMoFile = new Guna.UI2.WinForms.Guna2Button();
            this.btnMaHoaFile = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTenFile = new System.Windows.Forms.Label();
            this.btnDang = new Guna.UI2.WinForms.Guna2Button();
            this.txtPatch = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDuongDan = new System.Windows.Forms.Label();
            this.txtTenFile = new Guna.UI2.WinForms.Guna2TextBox();
            this.controlboxClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this;
            // 
            // btnMoFile
            // 
            this.btnMoFile.Animated = true;
            this.btnMoFile.BackColor = System.Drawing.Color.Transparent;
            this.btnMoFile.BorderRadius = 10;
            this.btnMoFile.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMoFile.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMoFile.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMoFile.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMoFile.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(112)))), ((int)(((byte)(156)))));
            this.btnMoFile.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnMoFile.ForeColor = System.Drawing.Color.White;
            this.btnMoFile.Location = new System.Drawing.Point(333, 548);
            this.btnMoFile.Name = "btnMoFile";
            this.btnMoFile.Size = new System.Drawing.Size(168, 47);
            this.btnMoFile.TabIndex = 15;
            this.btnMoFile.Text = "Tải lên";
            this.btnMoFile.Click += new System.EventHandler(this.btnMoFile_Click);
            // 
            // btnMaHoaFile
            // 
            this.btnMaHoaFile.Animated = true;
            this.btnMaHoaFile.BackColor = System.Drawing.Color.Transparent;
            this.btnMaHoaFile.BorderRadius = 10;
            this.btnMaHoaFile.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMaHoaFile.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMaHoaFile.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMaHoaFile.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMaHoaFile.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(112)))), ((int)(((byte)(156)))));
            this.btnMaHoaFile.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnMaHoaFile.ForeColor = System.Drawing.Color.White;
            this.btnMaHoaFile.Location = new System.Drawing.Point(627, 548);
            this.btnMaHoaFile.Name = "btnMaHoaFile";
            this.btnMaHoaFile.Size = new System.Drawing.Size(168, 47);
            this.btnMaHoaFile.TabIndex = 16;
            this.btnMaHoaFile.Text = "Mã hóa File";
            // 
            // guna2TextBox1
            // 
            this.guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox1.DefaultText = "";
            this.guna2TextBox1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Location = new System.Drawing.Point(193, 821);
            this.guna2TextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PasswordChar = '\0';
            this.guna2TextBox1.PlaceholderText = "";
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(856, 56);
            this.guna2TextBox1.TabIndex = 17;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.BorderColor = System.Drawing.Color.White;
            this.guna2Panel1.BorderRadius = 10;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.lblTenFile);
            this.guna2Panel1.Location = new System.Drawing.Point(39, 616);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(115, 42);
            this.guna2Panel1.TabIndex = 18;
            // 
            // lblTenFile
            // 
            this.lblTenFile.AutoSize = true;
            this.lblTenFile.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTenFile.Location = new System.Drawing.Point(21, 8);
            this.lblTenFile.Name = "lblTenFile";
            this.lblTenFile.Size = new System.Drawing.Size(73, 25);
            this.lblTenFile.TabIndex = 19;
            this.lblTenFile.Text = "Tên file";
            // 
            // btnDang
            // 
            this.btnDang.Animated = true;
            this.btnDang.BackColor = System.Drawing.Color.Transparent;
            this.btnDang.BorderRadius = 10;
            this.btnDang.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDang.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDang.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDang.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDang.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(112)))), ((int)(((byte)(156)))));
            this.btnDang.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnDang.ForeColor = System.Drawing.Color.White;
            this.btnDang.Location = new System.Drawing.Point(937, 636);
            this.btnDang.Name = "btnDang";
            this.btnDang.Size = new System.Drawing.Size(114, 61);
            this.btnDang.TabIndex = 19;
            this.btnDang.Text = "Tải xuống";
            this.btnDang.Click += new System.EventHandler(this.btnDang_Click);
            // 
            // txtPatch
            // 
            this.txtPatch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPatch.DefaultText = "";
            this.txtPatch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPatch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPatch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPatch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPatch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPatch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPatch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPatch.Location = new System.Drawing.Point(169, 671);
            this.txtPatch.Name = "txtPatch";
            this.txtPatch.PasswordChar = '\0';
            this.txtPatch.PlaceholderText = "";
            this.txtPatch.SelectedText = "";
            this.txtPatch.Size = new System.Drawing.Size(749, 42);
            this.txtPatch.TabIndex = 20;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.BorderColor = System.Drawing.Color.White;
            this.guna2Panel2.BorderRadius = 10;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.lblDuongDan);
            this.guna2Panel2.Location = new System.Drawing.Point(39, 671);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(115, 42);
            this.guna2Panel2.TabIndex = 20;
            // 
            // lblDuongDan
            // 
            this.lblDuongDan.AutoSize = true;
            this.lblDuongDan.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuongDan.Location = new System.Drawing.Point(3, 8);
            this.lblDuongDan.Name = "lblDuongDan";
            this.lblDuongDan.Size = new System.Drawing.Size(109, 25);
            this.lblDuongDan.TabIndex = 19;
            this.lblDuongDan.Text = "Đường dẫn";
            // 
            // txtTenFile
            // 
            this.txtTenFile.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenFile.DefaultText = "";
            this.txtTenFile.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTenFile.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTenFile.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenFile.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenFile.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenFile.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenFile.Location = new System.Drawing.Point(169, 616);
            this.txtTenFile.Name = "txtTenFile";
            this.txtTenFile.PasswordChar = '\0';
            this.txtTenFile.PlaceholderText = "";
            this.txtTenFile.SelectedText = "";
            this.txtTenFile.Size = new System.Drawing.Size(749, 42);
            this.txtTenFile.TabIndex = 21;
            this.txtTenFile.TextChanged += new System.EventHandler(this.txtTenFile_TextChanged);
            // 
            // controlboxClose
            // 
            this.controlboxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlboxClose.BackColor = System.Drawing.Color.OldLace;
            this.controlboxClose.FillColor = System.Drawing.Color.Transparent;
            this.controlboxClose.IconColor = System.Drawing.Color.Black;
            this.controlboxClose.Location = new System.Drawing.Point(1085, 7);
            this.controlboxClose.Name = "controlboxClose";
            this.controlboxClose.Size = new System.Drawing.Size(31, 29);
            this.controlboxClose.TabIndex = 24;
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 40);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1072, 479);
            this.listView1.TabIndex = 25;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // khotailieu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1128, 725);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.controlboxClose);
            this.Controls.Add(this.txtTenFile);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.txtPatch);
            this.Controls.Add(this.btnDang);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2TextBox1);
            this.Controls.Add(this.btnMaHoaFile);
            this.Controls.Add(this.btnMoFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "khotailieu";
            this.Text = "khotailieu";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        private void txtPatch_TextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnMaHoaFile_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2Button btnMaHoaFile;
        private Guna.UI2.WinForms.Guna2Button btnMoFile;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label lblTenFile;
        private Guna.UI2.WinForms.Guna2TextBox txtPatch;
        private Guna.UI2.WinForms.Guna2Button btnDang;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label lblDuongDan;
        private Guna.UI2.WinForms.Guna2TextBox txtTenFile;
        private Guna.UI2.WinForms.Guna2ControlBox controlboxClose;
        private System.Windows.Forms.ListView listView1;
    }
}