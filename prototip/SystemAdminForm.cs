using System;
using System.Collections.Generic;
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

        public SystemAdminForm(string login)
        {
            InitializeComponent();
            currentUser = login;
            connectionString = DatabaseConfig.ConnectionString;

            // Отключаем вкладки, пока не выбран тип операции
            tabControl1.Enabled = false;

            // Показываем приветствие
            lblWelcome.Text = $"Системный администратор: {login}";
        }

        /// <summary>
        /// Проверка учетной записи администратора по умолчанию из конфигурации
        /// </summary>
        public static bool CheckDefaultAdmin(string login, string password)
        {
            // Здесь можно добавить проверку из App.config
            // Для примера используем жестко заданные значения
            return login == "admin" && password == "admin";
        }

        /// <summary>
        /// Выбор операции: восстановление структуры
        /// </summary>
        private void rbRestore_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRestore.Checked)
            {
                tabControl1.Enabled = true;
                tabControl1.SelectedTab = tabRestore;
                gbOperation.Text = "Операция: Восстановление структуры БД";
            }
        }

        /// <summary>
        /// Выбор операции: импорт данных
        /// </summary>
        private void rbImport_CheckedChanged(object sender, EventArgs e)
        {
            if (rbImport.Checked)
            {
                tabControl1.Enabled = true;
                tabControl1.SelectedTab = tabImport;
                gbOperation.Text = "Операция: Импорт данных из CSV";
                LoadTables();
            }
        }

        /// <summary>
        /// Выход из формы системного администратора
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму администратора
            this.Visible = false;

            // Создаем и открываем форму авторизации
            Autorisation auto = new Autorisation();
            auto.ShowDialog();

            // После закрытия формы авторизации снова показываем форму администратора
            this.Visible = true;
        }

        /// <summary>
        /// Кнопка восстановления структуры базы данных
        /// </summary>
        private void btnRestore_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Восстановление структуры базы данных приведет к ПОЛНОМУ УДАЛЕНИЮ всех существующих таблиц и данных!\n\n" +
                "Вы уверены, что хотите продолжить?",
                "Подтверждение операции",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                RestoreDatabaseStructure();
            }
        }

        /// <summary>
        /// Восстановление структуры базы данных
        /// </summary>
        private void RestoreDatabaseStructure()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Отключаем проверку внешних ключей для безопасного удаления
                    ExecuteNonQuery(conn, "SET FOREIGN_KEY_CHECKS = 0");

                    // Удаление таблиц в правильном порядке (с учетом внешних ключей)
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS orders");
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS services");
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS categories");
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS difficultylevels");
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS statuses");
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS clients");
                    ExecuteNonQuery(conn, "DROP TABLE IF EXISTS users");

                    // Создание таблицы users
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE users (
                            UserID INT NOT NULL AUTO_INCREMENT,
                            Login VARCHAR(50) NOT NULL,
                            Password VARCHAR(255) NOT NULL,
                            FIO VARCHAR(150) NOT NULL,
                            IDRole INT DEFAULT NULL,
                            PRIMARY KEY (UserID),
                            UNIQUE KEY Login (Login)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Создание таблицы clients
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE clients (
                            ClientID INT NOT NULL AUTO_INCREMENT,
                            FirstName VARCHAR(50) NOT NULL,
                            LastName VARCHAR(50) NOT NULL,
                            Surname VARCHAR(50) DEFAULT NULL,
                            PhoneNumber VARCHAR(20) NOT NULL,
                            Age INT DEFAULT NULL,
                            PRIMARY KEY (ClientID),
                            UNIQUE KEY PhoneNumber (PhoneNumber)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Создание таблицы categories
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE categories (
                            CategoriesID INT NOT NULL AUTO_INCREMENT,
                            Categorie VARCHAR(100) NOT NULL,
                            PRIMARY KEY (CategoriesID)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Создание таблицы difficultylevels
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE difficultylevels (
                            DifficultyID INT NOT NULL AUTO_INCREMENT,
                            Name VARCHAR(50) NOT NULL,
                            PRIMARY KEY (DifficultyID)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Создание таблицы statuses
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE statuses (
                            StatusID INT NOT NULL AUTO_INCREMENT,
                            Name VARCHAR(50) NOT NULL,
                            PRIMARY KEY (StatusID)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Создание таблицы services
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE services (
                            Article INT NOT NULL AUTO_INCREMENT,
                            Name VARCHAR(200) NOT NULL,
                            Description TEXT,
                            Price DECIMAL(10,2) NOT NULL,
                            Time INT DEFAULT NULL,
                            DayOfTheWeek INT DEFAULT NULL,
                            Picture VARCHAR(255) DEFAULT NULL,
                            MaxPeople INT DEFAULT NULL,
                            ISLevel INT DEFAULT NULL,
                            IDCategory INT DEFAULT NULL,
                            PRIMARY KEY (Article),
                            KEY ISLevel (ISLevel),
                            KEY IDCategory (IDCategory),
                            CONSTRAINT services_ibfk_1 FOREIGN KEY (ISLevel) REFERENCES difficultylevels (DifficultyID),
                            CONSTRAINT services_ibfk_2 FOREIGN KEY (IDCategory) REFERENCES categories (CategoriesID)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Создание таблицы orders
                    ExecuteNonQuery(conn, @"
                        CREATE TABLE orders (
                            ID INT NOT NULL AUTO_INCREMENT,
                            ClientID INT NOT NULL,
                            Article INT NOT NULL,
                            DateOfAdmission DATETIME NOT NULL,
                            DueDate DATETIME DEFAULT NULL,
                            StatusID INT NOT NULL,
                            UserID INT DEFAULT NULL,
                            ParticipantsCount INT DEFAULT NULL,
                            TotalPrice DECIMAL(10,2) DEFAULT NULL,
                            PRIMARY KEY (ID),
                            KEY ClientID (ClientID),
                            KEY Article (Article),
                            KEY StatusID (StatusID),
                            KEY UserID (UserID),
                            CONSTRAINT orders_ibfk_1 FOREIGN KEY (ClientID) REFERENCES clients (ClientID),
                            CONSTRAINT orders_ibfk_2 FOREIGN KEY (Article) REFERENCES services (Article),
                            CONSTRAINT orders_ibfk_3 FOREIGN KEY (StatusID) REFERENCES statuses (StatusID),
                            CONSTRAINT orders_ibfk_4 FOREIGN KEY (UserID) REFERENCES users (UserID)
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");

                    // Включаем обратно проверку внешних ключей
                    ExecuteNonQuery(conn, "SET FOREIGN_KEY_CHECKS = 1");

                    // Добавляем начальные данные для справочников
                    InsertInitialData(conn);

                    LogMessage("✓ Структура базы данных успешно восстановлена");
                    LogMessage("✓ Добавлены начальные данные в справочники");

                    MessageBox.Show("Структура базы данных успешно восстановлена!\n" +
                                  "Добавлены начальные данные в справочники.",
                        "Операция завершена",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ ОШИБКА: {ex.Message}");
                MessageBox.Show($"Ошибка при восстановлении структуры БД:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Добавление начальных данных в справочники
        /// </summary>
        private void InsertInitialData(MySqlConnection conn)
        {
            // Добавление уровней сложности
            ExecuteNonQuery(conn, @"
                INSERT INTO difficultylevels (Name) VALUES 
                ('Легкий'),
                ('Средний'),
                ('Сложный')");

            // Добавление статусов
            ExecuteNonQuery(conn, @"
                INSERT INTO statuses (Name) VALUES 
                ('В работе'),
                ('Выполнен'),
                ('Отменен')");

            // Добавление категорий
            ExecuteNonQuery(conn, @"
                INSERT INTO categories (Categorie) VALUES 
                ('Приключенческие'),
                ('Детективные'),
                ('Фэнтези'),
                ('Научная фантастика')");
        }

        /// <summary>
        /// Выполнение SQL-запроса без возврата данных
        /// </summary>
        private void ExecuteNonQuery(MySqlConnection conn, string query)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Загрузка списка таблиц для импорта
        /// </summary>
        private void LoadTables()
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
                MessageBox.Show($"Ошибка загрузки списка таблиц: {ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Выбор CSV файла
        /// </summary>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Выберите CSV файл для импорта";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;

                    // Показываем предпросмотр файла
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

                txtLog.AppendText(preview.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"Ошибка предпросмотра файла: {ex.Message}{Environment.NewLine}");
            }
        }

        /// <summary>
        /// Импорт данных из CSV
        /// </summary>
        private void btnImportData_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу для импорта",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Выберите CSV файл",
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string tableName = cmbTables.SelectedItem.ToString();
            ImportCSVData(tableName, txtFilePath.Text);
        }

        /// <summary>
        /// Импорт данных из CSV в указанную таблицу
        /// </summary>
        private void ImportCSVData(string tableName, string filePath)
        {
            try
            {
                // Чтение CSV файла с явным указанием кодировки UTF-8
                var lines = File.ReadAllLines(filePath, Encoding.UTF8)
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .ToArray();

                if (lines.Length == 0)
                {
                    MessageBox.Show("Файл пуст",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // Получение структуры таблицы
                DataTable tableSchema = GetTableSchema(tableName);
                if (tableSchema == null || tableSchema.Columns.Count == 0)
                {
                    MessageBox.Show("Не удалось получить структуру таблицы",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // ПОЛУЧАЕМ СПИСОК АВТОИНКРЕМЕНТНЫХ ПОЛЕЙ
                List<string> autoIncrementColumns = GetAutoIncrementColumns(tableName);

                // Определяем разделитель (; или ,)
                char separator = lines[0].Contains(';') ? ';' : ',';

                // Проверка заголовков (первая строка может содержать названия колонок)
                string[] headers = lines[0].Split(separator);
                int startRow = 0;

                // ОЧИЩАЕМ ЗАГОЛОВКИ ОТ КАВЫЧЕК
                for (int h = 0; h < headers.Length; h++)
                {
                    headers[h] = headers[h].Trim().Trim('"').Trim('\'');
                }

                // Проверяем, является ли первая строка заголовками
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

                // ОПРЕДЕЛЯЕМ КОЛИЧЕСТВО ПОЛЕЙ ДЛЯ ВСТАВКИ (без автоинкрементных)
                int expectedColumns = tableSchema.Columns.Count - autoIncrementColumns.Count;

                // Проверка соответствия количества полей в первой строке данных
                string[] firstDataRow = lines[startRow].Split(separator);

                if (firstDataRow.Length != expectedColumns)
                {
                    string errorMsg = $"Несоответствие количества полей!\n" +
                                    $"В CSV файле: {firstDataRow.Length} полей\n" +
                                    $"В таблице '{tableName}': {tableSchema.Columns.Count} полей\n" +
                                    $"Автоинкрементные поля (не нужно в CSV): {string.Join(", ", autoIncrementColumns)}\n" +
                                    $"Ожидаемое количество полей в CSV: {expectedColumns}\n\n" +
                                    $"Структура таблицы:\n" +
                                    string.Join(", ", tableSchema.Columns.Cast<DataColumn>().Select(c => c.ColumnName));

                    LogMessage($"✗ ОШИБКА: {errorMsg}");

                    MessageBox.Show(errorMsg,
                        "Ошибка импорта",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // СОЗДАЕМ МЭППИНГ ПОЛЕЙ - какие колонки таблицы соответствуют колонкам CSV
                List<int> columnMapping = new List<int>();

                if (hasHeader)
                {
                    // Если есть заголовки, сопоставляем по именам
                    for (int i = 0; i < headers.Length; i++)
                    {
                        string header = headers[i];
                        if (tableSchema.Columns.Contains(header) && !autoIncrementColumns.Contains(header))
                        {
                            columnMapping.Add(tableSchema.Columns[header].Ordinal);
                        }
                    }
                }
                else
                {
                    // Если нет заголовков, предполагаем, что порядок полей правильный (без автоинкрементных)
                    for (int i = 0; i < tableSchema.Columns.Count; i++)
                    {
                        string colName = tableSchema.Columns[i].ColumnName;
                        if (!autoIncrementColumns.Contains(colName))
                        {
                            columnMapping.Add(i);
                        }
                    }
                }

                int importedCount = 0;
                int errorCount = 0;
                List<string> errors = new List<string>();

                // Добавляем настройки подключения для поддержки кириллицы
                string connectionStringWithCharset = connectionString + ";Charset=utf8;";

                using (MySqlConnection conn = new MySqlConnection(connectionStringWithCharset))
                {
                    conn.Open();

                    // Устанавливаем кодировку для соединения
                    MySqlCommand setCharsetCmd = new MySqlCommand("SET NAMES 'utf8mb4'", conn);
                    setCharsetCmd.ExecuteNonQuery();

                    for (int i = startRow; i < lines.Length; i++)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line)) continue;

                        // Убираем кавычки из всей строки
                        line = line.Replace("\"", "");
                        string[] values = line.Split(separator);

                        try
                        {
                            // ГЕНЕРИРУЕМ ЗАПРОС ТОЛЬКО ДЛЯ НУЖНЫХ ПОЛЕЙ
                            string query = GenerateInsertQuery(tableName, tableSchema, autoIncrementColumns);
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            int paramIndex = 0;
                            for (int j = 0; j < columnMapping.Count; j++)
                            {
                                int tableColIndex = columnMapping[j];
                                string value = values[j].Trim();

                                // Определяем тип колонки
                                Type columnType = tableSchema.Columns[tableColIndex].DataType;

                                // Преобразование пустых строк в NULL
                                if (string.IsNullOrEmpty(value))
                                {
                                    cmd.Parameters.AddWithValue($"@p{paramIndex}", DBNull.Value);
                                }
                                else
                                {
                                    // Пытаемся преобразовать значение в соответствующий тип
                                    try
                                    {
                                        if (columnType == typeof(int) || columnType == typeof(long))
                                        {
                                            if (int.TryParse(value, out int intValue))
                                                cmd.Parameters.AddWithValue($"@p{paramIndex}", intValue);
                                            else
                                                throw new Exception($"Не удалось преобразовать '{value}' в число");
                                        }
                                        else if (columnType == typeof(decimal) || columnType == typeof(double))
                                        {
                                            // Заменяем точку на запятую для десятичных чисел
                                            value = value.Replace('.', ',');
                                            if (decimal.TryParse(value, out decimal decValue))
                                                cmd.Parameters.AddWithValue($"@p{paramIndex}", decValue);
                                            else
                                                throw new Exception($"Не удалось преобразовать '{value}' в число");
                                        }
                                        else if (columnType == typeof(DateTime))
                                        {
                                            if (DateTime.TryParse(value, out DateTime dateValue))
                                                cmd.Parameters.AddWithValue($"@p{paramIndex}", dateValue);
                                            else
                                                throw new Exception($"Не удалось преобразовать '{value}' в дату");
                                        }
                                        else
                                        {
                                            // Для строковых полей
                                            cmd.Parameters.Add($"@p{paramIndex}", MySqlDbType.VarChar).Value = value;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"Ошибка преобразования значения '{value}' в тип {columnType.Name}: {ex.Message}");
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
                }

                // Вывод результатов
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

                string message = $"Импорт завершен!\n\n" +
                               $"Успешно импортировано: {importedCount} записей\n" +
                               $"Ошибок: {errorCount}";

                MessageBox.Show(message,
                    "Результат импорта",
                    MessageBoxButtons.OK,
                    errorCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMessage($"✗ КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");

                MessageBox.Show($"Ошибка при импорте данных:\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

                    // Устанавливаем кодировку
                    MySqlCommand setCharsetCmd = new MySqlCommand("SET NAMES 'utf8mb4'", conn);
                    setCharsetCmd.ExecuteNonQuery();

                    string query = $"SELECT * FROM {tableName} LIMIT 0";
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
        /// Генерация SQL-запроса для вставки данных
        /// </summary>
        private string GenerateInsertQuery(string tableName, DataTable schema, List<string> autoIncrementColumns)
        {
            // Берем только те поля, которые не являются автоинкрементными
            var columns = schema.Columns.Cast<DataColumn>()
                .Where(c => !autoIncrementColumns.Contains(c.ColumnName))
                .Select(c => $"`{c.ColumnName}`")
                .ToList();

            string columnsStr = string.Join(", ", columns);
            string parameters = string.Join(", ", Enumerable.Range(0, columns.Count).Select(i => $"@p{i}"));

            return $"INSERT INTO `{tableName}` ({columnsStr}) VALUES ({parameters})";
        }

        /// <summary>
        /// Добавление сообщения в лог
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
        }


        private void ImportDataSmart(string tableName, string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8)
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .ToArray();

                if (lines.Length < 2)
                {
                    MessageBox.Show("Файл должен содержать заголовок и данные");
                    return;
                }

                // Получаем заголовки из первой строки
                string[] headers = lines[0].Split(';');

                // Получаем схему таблицы
                DataTable schema = GetTableSchema(tableName);

                // Определяем, какие поля из CSV соответствуют полям таблицы
                // Исключаем автоинкрементные поля
                List<int> columnIndexes = new List<int>();
                List<string> columnNames = new List<string>();

                // Получаем список автоинкрементных полей
                List<string> autoIncrementColumns = GetAutoIncrementColumns(tableName);

                for (int i = 0; i < headers.Length; i++)
                {
                    string header = headers[i].Trim();
                    if (schema.Columns.Contains(header) && !autoIncrementColumns.Contains(header))
                    {
                        columnIndexes.Add(i);
                        columnNames.Add(header);
                    }
                }

                string connectionStringWithCharset = connectionString + ";Charset=utf8;";
                int importedCount = 0;
                int errorCount = 0;

                using (MySqlConnection conn = new MySqlConnection(connectionStringWithCharset))
                {
                    conn.Open();

                    // Устанавливаем кодировку
                    MySqlCommand setCharsetCmd = new MySqlCommand("SET NAMES 'utf8mb4'", conn);
                    setCharsetCmd.ExecuteNonQuery();

                    // Формируем запрос
                    string columns = string.Join(", ", columnNames);
                    string parameters = string.Join(", ", columnNames.Select(c => "@" + c));
                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                    for (int i = 1; i < lines.Length; i++)
                    {
                        try
                        {
                            string[] values = lines[i].Split(';');

                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            for (int j = 0; j < columnIndexes.Count; j++)
                            {
                                int colIndex = columnIndexes[j];
                                string colName = columnNames[j];
                                string value = values[colIndex].Trim();

                                // Определяем тип колонки
                                Type colType = schema.Columns[colName].DataType;

                                if (colType == typeof(int) || colType == typeof(long))
                                {
                                    cmd.Parameters.AddWithValue("@" + colName, int.Parse(value));
                                }
                                else if (colType == typeof(decimal))
                                {
                                    cmd.Parameters.AddWithValue("@" + colName, decimal.Parse(value.Replace('.', ',')));
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@" + colName, value);
                                }
                            }

                            cmd.ExecuteNonQuery();
                            importedCount++;
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            LogMessage($"Ошибка в строке {i + 1}: {ex.Message}");
                        }
                    }
                }

                LogMessage($"\nИмпорт {tableName} завершен:");
                LogMessage($"Успешно: {importedCount}");
                LogMessage($"Ошибок: {errorCount}");

                MessageBox.Show($"Импорт {tableName} завершен!\nДобавлено: {importedCount}\nОшибок: {errorCount}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private List<string> GetAutoIncrementColumns(string tableName)
        {
            List<string> autoIncrementCols = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Запрос для получения информации о автоинкрементных полях
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
    }
}