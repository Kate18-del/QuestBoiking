using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace prototip
{
    /// <summary>
    /// Сервис для работы с пользователями, реализующий бизнес-логику
    /// Находится между пользовательским интерфейсом и репозиторием
    /// Отвечает за валидацию, хэширование паролей и выполнение бизнес-правил
    /// </summary>
    public class UserService
    {
        // Приватное поле для хранения экземпляра репозитория
        private readonly UserRepository _userRepository;

        /// <summary>
        /// Конструктор сервиса пользователей
        /// Инициализирует репозиторий для работы с данными
        /// </summary>
        public UserService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Получает список всех пользователей через репозиторий
        /// </summary>
        /// <returns>Список пользователей</returns>
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        /// <summary>
        /// Добавляет нового пользователя с проверкой уникальности логина и хэшированием пароля
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль (в открытом виде)</param>
        /// <param name="fio">ФИО пользователя</param>
        /// <param name="roleId">ID роли</param>
        /// <returns>true если добавление успешно</returns>
        public bool AddUser(string login, string password, string fio, int roleId)
        {
            // БИЗНЕС-ПРАВИЛО: Логин должен быть уникальным
            if (_userRepository.IsLoginExists(login))
                throw new Exception("Пользователь с таким логином уже существует");

            // Создание объекта пользователя
            var user = new User
            {
                Login = login,
                // БИЗНЕС-ПРАВИЛО: Пароль хранится только в хэшированном виде
                Password = ComputeSha256Hash(password),
                FIO = fio,
                IDRole = roleId
            };

            // Делегирование сохранения репозиторию
            return _userRepository.AddUser(user);
        }

        /// <summary>
        /// Обновляет данные существующего пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="login">Новый логин</param>
        /// <param name="password">Новый пароль (в открытом виде)</param>
        /// <param name="fio">Новое ФИО</param>
        /// <param name="roleId">Новая роль</param>
        /// <param name="currentPassword">Текущий пароль (для сравнения)</param>
        /// <returns>true если обновление успешно</returns>
        public bool UpdateUser(int userId, string login, string password, string fio, int roleId, string currentPassword)
        {
            // БИЗНЕС-ПРАВИЛО: Логин должен быть уникальным (исключая текущего пользователя)
            if (_userRepository.IsLoginExists(login, userId))
                throw new Exception("Пользователь с таким логином уже существует");

            var user = new User
            {
                UserID = userId,
                Login = login,
                // БИЗНЕС-ПРАВИЛО: Если пароль изменился - хэшируем, если нет - оставляем как есть
                Password = password == currentPassword ? currentPassword : ComputeSha256Hash(password),
                FIO = fio,
                IDRole = roleId
            };

            return _userRepository.UpdateUser(user);
        }

        /// <summary>
        /// Удаляет пользователя по ID
        /// </summary>
        /// <param name="userId">ID пользователя для удаления</param>
        /// <returns>true если удаление успешно</returns>
        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        /// <summary>
        /// Приватный метод для вычисления хэша SHA256 строки
        /// Используется для безопасного хранения паролей
        /// </summary>
        /// <param name="rawData">Исходная строка (пароль)</param>
        /// <returns>Хэш строки в шестнадцатеричном формате</returns>
        private string ComputeSha256Hash(string rawData)
        {
            // Создание объекта для вычисления хэша
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразование строки в массив байт и вычисление хэша
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Преобразование массива байт в строку шестнадцатеричных цифр
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // "x2" - две шестнадцатеричные цифры
                }
                return builder.ToString();
            }
        }
    }
}