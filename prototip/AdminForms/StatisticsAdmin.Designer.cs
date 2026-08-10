namespace prototip
{
    partial class StatisticsAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsAdmin));
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnMenu = new System.Windows.Forms.Button();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblTotalParticipants = new System.Windows.Forms.Label();
            this.lblAvgCheck = new System.Windows.Forms.Label();
            this.lblCancelledOrders = new System.Windows.Forms.Label();
            this.lblCompletedOrders = new System.Windows.Forms.Label();
            this.lblNewOrders = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.lblTotalOrders = new System.Windows.Forms.Label();
            this.panelCharts = new System.Windows.Forms.Panel();
            this.chartDayOfWeek = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartStatus = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMonthly = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartServices = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDayOfWeek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartServices)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.lblUser);
            this.panelTop.Controls.Add(this.btnMenu);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1300, 60);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Comic Sans MS", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Статистика и отчёты";
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(195)))), ((int)(((byte)(199)))));
            this.lblUser.Location = new System.Drawing.Point(300, 20);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(880, 20);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "администратор";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnMenu
            // 
            this.btnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMenu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.btnMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMenu.Location = new System.Drawing.Point(1180, 12);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(100, 35);
            this.btnMenu.TabIndex = 2;
            this.btnMenu.Text = "Меню";
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.lblTotalParticipants);
            this.panelSummary.Controls.Add(this.lblAvgCheck);
            this.panelSummary.Controls.Add(this.lblCancelledOrders);
            this.panelSummary.Controls.Add(this.lblCompletedOrders);
            this.panelSummary.Controls.Add(this.lblNewOrders);
            this.panelSummary.Controls.Add(this.lblTotalRevenue);
            this.panelSummary.Controls.Add(this.lblTotalOrders);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 60);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1300, 40);
            this.panelSummary.TabIndex = 1;
            // 
            // lblTotalParticipants
            // 
            this.lblTotalParticipants.AutoSize = true;
            this.lblTotalParticipants.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.lblTotalParticipants.Location = new System.Drawing.Point(1100, 11);
            this.lblTotalParticipants.Name = "lblTotalParticipants";
            this.lblTotalParticipants.Size = new System.Drawing.Size(102, 19);
            this.lblTotalParticipants.TabIndex = 6;
            this.lblTotalParticipants.Text = "Участников: 0";
            // 
            // lblAvgCheck
            // 
            this.lblAvgCheck.AutoSize = true;
            this.lblAvgCheck.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.lblAvgCheck.Location = new System.Drawing.Point(900, 11);
            this.lblAvgCheck.Name = "lblAvgCheck";
            this.lblAvgCheck.Size = new System.Drawing.Size(137, 19);
            this.lblAvgCheck.TabIndex = 5;
            this.lblAvgCheck.Text = "Средний чек: 0 руб.";
            // 
            // lblCancelledOrders
            // 
            this.lblCancelledOrders.AutoSize = true;
            this.lblCancelledOrders.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.lblCancelledOrders.ForeColor = System.Drawing.Color.Black;
            this.lblCancelledOrders.Location = new System.Drawing.Point(740, 11);
            this.lblCancelledOrders.Name = "lblCancelledOrders";
            this.lblCancelledOrders.Size = new System.Drawing.Size(94, 19);
            this.lblCancelledOrders.TabIndex = 4;
            this.lblCancelledOrders.Text = "Отменено: 0";
            // 
            // lblCompletedOrders
            // 
            this.lblCompletedOrders.AutoSize = true;
            this.lblCompletedOrders.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.lblCompletedOrders.ForeColor = System.Drawing.Color.Black;
            this.lblCompletedOrders.Location = new System.Drawing.Point(580, 11);
            this.lblCompletedOrders.Name = "lblCompletedOrders";
            this.lblCompletedOrders.Size = new System.Drawing.Size(104, 19);
            this.lblCompletedOrders.TabIndex = 3;
            this.lblCompletedOrders.Text = "Выполнено: 0";
            // 
            // lblNewOrders
            // 
            this.lblNewOrders.AutoSize = true;
            this.lblNewOrders.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.lblNewOrders.ForeColor = System.Drawing.Color.Black;
            this.lblNewOrders.Location = new System.Drawing.Point(450, 11);
            this.lblNewOrders.Name = "lblNewOrders";
            this.lblNewOrders.Size = new System.Drawing.Size(71, 19);
            this.lblNewOrders.TabIndex = 2;
            this.lblNewOrders.Text = "Новых: 0";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Font = new System.Drawing.Font("Comic Sans MS", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalRevenue.ForeColor = System.Drawing.Color.Green;
            this.lblTotalRevenue.Location = new System.Drawing.Point(200, 10);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(131, 21);
            this.lblTotalRevenue.TabIndex = 1;
            this.lblTotalRevenue.Text = "Выручка: 0 руб.";
            // 
            // lblTotalOrders
            // 
            this.lblTotalOrders.AutoSize = true;
            this.lblTotalOrders.Font = new System.Drawing.Font("Comic Sans MS", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotalOrders.Location = new System.Drawing.Point(20, 10);
            this.lblTotalOrders.Name = "lblTotalOrders";
            this.lblTotalOrders.Size = new System.Drawing.Size(135, 21);
            this.lblTotalOrders.TabIndex = 0;
            this.lblTotalOrders.Text = "Всего заказов: 0";
            // 
            // panelCharts
            // 
            this.panelCharts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelCharts.Controls.Add(this.chartDayOfWeek);
            this.panelCharts.Controls.Add(this.chartStatus);
            this.panelCharts.Controls.Add(this.chartMonthly);
            this.panelCharts.Controls.Add(this.chartServices);
            this.panelCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCharts.Location = new System.Drawing.Point(0, 100);
            this.panelCharts.Name = "panelCharts";
            this.panelCharts.Size = new System.Drawing.Size(1300, 520);
            this.panelCharts.TabIndex = 2;
            // 
            // chartDayOfWeek
            // 
            this.chartDayOfWeek.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartDayOfWeek.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartDayOfWeek.Legends.Add(legend1);
            this.chartDayOfWeek.Location = new System.Drawing.Point(658, 263);
            this.chartDayOfWeek.Name = "chartDayOfWeek";
            this.chartDayOfWeek.Size = new System.Drawing.Size(630, 245);
            this.chartDayOfWeek.TabIndex = 3;
            this.chartDayOfWeek.Text = "Заказы по дням недели";
            // 
            // chartStatus
            // 
            chartArea2.Name = "ChartArea1";
            this.chartStatus.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartStatus.Legends.Add(legend2);
            this.chartStatus.Location = new System.Drawing.Point(12, 263);
            this.chartStatus.Name = "chartStatus";
            this.chartStatus.Size = new System.Drawing.Size(630, 245);
            this.chartStatus.TabIndex = 2;
            this.chartStatus.Text = "Статусы заказов";
            // 
            // chartMonthly
            // 
            this.chartMonthly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.chartMonthly.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartMonthly.Legends.Add(legend3);
            this.chartMonthly.Location = new System.Drawing.Point(658, 12);
            this.chartMonthly.Name = "chartMonthly";
            this.chartMonthly.Size = new System.Drawing.Size(630, 245);
            this.chartMonthly.TabIndex = 1;
            this.chartMonthly.Text = "Выручка по месяцам";
            // 
            // chartServices
            // 
            chartArea4.Name = "ChartArea1";
            this.chartServices.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartServices.Legends.Add(legend4);
            this.chartServices.Location = new System.Drawing.Point(12, 12);
            this.chartServices.Name = "chartServices";
            this.chartServices.Size = new System.Drawing.Size(630, 245);
            this.chartServices.TabIndex = 0;
            this.chartServices.Text = "Статистика по услугам";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelBottom.Controls.Add(this.btnExportReport);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 620);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1300, 50);
            this.panelBottom.TabIndex = 3;
            // 
            // btnExportReport
            // 
            this.btnExportReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportReport.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnExportReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportReport.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportReport.ForeColor = System.Drawing.Color.Black;
            this.btnExportReport.Location = new System.Drawing.Point(1112, 6);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(176, 35);
            this.btnExportReport.TabIndex = 1;
            this.btnExportReport.Text = "📊 Excel";
            this.btnExportReport.UseVisualStyleBackColor = false;
            this.btnExportReport.Click += new System.EventHandler(this.btnExportReport_Click);
            // 
            // StatisticsAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1300, 670);
            this.Controls.Add(this.panelCharts);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "StatisticsAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Статистика и отчёты - Администратор";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            this.panelCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDayOfWeek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartServices)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblTotalOrders;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label lblNewOrders;
        private System.Windows.Forms.Label lblCompletedOrders;
        private System.Windows.Forms.Label lblCancelledOrders;
        private System.Windows.Forms.Label lblAvgCheck;
        private System.Windows.Forms.Label lblTotalParticipants;
        private System.Windows.Forms.Panel panelCharts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartServices;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthly;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStatus;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDayOfWeek;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnExportReport;
    }
    #endregion
}
