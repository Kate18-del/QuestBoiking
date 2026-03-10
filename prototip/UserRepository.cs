using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Репозиторий для работы с данными пользователей в базе данных
    /// Реализует паттерн Repository для изоляции логики доступа к данным пользователей
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Получает список всех пользователей из базы данных
        /// </summary>
        /// <returns>Список объектов User с полной информацией о пользователях</returns>
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            try
            {
                // SQL-запрос с CASE для преобразования ID роли в текстовое название
                string query = @"
                    SELECT 
                        u.UserID,
                        u.Login,
                        u.Password,
                        u.FIO,
                        u.IDRole,
                        CASE 
                            WHEN u.IDRole = 1 THEN 'Администратор'
                            WHEN u.IDRole = 2 THEN 'Директор' 
                            WHEN u.IDRole = 3 THEN 'Менеджер'
                            ELSE 'Неизвестно'
                        END as RoleName
                    FROM users u
                    ORDER BY u.UserID"; // Сортировка по ID для стабильного порядка

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта User и заполнение данными из БД
                            var user = new User
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Login = reader["Login"].ToString(),
                                Password = reader["Password"].ToString(), // Хэшированный пароль
                                FIO = reader["FIO"].ToString(),
                                IDRole = Convert.ToInt32(reader["IDRole"]),
                                RoleName = reader["RoleName"].ToString() // Текстовое название роли
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Пробрасываем исключение выше с дополнительной информацией
                throw new Exception($"Ошибка загрузки пользователей: {ex.Message}");
            }

            return users;
        }

        /// <summary>
        /// Добавляет нового пользователя в базу данных
        /// </summary>
        /// <param name="user">Объект User с данными нового пользователя</param>
        /// <returns>true если добавление успешно</returns>
        public bool AddUser(User user)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO users (Login, Password, FIO, IDRole) 
                                   VALUES (@login, @password, @fio, @roleId)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    // Использование параметров для защиты от SQL-инъекций
                    cmd.Parameters.AddWithValue("@login", user.Login);
                    cmd.Parameters.AddWithValue("@password", user.Password); // Уже хэшированный пароль
                    cmd.Parameters.AddWithValue("@fio", user.FIO);
                    cmd.Parameters.AddWithValue("@roleId", user.IDRole);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления пользователя: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет данные существующего пользователя
        /// </summary>
        /// <param name="user">Объект User с обновленными данными</param>
        /// <returns>true если обновление успешно</returns>
        public bool UpdateUser(User user)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"UPDATE users 
                                   SET Login = @login, Password = @password, FIO = @fio, IDRole = @roleId 
                                   WHERE UserID = @userId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@login", user.Login);
                    cmd.Parameters.AddWithValue("@password", user.Password); // Может быть новым хэшем
                    cmd.Parameters.AddWithValue("@fio", user.FIO);
                    cmd.Parameters.AddWithValue("@roleId", user.IDRole);
                    cmd.Parameters.AddWithValue("@userId", user.UserID);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления пользователя: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет пользователя из базы данных
        /// </summary>
        /// <param name="userId">ID пользователя для удаления</param>
        /// <returns>true если удаление успешно</returns>
        public bool DeleteUser(int userId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM users WHERE UserID = @userId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления пользователя: {ex.Message}");
            }
        }

        /// <summary>
        /// Проверяет, существует ли логин в базе данных
        /// Используется при добавлении и редактировании для обеспечения уникальности логинов
        /// </summary>
        /// <param name="login">Проверяемый логин</param>
        /// <param name="excludeUserId">ID пользователя, которого нужно исключить из проверки (при редактировании)</param>
        /// <returns>true если логин уже существует</returns>
        public bool IsLoginExists(string login, int excludeUserId = 0)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    // Проверяем существование логина, исключая текущего пользователя при редактировании
                    string query = "SELECT COUNT(*) FROM users WHERE Login = @login AND UserID != @excludeUserId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@excludeUserId", excludeUserId);

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch
            {
                return false; // В случае ошибки считаем, что логин не существует
            }
        }
    }
}