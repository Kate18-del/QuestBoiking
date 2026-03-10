namespace prototip
{
    partial class SystemAdminForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.GroupBox gbOperation;
        private System.Windows.Forms.RadioButton rbRestore;
        private System.Windows.Forms.RadioButton rbImport;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRestore;
        private System.Windows.Forms.TabPage tabImport;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ComboBox cmbTables;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Panel panelRestore;

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
            this.gbOperation = new System.Windows.Forms.GroupBox();
            this.rbRestore = new System.Windows.Forms.RadioButton();
            this.rbImport = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRestore = new System.Windows.Forms.TabPage();
            this.panelRestore = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.btnRestore = new System.Windows.Forms.Button();
            this.tabImport = new System.Windows.Forms.TabPage();
            this.btnImportData = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.gbOperation.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabRestore.SuspendLayout();
            this.panelRestore.SuspendLayout();
            this.tabImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(17, 18);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(242, 20);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Системный администратор";
            // 
            // gbOperation
            // 
            this.gbOperation.Controls.Add(this.rbRestore);
            this.gbOperation.Controls.Add(this.rbImport);
            this.gbOperation.Location = new System.Drawing.Point(17, 57);
            this.gbOperation.Margin = new System.Windows.Forms.Padding(2);
            this.gbOperation.Name = "gbOperation";
            this.gbOperation.Padding = new System.Windows.Forms.Padding(2);
            this.gbOperation.Size = new System.Drawing.Size(752, 66);
            this.gbOperation.TabIndex = 1;
            this.gbOperation.TabStop = false;
            this.gbOperation.Text = "Выберите операцию";
            // 
            // rbRestore
            // 
            this.rbRestore.AutoSize = true;
            this.rbRestore.Location = new System.Drawing.Point(26, 28);
            this.rbRestore.Margin = new System.Windows.Forms.Padding(2);
            this.rbRestore.Name = "rbRestore";
            this.rbRestore.Size = new System.Drawing.Size(243, 19);
            this.rbRestore.TabIndex = 0;
            this.rbRestore.Text = "Восстановление структуры базы данных";
            this.rbRestore.UseVisualStyleBackColor = true;
            this.rbRestore.CheckedChanged += new System.EventHandler(this.rbRestore_CheckedChanged);
            // 
            // rbImport
            // 
            this.rbImport.AutoSize = true;
            this.rbImport.Location = new System.Drawing.Point(306, 28);
            this.rbImport.Margin = new System.Windows.Forms.Padding(2);
            this.rbImport.Name = "rbImport";
            this.rbImport.Size = new System.Drawing.Size(150, 19);
            this.rbImport.TabIndex = 1;
            this.rbImport.Text = "Импорт данных из CSV";
            this.rbImport.UseVisualStyleBackColor = true;
            this.rbImport.CheckedChanged += new System.EventHandler(this.rbImport_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRestore);
            this.tabControl1.Controls.Add(this.tabImport);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(17, 132);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(752, 282);
            this.tabControl1.TabIndex = 2;
            // 
            // tabRestore
            // 
            this.tabRestore.Controls.Add(this.panelRestore);
            this.tabRestore.Location = new System.Drawing.Point(4, 24);
            this.tabRestore.Margin = new System.Windows.Forms.Padding(2);
            this.tabRestore.Name = "tabRestore";
            this.tabRestore.Padding = new System.Windows.Forms.Padding(2);
            this.tabRestore.Size = new System.Drawing.Size(744, 254);
            this.tabRestore.TabIndex = 0;
            this.tabRestore.Text = "Восстановление";
            this.tabRestore.UseVisualStyleBackColor = true;
            // 
            // panelRestore
            // 
            this.panelRestore.Controls.Add(this.lblWarning);
            this.panelRestore.Controls.Add(this.btnRestore);
            this.panelRestore.Location = new System.Drawing.Point(17, 18);
            this.panelRestore.Margin = new System.Windows.Forms.Padding(2);
            this.panelRestore.Name = "panelRestore";
            this.panelRestore.Size = new System.Drawing.Size(700, 207);
            this.panelRestore.TabIndex = 0;
            // 
            // lblWarning
            // 
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(44, 28);
            this.lblWarning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(612, 75);
            this.lblWarning.TabIndex = 1;
            this.lblWarning.Text = "ВНИМАНИЕ! Эта операция полностью удалит все существующие таблицы и данные!\n\nБудут" +
    " созданы новые таблицы с правильной структурой и добавлены начальные справочники" +
    ".";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestore.Location = new System.Drawing.Point(219, 132);
            this.btnRestore.Margin = new System.Windows.Forms.Padding(2);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(262, 47);
            this.btnRestore.TabIndex = 0;
            this.btnRestore.Text = "ВОССТАНОВИТЬ СТРУКТУРУ БД";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // tabImport
            // 
            this.tabImport.Controls.Add(this.btnImportData);
            this.tabImport.Controls.Add(this.btnSelectFile);
            this.tabImport.Controls.Add(this.txtFilePath);
            this.tabImport.Controls.Add(this.lblFile);
            this.tabImport.Controls.Add(this.cmbTables);
            this.tabImport.Controls.Add(this.lblTable);
            this.tabImport.Location = new System.Drawing.Point(4, 24);
            this.tabImport.Margin = new System.Windows.Forms.Padding(2);
            this.tabImport.Name = "tabImport";
            this.tabImport.Padding = new System.Windows.Forms.Padding(2);
            this.tabImport.Size = new System.Drawing.Size(744, 254);
            this.tabImport.TabIndex = 1;
            this.tabImport.Text = "Импорт";
            this.tabImport.UseVisualStyleBackColor = true;
            // 
            // btnImportData
            // 
            this.btnImportData.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnImportData.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnImportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnImportData.Location = new System.Drawing.Point(219, 150);
            this.btnImportData.Margin = new System.Windows.Forms.Padding(2);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(175, 37);
            this.btnImportData.TabIndex = 5;
            this.btnImportData.Text = "ИМПОРТИРОВАТЬ";
            this.btnImportData.UseVisualStyleBackColor = false;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnSelectFile.Location = new System.Drawing.Point(534, 89);
            this.btnSelectFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(35, 27);
            this.btnSelectFile.TabIndex = 4;
            this.btnSelectFile.Text = "...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtFilePath.Location = new System.Drawing.Point(219, 91);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(307, 21);
            this.txtFilePath.TabIndex = 3;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblFile.Location = new System.Drawing.Point(131, 93);
            this.lblFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(68, 15);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "CSV файл:";
            // 
            // cmbTables
            // 
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.cmbTables.FormattingEnabled = true;
            this.cmbTables.Location = new System.Drawing.Point(219, 44);
            this.cmbTables.Margin = new System.Windows.Forms.Padding(2);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(219, 23);
            this.cmbTables.TabIndex = 1;
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblTable.Location = new System.Drawing.Point(131, 47);
            this.lblTable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(59, 15);
            this.lblTable.TabIndex = 0;
            this.lblTable.Text = "Таблица:";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnExit.Location = new System.Drawing.Point(665, 563);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(105, 28);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.Location = new System.Drawing.Point(17, 459);
            this.txtLog.Margin = new System.Windows.Forms.Padding(2);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(753, 94);
            this.txtLog.TabIndex = 5;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblLog.Location = new System.Drawing.Point(17, 432);
            this.lblLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(129, 15);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "Журнал операций:";
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearLog.Location = new System.Drawing.Point(665, 432);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(105, 23);
            this.btnClearLog.TabIndex = 6;
            this.btnClearLog.Text = "Очистить журнал";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // SystemAdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(787, 609);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gbOperation);
            this.Controls.Add(this.lblWelcome);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemAdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Системный администратор - Управление БД";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.btnExit_Click);
            this.gbOperation.ResumeLayout(false);
            this.gbOperation.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabRestore.ResumeLayout(false);
            this.panelRestore.ResumeLayout(false);
            this.tabImport.ResumeLayout(false);
            this.tabImport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}