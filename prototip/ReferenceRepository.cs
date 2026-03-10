using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Репозиторий для работы со справочными данными в базе данных
    /// Предоставляет унифицированные методы для работы с различными справочниками
    /// </summary>
    public class ReferenceRepository
    {
        /// <summary>
        /// Получает все записи из указанной таблицы справочника
        /// </summary>
        /// <param name="tableName">Название таблицы (statuses, categories, difficultylevels)</param>
        /// <returns>Список объектов ReferenceItem с данными из таблицы</returns>
        public List<ReferenceItem> GetAllItems(string tableName)
        {
            var items = new List<ReferenceItem>();

            try
            {
                // Определяем имена столбцов в зависимости от таблицы
                string idColumn = GetIdColumnName(tableName);
                string nameColumn = GetNameColumnName(tableName);

                // Динамическое формирование SQL-запроса с учетом структуры таблицы
                string query = $"SELECT {idColumn} as ID, {nameColumn} as Name FROM {tableName} ORDER BY {idColumn}";

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ReferenceItem
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                TableName = tableName // Сохраняем тип справочника
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки данных: {ex.Message}");
            }

            return items;
        }

        /// <summary>
        /// Добавляет новую запись в указанную таблицу справочника
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="name">Наименование добавляемой записи</param>
        /// <returns>true если добавление успешно</returns>
        public bool AddItem(string tableName, string name)
        {
            try
            {
                // Получаем название столбца с именем для данной таблицы
                string nameColumn = GetNameColumnName(tableName);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    // INSERT-запрос с динамическим именем таблицы и столбца
                    string query = $"INSERT INTO {tableName} ({nameColumn}) VALUES (@name)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", name);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет существующую запись в справочнике
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="id">ID записи для обновления</param>
        /// <param name="name">Новое наименование</param>
        /// <returns>true если обновление успешно</returns>
        public bool UpdateItem(string tableName, int id, string name)
        {
            try
            {
                // Получаем названия столбцов для данной таблицы
                string idColumn = GetIdColumnName(tableName);
                string nameColumn = GetNameColumnName(tableName);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    // UPDATE-запрос с динамическими именами столбцов
                    string query = $"UPDATE {tableName} SET {nameColumn} = @name WHERE {idColumn} = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет запись из справочника
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="id">ID записи для удаления</param>
        /// <returns>true если удаление успешно</returns>
        public bool DeleteItem(string tableName, int id)
        {
            try
            {
                // Получаем название столбца с ID для данной таблицы
                string idColumn = GetIdColumnName(tableName);

                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    // DELETE-запрос с динамическим именем столбца ID
                    string query = $"DELETE FROM {tableName} WHERE {idColumn} = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления: {ex.Message}");
            }
        }

        /// <summary>
        /// Вспомогательный метод для определения названия столбца с ID
        /// Учитывает разную структуру таблиц в базе данных
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <returns>Название столбца с идентификатором</returns>
        private string GetIdColumnName(string tableName)
        {
            switch (tableName)
            {
                case "categories": return "CategoriesID";      // В таблице categories поле называется CategoriesID
                case "statuses": return "StatusID";            // В таблице statuses поле называется StatusID
                case "difficultylevels": return "DifficultyID"; // В таблице difficultylevels поле называется DifficultyID
                default: return "ID";                           // По умолчанию предполагаем ID
            }
        }

        /// <summary>
        /// Вспомогательный метод для определения названия столбца с наименованием
        /// Учитывает разную структуру таблиц в базе данных
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <returns>Название столбца с наименованием</returns>
        private string GetNameColumnName(string tableName)
        {
            switch (tableName)
            {
                case "categories": return "Categorie";         // В таблице categories поле называется Categorie
                case "statuses": return "Name";                 // В таблице statuses поле называется Name
                case "difficultylevels": return "Name";         // В таблице difficultylevels поле называется Name
                default: return "Name";                          // По умолчанию предполагаем Name
            }
        }
    }
}