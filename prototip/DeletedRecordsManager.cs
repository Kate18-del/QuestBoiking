using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Класс для управления записями об удаленных клиентах
    /// Реализует механизм "мягкого удаления" - физически записи не удаляются из БД,
    /// а их ID сохраняются в файл для фильтрации при отображении
    /// </summary>
    public class DeletedRecordsManager
    {
        /// <summary>
        /// Путь к файлу в папке Resources проекта, где хранятся ID удаленных клиентов
        /// </summary>
        private static readonly string FilePath = GetResourceFilePath("deleted_clients.txt");

        /// <summary>
        /// Метод для получения полного пути к файлу в папке Resources
        /// Создает папку Resources, если она не существует
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Полный путь к файлу</returns>
        private static string GetResourceFilePath(string fileName)
        {
            // Получаем путь к папке проекта (поднимаемся на два уровня выше от папки Debug)
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            // Формируем путь к папке Resources
            string resourcesPath = Path.Combine(projectDirectory, "Resources");

            // Создаем папку, если её нет
            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }

            return Path.Combine(resourcesPath, fileName);
        }

        /// <summary>
        /// Добавляет информацию об удаленном клиенте в файл
        /// Сохраняется ID, ФИО и телефон для возможности восстановления или анализа
        /// </summary>
        /// <param name="clientId">ID клиента</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="surname">Отчество</param>
        /// <param name="phoneNumber">Телефон</param>
        public static void AddDeletedClient(int clientId, string lastName, string firstName, string surname, string phoneNumber)
        {
            try
            {
                // Убеждаемся, что папка существует
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

                // Форматируем ФИО в единую строку
                string fullName = GetFullName(lastName, firstName, surname);

                // Форматируем запись с разделителем '|' (pipe - чтобы избежать проблем с запятыми)
                string record = $"{clientId}|{fullName}|{phoneNumber}";

                // Добавляем запись в конец файла
                using (StreamWriter writer = File.AppendText(FilePath))
                {
                    writer.WriteLine(record);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения удаленного ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Форматирует ФИО в единую строку
        /// Если отчество не указано, возвращает фамилию и имя
        /// </summary>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="surname">Отчество (может быть null)</param>
        /// <returns>Отформатированное ФИО</returns>
        private static string GetFullName(string lastName, string firstName, string surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
                return $"{lastName} {firstName}";
            else
                return $"{lastName} {firstName} {surname}";
        }

        /// <summary>
        /// Получает множество ID удаленных клиентов из файла
        /// Используется для фильтрации при загрузке списка клиентов
        /// </summary>
        /// <returns>HashSet с ID удаленных клиентов</returns>
        public static HashSet<int> GetDeletedClientIds()
        {
            HashSet<int> deletedIds = new HashSet<int>();

            // Если файл не существует, возвращаем пустое множество
            if (!File.Exists(FilePath))
                return deletedIds;

            try
            {
                // Читаем все строки из файла
                var lines = File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    // Разбиваем строку по разделителю '|'
                    var parts = line.Split('|');
                    if (parts.Length >= 1 && int.TryParse(parts[0].Trim(), out int id))
                    {
                        deletedIds.Add(id);
                    }
                }
            }
            catch { /* Игнорируем ошибки чтения */ }

            return deletedIds;
        }
    }

    /// <summary>
    /// Класс для хранения полной информации об удаленном клиенте
    /// Используется при необходимости восстановления данных
    /// </summary>
    public class DeletedClientInfo
    {
        public int ClientID { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}