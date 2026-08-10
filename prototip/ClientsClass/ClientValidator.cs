using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Статический класс для валидации и форматирования данных клиента
    /// Содержит методы проверки корректности ввода и автоматического форматирования
    /// </summary>
    public static class ClientValidator
    {
        /// <summary>
        /// Комплексная проверка всех полей формы клиента
        /// </summary>
        /// <param name="lastName">Фамилия</param>
        /// <param name="firstName">Имя</param>
        /// <param name="phoneNumber">Номер телефона (с маской)</param>
        /// <param name="ageText">Возраст в виде строки</param>
        /// <param name="age">Выходной параметр - возраст в числовом формате (nullable)</param>
        /// <param name="errorMessage">Выходной параметр - сообщение об ошибке</param>
        /// <returns>true если все поля корректны</returns>
        public static bool ValidateInput(string lastName, string firstName, string phoneNumber,
                                         string ageText, out int? age, out string errorMessage)
        {
            age = null;
            errorMessage = "";

            // ПРОВЕРКА ФАМИЛИИ
            if (string.IsNullOrWhiteSpace(lastName) || lastName == "Фамилия")
            {
                errorMessage = "Введите фамилию клиента";
                return false;
            }

            // ПРОВЕРКА ИМЕНИ
            if (string.IsNullOrWhiteSpace(firstName) || firstName == "Имя")
            {
                errorMessage = "Введите имя клиента";
                return false;
            }

            // ПРОВЕРКА ТЕЛЕФОНА - проверяем, заполнена ли маска полностью
            if (!IsPhoneMaskComplete(phoneNumber))
            {
                errorMessage = "Введите номер телефона полностью";
                return false;
            }

            // ПРОВЕРКА ВОЗРАСТА (если введен)
            if (!string.IsNullOrWhiteSpace(ageText) && ageText != "Возраст")
            {
                if (int.TryParse(ageText, out int parsedAge))
                {
                    // Проверяем только диапазон (от 18 до 80 лет)
                    if (parsedAge < 18 || parsedAge > 80)
                    {
                        errorMessage = "Возраст должен быть от 18 до 80 лет";
                        return false;
                    }

                    age = parsedAge;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверяет, полностью ли заполнена маска телефона
        /// </summary>
        /// <param name="phoneWithMask">Номер телефона с маской</param>
        /// <returns>true если маска заполнена полностью</returns>
        public static bool IsPhoneMaskComplete(string phoneWithMask)
        {
            if (string.IsNullOrWhiteSpace(phoneWithMask))
                return false;

            // Если в строке есть подчеркивание '_' (символ заполнения маски) - значит маска не заполнена
            return !phoneWithMask.Contains('_');
        }

        /// <summary>
        /// Валидация ввода для полей с русским текстом
        /// Разрешает русские буквы, пробелы и управляющие символы
        /// </summary>
        /// <param name="e">Событие KeyPress</param>
        /// <returns>true если символ допустим</returns>
        public static bool ValidateRussianInput(KeyPressEventArgs e)
        {
            // Разрешаем русские буквы, пробел и Backspace
            if (char.IsLetter(e.KeyChar) && IsRussianLetter(e.KeyChar) ||
                e.KeyChar == ' ' ||
                e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false; // Разрешаем ввод
                return true;
            }
            else
            {
                e.Handled = true; // Блокируем ввод
                return false;
            }
        }

        /// <summary>
        /// Валидация ввода для числовых полей
        /// Разрешает только цифры и управляющие символы
        /// </summary>
        /// <param name="e">Событие KeyPress</param>
        /// <returns>true если символ допустим</returns>
        public static bool ValidateDigitInput(KeyPressEventArgs e)
        {
            // Разрешаем только цифры и Backspace
            if (char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false; // Разрешаем ввод
                return true;
            }
            else
            {
                e.Handled = true; // Блокируем ввод
                return false;
            }
        }

        /// <summary>
        /// Проверка, является ли символ русской буквой
        /// </summary>
        /// <param name="c">Проверяемый символ</param>
        /// <returns>true если символ - русская буква</returns>
        private static bool IsRussianLetter(char c)
        {
            return (c >= 'А' && c <= 'я') || c == 'Ё' || c == 'ё';
        }

        /// <summary>
        /// Форматирование слова (первая буква заглавная, остальные строчные)
        /// Корректно обрабатывает слова с дефисом (например, "Салтыков-Щедрин")
        /// </summary>
        /// <param name="word">Исходное слово</param>
        /// <returns>Отформатированное слово</returns>
        public static string FormatWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return word;

            // Убираем лишние пробелы в начале и конце
            word = word.Trim();

            // Если есть дефис, форматируем каждую часть отдельно
            if (word.Contains('-'))
            {
                var parts = word.Split('-');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Length > 0)
                    {
                        // Первая буква заглавная, остальные строчные
                        parts[i] = char.ToUpper(parts[i][0], CultureInfo.CurrentCulture) +
                                   parts[i].Substring(1).ToLower(CultureInfo.CurrentCulture);
                    }
                }
                return string.Join("-", parts);
            }

            // Если слово без дефиса, просто форматируем
            if (word.Length > 0)
            {
                return char.ToUpper(word[0], CultureInfo.CurrentCulture) +
                       word.Substring(1).ToLower(CultureInfo.CurrentCulture);
            }

            return word;
        }

        /// <summary>
        /// Автоматическое форматирование слова при изменении текста
        /// Используется для "живого" форматирования во время ввода
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
        public static void FormatWordOnTextChanged(TextBox textBox)
        {
            // Проверяем, что поле не пустое и не содержит плейсхолдер
            if (textBox == null || string.IsNullOrEmpty(textBox.Text) ||
                textBox.Text == "Фамилия" || textBox.Text == "Имя" || textBox.Text == "Отчество")
                return;

            // Получаем текущую позицию курсора
            int cursorPosition = textBox.SelectionStart;

            // Форматируем текст
            string originalText = textBox.Text;
            string formattedText = FormatWord(originalText);

            // Если текст изменился, обновляем
            if (originalText != formattedText)
            {
                textBox.Text = formattedText;

                // Восстанавливаем позицию курсора (чтобы курсор не прыгал)
                if (cursorPosition <= formattedText.Length)
                {
                    textBox.SelectionStart = cursorPosition;
                }
                else
                {
                    textBox.SelectionStart = formattedText.Length;
                }
            }
        }

        /// <summary>
        /// Форматирование слова при потере фокуса
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
        /// <param name="placeholder">Текст плейсхолдера</param>
        public static void FormatWordOnLeave(TextBox textBox, string placeholder)
        {
            if (textBox == null || string.IsNullOrWhiteSpace(textBox.Text) ||
                textBox.Text == placeholder)
                return;

            string formatted = FormatWord(textBox.Text);
            if (formatted != textBox.Text)
            {
                textBox.Text = formatted;
            }
        }

        /// <summary>
        /// Форматирование при нажатии пробела
        /// Форматирует уже введенную часть слова перед добавлением пробела
        /// </summary>
        /// <param name="textBox">Текстовое поле</param>
        /// <param name="e">Событие KeyPress</param>
        public static void FormatOnSpacePress(TextBox textBox, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && !string.IsNullOrEmpty(textBox.Text))
            {
                string formatted = FormatWord(textBox.Text);
                if (formatted != textBox.Text)
                {
                    textBox.Text = formatted;
                    // Ставим курсор в конец после форматирования
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }
    }
}