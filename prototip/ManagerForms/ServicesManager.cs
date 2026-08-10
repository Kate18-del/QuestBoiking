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

        private List<int> foundIndexes = new List<int>();
        private int currentFoundIndex = -1;
        private string lastSearchText = "";

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
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;

            // Установка высоты строк
            dataGridView2.RowTemplate.Height = 140;

            // Установка шрифта для всех ячеек
            dataGridView2.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);
            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Очистка существующих столбцов
            dataGridView2.Columns.Clear();

            // ========================================
            // Колонка 1: Изображение (узкая)
            // ========================================
            DataGridViewImageColumn imgColumn = new DataGridViewImageColumn()
            {
                Name = "Picture",
                HeaderText = "Фото",
                DataPropertyName = "ServiceImage",
                Width = 140,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView2.Columns.Add(imgColumn);

            // ========================================
            // Колонка 2: Вся информация (широкая, с WrapMode)
            // ========================================
            DataGridViewTextBoxColumn infoColumn = new DataGridViewTextBoxColumn()
            {
                Name = "FullInfo",
                HeaderText = "Информация об услуге",
                DataPropertyName = "FullInfo",
                Width = 500,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            infoColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            infoColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            infoColumn.DefaultCellStyle.Padding = new Padding(8, 5, 5, 5);
            dataGridView2.Columns.Add(infoColumn);

            // ========================================
            // Колонка 3: Стоимость 
            // ========================================
            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = "Стоимость",
                DataPropertyName = "Price",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "0.##' руб.'",
                    Alignment = DataGridViewContentAlignment.TopCenter,
                    Font = new Font("Comic Sans MS", 11, FontStyle.Bold),
                    Padding = new Padding(5, 10, 5, 5)
                }
            };
            dataGridView2.Columns.Add(priceColumn);

            // Скрытые колонки для привязки данных
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Article",
                DataPropertyName = "Article",
                Visible = false
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                DataPropertyName = "Name",
                Visible = false
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Description",
                DataPropertyName = "Description",
                Visible = false
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Time",
                DataPropertyName = "Time",
                Visible = false
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "DifficultyLevel",
                DataPropertyName = "DifficultyLevel",
                Visible = false
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "CategoryName",
                DataPropertyName = "CategoryName",
                Visible = false
            });

            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MaxPeople",
                DataPropertyName = "MaxPeople",
                Visible = false
            });

            // Настройка чередования цветов строк
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);
            dataGridView2.ColumnHeadersHeight = 40;
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Контекстное меню
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
                    comboBox1.Items.Add("Все категории");

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
                    comboBox2.Items.AddRange(new[] { "Цена по возрастанию", "Цена по убыванию" });
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
                            var service = new Service
                            {
                                Article = Convert.ToInt32(reader["Article"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "",
                                Price = Convert.ToDecimal(reader["Price"]),
                                Time = Convert.ToInt32(reader["Time"]),
                                MaxPeople = Convert.ToInt32(reader["MaxPeople"]),
                                DifficultyLevel = reader["DifficultyLevel"].ToString(),
                                CategoryName = reader["CategoryName"].ToString()
                            };

                            // Изображение
                            if (reader["Picture"] != DBNull.Value)
                            {
                                service.PictureData = (byte[])reader["Picture"];
                            }

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
            // Поиск по названию - "живой" поиск при вводе
            textBox1.TextChanged += textBox1_TextChanged;

            // Фильтрация по категории
            comboBox1.SelectedIndexChanged += OnFilterChanged;

            // Сортировка по цене
            comboBox2.SelectedIndexChanged += OnSortChanged;

            // Сброс фильтров
            button1.Click += btnReset_Click;

            // Кнопка меню
            btnMenu.Click += btnMenu_Click;

            // Плейсхолдер для поиска
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            // Валидация ввода - только русские буквы и пробелы
            textBox1.KeyPress += textBox1_KeyPress;

            // Навигация стрелками между найденными записями
            textBox1.KeyDown += TextBox1_KeyDown;
        }

        /// <summary>
        /// Обработчик нажатия клавиш в поле поиска
        /// Стрелка вниз - переход к следующему совпадению
        /// Стрелка вверх - переход к предыдущему совпадению
        /// </summary>
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Если нет найденных совпадений - ничего не делаем
            if (foundIndexes.Count == 0) return;

            // Переключение между найденными записями по стрелке вниз
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                // Переходим к следующему совпадению
                currentFoundIndex++;
                if (currentFoundIndex >= foundIndexes.Count)
                    currentFoundIndex = 0; // Зацикливаем

                SelectFoundRow(foundIndexes[currentFoundIndex]);
            }
            // Переключение к предыдущему совпадению по стрелке вверх
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                // Переходим к предыдущему совпадению
                currentFoundIndex--;
                if (currentFoundIndex < 0)
                    currentFoundIndex = foundIndexes.Count - 1; // Зацикливаем

                SelectFoundRow(foundIndexes[currentFoundIndex]);
            }
        }

        /// <summary>
        /// Выделение найденной строки и прокрутка к ней
        /// </summary>
        private void SelectFoundRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView2.Rows.Count)
            {
                dataGridView2.ClearSelection();
                dataGridView2.Rows[rowIndex].Selected = true;

                // Прокручиваем к выбранной строке, только если она не видна
                if (rowIndex < dataGridView2.FirstDisplayedScrollingRowIndex ||
                    rowIndex > dataGridView2.FirstDisplayedScrollingRowIndex + dataGridView2.DisplayedRowCount(false) - 1)
                {
                    dataGridView2.FirstDisplayedScrollingRowIndex = rowIndex;
                }
            }
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
                ApplyFilters();
            }
        }

        /// <summary>
        /// Применение всех активных фильтров и сортировки к списку услуг
        /// </summary>
        private void ApplyFilters()
        {
            if (allServices == null) return;

            var filtered = allServices.AsEnumerable();

            // Фильтрация по категории
            if (comboBox1.SelectedIndex > 0)
                filtered = filtered.Where(s => s.CategoryName == comboBox1.SelectedItem.ToString());

            // Сортировка
            if (comboBox2.SelectedItem != null)
            {
                string sort = comboBox2.SelectedItem.ToString();
                filtered = sort == "Цена по возрастанию" ? filtered.OrderBy(s => s.Price) : filtered.OrderByDescending(s => s.Price);
            }

            filteredServices = new BindingList<Service>(filtered.ToList());
            dataGridView2.DataSource = filteredServices;
            UpdateRecordCount();

            // Сбрасываем поиск при смене фильтра
            foundIndexes.Clear();
            currentFoundIndex = -1;
            lastSearchText = "";
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

            // Сброс сортировки
            comboBox2.SelectedIndex = 0;

            // Применяем фильтры
            ApplyFilters();
        }

        /// <summary>
        /// Установка изображения по умолчанию для строки DataGridView
        /// </summary>
        private void SetDefaultImage(DataGridViewRow row)
        {
            try
            {
                row.Cells["Picture"].Value = global::prototip.Properties.Resources.zagl;
            }
            catch
            {
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
        /// Ограничение ввода в поле поиска только русскими буквами и пробелами
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем управляющие символы (Backspace, Delete, стрелки и т.д.)
            if (char.IsControl(e.KeyChar))
                return;

            // Разрешаем пробел
            if (e.KeyChar == ' ')
                return;

            // Проверяем, является ли символ русской буквой (строчной или заглавной)
            if ((e.KeyChar >= 'а' && e.KeyChar <= 'я') ||
                (e.KeyChar >= 'А' && e.KeyChar <= 'Я') ||
                e.KeyChar == 'ё' ||
                e.KeyChar == 'Ё')
                return;

            // Все остальные символы блокируем
            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// Обработчик кнопки добавления новой услуги
        /// </summary>
        private void btnAddServices_Click(object sender, EventArgs e)
        {
            ServiceAdd addForm = new ServiceAdd();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadServices();
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

        #region Управление изображениями через контекстное меню

        /// <summary>
        /// Обработчик клика правой кнопкой мыши по ячейке
        /// </summary>
        private void DataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dataGridView2.Columns[e.ColumnIndex];
                if (column.Name == "Picture")
                {
                    DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];

                    if (selectedRow.Cells["Article"].Value != null)
                    {
                        int article = Convert.ToInt32(selectedRow.Cells["Article"].Value);

                        ContextMenuStrip contextMenu = new ContextMenuStrip();

                        ToolStripMenuItem changeItem = new ToolStripMenuItem("Изменить изображение");
                        ToolStripMenuItem deleteItem = new ToolStripMenuItem("Удалить изображение");

                        changeItem.Click += (s, args) => ChangeImage(article, selectedRow);
                        deleteItem.Click += (s, args) => DeleteImage(article, selectedRow);

                        contextMenu.Items.Add(changeItem);
                        contextMenu.Items.Add(deleteItem);

                        contextMenu.Show(dataGridView2, e.Location);
                    }
                }
            }
        }

        /// <summary>
        /// Метод для изменения изображения услуги
        /// </summary>
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

                        byte[] imageData = File.ReadAllBytes(imagePath);

                        long maxSize = 3 * 1024 * 1024;
                        if (imageData.Length > maxSize)
                        {
                            imageData = CompressImage(imageData, maxSize);
                            MessageBox.Show($"Изображение было сжато до {imageData.Length / 1024}KB", "Сжатие");
                        }

                        UpdateImageInDatabase(article, imageData);

                        if (row.DataBoundItem is Service service)
                        {
                            service.PictureData = imageData;
                        }

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
        private void DeleteImage(int article, DataGridViewRow row)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить изображение?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateImageInDatabase(article, null);

                    if (row.DataBoundItem is Service service)
                    {
                        service.PictureData = null;
                    }

                    MessageBox.Show("Изображение успешно удалено!", "Успех");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении изображения: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Обновляет изображение в БД
        /// </summary>
        private void UpdateImageInDatabase(int article, byte[] imageData)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "UPDATE services SET Picture = @picture WHERE Article = @article";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (imageData != null && imageData.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@picture", imageData);
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
        /// Вычисление хеша массива байтов (MD5)
        /// </summary>
        private string ComputeByteArrayHash(byte[] data)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// Сжатие изображения до указанного максимального размера в байтах
        /// </summary>
        private byte[] CompressImage(byte[] imageData, long maxSizeBytes = 2 * 1024 * 1024)
        {
            if (imageData.Length <= maxSizeBytes)
                return imageData;

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                using (Image originalImage = Image.FromStream(ms))
                {
                    long quality = 90;
                    byte[] compressedData;

                    do
                    {
                        compressedData = CompressImageWithQuality(originalImage, quality);
                        quality -= 10;
                    }
                    while (compressedData.Length > maxSizeBytes && quality > 10);

                    if (compressedData.Length > maxSizeBytes)
                    {
                        compressedData = ResizeImage(originalImage, maxSizeBytes);
                    }

                    return compressedData;
                }
            }
        }

        /// <summary>
        /// Сжатие с указанным качеством
        /// </summary>
        private byte[] CompressImageWithQuality(Image image, long quality)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                System.Drawing.Imaging.ImageCodecInfo jpegCodec =
                    System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
                    .First(c => c.MimeType == "image/jpeg");

                System.Drawing.Imaging.EncoderParameters encoderParams =
                    new System.Drawing.Imaging.EncoderParameters(1);
                encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(
                    System.Drawing.Imaging.Encoder.Quality, quality);

                image.Save(ms, jpegCodec, encoderParams);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Уменьшение размеров изображения
        /// </summary>
        private byte[] ResizeImage(Image image, long maxSizeBytes)
        {
            int width = image.Width;
            int height = image.Height;
            byte[] result;

            do
            {
                width = (int)(width * 0.8);
                height = (int)(height * 0.8);

                using (Bitmap resized = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(resized))
                    {
                        g.DrawImage(image, 0, 0, width, height);
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        resized.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        result = ms.ToArray();
                    }
                }
            }
            while (result.Length > maxSizeBytes && width > 100);

            return result;
        }

        #endregion

        /// <summary>
        /// Обработчик кнопки редактирования услуги
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int article = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["Article"].Value);
                ServiceAdd editForm = new ServiceAdd(article);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadServices();
                }
            }
        }

        /// <summary>
        /// Обработчик изменения текста в поле поиска
        /// Выполняет поиск по названию услуги (без учета регистра)
        /// Минимальная длина поискового запроса - 4 символа
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            // Если поле поиска пустое или содержит плейсхолдер - сбрасываем поиск
            if (string.IsNullOrEmpty(searchText) || searchText == "Поиск")
            {
                foundIndexes.Clear();
                currentFoundIndex = -1;
                lastSearchText = "";

                // Снимаем выделение со всех строк
                dataGridView2.ClearSelection();
                return;
            }

            // Поиск начинается только при вводе от 4 символов
            if (searchText.Length < 4)
            {
                foundIndexes.Clear();
                currentFoundIndex = -1;
                return;
            }

            // Если текст не изменился - выходим
            if (lastSearchText == searchText) return;
            lastSearchText = searchText;

            // Ищем совпадения по названию (без учета регистра)
            foundIndexes.Clear();
            for (int i = 0; i < filteredServices.Count; i++)
            {
                if (filteredServices[i].Name.ToLower().Contains(searchText.ToLower()))
                    foundIndexes.Add(i);
            }

            // Выделяем первое совпадение
            if (foundIndexes.Count > 0)
            {
                currentFoundIndex = 0;
                SelectFoundRow(foundIndexes[0]);
            }
            else
            {
                currentFoundIndex = -1;
                dataGridView2.ClearSelection();
            }
        }
    }
}