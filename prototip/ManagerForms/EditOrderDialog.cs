using Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using DataTable = System.Data.DataTable;

namespace prototip.ManagerForms
{
    public partial class EditOrderDialog : Form
    {
        private int orderId;
        private int serviceId;
        private string serviceName;
        private int serviceDuration;
        private decimal servicePrice;
        private int maxPeople;
        private DateTime orderDate;
        private int currentStatusId;

        public EditOrderDialog(int orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            this.Load += EditOrderDialog_Load;

            // Валидация имени
            txtClientName.KeyPress += (s, e) =>
            {
                if (char.IsControl(e.KeyChar)) return;
                if (e.KeyChar == ' ') return;
                if ((e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'Ё' || e.KeyChar == 'ё') return;
                e.Handled = true;
            };

            // Маска телефона
            txtClientPhone.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Back) return;
                if (!char.IsDigit(e.KeyChar)) { e.Handled = true; return; }
                string digits = new string(txtClientPhone.Text.Where(char.IsDigit).ToArray());
                if (digits.Length >= 11) e.Handled = true;
            };
            txtClientPhone.TextChanged += (s, e) =>
            {
                int pos = txtClientPhone.SelectionStart;
                string digits = new string(txtClientPhone.Text.Where(char.IsDigit).ToArray());
                if (digits.StartsWith("7") || digits.StartsWith("8")) digits = digits.Substring(1);
                if (digits.Length > 10) digits = digits.Substring(0, 10);
                string fmt = "+7 ";
                if (digits.Length > 0) fmt += "(" + digits.Substring(0, Math.Min(3, digits.Length));
                if (digits.Length >= 4) fmt += ") " + digits.Substring(3, Math.Min(3, digits.Length - 3));
                if (digits.Length >= 7) fmt += "-" + digits.Substring(6, Math.Min(2, digits.Length - 6));
                if (digits.Length >= 9) fmt += "-" + digits.Substring(8, Math.Min(2, digits.Length - 8));
                txtClientPhone.Text = fmt;
                txtClientPhone.SelectionStart = Math.Min(pos, txtClientPhone.Text.Length);
            };

            // Пересчёт суммы при изменении количества
            nudParticipants.ValueChanged += (s, e) => UpdateTotalPrice();
        }

        private void EditOrderDialog_Load(object sender, EventArgs e)
        {
            LoadOrderData();
        }

        private void LoadOrderData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT o.*, s.Time, s.Price, s.MaxPeople 
                                     FROM orders o 
                                     JOIN services s ON o.ServiceID = s.Article 
                                     WHERE o.ID = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", orderId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serviceId = reader.GetInt32("ServiceID");
                            serviceName = reader.GetString("ServiceName");
                            serviceDuration = reader.GetInt32("Time");
                            servicePrice = reader.GetDecimal("Price");
                            maxPeople = reader.GetInt32("MaxPeople");
                            orderDate = reader.GetDateTime("StartTime").Date;
                            currentStatusId = reader.GetInt32("StatusID");

                            lblServiceInfo.Text = $"  {serviceName} |  Макс: {maxPeople} чел.";
                            lblDuration.Text = $"Длительность: {serviceDuration} мин";
                            dtpTime.Value = reader.GetDateTime("StartTime");
                            txtClientName.Text = reader["ClientName"].ToString();
                            txtClientPhone.Text = reader["ClientPhone"].ToString();
                            nudParticipants.Maximum = maxPeople;
                            nudParticipants.Value = reader.GetInt32("ParticipantsCount");

                            // Загружаем статусы в ComboBox
                            LoadStatuses();

                            if (currentStatusId == 3)
                            {
                                LockAllFields();
                            }

                            UpdateTotalPrice();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LockAllFields()
        {
            // Блокируем все поля редактирования
            txtClientName.Enabled = false;
            txtClientPhone.Enabled = false;
            dtpTime.Enabled = false;
            nudParticipants.Enabled = false;
            cmbStatus.Enabled = false;
            btnSave.Enabled = false;
            btnPrintReceipt.Enabled = false;
            btnPrintAgreement.Enabled = false;

            // Меняем заголовок формы
            this.Text = "Просмотр заказа (ОТМЕНЕН)";
        }

