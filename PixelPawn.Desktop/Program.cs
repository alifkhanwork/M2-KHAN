using PixelPawn.Desktop.Forms;

namespace PixelPawn.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            using (var login = new frmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }
            Application.Run(new frmMain());
        }
    }
}
