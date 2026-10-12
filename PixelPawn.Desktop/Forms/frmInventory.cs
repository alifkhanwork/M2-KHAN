using PixelPawn.Desktop.Models;
using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    public class frmInventory : Form
    {
        private readonly TextBox txtSearch = Theme.Txt(90, 20, 260);
        private readonly ComboBox cboPlatform = Theme.Combo(450, 20, 160);
        private readonly CheckBox chkLowStock = new() { Text = "Low stock only", Left = 630, Top = 24, Width = 170, ForeColor = Theme.Yellow, BackColor = Color.Transparent };
        private readonly DataGridView dgvItems = Theme.Grid(20, 70, 960, 400);
        private readonly Label lblCount = new() { Left = 20, Top = 485, Width = 300, ForeColor = Theme.Yellow, BackColor = Color.Transparent };
        private List<ItemModel> _all = new();
        private bool _loading;

        public frmInventory()
        {
            Theme.StyleForm(this, "Inventory", 1000, 540);
            Controls.Add(Theme.Lbl("Search", 20, 20, 70));
            Controls.Add(Theme.Lbl("Platform", 370, 20, 80));
            var btnAdd = Theme.Btn("Add", 580, 480, 100);
            var btnEdit = Theme.Btn("Edit", 690, 480, 100);
            var btnDelete = Theme.Btn("Delete", 800, 480, 100, Color.FromArgb(150, 30, 60));
            var btnRefresh = Theme.Btn("Refresh", 900, 480, 80, Theme.Cyan);
            btnRefresh.ForeColor = Color.Black;
            Controls.AddRange(new Control[] { txtSearch, cboPlatform, chkLowStock, dgvItems, lblCount, btnAdd, btnEdit, btnDelete, btnRefresh });

            Load += async (s, e) => await LoadItems();
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            cboPlatform.SelectedIndexChanged += (s, e) => ApplyFilter();
            chkLowStock.CheckedChanged += (s, e) => ApplyFilter();
            btnRefresh.Click += async (s, e) => await LoadItems();
            dgvItems.CellFormatting += Grid_CellFormatting;
            dgvItems.DataBindingComplete += (s, e) =>
            {
                if (dgvItems.Columns["Id"] != null) dgvItems.Columns["Id"].Visible = false;
                if (dgvItems.Columns["UnitPrice"] != null) dgvItems.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            };
            dgvItems.CellDoubleClick += async (s, e) => { if (e.RowIndex >= 0) await EditSelected(); };

            btnAdd.Click += async (s, e) =>
            {
                using var f = new frmItem(null);
                if (f.ShowDialog(this) == DialogResult.OK) await LoadItems();
            };
            btnEdit.Click += async (s, e) => await EditSelected();
            btnDelete.Click += async (s, e) => await DeleteSelected();
        }

        private ItemModel? Selected => dgvItems.CurrentRow?.DataBoundItem as ItemModel;

        private async Task LoadItems()
        {
            try
            {
                _all = await ApiClient.GetItemsAsync();
                _loading = true;
                var current = cboPlatform.Text;
                cboPlatform.Items.Clear();
                cboPlatform.Items.Add("All");
                foreach (var p in _all.Select(i => i.Platform).Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().OrderBy(p => p))
                    cboPlatform.Items.Add(p);
                cboPlatform.SelectedItem = cboPlatform.Items.Contains(current) ? current : "All";
                _loading = false;
                ApplyFilter();
            }
            catch (HttpRequestException)
            { MessageBox.Show("Cannot reach the server. Is ItemAPI running?"); }
        }

        private void ApplyFilter()
        {
            if (_loading) return;
            var q = txtSearch.Text.Trim().ToLower();
            var plat = cboPlatform.Text;
            var list = _all.Where(i =>
                   (q == "" || (i.Name ?? "").ToLower().Contains(q) || (i.Code ?? "").ToLower().Contains(q)
                            || (i.Brand ?? "").ToLower().Contains(q) || (i.Platform ?? "").ToLower().Contains(q))
                && (plat == "" || plat == "All" || i.Platform == plat)
                && (!chkLowStock.Checked || i.Quantity <= i.ReorderLevel)).ToList();
            dgvItems.DataSource = new SortableBindingList<ItemModel>(list);
            lblCount.Text = list.Count == 0 ? "No items match. Insert coin to add one!" : $"{list.Count} item(s)";
        }

        private void Grid_CellFormatting(object? s, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvItems.Rows[e.RowIndex].DataBoundItem is ItemModel i && i.Quantity <= i.ReorderLevel)
                dgvItems.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Theme.Yellow;
        }

        private async Task EditSelected()
        {
            if (Selected is not ItemModel item) return;
            using var f = new frmItem(item);
            if (f.ShowDialog(this) == DialogResult.OK) await LoadItems();
        }

        private async Task DeleteSelected()
        {
            if (Selected is not ItemModel item) return;
            if (MessageBox.Show($"Delete {item.Name}?", "Confirm delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            var (ok, msg) = await ApiClient.DeleteItemAsync(item.Id);
            if (!ok) MessageBox.Show(msg, "Could not delete");
            await LoadItems();
        }
    }
}
