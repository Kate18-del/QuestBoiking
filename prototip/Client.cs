using System;

namespace prototip
{
    /// <summary>
    /// Модель данных клиента
    /// </summary>
    public class Client
    {
        public int ClientID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }

    }
}