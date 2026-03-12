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
    /// Форма для учета и просмотра заказов с правами администратора
    /// Предоставляет расширенные возможности фильтрации и просмотра всех заказов
    /// </summary>
    public partial class OrderAccountingAdmin : Form
    {
        // Коллекции для хранения заказов с поддержкой привязки к элементам управления
        private BindingList<Order> allOrders;        // Все заказы из базы данных
        private BindingList<Order> filteredOrders;    // Отфильтрованные заказы
        private BindingList<Order> pagedOrders;       // Заказы для текущей страницы

        // Репозиторий для работы с данными заказов
        private OrderRepository orderRepository;

        // Элементы управления для фильтрации по дате (создаются программно)
        private DateTimePicker dtpStartDate;  // Начальная дата периода
        private DateTimePicker dtpEndDate;     // Конечная дата периода

        // Элементы управления для пагинации (будут добавлены программно)
        private Panel pnlPagination;
        private Label lblPageInfo;
        private Button btnFirstPage;
        private Button btnPrevPage;
        private Button btnNextPage;
        private Button btnLastPage;
        private ComboBox cmbPageSelector;
        private ToolTip toolTip;

        // Параметры пагинации
        private int currentPage = 1;
        private int pageSize = 20;
        private int totalPages = 1;
        private int totalRecords = 0;
        private int displayedRecords = 0;

        // Флаг для маскировки персональных данных
        private bool maskPersonalData = true;

        // Цвета для условного форматирования
        private Color urgentColor = Color.LightCoral;      // Для срочных заказов (сегодня)
        private Color tomorrowColor = Color.LightYellow;    // Для заказов на завтра
        private Color cancelledColor = Color.LightGray;     // Для отмененных заказов
        private Color normalColor = Color.White;            // Для обычных заказов

        /// <summary>
        /// Конструктор формы учета заказов
        /// </summary>
        public OrderAccountingAdmin()
        {
            InitializeComponent();
            // Инициализация репозитория для работы с БД
            orderRepository = new OrderRepository();
            // Настройка дополнительных элементов формы
            InitializeAdditionalControls();
            // Отображение информации о текущем пользователе
            DisplayCurrentUser();
            // Загрузка данных из базы
            LoadOrders();
        }

        /// <summary>
        /// Инициализация дополнительных элементов управления
        /// </summary>
        private void InitializeAdditionalControls()
        {
            // Создание фильтров по дате
            CreateDateFilters();

            // Настройка таблицы для отображения заказов
            ConfigureDataGridView();

            // Настройка выпадающего списка статусов
            ConfigureStatusComboBox();

            // Создание элементов пагинации
            CreatePaginationControls();

            // Создание подсказки для цветового форматирования
            CreateColorToolTip();

            // Подписка на события для автоматического обновления фильтров
            SubscribeToEvents();
        }

        /// <summary>
        /// Создание подсказки для описания цветов
        /// </summary>
        private void CreateColorToolTip()
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(lblRecordCount, "Красный - срочные заказы (сегодня)\nЖелтый - заказы на завтра\nСерый - отмененные заказы");
            toolTip.SetToolTip(dataGridView, "Красный - срочные заказы (сегодня)\nЖелтый - заказы на завтра\nСерый - отмененные заказы");
        }

        /// <summary>
        /// Создание элементов управления для фильтрации по дате
        /// Размещает поля выбора даты начала и конца периода
        /// </summary>
        private void CreateDateFilters()
        {
            // Метка для поля "С" (начало периода)
            Label lblStartDate = new Label
            {
                Text = "С:",
                Location = new Point(660, 91),
                Size = new Size(20, 20),
                Font = new Font("Comic Sans MS", 9),
                BackColor = Color.Transparent
            };

            // Метка для поля "По" (конец периода)
            Label lblEndDate = new Label
            {
                Text = "По:",
                Location = new Point(790, 91),
                Size = new Size(30, 20),
                Font = new Font("Comic Sans MS", 9),
                BackColor = Color.Transparent
            };

            // Поле выбора начальной даты
            dtpStartDate = new DateTimePicker
            {
                Location = new Point(680, 91),
                Size = new Size(100, 20),
                Format = DateTimePickerFormat.Short,
                Value = new DateTime(2025, 1, 1),
                MinDate = new DateTime(2025, 1, 1),
                MaxDate = DateTime.Today
            };

            // Поле выбора конечной даты
            dtpEndDate = new DateTimePicker
            {
                Location = new Point(820, 91),
                Size = new Size(100, 20),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                MinDate = new DateTime(2025, 1, 1),
                MaxDate = DateTime.Today
            };

            // Добавление элементов на форму в groupBox1
            groupBox1.Controls.Add(lblStartDate);
            groupBox1.Controls.Add(dtpStartDate);
            groupBox1.Controls.Add(lblEndDate);
            groupBox1.Controls.Add(dtpEndDate);

            // Установка ограничения: конечная дата не может быть меньше начальной
            dtpEndDate.MinDate = dtpStartDate.Value;
        }

        /// <summary>
        /// Создание элементов управления для пагинации
        /// </summary>
        private void CreatePaginationControls()
        {
            // Панель для элементов пагинации
            pnlPagination = new Panel
            {
                Location = new Point(12, 480),
                Size = new Size(1100, 40),
                BackColor = Color.Transparent
            };

            // Кнопка "Первая страница"
            btnFirstPage = new Button
            {
                Location = new Point(350, 8),
                Size = new Size(40, 25),
                Text = "⏮",
                Font = new Font("Arial", 10),
                BackColor = SystemColors.GradientInactiveCaption,
                FlatStyle = FlatStyle.Popup,
                Enabled = false
            };
            btnFirstPage.Click += BtnFirstPage_Click;

            // Кнопка "Предыдущая страница"
            btnPrevPage = new Button
            {
                Location = new Point(395, 8),
                Size = new Size(40, 25),
                Text = "◀",
                Font = new Font("Arial", 10),
                BackColor = SystemColors.GradientInactiveCaption,
                FlatStyle = FlatStyle.Popup,
                Enabled = false
            };
            btnPrevPage.Click += BtnPrevPage_Click;

            // Метка с информацией о странице
            lblPageInfo = new Label
            {
                Location = new Point(440, 10),
                Size = new Size(150, 20),
                Font = new Font("Comic Sans MS", 9),
                Text = "Страница 1 из 1",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Кнопка "Следующая страница"
            btnNextPage = new Button
            {
                Location = new Point(595, 8),
                Size = new Size(40, 25),
                Text = "▶",
                Font = new Font("Arial", 10),
                BackColor = SystemColors.GradientInactiveCaption,
                FlatStyle = FlatStyle.Popup,
                Enabled = false
            };
            btnNextPage.Click += BtnNextPage_Click;

            // Кнопка "Последняя страница"
            btnLastPage = new Button
            {
                Location = new Point(640, 8),
                Size = new Size(40, 25),
                Text = "⏭",
                Font = new Font("Arial", 10),
                BackColor = SystemColors.GradientInactiveCaption,
                FlatStyle = FlatStyle.Popup,
                Enabled = false
            };
            btnLastPage.Click += BtnLastPage_Click;

            // Метка для выбора страницы
            Label lblGoToPage = new Label
            {
                Location = new Point(690, 10),
                Size = new Size(120, 20),
                Font = new Font("Comic Sans MS", 9),
                Text = "Перейти к странице:",
                TextAlign = ContentAlignment.MiddleRight
            };

            // Выпадающий список для выбора страницы
            cmbPageSelector = new ComboBox
            {
                Location = new Point(815, 8),
                Size = new Size(60, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Comic Sans MS", 9),
                BackColor = Color.White
            };
            cmbPageSelector.SelectedIndexChanged += CmbPageSelector_SelectedIndexChanged;

            // Добавляем элементы на панель
            pnlPagination.Controls.AddRange(new Control[] {
                btnFirstPage,
                btnPrevPage,
                lblPageInfo,
                btnNextPage,
                btnLastPage,
                lblGoToPage,
                cmbPageSelector
            });

            // Добавляем панель на форму
            this.Controls.Add(pnlPagination);
        }

        /// <summary>
        /// Настройка столбцов DataGridView для отображения информации о заказах
        /// </summary>
        private void ConfigureDataGridView()
        {
            // Отключаем автоматическую генерацию столбцов
            dataGridView.AutoGenerateColumns = false;

            // Настройка режимов выделения и редактирования
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.RowHeadersVisible = false;

            // Установка шрифта
            dataGridView.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Comic Sans MS", 9, FontStyle.Bold);

            // Включаем отрисовку строк с учетом статуса
            dataGridView.RowPrePaint += DataGridView_RowPrePaint;

            // Очистка существующих столбцов
            dataGridView.Columns.Clear();

            // Добавление столбцов с настройками отображения
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                HeaderText = "№ Заказа",
                DataPropertyName = "ID",
                Width = 80
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ClientName",
                HeaderText = "Клиент",
                DataPropertyName = "DisplayClientName",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PhoneNumber",
                HeaderText = "Телефон",
                DataPropertyName = "DisplayPhoneNumber",
                Width = 120
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "QuestName",
                HeaderText = "Квест",
                DataPropertyName = "QuestName",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DateOfAdmission",
                HeaderText = "Дата начала",
                DataPropertyName = "DateOfAdmission",
                Width = 130,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd.MM.yyyy HH:mm" }
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "StatusName",
                HeaderText = "Статус",
                DataPropertyName = "StatusName",
                Width = 100
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ManagerName",
                HeaderText = "Менеджер",
                DataPropertyName = "ManagerName",
                Width = 120
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ParticipantsCount",
                HeaderText = "Участники",
                DataPropertyName = "ParticipantsCount",
                Width = 80
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TotalPrice",
                HeaderText = "Стоимость",
                DataPropertyName = "TotalPrice",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "0.##' руб.'" }
            });

            // Добавляем скрытые колонки для хранения полных данных
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FullClientName",
                DataPropertyName = "ClientName",
                Visible = false
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FullPhoneNumber",
                DataPropertyName = "PhoneNumber",
                Visible = false
            });
        }

        /// <summary>
        /// Обработчик предварительной отрисовки строк для условного форматирования
        /// </summary>
        private void DataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (pagedOrders == null || e.RowIndex >= pagedOrders.Count) return;

            var order = pagedOrders[e.RowIndex];

            // Определяем цвет строки на основе статуса и даты
            if (order.StatusName == "Отменен")
            {
                dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = cancelledColor;
            }
            else if (order.DateOfAdmission.Date == DateTime.Today)
            {
                // Красный цвет для заказов на сегодня (срочные)
                dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = urgentColor;
            }
            else if (order.DateOfAdmission.Date == DateTime.Today.AddDays(1))
            {
                // Желтый цвет для заказов на завтра
                dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = tomorrowColor;
            }
            else
            {
                // Белый цвет для остальных заказов
                dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = normalColor;
            }
        }

        /// <summary>
        /// Настройка выпадающего списка статусов
        /// </summary>
        private void ConfigureStatusComboBox()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Все статусы");

            try
            {
                var statuses = orderRepository.GetAllStatuses();
                foreach (var status in statuses)
                {
                    cmbStatus.Items.Add(status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cmbStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// Отображает информацию о текущем администраторе
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                label2.Text = $"администратор {shortName}";
            }
        }

        /// <summary>
        /// Подписка на события элементов управления
        /// </summary>
        private void SubscribeToEvents()
        {
            txtSearch.TextChanged += OnFilterChanged;
            cmbStatus.SelectedIndexChanged += OnFilterChanged;
            dtpStartDate.ValueChanged += OnFilterChanged;
            dtpEndDate.ValueChanged += OnFilterChanged;

            dtpStartDate.ValueChanged += OnDateChanged;
            dtpEndDate.ValueChanged += OnDateChanged;

            dataGridView.CellFormatting += DataGridView_CellFormatting;
        }

        /// <summary>
        /// Обработчик изменения дат
        /// </summary>
        private void OnDateChanged(object sender, EventArgs e)
        {
            if (sender == dtpStartDate)
            {
                dtpEndDate.MinDate = dtpStartDate.Value;
                if (dtpEndDate.Value < dtpStartDate.Value)
                {
                    dtpEndDate.Value = dtpStartDate.Value;
                }
                OnFilterChanged(sender, e);
            }
        }

        /// <summary>
        /// Обработчик изменения фильтра
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Загрузка всех заказов
        /// </summary>
        private void LoadOrders()
        {
            try
            {
                var orders = orderRepository.GetAllOrders();

                foreach (var order in orders)
                {
                    order.DisplayClientName = MaskClientName(order.ClientName);
                    order.DisplayPhoneNumber = MaskPhoneNumber(order.PhoneNumber);
                }

                allOrders = new BindingList<Order>(orders);
                totalRecords = allOrders.Count;

                ApplyFilters();

                if (allOrders.Count == 0)
                {
                    MessageBox.Show("В базе данных нет заказов", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                allOrders = new BindingList<Order>();
            }
        }

        /// <summary>
        /// Применение фильтров
        /// </summary>
        private void ApplyFilters()
        {
            if (allOrders == null || allOrders.Count == 0)
            {
                UpdatePagination();
                return;
            }

            IEnumerable<Order> filtered = allOrders;

            // Фильтрация по дате
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);
            filtered = filtered.Where(o => o.DateOfAdmission >= startDate && o.DateOfAdmission <= endDate);

            // Фильтрация по статусу
            if (cmbStatus.SelectedIndex > 0)
            {
                string selectedStatus = cmbStatus.SelectedItem.ToString();
                filtered = filtered.Where(o => o.StatusName == selectedStatus);
            }

            // Фильтрация по номеру заказа
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchText = txtSearch.Text.Trim();
                filtered = filtered.Where(o => o.ID.ToString().StartsWith(searchText));
            }

            filteredOrders = new BindingList<Order>(filtered.ToList());
            displayedRecords = filteredOrders.Count;

            foreach (var order in filteredOrders)
            {
                order.DisplayClientName = MaskClientName(order.ClientName);
                order.DisplayPhoneNumber = MaskPhoneNumber(order.PhoneNumber);
            }

            // Сброс на первую страницу при изменении фильтра
            currentPage = 1;
            UpdatePagination();
            DisplayCurrentPage();
        }

        /// <summary>
        /// Обновление информации о пагинации
        /// </summary>
        private void UpdatePagination()
        {
            totalPages = (int)Math.Ceiling((double)displayedRecords / pageSize);
            if (totalPages == 0) totalPages = 1;

            // Обновляем информацию о записях
            lblRecordCount.Text = $"Записей: {displayedRecords} из {totalRecords}";

            // Обновляем информацию о странице
            lblPageInfo.Text = $"Страница {currentPage} из {totalPages}";

            // Обновляем выпадающий список страниц
            cmbPageSelector.Items.Clear();
            for (int i = 1; i <= totalPages; i++)
            {
                cmbPageSelector.Items.Add(i.ToString());
            }

            if (cmbPageSelector.Items.Count > 0)
            {
                cmbPageSelector.SelectedIndex = currentPage - 1;
            }

            UpdateNavigationButtons();
        }

        /// <summary>
        /// Обновление состояния кнопок навигации
        /// </summary>
        private void UpdateNavigationButtons()
        {
            btnFirstPage.Enabled = currentPage > 1 && displayedRecords > 0;
            btnPrevPage.Enabled = currentPage > 1 && displayedRecords > 0;
            btnNextPage.Enabled = currentPage < totalPages && displayedRecords > 0;
            btnLastPage.Enabled = currentPage < totalPages && displayedRecords > 0;
        }

        /// <summary>
        /// Отображение текущей страницы
        /// </summary>
        private void DisplayCurrentPage()
        {
            if (filteredOrders == null || filteredOrders.Count == 0)
            {
                pagedOrders = new BindingList<Order>();
                dataGridView.DataSource = null;
                UpdatePagination();
                return;
            }

            var paged = filteredOrders
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            pagedOrders = new BindingList<Order>(paged);
            dataGridView.DataSource = pagedOrders;
        }

        #region Навигация по страницам

        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            DisplayCurrentPage();
            UpdatePagination();
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayCurrentPage();
                UpdatePagination();
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayCurrentPage();
                UpdatePagination();
            }
        }

        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            currentPage = totalPages;
            DisplayCurrentPage();
            UpdatePagination();
        }

        private void CmbPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSelector.SelectedItem != null && !cmbPageSelector.IsHandleCreated)
            {
                int selectedPage = int.Parse(cmbPageSelector.SelectedItem.ToString());
                if (selectedPage != currentPage)
                {
                    currentPage = selectedPage;
                    DisplayCurrentPage();
                    UpdatePagination();
                }
            }
        }

        #endregion

        #region Маскирование данных

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!maskPersonalData) return;

            if (dataGridView.Columns[e.ColumnIndex].Name == "ClientName" && e.Value != null)
            {
                string fullName = e.Value.ToString();
                e.Value = MaskClientName(fullName);
                e.FormattingApplied = true;
            }
            else if (dataGridView.Columns[e.ColumnIndex].Name == "PhoneNumber" && e.Value != null)
            {
                string phone = e.Value.ToString();
                e.Value = MaskPhoneNumber(phone);
                e.FormattingApplied = true;
            }
            else if (dataGridView.Columns[e.ColumnIndex].Name == "ManagerName" && e.Value != null)
            {
                string manager = e.Value.ToString();
                e.Value = MaskManagerName(manager);
                e.FormattingApplied = true;
            }
        }

        private string MaskClientName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName)) return "";

            string[] parts = fullName.Split(' ');
            if (parts.Length >= 1)
            {
                string result = parts[0];

                if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1]))
                {
                    result += " " + parts[1].Substring(0, 1) + ".";
                }

                if (parts.Length >= 3 && !string.IsNullOrEmpty(parts[2]))
                {
                    result += " " + parts[2].Substring(0, 1) + ".";
                }

                return result;
            }

            return fullName;
        }

        private string MaskPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return "";

            string digits = new string(phone.Where(char.IsDigit).ToArray());

            if (digits.Length >= 11)
            {
                string last4 = digits.Substring(digits.Length - 4, 4);
                return "+7 *** **-" + last4;
            }

            return phone;
        }

        private string MaskManagerName(string managerName)
        {
            if (string.IsNullOrEmpty(managerName)) return "";

            string[] parts = managerName.Split(' ');
            if (parts.Length >= 1)
            {
                string result = parts[0];

                if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1]))
                {
                    result += " " + parts[1].Substring(0, 1) + ".";
                }

                return result;
            }

            return managerName;
        }

        #endregion

        #region Обработчики кнопок

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int orderId = (int)dataGridView.SelectedRows[0].Cells["ID"].Value;
                OpenOrderDetails(orderId);
            }
            else
            {
                MessageBox.Show("Выберите заказ для просмотра", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainAdmin auto = new MainAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            dtpStartDate.Value = DateTime.Today.AddMonths(-1);
            dtpEndDate.Value = DateTime.Today;
            cmbStatus.SelectedIndex = 0;
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int orderId = (int)dataGridView.Rows[e.RowIndex].Cells["ID"].Value;
                OpenOrderDetails(orderId);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void OpenOrderDetails(int orderId)
        {
            this.Visible = false;
            ViewingOrderAdmin detailsForm = new ViewingOrderAdmin(orderId);
            detailsForm.ShowDialog();
            this.Visible = true;
            LoadOrders();
        }

        #endregion
    }
}