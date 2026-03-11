
namespace prototip
{
    partial class ClientDetailForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label lblBirthDateLabel;
        private System.Windows.Forms.TextBox txtBirthDate;
        private System.Windows.Forms.Label lblAddressLabel;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblEmailLabel;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassportLabel;
        private System.Windows.Forms.TextBox txtPassport;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAccessLevel;
        private System.Windows.Forms.GroupBox groupMainInfo;
        private System.Windows.Forms.GroupBox groupPersonalData;

 

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientDetailForm));
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupMainInfo = new System.Windows.Forms.GroupBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.groupPersonalData = new System.Windows.Forms.GroupBox();
            this.lblBirthDateLabel = new System.Windows.Forms.Label();
            this.txtBirthDate = new System.Windows.Forms.TextBox();
            this.lblAddressLabel = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblEmailLabel = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassportLabel = new System.Windows.Forms.Label();
            this.txtPassport = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAccessLevel = new System.Windows.Forms.Label();
            this.groupMainInfo.SuspendLayout();
            this.groupPersonalData.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(16, 13);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(245, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Детальная информация";
            // 
            // groupMainInfo
            // 
            this.groupMainInfo.Controls.Add(this.lblFullName);
            this.groupMainInfo.Controls.Add(this.lblPhone);
            this.groupMainInfo.Controls.Add(this.lblAge);
            this.groupMainInfo.Location = new System.Drawing.Point(16, 63);
            this.groupMainInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupMainInfo.Name = "groupMainInfo";
            this.groupMainInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupMainInfo.Size = new System.Drawing.Size(613, 139);
            this.groupMainInfo.TabIndex = 1;
            this.groupMainInfo.TabStop = false;
            this.groupMainInfo.Text = "Основная информация";
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblFullName.Location = new System.Drawing.Point(20, 34);
            this.lblFullName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(46, 17);
            this.lblFullName.TabIndex = 0;
            this.lblFullName.Text = "ФИО:";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblPhone.Location = new System.Drawing.Point(20, 69);
            this.lblPhone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(72, 17);
            this.lblPhone.TabIndex = 1;
            this.lblPhone.Text = "Телефон:";
            // 
            // lblAge
            // 
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblAge.Location = new System.Drawing.Point(20, 104);
            this.lblAge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(66, 17);
            this.lblAge.TabIndex = 2;
            this.lblAge.Text = "Возраст:";
            // 
            // groupPersonalData
            // 
            this.groupPersonalData.Controls.Add(this.lblBirthDateLabel);
            this.groupPersonalData.Controls.Add(this.txtBirthDate);
            this.groupPersonalData.Controls.Add(this.lblAddressLabel);
            this.groupPersonalData.Controls.Add(this.txtAddress);
            this.groupPersonalData.Controls.Add(this.lblEmailLabel);
            this.groupPersonalData.Controls.Add(this.txtEmail);
            this.groupPersonalData.Controls.Add(this.lblPassportLabel);
            this.groupPersonalData.Controls.Add(this.txtPassport);
            this.groupPersonalData.Location = new System.Drawing.Point(16, 221);
            this.groupPersonalData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupPersonalData.Name = "groupPersonalData";
            this.groupPersonalData.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupPersonalData.Size = new System.Drawing.Size(613, 277);
            this.groupPersonalData.TabIndex = 2;
            this.groupPersonalData.TabStop = false;
            this.groupPersonalData.Text = "Персональные данные";
            // 
            // lblBirthDateLabel
            // 
            this.lblBirthDateLabel.AutoSize = true;
            this.lblBirthDateLabel.Location = new System.Drawing.Point(20, 34);
            this.lblBirthDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBirthDateLabel.Name = "lblBirthDateLabel";
            this.lblBirthDateLabel.Size = new System.Drawing.Size(107, 18);
            this.lblBirthDateLabel.TabIndex = 0;
            this.lblBirthDateLabel.Text = "Дата рождения:";
            // 
            // txtBirthDate
            // 
            this.txtBirthDate.Location = new System.Drawing.Point(160, 31);
            this.txtBirthDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBirthDate.Name = "txtBirthDate";
            this.txtBirthDate.Size = new System.Drawing.Size(199, 26);
            this.txtBirthDate.TabIndex = 1;
            // 
            // lblAddressLabel
            // 
            this.lblAddressLabel.AutoSize = true;
            this.lblAddressLabel.Location = new System.Drawing.Point(20, 76);
            this.lblAddressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddressLabel.Name = "lblAddressLabel";
            this.lblAddressLabel.Size = new System.Drawing.Size(130, 18);
            this.lblAddressLabel.TabIndex = 2;
            this.lblAddressLabel.Text = "Адрес проживания:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(160, 72);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(399, 54);
            this.txtAddress.TabIndex = 3;
            // 
            // lblEmailLabel
            // 
            this.lblEmailLabel.AutoSize = true;
            this.lblEmailLabel.Location = new System.Drawing.Point(20, 146);
            this.lblEmailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmailLabel.Name = "lblEmailLabel";
            this.lblEmailLabel.Size = new System.Drawing.Size(44, 18);
            this.lblEmailLabel.TabIndex = 4;
            this.lblEmailLabel.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(160, 141);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(265, 26);
            this.txtEmail.TabIndex = 5;
            // 
            // lblPassportLabel
            // 
            this.lblPassportLabel.AutoSize = true;
            this.lblPassportLabel.Location = new System.Drawing.Point(20, 187);
            this.lblPassportLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassportLabel.Name = "lblPassportLabel";
            this.lblPassportLabel.Size = new System.Drawing.Size(141, 18);
            this.lblPassportLabel.TabIndex = 6;
            this.lblPassportLabel.Text = "Паспортные данные:";
            // 
            // txtPassport
            // 
            this.txtPassport.Location = new System.Drawing.Point(160, 183);
            this.txtPassport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassport.Name = "txtPassport";
            this.txtPassport.Size = new System.Drawing.Size(265, 26);
            this.txtPassport.TabIndex = 7;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(16, 526);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(133, 41);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(157, 526);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(133, 41);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(299, 526);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 41);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(496, 526);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(133, 41);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblAccessLevel
            // 
            this.lblAccessLevel.AutoSize = true;
            this.lblAccessLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Italic);
            this.lblAccessLevel.ForeColor = System.Drawing.Color.Gray;
            this.lblAccessLevel.Location = new System.Drawing.Point(16, 581);
            this.lblAccessLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccessLevel.Name = "lblAccessLevel";
            this.lblAccessLevel.Size = new System.Drawing.Size(94, 13);
            this.lblAccessLevel.TabIndex = 7;
            this.lblAccessLevel.Text = "Уровень доступа";
            // 
            // ClientDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(645, 586);
            this.Controls.Add(this.lblAccessLevel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.groupPersonalData);
            this.Controls.Add(this.groupMainInfo);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Детальная информация о клиенте";
            this.groupMainInfo.ResumeLayout(false);
            this.groupMainInfo.PerformLayout();
            this.groupPersonalData.ResumeLayout(false);
            this.groupPersonalData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}