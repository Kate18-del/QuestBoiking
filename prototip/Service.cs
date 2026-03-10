using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Класс, представляющий модель данных услуги (квеста)
    /// Используется для хранения информации о квестах и передачи данных между слоями приложения
    /// </summary>
    public class Service
    {
        public int Article { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Time { get; set; }
        public int? DayOfTheWeek { get; set; }
        public string Picture { get; set; } 
        public int MaxPeople { get; set; }
        public int ISLevel { get; set; }
        public string DifficultyLevel { get; set; }
        public int IDCategory { get; set; }
        public string CategoryName { get; set; }

    }
}
