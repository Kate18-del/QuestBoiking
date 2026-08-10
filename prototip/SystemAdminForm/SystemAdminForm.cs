using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace prototip
{
    public partial class SystemAdminForm : Form
    {
        private MySqlConnection connection;
        private string connectionString;
        private string currentUser;
        private Timer inactivityTimer;
        private int inactivityTimeoutSeconds = 300; // По умолчанию 5 минут (300 сек)
        private DateTime lastActivityTime;
        private string importPath = Path.Combine(Application.StartupPath, "Импорт");
        private string exportPath = Path.Combine(Application.StartupPath, "Экспорт");

        public SystemAdminForm(string login)
        {
            InitializeComponent();
            currentUser = login;
            connectionString = DatabaseConfig.ConnectionString;

            // Настройка элементов на вкладке безопасности
            SetupSecurityTab();

            // Загружаем настройки бездействия из конфига
            LoadInactivitySettings();

            // Загружаем таблицы для импорта и экспорта
            LoadTablesForImport();
            LoadTablesForExport();

            // Загружаем настройки подключения
            LoadConnectionSettings();


            // Показываем приветствие
            lblWelcome.Text = $"Системный администратор";

            // Подписываемся на события
            this.btnSelectFile.Click += btnSelectFile_Click;
            this.btnImportData.Click += btnImportData_Click;
            this.btnTestConnection.Click += btnTestConnection_Click;
            this.btnSaveSettings.Click += btnSaveSettings_Click;
            this.btnManualBackup.Click += BtnManualBackup_Click;
            this.btnClearLog.Click += btnClearLog_Click;
            this.btnExit.Click += btnExit_Click;

            // Подписываемся на события для экспорта
            this.button1.Click += btnExportCSV_Click;

            // Подписываемся на события для восстановления
            this.button2.Click += BtnRestoreBackup_Click;

            // Подписываемся на события для настроек безопасности
            this.button3.Click += btnSaveSecuritySettings_Click;

            // Подписываемся на события отслеживания активности
            this.MouseMove += OnUserActivity;
            this.KeyPress += OnUserActivity;

            // Подписываем все контролы на форме
            foreach (Control control in this.Controls)
            {
                control.MouseMove += OnUserActivity;
                control.KeyPress += OnUserActivity;
            }

            // Также подписываем контролы внутри TabControl
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    control.MouseMove += OnUserActivity;
                    control.KeyPress += OnUserActivity;
                }
            }

            this.FormClosing += SystemAdminForm_FormClosing;

            // Запускаем таймер бездействия
            StartInactivityTimer();

            // Создаем директории для импорта и экспорта
            CreateDirectories();
        }

        /// <summary>
        /// Создание директорий для импорта и экспорта
        /// </summary>
        private void CreateDirectories()
        {
            try
            {
                if (!Directory.Exists(importPath))
                {
                    Directory.CreateDirectory(importPath);
                    LogMessage($"✓ Создана папка для импорта: {importPath}");
                }

                if (!Directory.Exists(exportPath))
                {
                    Directory.CreateDirectory(exportPath);
                    LogMessage($"✓ Создана папка для экспорта: {exportPath}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Ошибка создания папок: {ex.Message}");
            }
        }
        /// <summary>
        /// Настройка элементов управления на вкладке безопасности
        /// </summary>
        private void SetupSecurityTab()
        {
            // Сначала устанавливаем минимальное и максимальное значение
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 60;
            numericUpDown1.Value = 5; // Временно устанавливаем значение по умолчанию

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(numericUpDown1, "Время бездействия в минутах (от 1 до 60)");
            toolTip.SetToolTip(checkBox1, "Включить автоматическую блокировку при бездействии");
        }

        /// <summary>
        /// Загрузка настроек бездействия из конфигурации
        /// </summary>
        private void LoadInactivitySettings()
        {
            try
            {
                // Сначала загружаем строку из конфига
                string timeout = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"];

                if (!string.IsNullOrEmpty(timeout) && int.TryParse(timeout, out int seconds) && seconds > 0)
                {
                    inactivityTimeoutSeconds = seconds;
                    int minutes = seconds / 60;

                    // Проверяем, что значение в допустимом диапазоне
                    if (minutes >= 1 && minutes <= 60)
                    {
                        numericUpDown1.Value = minutes;
                    }
                    else if (minutes < 1)
                    {
                        numericUpDown1.Value = 1;
                        inactivityTimeoutSeconds = 60;
                    }
                    else if (minutes > 60)
                    {
                        numericUpDown1.Value = 60;
                        inactivityTimeoutSeconds = 3600;
                    }
                }
                else
                {
                    // Значение по умолчанию - 5 минут (300 секунд)
                    inactivityTimeoutSeconds = 300;
                    numericUpDown1.Value = 5;
                }

                // Загружаем состояние чекбокса
                string enabled = ConfigurationManager.AppSettings["EnableAutoLock"];
                if (!string.IsNullOrEmpty(enabled) && bool.TryParse(enabled, out bool isEnabled))
                {
                    checkBox1.Checked = isEnabled;
                }
                else
                {
                    checkBox1.Checked = true;
                }

                LogMessage($"ℹ Загружены настройки: таймаут = {numericUpDown1.Value} мин, блокировка = {(checkBox1.Checked ? "ВКЛ" : "ВЫКЛ")}");
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка загрузки настроек бездействия: {ex.Message}");
                // Устанавливаем безопасные значения по умолчанию
                inactivityTimeoutSeconds = 300;
                try
                {
                    numericUpDown1.Value = 5;
                }
                catch
                {
                    // Если не удалось установить значение, игнорируем
                }
                checkBox1.Checked = true;
            }
        }

        /// <summary>
        /// Сохранение настроек безопасности
        /// </summary>
        private void btnSaveSecuritySettings_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем значение из NumericUpDown
                int minutes = (int)numericUpDown1.Value;

                // Проверяем, что минуты в допустимом диапазоне
                if (minutes < 1) minutes = 1;
                if (minutes > 60) minutes = 60;

                int seconds = minutes * 60;
                bool isEnabled = checkBox1.Checked;

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Сохраняем таймаут в секундах
                if (config.AppSettings.Settings["InactivityTimeoutSeconds"] != null)
                {
                    config.AppSettings.Settings["InactivityTimeoutSeconds"].Value = seconds.ToString();
                }
                else
                {
                    config.AppSettings.Settings.Add("InactivityTimeoutSeconds", seconds.ToString());
                }

                // Сохраняем флаг включения
                if (config.AppSettings.Settings["EnableAutoLock"] != null)
                {
                    config.AppSettings.Settings["EnableAutoLock"].Value = isEnabled.ToString().ToLower();
                }
                else
                {
                    config.AppSettings.Settings.Add("EnableAutoLock", isEnabled.ToString().ToLower());
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                // Обновляем переменные для текущей сессии
                inactivityTimeoutSeconds = seconds;

                // Перезапускаем таймер с новыми настройками
                RestartInactivityTimer();

                LogMessage($"✓ Настройки безопасности сохранены: таймаут = {minutes} мин ({seconds} сек), блокировка {(isEnabled ? "ВКЛЮЧЕНА" : "ВЫКЛЮЧЕНА")}");

                MessageBox.Show($"Настройки безопасности сохранены!\n\n" +
                    $"Время бездействия: {minutes} минут\n" +
                    $"Автоматическая блокировка: {(isEnabled ? "Включена" : "Выключена")}",
                    "Успех",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Ошибка сохранения настроек безопасности: {ex.Message}");
                MessageBox.Show($"Ошибка сохранения настроек: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Перезапуск таймера бездействия
        /// </summary>
        private void RestartInactivityTimer()
        {
            if (inactivityTimer != null)
            {
                inactivityTimer.Stop();
                inactivityTimer.Dispose();
            }
            StartInactivityTimer();
        }

        /// <summary>
        /// Запуск таймера отслеживания бездействия
        /// </summary>
        private void StartInactivityTimer()
        {
            lastActivityTime = DateTime.Now;

            if (checkBox1.Checked && inactivityTimeoutSeconds > 0)
            {
                inactivityTimer = new Timer();
                inactivityTimer.Interval = 1000;
                inactivityTimer.Tick += InactivityTimer_Tick;
                inactivityTimer.Start();

                LogMessage($"✓ Отслеживание бездействия запущено (таймаут: {inactivityTimeoutSeconds / 60} мин)");
            }
            else
            {
                LogMessage($"ℹ Отслеживание бездействия отключено");
            }
        }

        /// <summary>
        /// Проверка бездействия пользователя
        /// </summary>
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan inactiveDuration = DateTime.Now - lastActivityTime;

            if (inactiveDuration.TotalSeconds >= inactivityTimeoutSeconds)
            {
                inactivityTimer.Stop();

                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(
                        $"Вы были неактивны более {inactivityTimeoutSeconds / 60} минут.\n\nДля продолжения работы необходимо авторизоваться заново.",
                        "Сессия истекла",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.Close();

                    Autorisation authForm = new Autorisation();
                    authForm.ShowDialog();
                }));
            }
        }

        /// <summary>
        /// Обновление времени последней активности
        /// </summary>
        private void OnUserActivity(object sender, EventArgs e)
        {
            lastActivityTime = DateTime.Now;
        }

        /// <summary>
        /// Загрузка таблиц для импорта
        /// </summary>
        private void LoadTablesForImport()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    DataTable schema = conn.GetSchema("Tables");

                    cmbTables.Items.Clear();
                    foreach (DataRow row in schema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        cmbTables.Items.Add(tableName);
                    }

                    if (cmbTables.Items.Count > 0)
                        cmbTables.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка загрузки списка таблиц для импорта: {ex.Message}");
            }
        }

        /// <summary>
        /// Загрузка таблиц для экспорта
        /// </summary>
        private void LoadTablesForExport()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    DataTable schema = conn.GetSchema("Tables");

                    comboBox1.Items.Clear();
                    foreach (DataRow row in schema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        comboBox1.Items.Add(tableName);
                    }

                    if (comboBox1.Items.Count > 0)
                        comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка загрузки списка таблиц для экспорта: {ex.Message}");
            }
        }

        /// <summary>
        /// Автоматическое резервное копирование при выходе
        /// </summary>
        private void SystemAdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            inactivityTimer?.Stop();
            inactivityTimer?.Dispose();

            string autoBackupEnabled = ConfigurationManager.AppSettings["AutoBackupEnabled"];
            if (autoBackupEnabled?.ToLower() == "true")
            {
                CreateAutoBackup();
            }
        }

        /// <summary>
        /// Создание автоматической резервной копии
        /// </summary>
        private void CreateAutoBackup()
        {
            try
            {
                string folder = Path.Combine(Application.StartupPath, "AutoBackups");
                Directory.CreateDirectory(folder);
                string filePath = Path.Combine(folder, $"auto_backup_{DateTime.Now:yyyyMMdd_HHmm}.sql");
                CreateBackup(filePath);
                CleanOldBackups(folder, 5);
                LogMessage($"✓ Автоматический бэкап создан: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex) { LogMessage($"✗ Ошибка авто-бэкапа: {ex.Message}"); }
        }

        /// <summary>
        /// Очистка старых бэкапов
        /// </summary>
        private void CleanOldBackups(string folder, int keepCount)
        {
            try
            {
                var files = new DirectoryInfo(folder).GetFiles("auto_backup_*.sql")
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(keepCount);

                foreach (var file in files)
                {
                    file.Delete();
                }
            }
            catch { }
        }

        /// <summary>
        /// Выход
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Autorisation autorisation = new Autorisation();
            autorisation.ShowDialog();
            this.Close();   
        }

        /// <summary>
        /// Выполнение SQL-запроса
        /// </summary>
        private void ExecuteNonQuery(MySqlConnection conn, string query)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Выбор CSV файла для импорта
        /// </summary>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Выберите CSV файл для импорта";

                // Устанавливаем начальную директорию на папку "Импорт"
                if (Directory.Exists(importPath))
                {
                    openFileDialog.InitialDirectory = importPath;
                }
                else
                {
                    openFileDialog.InitialDirectory = Application.StartupPath;
                }

                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                    PreviewCSVFile(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Предпросмотр CSV файла
        /// </summary>
        private void PreviewCSVFile(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8).Take(5).ToArray();
                StringBuilder preview = new StringBuilder();
                preview.AppendLine("Предпросмотр файла (первые 5 строк):");
                preview.AppendLine(new string('-', 50));

                for (int i = 0; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        preview.AppendLine($"Строка {i + 1}: {lines[i]}");
                    }
                }

                LogMessage(preview.ToString());
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка предпросмотра файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Импорт данных
        /// </summary>
        private void btnImportData_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу для импорта", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Выберите CSV файл", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = cmbTables.SelectedItem.ToString();
            ImportCSVData(tableName, txtFilePath.Text);
        }

        /// <summary>
        /// Импорт данных из CSV
        /// </summary>
        private void ImportCSVData(string tableName, string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8)
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .ToArray();

                if (lines.Length == 0)
                {
                    MessageBox.Show("Файл пуст", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataTable tableSchema = GetTableSchema(tableName);
                if (tableSchema == null || tableSchema.Columns.Count == 0)
                {
                    MessageBox.Show("Не удалось получить структуру таблицы", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<string> autoIncrementColumns = GetAutoIncrementColumns(tableName);
                char separator = lines[0].Contains(';') ? ';' : ',';
                string[] headers = lines[0].Split(separator);
                int startRow = 0;

                for (int h = 0; h < headers.Length; h++)
                {
                    headers[h] = headers[h].Trim().Trim('"').Trim('\'');
                }

                bool hasHeader = false;
                foreach (string header in headers)
                {
                    if (tableSchema.Columns.Contains(header))
                    {
                        hasHeader = true;
                        break;
                    }
                }

                if (hasHeader)
                {
                    startRow = 1;
                    LogMessage($"✓ Обнаружена строка заголовков, импорт начнется со строки 2");
                }

                List<string> skipColumns = new List<string>();
                if (tableName.ToLower() == "services")
                {
                    skipColumns.Add("Picture");
                    LogMessage("✓ Поле Picture (BLOB) будет пропущено при импорте");
                }

                int expectedColumns = tableSchema.Columns.Count - autoIncrementColumns.Count - skipColumns.Count;
                string[] firstDataRow = lines[startRow].Split(separator);

                if (firstDataRow.Length != expectedColumns)
                {
                    string errorMsg = $"Несоответствие количества полей!\n" +
                                    $"В CSV файле: {firstDataRow.Length} полей\n" +
                                    $"В таблице '{tableName}': {tableSchema.Columns.Count} полей\n" +
                                    $"Автоинкрементные поля: {string.Join(", ", autoIncrementColumns)}\n" +
                                    $"Пропущенные поля: {string.Join(", ", skipColumns)}\n" +
                                    $"Ожидаемое количество полей в CSV: {expectedColumns}";

                    LogMessage($"✗ ОШИБКА: {errorMsg}");
                    MessageBox.Show(errorMsg, "Ошибка импорта", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<int> columnMapping = new List<int>();
                if (hasHeader)
                {
                    for (int i = 0; i < headers.Length; i++)
                    {
                        string header = headers[i];
                        if (tableSchema.Columns.Contains(header) &&
                            !autoIncrementColumns.Contains(header) &&
                            !skipColumns.Contains(header))
                        {
                            columnMapping.Add(tableSchema.Columns[header].Ordinal);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < tableSchema.Columns.Count; i++)
                    {
                        string colName = tableSchema.Columns[i].ColumnName;
                        if (!autoIncrementColumns.Contains(colName) && !skipColumns.Contains(colName))
                        {
                            columnMapping.Add(i);
                        }
                    }
                }

                int importedCount = 0;
                int errorCount = 0;
                List<string> errors = new List<string>();

                string connectionStringWithCharset = connectionString + ";Charset=utf8;";

                using (MySqlConnection conn = new MySqlConnection(connectionStringWithCharset))
                {
                    conn.Open();
                    MySqlCommand setCharsetCmd = new MySqlCommand("SET NAMES 'utf8mb4'", conn);
                    setCharsetCmd.ExecuteNonQuery();

                    if (tableName.ToLower() == "orders")
                    {
                        ExecuteNonQuery(conn, "SET FOREIGN_KEY_CHECKS = 0");
                    }

                    for (int i = startRow; i < lines.Length; i++)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line)) continue;

                        line = line.Replace("\"", "");
                        string[] values = line.Split(separator);

                        try
                        {
                            string query = GenerateInsertQuery(tableName, tableSchema, autoIncrementColumns, skipColumns);
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            int paramIndex = 0;
                            for (int j = 0; j < columnMapping.Count; j++)
                            {
                                int tableColIndex = columnMapping[j];
                                string value = values[j].Trim();
                                Type columnType = tableSchema.Columns[tableColIndex].DataType;

                                if (string.IsNullOrEmpty(value))
                                {
                                    cmd.Parameters.AddWithValue($"@p{paramIndex}", DBNull.Value);
                                }
                                else
                                {
                                    try
                                    {
                                        if (columnType == typeof(int) || columnType == typeof(long))
                                        {
                                            cmd.Parameters.AddWithValue($"@p{paramIndex}", int.Parse(value));
                                        }
                                        else if (columnType == typeof(decimal))
                                        {
                                            value = value.Replace('.', ',');
                                            cmd.Parameters.AddWithValue($"@p{paramIndex}", decimal.Parse(value));
                                        }
                                        else if (columnType == typeof(DateTime))
                                        {
                                            cmd.Parameters.AddWithValue($"@p{paramIndex}", DateTime.Parse(value));
                                        }
                                        else if (columnType == typeof(bool) || columnType == typeof(byte))
                                        {
                                            cmd.Parameters.AddWithValue($"@p{paramIndex}", value == "1" || value.ToLower() == "true" ? 1 : 0);
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add($"@p{paramIndex}", MySqlDbType.VarChar).Value = value;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"Ошибка преобразования '{value}' в {columnType.Name}: {ex.Message}");
                                    }
                                }
                                paramIndex++;
                            }

                            cmd.ExecuteNonQuery();
                            importedCount++;
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errors.Add($"Строка {i + 1}: {ex.Message}");
                        }
                    }

                    if (tableName.ToLower() == "orders")
                    {
                        ExecuteNonQuery(conn, "SET FOREIGN_KEY_CHECKS = 1");
                    }
                }

                // Вывод результатов в лог
                LogMessage($"\n=== РЕЗУЛЬТАТЫ ИМПОРТА ===");
                LogMessage($"Таблица: {tableName}");
                LogMessage($"Файл: {Path.GetFileName(filePath)}");
                LogMessage($"Успешно импортировано: {importedCount} записей");
                LogMessage($"Ошибок: {errorCount}");

                if (errors.Count > 0)
                {
                    LogMessage($"\nПервые 5 ошибок:");
                    foreach (string error in errors.Take(5))
                    {
                        LogMessage($"  • {error}");
                    }
                }

                // Показываем результат пользователю
                MessageBox.Show($"Импорт завершен!\n\n" +
                               $"Таблица: {tableName}\n" +
                               $"Файл: {Path.GetFileName(filePath)}\n\n" +
                               $"✓ Успешно: {importedCount} записей\n",
                    "Результат импорта",
                    MessageBoxButtons.OK,
                    errorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"✗ КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
                MessageBox.Show($"Ошибка при импорте данных:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Получение схемы таблицы
        /// </summary>
        private DataTable GetTableSchema(string tableName)
        {
            try
            {
                string connectionStringWithCharset = connectionString + ";Charset=utf8;";
                using (MySqlConnection conn = new MySqlConnection(connectionStringWithCharset))
                {
                    conn.Open();
                    MySqlCommand setCharsetCmd = new MySqlCommand("SET NAMES 'utf8mb4'", conn);
                    setCharsetCmd.ExecuteNonQuery();

                    string query = $"SELECT * FROM `{tableName}` LIMIT 0";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable schema = new DataTable();
                    adapter.Fill(schema);
                    return schema;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка получения схемы таблицы: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Генерация INSERT запроса
        /// </summary>
        private string GenerateInsertQuery(string tableName, DataTable schema, List<string> autoIncrementColumns, List<string> skipColumns = null)
        {
            var columns = schema.Columns.Cast<DataColumn>()
                .Where(c => !autoIncrementColumns.Contains(c.ColumnName))
                .Where(c => skipColumns == null || !skipColumns.Contains(c.ColumnName))
                .Select(c => $"`{c.ColumnName}`")
                .ToList();

            string columnsStr = string.Join(", ", columns);
            string parameters = string.Join(", ", Enumerable.Range(0, columns.Count).Select(i => $"@p{i}"));

            return $"INSERT INTO `{tableName}` ({columnsStr}) VALUES ({parameters})";
        }

        /// <summary>
        /// Логирование
        /// </summary>
        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action<string>(LogMessage), message);
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
                txtLog.ScrollToCaret();
            }
        }

        /// <summary>
        /// Очистка лога
        /// </summary>
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            LogMessage("Журнал очищен");
        }

        /// <summary>
        /// Получение автоинкрементных полей
        /// </summary>
        private List<string> GetAutoIncrementColumns(string tableName)
        {
            List<string> autoIncrementCols = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT COLUMN_NAME 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_SCHEMA = DATABASE() 
                AND TABLE_NAME = @tableName 
                AND EXTRA LIKE '%auto_increment%'";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tableName", tableName);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            autoIncrementCols.Add(reader["COLUMN_NAME"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Ошибка получения автоинкрементных полей: {ex.Message}");
            }

            return autoIncrementCols;
        }

        /// <summary>
        /// Проверка подключения
        /// </summary>
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            // Проверяем заполненность полей
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                lblConnectionStatus.Text = "⚠ Укажите сервер!";
                lblConnectionStatus.ForeColor = Color.Orange;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDatabase.Text))
            {
                lblConnectionStatus.Text = "⚠ Укажите базу данных!";
                lblConnectionStatus.ForeColor = Color.Orange;
                return;
            }

            lblConnectionStatus.Text = "Проверка подключения...";
            lblConnectionStatus.ForeColor = Color.Black;
            Application.DoEvents();

            string testConnectionString = $"server={txtServer.Text};database={txtDatabase.Text};uid={txtUsername.Text};pwd={txtPassword.Text};charset=utf8mb4;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(testConnectionString))
                {
                    conn.Open();

                    // Получаем версию сервера
                    string serverVersion = conn.ServerVersion;

                    lblConnectionStatus.Text = "✓ Подключение успешно!";
                    lblConnectionStatus.ForeColor = Color.Green;
                    LogMessage($"✓ Проверка подключения: успешно (MySQL версия: {serverVersion})");
                }
            }
            catch (MySqlException ex)
            {
                string errorMessage = ex.Message;

                // Даем более понятные сообщения для типичных ошибок
                if (ex.Number == 0)
                {
                    errorMessage = "Не удалось подключиться к серверу. Проверьте адрес сервера и порт.";
                }
                else if (ex.Number == 1045)
                {
                    errorMessage = "Неверное имя пользователя или пароль.";
                }
                else if (ex.Number == 1049)
                {
                    errorMessage = "Указанная база данных не существует.";
                }

                lblConnectionStatus.Text = $"✗ Ошибка подключения";
                lblConnectionStatus.ForeColor = Color.Red;
                LogMessage($"✗ Проверка подключения: {errorMessage}");
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = $"✗ Ошибка: {ex.Message}";
                lblConnectionStatus.ForeColor = Color.Red;
                LogMessage($"✗ Проверка подключения: {ex.Message}");
            }
        }

        /// <summary>
        /// Сохранение настроек подключения
        /// </summary>
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что поля не пустые
                if (string.IsNullOrWhiteSpace(txtServer.Text))
                {
                    MessageBox.Show("Укажите сервер!", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDatabase.Text))
                {
                    MessageBox.Show("Укажите базу данных!", "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Сначала проверяем подключение
                lblConnectionStatus.Text = "Проверка подключения...";
                lblConnectionStatus.ForeColor = Color.Black;
                Application.DoEvents();

                string testConnectionString = $"server={txtServer.Text};database={txtDatabase.Text};uid={txtUsername.Text};pwd={txtPassword.Text};charset=utf8mb4;";

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(testConnectionString))
                    {
                        conn.Open();

                        // Если подключение успешно - сохраняем настройки
                        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                        UpdateOrAddSetting(config, "Server", txtServer.Text);
                        UpdateOrAddSetting(config, "Database", txtDatabase.Text);
                        UpdateOrAddSetting(config, "Username", txtUsername.Text);
                        UpdateOrAddSetting(config, "Password", txtPassword.Text);

                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                        // Обновляем строку подключения
                        DatabaseConfig.UpdateConnectionString(txtServer.Text, txtDatabase.Text, txtUsername.Text, txtPassword.Text);
                        connectionString = DatabaseConfig.ConnectionString;

                        lblConnectionStatus.Text = "✓ Настройки сохранены. Подключение успешно!";
                        lblConnectionStatus.ForeColor = Color.Green;
                        LogMessage("✓ Настройки подключения сохранены. Подключение успешно!");

                        // Показываем сообщение и перезапускаем приложение
                        DialogResult result = MessageBox.Show(
                            "Настройки успешно сохранены!\nПодключение к базе данных работает корректно.\n\nДля применения настроек приложение будет перезапущено.",
                            "Успех",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.OK)
                        {
                            RestartApplication();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    string errorMessage = ex.Message;

                    // Даем более понятные сообщения для типичных ошибок
                    if (ex.Number == 0)
                    {
                        errorMessage = "Не удалось подключиться к серверу. Проверьте адрес сервера и порт.";
                    }
                    else if (ex.Number == 1045)
                    {
                        errorMessage = "Неверное имя пользователя или пароль.";
                    }
                    else if (ex.Number == 1049)
                    {
                        errorMessage = "Указанная база данных не существует.";
                    }

                    lblConnectionStatus.Text = "✗ Ошибка подключения. Настройки НЕ сохранены!";
                    lblConnectionStatus.ForeColor = Color.Red;
                    LogMessage($"✗ Ошибка подключения: {errorMessage}. Настройки НЕ сохранены!");

                    MessageBox.Show($"Не удалось подключиться к базе данных!\n\n" +
                                  $"Ошибка: {errorMessage}\n\n" +
                                  $"Настройки НЕ сохранены. Исправьте ошибку и попробуйте снова.",
                        "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    lblConnectionStatus.Text = "✗ Ошибка. Настройки НЕ сохранены!";
                    lblConnectionStatus.ForeColor = Color.Red;
                    LogMessage($"✗ Ошибка: {ex.Message}. Настройки НЕ сохранены!");

                    MessageBox.Show($"Неожиданная ошибка:\n{ex.Message}\n\nНастройки НЕ сохранены.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = "✗ Критическая ошибка";
                lblConnectionStatus.ForeColor = Color.Red;
                LogMessage($"✗ Критическая ошибка сохранения настроек: {ex.Message}");

                MessageBox.Show($"Критическая ошибка сохранения настроек: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Перезапуск приложения
        /// </summary>
        private void RestartApplication()
        {
            try
            {
                // Останавливаем таймер
                if (inactivityTimer != null)
                {
                    inactivityTimer.Stop();
                    inactivityTimer.Dispose();
                }

                // Запускаем новый экземпляр приложения
                System.Diagnostics.Process.Start(Application.ExecutablePath);

                // Закрываем текущий экземпляр
                Application.Exit();
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Ошибка перезапуска приложения: {ex.Message}");
                MessageBox.Show($"Ошибка перезапуска приложения: {ex.Message}\n\nПожалуйста, перезапустите приложение вручную.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateOrAddSetting(Configuration config, string key, string value)
        {
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }
        }
        /// <summary>
        /// Загрузка настроек подключения
        /// </summary>
        private void LoadConnectionSettings()
        {
            txtServer.Text = ConfigurationManager.AppSettings["Server"] ?? "localhost";
            txtDatabase.Text = ConfigurationManager.AppSettings["Database"] ?? "db99";
            txtUsername.Text = ConfigurationManager.AppSettings["Username"] ?? "root";
            txtPassword.Text = ConfigurationManager.AppSettings["Password"] ?? "";
        }

        /// <summary>
        /// Экспорт в CSV
        /// </summary>
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу для экспорта", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = comboBox1.SelectedItem.ToString();

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.FileName = $"{tableName}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
                saveFileDialog.Title = "Сохранить CSV файл";

                // Устанавливаем начальную директорию на папку "Экспорт"
                if (Directory.Exists(exportPath))
                {
                    saveFileDialog.InitialDirectory = exportPath;
                }
                else
                {
                    saveFileDialog.InitialDirectory = Application.StartupPath;
                }

                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(tableName, saveFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Экспорт таблицы в CSV
        /// </summary>
        private void ExportToCSV(string tableName, string filePath)
        {
            try
            {
                LogMessage($"Начало экспорта таблицы {tableName}...");

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    List<string> autoIncrementCols = GetAutoIncrementColumns(tableName);

                    List<string> skipColumns = new List<string>();
                    skipColumns.AddRange(autoIncrementCols);

                    if (tableName.ToLower() == "services")
                    {
                        skipColumns.Add("Picture");
                    }

                    DataTable schema = GetTableSchema(tableName);
                    List<string> exportColumns = new List<string>();
                    foreach (DataColumn col in schema.Columns)
                    {
                        if (!skipColumns.Contains(col.ColumnName))
                        {
                            exportColumns.Add(col.ColumnName);
                        }
                    }

                    string columnsStr = string.Join(", ", exportColumns.Select(c => $"`{c}`"));
                    string query = $"SELECT {columnsStr} FROM `{tableName}`";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.BaseStream.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                            writer.WriteLine(string.Join(";", exportColumns));

                            int rowCount = 0;
                            while (reader.Read())
                            {
                                List<string> row = new List<string>();
                                for (int i = 0; i < exportColumns.Count; i++)
                                {
                                    string value = "";
                                    if (!reader.IsDBNull(i))
                                    {
                                        object val = reader.GetValue(i);
                                        if (val is DateTime dt)
                                        {
                                            value = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                        }
                                        else if (val is decimal dec)
                                        {
                                            value = dec.ToString("F2").Replace('.', ',');
                                        }
                                        else
                                        {
                                            value = val.ToString();
                                        }
                                    }
                                    row.Add(EscapeCsvField(value));
                                }
                                writer.WriteLine(string.Join(";", row));
                                rowCount++;
                            }

                            LogMessage($"✓ Экспорт завершен: {rowCount} записей");
                            LogMessage($"✓ Файл сохранен: {filePath}");
                        }
                    }
                }

                MessageBox.Show($"Экспорт успешно завершен!\n\nФайл: {filePath}",
                    "Экспорт завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"✗ ОШИБКА ЭКСПОРТА: {ex.Message}");
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экранирование CSV поля
        /// </summary>
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                field = field.Replace("\"", "\"\"");
                field = $"\"{field}\"";
            }

            return field;
        }

        /// <summary>
        /// Ручное создание бэкапа
        /// </summary>
        private void BtnManualBackup_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "SQL Backup (*.sql)|*.sql";
                saveFileDialog.FileName = $"backup_{DateTime.Now:yyyyMMdd_HHmm}.sql";
                saveFileDialog.Title = "Сохранить резервную копию";

                // Создаем папку если её нет
                string backupPath = Path.Combine(Application.StartupPath, "Резервные копии");
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }
                saveFileDialog.InitialDirectory = backupPath;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    CreateBackup(saveFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Создание резервной копии
        /// </summary>
        private void CreateBackup(string filePath)
        {
            try
            {
                
                Application.DoEvents();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    DataTable tables = conn.GetSchema("Tables");
                    List<string> tableNames = new List<string>();
                    foreach (DataRow row in tables.Rows)
                    {
                        tableNames.Add(row["TABLE_NAME"].ToString());
                    }

                    using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        writer.BaseStream.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);

                        writer.WriteLine("/*!40101 SET NAMES utf8mb4 */;");
                        writer.WriteLine("/*!40101 SET FOREIGN_KEY_CHECKS=0 */;");
                        writer.WriteLine();
                        writer.WriteLine($"-- Backup created: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                        writer.WriteLine();

                        foreach (string tableName in tableNames)
                        {
                            writer.WriteLine($"-- Table structure for `{tableName}`");
                            writer.WriteLine();

                            string showCreate = $"SHOW CREATE TABLE `{tableName}`";
                            MySqlCommand cmdCreate = new MySqlCommand(showCreate, conn);
                            using (MySqlDataReader reader = cmdCreate.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    writer.WriteLine($"DROP TABLE IF EXISTS `{tableName}`;");
                                    writer.WriteLine(reader.GetString(1) + ";");
                                    writer.WriteLine();
                                }
                            }

                            DataTable schema = GetTableSchema(tableName);
                            List<string> columns = new List<string>();

                            foreach (DataColumn col in schema.Columns)
                            {
                                if (col.DataType != typeof(byte[]))
                                {
                                    columns.Add(col.ColumnName);
                                }
                            }

                            if (columns.Count > 0)
                            {
                                string colsStr = string.Join("`, `", columns);
                                string query = $"SELECT `{colsStr}` FROM `{tableName}`";

                                MySqlCommand cmdData = new MySqlCommand(query, conn);
                                using (MySqlDataReader reader = cmdData.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        writer.WriteLine($"-- Data for `{tableName}`");

                                        while (reader.Read())
                                        {
                                            List<string> values = new List<string>();
                                            foreach (string col in columns)
                                            {
                                                int ordinal = reader.GetOrdinal(col);
                                                if (reader.IsDBNull(ordinal))
                                                {
                                                    values.Add("NULL");
                                                }
                                                else
                                                {
                                                    string val = reader.GetValue(ordinal).ToString();
                                                    val = val.Replace("\\", "\\\\").Replace("'", "\\'");
                                                    values.Add($"'{val}'");
                                                }
                                            }

                                            string valsStr = string.Join(", ", values);
                                            writer.WriteLine($"INSERT INTO `{tableName}` (`{string.Join("`, `", columns)}`) VALUES ({valsStr});");
                                        }
                                        writer.WriteLine();
                                    }
                                }
                            }
                        }

                        writer.WriteLine("/*!40101 SET FOREIGN_KEY_CHECKS=1 */;");
                    }
                }

                FileInfo fi = new FileInfo(filePath);
              
                LogMessage($"✓ Резервная копия создана: {filePath}");
             
            }
            catch (Exception ex)
            {
               
                LogMessage($"✗ Ошибка создания бэкапа: {ex.Message}");
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Восстановление из бэкапа
        /// </summary>
        private void BtnRestoreBackup_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Восстановление из резервной копии УДАЛИТ все текущие данные!\n\nПродолжить?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SQL Backup (*.sql)|*.sql|All files (*.*)|*.*";
                openFileDialog.Title = "Выберите файл резервной копии";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    RestoreBackup(openFileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Восстановление БД
        /// </summary>
        private void RestoreBackup(string filePath)
        {
            try
            {
               
                Application.DoEvents();

                string sql = File.ReadAllText(filePath, Encoding.UTF8);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    ExecuteNonQuery(conn, "SET FOREIGN_KEY_CHECKS = 0");

                    string[] queries = sql.Split(new[] { ";\n", ";\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string query in queries)
                    {
                        string trimmedQuery = query.Trim();
                        if (!string.IsNullOrEmpty(trimmedQuery) &&
                            !trimmedQuery.StartsWith("--") &&
                            !trimmedQuery.StartsWith("/*"))
                        {
                            try
                            {
                                ExecuteNonQuery(conn, trimmedQuery);
                            }
                            catch (Exception ex)
                            {
                                LogMessage($"⚠ Предупреждение: {ex.Message}");
                            }
                        }
                    }

                    ExecuteNonQuery(conn, "SET FOREIGN_KEY_CHECKS = 1");
                }

              

                LogMessage($"✓ БД восстановлена из: {filePath}");
                MessageBox.Show("База данных успешно восстановлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
              
                LogMessage($"✗ Ошибка восстановления: {ex.Message}");
                MessageBox.Show($"Ошибка восстановления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}