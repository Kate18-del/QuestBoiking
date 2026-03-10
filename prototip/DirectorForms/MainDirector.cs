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
    /// Главная форма для пользователей с ролью "Директор"
    /// Предоставляет доступ к основным разделам системы для директора
    /// </summary>
    public partial class MainDirector : Form
    {
        /// <summary>
        /// Конструктор формы главного меню директора
        /// </summary>
        public MainDirector()
        {
            InitializeComponent();
            // Отображаем информацию о текущем пользователе при загрузке формы
            DisplayCurrentUser();
        }

        /// <summary>
        /// Отображает информацию о текущем директоре в интерфейсе
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
                // Например: "Петров Петр Петрович" -> "Петров П.П."
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";

                // Отображаем приветствие с указанием роли и кратким ФИО
                label2.Text = $"директор {shortName}";
            }
        }

        /// <summary>
        /// Обработчик кнопки выхода из системы
        /// Возвращает пользователя на форму авторизации
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму директора
            this.Visible = false;

            // Создаем и открываем форму авторизации
            Autorisation auto = new Autorisation();
            auto.ShowDialog();

            // После закрытия формы авторизации снова показываем форму директора
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к учету заказов
        /// Открывает форму для просмотра и анализа заказов (для директора)
        /// </summary>
        private void btnOrders_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму учета заказов (для директора)
            // Директор имеет доступ к просмотру заказов, но с ограниченными правами
            OrderAccountingDirector auto = new OrderAccountingDirector();
            auto.ShowDialog();

            // После закрытия формы учета заказов возвращаемся в главное меню
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки перехода к управлению услугами
        /// Открывает форму для просмотра и редактирования услуг (квестов)
        /// </summary>
        private void btnServices_Click(object sender, EventArgs e)
        {
            // Скрываем текущую форму
            this.Visible = false;

            // Создаем и открываем форму управления услугами (для директора)
            // Директор имеет доступ к управлению услугами (добавление, редактирование, удаление)
            ServicesDirector auto = new ServicesDirector();
            auto.ShowDialog();

            // После закрытия формы управления услугами возвращаемся в главное меню
            this.Visible = true;
        }
    }
}