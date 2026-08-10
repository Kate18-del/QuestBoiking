using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Класс, представляющий модель данных заказа
    /// Используется для хранения информации о заказах и передачи данных между слоями приложения
    /// </summary>
    public class Order
    {
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ServiceName { get; set; }
        public DateTime StartTime { get; set; }
        public string StatusName { get; set; }
        public int ParticipantsCount { get; set; }
        public decimal TotalPrice { get; set; }

        // Эти свойства ОБЯЗАТЕЛЬНО должны быть для DataGridView
        public string DisplayClientName => ClientName;
        public string DisplayPhone => ClientPhone;
    }
}