using PixelPawn.Desktop.Models;
using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    public class frmHistory : Form
    {
        private readonly DateTimePicker dtpFrom = new() { Left = 90, Top = 22, Width = 150, Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker dtpTo = new() { Left = 310, Top = 22, Width = 150, Format = DateTimePickerFormat.Short };
        private readonly DataGridView dgvTransactions = Theme.Grid(20, 70, 960, 400);
        private readonly Label lblCount = new() { Left = 20, Top = 485, Width = 400, ForeColor = Theme.Yellow, BackColor = Color.Transparent };

        public frmHistory()
        {
            Theme.StyleForm(this, "Transaction History", 1000, 530);
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            var btnFilter = Theme.Btn("Filter", 490, 18, 120);
            var btnAll = Theme.Btn("Show all", 620, 18, 120, Theme.Cyan);
            btnAll.ForeColor = Color.Black;
            Controls.Add(Theme.Lbl("From", 20, 22, 70));
            Controls.Add(Theme.Lbl("To", 260, 22, 50));
            Controls.AddRange(new Control[] { dtpFrom, dtpTo, btnFilter, btnAll, dgvTransactions, lblCount });

            Load += async (s, e) => await LoadData(null, null);
            btnFilter.Click += async (s, e) => await LoadData(dtpFrom.Value.Date, dtpTo.Value.Date);
            btnAll.Click += async (s, e) => await LoadData(null, null);
            dgvTransactions.DataBindingComplete += (s, e) =>
            {
                foreach (var name in new[] { "Id", "ItemId" })
                    if (dgvTransactions.Columns[name] != null) dgvTransactions.Columns[name].Visible = false;
                if (dgvTransactions.Columns["CreatedAt"] != null) dgvTransactions.Columns["CreatedAt"].DefaultCellStyle.Format = "g";
                if (dgvTransactions.Columns["UnitPrice"] != null) dgvTransactions.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            };
        }

        private async Task LoadData(DateTime? from, DateTime? to)
        {
            try
            {
                var list = await ApiClient.GetTransactionsAsync(from, to);
                dgvTransactions.DataSource = new SortableBindingList<TransactionModel>(list);
                lblCount.Text = $"{list.Count} transaction(s)";
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
        }
    }
}
