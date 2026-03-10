using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Форма для управления пользователями системы
    /// Позволяет просматривать, добавлять, редактировать и удалять пользователей
    /// Доступна только для администраторов
    /// </summary>
    public partial class Users : Form
    {
        // Коллекция для хранения всех пользователей с поддержкой привязки к DataGridView
        private BindingList<User> allUsers;

        // Текущий выбранный пользователь в таблице
        private User selectedUser;

        // Сервис для работы с пользователями (бизнес-логика)
        private readonly UserService _userService;

        /// <summary>
        /// Конструктор формы управления пользователями
        /// </summary>
        public Users()
        {
            InitializeComponent();
            // Инициализация сервиса пользователей
            _userService = new UserService();
            // Настройка формы и загрузка данных
            InitializeForm();
        }

        /// <summary>
        /// Инициализация всех элементов формы
        /// </summary>
        private void InitializeForm()
        {
            // Настройка таблицы для отображения пользователей
            ConfigureDataGridView();

            // Загрузка ролей в выпадающий список
            LoadRoles();

            // Загрузка списка пользователей
            LoadUsers();

            // Подписка на события
            SubscribeToEvents();

            // Отображение информации о текущем пользователе
            DisplayCurrentUser();

            // Очистка полей ввода (установка плейсхолдеров)
            ClearFormFields();
        }

        /// <summary>
        /// Очистка полей формы и сброс выделения
        /// Устанавливает плейсхолдеры и отключает кнопку редактирования
        /// </summary>
        private void ClearFormFields()
        {
            // Установка плейсхолдеров для текстовых полей
            textBox2.Text = "ФИО";
            textBox2.ForeColor = SystemColors.GrayText;

            textBox3.Text = "Логин";
            textBox3.ForeColor = SystemColors.GrayText;

            textBox4.Text = "Пароль";
            textBox4.ForeColor = SystemColors.GrayText;
            textBox4.PasswordChar = '\0'; // Без звездочек для подсказки

            // Сброс выпадающего списка ролей
            comboBox3.SelectedIndex = -1;
            comboBox3.Text = "Роль";

            // Сброс выбранного пользователя и отключение кнопки редактирования
            selectedUser = null;
            button3.Enabled = false;
        }

        /// <summary>
        /// Настройка столбцов DataGridView для отображения информации о пользователях
        /// </summary>
        private void ConfigureDataGridView()
        {
            // Отключаем автоматическую генерацию столбцов
            dataGridView1.AutoGenerateColumns = false;

            // Настройка режимов выделения и редактирования
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;           // Только для чтения
            dataGridView1.AllowUserToAddRows = false; // Запрет добавления строк пользователем

            // Растягиваем колонки на всю доступную ширину
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false; // Убираем заголовки строк (нумерацию)

            // Очистка существующих колонок
            dataGridView1.Columns.Clear();

            // Добавление столбца для логина
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Login",
                HeaderText = "Логин",
                DataPropertyName = "Login", // Привязка к свойству User.Login
                Width = 120
            });

            // Добавление столбца для ФИО
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FIO",
                HeaderText = "ФИО",
                DataPropertyName = "FIO", // Привязка к свойству User.FIO
                Width = 200
            });

            // Добавление столбца для названия роли
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "RoleName",
                HeaderText = "Роль",
                DataPropertyName = "RoleName", // Привязка к свойству User.RoleName
                Width = 120
            });
        }

        /// <summary>
        /// Загрузка доступных ролей в выпадающий список
        /// </summary>
        private void LoadRoles()
        {
            comboBox3.Items.AddRange(new[] { "Администратор", "Директор", "Менеджер" });
        }

        /// <summary>
        /// Загрузка списка пользователей из базы данных
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                // Получение всех пользователей через сервис
                var users = _userService.GetAllUsers();
                allUsers = new BindingList<User>(users);

                // Привязка данных к таблице
                dataGridView1.DataSource = allUsers;

                // Обновление счетчика записей
                FormStateManager.UpdateRecordCount(label3, allUsers.Count);

                // Снятие выделения со всех строк
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Подписка на все события формы
        /// </summary>
        private void SubscribeToEvents()
        {
            // Подписка на события
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            button2.Click += btnAdd_Click;
            button3.Click += btnEdit_Click;
            dataGridView1.KeyDown += DataGridView1_KeyDown;
            button1.Click += button1_Click;

            // Обработчик ввода в поле ФИО с валидацией и автоформатированием
            textBox2.KeyPress += (s, e) =>
            {
                // Сначала валидация (только русские буквы)
                UserValidator.ValidateRussianInput(e);

                // Затем автоформатирование (заглавные буквы) если ввод не запрещен
                if (!e.Handled)
                {
                    UserValidator.FormatFIOOnKeyPress(textBox2, e);
                }
            };

            // Обработчики для плейсхолдеров (аналогично справочникам)
            textBox2.Enter += (s, e) => TextBoxEnter(textBox2, "ФИО");
            textBox2.Leave += (s, e) => TextBoxLeave(textBox2, "ФИО");

            textBox3.Enter += (s, e) => TextBoxEnter(textBox3, "Логин");
            textBox3.Leave += (s, e) => TextBoxLeave(textBox3, "Логин");

            textBox4.Enter += (s, e) => TextBoxEnter(textBox4, "Пароль");
            textBox4.Leave += (s, e) => TextBoxLeave(textBox4, "Пароль");
        }

        /// <summary>
        /// Обработчик получения фокуса текстовым полем
        /// Очищает поле от плейсхолдера и настраивает отображение
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
        /// <param name="placeholder">Текст плейсхолдера</param>
        private void TextBoxEnter(TextBox textBox, string placeholder)
        {
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText;

                // Для поля пароля включаем маскирование звездочками
                if (placeholder == "Пароль")
                    textBox4.PasswordChar = '*';
            }
        }

        /// <summary>
        /// Обработчик потери фокуса текстовым полем
        /// Восстанавливает плейсхолдер если поле пустое
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
        /// <param name="placeholder">Текст плейсхолдера</param>
        private void TextBoxLeave(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = SystemColors.GrayText;

                // Для поля пароля отключаем маскирование
                if (placeholder == "Пароль")
                    textBox4.PasswordChar = '\0';
            }
        }

        /// <summary>
        /// Обработчик изменения выделения в DataGridView
        /// Заполняет поля формы данными выбранного пользователя
        /// </summary>
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].DataBoundItem is User user)
            {
                selectedUser = user;

                // Форматируем ФИО для корректного отображения
                string formattedFIO = UserValidator.FormatFIO(user.FIO);
                textBox2.Text = formattedFIO;
                textBox2.ForeColor = SystemColors.WindowText;

                // Отображаем логин
                textBox3.Text = user.Login;
                textBox3.ForeColor = SystemColors.WindowText;

                // Отображаем пароль (с маскированием)
                textBox4.Text = user.Password;
                textBox4.ForeColor = SystemColors.WindowText;
                textBox4.PasswordChar = '*';

                // Устанавливаем соответствующую роль в выпадающем списке
                if (selectedUser.IDRole >= 1 && selectedUser.IDRole <= 3)
                {
                    comboBox3.SelectedIndex = selectedUser.IDRole - 1;
                }

                // Включаем кнопку редактирования
                button3.Enabled = true;
            }
            else
            {
                // Если выделение снято, очищаем поля
                ClearFormFields();
            }
        }

        /// <summary>
        /// Обработчик кнопки добавления нового пользователя
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Автоматическое форматирование ФИО перед валидацией
            if (textBox2.Text != "ФИО" && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = UserValidator.FormatFIO(textBox2.Text);
            }

            // Валидация формы
            if (!UserValidator.ValidateForm(textBox2, textBox3, textBox4, comboBox3))
                return;

            try
            {
                // Добавление пользователя через сервис
                if (_userService.AddUser(
                    textBox3.Text.Trim(),           // Логин
                    textBox4.Text,                   // Пароль
                    textBox2.Text.Trim(),            // ФИО
                    comboBox3.SelectedIndex + 1))    // ID роли (индекс + 1)
                {
                    MessageBox.Show("Пользователь успешно добавлен!", "Успех");

                    // Очистка полей и обновление списка
                    ClearFormFields();
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки редактирования пользователя
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Автоматическое форматирование ФИО перед валидацией
            if (textBox2.Text != "ФИО" && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = UserValidator.FormatFIO(textBox2.Text);
            }

            // Валидация формы
            if (!UserValidator.ValidateForm(textBox2, textBox3, textBox4, comboBox3))
                return;

            try
            {
                // Обновление данных пользователя через сервис
                if (_userService.UpdateUser(
                    selectedUser.UserID,              // ID пользователя
                    textBox3.Text.Trim(),             // Новый логин
                    textBox4.Text,                     // Новый пароль
                    textBox2.Text.Trim(),              // Новое ФИО
                    comboBox3.SelectedIndex + 1,       // Новая роль
                    selectedUser.Password))            // Старый пароль (для проверки изменений)
                {
                    MessageBox.Show("Данные пользователя успешно обновлены!", "Успех");

                    // Очистка полей и обновление списка
                    ClearFormFields();
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Отображение информации о текущем пользователе
        /// Формирует краткое ФИО с указанием роли
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (!string.IsNullOrEmpty(CurrentUser.FIO))
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');

                // Формирование краткого ФИО в зависимости от количества частей
                if (fioParts.Length >= 3)
                {
                    // Полное ФИО: Иванов Иван Иванович -> Иванов И.И.
                    string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                    label2.Text = $"{CurrentUser.RoleName.ToLower()} {shortName}";
                }
                else if (fioParts.Length == 2)
                {
                    // ФИО без отчества: Иванов Иван -> Иванов И.
                    string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.";
                    label2.Text = $"{CurrentUser.RoleName.ToLower()} {shortName}";
                }
                else
                {
                    // Только фамилия
                    label2.Text = $"{CurrentUser.RoleName.ToLower()} {CurrentUser.FIO}";
                }
            }
            else
            {
                label2.Text = "пользователь не авторизован";
            }
        }

        /// <summary>
        /// Обработчик нажатия клавиш в DataGridView
        /// Обрабатывает нажатие Delete для удаления пользователя
        /// </summary>
        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true; // Предотвращаем стандартное поведение
                DeleteSelectedUser();
            }
        }

        /// <summary>
        /// Удаление выбранного пользователя
        /// </summary>
        private void DeleteSelectedUser()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Внимание",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedUser = dataGridView1.SelectedRows[0].DataBoundItem as User;

            if (selectedUser == null)
                return;

            // Не позволяем удалить самого себя (текущего пользователя)
            if (selectedUser.UserID == CurrentUser.UserID)
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Подтверждение удаления
            DialogResult result = MessageBox.Show(
                $"Вы точно хотите удалить пользователя:\n{selectedUser.FIO} ({selectedUser.Login})?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2); // По умолчанию выбран "Нет"

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_userService.DeleteUser(selectedUser.UserID))
                    {
                        MessageBox.Show("Пользователь успешно удален!", "Успех",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFormFields();
                        LoadUsers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Обработчик кнопки возврата в главное меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainAdmin auto = new MainAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки очистки формы
        /// Сбрасывает все поля и снимает выделение
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            ClearFormFields();
            dataGridView1.ClearSelection();
        }

        /// <summary>
        /// Обработчик ввода в поле логина
        /// Разрешает только латинские буквы и цифры
        /// </summary>
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            UserValidator.ValidateEnglishInput(e);
        }

        /// <summary>
        /// Обработчик ввода в поле пароля
        /// Разрешает латинские буквы, цифры и специальные символы
        /// </summary>
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            UserValidator.ValidateEnglishInputPas(e);
        }
    }
}