        private void LoadStatuses()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT StatusID, Name FROM statuses ORDER BY StatusID";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cmbStatus.DataSource = dt;
                    cmbStatus.DisplayMember = "Name";
                    cmbStatus.ValueMember = "StatusID";
                    cmbStatus.SelectedValue = currentStatusId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalPrice()
        {
            int participants = (int)nudParticipants.Value;
            if (participants > 0)
            {
                decimal total = servicePrice * participants;
                if (participants > 8) total *= 0.9m;
                txtTotalPrice.Text = total.ToString("N2") + " руб.";
            }
            else
            {
                txtTotalPrice.Text = "0 руб.";
            }
        }

        private bool IsTimeSlotAvailable(DateTime startTime, DateTime endTime, int excludeOrderId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT COUNT(*) FROM orders 
                        WHERE IsActive = 1 
                        AND DATE(StartTime) = @date
                        AND StartTime < @endTime 
                        AND EndTime > @startTime
                        AND ID != @excludeId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@startTime", startTime);
                    cmd.Parameters.AddWithValue("@endTime", endTime);
                    cmd.Parameters.AddWithValue("@date", startTime.Date);
                    cmd.Parameters.AddWithValue("@excludeId", excludeOrderId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки времени: {ex.Message}", "Ошибка БД",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Проверка имени
            if (string.IsNullOrWhiteSpace(txtClientName.Text))
            {
                MessageBox.Show("Введите имя клиента!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClientName.Focus();
                return;
            }

            // Проверка телефона
            string phoneDigits = new string(txtClientPhone.Text.Where(char.IsDigit).ToArray());
            if (phoneDigits.Length < 11)
            {
                MessageBox.Show("Введите полный номер телефона!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClientPhone.Focus();
                return;
            }

            // Проверка количества участников
            if ((int)nudParticipants.Value > maxPeople)
            {
                MessageBox.Show($"Максимальное количество участников: {maxPeople}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudParticipants.Focus();
                return;
            }

            if ((int)nudParticipants.Value < 1)
            {
                MessageBox.Show("Количество участников должно быть не менее 1!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudParticipants.Focus();
                return;
            }

            // Проверка времени
            TimeSpan start = dtpTime.Value.TimeOfDay;
            TimeSpan minTime = new TimeSpan(10, 0, 0);
            TimeSpan maxTime = new TimeSpan(22, 0, 0);

            if (start < minTime)
            {
                MessageBox.Show("Компания работает с 10:00. Выберите время не ранее 10:00.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                return;
            }

            if (start >= maxTime)
            {
                MessageBox.Show("Компания работает до 22:00. Выберите время до 22:00.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                return;
            }

            // Проверка, что квест завершится до 22:00
            TimeSpan endTime = start.Add(new TimeSpan(0, serviceDuration, 0));
            if (endTime > maxTime)
            {
                MessageBox.Show($"Квест длится {serviceDuration} мин и завершится в {endTime:hh\\:mm}.\n" +
                               $"Компания работает до 22:00. Выберите более раннее время.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpTime.Focus();
                return;
            }

            // Проверка на изменение статуса с "Отменен" на "Выполнен"
            int newStatusId = (int)cmbStatus.SelectedValue;
            if (currentStatusId == 2 && newStatusId == 1)
            {
                MessageBox.Show("Нельзя изменить статус отмененного заказа на \"Выполнен\"!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на занятость времени
            DateTime newStart = orderDate.Add(start);
            DateTime newEnd = newStart.AddMinutes(serviceDuration);

            if (!IsTimeSlotAvailable(newStart, newEnd, orderId))
            {
                MessageBox.Show(
                    $"На выбранное время ({newStart:HH:mm} - {newEnd:HH:mm}) уже есть запись!\n" +
                    "Выберите другое время.",
                    "Время занято",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                dtpTime.Focus();
                return;
            }

            // Сохранение изменений
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    decimal? total = null;
                    if ((int)nudParticipants.Value > 0)
                    {
                        total = servicePrice * (int)nudParticipants.Value;
                        if ((int)nudParticipants.Value > 8)
                            total *= 0.9m;
                    }

                    string query = @"UPDATE orders SET 
                        StartTime = @st, 
                        EndTime = @et, 
                        ClientName = @cn, 
                        ClientPhone = @cp,
                        ParticipantsCount = @pc, 
                        TotalPrice = @tp, 
                        StatusID = @sid 
                        WHERE ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@st", newStart);
                    cmd.Parameters.AddWithValue("@et", newEnd);
                    cmd.Parameters.AddWithValue("@cn", txtClientName.Text.Trim());
                    cmd.Parameters.AddWithValue("@cp", txtClientPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@pc", (int)nudParticipants.Value);
                    cmd.Parameters.AddWithValue("@tp", (object)total ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sid", newStatusId);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Изменения успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrintReceipt_Click(object sender, EventArgs e)
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                wordApp = new Word.Application();
                doc = wordApp.Documents.Add();
                wordApp.Visible = true;

                doc.PageSetup.TopMargin = 15;
                doc.PageSetup.BottomMargin = 15;
                doc.PageSetup.LeftMargin = 15;
                doc.PageSetup.RightMargin = 15;

                string managerShortName = GetCurrentManagerName();

                Word.Paragraph allText = doc.Paragraphs.Add();
                allText.Range.Text =
                    "─────────────────────────────────\n" +
                    "          КАССОВЫЙ ЧЕК\n" +
                    "         ООО \"Квестиум\"\n" +
                    $"Чек №{orderId} от {DateTime.Now:dd.MM.yy HH:mm}\n" +
                    $"Кассир: {managerShortName}\n" +
                    "─────────────────────────────────\n" +
                    $"\nУслуга: {serviceName}\n" +
                    $"Клиент: {txtClientName.Text.Trim()}\n" +
                    $"Дата квеста: {orderDate:dd.MM.yy HH:mm}\n" +
                    $"\nКол-во участников: {nudParticipants.Value}\n" +
                    $"Стоимость без скидки: {servicePrice * nudParticipants.Value:C}\n" +
                    $"Скидка: {(nudParticipants.Value > 8 ? $"-{servicePrice * nudParticipants.Value * 0.1m:C}" : "0%")}\n" +
                    $"ИТОГО: {txtTotalPrice.Text}\n" +
                    "─────────────────────────────────\n" +
                    $"\nПодпись: ___________________\n" +
                    $"\n*Чек для бухгалтерии";

                allText.Range.Font.Name = "Courier New";
                allText.Range.Font.Size = 11;
                allText.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                string fileName = $"Чек_{orderId}_{DateTime.Now:yyyyMMdd_HHmm}.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                doc.SaveAs(filePath);
                MessageBox.Show($"Чек сохранен: {filePath}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseWordDocument(doc, wordApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void BtnPrintAgreement_Click(object sender, EventArgs e)
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                wordApp = new Word.Application();
                doc = wordApp.Documents.Add();
                wordApp.Visible = true;

                doc.PageSetup.TopMargin = 50;
                doc.PageSetup.BottomMargin = 40;
                doc.PageSetup.LeftMargin = 60;
                doc.PageSetup.RightMargin = 40;

                // Заголовок
                Word.Paragraph title = doc.Paragraphs.Add();
                title.Range.Text = "ДОГОВОР СОГЛАСИЯ НА УЧАСТИЕ В КВЕСТЕ";
                title.Range.Font.Name = "Times New Roman";
                title.Range.Font.Size = 14;
                title.Range.Font.Bold = 1;
                title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.ParagraphFormat.SpaceAfter = 15;
                title.Range.InsertParagraphAfter();

                // Номер и дата
                Word.Paragraph header = doc.Paragraphs.Add();
                header.Range.Text = $"Договор № {orderId} от {DateTime.Now:dd.MM.yyyy} г.";
                header.Range.Font.Name = "Times New Roman";
                header.Range.Font.Size = 12;
                header.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                header.Range.ParagraphFormat.SpaceAfter = 20;
                header.Range.InsertParagraphAfter();

                // Город
                Word.Paragraph city = doc.Paragraphs.Add();
                city.Range.Text = "г. Москва";
                city.Range.Font.Name = "Times New Roman";
                city.Range.Font.Size = 12;
                city.Range.ParagraphFormat.SpaceAfter = 6;
                city.Range.InsertParagraphAfter();

                // Стороны
                Word.Paragraph parties = doc.Paragraphs.Add();
                parties.Range.Font.Name = "Times New Roman";
                parties.Range.Font.Size = 12;
                parties.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                parties.Range.ParagraphFormat.FirstLineIndent = 36;
                parties.Range.ParagraphFormat.SpaceAfter = 6;
                parties.Range.Text = $"ООО «Квестиум», именуемое в дальнейшем «Исполнитель», в лице директора Иванова И.И., с одной стороны, и группа участников в составе {nudParticipants.Value} человек, с другой стороны, заключили настоящий договор:";
                parties.Range.InsertParagraphAfter();

                // Разделы
                AddSectionCompact(doc, "1. ПРЕДМЕТ ДОГОВОРА",
                    "1.1. Исполнитель обязуется предоставить, а Участники обязуются оплатить услуги по организации и проведению квеста.");

                AddSectionCompact(doc, "2. УСЛОВИЯ ПРОВЕДЕНИЯ КВЕСТА",
                    "2.1. Наименование квеста: " + serviceName + "\n" +
                    "2.2. Дата и время: " + orderDate.ToString("dd.MM.yyyy в HH:mm") + "\n" +
                    "2.3. Количество участников: " + nudParticipants.Value + " человек\n" +
                    "2.4. Продолжительность: " + serviceDuration + " минут\n" +
                    "2.5. Адрес: г. Москва, ул. Примерная, д. 1");

                AddSectionCompact(doc, "3. ПРАВА И ОБЯЗАННОСТИ",
                    "3.1. Исполнитель обязуется обеспечить безопасность и предоставить оборудование.\n" +
                    "3.2. Участники обязуются соблюдать правила и не наносить ущерб имуществу.");

                AddSectionCompact(doc, "4. СТОИМОСТЬ УСЛУГ",
                    "4.1. Стоимость: " + txtTotalPrice.Text + " (включая НДС 20%).\n" +
                    "4.2. Оплата производится не менее чем за 24 часа до начала.");

                AddSectionCompact(doc, "5. ОТВЕТСТВЕННОСТЬ",
                    "5.1. Исполнитель не несет ответственности за личные вещи участников.\n" +
                    "5.2. Участники несут ответственность за ущерб имуществу Исполнителя.");

                AddSectionCompact(doc, "6. ЗАКЛЮЧИТЕЛЬНЫЕ ПОЛОЖЕНИЯ",
                    "6.1. Договор вступает в силу с момента подписания.\n" +
                    "6.2. Споры решаются путем переговоров.\n" +
                    "6.3. Договор составлен в двух экземплярах.");

                // Подписи
                Word.Paragraph signTitle = doc.Paragraphs.Add();
                signTitle.Range.Text = "ПОДПИСИ СТОРОН";
                signTitle.Range.Font.Name = "Times New Roman";
                signTitle.Range.Font.Size = 12;
                signTitle.Range.Font.Bold = 1;
                signTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                signTitle.Range.ParagraphFormat.SpaceBefore = 20;
                signTitle.Range.ParagraphFormat.SpaceAfter = 20;
                signTitle.Range.InsertParagraphAfter();

                // Таблица подписей
                int participantsCount = (int)nudParticipants.Value;
                int totalRows = participantsCount + 1; // +1 для Исполнителя

                Word.Paragraph tblPar = doc.Paragraphs.Add();
                Word.Table signTable = doc.Tables.Add(tblPar.Range, totalRows, 3);
                signTable.Borders.Enable = 1;
                signTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                signTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                signTable.Range.Font.Name = "Times New Roman";
                signTable.Range.Font.Size = 12;

                // Заголовки таблицы
                signTable.Cell(1, 1).Range.Text = "Сторона";
                signTable.Cell(1, 1).Range.Font.Bold = 1;
                signTable.Cell(1, 1).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray15;

                signTable.Cell(1, 2).Range.Text = "ФИО";
                signTable.Cell(1, 2).Range.Font.Bold = 1;
                signTable.Cell(1, 2).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray15;

                signTable.Cell(1, 3).Range.Text = "Подпись";
                signTable.Cell(1, 3).Range.Font.Bold = 1;
                signTable.Cell(1, 3).Shading.BackgroundPatternColor = Word.WdColor.wdColorGray15;

                // Строка Исполнителя
                signTable.Cell(2, 1).Range.Text = "Исполнитель:";
                signTable.Cell(2, 1).Range.Font.Bold = 1;
                signTable.Cell(2, 2).Range.Text = "Иванов И.И.";
                signTable.Cell(2, 3).Range.Text = "________________";

                // Строки для участников
                for (int i = 0; i < participantsCount; i++)
                {
                    int row = i + 3; // Начинаем с 3 строки (после заголовка и исполнителя)

                    signTable.Cell(row, 1).Range.Text = $"Участник {i + 1}:";
                    signTable.Cell(row, 1).Range.Font.Bold = 1;
                    signTable.Cell(row, 2).Range.Text = "____________________________";
                    signTable.Cell(row, 3).Range.Text = "________________";
                }

                // Настройка ширины столбцов
                signTable.Columns[1].Width = 120;
                signTable.Columns[2].Width = 250;
                signTable.Columns[3].Width = 150;

                // Выравнивание в ячейках
                for (int row = 1; row <= totalRows; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        signTable.Cell(row, col).VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        if (col == 3)
                            signTable.Cell(row, col).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

                // Примечание
                Word.Paragraph note = doc.Paragraphs.Add();
                note.Range.Text = "\n* Каждый участник обязан подписать договор лично";
                note.Range.Font.Name = "Times New Roman";
                note.Range.Font.Size = 10;
                note.Range.Font.Italic = 1;
                note.Range.ParagraphFormat.SpaceBefore = 10;

                string fileName = $"Договор_{orderId}_{DateTime.Now:yyyyMMdd_HHmm}.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                doc.SaveAs(filePath);
                MessageBox.Show($"Договор сохранен: {filePath}", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseWordDocument(doc, wordApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private string GetCurrentManagerName()
        {
            if (CurrentUser.FIO != null)
            {
                string[] parts = CurrentUser.FIO.Split(' ');
                if (parts.Length >= 3)
                    return $"{parts[0]} {parts[1][0]}.{parts[2][0]}.";
                else if (parts.Length == 2)
                    return $"{parts[0]} {parts[1][0]}.";
                else
                    return CurrentUser.FIO;
            }
            return "Иванов И.И.";
        }

        private void AddSection(Word.Document doc, string title, string content)
        {
            Word.Paragraph sectionTitle = doc.Paragraphs.Add();
            sectionTitle.Range.Text = title;
            sectionTitle.Range.Font.Name = "Times New Roman";
            sectionTitle.Range.Font.Size = 12;
            sectionTitle.Range.Font.Bold = 1;
            sectionTitle.Range.ParagraphFormat.SpaceBefore = 16;
            sectionTitle.Range.ParagraphFormat.SpaceAfter = 8;
            sectionTitle.Range.InsertParagraphAfter();

            Word.Paragraph sectionContent = doc.Paragraphs.Add();
            sectionContent.Range.Text = content;
            sectionContent.Range.Font.Name = "Times New Roman";
            sectionContent.Range.Font.Size = 12;
            sectionContent.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            sectionContent.Range.ParagraphFormat.FirstLineIndent = 35;
            sectionContent.Range.ParagraphFormat.SpaceAfter = 4;
            sectionContent.Range.InsertParagraphAfter();
        }

        private void AddSectionCompact(Word.Document doc, string title, string content)
        {
            Word.Paragraph sectionTitle = doc.Paragraphs.Add();
            sectionTitle.Range.Text = title;
            sectionTitle.Range.Font.Name = "Times New Roman";
            sectionTitle.Range.Font.Size = 12;
            sectionTitle.Range.Font.Bold = 1;
            sectionTitle.Range.ParagraphFormat.SpaceBefore = 12;
            sectionTitle.Range.ParagraphFormat.SpaceAfter = 4;
            sectionTitle.Range.InsertParagraphAfter();

            Word.Paragraph sectionContent = doc.Paragraphs.Add();
            sectionContent.Range.Text = content;
            sectionContent.Range.Font.Name = "Times New Roman";
            sectionContent.Range.Font.Size = 12;
            sectionContent.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            sectionContent.Range.ParagraphFormat.FirstLineIndent = 35;
            sectionContent.Range.ParagraphFormat.SpaceAfter = 4;
            sectionContent.Range.InsertParagraphAfter();
        }

        private void CloseWordDocument(Word.Document doc, Word.Application wordApp)
        {
            if (doc != null)
            {
                try
                {
                    object saveChanges = false;
                    object originalFormat = Word.WdSaveFormat.wdFormatDocument;
                    object routeDocument = false;
                    doc.Close(ref saveChanges, ref originalFormat, ref routeDocument);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                }
                catch { }
            }
            if (wordApp != null)
            {
                try
                {
                    wordApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                }
                catch { }
            }
        }
    }
}