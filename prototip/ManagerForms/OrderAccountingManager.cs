using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace prototip
{
    /// <summary>
    /// Форма для учета и анализа заказов с правами менеджера
    /// Предоставляет возможности фильтрации, поиска и экспорта в Excel
    /// </summary>
    public partial class OrderAccountingManager : Form
    {
        // Коллекции для хранения заказов с поддержкой привязки к элементам управления
        private BindingList<Order> allOrders;        // Все заказы из базы данных
        private BindingList<Order> filteredOrders;    // Отфильтрованные заказы

        // Репозиторий для работы с данными заказов
        private readonly OrderRepository _orderRepository;

        // Элементы управления для фильтрации по дате (создаются программно)
        private DateTimePicker dtpStartDate;  // Начальная дата периода
        private DateTimePicker dtpEndDate;     // Конечная дата периода

        // Метка для отображения общей выручки
        private Label lblTotalRevenue;

        // Общая сумма выручки по отфильтрованным заказам
        private decimal totalRevenue = 0;

        /// <summary>
        /// Конструктор формы учета заказов для менеджера
        /// </summary>
        public OrderAccountingManager()
        {
            InitializeComponent();
            // Инициализация репозитория для работы с БД
            _orderRepository = new OrderRepository();
            // Настройка всех элементов формы
            InitializeForm();
        }

        /// <summary>
        /// Инициализация всех элементов формы
        /// </summary>
        private void InitializeForm()
        {
            // Создание фильтров по дате
            CreateDateFilters();

            // Настройка таблицы для отображения заказов
            ConfigureDataGridView();

            // Загрузка заказов из базы данных
            LoadOrders();

            // Загрузка статусов в выпадающий список
            LoadStatuses();

            // Настройка обработчиков событий
            SetupEventHandlers();

            // Отображение информации о текущем менеджере
            DisplayCurrentUser();
        }

        /// <summary>
        /// Создание элементов управления для фильтрации по дате и отображения выручки
        /// </summary>
        private void CreateDateFilters()
        {
            // Метка для поля "С" (начало периода)
            Label lblStartDate = new Label
            {
                Text = "С:",
                Location = new Point(650, 86),
                Size = new Size(20, 20),
                Font = new Font("Comic Sans MS", 9)
            };

            // Метка для поля "По" (конец периода)
            Label lblEndDate = new Label
            {
                Text = "По:",
                Location = new Point(780, 86),
                Size = new Size(30, 20),
                Font = new Font("Comic Sans MS", 9)
            };

            // Поле выбора начальной даты
            dtpStartDate = new DateTimePicker
            {
                Location = new Point(670, 86),
                Size = new Size(100, 20),
                Format = DateTimePickerFormat.Short,
                Value = new DateTime(2025, 1, 1), // Начало периода - 1 января 2025
                Tag = DateTime.Today,
                MinDate = new DateTime(2025, 1, 1), // Минимально допустимая дата
                MaxDate = DateTime.Today // Максимальная дата - сегодня
            };

            // Поле выбора конечной даты
            dtpEndDate = new DateTimePicker
            {
                Location = new Point(810, 86),
                Size = new Size(100, 20),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Tag = DateTime.Today,
                MinDate = new DateTime(2025, 1, 1),
                MaxDate = DateTime.Today
            };

            // Метка для отображения общей выручки (внизу формы)
            lblTotalRevenue = new Label
            {
                Text = "Выручка: 0 руб.",
                Location = new Point(12, 560),
                Size = new Size(250, 25),
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };

            // Добавление элементов на форму
            groupBox1.Controls.Add(lblStartDate);
            groupBox1.Controls.Add(dtpStartDate);
            groupBox1.Controls.Add(lblEndDate);
            groupBox1.Controls.Add(dtpEndDate);
            this.Controls.Add(lblTotalRevenue);

            // Установка ограничения: конечная дата не может быть меньше начальной
            dtpEndDate.MinDate = dtpStartDate.Value;
        }

        /// <summary>
        /// Настройка столбцов DataGridView для отображения информации о заказах
        /// Используется автоматическое распределение ширины через FillWeight
        /// </summary>
        private void ConfigureDataGridView()
        {
            // Отключаем автоматическую генерацию столбцов
            dataGridView2.AutoGenerateColumns = false;

            // Настройка режимов выделения и редактирования
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;

            // Установка позиции и размера таблицы
            dataGridView2.Location = new Point(8, 118);
            dataGridView2.Size = new Size(1037, 391);

            // Установка шрифта
            this.dataGridView2.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);

            // Очистка существующих столбцов
            dataGridView2.Columns.Clear();

            // Автоматическое распределение ширины колонок
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Добавление столбцов с настройками отображения и FillWeight для пропорционального распределения
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                HeaderText = "№ заказа",
                DataPropertyName = "ID",
                FillWeight = 5 // Процент от общей ширины
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DateOfAdmission",
                HeaderText = "Дата приема",
                DataPropertyName = "DateOfAdmission",
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "dd.MM.yyyy HH:mm" // Формат даты и времени
                }
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DueDate",
                HeaderText = "Срок исполнения",
                DataPropertyName = "DueDate",
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "dd.MM.yyyy HH:mm",
                    NullValue = "" // Пустая строка для NULL значений
                }
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ClientName",
                HeaderText = "Клиент",
                DataPropertyName = "ClientName",
                FillWeight = 15
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PhoneNumber",
                HeaderText = "Телефон",
                DataPropertyName = "PhoneNumber",
                FillWeight = 10
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "QuestName",
                HeaderText = "Квест",
                DataPropertyName = "QuestName",
                FillWeight = 15
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "StatusName",
                HeaderText = "Статус",
                DataPropertyName = "StatusName",
                FillWeight = 8
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ParticipantsCount",
                HeaderText = "Кол-во чел.",
                DataPropertyName = "ParticipantsCount",
                FillWeight = 6,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    NullValue = "0"
                }
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TotalPrice",
                HeaderText = "Сумма",
                DataPropertyName = "TotalPrice",
                FillWeight = 8,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "0.##' руб.'", // Формат с валютой
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    NullValue = "0"
                }
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ManagerName",
                HeaderText = "Менеджер",
                DataPropertyName = "ManagerName",
                FillWeight = 13
            });
        }

        /// <summary>
        /// Загрузка всех заказов из базы данных
        /// </summary>
        private void LoadOrders()
        {
            try
            {
                // Получение всех заказов через репозиторий
                var orders = _orderRepository.GetAllOrders();
                allOrders = new BindingList<Order>(orders);
                filteredOrders = new BindingList<Order>(orders);
                dataGridView2.DataSource = filteredOrders;

                // Расчет общей выручки и обновление счетчика
                CalculateTotalRevenue();
                UpdateRecordCount();
                dataGridView2.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загрузка статусов заказов в выпадающий список
        /// </summary>
        private void LoadStatuses()
        {
            try
            {
                var statuses = _orderRepository.GetAllStatuses();

                comboBox1.Items.Clear();
                comboBox1.Items.Add("Все статусы"); // Опция для отображения всех статусов
                foreach (var status in statuses)
                {
                    comboBox1.Items.Add(status);
                }
                comboBox1.SelectedIndex = 0;

                comboBox2.Visible = false; // Второй комбобокс не используется в этой форме
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Настройка всех обработчиков событий
        /// </summary>
        private void SetupEventHandlers()
        {
            // Живой поиск по номеру заказа - фильтрация при вводе каждой цифры
            textBox1.TextChanged += TextBox1_TextChanged;

            // Фильтрация по статусу
            comboBox1.SelectedIndexChanged += OnFilterChanged;

            // Фильтрация по датам
            dtpStartDate.ValueChanged += OnFilterChanged;
            dtpEndDate.ValueChanged += OnFilterChanged;

            // Сброс фильтров
            button1.Click += btnReset_Click;

            // Кнопка меню (возврат в главное меню)
            btnMenu.Click += btnMenu_Click;

            // Кнопка формирования отчета в Excel
            button2.Click += btnGenerateReport_Click;

            // Настройка плейсхолдера для поиска
            SetupSearchPlaceholder();

            // Отдельные обработчики для синхронизации дат
            dtpStartDate.ValueChanged += OnDateChanged;
            dtpEndDate.ValueChanged += OnDateChanged;
        }

        /// <summary>
        /// Обработчик изменения дат
        /// Синхронизирует минимальную дату окончания с выбранной датой начала
        /// </summary>
        private void OnDateChanged(object sender, EventArgs e)
        {
            if (sender == dtpStartDate)
            {
                // При изменении даты начала:
                // 1. Обновляем минимальную дату окончания
                dtpEndDate.MinDate = dtpStartDate.Value;

                // 2. Если текущая дата окончания меньше новой даты начала,
                //    корректируем дату окончания
                if (dtpEndDate.Value < dtpStartDate.Value)
                {
                    dtpEndDate.Value = dtpStartDate.Value;
                }

                // 3. Обновляем фильтр
                OnFilterChanged(sender, e);
            }
            else if (sender == dtpEndDate)
            {
                // При изменении даты окончания просто обновляем фильтр
                OnFilterChanged(sender, e);
            }
        }

        /// <summary>
        /// Настройка плейсхолдера для поля поиска
        /// </summary>
        private void SetupSearchPlaceholder()
        {
            textBox1.Enter += (s, e) =>
            {
                if (textBox1.Text == "Поиск")
                {
                    textBox1.Text = "";
                    textBox1.ForeColor = SystemColors.WindowText;
                }
            };

            textBox1.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    textBox1.Text = "Поиск";
                    textBox1.ForeColor = SystemColors.GrayText;
                }
            };
        }

        /// <summary>
        /// Обработчик изменения текста в поле поиска
        /// Немедленно применяет фильтрацию
        /// </summary>
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Общий обработчик изменения любого фильтра
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Применение всех активных фильтров к списку заказов
        /// </summary>
        private void ApplyFilters()
        {
            if (allOrders == null) return;

            // Начинаем со всех заказов
            IEnumerable<Order> filtered = allOrders;

            // 1. ПОИСК ПО НОМЕРУ ЗАКАЗА (приоритетная фильтрация)
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text != "Поиск")
            {
                string searchText = textBox1.Text.Trim();
                filtered = filtered.Where(o => o.ID.ToString().StartsWith(searchText));
            }

            // 2. ФИЛЬТРАЦИЯ ПО ПЕРИОДУ ДАТ
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1); // Включительно до конца дня

            filtered = filtered.Where(o => o.DateOfAdmission >= startDate && o.DateOfAdmission <= endDate);

            // 3. ФИЛЬТРАЦИЯ ПО СТАТУСУ ЗАКАЗА
            if (comboBox1.SelectedIndex > 0) // Индекс 0 - "Все статусы"
            {
                string selectedStatus = comboBox1.SelectedItem.ToString();
                filtered = filtered.Where(o => o.StatusName == selectedStatus);
            }

            // Обновление данных
            filteredOrders = new BindingList<Order>(filtered.ToList());
            dataGridView2.DataSource = filteredOrders;

            // Обновление статистики
            CalculateTotalRevenue();
            UpdateRecordCount();
        }

        /// <summary>
        /// Расчет общей выручки по отфильтрованным заказам
        /// </summary>
        private void CalculateTotalRevenue()
        {
            totalRevenue = filteredOrders
                .Where(o => o.TotalPrice.HasValue) // Только заказы с указанной ценой
                .Sum(o => o.TotalPrice.Value);     // Суммирование

            lblTotalRevenue.Text = $"Выручка: {totalRevenue:N0} руб."; // Форматирование с разделителями
        }

        /// <summary>
        /// Обновление счетчика количества записей
        /// </summary>
        private void UpdateRecordCount()
        {
            label3.Text = $"Количество записей: {filteredOrders?.Count ?? 0}";
        }

        /// <summary>
        /// Отображение информации о текущем менеджере
        /// Формирует краткое ФИО в формате "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (!string.IsNullOrEmpty(CurrentUser.FIO))
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');

                // Формирование краткого ФИО в зависимости от количества частей
                if (fioParts.Length >= 3)
                {
                    string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                    label2.Text = $"менеджер {shortName}";
                }
                else if (fioParts.Length == 2)
                {
                    string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.";
                    label2.Text = $"менеджер {shortName}";
                }
                else
                {
                    label2.Text = $"менеджер {CurrentUser.FIO}";
                }
            }
            else
            {
                label2.Text = "менеджер не авторизован";
            }
        }

        /// <summary>
        /// Обработчик кнопки сброса всех фильтров
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            // Сброс поиска
            textBox1.Text = "Поиск";
            textBox1.ForeColor = SystemColors.GrayText;

            // Сброс фильтра статуса
            comboBox1.SelectedIndex = 0;

            // Сброс дат (последний месяц)
            dtpStartDate.Value = DateTime.Today.AddMonths(-1);
            dtpEndDate.Value = DateTime.Today;

            // Применение сброшенных фильтров
            ApplyFilters();
        }

        /// <summary>
        /// Обработчик кнопки формирования отчета в Excel
        /// </summary>
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (filteredOrders.Count == 0)
            {
                MessageBox.Show("Нет данных для отчета!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Диалог выбора места сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Сохранить отчет Excel";
                saveFileDialog.FileName = $"Отчет_заказов_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Экспорт данных в Excel
                    ExportToExcel(saveFileDialog.FileName);
                    MessageBox.Show($"Отчет успешно сохранен!\n{saveFileDialog.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Экспорт отфильтрованных заказов в Excel-файл
        /// </summary>
        /// <param name="filePath">Путь для сохранения файла</param>
        private void ExportToExcel(string filePath)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Создание нового приложения Excel
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = workbook.ActiveSheet;

                // Заголовок отчета
                worksheet.Cells[1, 1] = "Отчет по заказам";
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 11]].Merge();
                worksheet.Cells[1, 1].Font.Bold = true;
                worksheet.Cells[1, 1].Font.Size = 14;
                worksheet.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Период отчета
                worksheet.Cells[2, 1] = $"Период: {dtpStartDate.Value:dd.MM.yyyy} - {dtpEndDate.Value:dd.MM.yyyy}";
                worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, 11]].Merge();
                worksheet.Cells[2, 1].Font.Italic = true;

                // Статус фильтра
                string statusFilter = comboBox1.SelectedIndex > 0 ? $"Статус: {comboBox1.SelectedItem}" : "Статус: Все";
                worksheet.Cells[3, 1] = statusFilter;
                worksheet.Range[worksheet.Cells[3, 1], worksheet.Cells[3, 11]].Merge();

                // Общая выручка
                worksheet.Cells[4, 1] = $"Общая выручка: {totalRevenue:N0} руб.";
                worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[4, 11]].Merge();
                worksheet.Cells[4, 1].Font.Bold = true;
                worksheet.Cells[4, 1].Font.Color = ColorTranslator.ToOle(Color.Green);

                // Количество записей
                worksheet.Cells[5, 1] = $"Количество записей: {filteredOrders.Count}";
                worksheet.Range[worksheet.Cells[5, 1], worksheet.Cells[5, 11]].Merge();

                // Пустая строка для отступа
                worksheet.Cells[6, 1] = "";

                // Заголовки колонок
                string[] headers = { "№ заказа", "Дата приема", "Срок исполнения", "Клиент", "Телефон",
                           "Квест", "Статус", "Кол-во чел.", "Сумма", "Менеджер" };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[7, i + 1] = headers[i];
                    worksheet.Cells[7, i + 1].Font.Bold = true;
                    worksheet.Cells[7, i + 1].Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                    worksheet.Cells[7, i + 1].Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }

                // Заполнение данными
                int row = 8;
                foreach (var order in filteredOrders)
                {
                    worksheet.Cells[row, 1] = order.ID;

                    // ДАТА ПРИЕМА - как строка в правильном формате
                    worksheet.Cells[row, 2] = order.DateOfAdmission.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells[row, 2].NumberFormat = "@"; // Текстовый формат

                    // СРОК ИСПОЛНЕНИЯ - как строка
                    if (order.DueDate.HasValue)
                    {
                        worksheet.Cells[row, 3] = order.DueDate.Value.ToString("dd.MM.yyyy HH:mm");
                    }
                    else
                    {
                        worksheet.Cells[row, 3] = "";
                    }
                    worksheet.Cells[row, 3].NumberFormat = "@"; // Текстовый формат

                    worksheet.Cells[row, 4] = order.ClientName;
                    worksheet.Cells[row, 5] = order.PhoneNumber;
                    worksheet.Cells[row, 6] = order.QuestName;
                    worksheet.Cells[row, 7] = order.StatusName;
                    worksheet.Cells[row, 8] = order.ParticipantsCount ?? 0;
                    worksheet.Cells[row, 9] = order.TotalPrice ?? 0;
                    worksheet.Cells[row, 10] = order.ManagerName;

                    // Форматирование для суммы
                    if (order.TotalPrice.HasValue)
                    {
                        worksheet.Cells[row, 9].NumberFormat = "#,##0\" руб.\"";
                    }

                    // Форматирование для колонки с количеством людей
                    worksheet.Cells[row, 8].NumberFormat = "0";

                    row++;
                }

                // Настройка ширины колонок
                worksheet.Columns.AutoFit(); // Автоподбор ширины

                // Ручная установка ширины для колонок с датами
                worksheet.Columns[2].ColumnWidth = 15; // Дата приема
                worksheet.Columns[3].ColumnWidth = 15; // Срок исполнения

                // Ручная установка ширины для других колонок
                worksheet.Columns[4].ColumnWidth = 20; // Клиент
                worksheet.Columns[5].ColumnWidth = 12; // Телефон
                worksheet.Columns[6].ColumnWidth = 20; // Квест
                worksheet.Columns[7].ColumnWidth = 10; // Статус
                worksheet.Columns[8].ColumnWidth = 10; // Кол-во чел.
                worksheet.Columns[9].ColumnWidth = 12; // Сумма
                worksheet.Columns[10].ColumnWidth = 15; // Менеджер

                // Добавление границ для области данных
                Excel.Range dataRange = worksheet.Range[worksheet.Cells[7, 1], worksheet.Cells[row - 1, 10]];
                dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // Заморозка строки с заголовками
                worksheet.Application.ActiveWindow.SplitRow = 7;
                worksheet.Application.ActiveWindow.FreezePanes = true;

                // Сохранение файла
                workbook.SaveAs(filePath);
            }
            finally
            {
                // Освобождение ресурсов COM-объектов
                if (workbook != null)
                {
                    workbook.Close(false);
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                }

                // Очистка COM-объектов для предотвращения утечек памяти
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        /// <summary>
        /// Ограничение ввода в поле поиска только цифрами
        /// Так как поиск осуществляется по номеру заказа
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Блокировка нецифровых символов
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