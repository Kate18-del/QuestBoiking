using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Класс, представляющий модель данных пользователя системы
    /// Используется для хранения информации о пользователях и передачи данных между слоями приложения
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FIO { get; set; }
        public int IDRole { get; set; }
        public string RoleName { get; set; }
    }
}
