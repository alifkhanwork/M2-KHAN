using PixelPawn.Desktop.Services;

namespace PixelPawn.Desktop.Forms
{
    public class frmLogin : Form
    {
        private readonly TextBox txtUsername = Theme.Txt(150, 150, 260);
        private readonly TextBox txtPassword = Theme.Txt(150, 195, 260);
        private readonly Button btnLogin = Theme.Btn("START", 150, 245, 260);
        private readonly LinkLabel linkRegister = new() { Text = "New Player? Register here", Left = 150, Top = 295, Width = 260, LinkColor = Theme.Yellow, ActiveLinkColor = Theme.Pink, BackColor = Color.Transparent };
        private readonly Label lblError = new() { Left = 30, Top = 330, Width = 380, Height = 40, ForeColor = Theme.Pink, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter };

        public frmLogin()
        {
            Theme.StyleForm(this, "Player Login", 440, 390);
            txtPassword.UseSystemPasswordChar = true;
            Controls.Add(Theme.Heading("PIXELPAWN", 30, 30, 380));
            Controls.Add(Theme.Lbl("Retro Game Exchange", 30, 75, 380, Theme.Pink));
            Controls.Add(Theme.Lbl("Username", 30, 150, 110));
            Controls.Add(Theme.Lbl("Password", 30, 195, 110));
            Controls.AddRange(new Control[] { txtUsername, txtPassword, btnLogin, linkRegister, lblError });
            AcceptButton = btnLogin;

            btnLogin.Click += async (s, e) => await DoLogin();
            linkRegister.LinkClicked += (s, e) => { using var f = new frmRegister(); f.ShowDialog(this); };
        }

        private async Task DoLogin()
        {
            lblError.Text = "";
            if (txtUsername.Text.Trim() == "" || txtPassword.Text == "")
            { lblError.Text = "Enter your username and password."; return; }

            btnLogin.Enabled = false;
            try
            {
                var (ok, msg) = await ApiClient.LoginAsync(txtUsername.Text.Trim(), txtPassword.Text);
                if (ok) { DialogResult = DialogResult.OK; Close(); }
                else lblError.Text = msg;
            }
            catch (HttpRequestException)
            { lblError.Text = "Cannot reach the server. Is ItemAPI running?"; }
            finally { btnLogin.Enabled = true; }
        }
    }
}
