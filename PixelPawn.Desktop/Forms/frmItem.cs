using PixelPawn.Desktop.Models;
using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    // Used for both Add (item == null) and Edit.
    public class frmItem : Form
    {
        private readonly ItemModel _item;
        private readonly TextBox txtName = Theme.Txt(180, 70, 300);
        private readonly TextBox txtCode = Theme.Txt(180, 115, 300);
        private readonly TextBox txtBrand = Theme.Txt(180, 160, 300);
        private readonly ComboBox cboPlatform = Theme.Combo(180, 205, 300, listOnly: false);
        private readonly ComboBox cboCondition = Theme.Combo(180, 250, 300);
        private readonly NumericUpDown numPrice = Theme.Num(180, 295, 150, 2);
        private readonly NumericUpDown numQty = Theme.Num(180, 340, 150);
        private readonly NumericUpDown numReorder = Theme.Num(180, 385, 150);
        private readonly Button btnSave = Theme.Btn("SAVE", 180, 440, 140);
        private readonly Button btnCancel = Theme.Btn("Cancel", 340, 440, 140, Color.FromArgb(70, 70, 100));

        public frmItem(ItemModel? item)
        {
            _item = item ?? new ItemModel();
            Theme.StyleForm(this, item == null ? "Add Item" : "Edit Item", 520, 510);
            Controls.Add(Theme.Heading(item == null ? "ADD ITEM" : "EDIT ITEM", 30, 20, 400));

            string[] labels = { "Name", "Code", "Brand", "Platform", "Condition", "Unit price", "Quantity", "Reorder level" };
            for (int i = 0; i < labels.Length; i++) Controls.Add(Theme.Lbl(labels[i], 30, 70 + i * 45, 140));

            cboPlatform.Items.AddRange(new object[] { "NES", "SNES", "N64", "Game Boy", "Genesis", "PlayStation", "PS2", "Dreamcast", "Atari" });
            cboCondition.Items.AddRange(new object[] { "Mint", "Good", "Fair" });
            numPrice.Maximum = 1000000;
            numReorder.Value = 2;
            cboCondition.SelectedIndex = 1;

            Controls.AddRange(new Control[] { txtName, txtCode, txtBrand, cboPlatform, cboCondition, numPrice, numQty, numReorder, btnSave, btnCancel });
            AcceptButton = btnSave;
            CancelButton = btnCancel;

            if (item != null)
            {
                txtName.Text = item.Name; txtCode.Text = item.Code; txtBrand.Text = item.Brand;
                cboPlatform.Text = item.Platform;
                cboCondition.SelectedItem = item.Condition;
                numPrice.Value = item.UnitPrice; numQty.Value = item.Quantity; numReorder.Value = item.ReorderLevel;
            }

            btnSave.Click += async (s, e) => await Save();
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCode.Text)
                || string.IsNullOrWhiteSpace(txtBrand.Text) || string.IsNullOrWhiteSpace(cboPlatform.Text))
            { MessageBox.Show("Name, Code, Brand, and Platform are required.", "Missing data"); return; }

            _item.Name = txtName.Text.Trim();
            _item.Code = txtCode.Text.Trim();
            _item.Brand = txtBrand.Text.Trim();
            _item.Platform = cboPlatform.Text.Trim();
            _item.Condition = cboCondition.Text;
            _item.UnitPrice = numPrice.Value;
            _item.Quantity = (int)numQty.Value;
            _item.ReorderLevel = (int)numReorder.Value;

            btnSave.Enabled = false;
            try
            {
                var (ok, msg) = await ApiClient.SaveItemAsync(_item);
                if (ok) { DialogResult = DialogResult.OK; Close(); }
                else MessageBox.Show(msg, "Could not save");   // e.g. "Item code already exists."
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
            finally { btnSave.Enabled = true; }
        }
    }
}
