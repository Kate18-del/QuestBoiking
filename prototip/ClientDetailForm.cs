using System;
using System.Windows.Forms;

namespace prototip
{
    /// <summary>
    /// Форма для детального просмотра персональных данных клиента
    /// </summary>
    public partial class ClientDetailForm : Form
    {
        private Client _client;
        private bool _isAuthorized;

        /// <summary>
        /// Конструктор формы детального просмотра
        /// </summary>
        /// <param name="client">Клиент для просмотра</param>
        /// <param name="isAuthorized">Флаг авторизации для просмотра полных данных</param>
        public ClientDetailForm(Client client, bool isAuthorized = false)
        {
            InitializeComponent();
            _client = client;
            _isAuthorized = isAuthorized;

            // Загружаем данные клиента
            LoadClientData();

            // Настраиваем отображение в зависимости от прав
            ConfigureAccess();
        }

        /// <summary>
        /// Загрузка данных клиента в форму
        /// </summary>
        private void LoadClientData()
        {
            // Основная информация
            lblFullName.Text = $"{_client.LastName} {_client.FirstName} {_client.Surname}".Trim();
            lblPhone.Text = _client.PhoneNumber ?? "Не указан";
            lblAge.Text = _client.Age?.ToString() ?? "Не указан";

            // Дополнительная информация (скрыта по умолчанию)
            lblBirthDateLabel.Text = _client.BirthDate?.ToString("dd.MM.yyyy") ?? "Не указана";
            lblAddressLabel.Text = _client.Address ?? "Не указан";
            lblEmailLabel.Text = _client.Email ?? "Не указан";
            lblPassportLabel.Text = _client.PassportData ?? "Не указаны";

            // Информация о доступе
            lblAccessLevel.Text = _isAuthorized ? "Полный доступ" : "Ограниченный доступ (только для чтения)";
        }

        /// <summary>
        /// Настройка доступа к данным
        /// </summary>
        private void ConfigureAccess()
        {
            if (!_isAuthorized)
            {
                // Если нет прав - блокируем кнопки редактирования
                btnEdit.Visible = false;
                btnSave.Visible = false;

                // Делаем поля только для чтения
                txtBirthDate.ReadOnly = true;
                txtAddress.ReadOnly = true;
                txtEmail.ReadOnly = true;
                txtPassport.ReadOnly = true;

                // Отображаем данные, но без возможности изменения
                txtBirthDate.Text = _client.BirthDate?.ToString("dd.MM.yyyy") ?? "";
                txtAddress.Text = _client.Address ?? "";
                txtEmail.Text = _client.Email ?? "";
                txtPassport.Text = _client.PassportData ?? "";
            }
            else
            {
                // Если есть права - показываем поля для редактирования
                txtBirthDate.Text = _client.BirthDate?.ToString("dd.MM.yyyy") ?? "";
                txtAddress.Text = _client.Address ?? "";
                txtEmail.Text = _client.Email ?? "";
                txtPassport.Text = _client.PassportData ?? "";
            }
        }

        /// <summary>
        /// Обработчик кнопки редактирования
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Включаем режим редактирования
            txtBirthDate.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtEmail.ReadOnly = false;
            txtPassport.ReadOnly = false;

            btnEdit.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        /// <summary>
        /// Обработчик кнопки сохранения
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Сохраняем изменения
                if (DateTime.TryParse(txtBirthDate.Text, out DateTime birthDate))
                    _client.BirthDate = birthDate;

                _client.Address = txtAddress.Text;
                _client.Email = txtEmail.Text;
                _client.PassportData = txtPassport.Text;

                // Здесь должен быть код сохранения в БД
                // clientRepository.UpdateClient(_client);

                MessageBox.Show("Данные успешно сохранены", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Возвращаемся в режим просмотра
                txtBirthDate.ReadOnly = true;
                txtAddress.ReadOnly = true;
                txtEmail.ReadOnly = true;
                txtPassport.ReadOnly = true;

                btnEdit.Visible = true;
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки отмены
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Отменяем изменения и возвращаем исходные данные
            txtBirthDate.Text = _client.BirthDate?.ToString("dd.MM.yyyy") ?? "";
            txtAddress.Text = _client.Address ?? "";
            txtEmail.Text = _client.Email ?? "";
            txtPassport.Text = _client.PassportData ?? "";

            txtBirthDate.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtPassport.ReadOnly = true;

            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        /// <summary>
        /// Обработчик кнопки закрытия
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}