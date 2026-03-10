using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Класс, представляющий модель данных клиента
    /// Используется для хранения информации о клиентах и передачи данных между слоями приложения
    /// </summary>
    public class Client
    {
 
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }

        /// <summary>
        /// Вычисляемое свойство, возвращающее полное ФИО клиента
        /// Объединяет фамилию, имя и отчество через пробел
        /// Метод Trim() удаляет лишние пробелы в начале и конце строки
        /// </summary>
        public string FullName
        {
            get
            {
                // Объединение фамилии, имени и отчества в одну строку
                // Trim() удаляет лишние пробелы, если отчество не указано
                return $"{LastName} {FirstName} {Surname}".Trim();
            }
        }
    }
}