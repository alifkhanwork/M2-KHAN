using PixelPawn.Desktop.Models;
using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    // One form for both "Record Sale" (type = "SALE") and "Stock-In" (type = "STOCKIN").
    public class frmTransaction : Form
    {
        private readonly string _type;
        private readonly ComboBox cboItem = Theme.Combo(180, 80, 340);
        private readonly Label lblStock = new() { Left = 180, Top = 120, Width = 340, ForeColor = Theme.Yellow, BackColor = Color.Transparent };
        private readonly NumericUpDown numQty = Theme.Num(180, 155, 120);
        private readonly NumericUpDown numPrice = Theme.Num(180, 200, 150, 2, 1000000);
        private readonly TextBox txtNotes = Theme.Txt(180, 245, 340);
        private readonly Button btnConfirm;

        public frmTransaction(string type)
        {
            _type = type;
            bool sale = type == "SALE";
            Theme.StyleForm(this, sale ? "Record Sale" : "Stock-In", 560, 380);
            btnConfirm = Theme.Btn(sale ? "CONFIRM SALE" : "CONFIRM STOCK-IN", 180, 300, 200);

            Controls.Add(Theme.Heading(sale ? "RECORD SALE" : "STOCK-IN", 30, 20, 450));
            Controls.Add(Theme.Lbl("Item", 30, 80, 140));
            Controls.Add(Theme.Lbl("Quantity", 30, 155, 140));
            Controls.Add(Theme.Lbl(sale ? "Selling price" : "Unit price", 30, 200, 140));
            Controls.Add(Theme.Lbl("Notes", 30, 245, 140));
            numQty.Minimum = 1; numQty.Value = 1;
            Controls.AddRange(new Control[] { cboItem, lblStock, numQty, numPrice, txtNotes, btnConfirm });
            AcceptButton = btnConfirm;

            Load += async (s, e) => await LoadItems();
            cboItem.SelectedIndexChanged += (s, e) =>
            {
                if (cboItem.SelectedItem is ItemModel i)
                {
                    numPrice.Value = Math.Min(i.UnitPrice, numPrice.Maximum);
                    lblStock.Text = $"In stock: {i.Quantity}";
                }
            };
            btnConfirm.Click += async (s, e) => await Confirm();
        }

        private async Task LoadItems()
        {
            try
            {
                var items = await ApiClient.GetItemsAsync();
                cboItem.DataSource = items;
                cboItem.DisplayMember = "Name";
                cboItem.ValueMember = "Id";
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
        }

        private async Task Confirm()
        {
            if (cboItem.SelectedItem is not ItemModel i) { MessageBox.Show("Select an item."); return; }
            var t = new TransactionModel
            {
                ItemId = i.Id, Type = _type, Quantity = (int)numQty.Value,
                UnitPrice = numPrice.Value, Notes = txtNotes.Text.Trim()
            };
            btnConfirm.Enabled = false;
            try
            {
                var (ok, msg) = await ApiClient.AddTransactionAsync(t);
                if (ok)
                {
                    MessageBox.Show(_type == "SALE" ? "Sale recorded." : "Stock added.", "PixelPawn");
                    DialogResult = DialogResult.OK; Close();
                }
                else MessageBox.Show(msg, "Transaction failed");   // e.g. "Insufficient stock."
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
            finally { btnConfirm.Enabled = true; }
        }
    }
}
