using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Главная форма для пользователей с ролью "Администратор"
    /// Предоставляет доступ ко всем функциям управления системой
    /// </summary>
    public partial class MainAdmin : Form
    {
        private InactivityTracker inactivityTracker;
        /// <summary>
        /// Конструктор формы главного меню администратора
        /// </summary>
        public MainAdmin()
        {
            InitializeComponent();
            // Отображаем информацию о текущем пользователе при загрузке формы
            DisplayCurrentUser();

            InitializeInactivityTracker();
;
        }

        /// <summary>
        /// Отображает информацию о текущем администраторе в интерфейсе
        /// Формирует краткое ФИО в формате: "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            // Проверяем, что данные о пользователе существуют
            if (CurrentUser.FIO != null)
            {
                // Разделяем полное ФИО на части (Фамилия, Имя, Отчество)
                string[] fioParts = CurrentUser.FIO.Split(' ');

                // Формируем краткое ФИО: первая часть полностью, от второй и третьей - первые буквы
                // Например: "Иванов Иван Иванович" -> "Иванов И.И."
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";

                // Отображаем приветствие с указанием роли и кратким ФИО
                label2.Text = $"администратор {shortName}";
            }
        }

        private void InitializeInactivityTracker()
        {
            inactivityTracker = new InactivityTracker(this);
            inactivityTracker.InactivityDetected += InactivityTracker_InactivityDetected;
            inactivityTracker.Start();
        }


        private void InactivityTracker_InactivityDetected(object sender, EventArgs e)
        {
            // Останавливаем трекер
            inactivityTracker.Stop();

            // Запоминаем текущую форму
            Autorisation.LastActiveForm = this;

            // Скрываем текущую форму
            this.Visible = false;

            // Создаем форму авторизации
            Autorisation authForm = new Autorisation();

            // Подписываемся на событие успешного входа
            authForm.LoginSucceeded += (s, args) =>
            {
                // Показываем форму заново
                this.Visible = true;

                // Перезапускаем трекер
                inactivityTracker.Start();
            };

            authForm.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            inactivityTracker?.Stop();
            base.OnFormClosing(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            inactivityTracker?.Start();
        }


        /// <summary>
        /// Обработчик кнопки выхода из системы
        /// Возвращает пользователя на форму авторизации
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму администратора
            this.Visible = false;

            // Создаем и открываем форму авторизации
            Autorisation auto = new Autorisation();
            auto.ShowDialog();

            // После закрытия формы авторизации снова показываем форму администратора
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к управлению пользователями
        /// Открывает форму для работы со списком пользователей
        /// </summary>
        private void btnUsers_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму управления пользователями
            Users auto = new Users();
            auto.ShowDialog();

            // После закрытия формы управления пользователями возвращаемся в главное меню
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к учету заказов
        /// Открывает форму для просмотра и управления заказами
        /// </summary>
        private void btnOrders_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму учета заказов (для администратора)
            OrderAccountingAdmin auto = new OrderAccountingAdmin();
            auto.ShowDialog();

            // После закрытия формы учета заказов возвращаемся в главное меню
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к справочникам
        /// Открывает форму для работы со справочной информацией (книги, авторы и т.д.)
        /// </summary>
        private void btnBooks_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму справочников (для администратора)
            ReferenceBooksAdmin auto = new ReferenceBooksAdmin();
            auto.ShowDialog();

            // После закрытия формы справочников возвращаемся в главное меню
            this.Visible = true;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                // Останавливаем трекер на время настроек
                inactivityTracker?.Stop();

                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // Если настройки сохранены, обновляем трекер
                    string timeout = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"];
                    if (!string.IsNullOrEmpty(timeout) && int.TryParse(timeout, out int seconds))
                    {
                        inactivityTracker?.UpdateTimeout(seconds);
                    }

                    string enabled = ConfigurationManager.AppSettings["EnableAutoLock"];
                    if (!string.IsNullOrEmpty(enabled) && bool.TryParse(enabled, out bool isEnabled))
                    {
                        inactivityTracker?.SetEnabled(isEnabled);
                    }

                    MessageBox.Show("Настройки применены!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Возобновляем трекер
                inactivityTracker?.Start();
            }
        }
    }
}