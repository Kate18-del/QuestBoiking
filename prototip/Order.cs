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
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        public int Article { get; set; }
        public string QuestName { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime? DueDate { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public int? UserID { get; set; }
        public string ManagerName { get; set; }
        public int? ParticipantsCount { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}