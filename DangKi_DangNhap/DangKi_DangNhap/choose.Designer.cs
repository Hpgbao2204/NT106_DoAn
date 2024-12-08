namespace DangKi_DangNhap
{
    partial class choose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(choose));
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.btnXoaQuiz = new Guna.UI2.WinForms.Guna2Button();
            this.btnLamQuiz = new Guna.UI2.WinForms.Guna2Button();
            this.controlboxClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.OldLace;
            this.guna2Panel1.Controls.Add(this.btnLamQuiz);
            this.guna2Panel1.Controls.Add(this.btnXoaQuiz);
            this.guna2Panel1.Location = new System.Drawing.Point(42, 44);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(285, 240);
            this.guna2Panel1.TabIndex = 0;
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 20;
            this.guna2Elipse2.TargetControl = this.guna2Panel1;
            // 
            // btnXoaQuiz
            // 
            this.btnXoaQuiz.Animated = true;
            this.btnXoaQuiz.BackColor = System.Drawing.Color.Transparent;
            this.btnXoaQuiz.BorderRadius = 10;
            this.btnXoaQuiz.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaQuiz.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXoaQuiz.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXoaQuiz.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXoaQuiz.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(112)))), ((int)(((byte)(156)))));
            this.btnXoaQuiz.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnXoaQuiz.ForeColor = System.Drawing.Color.White;
            this.btnXoaQuiz.Location = new System.Drawing.Point(58, 41);
            this.btnXoaQuiz.Name = "btnXoaQuiz";
            this.btnXoaQuiz.Size = new System.Drawing.Size(168, 57);
            this.btnXoaQuiz.TabIndex = 23;
            this.btnXoaQuiz.Text = "Xóa Quiz";
            // 
            // btnLamQuiz
            // 
            this.btnLamQuiz.Animated = true;
            this.btnLamQuiz.BackColor = System.Drawing.Color.Transparent;
            this.btnLamQuiz.BorderRadius = 10;
            this.btnLamQuiz.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLamQuiz.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLamQuiz.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLamQuiz.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLamQuiz.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(112)))), ((int)(((byte)(156)))));
            this.btnLamQuiz.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnLamQuiz.ForeColor = System.Drawing.Color.White;
            this.btnLamQuiz.Location = new System.Drawing.Point(58, 133);
            this.btnLamQuiz.Name = "btnLamQuiz";
            this.btnLamQuiz.Size = new System.Drawing.Size(168, 57);
            this.btnLamQuiz.TabIndex = 24;
            this.btnLamQuiz.Text = "Làm Quiz";
            // 
            // controlboxClose
            // 
            this.controlboxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlboxClose.BackColor = System.Drawing.Color.FloralWhite;
            this.controlboxClose.FillColor = System.Drawing.Color.Transparent;
            this.controlboxClose.IconColor = System.Drawing.Color.Black;
            this.controlboxClose.Location = new System.Drawing.Point(331, 12);
            this.controlboxClose.Name = "controlboxClose";
            this.controlboxClose.Size = new System.Drawing.Size(31, 29);
            this.controlboxClose.TabIndex = 26;
            // 
            // choose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(374, 312);
            this.Controls.Add(this.controlboxClose);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "choose";
            this.Text = "choose";
            this.guna2Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
        private Guna.UI2.WinForms.Guna2Button btnLamQuiz;
        private Guna.UI2.WinForms.Guna2Button btnXoaQuiz;
        private Guna.UI2.WinForms.Guna2ControlBox controlboxClose;
    }
}