
namespace prototip
{
    partial class MainManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainManager));
            this.btnExit = new System.Windows.Forms.Button();
            this.btnMakingOrder = new System.Windows.Forms.Button();
            this.btnServices = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOrdersAccount = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClients = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnExit.Location = new System.Drawing.Point(208, 381);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(193, 42);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Выйти из аккаунта";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnMakingOrder
            // 
            this.btnMakingOrder.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnMakingOrder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMakingOrder.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMakingOrder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnMakingOrder.Location = new System.Drawing.Point(7, 138);
            this.btnMakingOrder.Name = "btnMakingOrder";
            this.btnMakingOrder.Size = new System.Drawing.Size(179, 42);
            this.btnMakingOrder.TabIndex = 11;
            this.btnMakingOrder.Text = "Оформление заказа";
            this.btnMakingOrder.UseVisualStyleBackColor = false;
            this.btnMakingOrder.Click += new System.EventHandler(this.btnMakingOrder_Click);
            // 
            // btnServices
            // 
            this.btnServices.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnServices.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnServices.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnServices.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnServices.Location = new System.Drawing.Point(10, 113);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(179, 42);
            this.btnServices.TabIndex = 10;
            this.btnServices.Text = "Услуги";
            this.btnServices.UseVisualStyleBackColor = false;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(213, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "менеджер Иванов.И.А.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(92, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "Бронирование квестов";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOrdersAccount);
            this.groupBox1.Controls.Add(this.btnMakingOrder);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.btnClients);
            this.groupBox1.Location = new System.Drawing.Point(3, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 332);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // btnOrdersAccount
            // 
            this.btnOrdersAccount.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnOrdersAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOrdersAccount.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnOrdersAccount.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOrdersAccount.Location = new System.Drawing.Point(7, 278);
            this.btnOrdersAccount.Name = "btnOrdersAccount";
            this.btnOrdersAccount.Size = new System.Drawing.Size(179, 42);
            this.btnOrdersAccount.TabIndex = 13;
            this.btnOrdersAccount.Text = "Учет заказов";
            this.btnOrdersAccount.UseVisualStyleBackColor = false;
            this.btnOrdersAccount.Click += new System.EventHandler(this.btnOrdersAccount_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::prototip.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(201, 70);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(193, 199);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnClients
            // 
            this.btnClients.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnClients.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClients.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClients.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClients.Location = new System.Drawing.Point(7, 209);
            this.btnClients.Name = "btnClients";
            this.btnClients.Size = new System.Drawing.Size(179, 42);
            this.btnClients.TabIndex = 12;
            this.btnClients.Text = "Клиенты";
            this.btnClients.UseVisualStyleBackColor = false;
            this.btnClients.Click += new System.EventHandler(this.btnClients_Click);
            // 
            // MainManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(410, 435);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnServices);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Главная -Менеджер";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMakingOrder;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClients;
        private System.Windows.Forms.Button btnOrdersAccount;
    }
}