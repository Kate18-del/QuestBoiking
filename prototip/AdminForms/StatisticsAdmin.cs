using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace prototip
{
    public partial class StatisticsAdmin : Form
    {
        public StatisticsAdmin()
        {
            InitializeComponent();
            DisplayCurrentUser();
            LoadStatistics();
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

        private void LoadStatistics()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Загружаем общую статистику
                    LoadSummaryStats(conn);

                    // Загружаем статистику по услугам
                    LoadServicesStats(conn);

                    // Загружаем статистику по месяцам
                    LoadMonthlyStats(conn);

                    // Загружаем статистику по статусам
                    LoadStatusStats(conn);

                    // Загружаем статистику по дням недели
                    LoadDayOfWeekStats(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статистики: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSummaryStats(MySqlConnection conn)
        {
            // Общее количество заказов
            string query = @"SELECT 
                COUNT(*) as TotalOrders,
                SUM(CASE WHEN StatusID = 1 THEN 1 ELSE 0 END) as NewOrders,
                SUM(CASE WHEN StatusID = 2 THEN 1 ELSE 0 END) as CompletedOrders,
                SUM(CASE WHEN StatusID = 3 THEN 1 ELSE 0 END) as CancelledOrders,
                COALESCE(SUM(TotalPrice), 0) as TotalRevenue,
                COALESCE(AVG(TotalPrice), 0) as AvgCheck,
                COALESCE(SUM(ParticipantsCount), 0) as TotalParticipants
                FROM orders WHERE IsActive = 1";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    lblTotalOrders.Text = $"Всего заказов: {reader["TotalOrders"]}";
                    lblNewOrders.Text = $"Новых: {reader["NewOrders"]}";
                    lblCompletedOrders.Text = $"Выполнено: {reader["CompletedOrders"]}";
                    lblCancelledOrders.Text = $"Отменено: {reader["CancelledOrders"]}";
                    lblTotalRevenue.Text = $"Выручка: {Convert.ToDecimal(reader["TotalRevenue"]):N0} руб.";
                    lblAvgCheck.Text = $"Средний чек: {Convert.ToDecimal(reader["AvgCheck"]):N0} руб.";
                    lblTotalParticipants.Text = $"Участников: {reader["TotalParticipants"]}";
                }
            }
        }

        private void LoadServicesStats(MySqlConnection conn)
        {
            string query = @"SELECT 
                s.Name as ServiceName,
                COUNT(o.ID) as OrderCount,
                COALESCE(SUM(o.TotalPrice), 0) as Revenue,
                COALESCE(AVG(o.TotalPrice), 0) as AvgPrice
                FROM services s
                LEFT JOIN orders o ON s.Article = o.ServiceID AND o.IsActive = 1
                GROUP BY s.Name
                ORDER BY Revenue DESC
                LIMIT 10";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            chartServices.Series.Clear();
            chartServices.Titles.Clear();
            chartServices.Titles.Add("ТОП-10 услуг по выручке и количеству заказов");
            chartServices.Titles[0].Font = new Font("Comic Sans MS", 12, FontStyle.Bold);

            // Очищаем серии перед добавлением
            chartServices.Series.Clear();

            // Добавляем серии для диаграммы
            Series seriesRevenue = new Series("Выручка")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(52, 152, 219)
            };

            Series seriesOrders = new Series("Количество заказов")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(46, 204, 113)
            };

            chartServices.Series.Add(seriesRevenue);
            chartServices.Series.Add(seriesOrders);

            // Настройка оси X
            chartServices.ChartAreas[0].AxisX.Interval = 1;
            chartServices.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartServices.ChartAreas[0].AxisX.Title = "Услуги";
            chartServices.ChartAreas[0].AxisY.Title = "Cумма";

            // Добавляем данные
            foreach (DataRow row in dt.Rows)
            {
                string serviceName = row["ServiceName"].ToString();
                if (serviceName.Length > 15)
                    serviceName = serviceName.Substring(0, 15) + "...";

                seriesRevenue.Points.AddXY(serviceName, Convert.ToDecimal(row["Revenue"]));
                seriesOrders.Points.AddXY(serviceName, Convert.ToInt32(row["OrderCount"]));
            }

            chartServices.Legends[0].Docking = Docking.Top;
        }

        private void LoadMonthlyStats(MySqlConnection conn)
        {
            string query = @"SELECT 
                DATE_FORMAT(StartTime, '%Y-%m') as Month,
                COUNT(*) as OrderCount,
                COALESCE(SUM(TotalPrice), 0) as Revenue
                FROM orders 
                WHERE IsActive = 1 
                AND StartTime >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH)
                GROUP BY DATE_FORMAT(StartTime, '%Y-%m')
                ORDER BY Month";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            chartMonthly.Series.Clear();
            chartMonthly.Titles.Clear();
            chartMonthly.Titles.Add("Динамика выручки по месяцам (за последний год)");
            chartMonthly.Titles[0].Font = new Font("Comic Sans MS", 12, FontStyle.Bold);
            chartMonthly.Legends[0].Enabled = false;

            chartMonthly.Series.Clear();

            Series seriesRevenue = new Series("Выручка")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(231, 76, 60),
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8
            };

            chartMonthly.Series.Add(seriesRevenue);

            chartMonthly.ChartAreas[0].AxisX.Title = "Месяц";
            chartMonthly.ChartAreas[0].AxisY.Title = "Выручка (руб.)";

            foreach (DataRow row in dt.Rows)
            {
                string month = Convert.ToDateTime(row["Month"] + "-01").ToString("MMM yyyy");
                seriesRevenue.Points.AddXY(month, Convert.ToDecimal(row["Revenue"]));
            }

            chartMonthly.Legends[0].Docking = Docking.Top;
        }

        private void LoadStatusStats(MySqlConnection conn)
        {
            string query = @"SELECT 
                s.Name as StatusName,
                COUNT(*) as Count
                FROM orders o
                JOIN statuses s ON o.StatusID = s.StatusID
                WHERE o.IsActive = 1
                GROUP BY s.Name";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            chartStatus.Series.Clear();
            chartStatus.Titles.Clear();
            chartStatus.Titles.Add("Распределение заказов по статусам");
            chartStatus.Titles[0].Font = new Font("Comic Sans MS", 12, FontStyle.Bold);
            chartStatus.Legends[0].Enabled = false;

            chartStatus.Series.Clear();

            Series series = new Series("Статусы заказов")
            {
                ChartType = SeriesChartType.Pie,
            };

            chartStatus.Series.Add(series);

            Color[] colors = { Color.FromArgb(46, 204, 113), Color.FromArgb(52, 152, 219), Color.FromArgb(231, 76, 60) };
            int colorIndex = 0;

            foreach (DataRow row in dt.Rows)
            {
                int pointIndex = series.Points.AddXY(row["StatusName"].ToString(), Convert.ToInt32(row["Count"]));
                series.Points[pointIndex].Color = colors[colorIndex % colors.Length];
                series.Points[pointIndex].Label = $"{row["StatusName"]}: {row["Count"]}";
                colorIndex++;
            }

            chartStatus.Legends[0].Docking = Docking.Bottom;
        }

        private void LoadDayOfWeekStats(MySqlConnection conn)
        {
            string query = @"SELECT 
                DAYOFWEEK(StartTime) as DayNum,
                COUNT(*) as OrderCount,
                COALESCE(SUM(TotalPrice), 0) as Revenue
                FROM orders 
                WHERE IsActive = 1
                GROUP BY DAYOFWEEK(StartTime)
                ORDER BY DayNum";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            chartDayOfWeek.Series.Clear();
            chartDayOfWeek.Titles.Clear();
            chartDayOfWeek.Titles.Add("Количество заказов по дням недели");
            chartDayOfWeek.Titles[0].Font = new Font("Comic Sans MS", 12, FontStyle.Bold);
            chartDayOfWeek.Legends[0].Enabled = false;

            chartDayOfWeek.Series.Clear();

            Series series = new Series("Заказы по дням")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(155, 89, 182)
            };

            chartDayOfWeek.Series.Add(series);

            string[] days = { "", "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };

            chartDayOfWeek.ChartAreas[0].AxisX.Title = "День недели";
            chartDayOfWeek.ChartAreas[0].AxisY.Title = "Количество заказов";

            foreach (DataRow row in dt.Rows)
            {
                int dayNum = Convert.ToInt32(row["DayNum"]);
                series.Points.AddXY(days[dayNum], Convert.ToInt32(row["OrderCount"]));
            }

            chartDayOfWeek.Legends[0].Docking = Docking.Top;
        }


        private void btnExportReport_Click(object sender, EventArgs e)
        {
            ExportToFile(false);
        }

        private void ExportToFile(bool isPdf)
        {
            try
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add();

                if (isPdf)
                {
                }
                else
                {
                    // Для Excel - полный отчёт с таблицами и диаграммами
                    Excel.Worksheet wsSummary = wb.ActiveSheet;
                    wsSummary.Name = "Общая статистика";
                    CreateSummarySheet(wsSummary);

                    Excel.Worksheet wsServices = wb.Worksheets.Add();
                    wsServices.Name = "По услугам";
                    CreateServicesSheet(wsServices);

                    Excel.Worksheet wsMonthly = wb.Worksheets.Add();
                    wsMonthly.Name = "По месяцам";
                    CreateMonthlySheet(wsMonthly);

                    Excel.Worksheet wsStatus = wb.Worksheets.Add();
                    wsStatus.Name = "По статусам";
                    CreateStatusSheet(wsStatus);

                    Excel.Worksheet wsDays = wb.Worksheets.Add();
                    wsDays.Name = "По дням недели";
                    CreateDaysSheet(wsDays);

                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string fileName = $"Статистика_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    string filePath = System.IO.Path.Combine(documentsPath, fileName);

                    wb.SaveAs(filePath);

                    wb.Close(false);
                    app.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

                    MessageBox.Show($"Excel отчёт сохранен в:\n{filePath}", "Экспорт завершен",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainAdmin().ShowDialog();
            this.Close();
        }

        private void CreateSummarySheet(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "ОБЩАЯ СТАТИСТИКА";
            ws.Range["A1:C1"].Merge();
            ws.Cells[1, 1].Font.Bold = true;
            ws.Cells[1, 1].Font.Size = 14;
            ws.Cells[2, 1] = $"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}";

            ws.Cells[4, 1] = "Показатель";
            ws.Cells[4, 2] = "Значение";
            ws.Cells[4, 1].Font.Bold = true;
            ws.Cells[4, 2].Font.Bold = true;

            ws.Cells[5, 1] = "Всего заказов";
            ws.Cells[6, 1] = "Новых";
            ws.Cells[7, 1] = "Выполнено";
            ws.Cells[8, 1] = "Отменено";
            ws.Cells[9, 1] = "Выручка";
            ws.Cells[10, 1] = "Средний чек";
            ws.Cells[11, 1] = "Участников";

            // Извлекаем значения из лейблов
            ws.Cells[5, 2] = ExtractNumber(lblTotalOrders.Text);
            ws.Cells[6, 2] = ExtractNumber(lblNewOrders.Text);
            ws.Cells[7, 2] = ExtractNumber(lblCompletedOrders.Text);
            ws.Cells[8, 2] = ExtractNumber(lblCancelledOrders.Text);
            ws.Cells[9, 2] = ExtractNumber(lblTotalRevenue.Text);
            ws.Cells[10, 2] = ExtractNumber(lblAvgCheck.Text);
            ws.Cells[11, 2] = ExtractNumber(lblTotalParticipants.Text);

            ws.Columns["A"].ColumnWidth = 25;
            ws.Columns["B"].ColumnWidth = 20;
        }

        private void CreateServicesSheet(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "СТАТИСТИКА ПО УСЛУГАМ";
            ws.Range["A1:D1"].Merge();
            ws.Cells[1, 1].Font.Bold = true;
            ws.Cells[1, 1].Font.Size = 14;

            string[] headers = { "Услуга", "Заказов", "Выручка", "Средний чек" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[3, i + 1] = headers[i];
                ws.Cells[3, i + 1].Font.Bold = true;
                ws.Cells[3, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            }

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = @"SELECT s.Name, COUNT(o.ID) as Orders, 
            COALESCE(SUM(o.TotalPrice), 0) as Revenue,
            COALESCE(AVG(o.TotalPrice), 0) as AvgPrice
            FROM services s
            LEFT JOIN orders o ON s.Article = o.ServiceID AND o.IsActive = 1
            GROUP BY s.Name ORDER BY Revenue DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int row = 4;
                    while (reader.Read())
                    {
                        ws.Cells[row, 1] = reader["Name"].ToString();
                        ws.Cells[row, 2] = Convert.ToInt32(reader["Orders"]);
                        ws.Cells[row, 3] = Convert.ToDecimal(reader["Revenue"]);
                        ws.Cells[row, 4] = Convert.ToDecimal(reader["AvgPrice"]);
                        row++;
                    }

                    if (row > 4)
                    {
                        // Создаём диаграмму
                        Excel.ChartObjects chartObjects = (Excel.ChartObjects)ws.ChartObjects();
                        Excel.ChartObject chartObj = chartObjects.Add(ws.Range["J2"].Left, ws.Range["J2"].Top, 500, 300);
                        Excel.Chart chart = chartObj.Chart;

                        chart.ChartType = Excel.XlChartType.xlColumnClustered;
                        chart.SetSourceData(ws.Range[$"A3:D{row - 1}"]);
                        chart.HasTitle = true;
                        chart.ChartTitle.Text = "ТОП услуг по выручке";
                        chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Услуги";
                        chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Сумма / Количество";
                    }
                }
            }

            ws.Columns["A"].ColumnWidth = 35;
            ws.Columns["B"].ColumnWidth = 12;
            ws.Columns["C"].ColumnWidth = 15;
            ws.Columns["D"].ColumnWidth = 15;
        }

        private void CreateMonthlySheet(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "ВЫРУЧКА ПО МЕСЯЦАМ";
            ws.Range["A1:C1"].Merge();
            ws.Cells[1, 1].Font.Bold = true;
            ws.Cells[1, 1].Font.Size = 14;

            string[] headers = { "Месяц", "Заказов", "Выручка" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[3, i + 1] = headers[i];
                ws.Cells[3, i + 1].Font.Bold = true;
                ws.Cells[3, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            }

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = @"SELECT DATE_FORMAT(StartTime, '%Y-%m') as Month,
            COUNT(*) as OrderCount, COALESCE(SUM(TotalPrice), 0) as Revenue
            FROM orders WHERE IsActive = 1 
            AND StartTime >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH)
            GROUP BY DATE_FORMAT(StartTime, '%Y-%m') ORDER BY Month";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int row = 4;
                    while (reader.Read())
                    {
                        ws.Cells[row, 1] = Convert.ToDateTime(reader["Month"] + "-01").ToString("MMMM yyyy");
                        ws.Cells[row, 2] = Convert.ToInt32(reader["OrderCount"]);
                        ws.Cells[row, 3] = Convert.ToDecimal(reader["Revenue"]);
                        row++;
                    }

                    if (row > 4)
                    {
                        Excel.ChartObjects chartObjects = (Excel.ChartObjects)ws.ChartObjects();
                        Excel.ChartObject chartObj = chartObjects.Add(ws.Range["J2"].Left, ws.Range["J2"].Top, 500, 300);
                        Excel.Chart chart = chartObj.Chart;

                        chart.ChartType = Excel.XlChartType.xlLine;
                        chart.SetSourceData(ws.Range[$"A3:C{row - 1}"]);
                        chart.HasTitle = true;
                        chart.ChartTitle.Text = "Динамика выручки по месяцам";
                        chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Месяц";
                        chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Выручка (руб.)";
                    }
                }
            }

            ws.Columns["A"].ColumnWidth = 25;
            ws.Columns["B"].ColumnWidth = 12;
            ws.Columns["C"].ColumnWidth = 15;
        }

        private void CreateStatusSheet(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "РАСПРЕДЕЛЕНИЕ ПО СТАТУСАМ";
            ws.Range["A1:B1"].Merge();
            ws.Cells[1, 1].Font.Bold = true;
            ws.Cells[1, 1].Font.Size = 14;

            string[] headers = { "Статус", "Количество" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[3, i + 1] = headers[i];
                ws.Cells[3, i + 1].Font.Bold = true;
                ws.Cells[3, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            }

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = @"SELECT s.Name as StatusName, COUNT(*) as Count
            FROM orders o JOIN statuses s ON o.StatusID = s.StatusID
            WHERE o.IsActive = 1 GROUP BY s.Name";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int row = 4;
                    while (reader.Read())
                    {
                        ws.Cells[row, 1] = reader["StatusName"].ToString();
                        ws.Cells[row, 2] = Convert.ToInt32(reader["Count"]);
                        row++;
                    }

                    if (row > 4)
                    {
                        Excel.ChartObjects chartObjects = (Excel.ChartObjects)ws.ChartObjects();
                        Excel.ChartObject chartObj = chartObjects.Add(ws.Range["J2"].Left, ws.Range["J2"].Top, 500, 300);
                        Excel.Chart chart = chartObj.Chart;

                        chart.ChartType = Excel.XlChartType.xlPie;
                        chart.SetSourceData(ws.Range[$"A3:B{row - 1}"]);
                        chart.HasTitle = true;
                        chart.ChartTitle.Text = "Статусы заказов";
                    }
                }
            }

            ws.Columns["A"].ColumnWidth = 20;
            ws.Columns["B"].ColumnWidth = 15;
        }

        private void CreateDaysSheet(Excel.Worksheet ws)
        {
            ws.Cells[1, 1] = "ЗАКАЗЫ ПО ДНЯМ НЕДЕЛИ";
            ws.Range["A1:C1"].Merge();
            ws.Cells[1, 1].Font.Bold = true;
            ws.Cells[1, 1].Font.Size = 14;

            string[] headers = { "День недели", "Заказов", "Выручка" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[3, i + 1] = headers[i];
                ws.Cells[3, i + 1].Font.Bold = true;
                ws.Cells[3, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            }

            string[] days = { "", "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                string query = @"SELECT DAYOFWEEK(StartTime) as DayNum,
            COUNT(*) as OrderCount, COALESCE(SUM(TotalPrice), 0) as Revenue
            FROM orders WHERE IsActive = 1
            GROUP BY DAYOFWEEK(StartTime) ORDER BY DayNum";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    int row = 4;
                    while (reader.Read())
                    {
                        int dayNum = Convert.ToInt32(reader["DayNum"]);
                        ws.Cells[row, 1] = days[dayNum];
                        ws.Cells[row, 2] = Convert.ToInt32(reader["OrderCount"]);
                        ws.Cells[row, 3] = Convert.ToDecimal(reader["Revenue"]);
                        row++;
                    }

                    if (row > 4)
                    {
                        Excel.ChartObjects chartObjects = (Excel.ChartObjects)ws.ChartObjects();
                        Excel.ChartObject chartObj = chartObjects.Add(ws.Range["J2"].Left, ws.Range["J2"].Top, 500, 300);
                        Excel.Chart chart = chartObj.Chart;

                        chart.ChartType = Excel.XlChartType.xlColumnClustered;
                        chart.SetSourceData(ws.Range[$"A3:C{row - 1}"]);
                        chart.HasTitle = true;
                        chart.ChartTitle.Text = "Заказы по дням недели";
                        chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "День недели";
                        chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).HasTitle = true;
                        chart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "Количество / Выручка";
                    }
                }
            }

            ws.Columns["A"].ColumnWidth = 20;
            ws.Columns["B"].ColumnWidth = 12;
            ws.Columns["C"].ColumnWidth = 15;
        }




        // Вспомогательный метод для извлечения числа из текста лейбла
        private int ExtractNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            string digits = new string(text.Where(char.IsDigit).ToArray());
            return string.IsNullOrEmpty(digits) ? 0 : int.Parse(digits);
        }   
    }
}