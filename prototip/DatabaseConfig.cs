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
        public static string ConnectionString => "server=10.207.106.12;database=db99;uid=user99;pwd=rx63;";
    }
}