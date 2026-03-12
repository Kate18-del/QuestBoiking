using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Репозиторий для работы с данными заказов в базе данных
    /// Реализует паттерн Repository для изоляции логики доступа к данным заказов
    /// </summary>
    public class OrderRepository
    {
        /// <summary>
        /// Получает список всех заказов из базы данных с присоединением связанных данных
        /// </summary>
        /// <returns>Список объектов Order с полной информацией о заказах</returns>
        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            try
            {
                // СЛОЖНЫЙ SQL-ЗАПРОС с LEFT JOIN для получения данных из связанных таблиц
                string query = @"
                    SELECT 
                        o.ID,
                        o.ClientID,
                        CONCAT(c.LastName, ' ', c.FirstName, ' ', c.Surname) as ClientName, -- Полное ФИО клиента
                        c.PhoneNumber,
                        o.Article,
                        s.Name as QuestName, -- Название квеста
                        o.DateOfAdmission,
                        o.DueDate,
                        o.StatusID,
                        st.Name as StatusName, -- Название статуса
                        o.UserID,
                        u.FIO as ManagerName, -- ФИО менеджера
                        o.ParticipantsCount,
                        o.TotalPrice
                    FROM orders o
                    LEFT JOIN clients c ON o.ClientID = c.ClientID
                    LEFT JOIN services s ON o.Article = s.Article
                    LEFT JOIN statuses st ON o.StatusID = st.StatusID
                    LEFT JOIN users u ON o.UserID = u.UserID
                    ORDER BY o.DateOfAdmission DESC"; 

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
                            // Создание объекта Order и заполнение данными из БД
                            var order = new Order
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                ClientName = reader["ClientName"]?.ToString(),
                                PhoneNumber = reader["PhoneNumber"]?.ToString(),
                                Article = Convert.ToInt32(reader["Article"]),
                                QuestName = reader["QuestName"]?.ToString(),
                                DateOfAdmission = Convert.ToDateTime(reader["DateOfAdmission"]),

                                // Обработка NULL значения для DueDate
                                DueDate = reader["DueDate"] != DBNull.Value ? Convert.ToDateTime(reader["DueDate"]) : (DateTime?)null,

                                StatusID = Convert.ToInt32(reader["StatusID"]),
                                StatusName = reader["StatusName"]?.ToString(),

                                // Обработка NULL значения для UserID
                                UserID = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : (int?)null,

                                ManagerName = reader["ManagerName"]?.ToString(),

                                // Обработка NULL значения для ParticipantsCount
                                ParticipantsCount = reader["ParticipantsCount"] != DBNull.Value ? Convert.ToInt32(reader["ParticipantsCount"]) : (int?)null,

                                // Обработка NULL значения для TotalPrice
                                TotalPrice = reader["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(reader["TotalPrice"]) : (decimal?)null
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Пробрасываем исключение выше с дополнительной информацией
                throw new Exception($"Ошибка загрузки заказов: {ex.Message}");
            }

            return orders;
        }

        /// <summary>
        /// Получает список всех статусов заказов из базы данных
        /// </summary>
        /// <returns>Список строк с названиями статусов</returns>
        public List<string> GetAllStatuses()
        {
            var statuses = new List<string>();

            try
            {
                // Простой запрос для получения всех статусов с сортировкой по имени
                string query = "SELECT Name FROM statuses ORDER BY Name";

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statuses.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки статусов: {ex.Message}");
            }

            return statuses;
        }

        /// <summary>
        /// Получает минимальную дату заказа из базы данных
        /// </summary>
        public DateTime GetMinOrderDate()
        {
            try
            {
                using (var connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT MIN(DateOfAdmission) FROM orders";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            return Convert.ToDateTime(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting min date: {ex.Message}");
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Получает максимальную дату заказа из базы данных
        /// </summary>
        public DateTime GetMaxOrderDate()
        {
            try
            {
                using (var connection = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT MAX(DateOfAdmission) FROM orders";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            return Convert.ToDateTime(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting max date: {ex.Message}");
            }
            return DateTime.MinValue;
        }
    }
}