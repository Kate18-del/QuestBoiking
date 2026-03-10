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
        private void CreateReceipt()
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
        private void CreateConsentAgreement()
        {
            Word.Application wordApp = null;
            Word.Document doc = null;

            try
            {
                // Создание документа Word
                wordApp = new Word.Application();
                doc = wordApp.Documents.Add();

                // Настройка документа
                wordApp.Visible = true;
                doc.PageSetup.Orientation = Word.WdOrientation.wdOrientPortrait;
                doc.PageSetup.TopMargin = 72;
                doc.PageSetup.BottomMargin = 72;
                doc.PageSetup.LeftMargin = 72;
                doc.PageSetup.RightMargin = 72;

                // ЗАГОЛОВОК ДОКУМЕНТА
                Word.Paragraph title = doc.Paragraphs.Add();
                title.Range.Text = "ДОГОВОР СОГЛАСИЯ НА УЧАСТИЕ В КВЕСТЕ\n";
                title.Range.Font.Name = "Times New Roman";
                title.Range.Font.Size = 16;
                title.Range.Font.Bold = 1;
                title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.InsertParagraphAfter();

                // Номер договора и дата
                Word.Paragraph header = doc.Paragraphs.Add();
                header.Range.Text = $"Договор № {orderId} от {DateTime.Now:dd.MM.yyyy}\n\n";
                header.Range.Font.Size = 12;
                header.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                header.Range.InsertParagraphAfter();

                // СТОРОНЫ ДОГОВОРА
                Word.Paragraph parties = doc.Paragraphs.Add();
                parties.Range.Text = "г. Москва\n\n";
                parties.Range.Text += "ООО \"Квестиум\", именуемое в дальнейшем \"Исполнитель\", в лице директора Иванова И.И., ";
                parties.Range.Text += "действующего на основании Устава, с одной стороны, и\n";
                parties.Range.Text += $"Гражданин(ка) {textBox2.Text}, именуемый(ая) в дальнейшем \"Участник\", с другой стороны, ";
                parties.Range.Text += "заключили настоящий договор о нижеследующем:\n\n";
                parties.Range.Font.Size = 12;
                parties.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                parties.Range.InsertParagraphAfter();

                // Разделы договора
                AddSection(doc, "1. ПРЕДМЕТ ДОГОВОРА",
                    "1.1. Исполнитель обязуется предоставить, а Участник обязуется оплатить услуги по организации и проведению квеста.");

                AddSection(doc, "2. УСЛОВИЯ ПРОВЕДЕНИЯ КВЕСТА",
                    "2.1. Наименование квеста: " + comboBox1.Text + "\n" +
                    "2.2. Дата и время проведения: " + dateTimePicker2.Value.ToString("dd.MM.yyyy HH:mm") + "\n" +
                    "2.3. Количество участников: " + participantsCount + " человек\n" +
                    "2.4. Продолжительность: 60 минут\n" +
                    "2.5. Адрес проведения: г. Москва, ул. Примерная, д. 1, квест-рум \"Adventure Zone\"");

                AddSection(doc, "3. ПРАВА И ОБЯЗАННОСТИ СТОРОН",
                    "3.1. Исполнитель обязуется:\n" +
                    "   а) Обеспечить безопасность участников во время прохождения квеста;\n" +
                    "   б) Предоставить необходимое оборудование и реквизит;\n" +
                    "   в) Соблюдать конфиденциальность персональных данных участников.\n\n" +
                    "3.2. Участник обязуется:\n" +
                    "   а) Соблюдать правила техники безопасности;\n" +
                    "   б) Не наносить ущерб имуществу Исполнителя;\n" +
                    "   в) Прибыть на квест за 15 минут до назначенного времени.");

                AddSection(doc, "4. СТОИМОСТЬ УСЛУГ И ПОРЯДОК РАСЧЕТОВ",
                    "4.1. Стоимость услуг составляет: " + finalPrice.ToString("C") + " (включая НДС 20%).\n" +
                    "4.2. Оплата производится в полном объеме не менее чем за 24 часа до начала квеста.\n" +
                    "4.3. В случае отмены бронирования менее чем за 24 часа, предоплата не возвращается.");

                AddSection(doc, "5. ОТВЕТСТВЕННОСТЬ СТОРОН",
                    "5.1. Исполнитель не несет ответственности за:\n" +
                    "   а) Личные вещи участников;\n" +
                    "   б) Действия участников, повлекшие причинение вреда их здоровью;\n" +
                    "   в) Неявку участников в назначенное время.\n\n" +
                    "5.2. Участник несет полную материальную ответственность за ущерб, причиненный имуществу Исполнителя.");

                AddSection(doc, "6. ПЕРСОНАЛЬНЫЕ ДАННЫЕ",
                    "6.1. Участник дает согласие на обработку своих персональных данных в соответствии с Федеральным законом №152-ФЗ.\n" +
                    "6.2. Исполнитель обязуется не передавать персональные данные третьим лицам.");

                AddSection(doc, "7. ЗАКЛЮЧИТЕЛЬНЫЕ ПОЛОЖЕНИЯ",
                    "7.1. Настоящий договор вступает в силу с момента подписания и действует до полного исполнения обязательств.\n" +
                    "7.2. Все споры решаются путем переговоров, а при невозможности достижения согласия - в судебном порядке.");

                doc.Paragraphs.Add();

                // ПОДПИСИ СТОРОН
                Word.Paragraph signatures = doc.Paragraphs.Add();
                signatures.Range.Text = "ПОДПИСИ СТОРОН:\n\n";
                signatures.Range.Font.Bold = 1;
                signatures.Range.InsertParagraphAfter();

                // Таблица для подписей
                Word.Table signTable = doc.Tables.Add(doc.Paragraphs.Add().Range, 2, 3);
                signTable.Borders.Enable = 1;

                signTable.Cell(1, 1).Range.Text = "ИСПОЛНИТЕЛЬ:";
                signTable.Cell(1, 2).Range.Text = "ООО \"Квестиум\"";
                signTable.Cell(1, 3).Range.Text = "___________________\nИванов И.И.";

                signTable.Cell(2, 1).Range.Text = "УЧАСТНИК:";
                signTable.Cell(2, 2).Range.Text = textBox2.Text;
                signTable.Cell(2, 3).Range.Text = "___________________\n" + textBox2.Text;

                // Настройка ширины колонок
                signTable.Columns[1].Width = 120;
                signTable.Columns[2].Width = 200;
                signTable.Columns[3].Width = 200;

                // Сохранение документа
                string fileName = $"Договор_согласия_{orderId}_{DateTime.Now:yyyyMMdd_HHmm}.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                try
                {
                    doc.SaveAs(filePath);
                    MessageBox.Show($"Договор согласия успешно создан!\nФайл сохранен: {filePath}",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Документ создан, но возникла ошибка при сохранении:\n{ex.Message}",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания договора: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Правильное закрытие Word и освобождение COM-объектов
                CloseWordDocument(doc, wordApp);

                // Принудительная сборка мусора для освобождения COM-объектов
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Добавление горизонтальной линии в документ
        /// </summary>
        private void AddHorizontalLine(Word.Document doc)
        {
            Word.Paragraph line = doc.Paragraphs.Add();
            line.Range.Text = "_________________________________________\n";
            line.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            line.Range.InsertParagraphAfter();
        }

        /// <summary>
        /// Добавление раздела в документ с заголовком и содержимым
        /// </summary>
        private void AddSection(Word.Document doc, string title, string content)
        {
            Word.Paragraph sectionTitle = doc.Paragraphs.Add();
            sectionTitle.Range.Text = title + "\n";
            sectionTitle.Range.Font.Bold = 1;
            sectionTitle.Range.Font.Size = 12;
            sectionTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            sectionTitle.Range.InsertParagraphAfter();

            Word.Paragraph sectionContent = doc.Paragraphs.Add();
            sectionContent.Range.Text = content + "\n\n";
            sectionContent.Range.Font.Size = 12;
            sectionContent.Range.ParagraphFormat.FirstLineIndent = 36;
            sectionContent.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
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
            this.Visible = false;
            MainManager auto = new MainManager();
            auto.ShowDialog();
            this.Visible = true;
        }
    }
}