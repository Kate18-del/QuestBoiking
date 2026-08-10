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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace prototip
{
    /// <summary>
    /// Форма для просмотра деталей заказа и печати документов (чека и договора согласия)
    /// Доступна для менеджеров после создания заказа
    /// </summary>
    public partial class ViewingOrderManager : Form
    {
        // Идентификатор просматриваемого заказа
        private int orderId;

        // Данные для расчета стоимости
        private decimal originalPrice;      // Исходная цена без скидки
        private decimal discount;           // Сумма скидки
        private decimal finalPrice;          // Итоговая цена со скидкой
        private int participantsCount;       // Количество участников

        /// <summary>
        /// Конструктор формы просмотра заказа
        /// </summary>
        /// <param name="orderId">ID заказа для отображения</param>
        public ViewingOrderManager(int orderId)
        {
            InitializeComponent();
            this.orderId = orderId;

            // Установка режима только для чтения для всех полей
            InitializeReadOnlyControls();

            // Подписка на события
            SubscribeToEvents();

            // Загрузка данных заказа
            LoadOrderData();

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
        /// Установка всех полей в режим только для чтения
        /// Менеджер не может редактировать заказ, только просматривать
        /// </summary>
        private void InitializeReadOnlyControls()
        {
            // Делаем все поля только для чтения
            textBox2.ReadOnly = true;   // ФИО клиента
            textBox3.ReadOnly = true;   // Количество участников
            textBox1.ReadOnly = true;   // Статус
            comboBox1.Enabled = false;  // Название услуги
            dateTimePicker1.Enabled = false; // Дата оформления
            dateTimePicker2.Enabled = false; // Дата выполнения

            // Убираем плейсхолдеры
            textBox2.ForeColor = SystemColors.WindowText;
            textBox3.ForeColor = SystemColors.WindowText;
        }

        /// <summary>
        /// Подписка на события кнопок
        /// </summary>
        private void SubscribeToEvents()
        {
            btnMenu.Click += btnMenu_Click;       // Возврат в меню
            button1.Click += button1_Click;       // Печать чека
            button2.Click += button2_Click;       // Печать договора согласия
        }

        /// <summary>
        /// Загрузка данных заказа из базы данных
        /// </summary>
        private void LoadOrderData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Загрузка данных заказа с присоединением связанных таблиц
                    string query = @"
                        SELECT 
                            o.ID,
                            o.DateOfAdmission,
                            o.DueDate,
                            o.ParticipantsCount,
                            o.TotalPrice,
                            o.StatusID,
                            s.Name as ServiceName,
                            s.Article as ServiceArticle,
                            s.Price as ServicePrice,
                            CONCAT(c.LastName, ' ', c.FirstName, ' ', COALESCE(c.Surname, '')) as ClientName,
                            c.PhoneNumber,
                            st.Name as StatusName
                        FROM orders o
                        JOIN services s ON o.Article = s.Article
                        JOIN clients c ON o.ClientID = c.ClientID
                        JOIN statuses st ON o.StatusID = st.StatusID
                        WHERE o.ID = @OrderId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Заполнение полей данными
                            textBox2.Text = reader["ClientName"].ToString();      // ФИО клиента
                            comboBox1.Text = reader["ServiceName"].ToString();    // Название услуги
                            dateTimePicker1.Value = Convert.ToDateTime(reader["DateOfAdmission"]); // Дата оформления
                            dateTimePicker2.Value = Convert.ToDateTime(reader["DueDate"]); // Дата выполнения
                            textBox3.Text = reader["ParticipantsCount"].ToString(); // Количество участников
                            textBox1.Text = reader["StatusName"].ToString();       // Статус заказа

                            // Сохраняем данные для расчета
                            participantsCount = Convert.ToInt32(reader["ParticipantsCount"]);
                            originalPrice = Convert.ToDecimal(reader["ServicePrice"]) * participantsCount;

                            // Расчет скидки и обновление меток
                            CalculateDiscount();
                            UpdatePriceLabels();

                            // Установка цвета статуса
                            SetStatusColor(reader["StatusName"].ToString());
                        }
                        else
                        {
                            MessageBox.Show("Заказ не найден!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных заказа: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Установка цвета текста статуса в зависимости от его значения
        /// </summary>
        private void SetStatusColor(string statusName)
        {
            switch (statusName.ToLower())
            {
                case "в работе":
                    textBox1.ForeColor = Color.Blue;
                    break;
                case "выполнен":
                    textBox1.ForeColor = Color.Green;
                    break;
                case "отменен":
                    textBox1.ForeColor = Color.Red;
                    break;
                default:
                    textBox1.ForeColor = Color.Black;
                    break;
            }
        }

        /// <summary>
        /// Расчет скидки (10% при количестве участников более 8)
        /// </summary>
        private void CalculateDiscount()
        {
            // Скидка 10% если более 8 человек
            discount = participantsCount > 8 ? originalPrice * 0.10m : 0;
            finalPrice = originalPrice - discount;
        }

        /// <summary>
        /// Настройка цветов меток для лучшей читаемости
        /// </summary>
        private void SetupPriceLabels()
        {
            label6.ForeColor = Color.DarkSlateGray;
            label7.ForeColor = Color.DarkSlateGray;
            label8.ForeColor = Color.DarkSlateGray;
            label9.ForeColor = Color.FromArgb(0, 102, 0); // Темно-зеленый

            // Жирный шрифт для итоговой суммы
            label9.Font = new Font(label9.Font, FontStyle.Bold);
        }

        /// <summary>
        /// Обновление меток с ценами
        /// </summary>
        private void UpdatePriceLabels()
        {
            label6.Text = $"Сумма заказа без учета скидки - {originalPrice:C}";
            label8.Text = $"Скидка - {(discount > 0 ? "10%" : "0%")}";
            label7.Text = $"Сумма заказа с учетом скидки - {(originalPrice - discount):C}";
            label9.Text = $"Общая сумма заказа - {finalPrice:C}";

            // Выделение скидки цветом
            if (discount > 0)
            {
                label8.ForeColor = Color.Red;
                label8.Font = new Font(label8.Font, FontStyle.Bold);
            }
        }

        /// <summary>
        /// Кнопка "Распечатать чек" - создает кассовый чек в Word
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            CreateReceipt();
        }

        /// <summary>
        /// Кнопка "Распечатать согласие" - создает договор согласия в Word
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            CreateConsentAgreement();
        }

        /// <summary>
        /// Создание кассового чека в формате Word
        /// </summary>
        public void CreateReceipt()
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                // Создание нового документа Word
                wordApp = new Word.Application();
                doc = wordApp.Documents.Add();
                wordApp.Visible = true;

                // МИНИМАЛЬНЫЕ ОТСТУПЫ для компактного чека
                doc.PageSetup.TopMargin = 15;
                doc.PageSetup.BottomMargin = 15;
                doc.PageSetup.LeftMargin = 15;
                doc.PageSetup.RightMargin = 15;

                // Получаем краткое имя менеджера
                string managerShortName = GetCurrentManagerName();

                // ВСЁ В ОДНОМ АБЗАЦЕ - компактный формат чека
                Word.Paragraph allText = doc.Paragraphs.Add();
                allText.Range.Text =
                    "─────────────────────────────────\n" +
                    "          КАССОВЫЙ ЧЕК\n" +
                    "     ООО \"Квестиум\"\n" +
                    $"Чек №{orderId} от {DateTime.Now:dd.MM.yy HH:mm}\n" +
                    $"Кассир: {managerShortName}\n" +
                    "─────────────────────────────────\n" +
                    $"\nУслуга: {comboBox1.Text}\n" +
                    $"Клиент: {textBox2.Text}\n" +
                    $"Дата квеста: {dateTimePicker2.Value:dd.MM.yy HH:mm}\n" +
                    $"\nКол-во участников: {participantsCount}\n" +
                    $"Стоимость без скидки: {originalPrice:C}\n" +
                    $"Скидка: {(discount > 0 ? $"-{discount:C}" : "0%")}\n" +
                    $"ИТОГО: {finalPrice:C}\n" +
                    "─────────────────────────────────\n" +
                    $"\nПодпись: ___________________\n" +
                    $"\n*Чек для бухгалтерии";

                // ЕДИНЫЙ СТИЛЬ - моноширинный шрифт для ровных колонок
                allText.Range.Font.Name = "Courier New";
                allText.Range.Font.Size = 11;
                allText.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

                // Сохранение документа
                string fileName = $"Чек_{orderId}_{DateTime.Now:yyyyMMdd_HHmm}.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                doc.SaveAs(filePath);
                MessageBox.Show($"Чек сохранен: {filePath}", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                // Правильное закрытие Word и освобождение COM-объектов
                CloseWordDocument(doc, wordApp);
            }
            // Принудительная сборка мусора для освобождения COM-объектов
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Получение краткого имени текущего менеджера для печати в чеке
        /// </summary>
        private string GetCurrentManagerName()
        {
            try
            {
                if (CurrentUser.FIO != null)
                {
                    string[] fioParts = CurrentUser.FIO.Split(' ');
                    if (fioParts.Length >= 3)
                    {
                        return $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                    }
                    else if (fioParts.Length == 2)
                    {
                        return $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.";
                    }
                    else
                    {
                        return CurrentUser.FIO;
                    }
                }
            }
            catch
            {
                // Если возникла ошибка, возвращаем значение по умолчанию
            }

            return "Иванов И.И."; // Значение по умолчанию
        }

        /// <summary>
        /// Создание договора согласия на участие в квесте
        /// </summary>
        /// <summary>
        /// Создание договора согласия на участие в квесте
        /// </summary>
        public void CreateConsentAgreement()
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                wordApp = new Word.Application();
                doc = wordApp.Documents.Add();
                wordApp.Visible = true;

                // Настройка страницы (уменьшенные отступы для экономии места)
                doc.PageSetup.TopMargin = 50;
                doc.PageSetup.BottomMargin = 40;
                doc.PageSetup.LeftMargin = 60;
                doc.PageSetup.RightMargin = 40;

                // ===== ЗАГОЛОВОК =====
                Word.Paragraph title = doc.Paragraphs.Add();
                title.Range.Text = "ДОГОВОР СОГЛАСИЯ НА УЧАСТИЕ В КВЕСТЕ";
                title.Range.Font.Name = "Times New Roman";
                title.Range.Font.Size = 14;
                title.Range.Font.Bold = 1;
                title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.ParagraphFormat.SpaceAfter = 15;
                title.Range.InsertParagraphAfter();

                // ===== Номер и дата =====
                Word.Paragraph header = doc.Paragraphs.Add();
                header.Range.Text = $"Договор № {orderId} от {DateTime.Now:dd.MM.yyyy} г.";
                header.Range.Font.Name = "Times New Roman";
                header.Range.Font.Size = 12;
                header.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                header.Range.ParagraphFormat.SpaceAfter = 20;
                header.Range.InsertParagraphAfter();

                // ===== Город =====
                Word.Paragraph city = doc.Paragraphs.Add();
                city.Range.Text = "г. Москва";
                city.Range.Font.Name = "Times New Roman";
                city.Range.Font.Size = 12;
                city.Range.ParagraphFormat.SpaceAfter = 6;
                city.Range.InsertParagraphAfter();

                // ===== СТОРОНЫ =====
                Word.Paragraph parties = doc.Paragraphs.Add();
                parties.Range.Font.Name = "Times New Roman";
                parties.Range.Font.Size = 12;
                parties.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                parties.Range.ParagraphFormat.FirstLineIndent = 36;
                parties.Range.ParagraphFormat.SpaceAfter = 6;
                parties.Range.Text = $"ООО «Квестиум», именуемое в дальнейшем «Исполнитель», в лице директора Иванова И.И., с одной стороны, и гражданин(ка) {textBox2.Text}, именуемый(ая) «Участник», с другой стороны, заключили настоящий договор:";
                parties.Range.InsertParagraphAfter();

                // ===== РАЗДЕЛЫ (компактные) =====
                AddSection(doc, "1. ПРЕДМЕТ ДОГОВОРА",
                    "1.1. Исполнитель обязуется предоставить, а Участник обязуется оплатить услуги по организации и проведению квеста.");

                AddSection(doc, "2. УСЛОВИЯ ПРОВЕДЕНИЯ КВЕСТА",
                    "2.1. Наименование квеста: " + comboBox1.Text + "\n" +
                    "2.2. Дата и время: " + dateTimePicker2.Value.ToString("dd.MM.yyyy в HH:mm") + "\n" +
                    "2.3. Количество участников: " + participantsCount + " человек\n" +
                    "2.4. Продолжительность: 60 минут\n" +
                    "2.5. Адрес: г. Москва, ул. Примерная, д. 1");

                AddSection(doc, "3. ПРАВА И ОБЯЗАННОСТИ",
                    "3.1. Исполнитель обязуется обеспечить безопасность и предоставить оборудование.\n" +
                    "3.2. Участник обязуется соблюдать правила и не наносить ущерб имуществу.");

                AddSection(doc, "4. СТОИМОСТЬ УСЛУГ",
                    "4.1. Стоимость: " + finalPrice.ToString("C") + " (включая НДС 20%).\n" +
                    "4.2. Оплата производится не менее чем за 24 часа до начала.");

                AddSection(doc, "5. ОТВЕТСТВЕННОСТЬ",
                    "5.1. Исполнитель не несет ответственности за личные вещи участников.\n" +
                    "5.2. Участник несет ответственность за ущерб имуществу Исполнителя.");

                AddSection(doc, "6. ПЕРСОНАЛЬНЫЕ ДАННЫЕ",
                    "6.1. Участник дает согласие на обработку персональных данных согласно ФЗ № 152-ФЗ.\n" +
                    "6.2. Исполнитель обязуется не передавать данные третьим лицам.");

                AddSection(doc, "7. ЗАКЛЮЧИТЕЛЬНЫЕ ПОЛОЖЕНИЯ",
                    "7.1. Договор вступает в силу с момента подписания.\n" +
                    "7.2. Споры решаются путем переговоров.\n" +
                    "7.3. Договор составлен в двух экземплярах.");

                // ===== ПОДПИСИ СТОРОН =====
                Word.Paragraph signTitle = doc.Paragraphs.Add();
                signTitle.Range.Text = "ПОДПИСИ СТОРОН";
                signTitle.Range.Font.Name = "Times New Roman";
                signTitle.Range.Font.Size = 12;
                signTitle.Range.Font.Bold = 1;
                signTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                signTitle.Range.ParagraphFormat.SpaceBefore = 20;
                signTitle.Range.ParagraphFormat.SpaceAfter = 15;
                signTitle.Range.InsertParagraphAfter();

                // Таблица подписей
                Word.Paragraph tblPar = doc.Paragraphs.Add();
                Word.Table signTable = doc.Tables.Add(tblPar.Range, 2, 3);
                signTable.Borders.Enable = 0;
                signTable.Range.Font.Name = "Times New Roman";
                signTable.Range.Font.Size = 12;

                signTable.Cell(1, 1).Range.Text = "Исполнитель:";
                signTable.Cell(1, 1).Range.Font.Bold = 1;
                signTable.Cell(1, 2).Range.Text = "ООО «Квестиум»";
                signTable.Cell(1, 3).Range.Text = "___________ /Иванов И.И./";

                signTable.Cell(2, 1).Range.Text = "Участник:";
                signTable.Cell(2, 1).Range.Font.Bold = 1;
                signTable.Cell(2, 2).Range.Text = textBox2.Text;
                signTable.Cell(2, 3).Range.Text = "___________ /" + textBox2.Text + "/";

                // Настройка ширины колонок
                signTable.Columns[1].Width = 100;
                signTable.Columns[2].Width = 200;
                signTable.Columns[3].Width = 200;

                // Сохранение
                string fileName = $"Договор_согласия_{orderId}_{DateTime.Now:yyyyMMdd_HHmm}.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                doc.SaveAs(filePath);
                MessageBox.Show($"Договор сохранен: {filePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка");
            }
            finally
            {
                CloseWordDocument(doc, wordApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Добавление раздела с заголовком и содержимым (нормальные отступы)
        /// </summary>
        private void AddSection(Word.Document doc, string title, string content)
        {
            // Заголовок раздела
            Word.Paragraph sectionTitle = doc.Paragraphs.Add();
            sectionTitle.Range.Text = title;
            sectionTitle.Range.Font.Name = "Times New Roman";
            sectionTitle.Range.Font.Size = 12;
            sectionTitle.Range.Font.Bold = 1;
            sectionTitle.Range.ParagraphFormat.SpaceBefore = 16;
            sectionTitle.Range.ParagraphFormat.SpaceAfter = 8;
            sectionTitle.Range.InsertParagraphAfter();

            // Содержимое
            Word.Paragraph sectionContent = doc.Paragraphs.Add();
            sectionContent.Range.Text = content;
            sectionContent.Range.Font.Name = "Times New Roman";
            sectionContent.Range.Font.Size = 12;
            sectionContent.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
            sectionContent.Range.ParagraphFormat.FirstLineIndent = 35;
            sectionContent.Range.ParagraphFormat.SpaceAfter = 4;
            sectionContent.Range.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpace1pt5;
            sectionContent.Range.InsertParagraphAfter();
        }


        /// <summary>
        /// Закрытие документа Word и освобождение COM-объектов
        /// </summary>
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
                    doc = null;
                }
                catch { }
            }

            if (wordApp != null)
            {
                try
                {
                    wordApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                    wordApp = null;
                }
                catch { }
            }
        }

        /// <summary>
        /// Обработчик кнопки возврата в главное меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainManager auto = new MainManager();
            auto.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Создание чека в PDF (для повторной печати)
        /// </summary>
        public void CreateReceiptPdf()
        {
            var excelApp = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"Чек_№{orderId}_{DateTime.Now:yyyyMMdd}.pdf";
                    saveDialog.Title = "Сохранить чек как PDF";

                    if (saveDialog.ShowDialog() != DialogResult.OK) return;

                    excelApp.Visible = false;
                    workbook = excelApp.Workbooks.Add();
                    worksheet = workbook.ActiveSheet;

                    int row = 1;
                    worksheet.Cells[row, 1] = "КАССОВЫЙ ЧЕК";
                    worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 3]].Merge();
                    worksheet.Cells[row, 1].Font.Bold = true;
                    worksheet.Cells[row, 1].Font.Size = 14;
                    worksheet.Cells[row, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    row += 2;

                    worksheet.Cells[row, 1] = "ООО \"Квестиум\"";
                    worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 3]].Merge();
                    row++;
                    worksheet.Cells[row, 1] = $"Чек №{orderId} от {DateTime.Now:dd.MM.yy HH:mm}";
                    worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 3]].Merge();
                    row++;
                    worksheet.Cells[row, 1] = $"Кассир: {GetCurrentManagerName()}";
                    worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 3]].Merge();
                    row += 2;

                    worksheet.Cells[row, 1] = new string('─', 40);
                    worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 3]].Merge();
                    row++;

                    worksheet.Cells[row, 1] = "Услуга:";
                    worksheet.Cells[row, 2] = comboBox1.Text;
                    row++;
                    worksheet.Cells[row, 1] = "Клиент:";
                    worksheet.Cells[row, 2] = textBox2.Text;
                    row++;
                    worksheet.Cells[row, 1] = "Дата квеста:";
                    worksheet.Cells[row, 2] = dateTimePicker2.Value.ToString("dd.MM.yy HH:mm");
                    row++;
                    worksheet.Cells[row, 1] = "Кол-во участников:";
                    worksheet.Cells[row, 2] = participantsCount.ToString();
                    row++;

                    if (discount > 0)
                    {
                        worksheet.Cells[row, 1] = "Скидка (10%):";
                        worksheet.Cells[row, 2] = $"-{discount:N0} руб.";
                        row++;
                    }

                    worksheet.Cells[row, 1] = "ИТОГО:";
                    worksheet.Cells[row, 2] = $"{finalPrice:N0} руб.";
                    worksheet.Cells[row, 2].Font.Bold = true;
                    row += 2;

                    worksheet.Cells[row, 1] = new string('─', 40);
                    worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[row, 3]].Merge();
                    row += 2;
                    worksheet.Cells[row, 1] = "Подпись: ___________________";
                    row += 2;
                    worksheet.Cells[row, 1] = "* Чек действителен для бухгалтерии";

                    worksheet.Columns[1].ColumnWidth = 18;
                    worksheet.Columns[2].ColumnWidth = 25;

                    worksheet.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, saveDialog.FileName);

                    workbook.Close(false);
                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                    System.Diagnostics.Process.Start(saveDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (excelApp != null) { excelApp.Quit(); System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp); }
            }
        }

        /// <summary>
        /// Создание договора в PDF (для повторной печати)
        /// </summary>
        public void CreateConsentAgreementPdf()
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveDialog.FileName = $"Договор_№{orderId}_{DateTime.Now:yyyyMMdd}.pdf";
                    saveDialog.Title = "Сохранить договор как PDF";

                    if (saveDialog.ShowDialog() != DialogResult.OK) return;

                    wordApp = new Word.Application();
                    wordApp.Visible = false;
                    doc = wordApp.Documents.Add();

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

                    // Стороны
                    Word.Paragraph city = doc.Paragraphs.Add();
                    city.Range.Font.Name = "Times New Roman";
                    city.Range.Font.Size = 12;
                    city.Range.ParagraphFormat.SpaceAfter = 6;
                    city.Range.Text = "г. Москва";
                    city.Range.InsertParagraphAfter();

                    Word.Paragraph p1 = doc.Paragraphs.Add();
                    p1.Range.Font.Name = "Times New Roman";
                    p1.Range.Font.Size = 12;
                    p1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    p1.Range.ParagraphFormat.FirstLineIndent = 36;
                    p1.Range.ParagraphFormat.SpaceAfter = 6;
                    p1.Range.Text = $"ООО «Квестиум», именуемое в дальнейшем «Исполнитель», в лице директора Иванова И.И., с одной стороны, и гражданин(ка) {textBox2.Text}, именуемый(ая) «Участник», с другой стороны, заключили настоящий договор:";
                    p1.Range.InsertParagraphAfter();

                    // Разделы
                    AddSection(doc, "1. ПРЕДМЕТ ДОГОВОРА",
                        "1.1. Исполнитель обязуется предоставить, а Участник обязуется оплатить услуги по организации и проведению квеста.");

                    AddSection(doc, "2. УСЛОВИЯ ПРОВЕДЕНИЯ КВЕСТА",
                        "2.1. Наименование квеста: " + comboBox1.Text + "\n" +
                        "2.2. Дата и время: " + dateTimePicker2.Value.ToString("dd.MM.yyyy в HH:mm") + "\n" +
                        "2.3. Количество участников: " + participantsCount + " человек\n" +
                        "2.4. Продолжительность: 60 минут\n" +
                        "2.5. Адрес: г. Москва, ул. Примерная, д. 1");

                    AddSection(doc, "3. ПРАВА И ОБЯЗАННОСТИ",
                        "3.1. Исполнитель обязуется обеспечить безопасность и предоставить оборудование.\n" +
                        "3.2. Участник обязуется соблюдать правила и не наносить ущерб имуществу.");

                    AddSection(doc, "4. СТОИМОСТЬ УСЛУГ",
                        "4.1. Стоимость: " + finalPrice.ToString("C") + " (включая НДС 20%).\n" +
                        "4.2. Оплата производится не менее чем за 24 часа до начала.");

                    AddSection(doc, "5. ОТВЕТСТВЕННОСТЬ",
                        "5.1. Исполнитель не несет ответственности за личные вещи участников.\n" +
                        "5.2. Участник несет ответственность за ущерб имуществу Исполнителя.");

                    AddSection(doc, "6. ПЕРСОНАЛЬНЫЕ ДАННЫЕ",
                        "6.1. Участник дает согласие на обработку персональных данных согласно ФЗ № 152-ФЗ.\n" +
                        "6.2. Исполнитель обязуется не передавать данные третьим лицам.");

                    AddSection(doc, "7. ЗАКЛЮЧИТЕЛЬНЫЕ ПОЛОЖЕНИЯ",
                        "7.1. Договор вступает в силу с момента подписания.\n" +
                        "7.2. Споры решаются путем переговоров.\n" +
                        "7.3. Договор составлен в двух экземплярах.");

                    // Подписи
                    Word.Paragraph signTitle = doc.Paragraphs.Add();
                    signTitle.Range.Text = "ПОДПИСИ СТОРОН";
                    signTitle.Range.Font.Name = "Times New Roman";
                    signTitle.Range.Font.Size = 12;
                    signTitle.Range.Font.Bold = 1;
                    signTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    signTitle.Range.ParagraphFormat.SpaceBefore = 20;
                    signTitle.Range.ParagraphFormat.SpaceAfter = 15;
                    signTitle.Range.InsertParagraphAfter();

                    Word.Paragraph tblPar = doc.Paragraphs.Add();
                    Word.Table signTable = doc.Tables.Add(tblPar.Range, 2, 3);
                    signTable.Borders.Enable = 0;
                    signTable.Range.Font.Name = "Times New Roman";
                    signTable.Range.Font.Size = 12;
                    signTable.Cell(1, 1).Range.Text = "Исполнитель:";
                    signTable.Cell(1, 1).Range.Font.Bold = 1;
                    signTable.Cell(1, 2).Range.Text = "ООО «Квестиум»";
                    signTable.Cell(1, 3).Range.Text = "___________ /Иванов И.И./";
                    signTable.Cell(2, 1).Range.Text = "Участник:";
                    signTable.Cell(2, 1).Range.Font.Bold = 1;
                    signTable.Cell(2, 2).Range.Text = textBox2.Text;
                    signTable.Cell(2, 3).Range.Text = "___________ /" + textBox2.Text + "/";
                    signTable.Columns[1].Width = 100;
                    signTable.Columns[2].Width = 200;
                    signTable.Columns[3].Width = 200;

                    // Сохраняем как PDF
                    doc.SaveAs2(saveDialog.FileName, Word.WdSaveFormat.wdFormatPDF);
                    doc.Close(false);
                    wordApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);

                    System.Diagnostics.Process.Start(saveDialog.FileName);

                    MessageBox.Show($"Договор сохранен в PDF!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка");
                if (doc != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                if (wordApp != null) { wordApp.Quit(); System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp); }
            }
        }
    }
}