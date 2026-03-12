
namespace prototip
{
    partial class MainAdmin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainAdmin));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnBooks = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnSettings = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(93, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Бронирование квестов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(166, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "администратор Иванов.И.А.";
            // 
            // btnUsers
            // 
            this.btnUsers.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUsers.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUsers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUsers.Location = new System.Drawing.Point(11, 117);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(158, 42);
            this.btnUsers.TabIndex = 0;
            this.btnUsers.Text = "Пользователи";
            this.btnUsers.UseVisualStyleBackColor = false;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // btnOrders
            // 
            this.btnOrders.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOrders.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOrders.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOrders.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOrders.Location = new System.Drawing.Point(11, 195);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(158, 42);
            this.btnOrders.TabIndex = 1;
            this.btnOrders.Text = "Учет заказов";
            this.btnOrders.UseVisualStyleBackColor = false;
            this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.Location = new System.Drawing.Point(196, 381);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(193, 42);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Выйти из аккаунта";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnBooks
            // 
            this.btnBooks.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnBooks.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBooks.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBooks.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBooks.Location = new System.Drawing.Point(11, 274);
            this.btnBooks.Name = "btnBooks";
            this.btnBooks.Size = new System.Drawing.Size(158, 42);
            this.btnBooks.TabIndex = 2;
            this.btnBooks.Text = "Справочники";
            this.btnBooks.UseVisualStyleBackColor = false;
            this.btnBooks.Click += new System.EventHandler(this.btnBooks_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::prototip.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(196, 117);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(193, 199);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(3, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(391, 318);
            this.panel1.TabIndex = 4;
            // 
            // BtnSettings
            // 
            this.BtnSettings.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BtnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnSettings.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnSettings.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnSettings.Location = new System.Drawing.Point(3, 381);
            this.BtnSettings.Name = "BtnSettings";
            this.BtnSettings.Size = new System.Drawing.Size(128, 42);
            this.BtnSettings.TabIndex = 5;
            this.BtnSettings.Text = "Настройки";
            this.BtnSettings.UseVisualStyleBackColor = false;
            this.BtnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // MainAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(401, 435);
            this.Controls.Add(this.BtnSettings);
            this.Controls.Add(this.btnBooks);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOrders);
            this.Controls.Add(this.btnUsers);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Главная - Администратор";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnOrders;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBooks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnSettings;
    }
}