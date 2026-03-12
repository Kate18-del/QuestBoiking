using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace prototip
{
    public partial class Autorisation : Form
    {
        public static Form LastActiveForm { get; set; }
        public event EventHandler LoginSucceeded;

        private int failedLoginAttempts = 0;
        private bool requireCaptcha = false;
        private bool isBlocked = false;
        private int blockTimeSeconds = 10;
        private bool captchaWasShown = false; // Флаг, что капча уже была показана

        private string captchaCode;
        private Bitmap captchaImage;
        private Timer blockTimer;
        private Random random = new Random();

        public Autorisation()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            this.Load += (s, e) => txtLogin.Focus();

            // Подписка на события CAPTCHA
            this.btnRefreshCaptcha.Click += BtnRefreshCaptcha_Click;
            this.txtCaptcha.TextChanged += TxtCaptcha_TextChanged;

            // Изначально скрываем CAPTCHA
            HideCaptcha();
        }

        /// <summary>
        /// Генерация новой CAPTCHA
        /// </summary>
        private void GenerateNewCaptcha()
        {
            if (isBlocked) return;

            // Генерация случайного кода из 4 символов
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
            captchaCode = "";
            for (int i = 0; i < 4; i++)
            {
                captchaCode += chars[random.Next(chars.Length)];
            }

            // Создание изображения
            captchaImage = new Bitmap(250, 80);
            using (Graphics g = Graphics.FromImage(captchaImage))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Разные позиции для символов (не в одной линии)
                int[] xPos = { 30, 80, 130, 180 };
                int[] yPos = {
                    random.Next(15, 35),
                    random.Next(25, 45),
                    random.Next(10, 30),
                    random.Next(20, 40)
                };

                for (int i = 0; i < captchaCode.Length; i++)
                {
                    // Разный размер шрифта
                    float fontSize = random.Next(24, 32);

                    using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.FromArgb(
                        random.Next(50, 200),
                        random.Next(50, 200),
                        random.Next(50, 200))))
                    {
                        // Рисуем символ
                        g.DrawString(captchaCode[i].ToString(), font, brush, xPos[i], yPos[i]);
                    }

                    // Перечеркивание некоторых символов (примерно половина)
                    if (random.Next(2) == 0)
                    {
                        using (Pen pen = new Pen(Color.FromArgb(150, Color.Red), 2))
                        {
                            g.DrawLine(pen, xPos[i] - 5, yPos[i] - 5,
                                          xPos[i] + 35, yPos[i] + 20);
                        }
                    }
                }

                // Графический шум - линии
                for (int i = 0; i < 6; i++)
                {
                    using (Pen pen = new Pen(Color.FromArgb(100, Color.Gray), 1))
                    {
                        g.DrawLine(pen,
                            random.Next(0, 250), random.Next(0, 80),
                            random.Next(0, 250), random.Next(0, 80));
                    }
                }

                // Графический шум - точки
                for (int i = 0; i < 150; i++)
                {
                    int x = random.Next(0, 250);
                    int y = random.Next(0, 80);
                    if (x < 250 && y < 80)
                    {
                        captchaImage.SetPixel(x, y,
                            Color.FromArgb(random.Next(100, 200),
                            random.Next(100, 200),
                            random.Next(100, 200)));
                    }
                }
            }

            if (pbCaptcha.Image != null)
                pbCaptcha.Image.Dispose();
            pbCaptcha.Image = captchaImage;
        }

        /// <summary>
        /// Показать CAPTCHA
        /// </summary>
        private void ShowCaptcha()
        {
            pbCaptcha.Visible = true;
            btnRefreshCaptcha.Visible = true;
            txtCaptcha.Visible = true;
            lblCaptchaError.Visible = false;
            GenerateNewCaptcha();
            txtCaptcha.Clear();
            txtCaptcha.Focus();

            // Показываем сообщение о необходимости ввести капчу
            MessageBox.Show("Для продолжения введите код с картинки", "Проверка безопасности",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Скрыть CAPTCHA
        /// </summary>
        private void HideCaptcha()
        {
            pbCaptcha.Visible = false;
            btnRefreshCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            lblCaptchaError.Visible = false;
        }

        /// <summary>
        /// Проверка CAPTCHA
        /// </summary>
        private bool ValidateCaptcha()
        {
            string input = txtCaptcha.Text.ToUpper();

            if (string.IsNullOrEmpty(input) || input.Length != 4)
            {
                lblCaptchaError.Text = "Введите 4 символа!";
                lblCaptchaError.Visible = true;
                return false;
            }

            if (input == captchaCode)
            {
                lblCaptchaError.Visible = false;
                return true;
            }
            else
            {
                lblCaptchaError.Text = "Неверный код!";
                lblCaptchaError.Visible = true;
                return false;
            }
        }

        /// <summary>
        /// Блокировка на 10 секунд
        /// </summary>
        private void StartBlockTimer()
        {
            isBlocked = true;
            int remaining = blockTimeSeconds;

            // Блокируем все элементы
            txtLogin.Enabled = false;
            txtPassword.Enabled = false;
            btnLogin.Enabled = false;
            txtCaptcha.Enabled = false;
            btnRefreshCaptcha.Enabled = false;

            lblBlockTimer.Visible = true;
            lblBlockTimer.Text = $"⏱ Блокировка: {remaining} сек";
            lblBlockTimer.Location = new Point(334, 232);

            blockTimer = new Timer();
            blockTimer.Interval = 1000;
            blockTimer.Tick += (s, e) =>
            {
                remaining--;
                lblBlockTimer.Text = $"⏱ Блокировка: {remaining} сек";

                if (remaining <= 0)
                {
                    blockTimer.Stop();
                    isBlocked = false;

                    // Разблокируем элементы
                    txtLogin.Enabled = true;
                    txtPassword.Enabled = true;
                    btnLogin.Enabled = true;

                    if (requireCaptcha)
                    {
                        txtCaptcha.Enabled = true;
                        btnRefreshCaptcha.Enabled = true;
                        ShowCaptcha();
                    }

                    lblBlockTimer.Visible = false;
                }
            };
            blockTimer.Start();

            MessageBox.Show($"Слишком много неудачных попыток. Вход заблокирован на {blockTimeSeconds} секунд.",
                "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            if (!isBlocked)
            {
                GenerateNewCaptcha();
                txtCaptcha.Clear();
                txtCaptcha.Focus();
            }
        }

        private void TxtCaptcha_TextChanged(object sender, EventArgs e)
        {
            // Автоматическая проверка при вводе 4 символов
            if (txtCaptcha.Text.Length == 4 && !isBlocked && requireCaptcha)
            {
                ProcessCaptchaInput();
            }
        }

        /// <summary>
        /// Обработка ввода CAPTCHA
        /// </summary>
        private void ProcessCaptchaInput()
        {
            if (!ValidateCaptcha())
            {
                // Неверная CAPTCHA - блокировка на 10 секунд
                StartBlockTimer();
                return;
            }

            // CAPTCHA верна
            MessageBox.Show("Капча пройдена, введите логин и пароль", "Успешно",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            requireCaptcha = false;
            captchaWasShown = true; // Запоминаем, что капча уже была показана
            HideCaptcha();

            // Очищаем поля для нового ввода
            txtPassword.Clear();
            txtLogin.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (isBlocked) return;

            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CheckLogin(login, password);
        }

        private void CheckLogin(string login, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Проверка администратора по умолчанию
                    if (login == "admin" && password == "admin")
                    {
                        LoginSucceeded?.Invoke(this, EventArgs.Empty);
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
                            // Успешный вход
                            failedLoginAttempts = 0;
                            requireCaptcha = false;
                            captchaWasShown = false; // Сбрасываем флаг капчи
                            HideCaptcha();

                            CurrentUser.UserID = Convert.ToInt32(reader["UserID"]);
                            CurrentUser.Login = reader["Login"].ToString();
                            CurrentUser.FIO = reader["FIO"].ToString();
                            CurrentUser.Role = Convert.ToInt32(reader["IDRole"]);

                            MessageBox.Show($"Добро пожаловать, {CurrentUser.FIO}!", "Успешный вход");
                            LoginSucceeded?.Invoke(this, EventArgs.Empty);

                            this.Hide();

                            if (CurrentUser.Role == 1)
                                new MainAdmin().ShowDialog();
                            else if (CurrentUser.Role == 2)
                                new MainDirector().ShowDialog();
                            else
                                new MainManager().ShowDialog();

                            this.Close();
                        }
                        else
                        {
                            // НЕУСПЕШНАЯ АВТОРИЗАЦИЯ
                            failedLoginAttempts++;

                            // Сначала сообщение об ошибке
                            MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Если капча уже была показана ранее (после успешного ввода капчи) - сразу блокировка
                            if (captchaWasShown)
                            {
                                StartBlockTimer();
                                return;
                            }

                            // После второй неудачной попытки показываем капчу
                            if (failedLoginAttempts >= 2 && !requireCaptcha)
                            {
                                requireCaptcha = true;
                                ShowCaptcha();
                            }

                            // Очищаем поле пароля для новой попытки
                            txtPassword.Clear();

                            if (!requireCaptcha)
                            {
                                txtLogin.Focus();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9@._%+-]") && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[a-zA-Z0-9@._%+-]") && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            blockTimer?.Stop();
            pbCaptcha.Image?.Dispose();
            base.OnFormClosing(e);
        }
    }
}