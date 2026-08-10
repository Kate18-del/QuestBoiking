using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Класс, представляющий элемент справочника
    /// Используется для унифицированной работы со справочными данными
    /// </summary>
    public class ReferenceItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; } 
    }
}
