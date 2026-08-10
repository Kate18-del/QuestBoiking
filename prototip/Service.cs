using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Класс, представляющий модель данных услуги (квеста)
    /// Используется для хранения информации о квестах и передачи данных между слоями приложения
    /// </summary>
    public class Service : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private byte[] pictureData;
        private Image serviceImage;

        public int Article { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Time { get; set; }
        public int? DayOfTheWeek { get; set; }

        public byte[] PictureData
        {
            get => pictureData;
            set
            {
                pictureData = value;
                serviceImage = null; // Сбрасываем кешированное изображение
                OnPropertyChanged(nameof(PictureData));
                OnPropertyChanged(nameof(ServiceImage));
            }
        }

        public int MaxPeople { get; set; }
        public int ISLevel { get; set; }
        public string DifficultyLevel { get; set; }
        public int IDCategory { get; set; }
        public string CategoryName { get; set; }

        // Для обратной совместимости (если где-то используется)
        public string Picture { get; set; }

        // Свойство для DataGridView
        public Image ServiceImage
        {
            get
            {
                // Если есть данные изображения - возвращаем их
                if (PictureData != null && PictureData.Length > 0)
                {
                    if (serviceImage == null)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(PictureData))
                            {
                                serviceImage = Image.FromStream(ms);
                            }
                        }
                        catch
                        {
                            serviceImage = GetDefaultImage();
                        }
                    }
                    return serviceImage;
                }

                // Если нет изображения - возвращаем заглушку
                return GetDefaultImage();
            }
        }

        // Статическая переменная для кеширования заглушки
        private static Image defaultImage = null;

        /// <summary>
        /// Получение изображения-заглушки для услуг без фото
        /// </summary>
        private Image GetDefaultImage()
        {
            if (defaultImage == null)
            {
                try
                {
                    // Пробуем загрузить из ресурсов
                    defaultImage = Properties.Resources.zagl;
                }
                catch
                {
                    // Если ресурса нет - создаем программно
                    Bitmap bmp = new Bitmap(100, 100);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.FromArgb(240, 240, 240));
                        g.DrawRectangle(Pens.LightGray, 0, 0, 99, 99);

                        using (Font font = new Font("Arial", 9))
                        {
                            g.DrawString("НЕТ", font, Brushes.Gray, new PointF(35, 35));
                            g.DrawString("ФОТО", font, Brushes.Gray, new PointF(30, 50));
                        }
                    }
                    defaultImage = bmp;
                }
            }
            return defaultImage;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Полная информация об услуге для отображения в одной колонке
        /// </summary>
        public string FullInfo
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                // Название
                sb.AppendLine(Name);
                sb.AppendLine();

                // Артикул
                sb.AppendLine($"Артикул: {Article}");

                // Описание
                if (!string.IsNullOrEmpty(Description))
                {
                    sb.AppendLine(Description);
                }

                // Время
                sb.Append($"Время: {Time} мин");

                // Сложность
                string difficultyText;
                if (DifficultyLevel == "Легкий")
                    difficultyText = "Легкий";
                else if (DifficultyLevel == "Средний")
                    difficultyText = "Средний";
                else if (DifficultyLevel == "Сложный")
                    difficultyText = "Сложный";
                else
                    difficultyText = "Не указана";

                sb.AppendLine($"  |  Сложность: {difficultyText}");

                // Категория
                sb.AppendLine($"Категория: {CategoryName}");

                // Максимум человек
                sb.Append($"Макс. человек: {MaxPeople}");

                return sb.ToString();
            }
        }
    }
}