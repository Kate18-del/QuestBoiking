using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Статический класс для валидации и форматирования данных пользователя
    /// Содержит методы проверки корректности ввода, форматирования ФИО и управления плейсхолдерами
    /// </summary>
    public static class UserValidator
    {
        /// <summary>
        /// Комплексная проверка всех полей формы пользователя
        /// </summary>
        /// <param name="txtFIO">Поле ввода ФИО</param>
        /// <param name="txtLogin">Поле ввода логина</param>
        /// <param name="txtPassword">Поле ввода пароля</param>
        /// <param name="cmbRole">Выпадающий список ролей</param>
        /// <returns>true если все поля заполнены корректно</returns>
        public static bool ValidateForm(TextBox txtFIO, TextBox txtLogin, TextBox txtPassword, ComboBox cmbRole)
        {
            // ПРОВЕРКА ФИО
            if (txtFIO.Text == "ФИО" || string.IsNullOrWhiteSpace(txtFIO.Text))
            {
                MessageBox.Show("Введите ФИО пользователя!", "Ошибка");
                txtFIO.Focus();
                return false;
            }

            // Проверяем количество пробелов в ФИО (максимум 2 пробела = 3 слова)
            if (CountSpaces(txtFIO.Text) > 2)
            {
                MessageBox.Show("ФИО должно содержать только фамилию, имя и отчество (максимум 2 пробела)!", "Ошибка");
                txtFIO.Focus();
                return false;
            }

            // ПРОВЕРКА ЛОГИНА
            if (txtLogin.Text == "Логин" || string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин пользователя!", "Ошибка");
                txtLogin.Focus();
                return false;
            }

            // ПРОВЕРКА ПАРОЛЯ
            if (txtPassword.Text == "Пароль" || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль пользователя!", "Ошибка");
                txtPassword.Focus();
                return false;
            }

            // ПРОВЕРКА РОЛИ
            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите роль пользователя!", "Ошибка");
                cmbRole.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Подсчет количества пробелов в строке
        /// Используется для проверки формата ФИО (фамилия, имя, отчество)
        /// </summary>
        /// <param name="text">Анализируемая строка</param>
        /// <returns>Количество пробелов</returns>
        private static int CountSpaces(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            int spaceCount = 0;
            foreach (char c in text)
            {
                if (c == ' ')
                    spaceCount++;
            }
            return spaceCount;
        }

        /// <summary>
        /// Проверка, состоит ли текст только из русских букв и пробелов
        /// </summary>
        public static bool IsRussianText(string text)
        {
            return Regex.IsMatch(text, @"^[а-яА-ЯёЁ\s]+$");
        }

        /// <summary>
        /// Проверка, состоит ли текст только из английских букв, цифр и допустимых спецсимволов
        /// </summary>
        public static bool IsEnglishText(string text)
        {
            // Разрешаем английские буквы, цифры и некоторые спецсимволы для логина
            return Regex.IsMatch(text, @"^[a-zA-Z0-9_\-\.]+$");
        }

        /// <summary>
        /// Валидация ввода для полей с русским текстом
        /// Разрешает русские буквы, пробелы и управляющие символы
        /// </summary>
        public static void ValidateRussianInput(KeyPressEventArgs e)
        {
            // Разрешаем русские буквы (без пробела), пробел и управляющие символы
            if (!IsRussianLetterNoSpace(e.KeyChar) && e.KeyChar != ' ' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        /// <summary>
        /// Валидация ввода для поля пароля
        /// Разрешает английские буквы, цифры и специальные символы
        /// </summary>
        public static void ValidateEnglishInputPas(KeyPressEventArgs e)
        {
            // Разрешаем: английские буквы, цифры, символы _ - . @
            // и управляющие символы (Backspace, Delete и т.д.)
            bool isEnglishLetter = (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                                   (e.KeyChar >= 'A' && e.KeyChar <= 'Z');
            bool isDigit = char.IsDigit(e.KeyChar);
            bool isAllowedSymbol = e.KeyChar == '_' || e.KeyChar == '-' ||
                                  e.KeyChar == '.' || e.KeyChar == '@';
            bool isControlChar = char.IsControl(e.KeyChar);

            if (!(isEnglishLetter || isDigit || isAllowedSymbol || isControlChar))
            {
                e.Handled = true; // Блокируем ввод
            }
        }

        /// <summary>
        /// Форматирование полного ФИО
        /// Каждое слово начинается с заглавной буквы, остальные строчные
        /// </summary>
        /// <param name="fio">Исходная строка ФИО</param>
        /// <returns>Отформатированное ФИО</returns>
        public static string FormatFIO(string fio)
        {
            if (string.IsNullOrWhiteSpace(fio))
                return fio;

            // Убираем лишние пробелы в начале и конце
            fio = fio.Trim();

            // Разбиваем на слова по пробелам
            var words = fio.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Форматируем каждое слово
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = FormatWord(words[i]);
            }

            return string.Join(" ", words);
        }

        /// <summary>
        /// Форматирование отдельного слова с учетом дефиса
        /// </summary>
        /// <param name="word">Исходное слово</param>
        /// <returns>Отформатированное слово</returns>
        private static string FormatWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return word;

            // Если есть дефис, форматируем каждую часть отдельно
            if (word.Contains('-'))
            {
                var parts = word.Split('-');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Length > 0)
                    {
                        parts[i] = char.ToUpper(parts[i][0], CultureInfo.CurrentCulture) +
                                   parts[i].Substring(1).ToLower(CultureInfo.CurrentCulture);
                    }
                }
                return string.Join("-", parts);
            }

            // Если слово без дефиса, просто форматируем
            return char.ToUpper(word[0], CultureInfo.CurrentCulture) +
                   word.Substring(1).ToLower(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Форматирование ФИО при изменении текста (live-форматирование)
        /// Сохраняет позицию курсора после форматирования
        /// </summary>
        /// <param name="textBox">Текстовое поле с ФИО</param>
        public static void FormatFIOOnTextChanged(TextBox textBox)
        {
            if (textBox == null || string.IsNullOrEmpty(textBox.Text) || textBox.Text == "ФИО")
                return;

            // Получаем текущую позицию курсора
            int cursorPosition = textBox.SelectionStart;

            // Форматируем текст
            string originalText = textBox.Text;
            string formattedText = FormatFIO(originalText);

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
        /// Валидация ввода для поля логина
        /// Разрешает только английские буквы
        /// </summary>
        public static void ValidateEnglishInput(KeyPressEventArgs e)
        {
            if (!IsEnglishLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Проверка, является ли символ русской буквой (включая пробел)
        /// </summary>
        private static bool IsRussianLetter(char c)
        {
            return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') || c == 'Ё' || c == 'ё' || c == ' ';
        }

        /// <summary>
        /// Проверка, является ли символ английской буквой
        /// </summary>
        private static bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        /// <summary>
        /// Интеллектуальное форматирование при вводе каждой буквы
        /// Автоматически делает заглавными первые буквы слов и строчными остальные
        /// </summary>
        /// <param name="textBox">Текстовое поле с ФИО</param>
        /// <param name="e">Событие KeyPress</param>
        public static void FormatFIOOnKeyPress(TextBox textBox, KeyPressEventArgs e)
        {
            // Если это управляющий символ или валидация запретила ввод - выходим
            if (e.Handled || char.IsControl(e.KeyChar))
                return;

            // Получаем текущий текст и позицию курсора
            string text = textBox.Text;
            int cursorPos = textBox.SelectionStart;

            // ПРОВЕРКА НА ПРЕВЫШЕНИЕ ЛИМИТА ПРОБЕЛОВ
            if (e.KeyChar == ' ')
            {
                // Считаем текущее количество пробелов
                int currentSpaceCount = CountSpaces(text);

                // Если уже есть 2 пробела, запрещаем ввод еще одного (без сообщения)
                if (currentSpaceCount >= 2)
                {
                    e.Handled = true;
                    return;
                }

                // Если вводится пробел - форматируем предыдущее слово
                // Находим начало последнего слова
                int wordStart = FindLastWordStart(text, cursorPos);

                if (wordStart >= 0 && cursorPos > wordStart)
                {
                    string lastWord = text.Substring(wordStart, cursorPos - wordStart);
                    if (!string.IsNullOrWhiteSpace(lastWord) && lastWord.Length > 0)
                    {
                        // Форматируем слово
                        string formattedWord = FormatWord(lastWord);

                        // Если слово изменилось
                        if (formattedWord != lastWord)
                        {
                            // Заменяем слово в тексте
                            textBox.Text = text.Substring(0, wordStart) + formattedWord +
                                          text.Substring(cursorPos);

                            // Ставим курсор после отформатированного слова (перед пробелом)
                            textBox.SelectionStart = wordStart + formattedWord.Length;
                            e.Handled = false; // Разрешаем пробел
                        }
                    }
                }
            }
            // Если вводится буква в начале слова - делаем её заглавной
            else if (IsRussianLetterNoSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Проверяем, находимся ли мы в начале слова
                if (cursorPos == 0 ||
                    (cursorPos > 0 && text[cursorPos - 1] == ' ') ||
                    (cursorPos > 0 && text.Length > 0 && text[cursorPos - 1] == '-'))
                {
                    // Делаем букву заглавной
                    e.KeyChar = char.ToUpper(e.KeyChar, CultureInfo.CurrentCulture);
                }
                else
                {
                    // В середине слова - оставляем строчной
                    e.KeyChar = char.ToLower(e.KeyChar, CultureInfo.CurrentCulture);
                }
            }
        }

        /// <summary>
        /// Находит начало последнего слова перед курсором
        /// </summary>
        /// <param name="text">Текст в поле</param>
        /// <param name="cursorPos">Позиция курсора</param>
        /// <returns>Индекс начала слова</returns>
        private static int FindLastWordStart(string text, int cursorPos)
        {
            if (string.IsNullOrEmpty(text) || cursorPos == 0)
                return 0;

            // Ищем последний пробел перед курсором
            int lastSpace = text.LastIndexOf(' ', cursorPos - 1);

            // Если пробел найден, начало слова - после пробела
            // Если нет - начало с первого символа
            return lastSpace == -1 ? 0 : lastSpace + 1;
        }

        /// <summary>
        /// Проверяет, является ли символ русской буквой (без пробела)
        /// </summary>
        private static bool IsRussianLetterNoSpace(char c)
        {
            return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') || c == 'Ё' || c == 'ё';
        }
    }
}