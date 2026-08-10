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
        private byte[] imageData = null;

        // Режим работы формы
        private bool isEditMode = false;
        private int editingArticle = 0;

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
        /// Конструктор для редактирования существующей услуги
        /// </summary>
        public ServiceAdd(int article) : this()
        {
            isEditMode = true;
            editingArticle = article;
            LoadServiceData(article);

            // Меняем заголовок и кнопку
            this.Text = "Редактирование услуги";
            button1.Text = "Сохранить";
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
        /// Загрузка данных услуги для редактирования
        /// </summary>
        private void LoadServiceData(int article)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    Name, Description, Price, Time, DayOfTheWeek, 
                    MaxPeople, ISLevel, IDCategory, Picture
                FROM services 
                WHERE Article = @Article";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Article", article);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox2.Text = reader["Name"].ToString();
                            textBox2.ForeColor = Color.Black;

                            textBox4.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                            textBox4.ForeColor = Color.Black;

                            textBox1.Text = reader["Price"].ToString();
                            textBox1.ForeColor = Color.Black;

                            // Время
                            int totalMinutes = Convert.ToInt32(reader["Time"]);
                            int hours = totalMinutes / 60;
                            int minutes = totalMinutes % 60;
                            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);

                            // DayOfTheWeek (исправлено)
                            int dayOfWeek = reader["DayOfTheWeek"] != DBNull.Value ? Convert.ToInt32(reader["DayOfTheWeek"]) : 30;
                            if (dayOfWeek != 30)
                            {
                                DateTime nearestDate = GetNextDayOfWeek(DateTime.Now, dayOfWeek);
                                dateTimePicker2.Value = nearestDate;
                            }

                            // Категория
                            int idCategory = Convert.ToInt32(reader["IDCategory"]);
                            for (int i = 0; i < comboBox1.Items.Count; i++)
                            {
                                if (((ComboBoxItem)comboBox1.Items[i]).Value == idCategory)
                                {
                                    comboBox1.SelectedIndex = i;
                                    break;
                                }
                            }

                            // Сложность
                            int isLevel = Convert.ToInt32(reader["ISLevel"]);
                            for (int i = 0; i < comboBox2.Items.Count; i++)
                            {
                                if (((ComboBoxItem)comboBox2.Items[i]).Value == isLevel)
                                {
                                    comboBox2.SelectedIndex = i;
                                    break;
                                }
                            }

                            // Макс. человек
                            int maxPeople = Convert.ToInt32(reader["MaxPeople"]);
                            for (int i = 0; i < comboBox3.Items.Count; i++)
                            {
                                if (((ComboBoxItem)comboBox3.Items[i]).Value == maxPeople)
                                {
                                    comboBox3.SelectedIndex = i;
                                    break;
                                }
                            }

                            // Изображение
                            if (reader["Picture"] != DBNull.Value)
                            {
                                imageData = (byte[])reader["Picture"];
                                using (MemoryStream ms = new MemoryStream(imageData))
                                {
                                    pictureBox1.Image = Image.FromStream(ms);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных услуги: {ex.Message}", "Ошибка");
            }
        }
        private DateTime GetNextDayOfWeek(DateTime startDate, int targetDayOfWeek)
        {
            int daysToAdd = ((targetDayOfWeek - (int)startDate.DayOfWeek + 7) % 7);
            if (daysToAdd == 0) daysToAdd = 0; // Сегодня
            return startDate.AddDays(daysToAdd);
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

                        // Читаем файл
                        byte[] newImageData = File.ReadAllBytes(imagePath);

                        // Сжимаем если больше 3MB
                        long maxSize = 3 * 1024 * 1024;
                        if (newImageData.Length > maxSize)
                        {
                            newImageData = CompressImage(newImageData, maxSize);
                            MessageBox.Show($"Изображение было сжато до {newImageData.Length / 1024}KB", "Сжатие");
                        }

                        // Сохраняем
                        imageData = newImageData;

                        // Показываем в PictureBox
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }

                        MessageBox.Show("Изображение успешно загружено!", "Успех");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка");
                    }
                }
            }
        }


        /// <summary>
        /// Сжатие изображения до указанного максимального размера в байтах
        /// </summary>
        private byte[] CompressImage(byte[] imageData, long maxSizeBytes = 2 * 1024 * 1024)
        {
            // Если изображение уже меньше максимального размера - возвращаем как есть
            if (imageData.Length <= maxSizeBytes)
                return imageData;

            using (MemoryStream ms = new MemoryStream(imageData))
            {
                using (Image originalImage = Image.FromStream(ms))
                {
                    // Начинаем с качества 90%
                    long quality = 90;
                    byte[] compressedData;

                    do
                    {
                        compressedData = CompressImageWithQuality(originalImage, quality);
                        quality -= 10; // Уменьшаем качество на 10%
                    }
                    while (compressedData.Length > maxSizeBytes && quality > 10);

                    // Если все еще больше - уменьшаем размер изображения
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
                // Настройка кодировщика JPEG
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

        /// <summary>
        /// Обработчик кнопки добавления услуги
        /// Проверяет все поля и сохраняет услугу в БД
        /// </summary>
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text == "Наименование")
                {
                    MessageBox.Show("Введите наименование услуги", "Ошибка");
                    return;
                }

                if (!decimal.TryParse(textBox1.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Введите корректную цену", "Ошибка");
                    return;
                }

                if (!IsRussianText(textBox2.Text) && textBox2.Text != "Наименование")
                {
                    MessageBox.Show("В наименовании можно использовать только русские буквы", "Ошибка");
                    return;
                }

                if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Выберите значения из всех выпадающих списков", "Ошибка");
                    return;
                }

                int categoryId = ((ComboBoxItem)comboBox1.SelectedItem).Value;
                int difficultyId = ((ComboBoxItem)comboBox2.SelectedItem).Value;
                int maxPeople = ((ComboBoxItem)comboBox3.SelectedItem).Value;
                int timeInMinutes = dateTimePicker1.Value.Hour * 60 + dateTimePicker1.Value.Minute;
                int dayOfTheWeek = dateTimePicker2.Value.Day;
                string description = textBox4.Text == "Описание" ? "" : textBox4.Text;

                if (isEditMode)
                {
                    UpdateService(editingArticle, textBox2.Text, description, price, timeInMinutes, dayOfTheWeek, maxPeople, difficultyId, categoryId, imageData);
                    MessageBox.Show("Услуга успешно обновлена!", "Успех");
                }
                else
                {
                    int newArticle = SaveServiceToDatabase(textBox2.Text, description, price, timeInMinutes, dayOfTheWeek, maxPeople, difficultyId, categoryId, imageData);
                    MessageBox.Show($"Услуга успешно добавлена! Артикул: {newArticle}", "Успех");
                }

                ClearForm();
                this.DialogResult = DialogResult.OK; 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        /// <summary>
        /// Обновление существующей услуги
        /// </summary>
        private void UpdateService(int article, string name, string description, decimal price,
                                   int time, int dayOfTheWeek, int maxPeople,
                                   int difficultyId, int categoryId, byte[] imageData)
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();

                string query = @"UPDATE services SET 
                    Name = @name, 
                    Description = @description, 
                    Price = @price, 
                    Time = @time, 
                    DayOfTheWeek = @dayOfTheWeek, 
                    MaxPeople = @maxPeople, 
                    ISLevel = @difficultyId, 
                    IDCategory = @categoryId, 
                    Picture = @picture 
                    WHERE Article = @article";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@dayOfTheWeek", dayOfTheWeek);
                    cmd.Parameters.AddWithValue("@maxPeople", maxPeople);
                    cmd.Parameters.AddWithValue("@difficultyId", difficultyId);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@article", article);

                    if (imageData != null && imageData.Length > 0)
                        cmd.Parameters.AddWithValue("@picture", imageData);
                    else
                        cmd.Parameters.AddWithValue("@picture", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool IsRussianText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return true;
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    if (!((c >= 'А' && c <= 'я') || c == 'Ё' || c == 'ё'))
                        return false;
                }
            }
            return true;
        }

        private int SaveServiceToDatabase(string name, string description, decimal price,
                                          int time, int dayOfTheWeek, int maxPeople,
                                          int difficultyId, int categoryId, byte[] imageData)
        {
            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();

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

                    if (imageData != null && imageData.Length > 0)
                        cmd.Parameters.AddWithValue("@picture", imageData);
                    else
                        cmd.Parameters.AddWithValue("@picture", DBNull.Value);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
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
        }

        /// <summary>
        /// Обработчик кнопки возврата в меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            // Проверяем, были ли внесены изменения в форму
            bool hasChanges = false;

            // Проверяем текстовые поля
            if ((!string.IsNullOrWhiteSpace(textBox2.Text) && textBox2.Text != "Наименование" && textBox2.ForeColor == Color.Black) ||
                (!string.IsNullOrWhiteSpace(textBox4.Text) && textBox4.Text != "Описание" && textBox4.ForeColor == Color.Black) ||
                (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text != "Цена" && textBox1.ForeColor == Color.Black))
            {
                hasChanges = true;
            }

            // Проверяем, загружено ли изображение
            if (imageData != null)
            {
                hasChanges = true;
            }

            // Если есть изменения, показываем предупреждение
            if (hasChanges)
            {
                DialogResult result = MessageBox.Show(
                    "Внимание! Все несохраненные данные будут утеряны.\nВы действительно хотите выйти?",
                    "Подтверждение выхода",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return; // Отменяем выход
                }
            }

            // Если изменений нет или пользователь подтвердил выход
            this.Hide();
            ServicesManager auto = new ServicesManager();
            auto.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Вычисление хеша массива байтов (MD5)
        /// </summary>
        /// <param name="data">Массив байтов</param>
        /// <returns>Хеш в виде строки</returns>
        private string ComputeByteArrayHash(byte[] data)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
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