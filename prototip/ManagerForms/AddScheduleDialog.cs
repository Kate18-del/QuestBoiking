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

namespace prototip.ManagerForms
{
    public partial class AddScheduleDialog : Form
    {
        public DateTime SelectedDate { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int ServiceDuration { get; set; }
        public decimal ServicePrice { get; set; }
        public int MaxPeople { get; set; }

        public TimeSpan StartTime => dtpTime.Value.TimeOfDay;
        public string ClientName => txtClientName.Text.Trim();
        public string ClientPhone => txtClientPhone.Text.Trim();
        public int ParticipantsCount => (int)nudParticipants.Value;

        public AddScheduleDialog()
        {
            InitializeComponent();
            this.Load += AddScheduleDialog_Load;
            dtpTime.MinDate = DateTime.Today.AddHours(10);
            dtpTime.MaxDate = DateTime.Today.AddHours(22);

            // Валидация имени — только русские буквы и пробелы
            txtClientName.KeyPress += TxtClientName_KeyPress;

            // Маска телефона
            txtClientPhone.KeyPress += TxtClientPhone_KeyPress;
            txtClientPhone.TextChanged += TxtClientPhone_TextChanged;
            txtClientPhone.Text = "+7 (";
            txtClientPhone.SelectionStart = txtClientPhone.Text.Length;
        }
        private void AddScheduleDialog_Load(object sender, EventArgs e)
        {
            lblServiceInfo.Text = $"  {ServiceName}\n" +
                $"  Макс: {MaxPeople} чел.  |  Стоимость: {ServicePrice} руб.";
            lblDuration.Text = $"Длительность: {ServiceDuration} мин";
        }

        /// <summary>
        /// Валидация имени — только русские буквы, пробел и Backspace
        /// </summary>
        private void TxtClientName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем управляющие клавиши (Backspace)
            if (char.IsControl(e.KeyChar))
                return;

            // Проверяем, русская ли буква
            if ((e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'Ё' || e.KeyChar == 'ё')
                return;

            // Проверка на пробел — максимум 2 пробела
            if (e.KeyChar == ' ')
            {
                // Считаем количество пробелов в текущем тексте
                int spaceCount = txtClientName.Text.Count(c => c == ' ');

                // Если уже есть 2 пробела — блокируем
                if (spaceCount >= 2)
                {
                    e.Handled = true;
                    return;
                }

                // Не разрешаем пробел в начале строки
                if (txtClientName.Text.Length == 0 || txtClientName.SelectionStart == 0)
                {
                    e.Handled = true;
                    return;
                }

                // Не разрешаем два пробела подряд
                if (txtClientName.SelectionStart > 0 && txtClientName.Text[txtClientName.SelectionStart - 1] == ' ')
                {
                    e.Handled = true;
                    return;
                }

                return;
            }

            // Всё остальное блокируем
            e.Handled = true;
        }

        /// <summary>
        /// Маска телефона +7 (XXX) XXX-XX-XX
        /// </summary>
        private void TxtClientPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем Backspace
            if (e.KeyChar == (char)Keys.Back)
                return;

            // Только цифры
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Ограничение длины — максимум 11 цифр (+7 и 10 цифр номера)
            string digits = new string(txtClientPhone.Text.Where(char.IsDigit).ToArray());
            if (digits.Length >= 11)
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Автоматическое форматирование телефона при вводе
        /// </summary>
        private void TxtClientPhone_TextChanged(object sender, EventArgs e)
        {
            // Если текст пустой или только начинает вводиться
            if (string.IsNullOrEmpty(txtClientPhone.Text))
            {
                txtClientPhone.Text = "+7 (";
                txtClientPhone.SelectionStart = txtClientPhone.Text.Length;
                return;
            }

            // Сохраняем позицию курсора
            int cursorPos = txtClientPhone.SelectionStart;
            int oldLength = txtClientPhone.Text.Length;

            // Оставляем только цифры
            string digits = new string(txtClientPhone.Text.Where(char.IsDigit).ToArray());

            // Если нет цифр вообще
            if (digits.Length == 0)
            {
                txtClientPhone.TextChanged -= TxtClientPhone_TextChanged;
                txtClientPhone.Text = "+7 (";
                txtClientPhone.SelectionStart = txtClientPhone.Text.Length;
                txtClientPhone.TextChanged += TxtClientPhone_TextChanged;
                return;
            }

            // Убираем первую 7, если она есть (код страны уже добавлен)
            if (digits.StartsWith("7"))
            {
                digits = digits.Substring(1);
            }
            else if (digits.StartsWith("8"))
            {
                digits = digits.Substring(1);
            }

            // Ограничиваем 10 цифрами (остальные цифры номера после кода страны)
            if (digits.Length > 10)
            {
                digits = digits.Substring(0, 10);
            }

            // Форматируем
            string formatted = "+7 ";
            if (digits.Length > 0)
                formatted += "(" + digits.Substring(0, Math.Min(3, digits.Length));
            if (digits.Length >= 4)
                formatted += ") " + digits.Substring(3, Math.Min(3, digits.Length - 3));
            if (digits.Length >= 7)
                formatted += "-" + digits.Substring(6, Math.Min(2, digits.Length - 6));
            if (digits.Length >= 9)
                formatted += "-" + digits.Substring(8, Math.Min(2, digits.Length - 8));

            // Отключаем обработчик, чтобы избежать рекурсии
            txtClientPhone.TextChanged -= TxtClientPhone_TextChanged;
            txtClientPhone.Text = formatted;

            // Восстанавливаем позицию курсора
            // Если текст стал длиннее, сдвигаем курсор вправо
            if (formatted.Length > oldLength)
            {
                txtClientPhone.SelectionStart = cursorPos + (formatted.Length - oldLength);
            }
            else
            {
                txtClientPhone.SelectionStart = Math.Min(cursorPos, txtClientPhone.Text.Length);
            }

            // Включаем обработчик обратно
            txtClientPhone.TextChanged += TxtClientPhone_TextChanged;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // Проверка заполнения всех полей
            if (string.IsNullOrWhiteSpace(ClientName))
            {
                MessageBox.Show("Введите имя клиента!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClientName.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Проверка формата телефона (должен быть полностью заполнен)
            string phoneDigits = new string(txtClientPhone.Text.Where(char.IsDigit).ToArray());
            if (phoneDigits.Length < 11) // +7 и 10 цифр номера
            {
                MessageBox.Show("Введите полный номер телефона!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClientPhone.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Проверка на максимальное количество человек
            if (ParticipantsCount > MaxPeople)
            {
                MessageBox.Show($"Максимальное количество человек: {MaxPeople}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudParticipants.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Проверка, что количество участников больше 0
            if (ParticipantsCount < 1)
            {
                MessageBox.Show("Количество участников должно быть не менее 1!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudParticipants.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Проверка времени начала (не раньше 10:00 и не позже 22:00)
            TimeSpan startTime = dtpTime.Value.TimeOfDay;
            TimeSpan minTime = new TimeSpan(10, 0, 0);  // 10:00
            TimeSpan maxTime = new TimeSpan(22, 0, 0);  // 22:00

            if (startTime < minTime)
            {
                MessageBox.Show("Компания работает с 10:00. Выберите время не ранее 10:00.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (startTime >= maxTime)
            {
                MessageBox.Show("Компания работает до 22:00. Выберите время до 22:00.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Проверка, что квест завершится до 22:00
            TimeSpan endTime = startTime.Add(new TimeSpan(0, ServiceDuration, 0));
            if (endTime > maxTime)
            {
                MessageBox.Show($"Квест длится {ServiceDuration} мин и завершится в {endTime:hh\\:mm}.\n" +
                               $"Компания работает до 22:00. Выберите более раннее время.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Проверка для сегодняшней даты — время не должно быть в прошлом
            if (SelectedDate.Date == DateTime.Now.Date)
            {
                TimeSpan now = DateTime.Now.TimeOfDay;
                TimeSpan selectedTime = dtpTime.Value.TimeOfDay;

                if (selectedTime <= now)
                {
                    MessageBox.Show("Нельзя создать запись на прошедшее время сегодня!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpTime.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            // Проверка на занятость времени в базе данных
            if (!IsTimeSlotAvailable())
            {
                DateTime slotStartTime = SelectedDate.Date.Add(startTime);
                DateTime slotEndTime = slotStartTime.AddMinutes(ServiceDuration);

                MessageBox.Show(
                    $"На выбранное время ({slotStartTime:HH:mm} - {slotEndTime:HH:mm}) уже есть запись!\n" +
                    "Выберите другое время.",
                    "Время занято",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                dtpTime.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            // Всё ок
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Проверяет, свободен ли выбранный временной слот
        /// </summary>
        private bool IsTimeSlotAvailable()
        {
            DateTime startTime = SelectedDate.Date.Add(StartTime);
            DateTime endTime = startTime.AddMinutes(ServiceDuration);

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = @"
            SELECT COUNT(*) FROM orders 
            WHERE IsActive = 1 
            AND DATE(StartTime) = @date
            AND StartTime < @endTime 
            AND EndTime > @startTime";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@startTime", startTime);
                cmd.Parameters.AddWithValue("@endTime", endTime);
                cmd.Parameters.AddWithValue("@date", startTime.Date);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0;
            }
        }
    }
}
