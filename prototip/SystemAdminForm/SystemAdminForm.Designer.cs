using System.Drawing;
using System;
using System.Windows.Forms;

namespace prototip
{
    partial class SystemAdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblWelcome;
        private TabControl tabControl1;
        private TabPage tabImport;
        private Button btnImportData;
        private Button btnSelectFile;
        private TextBox txtFilePath;
        private Label lblFile;
        private ComboBox cmbTables;
        private Label lblTable;
        private TabPage tabSettings;
        private TextBox txtServer;
        private TextBox txtDatabase;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnSaveSettings;
        private Button btnTestConnection;
        private Label lblConnectionStatus;
        private Label lblServer;
        private Label lblDatabase;
        private Label lblUsername;
        private Label lblPassword;
        private Button btnExit;
        private TextBox txtLog;
        private Label lblLog;
        private Button btnClearLog;
        private Button btnSelectBackupFile;
        private TextBox txtBackupPath;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemAdminForm));
            this.lblWelcome = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tabImport = new System.Windows.Forms.TabPage();
            this.btnImportData = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.tabExport = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabRec = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.TabTime = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.tabBackup = new System.Windows.Forms.TabPage();
            this.btnManualBackup = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabImport.SuspendLayout();
            this.tabExport.SuspendLayout();
            this.tabRec.SuspendLayout();
            this.TabTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabBackup.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblWelcome.Location = new System.Drawing.Point(16, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(267, 27);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Системный администратор";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabImport);
            this.tabControl1.Controls.Add(this.tabExport);
            this.tabControl1.Controls.Add(this.tabBackup);
            this.tabControl1.Controls.Add(this.tabRec);
            this.tabControl1.Controls.Add(this.TabTime);
            this.tabControl1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(17, 44);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(808, 348);
            this.tabControl1.TabIndex = 2;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.btnTestConnection);
            this.tabSettings.Controls.Add(this.lblConnectionStatus);
            this.tabSettings.Controls.Add(this.btnSaveSettings);
            this.tabSettings.Controls.Add(this.txtPassword);
            this.tabSettings.Controls.Add(this.txtUsername);
            this.tabSettings.Controls.Add(this.txtDatabase);
            this.tabSettings.Controls.Add(this.txtServer);
            this.tabSettings.Controls.Add(this.lblServer);
            this.tabSettings.Controls.Add(this.lblDatabase);
            this.tabSettings.Controls.Add(this.lblUsername);
            this.tabSettings.Controls.Add(this.lblPassword);
            this.tabSettings.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabSettings.Location = new System.Drawing.Point(4, 32);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(800, 312);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Text = "Настройки БД";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTestConnection.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnTestConnection.Location = new System.Drawing.Point(12, 265);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(250, 35);
            this.btnTestConnection.TabIndex = 0;
            this.btnTestConnection.Text = "Проверить подключение";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConnectionStatus.Location = new System.Drawing.Point(502, 21);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(261, 35);
            this.lblConnectionStatus.TabIndex = 1;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveSettings.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveSettings.Location = new System.Drawing.Point(532, 265);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(250, 35);
            this.btnSaveSettings.TabIndex = 2;
            this.btnSaveSettings.Text = "Сохранить настройки";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPassword.Location = new System.Drawing.Point(226, 167);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 34);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtUsername.Location = new System.Drawing.Point(226, 121);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 34);
            this.txtUsername.TabIndex = 4;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDatabase.Location = new System.Drawing.Point(226, 72);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(250, 34);
            this.txtDatabase.TabIndex = 5;
            // 
            // txtServer
            // 
            this.txtServer.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtServer.Location = new System.Drawing.Point(226, 27);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(250, 34);
            this.txtServer.TabIndex = 6;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblServer.Location = new System.Drawing.Point(40, 30);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(148, 26);
            this.lblServer.TabIndex = 7;
            this.lblServer.Text = "Адрес сервера:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDatabase.Location = new System.Drawing.Point(40, 75);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(180, 26);
            this.lblDatabase.TabIndex = 8;
            this.lblDatabase.Text = "Имя базы данных:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUsername.Location = new System.Drawing.Point(40, 124);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(186, 26);
            this.lblUsername.TabIndex = 9;
            this.lblUsername.Text = "Имя пользователя:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPassword.Location = new System.Drawing.Point(40, 175);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(87, 26);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Пароль:";
            // 
            // tabImport
            // 
            this.tabImport.Controls.Add(this.btnImportData);
            this.tabImport.Controls.Add(this.btnSelectFile);
            this.tabImport.Controls.Add(this.txtFilePath);
            this.tabImport.Controls.Add(this.lblFile);
            this.tabImport.Controls.Add(this.cmbTables);
            this.tabImport.Controls.Add(this.lblTable);
            this.tabImport.Location = new System.Drawing.Point(4, 32);
            this.tabImport.Name = "tabImport";
            this.tabImport.Size = new System.Drawing.Size(800, 312);
            this.tabImport.TabIndex = 1;
            this.tabImport.Text = "Импорт";
            this.tabImport.UseVisualStyleBackColor = true;
            // 
            // btnImportData
            // 
            this.btnImportData.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnImportData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnImportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnImportData.Location = new System.Drawing.Point(226, 135);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(250, 37);
            this.btnImportData.TabIndex = 5;
            this.btnImportData.Text = "Импортировать";
            this.btnImportData.UseVisualStyleBackColor = false;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSelectFile.Location = new System.Drawing.Point(503, 74);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(35, 27);
            this.btnSelectFile.TabIndex = 4;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFilePath.Location = new System.Drawing.Point(226, 72);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(250, 29);
            this.txtFilePath.TabIndex = 3;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFile.Location = new System.Drawing.Point(40, 75);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(103, 24);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "CSV файл:";
            // 
            // cmbTables
            // 
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(226, 27);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(250, 32);
            this.cmbTables.TabIndex = 1;
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTable.Location = new System.Drawing.Point(40, 30);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(90, 24);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Таблица:";
            // 
            // tabExport
            // 
            this.tabExport.Controls.Add(this.comboBox1);
            this.tabExport.Controls.Add(this.label1);
            this.tabExport.Controls.Add(this.button1);
            this.tabExport.Location = new System.Drawing.Point(4, 32);
            this.tabExport.Name = "tabExport";
            this.tabExport.Size = new System.Drawing.Size(800, 312);
            this.tabExport.TabIndex = 4;
            this.tabExport.Text = "Экспорт";
            this.tabExport.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(226, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(250, 34);
            this.comboBox1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(40, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Таблица:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(226, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(250, 37);
            this.button1.TabIndex = 9;
            this.button1.Text = "Экспорт в csv";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // tabRec
            // 
            this.tabRec.Controls.Add(this.label5);
            this.tabRec.Controls.Add(this.button2);
            this.tabRec.Location = new System.Drawing.Point(4, 32);
            this.tabRec.Name = "tabRec";
            this.tabRec.Size = new System.Drawing.Size(800, 312);
            this.tabRec.TabIndex = 5;
            this.tabRec.Text = "Восстановление";
            this.tabRec.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(276, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(238, 35);
            this.button2.TabIndex = 2;
            this.button2.Text = "Восстановить из копии";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // TabTime
            // 
            this.TabTime.Controls.Add(this.button3);
            this.TabTime.Controls.Add(this.checkBox1);
            this.TabTime.Controls.Add(this.label3);
            this.TabTime.Controls.Add(this.numericUpDown1);
            this.TabTime.Controls.Add(this.label2);
            this.TabTime.Location = new System.Drawing.Point(4, 32);
            this.TabTime.Name = "TabTime";
            this.TabTime.Size = new System.Drawing.Size(800, 312);
            this.TabTime.TabIndex = 6;
            this.TabTime.Text = "Настройки безопасности";
            this.TabTime.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(12, 265);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(250, 37);
            this.button3.TabIndex = 5;
            this.button3.Text = "Сохранить";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(40, 75);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(61, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Включить автоматическую блокировку";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericUpDown1.Location = new System.Drawing.Point(328, 27);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(69, 34);
            this.numericUpDown1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(40, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Время бездействия (минут) :";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExit.Location = new System.Drawing.Point(716, 562);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(105, 35);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(21, 425);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(804, 125);
            this.txtLog.TabIndex = 5;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLog.Location = new System.Drawing.Point(16, 395);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(190, 27);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "Журнал операций:";
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearLog.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClearLog.Location = new System.Drawing.Point(21, 562);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(185, 35);
            this.btnClearLog.TabIndex = 6;
            this.btnClearLog.Text = "Очистить журнал";
            this.btnClearLog.UseVisualStyleBackColor = false;
            // 
            // tabBackup
            // 
            this.tabBackup.Controls.Add(this.label4);
            this.tabBackup.Controls.Add(this.btnManualBackup);
            this.tabBackup.Location = new System.Drawing.Point(4, 32);
            this.tabBackup.Name = "tabBackup";
            this.tabBackup.Size = new System.Drawing.Size(800, 312);
            this.tabBackup.TabIndex = 3;
            this.tabBackup.Text = "Резервное копирование";
            this.tabBackup.UseVisualStyleBackColor = true;
            this.tabBackup.Click += new System.EventHandler(this.BtnManualBackup_Click);
            // 
            // btnManualBackup
            // 
            this.btnManualBackup.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnManualBackup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnManualBackup.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnManualBackup.Location = new System.Drawing.Point(346, 26);
            this.btnManualBackup.Name = "btnManualBackup";
            this.btnManualBackup.Size = new System.Drawing.Size(263, 37);
            this.btnManualBackup.TabIndex = 0;
            this.btnManualBackup.Text = "Создать резервную копию";
            this.btnManualBackup.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(40, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(300, 26);
            this.label4.TabIndex = 8;
            this.label4.Text = "Ручное резервное копирование:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(40, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(230, 26);
            this.label5.TabIndex = 9;
            this.label5.Text = "Восстановить из копии:";
            // 
            // SystemAdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(837, 609);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblWelcome);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemAdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Системный администратор - Управление БД";
            this.tabControl1.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.tabImport.ResumeLayout(false);
            this.tabImport.PerformLayout();
            this.tabExport.ResumeLayout(false);
            this.tabExport.PerformLayout();
            this.tabRec.ResumeLayout(false);
            this.tabRec.PerformLayout();
            this.TabTime.ResumeLayout(false);
            this.TabTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabBackup.ResumeLayout(false);
            this.tabBackup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private TabPage tabExport;
        private ComboBox comboBox1;
        private Label label1;
        private Button button1;
        private TabPage tabRec;
        private Button button2;
        private TabPage TabTime;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private Label label3;
        private CheckBox checkBox1;
        private Button button3;
        private TabPage tabBackup;
        private Label label4;
        private Button btnManualBackup;
        private Label label5;
    }
}