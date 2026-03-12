
namespace prototip
{
    partial class Autorisation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Autorisation));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.pbCaptcha = new System.Windows.Forms.PictureBox();
            this.btnRefreshCaptcha = new System.Windows.Forms.Button();
            this.txtCaptcha = new System.Windows.Forms.TextBox();
            this.lblCaptchaError = new System.Windows.Forms.Label();
            this.lblBlockTimer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptcha)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(70, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Авторизация";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Логин";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(7, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "Пароль";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPassword.Location = new System.Drawing.Point(112, 144);
            this.txtPassword.MaxLength = 12;
            this.txtPassword.Multiline = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(173, 33);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogin.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLogin.Location = new System.Drawing.Point(113, 212);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(172, 39);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Войти";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.Location = new System.Drawing.Point(112, 277);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(172, 37);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Выйти";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtLogin
            // 
            this.txtLogin.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtLogin.Location = new System.Drawing.Point(112, 83);
            this.txtLogin.MaxLength = 20;
            this.txtLogin.Multiline = true;
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(173, 33);
            this.txtLogin.TabIndex = 3;
            this.txtLogin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLogin_KeyPress);
            // 
            // pbCaptcha
            // 
            this.pbCaptcha.BackColor = System.Drawing.Color.White;
            this.pbCaptcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCaptcha.Location = new System.Drawing.Point(320, 20);
            this.pbCaptcha.Name = "pbCaptcha";
            this.pbCaptcha.Size = new System.Drawing.Size(250, 80);
            this.pbCaptcha.TabIndex = 10;
            this.pbCaptcha.TabStop = false;
            this.pbCaptcha.Visible = false;
            // 
            // btnRefreshCaptcha
            // 
            this.btnRefreshCaptcha.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnRefreshCaptcha.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefreshCaptcha.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.btnRefreshCaptcha.Location = new System.Drawing.Point(521, 118);
            this.btnRefreshCaptcha.Name = "btnRefreshCaptcha";
            this.btnRefreshCaptcha.Size = new System.Drawing.Size(30, 30);
            this.btnRefreshCaptcha.TabIndex = 11;
            this.btnRefreshCaptcha.Text = "🔄";
            this.btnRefreshCaptcha.UseVisualStyleBackColor = false;
            this.btnRefreshCaptcha.Visible = false;
            // 
            // txtCaptcha
            // 
            this.txtCaptcha.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCaptcha.Font = new System.Drawing.Font("Comic Sans MS", 12F);
            this.txtCaptcha.Location = new System.Drawing.Point(378, 117);
            this.txtCaptcha.MaxLength = 4;
            this.txtCaptcha.Name = "txtCaptcha";
            this.txtCaptcha.Size = new System.Drawing.Size(100, 30);
            this.txtCaptcha.TabIndex = 12;
            this.txtCaptcha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCaptcha.Visible = false;
            // 
            // lblCaptchaError
            // 
            this.lblCaptchaError.AutoSize = true;
            this.lblCaptchaError.Font = new System.Drawing.Font("Comic Sans MS", 9F);
            this.lblCaptchaError.ForeColor = System.Drawing.Color.Red;
            this.lblCaptchaError.Location = new System.Drawing.Point(160, 277);
            this.lblCaptchaError.Name = "lblCaptchaError";
            this.lblCaptchaError.Size = new System.Drawing.Size(0, 17);
            this.lblCaptchaError.TabIndex = 13;
            this.lblCaptchaError.Visible = false;
            // 
            // lblBlockTimer
            // 
            this.lblBlockTimer.AutoSize = true;
            this.lblBlockTimer.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.lblBlockTimer.ForeColor = System.Drawing.Color.Red;
            this.lblBlockTimer.Location = new System.Drawing.Point(334, 232);
            this.lblBlockTimer.Name = "lblBlockTimer";
            this.lblBlockTimer.Size = new System.Drawing.Size(0, 19);
            this.lblBlockTimer.TabIndex = 14;
            // 
            // Autorisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(582, 333);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbCaptcha);
            this.Controls.Add(this.btnRefreshCaptcha);
            this.Controls.Add(this.txtCaptcha);
            this.Controls.Add(this.lblCaptchaError);
            this.Controls.Add(this.lblBlockTimer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Autorisation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.PictureBox pbCaptcha;
        private System.Windows.Forms.Button btnRefreshCaptcha;
        private System.Windows.Forms.TextBox txtCaptcha;
        private System.Windows.Forms.Label lblCaptchaError;
        private System.Windows.Forms.Label lblBlockTimer;
    }
}

