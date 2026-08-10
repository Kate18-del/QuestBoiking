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
    public partial class ScheduleForm : Form
    {
        public ScheduleForm()
        {
            InitializeComponent();
            LoadServices();
            DisplayCurrentUser();
            monthCalendar.SetDate(DateTime.Now);
            LoadSchedule(DateTime.Now);

            monthCalendar.MaxDate = DateTime.Now.Date.AddDays(14); // Не дальше 2 недель
            monthCalendar.SetDate(DateTime.Now);

            LoadSchedule(DateTime.Now);
        }

        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] parts = CurrentUser.FIO.Split(' ');
                if (parts.Length >= 3)
                    lblUser.Text = $"Менеджер {parts[0]} {parts[1][0]}.{parts[2][0]}.";
            }
        }

        private void LoadServices()
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(
                    "SELECT Article, Name, Time, Price, MaxPeople FROM services ORDER BY Name", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cmbService.DataSource = dt;
                cmbService.DisplayMember = "Name";
                cmbService.ValueMember = "Article";
            }
        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            LoadSchedule(e.Start);
        }

        private void LoadSchedule(DateTime date)
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = @"
                    SELECT o.ID, o.ServiceName, TIME(o.StartTime) as TimeSlot,
                           o.MaxPeople, o.ParticipantsCount, o.ClientName, o.ClientPhone,
                           s.Name as StatusName, o.TotalPrice, o.StartTime, o.EndTime
                    FROM orders o
                    LEFT JOIN statuses s ON o.StatusID = s.StatusID
                    WHERE DATE(o.StartTime) = @date AND o.IsActive = 1
                    ORDER BY o.StartTime";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@date", date.Date);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dt.Columns.Add("FreeSlots", typeof(int));
                foreach (DataRow row in dt.Rows)
                    row["FreeSlots"] = Convert.ToInt32(row["MaxPeople"]) - Convert.ToInt32(row["ParticipantsCount"]);

                // Маскировка персональных данных
                foreach (DataRow row in dt.Rows)
                {
                    row["ClientName"] = MaskName(row["ClientName"].ToString());
                    row["ClientPhone"] = MaskPhone(row["ClientPhone"].ToString());
                }

                dgvSchedule.DataSource = null;
                dgvSchedule.DataSource = dt;

                dgvSchedule.Columns["ID"].Visible = false;
                dgvSchedule.Columns["ServiceName"].HeaderText = "Квест";
                dgvSchedule.Columns["TimeSlot"].HeaderText = "Время";
                dgvSchedule.Columns["TimeSlot"].DefaultCellStyle.Format = "hh\\:mm"; // Убираем секунды
                dgvSchedule.Columns["MaxPeople"].HeaderText = "Всего мест";
                dgvSchedule.Columns["ParticipantsCount"].Visible = false;
                dgvSchedule.Columns["FreeSlots"].Visible = false;
                dgvSchedule.Columns["ClientName"].HeaderText = "Клиент";
                dgvSchedule.Columns["ClientPhone"].HeaderText = "Телефон";
                dgvSchedule.Columns["StatusName"].HeaderText = "Статус";
                dgvSchedule.Columns["TotalPrice"].HeaderText = "Сумма";
                dgvSchedule.Columns["TotalPrice"].DefaultCellStyle.Format = "0.##' руб.'";
                dgvSchedule.Columns["StartTime"].Visible = false;
                dgvSchedule.Columns["EndTime"].Visible = false;

                lblSchedule.Text = $"Расписание на {date:dd.MM.yyyy}:";
                lblRecordCount.Text = $"Записей: {dt.Rows.Count}";

                // Устанавливаем шрифт Comic Sans MS, размер 14
                Font comicSans14 = new Font("Comic Sans MS", 14F);
                Font comicSans14Bold = new Font("Comic Sans MS", 12F, FontStyle.Bold);

                dgvSchedule.DefaultCellStyle.Font = comicSans14;
                dgvSchedule.ColumnHeadersDefaultCellStyle.Font = comicSans14Bold;
            }
        }

        // Маскирование имени (Иванов И.И.)
        private string MaskName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            string[] parts = name.Split(' ');
            string result = parts[0]; // Фамилия полностью
            if (parts.Length >= 2) result += " " + parts[1][0] + "."; // Имя - первая буква с точкой
            if (parts.Length >= 3) result += " " + parts[2][0] + "."; // Отчество - первая буква с точкой
            return result;
        }

        // Маскирование телефона (+7 *** **-XXXX)
        private string MaskPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length < 7) return phone;
            string digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length >= 11) return "+7 *** **-" + digits.Substring(digits.Length - 4);
            return phone;
        }


        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (cmbService.SelectedValue == null)
            {
                MessageBox.Show("Выберите квест!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка даты
            DateTime selectedDate = monthCalendar.SelectionStart.Date;
            DateTime today = DateTime.Now.Date;
            DateTime maxDate = today.AddDays(14); // Максимум +2 недели вперёд

            // Нельзя добавлять в прошлое
            if (selectedDate < today)
            {
                MessageBox.Show("Нельзя добавить запись на прошедшую дату!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Нельзя добавлять на сегодня, если время уже прошло (проверим в диалоге)
            // Нельзя добавлять дальше чем на 2 недели
            if (selectedDate > maxDate)
            {
                MessageBox.Show($"Запись можно добавить не позднее {maxDate:dd.MM.yyyy} (2 недели вперёд).", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)cmbService.SelectedItem;
            int serviceDuration = Convert.ToInt32(row["Time"]); // Длительность квеста в минутах

            using (var dialog = new AddScheduleDialog())
            {
                dialog.SelectedDate = monthCalendar.SelectionStart;
                dialog.ServiceID = (int)cmbService.SelectedValue;
                dialog.ServiceName = row["Name"].ToString();
                dialog.ServiceDuration = serviceDuration;
                dialog.ServicePrice = Convert.ToDecimal(row["Price"]);
                dialog.MaxPeople = Convert.ToInt32(row["MaxPeople"]);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Проверка времени уже выполнена в диалоге
                    // Просто создаем заказ
                    CreateOrder(dialog);
                    LoadSchedule(monthCalendar.SelectionStart);
                }
            }
        }

        private void CreateOrder(AddScheduleDialog d)
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                DateTime start = d.SelectedDate.Date.Add(d.StartTime);
                DateTime end = start.AddMinutes(d.ServiceDuration);

                // Убираем секунды
                start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0);
                end = new DateTime(end.Year, end.Month, end.Day, end.Hour, end.Minute, 0);

                decimal? total = null;
                if (d.ParticipantsCount > 0)
                {
                    total = d.ServicePrice * d.ParticipantsCount;
                    if (d.ParticipantsCount > 8) total *= 0.9m;
                }

                string query = @"INSERT INTO orders 
                    (ServiceID, ServiceName, StartTime, EndTime, ClientName, ClientPhone,
                     StatusID, UserID, ParticipantsCount, MaxPeople, TotalPrice, DateOfAdmission)
                    VALUES (@sid, @sn, @st, @et, @cn, @cp, 1, @uid, @pc, @mp, @tp, NOW())";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", d.ServiceID);
                cmd.Parameters.AddWithValue("@sn", d.ServiceName);
                cmd.Parameters.AddWithValue("@st", start);
                cmd.Parameters.AddWithValue("@et", end);
                cmd.Parameters.AddWithValue("@cn", d.ClientName);
                cmd.Parameters.AddWithValue("@cp", d.ClientPhone);
                cmd.Parameters.AddWithValue("@uid", CurrentUser.UserID);
                cmd.Parameters.AddWithValue("@pc", d.ParticipantsCount);
                cmd.Parameters.AddWithValue("@mp", d.MaxPeople);
                cmd.Parameters.AddWithValue("@tp", (object)total ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainManager mainManager = new MainManager();
            mainManager.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvSchedule.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для редактирования!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvSchedule.SelectedRows[0].Cells["ID"].Value);

            using (EditOrderDialog dialog = new EditOrderDialog(orderId))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Обновляем данные в DataGridView
                    LoadSchedule(monthCalendar.SelectionStart);

                    // Дополнительно обновляем интерфейс
                    dgvSchedule.Refresh();
                }
            }
        }
    }
}