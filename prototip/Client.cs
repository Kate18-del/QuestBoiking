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

        // Дополнительные персональные данные для защиты
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PassportData { get; set; }

        // Флаг для маскировки данных (true - данные скрыты, false - показаны)
        public bool IsMasked { get; set; } = true;

        // Свойства для отображения в таблице с маскировкой
        public string DisplayLastName => IsMasked ? MaskString(LastName) : LastName;
        public string DisplayFirstName => IsMasked ? MaskString(FirstName) : FirstName;
        public string DisplaySurname => IsMasked ? MaskString(Surname) : Surname;
        public string DisplayPhoneNumber => IsMasked ? MaskPhoneNumber(PhoneNumber) : PhoneNumber;
        public string DisplayAge => IsMasked ? "**" : Age?.ToString() ?? "";
        public string DisplayBirthDate => IsMasked ? "**.**.****" : BirthDate?.ToString("dd.MM.yyyy") ?? "";
        public string DisplayAddress => IsMasked ? MaskString(Address, 10) : Address ?? "";

        /// <summary>
        /// Маскирование строки (оставляет первый символ, остальные заменяет на *)
        /// </summary>
        private string MaskString(string input, int maxLength = 5)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= 1) return "*";

            int visibleChars = Math.Min(1, input.Length);
            string visible = input.Substring(0, visibleChars);
            string masked = new string('*', Math.Min(input.Length - visibleChars, maxLength));

            return visible + masked;
        }

        /// <summary>
        /// Маскирование номера телефона (оставляет первые 4 и последние 2 цифры)
        /// </summary>
        private string MaskPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return "";

            // Удаляем все нецифровые символы
            string digits = new string(phone.Where(char.IsDigit).ToArray());

            if (digits.Length <= 6) return new string('*', digits.Length);

            string visibleStart = digits.Substring(0, 4);
            string visibleEnd = digits.Substring(digits.Length - 2, 2);
            string masked = new string('*', digits.Length - 6);

            // Восстанавливаем формат с маской
            return visibleStart + masked + visibleEnd;
        }
    }
}