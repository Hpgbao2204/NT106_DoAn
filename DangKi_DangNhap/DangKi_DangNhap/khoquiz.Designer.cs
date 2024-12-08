namespace DangKi_DangNhap
{
    partial class khoquiz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(khoquiz));
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.linklblDefault = new System.Windows.Forms.LinkLabel();
            this.lblKhoQuiz = new System.Windows.Forms.Label();
            this.controlboxClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 10;
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 50;
            this.guna2Elipse2.TargetControl = this.guna2Panel1;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.OldLace;
            this.guna2Panel1.Controls.Add(this.linklblDefault);
            this.guna2Panel1.Location = new System.Drawing.Point(53, 64);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(889, 509);
            this.guna2Panel1.TabIndex = 0;
            // 
            // linklblDefault
            // 
            this.linklblDefault.AutoSize = true;
            this.linklblDefault.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linklblDefault.Location = new System.Drawing.Point(33, 28);
            this.linklblDefault.Name = "linklblDefault";
            this.linklblDefault.Size = new System.Drawing.Size(76, 25);
            this.linklblDefault.TabIndex = 0;
            this.linklblDefault.TabStop = true;
            this.linklblDefault.Text = "Default";
            // 
            // lblKhoQuiz
            // 
            this.lblKhoQuiz.AutoSize = true;
            this.lblKhoQuiz.BackColor = System.Drawing.Color.Transparent;
            this.lblKhoQuiz.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKhoQuiz.ForeColor = System.Drawing.Color.White;
            this.lblKhoQuiz.Location = new System.Drawing.Point(411, 9);
            this.lblKhoQuiz.Name = "lblKhoQuiz";
            this.lblKhoQuiz.Size = new System.Drawing.Size(156, 45);
            this.lblKhoQuiz.TabIndex = 21;
            this.lblKhoQuiz.Text = "Kho Quiz";
            // 
            // controlboxClose
            // 
            this.controlboxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlboxClose.BackColor = System.Drawing.Color.FloralWhite;
            this.controlboxClose.FillColor = System.Drawing.Color.Transparent;
            this.controlboxClose.IconColor = System.Drawing.Color.Black;
            this.controlboxClose.Location = new System.Drawing.Point(957, 9);
            this.controlboxClose.Name = "controlboxClose";
            this.controlboxClose.Size = new System.Drawing.Size(31, 29);
            this.controlboxClose.TabIndex = 26;
            // 
            // khoquiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1000, 634);
            this.Controls.Add(this.controlboxClose);
            this.Controls.Add(this.lblKhoQuiz);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "khoquiz";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.LinkLabel linklblDefault;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse2;
        private System.Windows.Forms.Label lblKhoQuiz;
        private Guna.UI2.WinForms.Guna2ControlBox controlboxClose;
    }
}