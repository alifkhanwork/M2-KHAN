using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    public class frmRegister : Form
    {
        private readonly TextBox txtUsername = Theme.Txt(170, 100, 250);
        private readonly TextBox txtFirst = Theme.Txt(170, 145, 250);
        private readonly TextBox txtLast = Theme.Txt(170, 190, 250);
        private readonly TextBox txtPassword = Theme.Txt(170, 235, 250);
        private readonly TextBox txtConfirm = Theme.Txt(170, 280, 250);
        private readonly Button btnRegister = Theme.Btn("REGISTER", 170, 335, 250);
        private readonly Label lblError = new() { Left = 30, Top = 385, Width = 390, Height = 40, ForeColor = Theme.Pink, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter };

        public frmRegister()
        {
            Theme.StyleForm(this, "New Player", 450, 440);
            txtPassword.UseSystemPasswordChar = true;
            txtConfirm.UseSystemPasswordChar = true;
            Controls.Add(Theme.Heading("NEW PLAYER", 30, 30, 390));
            Controls.Add(Theme.Lbl("Username", 30, 100, 130));
            Controls.Add(Theme.Lbl("First name", 30, 145, 130));
            Controls.Add(Theme.Lbl("Last name", 30, 190, 130));
            Controls.Add(Theme.Lbl("Password", 30, 235, 130));
            Controls.Add(Theme.Lbl("Confirm", 30, 280, 130));
            Controls.AddRange(new Control[] { txtUsername, txtFirst, txtLast, txtPassword, txtConfirm, btnRegister, lblError });
            AcceptButton = btnRegister;
            btnRegister.Click += async (s, e) => await DoRegister();
        }

        private async Task DoRegister()
        {
            lblError.ForeColor = Theme.Pink;
            if (new[] { txtUsername.Text, txtFirst.Text, txtLast.Text, txtPassword.Text }.Any(string.IsNullOrWhiteSpace))
            { lblError.Text = "All fields are required."; return; }
            if (txtPassword.Text != txtConfirm.Text)
            { lblError.Text = "Passwords do not match."; return; }

            btnRegister.Enabled = false;
            try
            {
                var (ok, msg) = await ApiClient.RegisterAsync(txtUsername.Text.Trim(), txtFirst.Text.Trim(), txtLast.Text.Trim(), txtPassword.Text);
                if (ok)
                {
                    MessageBox.Show("Account created. You can log in now.", "PixelPawn");
                    Close();
                }
                else lblError.Text = string.IsNullOrWhiteSpace(msg) ? "Registration failed." : msg;
            }
            catch (HttpRequestException)
            { lblError.Text = "Cannot reach the server. Is ItemAPI running?"; }
            finally { btnRegister.Enabled = true; }
        }
    }
}
