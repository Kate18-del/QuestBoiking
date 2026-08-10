using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace prototip
{
    public partial class OrderAccountingAdmin : Form
    {
        private BindingList<Order> allOrders;
        private BindingList<Order> filteredOrders;
        private int currentPage = 1;
        private int pageSize = 20;

        public OrderAccountingAdmin()
        {
            InitializeComponent();
            DisplayCurrentUser();
            ConfigureDataGridView();
            LoadOrders();
        }

        private void DisplayCurrentUser()
        {
            if (CurrentUser.FIO != null)
            {
                string[] parts = CurrentUser.FIO.Split(' ');
                if (parts.Length >= 3)
                    lblUser.Text = $"Администратор {parts[0]} {parts[1][0]}.{parts[2][0]}.";
            }
        }

        private void ConfigureDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();

            // Устанавливаем шрифт Comic Sans MS, размер 14
            Font comicSans14 = new Font("Comic Sans MS", 14F);
            Font comicSans14Bold = new Font("Comic Sans MS", 12F, FontStyle.Bold);

            dataGridView.DefaultCellStyle.Font = comicSans14;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = comicSans14Bold;

            // Увеличиваем высоту строк для шрифта 14
            dataGridView.RowTemplate.Height = 35;
            dataGridView.ColumnHeadersHeight = 40;

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ID", HeaderText = "№", DataPropertyName = "ID", Width = 70 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ClientName", HeaderText = "Клиент", DataPropertyName = "DisplayClientName", Width = 180 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ClientPhone", HeaderText = "Телефон", DataPropertyName = "DisplayPhone", Width = 150 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ServiceName", HeaderText = "Квест", DataPropertyName = "ServiceName", Width = 200 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StartTime", HeaderText = "Дата и время", DataPropertyName = "StartTime", Width = 170, DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd.MM.yyyy HH:mm" } });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "StatusName", HeaderText = "Статус", DataPropertyName = "StatusName", Width = 110 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ParticipantsCount", HeaderText = "Чел.", DataPropertyName = "ParticipantsCount", Width = 70 });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "TotalPrice", HeaderText = "Сумма", DataPropertyName = "TotalPrice", Width = 130, DefaultCellStyle = new DataGridViewCellStyle() { Format = "0.##' руб.'" } });

            dataGridView.CellFormatting += (s, e) =>
            {
                if (e.Value != null && dataGridView.Columns[e.ColumnIndex].Name == "StatusName")
                {
                    string st = e.Value.ToString();
                    if (st == "Новый") e.CellStyle.BackColor = Color.FromArgb(144, 238, 144);
                    else if (st == "Выполнен") e.CellStyle.BackColor = Color.FromArgb(173, 216, 230);
                    else if (st == "Отменен") e.CellStyle.BackColor = Color.LightGray;
                }
            };
        }

        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    string query = @"SELECT o.ID, o.ClientName, o.ClientPhone, o.ServiceName, o.StartTime, 
                    s.Name as StatusName, o.ParticipantsCount, o.TotalPrice
                    FROM orders o LEFT JOIN statuses s ON o.StatusID = s.StatusID
                    ORDER BY o.StartTime DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);


                    allOrders = new BindingList<Order>();
                    foreach (DataRow row in dt.Rows)
                    {
                        allOrders.Add(new Order
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            ClientName = MaskName(row["ClientName"].ToString()),
                            ClientPhone = MaskPhone(row["ClientPhone"].ToString()),
                            ServiceName = row["ServiceName"].ToString(),
                            StartTime = Convert.ToDateTime(row["StartTime"]),
                            StatusName = row["StatusName"].ToString(),
                            ParticipantsCount = Convert.ToInt32(row["ParticipantsCount"]),
                            TotalPrice = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0
                        });
                    }
                }
            
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Ошибка");
            }
        }

        private void ApplyFilters()
        {
            if (allOrders == null) return;

            var filtered = allOrders.AsEnumerable();

            // Поиск
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "Поиск по номеру заказа")
                filtered = filtered.Where(o => o.ID.ToString().StartsWith(txtSearch.Text.Trim()));

            DateTime start = dtpStartDate.Value.Date;
            DateTime end = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);
            filtered = filtered.Where(o => o.StartTime >= start && o.StartTime <= end);

            if (cmbStatus.SelectedIndex > 0)
                filtered = filtered.Where(o => o.StatusName == cmbStatus.SelectedItem.ToString());

            var filteredList = new BindingList<Order>(filtered.ToList());
            filteredOrders = filteredList;  // ДОБАВИТЬ ЭТУ СТРОКУ

            // Обновляем статистику
            int totalRecords = filteredList.Count;
            decimal totalRevenue = filteredList.Where(o => o.TotalPrice > 0).Sum(o => o.TotalPrice);

            // Пагинация
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            var paged = filteredList.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            dataGridView.DataSource = new BindingList<Order>(paged);

            // Отображаем количество записей на текущей странице из общего
            int recordsOnPage = paged.Count;
            lblRecordCount.Text = $"Записей: {recordsOnPage} из {totalRecords}";
            lblTotalRevenue.Text = $"Выручка: {totalRevenue:N0} руб.";

            lblPageInfo.Text = $"Страница {currentPage} из {totalPages}";

            UpdatePageSelector(totalPages);  // Обновляем выпадающий список страниц

            // Обновляем навигацию
            btnFirstPage.Enabled = currentPage > 1;
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            btnLastPage.Enabled = currentPage < totalPages;
        }

        private void UpdatePageSelector(int totalPages)
        {
            if (cmbPageSelector == null) return;

            cmbPageSelector.Items.Clear();
            for (int i = 1; i <= totalPages; i++)
            {
                cmbPageSelector.Items.Add(i.ToString());
            }

            if (cmbPageSelector.Items.Count > 0)
            {
                cmbPageSelector.SelectedIndex = currentPage - 1;
            }
        }

        // Обработчики поля поиска 
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "Поиск по номеру заказа")
                ApplyFilters();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Поиск по номеру заказа")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Поиск по номеру заказа";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие символы
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.MinDate = dtpStartDate.Value;
            if (dtpEndDate.Value < dtpStartDate.Value) dtpEndDate.Value = dtpStartDate.Value;
            ApplyFilters();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e) => ApplyFilters();

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Поиск по номеру заказа";
            txtSearch.ForeColor = Color.Gray;
            cmbStatus.SelectedIndex = 0;
            dtpStartDate.Value = DateTime.Today.AddMonths(-1);
            dtpEndDate.Value = DateTime.Today;
            currentPage = 1;
            ApplyFilters();
        }

        // Навигация
        private void BtnFirstPage_Click(object sender, EventArgs e) { currentPage = 1; ApplyFilters(); }
        private void BtnPrevPage_Click(object sender, EventArgs e) { if (currentPage > 1) currentPage--; ApplyFilters(); }
        private void BtnNextPage_Click(object sender, EventArgs e) { if (currentPage < GetTotalPages()) currentPage++; ApplyFilters(); }
        private void BtnLastPage_Click(object sender, EventArgs e) { currentPage = GetTotalPages(); ApplyFilters(); }

        private int GetTotalPages()
        {
            if (filteredOrders == null) return 0;
            int count = filteredOrders.Count;
            return count == 0 ? 1 : (int)Math.Ceiling((double)count / pageSize);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainAdmin().ShowDialog();
            this.Close();
        }

        // Экспорт в Excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта!");
                return;
            }

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = $"Отчет_заказы_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string filePath = System.IO.Path.Combine(documentsPath, fileName);

            ExportToFile(filePath, false);
        }

        // Экспорт в PDF
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта!");
                return;
            }

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = $"Отчет_заказы_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = System.IO.Path.Combine(documentsPath, fileName);

            ExportToFile(filePath, true);
        }

        private void ExportToFile(string filePath, bool isPdf)
        {
            Excel.Application app = new Excel.Application();
            Excel.Workbook wb = app.Workbooks.Add();
            Excel.Worksheet ws = wb.ActiveSheet;

            // Рассчитываем выручку из отображаемых данных
            decimal totalRevenue = 0;
            int recordCount = 0;
            foreach (DataGridViewRow dgvRow in dataGridView.Rows)
            {
                if (!dgvRow.IsNewRow)
                {
                    recordCount++;
                    if (dgvRow.Cells["TotalPrice"].Value != null)
                        totalRevenue += Convert.ToDecimal(dgvRow.Cells["TotalPrice"].Value);
                }
            }

            // Заголовок
            ws.Cells[1, 1] = "Отчет по заказам";
            ws.Range["A1:H1"].Merge();
            ws.Cells[1, 1].Font.Bold = true;
            ws.Cells[1, 1].Font.Size = 14;

            // Информация о выборке
            ws.Cells[2, 1] = $"Период: {dtpStartDate.Value:dd.MM.yyyy} - {dtpEndDate.Value:dd.MM.yyyy}";
            ws.Cells[3, 1] = $"Выручка: {totalRevenue:N0} руб. | Записей: {recordCount}";

            // Заголовки таблицы
            string[] headers = { "№", "Клиент", "Телефон", "Квест", "Дата и время", "Статус", "Чел.", "Сумма" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[5, i + 1] = headers[i];
                ws.Cells[5, i + 1].Font.Bold = true;
                ws.Cells[5, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            }

            // Данные из DataGridView
            int row = 6;
            foreach (DataGridViewRow dgvRow in dataGridView.Rows)
            {
                if (!dgvRow.IsNewRow)
                {
                    ws.Cells[row, 1] = dgvRow.Cells["ID"].Value;
                    ws.Cells[row, 2] = dgvRow.Cells["ClientName"].Value;
                    ws.Cells[row, 3] = dgvRow.Cells["ClientPhone"].Value;
                    ws.Cells[row, 4] = dgvRow.Cells["ServiceName"].Value;
                    ws.Cells[row, 5] = dgvRow.Cells["StartTime"].Value;
                    ws.Cells[row, 6] = dgvRow.Cells["StatusName"].Value;
                    ws.Cells[row, 7] = dgvRow.Cells["ParticipantsCount"].Value;
                    ws.Cells[row, 8] = dgvRow.Cells["TotalPrice"].Value;
                    row++;
                }
            }

            // Настройка ширины столбцов
            ws.Columns[1].ColumnWidth = 8;   // №
            ws.Columns[2].ColumnWidth = 20;  // Клиент
            ws.Columns[3].ColumnWidth = 16;  // Телефон
            ws.Columns[4].ColumnWidth = 25;  // Квест
            ws.Columns[5].ColumnWidth = 18;  // Дата и время
            ws.Columns[6].ColumnWidth = 12;  // Статус
            ws.Columns[7].ColumnWidth = 7;   // Чел.
            ws.Columns[8].ColumnWidth = 13;  // Сумма

            // Настройка страницы для PDF
            if (isPdf)
            {
                ws.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                ws.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                ws.PageSetup.FitToPagesWide = 1;
                ws.PageSetup.FitToPagesTall = 1;
                ws.PageSetup.Zoom = false;
                ws.PageSetup.LeftMargin = app.InchesToPoints(0.5);
                ws.PageSetup.RightMargin = app.InchesToPoints(0.5);
                ws.PageSetup.TopMargin = app.InchesToPoints(0.5);
                ws.PageSetup.BottomMargin = app.InchesToPoints(0.5);
            }
            else
            {
                ws.Columns.AutoFit();
            }

            if (isPdf)
                ws.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, filePath);
            else
                wb.SaveAs(filePath);

            wb.Close(false);
            app.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

            MessageBox.Show($"Отчет сохранен в:\n{filePath}", "Экспорт завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(filePath);
        }

        // Маскирование данных
        private string MaskName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            string[] parts = name.Split(' ');
            string result = parts[0];
            if (parts.Length >= 2) result += " " + parts[1][0] + ".";
            if (parts.Length >= 3) result += " " + parts[2][0] + ".";
            return result;
        }

        private string MaskPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone) || phone.Length < 7) return phone;
            string digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length >= 11) return "+7 *** **-" + digits.Substring(digits.Length - 4);
            return phone;
        }

        private void cmbPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPageSelector.SelectedItem != null)
                {
                    int selectedPage = int.Parse(cmbPageSelector.SelectedItem.ToString());
                    if (selectedPage != currentPage)
                    {
                        currentPage = selectedPage;
                        ApplyFilters();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора страницы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}