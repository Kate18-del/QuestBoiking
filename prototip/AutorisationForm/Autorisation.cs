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
                    // Открываем соединение с базой данных
                    conn.Open();

                    // Хэшируем введенный пароль для сравнения с хранящимся в БД
                    string hashedPassword = ComputeSha256Hash(password);

                    // SQL-запрос для получения данных пользователя по логину и паролю
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT UserID, Login, FIO, IDRole FROM users WHERE Login = @login AND Password = @password",
                        conn);

                    // Добавляем параметры для защиты от SQL-инъекций
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    // Выполняем запрос и получаем результат в виде DataReader
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Если пользователь найден (есть хотя бы одна запись)
                        if (reader.Read())
                        {
                            // Извлекаем данные пользователя из результата запроса
                            int userId = Convert.ToInt32(reader["UserID"]);
                            string userLogin = reader["Login"].ToString();
                            string fio = reader["FIO"].ToString();
                            int roleId = Convert.ToInt32(reader["IDRole"]);

                            // Сохраняем данные текущего пользователя в статическом классе CurrentUser
                            CurrentUser.UserID = userId;
                            CurrentUser.Login = userLogin;
                            CurrentUser.FIO = fio;
                            CurrentUser.Role = roleId;

                            // Показываем приветственное сообщение
                            MessageBox.Show($"Добро пожаловать, {fio}!", "Успешный вход");

                            // Скрываем форму авторизации
                            this.Hide();

                            // Открываем соответствующую главную форму в зависимости от роли пользователя
                            if (roleId == 1) // Администратор (IDRole = 1)
                            {
                                new MainAdmin().ShowDialog();
                            }
                            else if (roleId == 2) // Директор (IDRole = 2)
                            {
                                new MainDirector().ShowDialog();
                            }
                            else // Менеджер (все остальные роли)
                            {
                                new MainManager().ShowDialog();
                            }

                            // Закрываем форму авторизации после завершения работы главной формы
                            this.Close();
                        }
                        else
                        {
                            // Если пользователь не найден, показываем сообщение об ошибке
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