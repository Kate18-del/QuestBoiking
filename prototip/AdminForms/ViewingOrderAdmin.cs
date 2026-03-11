using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
    /// Форма для просмотра и редактирования деталей конкретного заказа
    /// Доступна для администраторов и позволяет изменять статус заказа
    /// </summary>
    
    public partial class ViewingOrderAdmin : Form
    {
        // Идентификатор просматриваемого заказа
        private int orderId;

        /// <summary>
        /// Конструктор формы просмотра заказа
        /// </summary>
        /// <param name="orderId">ID заказа для отображения</param>
        public ViewingOrderAdmin(int orderId)
        {
            this.orderId = orderId;
            InitializeComponent();

            // Загрузка деталей заказа
            LoadOrderDetails();

            // Отображение информации о текущем пользователе
            DisplayCurrentUser();
        }

        /// <summary>
        /// Загрузка всех деталей заказа из базы данных
        /// Включает информацию о клиенте, услуге, статусе и расчет стоимости
        /// </summary>
        private void LoadOrderDetails()
        {
            // Загрузка доступных статусов в выпадающий список
            LoadStatuses();

            try
            {
                // SQL-запрос для получения полной информации о заказе
                string query = @"
    SELECT 
        o.ID,
        CONCAT(c.LastName, ' ', c.FirstName, ' ', c.Surname) as ClientName,
        c.PhoneNumber as ClientPhone,  
        s.Name as QuestName,
        s.Price,
        o.DateOfAdmission,
        st.Name as StatusName,
        COALESCE(o.ParticipantsCount, 1) as ParticipantsCount,
        COALESCE(o.TotalPrice, s.Price * COALESCE(o.ParticipantsCount, 1)) as TotalPrice
    FROM orders o
    LEFT JOIN clients c ON o.ClientID = c.ClientID
    LEFT JOIN services s ON o.Article = s.Article
    LEFT JOIN statuses st ON o.StatusID = st.StatusID
    WHERE o.ID = @orderId";

                // Подключение к базе данных
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    // Выполнение запроса и чтение результатов
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Безопасное преобразование данных с помощью Convert
                            // Заполнение полей формы полученными данными
                            textBox2.Text = Convert.ToString(reader["ClientName"]);      // ФИО клиента

                            // ИСПРАВЛЕНО: Загрузка телефона
                            if (reader["ClientPhone"] != DBNull.Value)
                            {
                                textBoxPhone.Text = Convert.ToString(reader["ClientPhone"]); // Телефон клиента
                            }
                            else
                            {
                                textBoxPhone.Text = "Не указан";
                            }

                            textBox4.Text = Convert.ToString(reader["QuestName"]);        // Название квеста
                            dateTimePicker1.Value = Convert.ToDateTime(reader["DateOfAdmission"]); // Дата начала

                            // Установка статуса в выпадающем списке
                            comboBox2.Text = Convert.ToString(reader["StatusName"]);

                            // Количество участников (по умолчанию 1)
                            int participants = Convert.ToInt32(reader["ParticipantsCount"]);
                            textBox3.Text = participants.ToString();

                            // Стоимость услуг
                            decimal price = Convert.ToDecimal(reader["Price"]);           // Цена за одного
                            decimal totalPrice = Convert.ToDecimal(reader["TotalPrice"]); // Общая стоимость

                            // Обновление расчетов стоимости и скидок
                            UpdatePriceLabels(price, totalPrice, participants);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обновление отображения цен и расчет скидок
        /// Автоматически применяет скидку при большом количестве участников
        /// </summary>
        /// <param name="basePrice">Базовая цена за одного участника</param>
        /// <param name="totalPrice">Общая стоимость заказа</param>
        /// <param name="participants">Количество участников</param>
        private void UpdatePriceLabels(decimal basePrice, decimal totalPrice, int participants)
        {
            // Базовая цена за всех участников (без скидки)
            decimal baseTotal = basePrice * participants;

            // Автоматическая скидка 10% если больше 8 участников
            decimal discountPercent = 0;
            if (participants > 8)
            {
                discountPercent = 10; // 10% скидка
            }

            // Расчет суммы скидки
            decimal discountAmount = baseTotal * (discountPercent / 100);
            decimal finalPrice = baseTotal - discountAmount;

            // Обновление текстовых меток с информацией о ценах
            label6.Text = $"Сумма заказа без учета скидки - {baseTotal:N0} руб.";      // Сумма без скидки
            label8.Text = $"Скидка - {discountPercent:F0} %";                           // Процент скидки
            label7.Text = $"Сумма заказа с учетом скидки - {finalPrice:N0} руб.";       // Сумма со скидкой
            label9.Text = $"Общая сумма заказа - {finalPrice:N0} руб.";                 // Итоговая сумма
        }

        /// <summary>
        /// Отображение информации о текущем пользователе (администраторе)
        /// Формирует краткое ФИО в формате "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                label10.Text = $"администратор {shortName}";
            }
        }

        /// <summary>
        /// Обработчик кнопки возврата к списку заказов
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            OrderAccountingAdmin auto = new OrderAccountingAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки изменения статуса заказа
        /// Обновляет статус заказа в базе данных
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // SQL-запрос для обновления статуса заказа
                    string updateQuery = "UPDATE orders SET StatusID = @statusId WHERE ID = @orderId";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);

                    // Определение ID статуса по его названию
                    int statusId = GetStatusId(comboBox2.Text);
                    cmd.Parameters.AddWithValue("@statusId", statusId);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    // Выполнение обновления
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Статус заказа успешно обновлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Установка результата диалога и закрытие формы
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении заказа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Получение ID статуса по его названию
        /// </summary>
        /// <param name="statusName">Название статуса</param>
        /// <returns>ID статуса в базе данных</returns>
        private int GetStatusId(string statusName)
        {
            switch (statusName)
            {
                case "В работе":
                    return 1;
                case "Выполнен":
                    return 2;
                case "Отменен":
                    return 3;
                default:
                    return 1; // По умолчанию "В работе"
            }
        }

        /// <summary>
        /// Загрузка доступных статусов в выпадающий список
        /// </summary>
        private void LoadStatuses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT Name FROM statuses", conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox2.Items.Clear();
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Обработчики событий текстовых полей (плейсхолдеры)

        /// <summary>
        /// Обработчик получения фокуса полем ФИО
        /// Очищает поле от плейсхолдера
        /// </summary>
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "ФИО")
            {
                textBox2.Text = "";
                textBox2.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ФИО
        /// Восстанавливает плейсхолдер если поле пустое
        /// </summary>
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "ФИО";
                textBox2.ForeColor = SystemColors.ScrollBar;
            }
        }

        /// <summary>
        /// Обработчик получения фокуса полем услуги
        /// Очищает поле от плейсхолдера
        /// </summary>
        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                textBox4.Text = "Услуга";
                textBox4.ForeColor = SystemColors.ScrollBar;
            }
        }

        /// <summary>
        /// Обработчик получения фокуса полем количества участников
        /// Очищает поле от плейсхолдера
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
        /// Обработчик потери фокуса полем количества участников
        /// Восстанавливает плейсхолдер если поле пустое
        /// </summary>
        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = "Количество человек";
                textBox3.ForeColor = SystemColors.ScrollBar;
            }
        }

        #endregion
    }
}