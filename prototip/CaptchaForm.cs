using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prototip
{
    public partial class CaptchaForm : Form
    {
        private string captchaCode;
        private Bitmap captchaImage;
        private Timer blockTimer;
        private int failedAttempts = 0;
        private bool isBlocked = false;
        private int blockTimeSeconds = 10;

       

        public string EnteredCode { get; private set; }
        public CaptchaForm()
        {
            InitializeComponent();
            GenerateNewCaptcha();
            this.btnRefresh.Click += BtnRefresh_Click;
            this.txtCaptcha.TextChanged += (s, e) => EnteredCode = txtCaptcha.Text;
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (!isBlocked)
            {
                GenerateNewCaptcha();
                txtCaptcha.Clear();
            }
        }

        private void GenerateNewCaptcha()
        {
            if (isBlocked) return;

            // Простой код из 4 символов
            Random rnd = new Random();
            string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
            captchaCode = "";
            for (int i = 0; i < 4; i++)
            {
                captchaCode += chars[rnd.Next(chars.Length)];
            }

            // Простое изображение
            captchaImage = new Bitmap(350, 120);
            using (Graphics g = Graphics.FromImage(captchaImage))
            {
                g.Clear(Color.White);

                // Рисуем символы в линию
                for (int i = 0; i < captchaCode.Length; i++)
                {
                    using (Font font = new Font("Arial", 30, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(captchaCode[i].ToString(), font, brush, 50 + i * 70, 40);
                    }
                }

                // Немного линий для шума
                for (int i = 0; i < 5; i++)
                {
                    using (Pen pen = new Pen(Color.Gray, 1))
                    {
                        g.DrawLine(pen, rnd.Next(350), rnd.Next(120), rnd.Next(350), rnd.Next(120));
                    }
                }
            }

            if (pbCaptcha.Image != null)
                pbCaptcha.Image.Dispose();
            pbCaptcha.Image = captchaImage;
        }

        public bool ValidateCaptcha(string input)
        {
            if (isBlocked)
            {
                MessageBox.Show($"Подождите {blockTimeSeconds} секунд!", "Блокировка");
                return false;
            }

            if (string.IsNullOrEmpty(input) || input.Length != 4)
            {
                lblError.Text = "Введите 4 символа!";
                lblError.Visible = true;
                return false;
            }

            if (input.ToUpper() == captchaCode.ToUpper())
            {
                failedAttempts = 0;
                lblError.Visible = false;
                return true;
            }
            else
            {
                failedAttempts++;
                lblError.Text = "Неверный код!";
                lblError.Visible = true;

                if (failedAttempts >= 1)
                {
                    isBlocked = true;
                    int timeLeft = blockTimeSeconds;

                    btnRefresh.Enabled = false;
                    txtCaptcha.Enabled = false;
                    btnOk.Enabled = false;

                    blockTimer = new Timer();
                    blockTimer.Interval = 1000;
                    blockTimer.Tick += (s, e) =>
                    {
                        timeLeft--;
                        lblTimer.Text = $"Блокировка: {timeLeft} сек";

                        if (timeLeft <= 0)
                        {
                            blockTimer.Stop();
                            isBlocked = false;
                            btnRefresh.Enabled = true;
                            txtCaptcha.Enabled = true;
                            btnOk.Enabled = true;
                            lblTimer.Text = "";
                            GenerateNewCaptcha();
                        }
                    };
                    blockTimer.Start();
                }

                GenerateNewCaptcha();
                txtCaptcha.Clear();
                return false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            blockTimer?.Stop();
            pbCaptcha.Image?.Dispose();
            base.OnFormClosing(e);
        }
 

}
}
