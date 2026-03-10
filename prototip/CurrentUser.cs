using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Статический класс для хранения информации о текущем авторизованном пользователе
    /// Используется для передачи данных о пользователе между формами без повторной авторизации
    /// Реализует паттерн "Одиночка" (Singleton) через статические члены
    /// </summary>
    public static class CurrentUser
    {
        public static int UserID { get; set; }
        public static string Login { get; set; }
        public static string FIO { get; set; }
        public static int Role { get; set; }
        public static string WelcomeMessage => $"Добро пожаловать, {FIO}!";

        /// <summary>
        /// Вычисляемое свойство для получения названия роли по её идентификатору
        /// Используется для отображения в интерфейсе вместо числового кода
        /// </summary>
        public static string RoleName
        {
            get
            {
                switch (Role)
                {
                    case 1: return "Администратор";
                    case 2: return "Директор";
                    case 3: return "Менеджер";
                    default: return "Неизвестно"; // На случай, если роль не определена
                }
            }
        }
    }
}