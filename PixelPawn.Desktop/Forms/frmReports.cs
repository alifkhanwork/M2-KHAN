using System.Text;
using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    public class frmReports : Form
    {
        private readonly ComboBox cboReport = Theme.Combo(100, 20, 200);
        private readonly DateTimePicker dtpFrom = new() { Left = 380, Top = 22, Width = 130, Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker dtpTo = new() { Left = 570, Top = 22, Width = 130, Format = DateTimePickerFormat.Short };
        private readonly DataGridView dgvReport = Theme.Grid(20, 70, 960, 390);
        private readonly Label lblTotal = new() { Left = 20, Top = 475, Width = 600, ForeColor = Theme.Yellow, BackColor = Color.Transparent };

        public frmReports()
        {
            Theme.StyleForm(this, "Reports", 1000, 530);
            cboReport.Items.AddRange(new object[] { "Stock Valuation", "Low Stock", "Sales" });
            cboReport.SelectedIndex = 0;
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            var btnRun = Theme.Btn("Run", 720, 18, 100);
            var btnExport = Theme.Btn("Export CSV", 830, 18, 150, Theme.Cyan);
            btnExport.ForeColor = Color.Black;
            Controls.Add(Theme.Lbl("Report", 20, 20, 80));
            Controls.Add(Theme.Lbl("From", 320, 22, 60));
            Controls.Add(Theme.Lbl("To", 530, 22, 40));
            Controls.AddRange(new Control[] { cboReport, dtpFrom, dtpTo, btnRun, btnExport, dgvReport, lblTotal });

            btnRun.Click += async (s, e) => await RunReport();
            btnExport.Click += (s, e) => ExportCsv();
            Load += async (s, e) => await RunReport();
        }

        private async Task RunReport()
        {
            try
            {
                lblTotal.Text = "";
                switch (cboReport.Text)
                {
                    case "Low Stock":
                        var low = await ApiClient.GetLowStockAsync();
                        dgvReport.DataSource = low.Select(i => new { i.Code, i.Name, i.Platform, i.Quantity, i.ReorderLevel }).ToList();
                        lblTotal.Text = $"{low.Count} item(s) at or below reorder level";
                        break;

                    case "Stock Valuation":
                        var items = await ApiClient.GetItemsAsync();
                        dgvReport.DataSource = items.Select(i => new
                        { i.Code, i.Name, i.Quantity, i.UnitPrice, Value = i.Quantity * i.UnitPrice }).ToList();
                        lblTotal.Text = $"Total inventory value: {items.Sum(i => i.Quantity * i.UnitPrice):N2}";
                        break;

                    case "Sales":
                        var tx = await ApiClient.GetTransactionsAsync(dtpFrom.Value.Date, dtpTo.Value.Date);
                        var sales = tx.Where(t => t.Type == "SALE").ToList();
                        dgvReport.DataSource = sales.Select(t => new
                        { Date = t.CreatedAt, Item = t.ItemName, t.Quantity, t.UnitPrice, Total = t.Quantity * t.UnitPrice }).ToList();
                        lblTotal.Text = $"{sales.Count} sale(s), total {sales.Sum(t => t.Quantity * t.UnitPrice):N2}";
                        break;
                }
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
        }

        private void ExportCsv()
        {
            if (dgvReport.Rows.Count == 0) { MessageBox.Show("Run a report first."); return; }
            using var dlg = new SaveFileDialog { Filter = "CSV file|*.csv", FileName = cboReport.Text.Replace(" ", "") + ".csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            static string Q(string? v) => "\"" + (v ?? "").Replace("\"", "\"\"") + "\"";
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", dgvReport.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => Q(c.HeaderText))));
            foreach (DataGridViewRow r in dgvReport.Rows)
                sb.AppendLine(string.Join(",", r.Cells.Cast<DataGridViewCell>().Where(c => c.OwningColumn.Visible).Select(c => Q(c.Value?.ToString()))));
            File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
            MessageBox.Show("Report exported.", "PixelPawn");
        }
    }
}
