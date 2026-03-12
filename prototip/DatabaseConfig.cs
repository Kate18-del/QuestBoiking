using System;
using System.Collections.Generic;
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
        public static string ConnectionString => "server=localhost;database=questbooking;uid=root;pwd=root;";
    }
}