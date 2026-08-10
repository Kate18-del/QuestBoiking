using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Статический класс для хранения конфигурации подключения к базе данных
    /// Централизует строку подключения, чтобы её можно было легко изменить при необходимости
    /// </summary>
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; private set; }

        static DatabaseConfig()
        {
            // При первом обращении загружаем настройки из конфига
            LoadFromConfig();
        }

        private static void LoadFromConfig()
        {
            try
            {
                string server = ConfigurationManager.AppSettings["Server"] ?? "localhost";
                string database = ConfigurationManager.AppSettings["Database"] ?? "questbooking";
                string username = ConfigurationManager.AppSettings["Username"] ?? "root";
                string password = ConfigurationManager.AppSettings["Password"] ?? "";

                UpdateConnectionString(server, database, username, password);
            }
            catch
            {
                // Значения по умолчанию
                UpdateConnectionString("localhost", "questbooking", "root", "");
            }
        }

        public static void UpdateConnectionString(string server, string database, string username, string password)
        {
            ConnectionString = $"server={server};database={database};uid={username};pwd={password};charset=utf8mb4;";
        }

        // Метод для принудительной перезагрузки
        public static void ReloadFromConfig()
        {
            LoadFromConfig();
        }
    }
}