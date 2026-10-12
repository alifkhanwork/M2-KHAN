using System.Globalization;
using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    public class frmMain : Form
    {
        private static readonly CultureInfo Ph = new("en-PH");
        private readonly Label lblItems, lblUnits, lblValue, lblLow, lblSales;
        private readonly Label lblStatus = new() { Dock = DockStyle.Bottom, Height = 28, ForeColor = Theme.Yellow, BackColor = Theme.Panel, TextAlign = ContentAlignment.MiddleLeft };

        public frmMain()
        {
            Theme.StyleForm(this, "Retro Game Exchange", 960, 560);

            // ----- left navigation panel -----
            var nav = new Panel { Left = 0, Top = 0, Width = 230, Height = 532, BackColor = Theme.Panel };
            nav.Controls.Add(Theme.Heading("PIXELPAWN", 15, 20, 210));
            string[] names = { "Inventory", "Record Sale", "Stock-In", "History", "Reports", "Logout" };
            for (int i = 0; i < names.Length; i++)
            {
                var b = Theme.Btn(names[i], 15, 90 + i * 60, 200, i == names.Length - 1 ? Color.FromArgb(90, 40, 90) : null);
                int idx = i;
                b.Click += async (s, e) => await Navigate(idx);
                nav.Controls.Add(b);
            }
            Controls.Add(nav);

            // ----- dashboard cards -----
            Controls.Add(Theme.Heading("DASHBOARD", 260, 20, 400));
            lblItems = Card("Total items", 260, 80);
            lblUnits = Card("Units in stock", 490, 80);
            lblValue = Card("Inventory value", 720, 80);
            lblLow   = Card("Low stock", 260, 240);
            lblSales = Card("Sales today", 490, 240);

            var btnRefresh = Theme.Btn("Refresh", 720, 270, 200, Theme.Cyan);
            btnRefresh.ForeColor = Color.Black;
            btnRefresh.Click += async (s, e) => await LoadSummary();
            Controls.Add(btnRefresh);

            Controls.Add(lblStatus);
            lblStatus.Text = "  Player: " + ApiClient.UserName;
            Load += async (s, e) => await LoadSummary();
        }

        private Label Card(string title, int x, int y)
        {
            var p = new Panel { Left = x, Top = y, Width = 210, Height = 130, BackColor = Theme.Panel, BorderStyle = BorderStyle.FixedSingle };
            p.Controls.Add(new Label { Text = title, Left = 10, Top = 10, Width = 190, ForeColor = Theme.Cyan });
            var v = new Label { Text = "-", Left = 10, Top = 50, Width = 190, Height = 50, ForeColor = Theme.Yellow, Font = new Font(Theme.FontName, 18f, FontStyle.Bold) };
            p.Controls.Add(v);
            Controls.Add(p);
            return v;
        }

        private async Task LoadSummary()
        {
            try
            {
                var m = await ApiClient.GetSummaryAsync();
                lblItems.Text = m.TotalItems.ToString();
                lblUnits.Text = m.TotalUnits.ToString();
                lblValue.Text = m.InventoryValue.ToString("C", Ph);
                lblLow.Text   = m.LowStockCount.ToString();
                lblLow.ForeColor = m.LowStockCount > 0 ? Theme.Pink : Theme.Yellow;
                lblSales.Text = m.SalesToday.ToString("C", Ph);
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
        }

        private async Task Navigate(int idx)
        {
            switch (idx)
            {
                case 0: using (var f = new frmInventory()) f.ShowDialog(this); break;
                case 1: using (var f = new frmTransaction("SALE")) f.ShowDialog(this); break;
                case 2: using (var f = new frmTransaction("STOCKIN")) f.ShowDialog(this); break;
                case 3: using (var f = new frmHistory()) f.ShowDialog(this); break;
                case 4: using (var f = new frmReports()) f.ShowDialog(this); break;
                case 5:
                    ApiClient.Logout();
                    Application.Restart();
                    return;
            }
            await LoadSummary();   // refresh the cards after any change
        }
    }
}
