using MySql.Data.MySqlClient;
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
    /// Форма для оформления нового заказа менеджером
    /// Предоставляет интеллектуальный поиск клиентов и услуг, проверку доступности дат
    /// </summary>
    public partial class MakingOrder : Form
    {
        // Текущий номер заказа (счетчик) - для отображения, но не используется в создании
        private int orderNumber = 0;

        // Подключение к базе данных
        private MySqlConnection connection;

        // Идентификаторы выбранных клиента и услуги
        private int currentClientID = 0;
        private int currentServiceArticle = 0;

        // Элементы для автодополнения (выпадающие списки)
        private ListBox listBoxClients;      // Список клиентов для автодополнения
        private ListBox listBoxServices;     // Список услуг для автодополнения

        // Данные для автодополнения
        private DataTable clientsData;        // Таблица с данными клиентов
        private DataTable servicesData;       // Таблица с данными услуг

        // Для хранения информации о выбранной услуге
        private int selectedServiceDayOfWeek = 0;    // День недели проведения квеста
        private int selectedServiceDuration = 0;     // Длительность квеста в минутах

        // Флаг для отслеживания состояния даты
        private bool isCurrentDateAvailable = true;

        /// <summary>
        /// Конструктор формы оформления заказа
        /// </summary>
        public MakingOrder()
        {
            InitializeComponent();

            // Инициализация подключения к БД
            InitializeDatabaseConnection();

            // Инициализация списков автодополнения
            InitializeAutoCompleteLists();

            // Загрузка текущего номера заказа
            LoadOrderNumber();

            // Инициализация элементов формы
            InitializeForm();

            // Отображение информации о текущем менеджере
            DisplayCurrentUser();
        }

        /// <summary>
        /// Отображение информации о текущем менеджере
        /// Формирует краткое ФИО в формате "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                label2.Text = $"менеджер {shortName}";
            }
        }

        /// <summary>
        /// Инициализация подключения к базе данных
        /// </summary>
        private void InitializeDatabaseConnection()
        {
            connection = new MySqlConnection(DatabaseConfig.ConnectionString);
        }

        /// <summary>
        /// Инициализация выпадающих списков для автодополнения
        /// Создает скрытые ListBox для отображения результатов поиска
        /// </summary>
        private void InitializeAutoCompleteLists()
        {
            // Создаем ListBox для клиентов
            listBoxClients = new ListBox();
            listBoxClients.Visible = false;
            listBoxClients.Font = new Font("Comic Sans MS", 9.75F);
            listBoxClients.BorderStyle = BorderStyle.FixedSingle;
            listBoxClients.SelectedIndexChanged += ListBoxClients_SelectedIndexChanged;
            listBoxClients.KeyDown += ListBoxClients_KeyDown;
            this.groupBox1.Controls.Add(listBoxClients);
            listBoxClients.BringToFront();

            // Создаем ListBox для услуг
            listBoxServices = new ListBox();
            listBoxServices.Visible = false;
            listBoxServices.Font = new Font("Comic Sans MS", 9.75F);
            listBoxServices.BorderStyle = BorderStyle.FixedSingle;
            listBoxServices.SelectedIndexChanged += ListBoxServices_SelectedIndexChanged;
            listBoxServices.KeyDown += ListBoxServices_KeyDown;
            this.groupBox1.Controls.Add(listBoxServices);
            listBoxServices.BringToFront();

            // Загружаем данные для автодополнения
            LoadClientsData();
            LoadServicesData();
        }

        /// <summary>
        /// Загрузка данных клиентов из базы данных
        /// </summary>
        private void LoadClientsData()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // SQL-запрос для получения ФИО и телефона клиентов
                string query = "SELECT ClientID, CONCAT(LastName, ' ', FirstName, ' ', COALESCE(Surname, '')) as FullName, PhoneNumber FROM clients ORDER BY LastName, FirstName";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                clientsData = new DataTable();
                adapter.Fill(clientsData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Загрузка данных услуг из базы данных
        /// </summary>
        private void LoadServicesData()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                string query = "SELECT Article, Name, Price, Time, DayOfTheWeek FROM services ORDER BY Name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                servicesData = new DataTable();
                adapter.Fill(servicesData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки услуг: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Загрузка следующего номера заказа
        /// Определяется как MAX(ID) + 1 из таблицы orders
        /// </summary>
        private void LoadOrderNumber()
        {
            try
            {
                connection.Open();
                string query = "SELECT MAX(ID) FROM orders";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    orderNumber = Convert.ToInt32(result) + 1;
                }
                else
                {
                    orderNumber = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки номера заказа: " + ex.Message);
                orderNumber = 1;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Инициализация элементов формы
        /// Настройка дат, полей ввода и подписок на события
        /// </summary>
        private void InitializeForm()
        {
            // Дата оформления - текущая системная дата (только для чтения)
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker1.Enabled = false;

            // Дата выполнения - завтра
            dateTimePicker2.Value = DateTime.Now.AddDays(1);
            dateTimePicker2.MinDate = DateTime.Now;

            // Подписываемся на событие изменения даты
            dateTimePicker2.ValueChanged += DateTimePicker2_ValueChanged;

            // Установка статуса "В работе" (StatusID = 1)
            textBox1.Text = "В работе";
            textBox1.Enabled = false;

            // Настройка текстовых полей с плейсхолдерами
            InitializeSearchTextBoxes();
        }

        /// <summary>
        /// Обработчик изменения даты выполнения
        /// Проверяет доступность выбранной даты
        /// </summary>
        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            // Проверяем доступность выбранной даты при ее изменении
            if (currentServiceArticle != 0)
            {
                CheckDateAvailability(dateTimePicker2.Value);
            }
        }

        /// <summary>
        /// Проверка доступности даты
        /// Если дата занята, предлагает следующую доступную
        /// </summary>
        private void CheckDateAvailability(DateTime selectedDate)
        {
            if (IsDateBooked(selectedDate))
            {
                isCurrentDateAvailable = false;
                // Показываем предупреждение только если дата изменилась и занята
                if (dateTimePicker2.Focused)
                {
                    MessageBox.Show($"Дата {selectedDate:dd.MM.yyyy} уже занята! Выберите другую дату.",
                        "Дата недоступна", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Предлагаем следующую доступную дату
                    SuggestNextAvailableDate();
                }
            }
            else
            {
                isCurrentDateAvailable = true;
            }
        }

        /// <summary>
        /// Настройка полей поиска с плейсхолдерами
        /// </summary>
        private void InitializeSearchTextBoxes()
        {
            // Поле ФИО клиента
            textBox2.Enter += (sender, e) => {
                if (textBox2.Text == "ФИО")
                {
                    textBox2.Text = "";
                    textBox2.ForeColor = SystemColors.WindowText;
                }
            };

            textBox2.Leave += (sender, e) => {
                if (string.IsNullOrWhiteSpace(textBox2.Text) && !listBoxClients.Focused)
                {
                    textBox2.Text = "ФИО";
                    textBox2.ForeColor = SystemColors.ScrollBar;
                    HideClientsList();
                }
            };

            textBox2.KeyDown += TextBox2_KeyDown;

            // Поле Услуга
            textBox4.Enter += (sender, e) => {
                if (textBox4.Text == "Услуга")
                {
                    textBox4.Text = "";
                    textBox4.ForeColor = SystemColors.WindowText;
                }
            };

            textBox4.Leave += (sender, e) => {
                if (string.IsNullOrWhiteSpace(textBox4.Text) && !listBoxServices.Focused)
                {
                    textBox4.Text = "Услуга";
                    textBox4.ForeColor = SystemColors.ScrollBar;
                    HideServicesList();
                }
            };

            textBox4.KeyDown += TextBox4_KeyDown;
        }

        #region Поиск и автодополнение клиентов

        /// <summary>
        /// Живой поиск клиента по номеру телефона или ФИО
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == "ФИО")
            {
                HideClientsList();
                currentClientID = 0;
                return;
            }

            // Фильтруем клиентов по номеру телефона или ФИО
            var filteredClients = clientsData.AsEnumerable()
                .Where(row => row["PhoneNumber"].ToString().Contains(searchText) ||
                             row["FullName"].ToString().ToLower().Contains(searchText.ToLower()))
                .Take(10) // Ограничиваем количество результатов
                .ToList();

            if (filteredClients.Count > 0)
            {
                ShowClientsList(filteredClients);
            }
            else
            {
                HideClientsList();
            }
        }

        /// <summary>
        /// Отображение списка найденных клиентов
        /// </summary>
        private void ShowClientsList(List<DataRow> clients)
        {
            listBoxClients.Items.Clear();
            foreach (DataRow row in clients)
            {
                listBoxClients.Items.Add($"{row["FullName"]} ({row["PhoneNumber"]}) - ID:{row["ClientID"]}");
            }

            listBoxClients.Visible = true;
            listBoxClients.BringToFront();
            listBoxClients.Size = new Size(textBox2.Width, Math.Min(200, clients.Count * listBoxClients.Font.Height + 4));
            listBoxClients.Location = new Point(textBox2.Location.X, textBox2.Location.Y + textBox2.Height);
        }

        /// <summary>
        /// Скрытие списка клиентов
        /// </summary>
        private void HideClientsList()
        {
            listBoxClients.Visible = false;
        }

        /// <summary>
        /// Выбор клиента из списка
        /// </summary>
        private void ListBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxClients.SelectedIndex >= 0)
            {
                string selectedItem = listBoxClients.SelectedItem.ToString();
                // Извлекаем ID клиента из строки вида "... - ID:123"
                int startId = selectedItem.LastIndexOf("ID:") + 3;
                if (startId > 0 && int.TryParse(selectedItem.Substring(startId), out int clientId))
                {
                    currentClientID = clientId;
                    // Устанавливаем ФИО в поле (без ID)
                    int endIndex = selectedItem.LastIndexOf(" - ID:");
                    textBox2.Text = selectedItem.Substring(0, endIndex);
                    textBox2.ForeColor = SystemColors.WindowText;
                }
                HideClientsList();
            }
        }

        /// <summary>
        /// Обработка нажатий клавиш в списке клиентов
        /// </summary>
        private void ListBoxClients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListBoxClients_SelectedIndexChanged(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideClientsList();
                textBox2.Focus();
            }
        }

        /// <summary>
        /// Обработка нажатий клавиш в поле поиска клиента
        /// </summary>
        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && listBoxClients.Visible && listBoxClients.Items.Count > 0)
            {
                listBoxClients.Focus();
                listBoxClients.SelectedIndex = 0;
            }
        }

        #endregion

        #region Поиск и автодополнение услуг

        /// <summary>
        /// Живой поиск услуг по названию
        /// </summary>
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(searchText) || searchText == "Услуга")
            {
                HideServicesList();
                currentServiceArticle = 0;
                return;
            }

            // Фильтруем услуги по началу названия
            var filteredServices = servicesData.AsEnumerable()
                .Where(row => row["Name"].ToString().StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .ToList();

            if (filteredServices.Count > 0)
            {
                ShowServicesList(filteredServices);
            }
            else
            {
                HideServicesList();
            }
        }

        /// <summary>
        /// Отображение списка найденных услуг
        /// </summary>
        private void ShowServicesList(List<DataRow> services)
        {
            listBoxServices.Items.Clear();
            foreach (DataRow row in services)
            {
                int dayOfWeek = Convert.ToInt32(row["DayOfTheWeek"]);
                string dayName = GetDayOfWeekName(dayOfWeek);
                listBoxServices.Items.Add($"{row["Name"]} - {Convert.ToDecimal(row["Price"]):C} ({dayName}, {row["Time"]} мин) (Арт:{row["Article"]})");
            }

            listBoxServices.Visible = true;
            listBoxServices.BringToFront();
            listBoxServices.Size = new Size(textBox4.Width, Math.Min(200, services.Count * listBoxServices.Font.Height + 4));
            listBoxServices.Location = new Point(textBox4.Location.X, textBox4.Location.Y + textBox4.Height);
        }

        /// <summary>
        /// Получение названия дня недели по номеру
        /// </summary>
        private string GetDayOfWeekName(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "Понедельник";
                case 2: return "Вторник";
                case 3: return "Среда";
                case 4: return "Четверг";
                case 5: return "Пятница";
                case 6: return "Суббота";
                case 7: return "Воскресенье";
                default: return "Любой день";
            }
        }

        /// <summary>
        /// Скрытие списка услуг
        /// </summary>
        private void HideServicesList()
        {
            listBoxServices.Visible = false;
        }

        /// <summary>
        /// Выбор услуги из списка
        /// </summary>
        private void ListBoxServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxServices.SelectedIndex >= 0)
            {
                string selectedItem = listBoxServices.SelectedItem.ToString();
                // Извлекаем артикул услуги из строки
                int startId = selectedItem.LastIndexOf("(Арт:") + 5;
                int endId = selectedItem.LastIndexOf(")");
                if (startId > 0 && endId > startId &&
                    int.TryParse(selectedItem.Substring(startId, endId - startId), out int article))
                {
                    currentServiceArticle = article;

                    // Получаем информацию о выбранной услуге
                    var serviceRow = servicesData.AsEnumerable()
                        .FirstOrDefault(row => Convert.ToInt32(row["Article"]) == article);

                    if (serviceRow != null)
                    {
                        selectedServiceDayOfWeek = Convert.ToInt32(serviceRow["DayOfTheWeek"]);
                        selectedServiceDuration = Convert.ToInt32(serviceRow["Time"]);

                        // Обновляем dateTimePicker2 с учетом дня недели услуги
                        UpdateDueDateBasedOnDayOfWeek();
                    }

                    // Устанавливаем название услуги в поле (без цены и артикула)
                    int endIndex = selectedItem.LastIndexOf(" - ");
                    textBox4.Text = selectedItem.Substring(0, endIndex);
                    textBox4.ForeColor = SystemColors.WindowText;
                }
                HideServicesList();
            }
        }

        /// <summary>
        /// Обновление даты выполнения с учетом дня недели услуги
        /// </summary>
        private void UpdateDueDateBasedOnDayOfWeek()
        {
            if (selectedServiceDayOfWeek == 0) return;

            DateTime currentDate = DateTime.Now.Date;
            DateTime dueDate;

            // Если DayOfTheWeek = 30, это означает "любой день"
            if (selectedServiceDayOfWeek == 30)
            {
                dueDate = currentDate.AddDays(1); // Завтра

                // Проверяем, не занят ли завтрашний день
                if (IsDateBooked(dueDate))
                {
                    dueDate = GetNextAvailableDate(currentDate.AddDays(2));
                }
            }
            else
            {
                dueDate = GetNextDayOfWeek(currentDate, selectedServiceDayOfWeek);

                // Проверяем, не занят ли этот день
                if (IsDateBooked(dueDate))
                {
                    dueDate = GetNextAvailableDayOfWeek(dueDate, selectedServiceDayOfWeek);
                }
            }

            // Устанавливаем дату
            dateTimePicker2.Value = dueDate;

            // Проверяем доступность установленной даты
            CheckDateAvailability(dueDate);
        }

        /// <summary>
        /// Получение следующей даты с указанным днем недели
        /// </summary>
        private DateTime GetNextDayOfWeek(DateTime startDate, int targetDayOfWeek)
        {
            int daysToAdd = ((targetDayOfWeek - (int)startDate.DayOfWeek + 7) % 7);
            if (daysToAdd == 0) daysToAdd = 7; // Если сегодня нужный день, берем следующий
            return startDate.AddDays(daysToAdd);
        }

        /// <summary>
        /// Получение следующей доступной даты с указанным днем недели
        /// </summary>
        private DateTime GetNextAvailableDayOfWeek(DateTime startDate, int targetDayOfWeek)
        {
            DateTime currentDate = startDate;
            int maxAttempts = 28; // Максимум 4 недели вперед

            for (int i = 1; i <= maxAttempts; i++)
            {
                currentDate = GetNextDayOfWeek(currentDate, targetDayOfWeek);
                if (!IsDateBooked(currentDate))
                {
                    return currentDate;
                }
            }

            return startDate; // Если не нашли свободный день, возвращаем исходную дату
        }

        /// <summary>
        /// Получение следующей доступной даты
        /// </summary>
        private DateTime GetNextAvailableDate(DateTime startDate)
        {
            DateTime currentDate = startDate;
            int maxAttempts = 30; // Максимум 30 дней вперед

            for (int i = 0; i < maxAttempts; i++)
            {
                if (!IsDateBooked(currentDate))
                {
                    return currentDate;
                }
                currentDate = currentDate.AddDays(1);
            }

            return startDate;
        }

        /// <summary>
        /// Проверка, занята ли дата (есть ли заказ на эту дату)
        /// </summary>
        private bool IsDateBooked(DateTime date)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // Проверяем, есть ли уже заказ на эту дату (по DueDate)
                string query = @"
                    SELECT COUNT(*) FROM orders 
                    WHERE DATE(DueDate) = DATE(@SelectedDate)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SelectedDate", date.Date);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка проверки доступности даты: " + ex.Message);
                return true; // В случае ошибки считаем дату занятой
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Обработка нажатий клавиш в списке услуг
        /// </summary>
        private void ListBoxServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListBoxServices_SelectedIndexChanged(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideServicesList();
                textBox4.Focus();
            }
        }

        /// <summary>
        /// Обработка нажатий клавиш в поле поиска услуги
        /// </summary>
        private void TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && listBoxServices.Visible && listBoxServices.Items.Count > 0)
            {
                listBoxServices.Focus();
                listBoxServices.SelectedIndex = 0;
            }
        }

        #endregion

        /// <summary>
        /// Обработчик кнопки возврата в главное меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainManager auto = new MainManager();
            auto.ShowDialog();
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки создания заказа
        /// Проверяет все поля и создает заказ в БД
        /// </summary>
        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            // Проверяем, заполнены ли обязательные поля
            if (currentClientID == 0)
            {
                MessageBox.Show("Выберите клиента!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            if (currentServiceArticle == 0)
            {
                MessageBox.Show("Выберите услугу!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text) || textBox3.Text == "Количество человек")
            {
                MessageBox.Show("Укажите количество человек!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }

            if (!int.TryParse(textBox3.Text, out int participants) || participants <= 0)
            {
                MessageBox.Show("Введите корректное количество человек!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }

            // Проверка ограничения на 25 человек
            if (participants > 25)
            {
                MessageBox.Show("Количество человек не может превышать 25!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                textBox3.SelectAll();
                return;
            }

            // Проверяем доступность выбранной даты
            if (IsDateBooked(dateTimePicker2.Value))
            {
                MessageBox.Show("На выбранную дату уже существует заказ! Пожалуйста, выберите другую дату.",
                    "Дата занята", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Предлагаем следующую доступную дату
                SuggestNextAvailableDate();
                return;
            }

            // Создаем заказ
            int newOrderId = 0;

            try
            {
                connection.Open();

                // Получаем цену услуги
                string getPriceQuery = "SELECT Price FROM services WHERE Article = @Article";
                MySqlCommand getPriceCmd = new MySqlCommand(getPriceQuery, connection);
                getPriceCmd.Parameters.AddWithValue("@Article", currentServiceArticle);
                decimal servicePrice = Convert.ToDecimal(getPriceCmd.ExecuteScalar());

                // Рассчитываем общую цену
                decimal totalPrice = servicePrice * participants;

                // Вставляем заказ
                string insertQuery = @"
                    INSERT INTO orders 
                    (ClientID, Article, DateOfAdmission, DueDate, StatusID, ParticipantsCount, TotalPrice, UserID) 
                    VALUES 
                    (@ClientID, @Article, @DateOfAdmission, @DueDate, @StatusID, @ParticipantsCount, @TotalPrice, @UserID)";

                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@ClientID", currentClientID);
                cmd.Parameters.AddWithValue("@Article", currentServiceArticle);
                cmd.Parameters.AddWithValue("@DateOfAdmission", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@DueDate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@StatusID", 1); // "В работе" 
                cmd.Parameters.AddWithValue("@ParticipantsCount", participants);
                cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                cmd.Parameters.AddWithValue("@UserID", CurrentUser.UserID); // Добавляем текущего пользователя

                cmd.ExecuteNonQuery();

                // Получаем ID созданного заказа
                newOrderId = (int)cmd.LastInsertedId;

                MessageBox.Show($"Заказ №{newOrderId} успешно создан!\nОбщая стоимость: {totalPrice:C}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Открываем форму просмотра заказа с переданным ID
                this.Visible = false;
                ViewingOrderManager viewOrderForm = new ViewingOrderManager(newOrderId);
                viewOrderForm.ShowDialog();
                this.Visible = true;

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания заказа: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// Предложение следующей доступной даты
        /// </summary>
        private void SuggestNextAvailableDate()
        {
            DateTime currentDate = dateTimePicker2.Value.Date;
            DateTime nextAvailableDate = GetNextAvailableDate(currentDate.AddDays(1));

            if (nextAvailableDate != currentDate)
            {
                DialogResult result = MessageBox.Show(
                    $"Выбранная дата {currentDate:dd.MM.yyyy} занята.\n" +
                    $"Следующая доступная дата: {nextAvailableDate:dd.MM.yyyy}\n\n" +
                    $"Перейти к этой дате?",
                    "Доступная дата",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dateTimePicker2.Value = nextAvailableDate;
                }
            }
            else
            {
                MessageBox.Show("Не удалось найти свободную дату в ближайшее время.",
                    "Нет свободных дат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Очистка формы после создания заказа
        /// </summary>
        private void ClearForm()
        {
            // Очищаем поля
            textBox2.Text = "ФИО";
            textBox2.ForeColor = SystemColors.ScrollBar;
            textBox4.Text = "Услуга";
            textBox4.ForeColor = SystemColors.ScrollBar;
            textBox3.Text = "Количество человек";
            textBox3.ForeColor = SystemColors.ScrollBar;

            currentClientID = 0;
            currentServiceArticle = 0;
            selectedServiceDayOfWeek = 0;
            selectedServiceDuration = 0;

            // Скрываем индикатор доступности
            label3.Visible = false;

            // Обновляем номер заказа
            LoadOrderNumber();

            // Сбрасываем даты
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now.AddDays(1);
        }

        /// <summary>
        /// Обработчик кнопки перехода к управлению клиентами
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Clients auto = new Clients();
            auto.ShowDialog();
            this.Visible = true;
        }

        #region Обработчики для поля количества человек

        /// <summary>
        /// Обработчик получения фокуса полем количества человек
        /// </summary>
        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Количество человек")
            {
                textBox3.Text = "";
                textBox3.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем количества человек
        /// </summary>
        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = "Количество человек";
                textBox3.ForeColor = SystemColors.ScrollBar;
            }
        }

        /// <summary>
        /// Ограничение ввода в поле количества человек только цифрами
        /// </summary>
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        #endregion

        /// <summary>
        /// Ограничение ввода в поле поиска клиента только цифрами
        /// </summary>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Ограничение ввода в поле поиска услуги только русскими буквами
        /// </summary>
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
                return;

            // Проверяем, является ли символ русской буквой
            bool isRussianLetter = (e.KeyChar >= 'а' && e.KeyChar <= 'я') ||
                                   (e.KeyChar >= 'А' && e.KeyChar <= 'Я') ||
                                   e.KeyChar == 'ё' || e.KeyChar == 'Ё';

            // Если не русская буква - блокируем
            if (!isRussianLetter)
            {
                e.Handled = true;
            }
        }
    }
}