using System;
using System.Configuration;
using System.Windows.Forms; // Для Forms.Timer
using System.Collections.Generic;

namespace prototip
{
    /// <summary>
    /// Класс для отслеживания бездействия пользователя и автоматической блокировки
    /// </summary>
    public class InactivityTracker
    {
        private System.Windows.Forms.Timer inactivityTimer; // Явно указываем Forms.Timer
        private Form targetForm;
        private int inactivityTimeoutSeconds;
        private bool enabled;

        // Событие, которое срабатывает при бездействии
        public event EventHandler InactivityDetected;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="form">Форма, на которой отслеживается активность</param>
        public InactivityTracker(Form form)
        {
            targetForm = form;

            // Загружаем настройки из конфигурации
            LoadSettings();

            // Создаем таймер (Forms.Timer)
            inactivityTimer = new System.Windows.Forms.Timer();
            inactivityTimer.Interval = inactivityTimeoutSeconds * 1000;
            inactivityTimer.Tick += InactivityTimer_Tick; // Используем Tick вместо Elapsed

            // Подписываемся на события активности
            SubscribeToActivityEvents();
        }

        /// <summary>
        /// Загрузка настроек из конфигурации
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                string timeoutSetting = ConfigurationManager.AppSettings["InactivityTimeoutSeconds"];
                if (!string.IsNullOrEmpty(timeoutSetting) && int.TryParse(timeoutSetting, out int timeout))
                {
                    inactivityTimeoutSeconds = timeout;
                }
                else
                {
                    inactivityTimeoutSeconds = 30;
                }

                string enableSetting = ConfigurationManager.AppSettings["EnableAutoLock"];
                if (!string.IsNullOrEmpty(enableSetting) && bool.TryParse(enableSetting, out bool enable))
                {
                    enabled = enable;
                }
                else
                {
                    enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки настроек: {ex.Message}");
                inactivityTimeoutSeconds = 30;
                enabled = true;
            }
        }

        /// <summary>
        /// Подписка на события активности
        /// </summary>
        private void SubscribeToActivityEvents()
        {
            if (targetForm == null) return;

            targetForm.MouseMove += ResetTimer;
            targetForm.MouseClick += ResetTimer;
            targetForm.MouseWheel += ResetTimer;
            targetForm.KeyPress += ResetTimer;
            targetForm.KeyDown += ResetTimer;

            foreach (Control control in GetAllControls(targetForm))
            {
                control.MouseMove += ResetTimer;
                control.MouseClick += ResetTimer;
                control.KeyPress += ResetTimer;
                control.KeyDown += ResetTimer;
                control.TextChanged += ResetTimer;

                if (control is ComboBox comboBox)
                    comboBox.SelectedIndexChanged += ResetTimer;

                if (control is DateTimePicker dateTimePicker)
                    dateTimePicker.ValueChanged += ResetTimer;

                if (control is NumericUpDown numericUpDown)
                    numericUpDown.ValueChanged += ResetTimer;

                if (control is CheckBox checkBox)
                    checkBox.CheckedChanged += ResetTimer;

                if (control is RadioButton radioButton)
                    radioButton.CheckedChanged += ResetTimer;
            }
        }

        /// <summary>
        /// Получение всех контролов на форме рекурсивно
        /// </summary>
        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Сброс таймера при активности
        /// </summary>
        private void ResetTimer(object sender, EventArgs e)
        {
            if (enabled && inactivityTimer != null)
            {
                inactivityTimer.Stop();
                inactivityTimer.Start();
            }
        }

        /// <summary>
        /// Обработчик срабатывания таймера
        /// </summary>
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            if (enabled && targetForm != null && !targetForm.IsDisposed)
            {
                inactivityTimer.Stop(); // Останавливаем таймер
                InactivityDetected?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Запуск отслеживания
        /// </summary>
        public void Start()
        {
            if (enabled && inactivityTimer != null)
            {
                inactivityTimer.Start();
            }
        }

        /// <summary>
        /// Остановка отслеживания
        /// </summary>
        public void Stop()
        {
            if (inactivityTimer != null)
            {
                inactivityTimer.Stop();
            }
        }

        /// <summary>
        /// Обновление таймаута
        /// </summary>
        public void UpdateTimeout(int seconds)
        {
            if (seconds > 0 && inactivityTimer != null)
            {
                inactivityTimeoutSeconds = seconds;
                inactivityTimer.Interval = seconds * 1000;
            }
        }

        /// <summary>
        /// Включение/выключение блокировки
        /// </summary>
        public void SetEnabled(bool enable)
        {
            enabled = enable;
            if (enable)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }
    }
}