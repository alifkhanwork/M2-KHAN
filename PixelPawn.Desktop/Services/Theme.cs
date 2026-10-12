namespace PixelPawn.Desktop.Services
{
    // Arcade theme + small helpers so every form is built the same way.
    public static class Theme
    {
        public static readonly Color Bg     = Color.FromArgb(0x0B, 0x0B, 0x1A);
        public static readonly Color Panel  = Color.FromArgb(0x16, 0x16, 0x30);
        public static readonly Color Pink   = Color.FromArgb(0xFF, 0x2E, 0x93);
        public static readonly Color Cyan   = Color.FromArgb(0x00, 0xE5, 0xFF);
        public static readonly Color Yellow = Color.FromArgb(0xFF, 0xD6, 0x0A);

        // Consolas is on every Windows PC. Change to "VT323" / "Press Start 2P" if installed.
        public const string FontName = "Consolas";
        public static Font Body  => new(FontName, 11f);
        public static Font Title => new(FontName, 18f, FontStyle.Bold);

        public static void StyleForm(Form f, string title, int w, int h)
        {
            f.Text = "PixelPawn - " + title;
            f.ClientSize = new Size(w, h);
            f.BackColor = Bg;
            f.ForeColor = Cyan;
            f.Font = Body;
            f.StartPosition = FormStartPosition.CenterScreen;
            f.FormBorderStyle = FormBorderStyle.FixedSingle;
            f.MaximizeBox = false;
        }

        public static Label Heading(string text, int x, int y, int w = 400)
            => new() { Text = text, Left = x, Top = y, Width = w, Height = 34, Font = Title, ForeColor = Yellow, BackColor = Color.Transparent };

        public static Label Lbl(string text, int x, int y, int w = 140, Color? color = null)
            => new() { Text = text, Left = x, Top = y + 4, Width = w, Height = 24, ForeColor = color ?? Cyan, BackColor = Color.Transparent };

        public static TextBox Txt(int x, int y, int w = 240)
            => new() { Left = x, Top = y, Width = w, BackColor = Panel, ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Font = Body };

        public static NumericUpDown Num(int x, int y, int w = 120, int decimals = 0, decimal max = 100000)
            => new() { Left = x, Top = y, Width = w, DecimalPlaces = decimals, Maximum = max, Minimum = 0, BackColor = Panel, ForeColor = Color.White, Font = Body };

        public static ComboBox Combo(int x, int y, int w = 240, bool listOnly = true)
            => new() { Left = x, Top = y, Width = w, BackColor = Panel, ForeColor = Color.White, Font = Body,
                       FlatStyle = FlatStyle.Flat, DropDownStyle = listOnly ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown };

        public static Button Btn(string text, int x, int y, int w = 140, Color? back = null)
        {
            var b = new Button { Text = text, Left = x, Top = y, Width = w, Height = 36, FlatStyle = FlatStyle.Flat,
                                 BackColor = back ?? Pink, ForeColor = Color.White, Font = Body, Cursor = Cursors.Hand };
            b.FlatAppearance.BorderColor = Yellow;
            b.FlatAppearance.BorderSize = 1;
            return b;
        }

        public static DataGridView Grid(int x, int y, int w, int h)
        {
            var g = new DataGridView
            {
                Left = x, Top = y, Width = w, Height = h,
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false, BorderStyle = BorderStyle.None,
                BackgroundColor = Bg, GridColor = Color.FromArgb(60, 40, 90),
                EnableHeadersVisualStyles = false, Font = Body,
                ColumnHeadersHeight = 34,
            };
            g.ColumnHeadersDefaultCellStyle.BackColor = Cyan;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            g.ColumnHeadersDefaultCellStyle.Font = new Font(FontName, 11f, FontStyle.Bold);
            g.DefaultCellStyle.BackColor = Bg;
            g.DefaultCellStyle.ForeColor = Cyan;
            g.DefaultCellStyle.SelectionBackColor = Pink;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.AlternatingRowsDefaultCellStyle.BackColor = Panel;
            return g;
        }
    }
}
