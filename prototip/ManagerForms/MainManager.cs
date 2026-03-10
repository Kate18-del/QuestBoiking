using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Главная форма для пользователей с ролью "Менеджер"
    /// Предоставляет доступ к основным разделам системы для работы с клиентами и заказами
    /// </summary>
    public partial class MainManager : Form
    {
        /// <summary>
        /// Конструктор формы главного меню менеджера
        /// </summary>
        public MainManager()
        {
            InitializeComponent();
            // Отображаем информацию о текущем пользователе при загрузке формы
            DisplayCurrentUser();
        }

        /// <summary>
        /// Отображает информацию о текущем менеджере в интерфейсе
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
                // Например: "Смирнова Анна Сергеевна" -> "Смирнова А.С."
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";

                // Отображаем приветствие с указанием роли и кратким ФИО
                label2.Text = $"менеджер {shortName}";
            }
        }

        /// <summary>
        /// Обработчик кнопки выхода из системы
        /// Возвращает пользователя на форму авторизации
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму менеджера
            this.Visible = false;

            // Создаем и открываем форму авторизации
            Autorisation auto = new Autorisation();
            auto.ShowDialog();

            // После закрытия формы авторизации снова показываем форму менеджера
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к просмотру услуг
        /// Открывает форму для просмотра доступных квестов и услуг
        /// </summary>
        private void btnServices_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму просмотра услуг (для менеджера)
            ServicesManager auto = new ServicesManager();
            auto.ShowDialog();

            // После закрытия формы возвращаемся в главное меню
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки создания нового заказа
        /// Открывает форму для оформления заказа клиенту
        /// </summary>
        private void btnMakingOrder_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму оформления заказа
            MakingOrder auto = new MakingOrder();
            auto.ShowDialog();

            // После закрытия формы возвращаемся в главное меню
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к управлению клиентами
        /// Открывает форму для работы со списком клиентов
        /// </summary>
        private void btnClients_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму управления клиентами
            Clients auto = new Clients();
            auto.ShowDialog();

            // После закрытия формы возвращаемся в главное меню
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к учету заказов
        /// Открывает форму для просмотра и управления заказами
        /// </summary>
        private void btnOrdersAccount_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму учета заказов (для менеджера)
            OrderAccountingManager auto = new OrderAccountingManager();
            auto.ShowDialog();

            // После закрытия формы возвращаемся в главное меню
            this.Visible = true;
        }
    }
}