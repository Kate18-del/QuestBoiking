using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Форма для управления справочной информацией (статусы, категории, уровни сложности)
    /// Предоставляет возможность просмотра, добавления, редактирования и удаления записей в справочниках
    /// </summary>
    public partial class ReferenceBooksAdmin : Form
    {
        // Коллекции для хранения данных справочников с поддержкой привязки к DataGridView
        private BindingList<ReferenceItem> statusList;      // Список статусов
        private BindingList<ReferenceItem> categoriesList;   // Список категорий
        private BindingList<ReferenceItem> difficultyList;   // Список уровней сложности

        // Выбранные элементы в каждой вкладке
        private ReferenceItem selectedStatus;      // Выбранный статус
        private ReferenceItem selectedCategory;    // Выбранная категория
        private ReferenceItem selectedDifficulty;  // Выбранный уровень сложности

        // Репозиторий для работы с данными справочников
        private readonly ReferenceRepository _repository;

        /// <summary>
        /// Конструктор формы справочников
        /// </summary>
        public ReferenceBooksAdmin()
        {
            InitializeComponent();
            // Инициализация репозитория
            _repository = new ReferenceRepository();
            // Настройка формы и загрузка данных
            InitializeForm();
        }

        /// <summary>
        /// Инициализация всех элементов формы
        /// </summary>
        private void InitializeForm()
        {
            // Настройка таблиц для отображения данных
            ConfigureDataGridViews();

            // Загрузка всех данных из базы
            LoadAllData();

            // Настройка обработчиков событий
            SetupEventHandlers();

            // Отключаем кнопки редактирования при старте (нет выбранных записей)
            button5.Enabled = false;  // Для вкладки статусов
            button8.Enabled = false;  // Для вкладки категорий
            button11.Enabled = false; // Для вкладки уровней сложности
        }

        /// <summary>
        /// Настройка всех DataGridView на форме
        /// </summary>
        private void ConfigureDataGridViews()
        {
            // Применяем одинаковую конфигурацию для всех таблиц
            ConfigureSingleGridView(dataGridView2); // Таблица статусов
            ConfigureSingleGridView(dataGridView3); // Таблица категорий
            ConfigureSingleGridView(dataGridView4); // Таблица уровней сложности
        }

        /// <summary>
        /// Настройка отдельного DataGridView
        /// </summary>
        /// <param name="dgv">Настраиваемая таблица</param>
        private void ConfigureSingleGridView(DataGridView dgv)
        {
            // Отключаем автоматическую генерацию столбцов
            dgv.AutoGenerateColumns = false;

            // Настройка режима выделения
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;           // Только для чтения
            dgv.AllowUserToAddRows = false; // Запрещаем добавление строк пользователем

            // Растягиваем колонки на всю доступную ширину
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false; // Убираем заголовки строк (нумерацию)

            // Очищаем существующие столбцы
            dgv.Columns.Clear();

            // Добавляем единственный столбец для отображения наименования
            dgv.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = "Наименование",
                DataPropertyName = "Name", // Привязка к свойству ReferenceItem.Name
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Растягиваем на всё пространство
            });
        }

        /// <summary>
        /// Загрузка всех данных из базы данных
        /// </summary>
        private void LoadAllData()
        {
            try
            {
                // Загружаем данные для каждой вкладки из соответствующих таблиц
                statusList = new BindingList<ReferenceItem>(_repository.GetAllItems("statuses"));
                categoriesList = new BindingList<ReferenceItem>(_repository.GetAllItems("categories"));
                difficultyList = new BindingList<ReferenceItem>(_repository.GetAllItems("difficultylevels"));

                // Привязываем данные к таблицам
                dataGridView2.DataSource = statusList;      // Статусы
                dataGridView3.DataSource = categoriesList;  // Категории
                dataGridView4.DataSource = difficultyList;  // Уровни сложности

                // Снимаем выделение со всех строк
                dataGridView2.ClearSelection();
                dataGridView3.ClearSelection();
                dataGridView4.ClearSelection();

                // Обновляем счетчики записей
                UpdateRecordCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обновление счетчиков количества записей для каждой вкладки
        /// </summary>
        private void UpdateRecordCounts()
        {
            label1.Text = $"Количество записей: {statusList.Count}";      // Статусы
            label2.Text = $"Количество записей: {categoriesList.Count}";  // Категории
            label4.Text = $"Количество записей: {difficultyList.Count}";  // Уровни сложности
        }

        /// <summary>
        /// Настройка всех обработчиков событий
        /// </summary>
        private void SetupEventHandlers()
        {
            // Обработчики выбора строк в таблицах
            dataGridView2.SelectionChanged += (s, e) => HandleSelection(dataGridView2, ref selectedStatus, textBox2);
            dataGridView3.SelectionChanged += (s, e) => HandleSelection(dataGridView3, ref selectedCategory, textBox3);
            dataGridView4.SelectionChanged += (s, e) => HandleSelection(dataGridView4, ref selectedDifficulty, textBox4);

            // Обработчики кнопок "Добавить" для каждой вкладки
            button6.Click += (s, e) => HandleAdd("statuses", textBox2, ref statusList, dataGridView2);
            button9.Click += (s, e) => HandleAdd("categories", textBox3, ref categoriesList, dataGridView3);
            button12.Click += (s, e) => HandleAdd("difficultylevels", textBox4, ref difficultyList, dataGridView4);

            // Обработчики кнопок "Изменить" для каждой вкладки
            button5.Click += (s, e) => HandleEdit(selectedStatus, "statuses", textBox2, ref statusList, dataGridView2);
            button8.Click += (s, e) => HandleEdit(selectedCategory, "categories", textBox3, ref categoriesList, dataGridView3);
            button11.Click += (s, e) => HandleEdit(selectedDifficulty, "difficultylevels", textBox4, ref difficultyList, dataGridView4);

            // Обработчики нажатия клавиш в таблицах (для удаления по Delete)
            dataGridView2.KeyDown += (s, e) => HandleDeleteKey(e, dataGridView2, "statuses", ref statusList, ref selectedStatus);
            dataGridView3.KeyDown += (s, e) => HandleDeleteKey(e, dataGridView3, "categories", ref categoriesList, ref selectedCategory);
            dataGridView4.KeyDown += (s, e) => HandleDeleteKey(e, dataGridView4, "difficultylevels", ref difficultyList, ref selectedDifficulty);

            // Настройка плейсхолдеров для текстовых полей
            SetupTextBoxPlaceholders();
        }

        /// <summary>
        /// Настройка плейсхолдеров для всех текстовых полей
        /// </summary>
        private void SetupTextBoxPlaceholders()
        {
            SetupSingleTextBox(textBox2, "Наименование");
            SetupSingleTextBox(textBox3, "Наименование");
            SetupSingleTextBox(textBox4, "Наименование");
        }

        /// <summary>
        /// Настройка плейсхолдера для отдельного текстового поля
        /// </summary>
        /// <param name="textBox">Настраиваемое поле</param>
        /// <param name="placeholder">Текст плейсхолдера</param>
        private void SetupSingleTextBox(TextBox textBox, string placeholder)
        {
            // При получении фокуса - очищаем поле, если там плейсхолдер
            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = SystemColors.WindowText;
                }
            };

            // При потере фокуса - восстанавливаем плейсхолдер, если поле пустое
            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = SystemColors.ScrollBar;
                }
            };
        }

        /// <summary>
        /// Обработка выбора строки в таблице
        /// </summary>
        /// <param name="dgv">Таблица, в которой произошло событие</param>
        /// <param name="selectedItem">Ссылка на выбранный элемент</param>
        /// <param name="textBox">Текстовое поле для отображения</param>
        private void HandleSelection(DataGridView dgv, ref ReferenceItem selectedItem, TextBox textBox)
        {
            // Проверяем, что действительно выбрана строка
            if (dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].DataBoundItem is ReferenceItem item)
            {
                selectedItem = item;
                textBox.Text = item.Name;
                textBox.ForeColor = SystemColors.WindowText;

                // Включаем кнопку "Изменить" для соответствующей вкладки
                UpdateButtonsState(dgv, true);
            }
            else
            {
                // Если выбор снят - отключаем кнопку и очищаем поле
                UpdateButtonsState(dgv, false);
                ClearTextBox(textBox);
            }
        }

        /// <summary>
        /// Обновление состояния кнопок редактирования в зависимости от выбора
        /// </summary>
        /// <param name="dgv">Таблица, для которой обновляется кнопка</param>
        /// <param name="isEnabled">Доступна ли кнопка</param>
        private void UpdateButtonsState(DataGridView dgv, bool isEnabled)
        {
            // Включаем/отключаем соответствующую кнопку
            if (dgv == dataGridView2)
                button5.Enabled = isEnabled;
            else if (dgv == dataGridView3)
                button8.Enabled = isEnabled;
            else if (dgv == dataGridView4)
                button11.Enabled = isEnabled;
        }

        /// <summary>
        /// Обработка добавления новой записи
        /// </summary>
        /// <param name="tableName">Название таблицы в БД</param>
        /// <param name="textBox">Поле ввода наименования</param>
        /// <param name="list">Ссылка на список записей</param>
        /// <param name="dgv">Таблица для отображения</param>
        private void HandleAdd(string tableName, TextBox textBox, ref BindingList<ReferenceItem> list, DataGridView dgv)
        {
            // Проверка корректности ввода
            if (!ValidateInput(textBox))
                return;

            string newName = textBox.Text.Trim();

            // Проверяем, существует ли уже такая запись
            if (ItemExists(tableName, newName, null))
            {
                MessageBox.Show($"Запись '{newName}' уже существует!\nПожалуйста, введите уникальное наименование.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Focus();
                textBox.SelectAll();
                return;
            }

            try
            {
                // Добавление записи через репозиторий
                if (_repository.AddItem(tableName, newName))
                {
                    MessageBox.Show("Запись успешно добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Обновление данных
                    var items = _repository.GetAllItems(tableName);
                    list = new BindingList<ReferenceItem>(items);
                    dgv.DataSource = list;
                    UpdateRecordCounts();
                    ClearTextBox(textBox);
                    dgv.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработка редактирования существующей записи
        /// </summary>
        /// <param name="selectedItem">Выбранный элемент</param>
        /// <param name="tableName">Название таблицы в БД</param>
        /// <param name="textBox">Поле ввода наименования</param>
        /// <param name="list">Ссылка на список записей</param>
        /// <param name="dgv">Таблица для отображения</param>
        private void HandleEdit(ReferenceItem selectedItem, string tableName, TextBox textBox,
                               ref BindingList<ReferenceItem> list, DataGridView dgv)
        {
            if (selectedItem == null || !ValidateInput(textBox))
                return;

            string newName = textBox.Text.Trim();

            // Если название не изменилось, разрешаем редактирование
            if (newName.Equals(selectedItem.Name, StringComparison.OrdinalIgnoreCase))
            {
                // Название не изменилось, можно обновлять
            }
            else
            {
                // Проверяем, существует ли уже такая запись (кроме редактируемой)
                if (ItemExists(tableName, newName, selectedItem.ID))
                {
                    MessageBox.Show($"Запись '{newName}' уже существует!\nПожалуйста, введите уникальное наименование.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Focus();
                    textBox.SelectAll();
                    return;
                }
            }

            try
            {
                // Обновление записи через репозиторий
                if (_repository.UpdateItem(tableName, selectedItem.ID, newName))
                {
                    MessageBox.Show("Запись успешно обновлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Обновление данных
                    var items = _repository.GetAllItems(tableName);
                    list = new BindingList<ReferenceItem>(items);
                    dgv.DataSource = list;
                    UpdateRecordCounts();
                    ClearTextBox(textBox);
                    dgv.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Проверка существования записи с таким наименованием
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="name">Проверяемое наименование</param>
        /// <param name="excludeId">ID записи, которую нужно исключить из проверки (для редактирования)</param>
        /// <returns>true если запись существует, false в противном случае</returns>
        private bool ItemExists(string tableName, string name, int? excludeId = null)
        {
            try
            {
                // Получаем все записи из таблицы
                var allItems = _repository.GetAllItems(tableName);

                // Проверяем существование записи с таким именем
                foreach (var item in allItems)
                {
                    // Игнорируем регистр при сравнении
                    if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Если excludeId задан, игнорируем запись с этим ID (для редактирования)
                        if (excludeId.HasValue && item.ID == excludeId.Value)
                            continue;

                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Обработка нажатия клавиши Delete для удаления записи
        /// </summary>
        /// <param name="e">Параметры события клавиатуры</param>
        /// <param name="dgv">Таблица, в которой произошло событие</param>
        /// <param name="tableName">Название таблицы в БД</param>
        /// <param name="list">Ссылка на список записей</param>
        /// <param name="selectedItem">Ссылка на выбранный элемент</param>
        private void HandleDeleteKey(KeyEventArgs e, DataGridView dgv, string tableName,
                                    ref BindingList<ReferenceItem> list, ref ReferenceItem selectedItem)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;

                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите запись для удаления!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedItem = dgv.SelectedRows[0].DataBoundItem as ReferenceItem;

                if (selectedItem == null)
                    return;

                // Подтверждение удаления
                DialogResult result = MessageBox.Show(
                    $"Вы точно хотите удалить запись:\n{selectedItem.Name}?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Удаление записи через репозиторий
                        if (_repository.DeleteItem(tableName, selectedItem.ID))
                        {
                            MessageBox.Show("Запись успешно удалена!", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Обновление данных
                            var items = _repository.GetAllItems(tableName);
                            list = new BindingList<ReferenceItem>(items);
                            dgv.DataSource = list;
                            UpdateRecordCounts();
                            ClearTextBox(GetTextBoxByDataGridView(dgv));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Получение соответствующего текстового поля по таблице
        /// </summary>
        /// <param name="dgv">Таблица</param>
        /// <returns>Текстовое поле для ввода</returns>
        private TextBox GetTextBoxByDataGridView(DataGridView dgv)
        {
            if (dgv == dataGridView2) return textBox2;
            if (dgv == dataGridView3) return textBox3;
            return textBox4;
        }

        /// <summary>
        /// Проверка корректности ввода в текстовом поле
        /// </summary>
        /// <param name="textBox">Проверяемое поле</param>
        /// <returns>true если ввод корректен</returns>
        private bool ValidateInput(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == "Наименование")
            {
                MessageBox.Show("Введите наименование!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Очистка текстового поля и установка плейсхолдера
        /// </summary>
        /// <param name="textBox">Очищаемое поле</param>
        private void ClearTextBox(TextBox textBox)
        {
            textBox.Text = "Наименование";
            textBox.ForeColor = SystemColors.ScrollBar;
        }

        /// <summary>
        /// Обработчики кнопок возврата в главное меню для разных вкладок
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainAdmin auto = new MainAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        private void btnMenu2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainAdmin auto = new MainAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        private void btnMenu3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainAdmin auto = new MainAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        private void btnMenu4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainAdmin auto = new MainAdmin();
            auto.ShowDialog();
            this.Visible = true;
        }

        /// <summary>
        /// Обработчики ввода символов в текстовые поля
        /// Ограничивают ввод только русскими буквами и пробелами
        /// </summary>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверка на русские буквы и пробелы
            bool isRussian = Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ\s]");

            // Разрешаем только русские буквы, пробелы и управляющие символы
            if (!isRussian && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isRussian = Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ\s]");

            if (!isRussian && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isRussian = Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ\s]");

            if (!isRussian && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}