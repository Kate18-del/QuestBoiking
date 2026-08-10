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
        private BindingList<Client> allClients;
        private List<Client> clientsList;
        private ClientRepository clientRepository;
        private Client selectedClient;

        // Для поиска
        private List<int> foundIndexes = new List<int>();
        private int currentFoundIndex = -1;

        public Clients()
        {
            InitializeComponent();
            clientRepository = new ClientRepository();
            InitializeForm();
            DisplayCurrentUser();
        }

        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] fioParts = CurrentUser.FIO.Split(' ');
                string shortName = $"{fioParts[0]} {fioParts[1].Substring(0, 1)}.{fioParts[2].Substring(0, 1)}.";
                label2.Text = $"Менеджер {shortName}";
            }
        }

        private void InitializeForm()
        {
            ConfigureDataGridView();
            ConfigureInputValidation();
            LoadClients();
            SubscribeToEvents();
            ClearFormFields();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.DefaultCellStyle.Font = new Font("Comic Sans MS", 9);
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "ClientID",
                HeaderText = "ClientID",
                DataPropertyName = "ClientID",
                Width = 170,
                Visible = false,
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "LastName",
                HeaderText = "Фамилия",
                DataPropertyName = "LastName",
                Width = 170
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FirstName",
                HeaderText = "Имя",
                DataPropertyName = "FirstName",
                Width = 170
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Surname",
                HeaderText = "Отчество",
                DataPropertyName = "Surname",
                Width = 150
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "PhoneNumber",
                HeaderText = "Телефон",
                DataPropertyName = "PhoneNumber",
                Width = 170
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Age",
                HeaderText = "Возраст",
                DataPropertyName = "Age",
                Width = 170
            });
        }

        private void ConfigureInputValidation()
        {
            textBox2.KeyPress += (s, e) => ClientValidator.ValidateRussianInput(e);
            textBox1.KeyPress += (s, e) => ClientValidator.ValidateRussianInput(e);
            textBox5.KeyPress += (s, e) => ClientValidator.ValidateRussianInput(e);
            textBox4.KeyPress += (s, e) => ClientValidator.ValidateDigitInput(e);
        }

        private void LoadClients()
        {
            try
            {
                var allClientsFromDb = clientRepository.GetAllClients();
                var deletedIds = DeletedRecordsManager.GetDeletedClientIds();
                var activeClients = allClientsFromDb
                    .Where(c => !deletedIds.Contains(c.ClientID))
                    .ToList();

                clientsList = activeClients;
                allClients = new BindingList<Client>(activeClients);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = allClients;
                UpdateRecordCount();
                dataGridView1.ClearSelection();
                selectedClient = null;
                btnEdit.Enabled = false;
                foundIndexes.Clear();
                currentFoundIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SubscribeToEvents()
        {
            textBox6.TextChanged += TextBox6_TextChanged;
            textBox6.KeyDown += TextBox6_KeyDown;
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnClear.Click += btnClear_Click;
            button1.Click += btnReset_Click;
            btnMenu.Click += btnMenu_Click;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            dataGridView1.KeyDown += DataGridView1_KeyDown;
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

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox6.Text.Trim();

            // Если поле пустое или плейсхолдер - выходим
            if (string.IsNullOrEmpty(searchText) || searchText == "Поиск по телефону...")
                return;

            // Оставляем только цифры
            string digits = new string(searchText.Where(char.IsDigit).ToArray());

            // Если меньше 4 цифр - выходим
            if (digits.Length < 4)
                return;

            // Ищем все совпадения
            foundIndexes.Clear();
            for (int i = 0; i < clientsList.Count; i++)
            {
                string phone = new string(clientsList[i].PhoneNumber.Where(char.IsDigit).ToArray());
                if (phone.Contains(digits))
                    foundIndexes.Add(i);
            }

            // Выделяем первое совпадение
            if (foundIndexes.Count > 0)
            {
                currentFoundIndex = 0;
                SelectFoundRow(foundIndexes[0]);
            }
        }

        private void TextBox6_KeyDown(object sender, KeyEventArgs e)
        {
            // Пробел - следующее совпадение
            if (e.KeyCode == Keys.Space && foundIndexes.Count > 0)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                currentFoundIndex++;
                if (currentFoundIndex >= foundIndexes.Count)
                    currentFoundIndex = 0;

                SelectFoundRow(foundIndexes[currentFoundIndex]);
            }
        }

        private void SelectFoundRow(int rowIndex)
        {
            if (rowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[rowIndex].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            textBox6.Text = "Поиск по телефону...";
            textBox6.ForeColor = SystemColors.GrayText;
            foundIndexes.Clear();
            currentFoundIndex = -1;
            dataGridView1.ClearSelection();
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Только цифры
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
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
        /// Обработчик кнопки возврата в главное меню
        /// </summary>
        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainManager auto = new MainManager();
            auto.ShowDialog();
            this.Close();
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
    }
}