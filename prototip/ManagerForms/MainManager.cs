using MySql.Data.MySqlClient;
using prototip.ManagerForms;
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
    /// Главная форма для пользователей с ролью "Менеджер"
    /// Предоставляет доступ к основным разделам системы для работы с клиентами и заказами
    /// </summary>
    public partial class MainManager : Form
    {
        /// <summary>
        /// Конструктор формы главного меню менеджера
        /// </summary>
        public MainManager()
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
            string autoBackupEnabled = ConfigurationManager.AppSettings["AutoBackupEnabled"];
            if (autoBackupEnabled?.ToLower() == "true")
            {
                try
                {
                    string folder = Path.Combine(Application.StartupPath, "AutoBackups");
                    Directory.CreateDirectory(folder);
                    string filePath = Path.Combine(folder, $"auto_backup_{DateTime.Now:yyyyMMdd_HHmm}.sql");

                    using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                    {
                        conn.Open();
                        DataTable tables = conn.GetSchema("Tables");

                        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                        {
                            writer.BaseStream.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                            writer.WriteLine("/*!40101 SET NAMES utf8mb4 */;");
                            writer.WriteLine("/*!40101 SET FOREIGN_KEY_CHECKS=0 */;");
                            writer.WriteLine($"\n-- Auto-backup: {DateTime.Now}\n");

                            foreach (DataRow row in tables.Rows)
                            {
                                string tn = row["TABLE_NAME"].ToString();
                                using (MySqlDataReader r = new MySqlCommand($"SHOW CREATE TABLE `{tn}`", conn).ExecuteReader())
                                    if (r.Read()) { writer.WriteLine($"DROP TABLE IF EXISTS `{tn}`;\n{r.GetString(1)};\n"); }

                                DataTable schema = new DataTable();
                                new MySqlDataAdapter($"SELECT * FROM `{tn}` LIMIT 0", conn).Fill(schema);
                                var cols = schema.Columns.Cast<DataColumn>().Where(c => c.DataType != typeof(byte[])).Select(c => c.ColumnName).ToList();
                                if (cols.Count == 0) continue;

                                using (MySqlDataReader r = new MySqlCommand($"SELECT `{string.Join("`, `", cols)}` FROM `{tn}`", conn).ExecuteReader())
                                    while (r.Read())
                                    {
                                        var vals = cols.Select(c => r.IsDBNull(r.GetOrdinal(c)) ? "NULL" : $"'{r.GetValue(r.GetOrdinal(c)).ToString().Replace("\\", "\\\\").Replace("'", "\\'")}'").ToList();
                                        writer.WriteLine($"INSERT INTO `{tn}` (`{string.Join("`, `", cols)}`) VALUES ({string.Join(", ", vals)});");
                                    }
                                writer.WriteLine();
                            }
                            writer.WriteLine("/*!40101 SET FOREIGN_KEY_CHECKS=1 */;");
                        }
                    }

                    // Удаляем старые (оставляем 5)
                    var files = new DirectoryInfo(folder).GetFiles("auto_backup_*.sql").OrderByDescending(f => f.CreationTime).Skip(5);
                    foreach (var f in files) f.Delete();
                }
                catch { }
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

        /// <summary>
        /// Отображает информацию о текущем менеджере в интерфейсе
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
                // Например: "Смирнова Анна Сергеевна" -> "Смирнова А.С."
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";

                // Отображаем приветствие с указанием роли и кратким ФИО
                label2.Text = $"менеджер {shortName}";
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
        /// Обработчик кнопки перехода к просмотру услуг
        /// Открывает форму для просмотра доступных квестов и услуг
        /// </summary>
        private void btnServices_Click(object sender, EventArgs e)
        {
            this.Hide();
            ServicesManager auto = new ServicesManager();
            auto.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ScheduleForm schedule = new ScheduleForm();
            schedule.ShowDialog();
            this.Close();
        }
    }
}