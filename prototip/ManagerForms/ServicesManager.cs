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

namespace prototip
{
    /// <summary>
    /// Форма для просмотра и управления услугами (квестами) с правами менеджера
    /// Предоставляет возможность просмотра, фильтрации, сортировки и управления изображениями
    /// </summary>
    public partial class ServicesManager : Form
    {
        // Коллекции для хранения услуг с поддержкой привязки к DataGridView
        private BindingList<Service> allServices;        // Все услуги из базы данных
        private BindingList<Service> filteredServices;    // Отфильтрованные услуги

        // Текущий порядок сортировки
        private string currentSortOrder = "По возрастанию";

        /// <summary>
        /// Конструктор формы просмотра услуг для менеджера
        /// </summary>
        public ServicesManager()
        {
            InitializeComponent();
            // Инициализация всех элементов формы
            InitializeForm();
        }

        /// <summary>
        /// Инициализация всех элементов формы
        /// </summary>
        private void InitializeForm()
        {
            // Настройка таблицы для отображения услуг
            ConfigureDataGridView();

            // Загрузка данных для фильтров (категории)
            LoadFilterData();

            // Загрузка услуг из базы данных
            LoadServices();

            // Подписка на события для автоматического обновления
            SubscribeToEvents();

            // Отображение информации о текущем менеджере
            DisplayCurrentUser();
        }

        /// <summary>
        /// Настройка столбцов DataGridView для отображения информации об услугах
        /// Включает колонку с изображением и настройку высоты строк
        /// </summary>
        private void ConfigureDataGridView()
        {
            // Отключаем автоматическую генерацию столбцов
            dataGridView2.AutoGenerateColumns = false;

            // Настройка режимов выделения и редактирования
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.ReadOnly = true;           // Только для чтения
            dataGridView2.AllowUserToAddRows = false; // Запрет добавления строк пользователем

            // Установка высоты строк (120 пикселей для комфортного отображения изображений)
            dataGridView2.RowTemplate.Height = 120;

            // Установка шрифта для всех ячеек
            this.dataGridView2.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);

            // Очистка существующих столбцов
            dataGridView2.Columns.Clear();

            // Добавление колонки для изображения (специальный тип DataGridViewImageColumn)
            dataGridView2.Columns.Add(new DataGridViewImageColumn()
            {
                Name = "Picture",
                HeaderText = "Изображение",
                DataPropertyName = "ServiceImage",
                Width = 150,
                ImageLayout = DataGridViewImageCellLayout.Zoom // Масштабирование с сохранением пропорций
            });

