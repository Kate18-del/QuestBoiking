using MySql.Data.MySqlClient;
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
using System.IO;

namespace prototip
{
    /// <summary>
    /// Форма для просмотра и управления услугами (квестами) с правами директора
    /// Предоставляет возможность просмотра, фильтрации и поиска услуг
    /// </summary>
    public partial class ServicesDirector : Form
    {
        // Коллекции для хранения услуг с поддержкой привязки к DataGridView
        private BindingList<Service> allServices;      // Все услуги из базы данных
        private BindingList<Service> filteredServices;  // Отфильтрованные услуги

        /// <summary>
        /// Конструктор формы управления услугами
        /// </summary>
        public ServicesDirector()
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

            // Загрузка данных для фильтров (категории и уровни сложности)
            LoadFilterData();

            // Загрузка услуг из базы данных
            LoadServices();

            // Подписка на события для автоматического обновления
            SubscribeToEvents();

            // Отображение информации о текущем директоре
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
                DataPropertyName = "ServiceImage", // Привязка к свойству Service.ServiceImage
                Width = 150,
                ImageLayout = DataGridViewImageCellLayout.Zoom // Масштабирование изображения с сохранением пропорций
            });

            // Артикул (скрытая колонка для хранения ID)
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Article",
                HeaderText = "Артикул",
                DataPropertyName = "Article",
                Width = 70,
                Visible = false, // Скрываем от пользователя
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
                DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True } // Перенос текста
            });

            // Цена с форматированием
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = "Цена",
                DataPropertyName = "Price",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "0.##' руб.'" } // Формат с валютой
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
        }

        /// <summary>
        /// Загрузка данных для фильтров (категории и уровни сложности) из базы данных
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

                    // ЗАГРУЗКА УРОВНЕЙ СЛОЖНОСТИ
                    comboBox2.Items.Clear();
                    comboBox2.Items.Add("Все уровни"); // Опция для отображения всех уровней

                    cmd = new MySqlCommand("SELECT Name FROM difficultylevels", conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader["Name"].ToString());
                        }
                    }
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
        /// Подписка на события элементов управления для автоматического обновления фильтров
        /// </summary>
        private void SubscribeToEvents()
        {
            textBox1.TextChanged += OnFilterChanged;          // Поиск по названию
            comboBox1.SelectedIndexChanged += OnFilterChanged; // Фильтр по категории
            comboBox2.SelectedIndexChanged += OnFilterChanged; // Фильтр по сложности
            button1.Click += btnReset_Click;                   // Кнопка сброса фильтров
        }

        /// <summary>
        /// Общий обработчик изменения любого фильтра
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Применение всех активных фильтров к списку услуг
        /// </summary>
        private void ApplyFilters()
        {
            if (allServices == null) return;

            // Начинаем со всех услуг
            var filtered = allServices.AsEnumerable();

            // 1. ПОИСК ПО НАЗВАНИЮ
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text != "Поиск")
            {
                string searchText = textBox1.Text.Trim();
                // Поиск с учетом регистра (OrdinalIgnoreCase)
                filtered = filtered.Where(s => s.Name.StartsWith(searchText, StringComparison.OrdinalIgnoreCase));
            }

            // 2. ФИЛЬТР ПО КАТЕГОРИИ
            if (comboBox1.SelectedIndex > 0) // Индекс 0 - "Все категории"
            {
                string selectedCategory = comboBox1.SelectedItem.ToString();
                filtered = filtered.Where(s => s.CategoryName == selectedCategory);
            }

            // 3. ФИЛЬТР ПО УРОВНЮ СЛОЖНОСТИ
            if (comboBox2.SelectedIndex > 0) // Индекс 0 - "Все уровни"
            {
                string selectedDifficulty = comboBox2.SelectedItem.ToString();
                filtered = filtered.Where(s => s.DifficultyLevel == selectedDifficulty);
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
        /// Отображение информации о текущем директоре
        /// Формирует краткое ФИО в формате "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                label2.Text = $"директор {shortName}";
            }
        }

        /// <summary>
        /// Обработчик кнопки сброса всех фильтров
        /// Возвращает фильтры к значениям по умолчанию
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            // Сброс поискового запроса
            textBox1.Text = "Поиск";
            textBox1.ForeColor = SystemColors.ScrollBar;

            // Сброс фильтров категории и сложности
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            // Применение сброшенных фильтров
            ApplyFilters();
        }

        #region Обработчики событий текстового поля поиска

        /// <summary>
        /// Обработчик получения фокуса полем поиска
        /// Очищает поле от плейсхолдера
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
        /// Восстанавливает плейсхолдер если поле пустое
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
        /// Ограничение ввода в поле поиска только русскими буквами
        /// Поиск осуществляется по названиям на русском языке
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только русские буквы и управляющие символы (Backspace, Enter и т.д.)
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"[а-яА-ЯёЁ]") && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        #endregion

        /// <summary>
        /// Обработчик кнопки возврата в главное меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainDirector auto = new MainDirector();
            auto.ShowDialog();
            this.Visible = true;
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

                // Получаем соответствующий объект Service из привязанных данных
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
            // Пробуем несколько возможных путей

            // 1. Относительный путь от текущей директории приложения
            string path1 = Path.Combine(Environment.CurrentDirectory, "Images", imageFileName);
            if (File.Exists(path1)) return path1;

            // 2. Путь от папки запуска приложения
            string path2 = Path.Combine(Application.StartupPath, "Images", imageFileName);
            if (File.Exists(path2)) return path2;

            // 3. Абсолютный путь (если в БД хранится полный путь)
            if (Path.IsPathRooted(imageFileName) && File.Exists(imageFileName))
                return imageFileName;

            // 4. Если файл не найден, возвращаем null
            return null;
        }

        /// <summary>
        /// Установка изображения по умолчанию для строки DataGridView
        /// Используется когда файл изображения не найден или произошла ошибка
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
                // Если нет ресурса или другая ошибка, устанавливаем пустое значение
                row.Cells["Picture"].Value = null;
            }
        }
    }
}