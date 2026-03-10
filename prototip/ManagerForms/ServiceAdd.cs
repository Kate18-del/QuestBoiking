using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Форма для добавления новой услуги (квеста) в систему
    /// Позволяет заполнить все характеристики услуги, загрузить изображение и проверить на дубликаты
    /// </summary>
    public partial class ServiceAdd : Form
    {
        // Для хранения изображения в памяти (массив байт)
        private byte[] imageData = null;

        // Имя файла изображения
        private string imageFileName = null;

        /// <summary>
        /// Конструктор формы добавления услуги
        /// </summary>
        public ServiceAdd()
        {
            InitializeComponent();
            // Инициализация элементов формы
            InitializeForm();
            // Загрузка справочных данных из БД
            LoadDataFromDatabase();
        }

        /// <summary>
        /// Инициализация элементов формы и настройка обработчиков событий
        /// </summary>
        private void InitializeForm()
        {
            // Настройка обработчиков для валидации ввода
            textBox1.KeyPress += TextBoxNumbersOnly_KeyPress; // Цена - только цифры
            textBox3.KeyPress += TextBoxNumbersOnly_KeyPress; // Артикул - только цифры (скрыт)
            textBox2.KeyPress += TextBoxRussianOnly_KeyPress; // Наименование - только русские буквы
            textBox4.KeyPress += TextBoxRussianOnly_KeyPress; // Описание - только русские буквы

            // Скрываем поле артикула (будет автоинкрементом в БД)
            textBox3.Visible = false;
            textBox3.Text = "0";

            // Устанавливаем цвет текста для комбобоксов
            comboBox1.ForeColor = Color.Black;
            comboBox2.ForeColor = Color.Black;
            comboBox3.ForeColor = Color.Black;

            // Настройка DateTimePicker для выбора времени
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "HH:mm";
            dateTimePicker1.ShowUpDown = true; // Режим выбора времени

            // Настройка DateTimePicker для выбора даты
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dddd dd.MM.yyyy";
            dateTimePicker2.ShowUpDown = false; // Режим выбора даты
            dateTimePicker2.MinDate = DateTime.Today; // Минимальная дата - сегодня
            dateTimePicker2.MaxDate = DateTime.Today.AddDays(7); // Максимальная дата - через неделю

            // Установка плейсхолдеров для текстовых полей
            SetupPlaceholder(textBox2, "Наименование");
            SetupPlaceholder(textBox4, "Описание");
            SetupPlaceholder(textBox1, "Цена");

            // Обработчики для кнопок
            button1.Click += ButtonAdd_Click;      // Добавление услуги
            button2.Click += ButtonSelectImage_Click; // Выбор изображения

            // Установка текста меток
            label3.Text = "Время:";
            label4.Text = "Дата:";
        }

        /// <summary>
        /// Загрузка справочных данных из базы данных
        /// </summary>
        private void LoadDataFromDatabase()
        {
            try
            {
                LoadCategories();          // Загрузка категорий
                LoadDifficultyLevels();     // Загрузка уровней сложности
                LoadMaxPeopleOptions();     // Загрузка вариантов количества человек
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Загрузка категорий из базы данных в выпадающий список
        /// </summary>
        private void LoadCategories()
        {
            comboBox1.Items.Clear();
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT CategoriesID, Categorie FROM categories ORDER BY Categorie";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(new ComboBoxItem
                        {
                            Text = reader.GetString("Categorie"), // Отображаемое название
                            Value = reader.GetInt32("CategoriesID") // ID категории
                        });
                    }
                }
            }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// Загрузка уровней сложности из базы данных в выпадающий список
        /// </summary>
        private void LoadDifficultyLevels()
        {
            comboBox2.Items.Clear();
            comboBox2.DisplayMember = "Text";
            comboBox2.ValueMember = "Value";

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = "SELECT DifficultyID, Name FROM difficultylevels ORDER BY DifficultyID";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox2.Items.Add(new ComboBoxItem
                        {
                            Text = reader.GetString("Name"), // Отображаемое название
                            Value = reader.GetInt32("DifficultyID") // ID уровня сложности
                        });
                    }
                }
            }

            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;
        }

        /// <summary>
        /// Загрузка вариантов максимального количества участников
        /// </summary>
        private void LoadMaxPeopleOptions()
        {
            comboBox3.Items.Clear();
            comboBox3.DisplayMember = "Text";
            comboBox3.ValueMember = "Value";

            // Добавляем типичные варианты от 1 до 20
            for (int i = 1; i <= 20; i++)
            {
                comboBox3.Items.Add(new ComboBoxItem { Text = i.ToString(), Value = i });
            }

            comboBox3.SelectedIndex = 2; // По умолчанию 3 человека
        }

        /// <summary>
        /// Настройка плейсхолдера для текстового поля
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
        /// <param name="placeholderText">Текст плейсхолдера</param>
        private void SetupPlaceholder(TextBox textBox, string placeholderText)
        {
            textBox.Enter += (sender, e) =>
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholderText;
                    textBox.ForeColor = SystemColors.ScrollBar;
                }
                else
                {
                    textBox.ForeColor = Color.Black;
                }
            };
        }

        /// <summary>
        /// Обработчик ввода - разрешает только цифры
        /// Для полей с числами (цена, артикул)
        /// </summary>
        private void TextBoxNumbersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Разрешаем только управляющие символы (Backspace) и цифры
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        /// <summary>
        /// Обработчик ввода - разрешает только русские буквы
        /// Для полей с текстом на русском (наименование, описание)
        /// </summary>
        private void TextBoxRussianOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем управляющие символы и пробелы
            if (char.IsControl(e.KeyChar) || char.IsWhiteSpace(e.KeyChar))
            {
                return;
            }

            // Проверяем, является ли символ русской буквой
            if ((e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 'Ё' || e.KeyChar == 'ё')
            {
                return;
            }

            e.Handled = true; // Блокируем ввод других символов
        }

        /// <summary>
        /// Обработчик кнопки выбора изображения
        /// Открывает диалог выбора файла и проверяет на дубликаты
        /// </summary>
        private void ButtonSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Выберите изображение для услуги";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
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
                        if (IsImageDuplicate(imagePath))
                        {
                            // Если дубликат найден, но пользователь согласился использовать существующий файл,
                            // imageFileName уже обновлен в IsImageDuplicate
                            if (!string.IsNullOrEmpty(imageFileName))
                            {
                                // Изображение уже загружено в IsImageDuplicate
                                return;
                            }
                            else
                            {
                                // Пользователь отказался от дубликата
                                return;
                            }
                        }

                        // Если дубликат не найден, загружаем новое изображение
                        imageData = File.ReadAllBytes(imagePath);
                        imageFileName = Path.GetFileName(imagePath);

                        // Загружаем изображение в PictureBox
                        pictureBox1.Image = System.Drawing.Image.FromFile(imagePath);
                        MessageBox.Show("Новое изображение успешно загружено!", "Успех");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка");
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик кнопки добавления услуги
        /// Проверяет все поля и сохраняет услугу в БД
        /// </summary>
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // ПРОВЕРКА ОБЯЗАТЕЛЬНЫХ ПОЛЕЙ
                if (string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text == "Наименование")
                {
                    MessageBox.Show("Введите наименование услуги", "Ошибка");
                    return;
                }

                // Проверка цены
                if (!decimal.TryParse(textBox1.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Введите корректную цену (положительное число)", "Ошибка");
                    return;
                }

                // Проверка на русские буквы в наименовании и описании
                if (!IsRussianText(textBox2.Text) && textBox2.Text != "Наименование")
                {
                    MessageBox.Show("В наименовании можно использовать только русские буквы, цифры и знаки препинания", "Ошибка");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(textBox4.Text) && textBox4.Text != "Описание" && !IsRussianText(textBox4.Text))
                {
                    MessageBox.Show("В описании можно использовать только русские буквы, цифры и знаки препинания", "Ошибка");
                    return;
                }

                // Проверка выбора значений в выпадающих списках
                if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Выберите значения из всех выпадающих списков", "Ошибка");
                    return;
                }

                // Получаем ID выбранных значений
                int categoryId = ((ComboBoxItem)comboBox1.SelectedItem).Value;
                int difficultyId = ((ComboBoxItem)comboBox2.SelectedItem).Value;
                int maxPeople = ((ComboBoxItem)comboBox3.SelectedItem).Value;

                // Время в минутах
                int timeInMinutes = dateTimePicker1.Value.Hour * 60 + dateTimePicker1.Value.Minute;

                // DayOfTheWeek - день месяца (1-31) для определения дня проведения
                int dayOfTheWeek = dateTimePicker2.Value.Day;

                // Сохранение услуги в базу данных (артикул будет автоинкрементом)
                int newArticle = SaveServiceToDatabase(textBox2.Text,
                    textBox4.Text == "Описание" ? "" : textBox4.Text,
                    price, timeInMinutes, dayOfTheWeek, maxPeople, difficultyId, categoryId);

                // Если есть изображение - сохраняем его
                if (imageData != null && newArticle > 0)
                {
                    UpdateServiceImage(newArticle);
                }

                MessageBox.Show($"Услуга успешно добавлена! Артикул: {newArticle}", "Успех");
                ClearForm(); // Очистка формы после успешного добавления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении услуги: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Проверка, содержит ли текст только русские буквы и допустимые символы
        /// </summary>
        /// <param name="text">Проверяемый текст</param>
        /// <returns>true если текст допустим, false в противном случае</returns>
        private bool IsRussianText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return true;

            // Проверяем каждый символ
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    // Проверяем, русская ли это буква
                    if (!((c >= 'А' && c <= 'я') || c == 'Ё' || c == 'ё'))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Сохранение услуги в базу данных
        /// </summary>
        /// <returns>Артикул созданной услуги</returns>
        private int SaveServiceToDatabase(string name, string description, decimal price,
                                          int time, int dayOfTheWeek, int maxPeople,
                                          int difficultyId, int categoryId)
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();

                // Добавляем новую запись (артикул автоинкремент)
                string insertQuery = @"INSERT INTO services 
                         (Name, Description, Price, Time, DayOfTheWeek, MaxPeople, ISLevel, IDCategory, Picture) 
                         VALUES 
                         (@name, @description, @price, @time, @dayOfTheWeek, @maxPeople, @difficultyId, @categoryId, @picture);
                         SELECT LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@dayOfTheWeek", dayOfTheWeek);
                    cmd.Parameters.AddWithValue("@maxPeople", maxPeople);
                    cmd.Parameters.AddWithValue("@difficultyId", difficultyId);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);

                    // Если есть изображение, сохраняем имя файла, иначе null
                    if (!string.IsNullOrEmpty(imageFileName))
                    {
                        // Здесь мы не можем сгенерировать уникальное имя, так как еще нет артикула
                        // Поэтому сохраняем оригинальное имя, а потом обновим его в UpdateServiceImage
                        cmd.Parameters.AddWithValue("@picture", "temp_" + imageFileName);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@picture", DBNull.Value);
                    }

                    // Выполняем запрос и получаем новый артикул
                    int newArticle = Convert.ToInt32(cmd.ExecuteScalar());
                    return newArticle;
                }
            }
        }

        /// <summary>
        /// Обновление изображения услуги в базе данных и сохранение файла на диск
        /// </summary>
        /// <param name="article">Артикул услуги</param>
        private void UpdateServiceImage(int article)
        {
            try
            {
                if (imageData == null || string.IsNullOrEmpty(imageFileName))
                    return;

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Формируем уникальное имя файла
                    string extension = Path.GetExtension(imageFileName);
                    string uniqueFileName = $"service_{article}_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";

                    // Сохраняем изображение в AppData
                    SaveImageToDisk(uniqueFileName, imageData);

                    // Обновляем запись в БД
                    string query = "UPDATE services SET Picture = @picture WHERE Article = @article";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@picture", uniqueFileName);
                        cmd.Parameters.AddWithValue("@article", article);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Обновляем pictureBox на форме, чтобы показать сохраненное изображение
                            string savedPath = GetImagePath(uniqueFileName);
                            if (File.Exists(savedPath))
                            {
                                pictureBox1.Image = System.Drawing.Image.FromFile(savedPath);
                            }
                            MessageBox.Show("Изображение успешно сохранено!", "Успех");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}", "Ошибка");
            }
        }


        /// <summary>
        /// Сохраняет изображение на диск в папку AppData
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
        /// Очистка формы после успешного добавления
        /// </summary>
        private void ClearForm()
        {
            // Очистка всех полей
            textBox2.Text = "Наименование";
            textBox2.ForeColor = SystemColors.ScrollBar;

            textBox4.Text = "Описание";
            textBox4.ForeColor = SystemColors.ScrollBar;

            textBox1.Text = "Цена";
            textBox1.ForeColor = SystemColors.ScrollBar;

            textBox3.Text = "0";

            // Устанавливаем текущее время и дату
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

            // Сбрасываем выпадающие списки на значения по умолчанию
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
            if (comboBox3.Items.Count > 0) comboBox3.SelectedIndex = 2;

            // Сбрасываем изображение на стандартное
            pictureBox1.Image = global::prototip.Properties.Resources.zagl1;
            imageData = null;
            imageFileName = null;
        }

        /// <summary>
        /// Обработчик кнопки возврата в меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ServicesManager auto = new ServicesManager();
            auto.ShowDialog();
            this.Visible = true;
        }

        /// <summary>
        /// Проверка, является ли выбранное изображение дубликатом существующего
        /// </summary>
        /// <param name="imagePath">Путь к выбранному изображению</param>
        /// <returns>true если дубликат найден</returns>
        private bool IsImageDuplicate(string imagePath)
        {
            try
            {
                // Вычисляем хеш нового файла (MD5)
                string newFileHash = ComputeFileHash(imagePath);

                // Получаем размер нового файла
                FileInfo newFileInfo = new FileInfo(imagePath);
                long newFileSize = newFileInfo.Length;

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Получаем все существующие изображения
                    string query = "SELECT Article, Picture FROM services WHERE Picture IS NOT NULL";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string existingFileName = reader["Picture"].ToString();
                            int existingArticle = Convert.ToInt32(reader["Article"]);

                            if (string.IsNullOrEmpty(existingFileName))
                                continue;

                            // Получаем путь к существующему файлу
                            string existingFilePath = GetImagePath(existingFileName);

                            if (File.Exists(existingFilePath))
                            {
                                // Сравниваем размеры для быстрой проверки
                                FileInfo existingFileInfo = new FileInfo(existingFilePath);

                                if (existingFileInfo.Length == newFileSize)
                                {
                                    // Если размеры совпадают, сравниваем хеши
                                    string existingFileHash = ComputeFileHash(existingFilePath);

                                    if (existingFileHash == newFileHash)
                                    {
                                        // Нашли дубликат - просто показываем сообщение
                                        MessageBox.Show(
                                            $"Это изображение уже используется для услуги с артикулом {existingArticle}.\n" +
                                            $"Пожалуйста, выберите другое изображение.",
                                            "Обнаружен дубликат",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        return true; // Дубликат найден
                                    }
                                }
                            }
                        }
                    }
                }

                return false; // Дубликат не найден
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
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Хеш файла в виде строки</returns>
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

        /// <summary>
        /// Получение пути к изображению по имени файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Полный путь к файлу или null</returns>
        private string GetImagePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            // 1. Сначала проверяем в AppData
            string appDataPath = Path.Combine(GetImagesFolderPath(), fileName);
            if (File.Exists(appDataPath))
                return appDataPath;

            // 2. Затем проверяем в папке приложения (для обратной совместимости)
            string appPath = Path.Combine(Application.StartupPath, "Images", fileName);
            if (File.Exists(appPath))
                return appPath;

            // 3. Проверяем в текущей директории
            string currentPath = Path.Combine(Environment.CurrentDirectory, "Images", fileName);
            if (File.Exists(currentPath))
                return currentPath;

            return null;
        }
    }

    /// <summary>
    /// Вспомогательный класс для ComboBox с поддержкой Text и Value
    /// </summary>
    public class ComboBoxItem
    {
        public string Text { get; set; } // Отображаемый текст
        public int Value { get; set; }   // Скрытое значение (ID)

        public override string ToString()
        {
            return Text;
        }
    }
}