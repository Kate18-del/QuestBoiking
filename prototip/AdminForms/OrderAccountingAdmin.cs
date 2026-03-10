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

        // Репозиторий для работы с данными заказов
        private OrderRepository orderRepository;

        // Элементы управления для фильтрации по дате (создаются программно)
        private DateTimePicker dtpStartDate;  // Начальная дата периода
        private DateTimePicker dtpEndDate;     // Конечная дата периода

        /// <summary>
        /// Конструктор формы учета заказов
        /// </summary>
        public OrderAccountingAdmin()
        {
            InitializeComponent();
            // Инициализация репозитория для работы с БД
            orderRepository = new OrderRepository();
            // Настройка элементов формы
            InitializeForm();
            // Отображение информации о текущем пользователе
            DisplayCurrentUser();
        }

        /// <summary>
        /// Отображает информацию о текущем администраторе
        /// Формирует краткое ФИО в формате "Фамилия И.О."
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
        /// Инициализация всех элементов формы
        /// Вызывается при создании формы
        /// </summary>
        private void InitializeForm()
        {
            // Создание фильтров по дате
            CreateDateFilters();

            // Настройка таблицы для отображения заказов
            ConfigureDataGridView();

            // Настройка выпадающего списка статусов
            ConfigureStatusComboBox();

            // Загрузка данных из базы
            LoadOrders();

            // Подписка на события для автоматического обновления фильтров
            SubscribeToEvents();
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
                Location = new Point(650, 91),
                Size = new Size(20, 20),
                Font = new Font("Comic Sans MS", 9)
            };

            // Метка для поля "По" (конец периода)
            Label lblEndDate = new Label
            {
                Text = "По:",
                Location = new Point(780, 91),
                Size = new Size(30, 20),
                Font = new Font("Comic Sans MS", 9)
            };

            // Поле выбора начальной даты
            dtpStartDate = new DateTimePicker
            {
                Location = new Point(670, 91),
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
                Location = new Point(810, 91),
                Size = new Size(100, 20),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today, // Конец периода - сегодня
                Tag = DateTime.Today,
                MinDate = new DateTime(2025, 1, 1),
                MaxDate = DateTime.Today
            };

            // Добавление элементов на форму
            groupBox1.Controls.Add(lblStartDate);
            groupBox1.Controls.Add(dtpStartDate);
            groupBox1.Controls.Add(lblEndDate);
            groupBox1.Controls.Add(dtpEndDate);

            // Установка ограничения: конечная дата не может быть меньше начальной
            dtpEndDate.MinDate = dtpStartDate.Value;
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

            // Установка шрифта
            this.dataGridView.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);

            // Очистка существующих столбцов
            dataGridView.Columns.Clear();

            // Добавление столбцов с настройками отображения
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                HeaderText = "№ Заказа",
                DataPropertyName = "ID",      // Привязка к свойству Order.ID
                Width = 80
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ClientName",
                HeaderText = "Клиент",
                DataPropertyName = "ClientName",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PhoneNumber",
                HeaderText = "Телефон",
                DataPropertyName = "PhoneNumber",
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
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd.MM.yyyy HH:mm" } // Формат даты и времени
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
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "0.##' руб.'" } // Формат с валютой
            });
        }

        /// <summary>
        /// Настройка выпадающего списка статусов заказов
        /// Загружает статусы из базы данных
        /// </summary>
        private void ConfigureStatusComboBox()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Все статусы"); // Добавляем опцию для отображения всех статусов

            try
            {
                // Загрузка списка статусов из репозитория
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

            // Устанавливаем значение по умолчанию - "Все статусы"
            cmbStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// Подписка на события элементов управления для автоматического обновления фильтров
        /// </summary>
        private void SubscribeToEvents()
        {
            // При изменении любого фильтра автоматически применяем фильтрацию
            txtSearch.TextChanged += OnFilterChanged;
            cmbStatus.SelectedIndexChanged += OnFilterChanged;
            dtpStartDate.ValueChanged += OnFilterChanged;
            dtpEndDate.ValueChanged += OnFilterChanged;

            // Отдельная подписка для обработки изменения дат
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
        /// Обработчик изменения любого фильтра
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Загрузка всех заказов из базы данных
        /// </summary>
        private void LoadOrders()
        {
            try
            {
                // Получаем все заказы через репозиторий
                var orders = orderRepository.GetAllOrders();
                allOrders = new BindingList<Order>(orders);

                // Применяем фильтры после загрузки
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
        /// Применение всех активных фильтров к списку заказов
        /// </summary>
        private void ApplyFilters()
        {
            if (allOrders == null || allOrders.Count == 0) return;

            IEnumerable<Order> filtered = allOrders;

            // 1. Фильтрация по периоду дат
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1); // Включительно до конца дня

            filtered = filtered.Where(o => o.DateOfAdmission >= startDate && o.DateOfAdmission <= endDate);

            // 2. Фильтрация по статусу заказа
            if (cmbStatus.SelectedIndex > 0) // Индекс 0 - "Все статусы"
            {
                string selectedStatus = cmbStatus.SelectedItem.ToString();
                filtered = filtered.Where(o => o.StatusName == selectedStatus);
            }

            // 3. Фильтрация по номеру заказа (поиск по началу номера)
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string searchText = txtSearch.Text.Trim();
                filtered = filtered.Where(o => o.ID.ToString().StartsWith(searchText));
            }

            // Сохраняем отфильтрованный список
            filteredOrders = new BindingList<Order>(filtered.ToList());

            // Обновляем источник данных DataGridView
            dataGridView.DataSource = filteredOrders;

            // Обновляем счетчик записей
            UpdateRecordCount();
        }

        /// <summary>
        /// Обновление счетчика количества отображаемых записей
        /// </summary>
        private void UpdateRecordCount()
        {
            lblRecordCount.Text = $"Количество записей: {filteredOrders?.Count ?? 0}";
        }

        /// <summary>
        /// Обработчик кнопки просмотра выбранного заказа
        /// </summary>
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

        /// <summary>
        /// Открытие формы детального просмотра заказа
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        private void OpenOrderDetails(int orderId)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Открываем форму просмотра заказа с передачей ID
            ViewingOrderAdmin detailsForm = new ViewingOrderAdmin(orderId);
            detailsForm.ShowDialog();

            // После закрытия возвращаемся и обновляем данные
            this.Visible = true;
            LoadOrders(); // Перезагрузка данных для отображения возможных изменений
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
        /// Обработчик кнопки сброса всех фильтров
        /// Возвращает фильтры к значениям по умолчанию
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            // Очистка поискового запроса
            txtSearch.Text = "";

            // Сброс дат на период последнего месяца
            dtpStartDate.Value = DateTime.Today.AddMonths(-1);
            dtpEndDate.Value = DateTime.Today;

            // Сброс статуса на "Все статусы"
            cmbStatus.SelectedIndex = 0;

            // Применение сброшенных фильтров
            ApplyFilters();
        }

        /// <summary>
        /// Обработчик двойного щелчка по ячейке DataGridView
        /// Открывает детальный просмотр выбранного заказа
        /// </summary>
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Проверка, что щелчок был по строке, а не по заголовку
            {
                int orderId = (int)dataGridView.Rows[e.RowIndex].Cells["ID"].Value;
                OpenOrderDetails(orderId);
            }
        }

        /// <summary>
        /// Ограничение ввода в поле поиска только цифрами
        /// Так как поиск осуществляется по номеру заказа
        /// </summary>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Блокируем ввод нецифровых символов
            }
        }
    }
}