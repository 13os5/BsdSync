namespace EncryptDecryptConStr
{
    partial class Form1
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
            this.gbEncrypt = new System.Windows.Forms.GroupBox();
            this.bEncrypt = new System.Windows.Forms.Button();
            this.txtOUsn = new System.Windows.Forms.TextBox();
            this.txtOPwd = new System.Windows.Forms.TextBox();
            this.txtODb = new System.Windows.Forms.TextBox();
            this.txtOIp = new System.Windows.Forms.TextBox();
            this.txtIUsn = new System.Windows.Forms.TextBox();
            this.txtIPwd = new System.Windows.Forms.TextBox();
            this.txtIDb = new System.Windows.Forms.TextBox();
            this.txtIIp = new System.Windows.Forms.TextBox();
            this.gbDecrypt = new System.Windows.Forms.GroupBox();
            this.bDecrypt = new System.Windows.Forms.Button();
            this.txtOUsnD = new System.Windows.Forms.TextBox();
            this.txtOPwdD = new System.Windows.Forms.TextBox();
            this.txtODbD = new System.Windows.Forms.TextBox();
            this.txtOIpD = new System.Windows.Forms.TextBox();
            this.txtIUsnD = new System.Windows.Forms.TextBox();
            this.txtIPwdD = new System.Windows.Forms.TextBox();
            this.txtIDbD = new System.Windows.Forms.TextBox();
            this.txtIIpD = new System.Windows.Forms.TextBox();
            this.bCopy = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.gbEncrypt.SuspendLayout();
            this.gbDecrypt.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbEncrypt
            // 
            this.gbEncrypt.Controls.Add(this.bCopy);
            this.gbEncrypt.Controls.Add(this.bEncrypt);
            this.gbEncrypt.Controls.Add(this.txtOUsn);
            this.gbEncrypt.Controls.Add(this.txtOPwd);
            this.gbEncrypt.Controls.Add(this.txtODb);
            this.gbEncrypt.Controls.Add(this.txtOIp);
            this.gbEncrypt.Controls.Add(this.txtIUsn);
            this.gbEncrypt.Controls.Add(this.txtIPwd);
            this.gbEncrypt.Controls.Add(this.txtIDb);
            this.gbEncrypt.Controls.Add(this.txtIIp);
            this.gbEncrypt.Location = new System.Drawing.Point(80, 27);
            this.gbEncrypt.Name = "gbEncrypt";
            this.gbEncrypt.Size = new System.Drawing.Size(454, 248);
            this.gbEncrypt.TabIndex = 0;
            this.gbEncrypt.TabStop = false;
            this.gbEncrypt.Text = "Encrypt";
            // 
            // bEncrypt
            // 
            this.bEncrypt.Location = new System.Drawing.Point(49, 175);
            this.bEncrypt.Name = "bEncrypt";
            this.bEncrypt.Size = new System.Drawing.Size(75, 23);
            this.bEncrypt.TabIndex = 8;
            this.bEncrypt.Text = "Encrypt";
            this.bEncrypt.UseVisualStyleBackColor = true;
            this.bEncrypt.Click += new System.EventHandler(this.bEncrypt_Click);
            // 
            // txtOUsn
            // 
            this.txtOUsn.Location = new System.Drawing.Point(249, 110);
            this.txtOUsn.Name = "txtOUsn";
            this.txtOUsn.Size = new System.Drawing.Size(143, 20);
            this.txtOUsn.TabIndex = 7;
            // 
            // txtOPwd
            // 
            this.txtOPwd.Location = new System.Drawing.Point(249, 136);
            this.txtOPwd.Name = "txtOPwd";
            this.txtOPwd.Size = new System.Drawing.Size(143, 20);
            this.txtOPwd.TabIndex = 6;
            // 
            // txtODb
            // 
            this.txtODb.Location = new System.Drawing.Point(249, 84);
            this.txtODb.Name = "txtODb";
            this.txtODb.Size = new System.Drawing.Size(143, 20);
            this.txtODb.TabIndex = 5;
            // 
            // txtOIp
            // 
            this.txtOIp.Location = new System.Drawing.Point(249, 58);
            this.txtOIp.Name = "txtOIp";
            this.txtOIp.Size = new System.Drawing.Size(143, 20);
            this.txtOIp.TabIndex = 4;
            // 
            // txtIUsn
            // 
            this.txtIUsn.Location = new System.Drawing.Point(49, 110);
            this.txtIUsn.Name = "txtIUsn";
            this.txtIUsn.Size = new System.Drawing.Size(143, 20);
            this.txtIUsn.TabIndex = 3;
            // 
            // txtIPwd
            // 
            this.txtIPwd.Location = new System.Drawing.Point(49, 136);
            this.txtIPwd.Name = "txtIPwd";
            this.txtIPwd.Size = new System.Drawing.Size(143, 20);
            this.txtIPwd.TabIndex = 2;
            // 
            // txtIDb
            // 
            this.txtIDb.Location = new System.Drawing.Point(49, 84);
            this.txtIDb.Name = "txtIDb";
            this.txtIDb.Size = new System.Drawing.Size(143, 20);
            this.txtIDb.TabIndex = 1;
            // 
            // txtIIp
            // 
            this.txtIIp.Location = new System.Drawing.Point(49, 58);
            this.txtIIp.Name = "txtIIp";
            this.txtIIp.Size = new System.Drawing.Size(143, 20);
            this.txtIIp.TabIndex = 0;
            // 
            // gbDecrypt
            // 
            this.gbDecrypt.Controls.Add(this.bDecrypt);
            this.gbDecrypt.Controls.Add(this.txtOUsnD);
            this.gbDecrypt.Controls.Add(this.txtOPwdD);
            this.gbDecrypt.Controls.Add(this.txtODbD);
            this.gbDecrypt.Controls.Add(this.txtOIpD);
            this.gbDecrypt.Controls.Add(this.txtIUsnD);
            this.gbDecrypt.Controls.Add(this.txtIPwdD);
            this.gbDecrypt.Controls.Add(this.txtIDbD);
            this.gbDecrypt.Controls.Add(this.txtIIpD);
            this.gbDecrypt.Location = new System.Drawing.Point(80, 311);
            this.gbDecrypt.Name = "gbDecrypt";
            this.gbDecrypt.Size = new System.Drawing.Size(454, 248);
            this.gbDecrypt.TabIndex = 1;
            this.gbDecrypt.TabStop = false;
            this.gbDecrypt.Text = "Decrypt";
            this.gbDecrypt.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // bDecrypt
            // 
            this.bDecrypt.Location = new System.Drawing.Point(49, 175);
            this.bDecrypt.Name = "bDecrypt";
            this.bDecrypt.Size = new System.Drawing.Size(75, 23);
            this.bDecrypt.TabIndex = 8;
            this.bDecrypt.Text = "Decrypt";
            this.bDecrypt.UseVisualStyleBackColor = true;
            this.bDecrypt.Click += new System.EventHandler(this.bDecrypt_Click);
            // 
            // txtOUsnD
            // 
            this.txtOUsnD.Location = new System.Drawing.Point(249, 110);
            this.txtOUsnD.Name = "txtOUsnD";
            this.txtOUsnD.Size = new System.Drawing.Size(143, 20);
            this.txtOUsnD.TabIndex = 7;
            // 
            // txtOPwdD
            // 
            this.txtOPwdD.Location = new System.Drawing.Point(249, 136);
            this.txtOPwdD.Name = "txtOPwdD";
            this.txtOPwdD.Size = new System.Drawing.Size(143, 20);
            this.txtOPwdD.TabIndex = 6;
            // 
            // txtODbD
            // 
            this.txtODbD.Location = new System.Drawing.Point(249, 84);
            this.txtODbD.Name = "txtODbD";
            this.txtODbD.Size = new System.Drawing.Size(143, 20);
            this.txtODbD.TabIndex = 5;
            // 
            // txtOIpD
            // 
            this.txtOIpD.Location = new System.Drawing.Point(249, 58);
            this.txtOIpD.Name = "txtOIpD";
            this.txtOIpD.Size = new System.Drawing.Size(143, 20);
            this.txtOIpD.TabIndex = 4;
            // 
            // txtIUsnD
            // 
            this.txtIUsnD.Location = new System.Drawing.Point(49, 110);
            this.txtIUsnD.Name = "txtIUsnD";
            this.txtIUsnD.Size = new System.Drawing.Size(143, 20);
            this.txtIUsnD.TabIndex = 3;
            // 
            // txtIPwdD
            // 
            this.txtIPwdD.Location = new System.Drawing.Point(49, 136);
            this.txtIPwdD.Name = "txtIPwdD";
            this.txtIPwdD.Size = new System.Drawing.Size(143, 20);
            this.txtIPwdD.TabIndex = 2;
            // 
            // txtIDbD
            // 
            this.txtIDbD.Location = new System.Drawing.Point(49, 84);
            this.txtIDbD.Name = "txtIDbD";
            this.txtIDbD.Size = new System.Drawing.Size(143, 20);
            this.txtIDbD.TabIndex = 1;
            // 
            // txtIIpD
            // 
            this.txtIIpD.Location = new System.Drawing.Point(49, 58);
            this.txtIIpD.Name = "txtIIpD";
            this.txtIIpD.Size = new System.Drawing.Size(143, 20);
            this.txtIIpD.TabIndex = 0;
            // 
            // bCopy
            // 
            this.bCopy.Location = new System.Drawing.Point(249, 175);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(75, 23);
            this.bCopy.TabIndex = 9;
            this.bCopy.Text = "Copy";
            this.bCopy.UseVisualStyleBackColor = true;
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(256, 282);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(75, 23);
            this.bClear.TabIndex = 10;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 609);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.gbDecrypt);
            this.Controls.Add(this.gbEncrypt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.gbEncrypt.ResumeLayout(false);
            this.gbEncrypt.PerformLayout();
            this.gbDecrypt.ResumeLayout(false);
            this.gbDecrypt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEncrypt;
        private System.Windows.Forms.Button bEncrypt;
        private System.Windows.Forms.TextBox txtOUsn;
        private System.Windows.Forms.TextBox txtOPwd;
        private System.Windows.Forms.TextBox txtODb;
        private System.Windows.Forms.TextBox txtOIp;
        private System.Windows.Forms.TextBox txtIUsn;
        private System.Windows.Forms.TextBox txtIPwd;
        private System.Windows.Forms.TextBox txtIDb;
        private System.Windows.Forms.TextBox txtIIp;
        private System.Windows.Forms.GroupBox gbDecrypt;
        private System.Windows.Forms.Button bDecrypt;
        private System.Windows.Forms.TextBox txtOUsnD;
        private System.Windows.Forms.TextBox txtOPwdD;
        private System.Windows.Forms.TextBox txtODbD;
        private System.Windows.Forms.TextBox txtOIpD;
        private System.Windows.Forms.TextBox txtIUsnD;
        private System.Windows.Forms.TextBox txtIPwdD;
        private System.Windows.Forms.TextBox txtIDbD;
        private System.Windows.Forms.TextBox txtIIpD;
        private System.Windows.Forms.Button bCopy;
        private System.Windows.Forms.Button bClear;
    }
}

