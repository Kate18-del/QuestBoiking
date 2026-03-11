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
    /// Форма для управления клиентами (доступна для менеджеров)
    /// Позволяет просматривать, добавлять, редактировать, удалять и искать клиентов
    /// </summary>
    public partial class Clients : Form
    {
        // Коллекция для привязки к DataGridView с поддержкой уведомлений об изменениях
        private BindingList<Client> allClients;

        // Список для поиска и фильтрации (без привязки)
        private List<Client> clientsList;

        // Репозиторий для работы с данными клиентов в БД
        private ClientRepository clientRepository;

        // Текущий выбранный клиент в таблице
        private Client selectedClient;

        /// <summary>
        /// Конструктор формы управления клиентами
        /// </summary>
        public Clients()
        {
            InitializeComponent();
            // Инициализация репозитория
            clientRepository = new ClientRepository();
            // Настройка всех элементов формы
            InitializeForm();
            // Отображение информации о текущем менеджере
            DisplayCurrentUser();
        }

        /// <summary>
        /// Отображение информации о текущем менеджере
        /// Формирует краткое ФИО в формате "Фамилия И.О."
        /// </summary>
        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                label2.Text = $"Менеджер {shortName}";
            }
        }

        /// <summary>
        /// Инициализация всех элементов формы
        /// </summary>
        private void InitializeForm()
        {
            // Настройка таблицы для отображения клиентов
            ConfigureDataGridView();

            // Настройка валидации ввода
            ConfigureInputValidation();

            // Загрузка списка клиентов
            LoadClients();

            // Подписка на события
            SubscribeToEvents();

            // Очистка полей ввода (установка плейсхолдеров)
            ClearFormFields();
        }
        /// <summary>
        /// Настройка столбцов DataGridView для отображения информации о клиентах
        /// с маскированием персональных данных
        /// </summary>
        private void ConfigureDataGridView()
        {
            // Отключаем автоматическую генерацию столбцов
            dataGridView1.AutoGenerateColumns = false;

            // Настройка режимов отображения
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Установка шрифта
            this.dataGridView1.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);

            // Очистка существующих столбцов
            dataGridView1.Columns.Clear();

            // ID клиента (скрытая колонка)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ClientID",
                HeaderText = "ClientID",
                DataPropertyName = "ClientID",
                Width = 170,
                Visible = false,
            });

            // Фамилия (с маскировкой)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "LastName",
                HeaderText = "Фамилия",
                DataPropertyName = "DisplayLastName", // Используем маскированное свойство
                Width = 170
            });

            // Имя (с маскировкой)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FirstName",
                HeaderText = "Имя",
                DataPropertyName = "DisplayFirstName", // Используем маскированное свойство
                Width = 170
            });

            // Отчество (с маскировкой)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Surname",
                HeaderText = "Отчество",
                DataPropertyName = "DisplaySurname", // Используем маскированное свойство
                Width = 150
            });

            // Номер телефона (с маскировкой)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PhoneNumber",
                HeaderText = "Телефон",
                DataPropertyName = "DisplayPhoneNumber", // Используем маскированное свойство
                Width = 170
            });

            // Возраст (скрыт)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Age",
                HeaderText = "Возраст",
                DataPropertyName = "DisplayAge", // Используем маскированное свойство
                Width = 170
            });

            // Добавляем колонку с кнопкой для просмотра детальной информации
            DataGridViewButtonColumn detailsButton = new DataGridViewButtonColumn();
            detailsButton.Name = "Details";
            detailsButton.HeaderText = "Детали";
            detailsButton.Text = "Просмотр";
            detailsButton.UseColumnTextForButtonValue = true;
            detailsButton.Width = 100;
            dataGridView1.Columns.Add(detailsButton);
        }

        /// <summary>
        /// Настройка валидации ввода для текстовых полей
        /// Использует статические методы из ClientValidator
        /// </summary>
        private void ConfigureInputValidation()
        {
            // Поля ФИО - только русские буквы
            textBox2.KeyPress += (s, e) => ClientValidator.ValidateRussianInput(e);
            textBox1.KeyPress += (s, e) => ClientValidator.ValidateRussianInput(e);
            textBox5.KeyPress += (s, e) => ClientValidator.ValidateRussianInput(e);

            // Поле возраста - только цифры
            textBox4.KeyPress += (s, e) => ClientValidator.ValidateDigitInput(e);
        }

        /// <summary>
        /// Загрузка всех активных клиентов из базы данных
        /// Применяет фильтрацию для скрытия "удаленных" записей
        /// </summary>
        private void LoadClients()
        {
            try
            {
                Console.WriteLine("=== Начало загрузки клиентов ===");

                // 1. Получаем ВСЕХ клиентов из БД (включая помеченных как удаленные)
                var allClientsFromDb = clientRepository.GetAllClients();
                Console.WriteLine($"Всего клиентов в БД: {allClientsFromDb.Count}");

                // 2. Получаем список ID клиентов, которые были удалены (но физически остались в БД)
                var deletedIds = DeletedRecordsManager.GetDeletedClientIds();
                Console.WriteLine($"Удаленных ID найдено: {deletedIds.Count}");
                if (deletedIds.Count > 0)
                {
                    Console.WriteLine($"Удаленные ID: {string.Join(", ", deletedIds)}");
                }

                // 3. Фильтруем: оставляем только НЕ удаленных клиентов
                var activeClients = allClientsFromDb
                    .Where(c => !deletedIds.Contains(c.ClientID))
                    .ToList();

                Console.WriteLine($"Будет показано активных клиентов: {activeClients.Count}");

                // 4. Логируем для проверки фильтрации
                foreach (var client in allClientsFromDb)
                {
                    if (deletedIds.Contains(client.ClientID))
                    {
                        Console.WriteLine($"Клиент ID {client.ClientID} {client.LastName} должен быть скрыт");
                    }
                }

                // 5. Сохраняем в оба списка для разных целей
                clientsList = activeClients; // Для поиска
                allClients = new BindingList<Client>(activeClients); // Для привязки

                // 6. Обновляем DataSource (сначала очищаем для корректного обновления)
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = allClients;

                // 7. Обновляем счетчик записей
                UpdateRecordCount();

                // 8. Снимаем выделение и сбрасываем выбранного клиента
                dataGridView1.ClearSelection();
                selectedClient = null;
                btnEdit.Enabled = false;

                Console.WriteLine("=== Загрузка клиентов завершена ===\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Подписка на все события формы
        /// </summary>
        private void SubscribeToEvents()
        {
            // Поиск при изменении текста
            textBox6.TextChanged += OnSearchTextChanged;

            // Кнопки
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnClear.Click += btnClear_Click;
            button1.Click += btnReset_Click;
            btnMenu.Click += btnMenu_Click;

            // События DataGridView
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.KeyDown += DataGridView1_KeyDown;

            // Настройка плейсхолдеров и автоформатирования
            SetupPlaceholders();
            SetupAutoFormatting();
        }

        /// <summary>
        /// Настройка автоматического форматирования полей ФИО
        /// (заглавная буква, форматирование после пробела)
        /// </summary>
        private void SetupAutoFormatting()
        {
            // Для поля "Фамилия"
            textBox2.KeyPress += (s, e) =>
            {
                ClientValidator.ValidateRussianInput(e);
                if (!e.Handled && e.KeyChar == ' ')
                {
                    ClientValidator.FormatOnSpacePress(textBox2, e);
                }
            };
            textBox2.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(textBox2.Text) && textBox2.Text != "Фамилия")
                {
                    ClientValidator.FormatWordOnTextChanged(textBox2);
                }
            };

            // Для поля "Имя" (аналогично)
            textBox1.KeyPress += (s, e) =>
            {
                ClientValidator.ValidateRussianInput(e);
                if (!e.Handled && e.KeyChar == ' ')
                {
                    ClientValidator.FormatOnSpacePress(textBox1, e);
                }
            };
            textBox1.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && textBox1.Text != "Имя")
                {
                    ClientValidator.FormatWordOnTextChanged(textBox1);
                }
            };

            // Для поля "Отчество" (аналогично)
            textBox5.KeyPress += (s, e) =>
            {
                ClientValidator.ValidateRussianInput(e);
                if (!e.Handled && e.KeyChar == ' ')
                {
                    ClientValidator.FormatOnSpacePress(textBox5, e);
                }
            };
            textBox5.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(textBox5.Text) && textBox5.Text != "Отчество")
                {
                    ClientValidator.FormatWordOnTextChanged(textBox5);
                }
            };
        }

        /// <summary>
        /// Настройка плейсхолдеров для всех текстовых полей
        /// </summary>
        private void SetupPlaceholders()
        {
            // Фамилия
            textBox2.Text = "Фамилия";
            textBox2.ForeColor = SystemColors.GrayText;
            textBox2.Enter += (s, e) => TextBoxEnter(textBox2, "Фамилия");
            textBox2.Leave += (s, e) => TextBoxLeave(textBox2, "Фамилия");

            // Имя
            textBox1.Text = "Имя";
            textBox1.ForeColor = SystemColors.GrayText;
            textBox1.Enter += (s, e) => TextBoxEnter(textBox1, "Имя");
            textBox1.Leave += (s, e) => TextBoxLeave(textBox1, "Имя");

            // Отчество
            textBox5.Text = "Отчество";
            textBox5.ForeColor = SystemColors.GrayText;
            textBox5.Enter += (s, e) => TextBoxEnter(textBox5, "Отчество");
            textBox5.Leave += (s, e) => TextBoxLeave(textBox5, "Отчество");

            // Возраст
            textBox4.Text = "Возраст";
            textBox4.ForeColor = SystemColors.GrayText;
            textBox4.Enter += (s, e) => TextBoxEnter(textBox4, "Возраст");
            textBox4.Leave += (s, e) => TextBoxLeave(textBox4, "Возраст");

            // Поиск
            textBox6.Text = "Поиск по телефону...";
            textBox6.ForeColor = SystemColors.GrayText;
            textBox6.Enter += (s, e) => TextBoxEnter(textBox6, "Поиск по телефону...");
            textBox6.Leave += (s, e) => TextBoxLeave(textBox6, "Поиск по телефону...");
        }

        /// <summary>
        /// Обработчик получения фокуса текстовым полем
        /// Очищает поле от плейсхолдера
        /// </summary>
        private void TextBoxEnter(TextBox textBox, string placeholder)
        {
            if (textBox.Text == placeholder)
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса текстовым полем
        /// Восстанавливает плейсхолдер если поле пустое
        /// </summary>
        private void TextBoxLeave(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = SystemColors.GrayText;
            }
        }

        /// <summary>
        /// Очистка всех полей ввода
        /// Сбрасывает плейсхолдеры и отключает кнопку редактирования
        /// </summary>
        private void ClearFormFields()
        {
            textBox2.Text = "Фамилия";
            textBox2.ForeColor = SystemColors.GrayText;

            textBox1.Text = "Имя";
            textBox1.ForeColor = SystemColors.GrayText;

            textBox5.Text = "Отчество";
            textBox5.ForeColor = SystemColors.GrayText;

            maskedTextBox1.Clear();

            textBox4.Text = "Возраст";
            textBox4.ForeColor = SystemColors.GrayText;

            selectedClient = null;
            btnEdit.Enabled = false;
            btnClear.Enabled = true;
        }

        /// <summary>
        /// Обновление счетчика количества записей
        /// </summary>
        private void UpdateRecordCount()
        {
            label3.Text = $"Количество записей: {dataGridView1.Rows.Count}";
        }

        /// <summary>
        /// Обработчик изменения текста поиска
        /// Запускает фильтрацию
        /// </summary>
        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        /// <summary>
        /// Применение фильтра поиска по номеру телефона
        /// Поиск осуществляется по первым 4 цифрам
        /// </summary>
        private void ApplySearchFilter()
        {
            if (clientsList == null) return;

            string searchText = textBox6.Text.Trim();

            // Если поиск пустой или плейсхолдер - показываем всех клиентов
            if (string.IsNullOrWhiteSpace(searchText) || searchText == "Поиск по телефону...")
            {
                dataGridView1.DataSource = allClients;
            }
            else
            {
                // Извлекаем только цифры из поискового запроса
                string phoneDigits = new string(searchText.Where(char.IsDigit).ToArray());

                // Если введено минимум 4 цифры, выполняем поиск
                if (phoneDigits.Length >= 4)
                {
                    var filtered = clientsList.Where(c =>
                        c.PhoneNumber.Contains(phoneDigits.Substring(0, Math.Min(4, phoneDigits.Length))))
                        .ToList();

                    dataGridView1.DataSource = new BindingList<Client>(filtered);
                }
                else
                {
                    // Если цифр меньше 4, показываем всех
                    dataGridView1.DataSource = allClients;
                }
            }

            UpdateRecordCount();
        }

        /// <summary>
        /// Обработчик изменения выделения в DataGridView
        /// Заполняет поля формы данными выбранного клиента
        /// </summary>
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].DataBoundItem is Client client)
            {
                selectedClient = client;

                // Заполнение полей данными выбранного клиента
                textBox2.Text = selectedClient.LastName;
                textBox2.ForeColor = SystemColors.WindowText;

                textBox1.Text = selectedClient.FirstName;
                textBox1.ForeColor = SystemColors.WindowText;

                textBox5.Text = selectedClient.Surname ?? "";
                textBox5.ForeColor = SystemColors.WindowText;

                maskedTextBox1.Text = selectedClient.PhoneNumber;

                textBox4.Text = selectedClient.Age.HasValue ? selectedClient.Age.Value.ToString() : "";
                textBox4.ForeColor = SystemColors.WindowText;

                // Включаем кнопку редактирования
                btnEdit.Enabled = true;
            }
            else
            {
                // Если выделение снято - сбрасываем
                selectedClient = null;
                btnEdit.Enabled = false;
            }
        }

        /// <summary>
        /// Обработчик двойного щелчка по ячейке
        /// Выделяет строку и заполняет поля
        /// </summary>
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView1_SelectionChanged(sender, e);
            }
        }

        /// <summary>
        /// Обработчик кнопки добавления нового клиента
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Автоматическое форматирование полей перед валидацией
                if (textBox2.Text != "Фамилия" && !string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    textBox2.Text = ClientValidator.FormatWord(textBox2.Text);
                }
                if (textBox1.Text != "Имя" && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    textBox1.Text = ClientValidator.FormatWord(textBox1.Text);
                }
                if (textBox5.Text != "Отчество" && !string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    textBox5.Text = ClientValidator.FormatWord(textBox5.Text);
                }

                // Валидация введенных данных
                if (!ClientValidator.ValidateInput(
                    textBox2.Text,
                    textBox1.Text,
                    maskedTextBox1.Text,
                    textBox4.Text, // Передаем текст с маской
                    out int? age,
                    out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Фокусируемся на проблемном поле
                    if (errorMessage.Contains("фамилию")) textBox2.Focus();
                    else if (errorMessage.Contains("имя")) textBox1.Focus();
                    else if (errorMessage.Contains("телефон")) maskedTextBox1.Focus();
                    else if (errorMessage.Contains("Возраст")) textBox4.Focus();

                    return;
                }

                // Дополнительная проверка маски телефона
                if (!maskedTextBox1.MaskCompleted)
                {
                    MessageBox.Show("Введите корректный номер телефона", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    maskedTextBox1.Focus();
                    return;
                }

                // Создание объекта клиента
                var client = new Client
                {
                    LastName = textBox2.Text.Trim(),
                    FirstName = textBox1.Text.Trim(),
                    Surname = string.IsNullOrWhiteSpace(textBox5.Text) || textBox5.Text == "Отчество" ?
                              null : textBox5.Text.Trim(),
                    PhoneNumber = maskedTextBox1.Text, // Сохраняем с маской
                    Age = age
                };

                // Добавление в базу данных
                int newClientId = clientRepository.AddClient(client);

                MessageBox.Show($"Клиент успешно добавлен", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей и перезагрузка списка
                ClearFormFields();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления клиента",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки редактирования клиента
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Автоматическое форматирование полей
                if (textBox2.Text != "Фамилия" && !string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    textBox2.Text = ClientValidator.FormatWord(textBox2.Text);
                }
                if (textBox1.Text != "Имя" && !string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    textBox1.Text = ClientValidator.FormatWord(textBox1.Text);
                }
                if (textBox5.Text != "Отчество" && !string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    textBox5.Text = ClientValidator.FormatWord(textBox5.Text);
                }

                // Валидация
                if (!ClientValidator.ValidateInput(
                    textBox2.Text,
                    textBox1.Text,
                    maskedTextBox1.Text,
                    textBox4.Text,
                    out int? age,
                    out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (errorMessage.Contains("фамилию")) textBox2.Focus();
                    else if (errorMessage.Contains("имя")) textBox1.Focus();
                    else if (errorMessage.Contains("телефон")) maskedTextBox1.Focus();
                    else if (errorMessage.Contains("Возраст")) textBox4.Focus();

                    return;
                }

                // Проверка маски телефона
                if (!maskedTextBox1.MaskCompleted)
                {
                    MessageBox.Show("Введите корректный номер телефона", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    maskedTextBox1.Focus();
                    return;
                }

                // Получение ID выбранного клиента
                var selectedRow = dataGridView1.SelectedRows[0];
                int clientId = Convert.ToInt32(selectedRow.Cells["ClientID"].Value);

                // Создание объекта с обновленными данными
                var client = new Client
                {
                    ClientID = clientId,
                    LastName = textBox2.Text.Trim(),
                    FirstName = textBox1.Text.Trim(),
                    Surname = string.IsNullOrWhiteSpace(textBox5.Text) || textBox5.Text == "Отчество" ?
                              null : textBox5.Text.Trim(),
                    PhoneNumber = maskedTextBox1.Text,
                    Age = age
                };

                // Обновление в БД
                bool success = clientRepository.UpdateClient(client);

                if (success)
                {
                    MessageBox.Show("Данные клиента успешно обновлены", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Очистка полей и перезагрузка списка
                    ClearFormFields();
                    LoadClients();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка обновления клиента",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки очистки формы
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
            dataGridView1.ClearSelection();
        }

        /// <summary>
        /// Обработчик кнопки сброса поиска
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            textBox6.Text = "Поиск по телефону...";
            textBox6.ForeColor = SystemColors.GrayText;
            LoadClients();
        }

        /// <summary>
        /// Обработчик кнопки возврата в главное меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MainManager auto = new MainManager();
            auto.ShowDialog();
            this.Visible = true;
        }

        /// <summary>
        /// Обработчик нажатия клавиш в DataGridView
        /// Обрабатывает Delete для удаления клиента
        /// </summary>
        private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dataGridView1.SelectedRows.Count > 0)
            {
                DeleteSelectedClient();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Удаление выбранного клиента (мягкое удаление с сохранением в архиве)
        /// </summary>
        private void DeleteSelectedClient()
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            // Получение данных выбранного клиента
            var selectedRow = dataGridView1.SelectedRows[0];
            int clientId = Convert.ToInt32(selectedRow.Cells["ClientID"].Value);
            string lastName = selectedRow.Cells["LastName"].Value?.ToString() ?? "";
            string firstName = selectedRow.Cells["FirstName"].Value?.ToString() ?? "";
            string surname = selectedRow.Cells["Surname"].Value?.ToString() ?? "";
            string phoneNumber = selectedRow.Cells["PhoneNumber"].Value?.ToString() ?? "";

            string clientName = $"{lastName} {firstName} {surname}".Trim();

            // Диалог подтверждения
            var result = MessageBox.Show($"Вы уверены, что хотите удалить клиента '{clientName}'?\n" +
                                        $"Телефон: {phoneNumber}",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Вызов метода удаления (мягкое удаление)
                    if (clientRepository.DeleteClient(clientId, lastName, firstName, surname, phoneNumber))
                    {
                        // Показ информации об удаленном клиенте
                        string deletedInfo = $"Удален клиент:\n" +
                                            $"ФИО: {clientName}\n" +
                                            $"Телефон: {phoneNumber}\n" +
                                            $"ID: {clientId}";

                        MessageBox.Show(deletedInfo, "Клиент успешно удален",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Очистка и перезагрузка
                        ClearFormFields();
                        dataGridView1.ClearSelection();
                        LoadClients();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Ограничение ввода в поле поиска только цифрами
        /// </summary>
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}