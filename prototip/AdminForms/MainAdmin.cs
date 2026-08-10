using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Главная форма для пользователей с ролью "Администратор"
    /// Предоставляет доступ ко всем функциям управления системой
    /// </summary>
    public partial class MainAdmin : Form
    {
        private InactivityTracker inactivityTracker;
        /// <summary>
        /// Конструктор формы главного меню администратора
        /// </summary>
        public MainAdmin()
        {
            InitializeComponent();
            // Отображаем информацию о текущем пользователе при загрузке формы
            DisplayCurrentUser();

            InitializeInactivityTracker();
            // Авто-бэкап при выходе
            this.FormClosing += MainForm_FormClosing;
            this.FormClosed += MainForm_FormClosed;
            ;
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

                Application.Exit();

        }

        /// <summary>
        /// Отображает информацию о текущем администраторе в интерфейсе
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
                // Например: "Иванов Иван Иванович" -> "Иванов И.И."
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";

                // Отображаем приветствие с указанием роли и кратким ФИО
                label2.Text = $"администратор {shortName}";
            }
        }

        private void InitializeInactivityTracker()
        {
            inactivityTracker = new InactivityTracker(this);
            inactivityTracker.InactivityDetected += InactivityTracker_InactivityDetected;
            inactivityTracker.Start();
        }


        private void InactivityTracker_InactivityDetected(object sender, EventArgs e)
        {
            // Останавливаем трекер
            inactivityTracker.Stop();

            // ПОКАЗЫВАЕМ СООБЩЕНИЕ О БЛОКИРОВКЕ
            MessageBox.Show(
                "Система заблокирована из-за длительного бездействия.\n" +
                "Пожалуйста, авторизуйтесь снова для продолжения работы.",
                "Блокировка системы",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Запоминаем текущую форму
            Autorisation.LastActiveForm = this;

            // Создаем форму авторизации
            Autorisation authForm = new Autorisation();

            // Подписываемся на событие успешного входа
            authForm.LoginSucceeded += (s, args) =>
            {
                // Показываем форму заново
                this.Visible = true;

                // Перезапускаем трекер
                inactivityTracker.Start();
            };

            authForm.ShowDialog();
        }


        /// <summary>
        /// Обработчик кнопки выхода из системы
        /// Возвращает пользователя на форму авторизации
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            inactivityTracker?.Stop();
            this.Hide();
            // Создаем и открываем форму авторизации
            Autorisation auto = new Autorisation();
            auto.ShowDialog();
            this.Close();
            inactivityTracker?.Start();
        }

        /// <summary>
        /// Обработчик кнопки перехода к управлению пользователями
        /// Открывает форму для работы со списком пользователей
        /// </summary>
        private void btnUsers_Click(object sender, EventArgs e)
        {
            inactivityTracker?.Stop();
            this.Hide();
            // Создаем и открываем форму управления пользователями
            Users auto = new Users();
            auto.ShowDialog();

            this.Close();

            inactivityTracker?.Start();
        }

        /// <summary>
        /// Обработчик кнопки перехода к учету заказов
        /// Открывает форму для просмотра и управления заказами
        /// </summary>
        private void btnOrders_Click(object sender, EventArgs e)
        {
            inactivityTracker?.Stop();
            this.Hide();
            // Создаем и открываем форму учета заказов (для администратора)
            OrderAccountingAdmin auto = new OrderAccountingAdmin();
            auto.ShowDialog();
            this.Close();

            inactivityTracker?.Start();
        }

        /// <summary>
        /// Обработчик кнопки перехода к справочникам
        /// Открывает форму для работы со справочной информацией (книги, авторы и т.д.)
        /// </summary>
        private void btnBooks_Click(object sender, EventArgs e)
        {
            inactivityTracker?.Stop();
            this.Hide();
            // Создаем и открываем форму справочников (для администратора)
            ReferenceBooksAdmin auto = new ReferenceBooksAdmin();
            auto.ShowDialog();
            this.Close();
            inactivityTracker?.Start();  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inactivityTracker?.Stop();
            this.Hide();
            // Создаем и открываем форму справочников (для администратора)
            StatisticsAdmin auto = new StatisticsAdmin();
            auto.ShowDialog();
            this.Close();
            inactivityTracker?.Start();
        }
    }
}