            // Артикул (уникальный идентификатор)
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Article",
                HeaderText = "Артикул",
                DataPropertyName = "Article",
                Width = 70
            });

            // Название услуги
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = "Название",
                DataPropertyName = "Name",
                Width = 180
            });

            // Описание услуги с поддержкой переноса текста
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Description",
                HeaderText = "Описание",
                DataPropertyName = "Description",
                Width = 250,
                DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True }
            });

            // Цена с форматированием
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = "Цена",
                DataPropertyName = "Price",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "0.##' руб.'" }
            });

            // Время прохождения
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Time",
                HeaderText = "Время (мин)",
                DataPropertyName = "Time",
                Width = 80
            });

            // Максимальное количество участников
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MaxPeople",
                HeaderText = "Макс. людей",
                DataPropertyName = "MaxPeople",
                Width = 100
            });

            // Уровень сложности
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DifficultyLevel",
                HeaderText = "Сложность",
                DataPropertyName = "DifficultyLevel",
                Width = 100
            });

            // Категория
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CategoryName",
                HeaderText = "Категория",
                DataPropertyName = "CategoryName",
                Width = 130
            });

            // Настройка автоматического изменения высоты строк для многострочного текста
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Подписка на событие завершения привязки данных для загрузки изображений
            dataGridView2.DataBindingComplete += DataGridView2_DataBindingComplete;

            // Добавление обработчика для правой кнопки мыши (контекстное меню изображений)
            dataGridView2.CellMouseClick += DataGridView2_CellMouseClick;
        }

        /// <summary>
        /// Загрузка данных для фильтров (категории) из базы данных
        /// </summary>
        private void LoadFilterData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // ЗАГРУЗКА КАТЕГОРИЙ
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Все категории"); // Опция для отображения всех категорий

                    MySqlCommand cmd = new MySqlCommand("SELECT Categorie FROM categories", conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["Categorie"].ToString());
                        }
                    }

                    // ЗАГРУЗКА ВАРИАНТОВ СОРТИРОВКИ
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(new[] { "По возрастанию", "По убыванию" });
                }

                // Установка значений по умолчанию
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фильтров: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загрузка всех услуг из базы данных с присоединением связанных данных
        /// </summary>
        private void LoadServices()
        {
            try
            {
                // SQL-запрос с LEFT JOIN для получения данных из связанных таблиц
                string query = @"
            SELECT 
                s.Article,
                s.Name,
                s.Description,
                s.Price,
                s.Time,
                s.DayOfTheWeek,
                s.Picture,
                s.MaxPeople,
                s.ISLevel,
                dl.Name as DifficultyLevel,
                s.IDCategory,
                c.Categorie as CategoryName
            FROM services s
            LEFT JOIN difficultylevels dl ON s.ISLevel = dl.DifficultyID
            LEFT JOIN categories c ON s.IDCategory = c.CategoriesID
            ORDER BY s.Name";

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        allServices = new BindingList<Service>();

                        while (reader.Read())
                        {
                            // Создание объекта Service и заполнение данными из БД
                            var service = new Service
                            {
                                Article = Convert.ToInt32(reader["Article"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Time = Convert.ToInt32(reader["Time"]),
                                DayOfTheWeek = reader["DayOfTheWeek"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["DayOfTheWeek"]),
                                Picture = reader["Picture"] == DBNull.Value ? null : reader["Picture"].ToString(),
                                MaxPeople = Convert.ToInt32(reader["MaxPeople"]),
                                ISLevel = Convert.ToInt32(reader["ISLevel"]),
                                DifficultyLevel = reader["DifficultyLevel"].ToString(),
                                IDCategory = Convert.ToInt32(reader["IDCategory"]),
                                CategoryName = reader["CategoryName"].ToString()
                            };

                            allServices.Add(service);
                        }
                    }
                }

                // Применение фильтров после загрузки
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Подписка на события элементов управления
        /// </summary>
        private void SubscribeToEvents()
        {
            // Поиск по артикулу - "живой" поиск при вводе
            textBox1.TextChanged += OnFilterChanged;

            // Фильтрация по категории
            comboBox1.SelectedIndexChanged += OnFilterChanged;

            // Сортировка по наименованию
            comboBox2.SelectedIndexChanged += OnSortChanged;

            // Сброс фильтров
            button1.Click += btnReset_Click;

            // Кнопка меню
            btnMenu.Click += btnMenu_Click;

            // Плейсхолдер для поиска
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            // Валидация ввода для поиска (только цифры)
            textBox1.KeyPress += textBox1_KeyPress;
        }

        /// <summary>
        /// Обработчик изменения фильтров поиска и категории
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Обработчик изменения сортировки
        /// </summary>
        private void OnSortChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                currentSortOrder = comboBox2.SelectedItem.ToString();
                ApplyFilters(); // Применяем фильтры с новой сортировкой
            }
        }

        /// <summary>
        /// Применение всех активных фильтров и сортировки к списку услуг
        /// </summary>
        private void ApplyFilters()
        {
            if (allServices == null) return;

            // Начинаем со всех услуг
            var filtered = allServices.AsEnumerable();

            // 1. ПОИСК ПО АРТИКУЛУ - фильтрация при совпадении с начала поля
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text != "Поиск")
            {
                string searchText = textBox1.Text.Trim();
                filtered = filtered.Where(s => s.Article.ToString().StartsWith(searchText));
            }

            // 2. ФИЛЬТРАЦИЯ ПО КАТЕГОРИИ
            if (comboBox1.SelectedIndex > 0) // Индекс 0 - "Все категории"
            {
                string selectedCategory = comboBox1.SelectedItem.ToString();
                filtered = filtered.Where(s => s.CategoryName == selectedCategory);
            }

            // 3. СОРТИРОВКА ПО НАИМЕНОВАНИЮ
            if (currentSortOrder == "По возрастанию")
            {
                filtered = filtered.OrderBy(s => s.Name);
            }
            else // "По убыванию"
            {
                filtered = filtered.OrderByDescending(s => s.Name);
            }

            // Сохраняем отфильтрованный список и обновляем источник данных
            filteredServices = new BindingList<Service>(filtered.ToList());
            dataGridView2.DataSource = filteredServices;

            // Обновляем счетчик записей
            UpdateRecordCount();
        }

        /// <summary>
        /// Обновление счетчика количества отображаемых записей
        /// </summary>
        private void UpdateRecordCount()
        {
            label3.Text = $"Количество записей: {filteredServices?.Count ?? 0}";
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
        /// Обработчик кнопки сброса всех фильтров
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            // Сброс поиска
            textBox1.Text = "Поиск";
            textBox1.ForeColor = SystemColors.ScrollBar;

            // Сброс фильтра категории
            comboBox1.SelectedIndex = 0;

            // Сброс сортировки (устанавливаем по возрастанию)
            comboBox2.SelectedIndex = 0;

            // Применяем фильтры (покажем все записи)
            ApplyFilters();
        }

        /// <summary>
        /// Обработчик завершения привязки данных к DataGridView
        /// Запускает загрузку изображений для каждой строки
        /// </summary>
        private void DataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            LoadImagesForDataGridView();
        }

        /// <summary>
        /// Загрузка изображений для всех строк DataGridView
        /// Обрабатывает пути к файлам и устанавливает изображения по умолчанию при ошибках
        /// </summary>
        private void LoadImagesForDataGridView()
        {
            // Если нет данных, выходим
            if (filteredServices == null || filteredServices.Count == 0)
                return;

            // Проходим по всем строкам DataGridView
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView2.Rows[i];

                // Получаем соответствующий объект Service
                if (row.DataBoundItem is Service service)
                {
                    // Проверяем, есть ли имя файла изображения
                    if (!string.IsNullOrEmpty(service.Picture))
                    {
                        try
                        {
                            // Формируем путь к изображению
                            string imagePath = GetImagePath(service.Picture);

                            // Проверяем существование файла
                            if (File.Exists(imagePath))
                            {
                                // Загружаем изображение из файла
                                Image img = Image.FromFile(imagePath);

                                // Устанавливаем изображение в ячейку
                                row.Cells["Picture"].Value = img;

                                // Сохраняем путь в Tag ячейки для возможного последующего использования
                                row.Cells["Picture"].Tag = imagePath;
                            }
                            else
                            {
                                // Если файл не найден, устанавливаем изображение по умолчанию
                                SetDefaultImage(row);
                            }
                        }
                        catch (Exception ex)
                        {
                            // В случае ошибки загрузки устанавливаем изображение по умолчанию
                            SetDefaultImage(row);
                            Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                        }
                    }
                    else
                    {
                        // Если нет имени файла, устанавливаем изображение по умолчанию
                        SetDefaultImage(row);
                    }
                }
            }
        }

        /// <summary>
        /// Получение полного пути к файлу изображения
        /// Проверяет несколько возможных расположений файла
        /// </summary>
        /// <param name="imageFileName">Имя файла изображения из БД</param>
        /// <returns>Полный путь к файлу или null, если файл не найден</returns>
        private string GetImagePath(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return null;

            // 1. Сначала проверяем в AppData
            string appDataPath = Path.Combine(GetImagesFolderPath(), imageFileName);
            if (File.Exists(appDataPath))
                return appDataPath;

            // 2. Затем проверяем в папке приложения (для обратной совместимости)
            string appPath = Path.Combine(Application.StartupPath, "Images", imageFileName);
            if (File.Exists(appPath))
                return appPath;

            // 3. Проверяем абсолютный путь
            if (Path.IsPathRooted(imageFileName) && File.Exists(imageFileName))
                return imageFileName;

            return null;
        }

        /// <summary>
        /// Установка изображения по умолчанию для строки DataGridView
        /// </summary>
        /// <param name="row">Строка DataGridView</param>
        private void SetDefaultImage(DataGridViewRow row)
        {
            try
            {
                // Устанавливаем изображение по умолчанию из ресурсов проекта
                row.Cells["Picture"].Value = global::prototip.Properties.Resources.zagl;
            }
            catch
            {
                // Если нет ресурса, устанавливаем пустое значение
                row.Cells["Picture"].Value = null;
            }
        }

        #region Обработчики поля поиска

        /// <summary>
        /// Обработчик получения фокуса полем поиска
        /// </summary>
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Поиск")
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем поиска
        /// </summary>
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Поиск";
                textBox1.ForeColor = SystemColors.ScrollBar;
            }
        }

        /// <summary>
        /// Ограничение ввода в поле поиска только цифрами
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие символы (Backspace, Delete)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        /// <summary>
        /// Обработчик кнопки добавления новой услуги
        /// </summary>
        private void btnAddServices_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ServiceAdd auto = new ServiceAdd();
            auto.ShowDialog();
            this.Visible = true;
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

        #region Управление изображениями через контекстное меню

        /// <summary>
        /// Обработчик клика правой кнопкой мыши по ячейке
        /// Показывает контекстное меню для управления изображением
        /// </summary>
        private void DataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Проверяем, что клик был правой кнопкой мыши и по ячейке с изображением
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dataGridView2.Columns[e.ColumnIndex];
                if (column.Name == "Picture")
                {
                    // Получаем выбранную строку
                    DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];

                    // Получаем артикул услуги из строки
                    if (selectedRow.Cells["Article"].Value != null)
                    {
                        int article = Convert.ToInt32(selectedRow.Cells["Article"].Value);

                        // Создаем контекстное меню
                        ContextMenuStrip contextMenu = new ContextMenuStrip();

                        // Добавляем пункты меню
                        ToolStripMenuItem changeItem = new ToolStripMenuItem("Изменить изображение");
                        ToolStripMenuItem deleteItem = new ToolStripMenuItem("Удалить изображение");

                        // Привязываем обработчики с передачей артикула
                        changeItem.Click += (s, args) => ChangeImage(article, selectedRow);
                        deleteItem.Click += (s, args) => DeleteImage(article, selectedRow);

                        contextMenu.Items.Add(changeItem);
                        contextMenu.Items.Add(deleteItem);

                        // Показываем меню в позиции клика
                        contextMenu.Show(dataGridView2, e.Location);
                    }
                }
            }
        }

        /// <summary>
        /// Метод для изменения изображения услуги
        /// </summary>
        /// <param name="article">Артикул услуги</param>
        /// <param name="row">Строка DataGridView</param>
        private void ChangeImage(int article, DataGridViewRow row)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
                    openFileDialog.Title = "Выберите новое изображение";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string imagePath = openFileDialog.FileName;

                        // Проверяем размер файла (максимум 2MB)
                        FileInfo fileInfo = new FileInfo(imagePath);
                        if (fileInfo.Length > 2 * 1024 * 1024)
                        {
                            MessageBox.Show("Размер файла не должен превышать 2MB", "Ошибка");
                            return;
                        }

                        // ПРОВЕРКА НА ДУБЛИКАТ
                        if (IsImageDuplicate(imagePath, article))
                        {
                            return; // Прерываем выполнение, если найден дубликат
                        }

                        // Читаем файл
                        byte[] imageData = File.ReadAllBytes(imagePath);
                        string fileName = Path.GetFileName(imagePath);
                        string extension = Path.GetExtension(fileName);

                        // Генерируем новое уникальное имя файла
                        string newFileName = $"service_{article}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                        // Сохраняем изображение на диск
                        SaveImageToDisk(newFileName, imageData);

                        // Обновляем запись в БД
                        UpdateImageInDatabase(article, newFileName);

                        // Обновляем изображение в DataGridView
                        UpdateImageInDataGridView(article, row, newFileName);

                        MessageBox.Show("Изображение успешно изменено!", "Успех");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении изображения: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Метод для удаления изображения услуги
        /// </summary>
        /// <param name="article">Артикул услуги</param>
        /// <param name="row">Строка DataGridView</param>
        private void DeleteImage(int article, DataGridViewRow row)
        {
            try
            {
                // Запрос подтверждения
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить изображение?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Получаем старое имя файла для удаления с диска
                    string oldFileName = GetCurrentImageFileName(article);

                    // Удаляем файл с диска (если он существует)
                    if (!string.IsNullOrEmpty(oldFileName))
                    {
                        string oldImagePath = GetImagePath(oldFileName);
                        if (File.Exists(oldImagePath))
                        {
                            try
                            {
                                File.Delete(oldImagePath);
                            }
                            catch
                            {
                                // Игнорируем ошибки при удалении файла
                            }
                        }
                    }

                    // Обновляем запись в БД (устанавливаем Picture в NULL)
                    UpdateImageInDatabase(article, null);

                    // Обновляем DataGridView (устанавливаем заглушку)
                    row.Cells["Picture"].Value = global::prototip.Properties.Resources.zagl;

                    MessageBox.Show("Изображение успешно удалено!", "Успех");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении изображения: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Сохраняет изображение на диск
        /// </summary>
        private void SaveImageToDisk(string fileName, byte[] imageData)
        {
            try
            {
                string imagesFolder = GetImagesFolderPath();
                string fullPath = Path.Combine(imagesFolder, fileName);

                File.WriteAllBytes(fullPath, imageData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения изображения: {ex.Message}");
            }
        }


        /// <summary>
        /// Получение пути к папке для хранения изображений в AppData
        /// </summary>
        private string GetImagesFolderPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataPath, "QuestManager", "Images");

            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            return appFolder;
        }

        /// <summary>
        /// Обновляет имя файла изображения в БД
        /// </summary>
        private void UpdateImageInDatabase(int article, string fileName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "UPDATE services SET Picture = @picture WHERE Article = @article";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            cmd.Parameters.AddWithValue("@picture", fileName);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@picture", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@article", article);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Услуга с указанным артикулом не найдена", "Ошибка");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления БД: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет изображение в DataGridView
        /// </summary>
        private void UpdateImageInDataGridView(int article, DataGridViewRow row, string fileName)
        {
            try
            {
                // Получаем путь к новому изображению
                string imagePath = GetImagePath(fileName);

                if (File.Exists(imagePath))
                {
                    // Загружаем новое изображение
                    Image img = Image.FromFile(imagePath);

                    // Обновляем ячейку
                    row.Cells["Picture"].Value = img;
                    row.Cells["Picture"].Tag = new { Path = imagePath, Article = article };
                }
                else
                {
                    // Если файл не найден, устанавливаем заглушку
                    row.Cells["Picture"].Value = global::prototip.Properties.Resources.zagl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления отображения: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Получает текущее имя файла изображения из БД
        /// </summary>
        private string GetCurrentImageFileName(int article)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT Picture FROM services WHERE Article = @article";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@article", article);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch
            {
                // В случае ошибки возвращаем null
            }

            return null;
        }

        /// <summary>
        /// Проверка на дубликат изображения
        /// </summary>
        /// <param name="imagePath">Путь к новому изображению</param>
        /// <param name="currentArticle">Артикул текущей услуги</param>
        /// <returns>true если дубликат найден</returns>
        private bool IsImageDuplicate(string imagePath, int currentArticle)
        {
            try
            {
                // Вычисляем хеш файла (MD5)
                string fileHash = ComputeFileHash(imagePath);

                // Получаем размер файла
                FileInfo newFileInfo = new FileInfo(imagePath);
                long fileSize = newFileInfo.Length;

                // Проверяем, есть ли уже такое изображение в БД
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT s.Article, s.Picture 
                FROM services s 
                WHERE s.Picture IS NOT NULL 
                  AND s.Article != @currentArticle";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@currentArticle", currentArticle);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string existingFileName = reader["Picture"].ToString();
                                int existingArticle = Convert.ToInt32(reader["Article"]);

                                if (string.IsNullOrEmpty(existingFileName))
                                    continue;

                                // Получаем полный путь к существующему файлу
                                string existingFilePath = GetImagePath(existingFileName);

                                if (File.Exists(existingFilePath))
                                {
                                    // Сравниваем размеры
                                    FileInfo existingFileInfo = new FileInfo(existingFilePath);

                                    // Если размеры совпадают, сравниваем хеши
                                    if (existingFileInfo.Length == fileSize)
                                    {
                                        string existingFileHash = ComputeFileHash(existingFilePath);

                                        if (existingFileHash == fileHash)
                                        {
                                            // Нашли дубликат
                                            MessageBox.Show($"Это изображение уже используется для услуги с артикулом {existingArticle}",
                                                "Обнаружен дубликат",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке дубликатов: {ex.Message}", "Ошибка");
                return false; // В случае ошибки разрешаем добавление
            }
        }

        /// <summary>
        /// Вычисление хеша файла (MD5)
        /// </summary>
        private string ComputeFileHash(string filePath)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        #endregion
    }
}
