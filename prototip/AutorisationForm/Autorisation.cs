using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Форма авторизации пользователей в системе
    /// </summary>
    public partial class Autorisation : Form
    {
        // Статическое свойство для отслеживания последней активной формы
        public static Form LastActiveForm { get; set; }

        // Событие успешной авторизации
        public event EventHandler LoginSucceeded;
        /// <summary>
        /// Конструктор формы авторизации
        /// </summary>
        public Autorisation()
        {
            InitializeComponent();

            // Настройка поля для ввода пароля: символы будут скрыты звездочками
            txtPassword.PasswordChar = '*';

            // При загрузке формы устанавливаем фокус на поле ввода логина
            // Используем лямбда-выражение для обработчика события Load
            this.Load += (s, e) => txtLogin.Focus();
        }

        /// <summary>
        /// Обработчик нажатия кнопки входа в систему
        /// Выполняет проверку введенных данных и аутентификацию пользователя
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Получаем введенные данные, удаляем лишние пробелы из логина
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            // Проверка на пустые поля ввода
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return; // Прерываем выполнение метода
            }

            // Создаем подключение к базе данных с использованием строки подключения из конфигурации
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Проверка учетной записи системного администратора по умолчанию
                    if (login == "admin" && password == "admin")
                    {
                        MessageBox.Show("Добро пожаловать, системный администратор!", "Успешный вход");
                        this.Hide();
                        new SystemAdminForm(login).ShowDialog();
                        this.Close();
                        return;
                    }

                    string hashedPassword = ComputeSha256Hash(password);

                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT UserID, Login, FIO, IDRole FROM users WHERE Login = @login AND Password = @password",
                        conn);

                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = Convert.ToInt32(reader["UserID"]);
                            string userLogin = reader["Login"].ToString();
                            string fio = reader["FIO"].ToString();
                            int roleId = Convert.ToInt32(reader["IDRole"]);

                            CurrentUser.UserID = userId;
                            CurrentUser.Login = userLogin;
                            CurrentUser.FIO = fio;
                            CurrentUser.Role = roleId;

                            MessageBox.Show($"Добро пожаловать, {fio}!", "Успешный вход");
                            // Вызываем событие успешного входа
                            LoginSucceeded?.Invoke(this, EventArgs.Empty);
                            this.Hide();

                            if (roleId == 1)
                            {
                                new MainAdmin().ShowDialog();
                            }
                            else if (roleId == 2)
                            {
                                new MainDirector().ShowDialog();
                            }
                            else
                            {
                                new MainManager().ShowDialog();
                            }

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка возможных ошибок при работе с базой данных
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Вычисляет хэш SHA256 для переданной строки
        /// </summary>
        /// <param name="rawData">Исходная строка для хэширования</param>
        /// <returns>Хэш строки в шестнадцатеричном формате</returns>
        private string ComputeSha256Hash(string rawData)
        {
            // Создаем объект для вычисления хэша SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем строку в массив байт и вычисляем хэш
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Создаем строку для хранения результата в шестнадцатеричном формате
                StringBuilder builder = new StringBuilder();

                // Преобразуем каждый байт в двухсимвольное шестнадцатеричное представление
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки выхода из приложения
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Завершаем работу приложения
            Application.Exit();
        }

        /// <summary>
        /// Обработчик события ввода символов в поле логина
        /// Ограничивает допустимые символы для ввода
        /// </summary>
        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли введенный символ допустимым
            // Разрешены: буквы (a-z, A-Z), цифры (0-9) и специальные символы @._%+-
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9@._%+-]") && !char.IsControl(e.KeyChar))
            {
                // Если символ недопустимый и не является управляющим (Backspace, Enter и т.д.),
                // то блокируем его ввод
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик события ввода символов в поле пароля
        /// Ограничивает допустимые символы для ввода
        /// </summary>
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Такая же проверка, как и для поля логина
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9@._%+-]") && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
