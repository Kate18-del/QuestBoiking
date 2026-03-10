using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Статический класс для управления общим состоянием форм
    /// Содержит вспомогательные методы для работы с элементами интерфейса
    /// </summary>
    public class FormStateManager
    {
        public static void UpdateRecordCount(Label label, int count)
        {
            // Устанавливаем текст метки в формате "Количество записей: X"
            label.Text = $"Количество записей: {count}";
        }
    }
}