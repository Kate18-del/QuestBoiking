using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Главная форма для пользователей с ролью "Директор"
    /// Предоставляет доступ к основным разделам системы для директора
    /// </summary>
    public partial class MainDirector : Form
    {
        /// <summary>
        /// Конструктор формы главного меню директора
        /// </summary>
        public MainDirector()
        {
            InitializeComponent();
            // Отображаем информацию о текущем пользователе при загрузке формы
            DisplayCurrentUser();
            // Авто-бэкап при выходе
            this.FormClosing += MainForm_FormClosing;
            this.FormClosed += MainForm_FormClosed;
        }

        /// <summary>
        /// Авто-бэкап при закрытии формы
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (BackupHelper.BackupAsked) return;

            string autoBackupEnabled = ConfigurationManager.AppSettings["AutoBackupEnabled"];
            if (autoBackupEnabled?.ToLower() == "true")
            {
                BackupHelper.BackupAsked = true;

                DialogResult result = MessageBox.Show(
                    "Создать резервную копию базы данных перед выходом?",
                    "Авто-резервное копирование",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    CreateAutoBackup();
                    MessageBox.Show("Резервная копия создана", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Если это последняя форма - выходим из приложения
            if (Application.OpenForms.Count <= 1)
            {
                Application.Exit();
            }
        }

        private void CreateAutoBackup()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                string backupFolder = Path.Combine(Application.StartupPath, "AutoBackups");
                if (!Directory.Exists(backupFolder))
                    Directory.CreateDirectory(backupFolder);

                string fileName = $"auto_backup_{DateTime.Now:yyyyMMdd_HHmm}.sql";
                string filePath = Path.Combine(backupFolder, fileName);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    DataTable tables = conn.GetSchema("Tables");

                    using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        writer.BaseStream.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                        writer.WriteLine("/*!40101 SET NAMES utf8mb4 */;");
                        writer.WriteLine("/*!40101 SET FOREIGN_KEY_CHECKS=0 */;");
                        writer.WriteLine();

                        foreach (DataRow row in tables.Rows)
                        {
                            string tableName = row["TABLE_NAME"].ToString();

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

                            // Данные (без BLOB)
                            string colsQuery = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = '{tableName}' AND DATA_TYPE != 'blob' AND DATA_TYPE != 'longblob'";
                            MySqlCommand colsCmd = new MySqlCommand(colsQuery, conn);
                            List<string> columns = new List<string>();
                            using (MySqlDataReader colReader = colsCmd.ExecuteReader())
                            {
                                while (colReader.Read())
                                    columns.Add(colReader.GetString(0));
                            }

                            if (columns.Count > 0)
                            {
                                string colsStr = string.Join("`, `", columns);
                                MySqlCommand cmdData = new MySqlCommand($"SELECT `{colsStr}` FROM `{tableName}`", conn);
                                using (MySqlDataReader reader = cmdData.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        List<string> values = new List<string>();
                                        foreach (string col in columns)
                                        {
                                            int ordinal = reader.GetOrdinal(col);
                                            if (reader.IsDBNull(ordinal))
                                                values.Add("NULL");
                                            else
                                            {
                                                string val = reader.GetValue(ordinal).ToString()
                                                    .Replace("\\", "\\\\").Replace("'", "\\'");
                                                values.Add($"'{val}'");
                                            }
                                        }
                                        writer.WriteLine($"INSERT INTO `{tableName}` (`{string.Join("`, `", columns)}`) VALUES ({string.Join(", ", values)});");
                                    }
                                }
                            }
                            writer.WriteLine();
                        }

                        writer.WriteLine("/*!40101 SET FOREIGN_KEY_CHECKS=1 */;");
                    }
                }

                Cursor.Current = Cursors.Default;

                // Удаляем старые бэкапы
                CleanOldBackups(backupFolder, 5);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void CleanOldBackups(string folder, int keepCount)
        {
            try
            {
                var files = new DirectoryInfo(folder).GetFiles("auto_backup_*.sql")
                    .OrderByDescending(f => f.CreationTime).Skip(keepCount);
                foreach (var file in files)
                    file.Delete();
            }
            catch { }
        }
        /// <summary>
        /// Отображает информацию о текущем директоре в интерфейсе
        /// Формирует краткое ФИО в формате: "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            // Проверяем, что данные о пользователе существуют
            if (CurrentUser.FIO != null)
            {
                // Разделяем полное ФИО на части (Фамилия, Имя, Отчество)
                string[] fioParts = CurrentUser.FIO.Split(' ');

                // Формируем краткое ФИО: первая часть полностью, от второй и третьей - первые буквы
                // Например: "Петров Петр Петрович" -> "Петров П.П."
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";

                // Отображаем приветствие с указанием роли и кратким ФИО
                label2.Text = $"директор {shortName}";
            }
        }

        /// <summary>
        /// Обработчик кнопки выхода из системы
        /// Возвращает пользователя на форму авторизации
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Autorisation auto = new Autorisation();
            auto.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Обработчик кнопки перехода к учету заказов
        /// Открывает форму для просмотра и анализа заказов (для директора)
        /// </summary>
        private void btnOrders_Click(object sender, EventArgs e)
        {
            this.Hide();
            OrderAccountingDirector auto = new OrderAccountingDirector();
            auto.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Обработчик кнопки перехода к управлению услугами
        /// Открывает форму для просмотра и редактирования услуг (квестов)
        /// </summary>
        private void btnServices_Click(object sender, EventArgs e)
        {
            this.Hide();
            ServicesDirector auto = new ServicesDirector();
            auto.ShowDialog();
            this.Close();
        }
    }
}