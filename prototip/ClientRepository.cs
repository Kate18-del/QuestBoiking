using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Репозиторий для работы с данными клиентов в базе данных
    /// Реализует паттерн Repository для изоляции логики доступа к данным
    /// </summary>
    public class ClientRepository
    {
        /// <summary>
        /// Получает список всех клиентов из базы данных
        /// </summary>
        /// <returns>Список объектов Client</returns>
        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();

            try
            {
                // SQL-запрос для получения всех клиентов с сортировкой по фамилии и имени
                string query = @"
                    SELECT 
                        ClientID,
                        LastName,
                        FirstName,
                        Surname,
                        PhoneNumber,
                        Age
                    FROM clients 
                    ORDER BY LastName, FirstName";

                // Создание подключения к БД с использованием строки подключения из конфигурации
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Выполнение запроса и чтение результатов
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта Client и заполнение данными из БД
                            var client = new Client
                            {
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                LastName = reader["LastName"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                // Обработка NULL значения для отчества
                                Surname = reader["Surname"] == DBNull.Value ? null : reader["Surname"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                // Обработка NULL значения для возраста (используем nullable int)
                                Age = reader["Age"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Age"])
                            };
                            clients.Add(client);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Пробрасываем исключение выше с дополнительной информацией
                throw new Exception($"Ошибка загрузки клиентов: {ex.Message}");
            }

            return clients;
        }

        /// <summary>
        /// Добавляет нового клиента в базу данных
        /// </summary>
        /// <param name="client">Объект Client с данными нового клиента</param>
        /// <returns>ID созданного клиента</returns>
        public int AddClient(Client client)
        {
            try
            {
                // Очищаем номер телефона от лишних символов
                string cleanPhone = CleanPhone(client.PhoneNumber);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // ПРОВЕРКА УНИКАЛЬНОСТИ НОМЕРА ТЕЛЕФОНА
                    string checkQuery = "SELECT COUNT(*) FROM clients WHERE PhoneNumber = @phoneNumber";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@phoneNumber", cleanPhone);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        throw new Exception("Клиент с таким номером телефона уже существует");
                    }

                    // Если номер уникальный - добавляем клиента
                    string insertQuery = @"INSERT INTO clients (LastName, FirstName, Surname, PhoneNumber, Age) 
                                         VALUES (@lastName, @firstName, @surname, @phoneNumber, @age);
                                         SELECT LAST_INSERT_ID();"; // Возвращает ID созданной записи

                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@lastName", client.LastName);
                    insertCmd.Parameters.AddWithValue("@firstName", client.FirstName);
                    // Если отчество не указано, передаем DBNull.Value
                    insertCmd.Parameters.AddWithValue("@surname", client.Surname ?? (object)DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@phoneNumber", cleanPhone);
                    // Если возраст не указан, передаем DBNull.Value
                    insertCmd.Parameters.AddWithValue("@age", client.Age ?? (object)DBNull.Value);

                    // Возвращаем ID созданной записи
                    return Convert.ToInt32(insertCmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления клиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет данные существующего клиента
        /// </summary>
        /// <param name="client">Объект Client с обновленными данными</param>
        /// <returns>true если обновление успешно</returns>
        public bool UpdateClient(Client client)
        {
            try
            {
                // Очищаем номер телефона
                string cleanPhone = CleanPhone(client.PhoneNumber);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Получаем текущий номер телефона клиента для сравнения
                    string getCurrentPhoneQuery = "SELECT PhoneNumber FROM clients WHERE ClientID = @clientId";
                    MySqlCommand getCurrentPhoneCmd = new MySqlCommand(getCurrentPhoneQuery, conn);
                    getCurrentPhoneCmd.Parameters.AddWithValue("@clientId", client.ClientID);

                    string currentPhone = getCurrentPhoneCmd.ExecuteScalar()?.ToString();

                    // Если номер изменился - проверяем уникальность нового номера
                    if (cleanPhone != currentPhone)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM clients WHERE PhoneNumber = @phoneNumber AND ClientID != @clientId";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@phoneNumber", cleanPhone);
                        checkCmd.Parameters.AddWithValue("@clientId", client.ClientID);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            throw new Exception("Клиент с таким номером телефона уже существует");
                        }
                    }

                    // Обновление данных клиента
                    string updateQuery = @"UPDATE clients 
                                         SET LastName = @lastName, 
                                             FirstName = @firstName, 
                                             Surname = @surname, 
                                             PhoneNumber = @phoneNumber, 
                                             Age = @age 
                                         WHERE ClientID = @clientId";

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@clientId", client.ClientID);
                    updateCmd.Parameters.AddWithValue("@lastName", client.LastName);
                    updateCmd.Parameters.AddWithValue("@firstName", client.FirstName);
                    updateCmd.Parameters.AddWithValue("@surname", client.Surname ?? (object)DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@phoneNumber", cleanPhone);
                    updateCmd.Parameters.AddWithValue("@age", client.Age ?? (object)DBNull.Value);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Клиент не найден или данные не изменились");
                    }

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                // Специальная обработка ошибки дубликата (код 1062)
                if (ex.Number == 1062)
                {
                    throw new Exception("Клиент с таким номером телефона уже существует");
                }
                throw new Exception($"Ошибка обновления клиента: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления клиента: {ex.Message}");
            }
        }

        /// <summary>
        /// "Мягкое" удаление клиента (не физическое, а логическое)
        /// Сохраняет ID клиента в файл для скрытия при отображении
        /// </summary>
        /// <param name="clientId">ID клиента</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="surname">Отчество</param>
        /// <param name="phoneNumber">Телефон</param>
        /// <returns>true если удаление успешно</returns>
        public bool DeleteClient(int clientId, string lastName = null, string firstName = null,
                          string surname = null, string phoneNumber = null)
        {
            try
            {
                // Сохраняем ID, ФИО и телефон в локальном файле для последующего скрытия
                DeletedRecordsManager.AddDeletedClient(clientId, lastName, firstName, surname, phoneNumber);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления клиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Приватный метод для проверки существования номера телефона
        /// Используется внутри класса для валидации
        /// </summary>
        /// <param name="phoneNumber">Номер телефона</param>
        /// <param name="excludeClientId">ID клиента для исключения из проверки (при редактировании)</param>
        /// <returns>true если номер уже существует</returns>
        private bool IsPhoneNumberExists(string phoneNumber, int excludeClientId = 0)
        {
            try
            {
                string cleanPhone = CleanPhone(phoneNumber);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM clients WHERE PhoneNumber = @phoneNumber AND ClientID != @excludeClientId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@phoneNumber", cleanPhone);
                    cmd.Parameters.AddWithValue("@excludeClientId", excludeClientId);

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Приватный метод для очистки номера телефона от лишних символов
        /// Приводит номер к единому формату для хранения в БД
        /// </summary>
        /// <param name="phone">Исходный номер телефона (может содержать скобки, дефисы, пробелы)</param>
        /// <returns>Очищенный номер в формате +7XXXXXXXXXX</returns>
        private string CleanPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return phone;

            // Убираем все нецифровые символы
            string digits = new string(phone.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(digits))
                return phone;

            // ПРИВЕДЕНИЕ К ЕДИНОМУ ФОРМАТУ
            // Если номер начинается с 8 и имеет 11 цифр (российский формат), заменяем 8 на +7
            if (digits.StartsWith("8") && digits.Length == 11)
                return "+7" + digits.Substring(1);
            // Если 11 цифр и начинается с 7, просто добавляем +
            else if (digits.StartsWith("7") && digits.Length == 11)
                return "+" + digits;
            // Если 10 цифр (без кода страны), добавляем +7
            else if (digits.Length == 10)
                return "+7" + digits;
            // В остальных случаях просто добавляем +
            else
                return "+" + digits;
        }
    }
}