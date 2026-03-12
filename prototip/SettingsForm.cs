using System;
using System.Configuration;
using System.Windows.Forms;


namespace prototip
{
    public partial class SettingsForm : Form
    {
        private NumericUpDown nudTimeout;
        private CheckBox chkEnableAutoLock;
        private Button btnSave;
        private Button btnCancel;

        public SettingsForm()
        {
            InitializeComponent();
            InitializeComponents();
            LoadSettings();
        }

        private void InitializeComponents()
        {
            this.Text = "Настройки безопасности";
            this.Size = new System.Drawing.Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label для таймаута
            Label lblTimeout = new Label()
            {
                Text = "Время бездействия (секунд):",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 20)
            };

            // NumericUpDown для ввода времени
            nudTimeout = new NumericUpDown()
            {
                Location = new System.Drawing.Point(230, 20),
                Size = new System.Drawing.Size(60, 20),
                Minimum = 10,
                Maximum = 600,
                Value = 30
            };

            // CheckBox для включения/выключения
            chkEnableAutoLock = new CheckBox()
            {
                Text = "Включить автоматическую блокировку",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(250, 20),
                Checked = true
            };

            // Кнопка сохранения
            btnSave = new Button()
            {
                Text = "Сохранить",
                Location = new System.Drawing.Point(120, 120),
                Size = new System.Drawing.Size(100, 30),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            // Кнопка отмены
            btnCancel = new Button()
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(230, 120),
                Size = new System.Drawing.Size(100, 30),
                DialogResult = DialogResult.Cancel
            };

            // Добавляем контролы на форму
            this.Controls.AddRange(new Control[] {
                lblTimeout,
                nudTimeout,
                chkEnableAutoLock,
                btnSave,
                btnCancel
            });
        }

        private void LoadSettings()
        {
            try
            {
                // Загружаем текущие настройки
                string timeout = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"];
                if (!string.IsNullOrEmpty(timeout) && int.TryParse(timeout, out int seconds))
                {
                    nudTimeout.Value = seconds;
                }

                string enabled = ConfigurationManager.AppSettings["EnableAutoLock"];
                if (!string.IsNullOrEmpty(enabled) && bool.TryParse(enabled, out bool isEnabled))
                {
                    chkEnableAutoLock.Checked = isEnabled;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки настроек: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Сохраняем настройки
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Сохраняем таймаут
                if (config.AppSettings.Settings["InactivityTimeoutSeconds"] != null)
                {
                    config.AppSettings.Settings["InactivityTimeoutSeconds"].Value = nudTimeout.Value.ToString();
                }
                else
                {
                    config.AppSettings.Settings.Add("InactivityTimeoutSeconds", nudTimeout.Value.ToString());
                }

                // Сохраняем флаг включения
                if (config.AppSettings.Settings["EnableAutoLock"] != null)
                {
                    config.AppSettings.Settings["EnableAutoLock"].Value = chkEnableAutoLock.Checked.ToString().ToLower();
                }
                else
                {
                    config.AppSettings.Settings.Add("EnableAutoLock", chkEnableAutoLock.Checked.ToString().ToLower());
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                MessageBox.Show("Настройки успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения настроек: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}