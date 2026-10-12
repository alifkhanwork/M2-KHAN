
namespace PixelPawn.Desktop.Models
{
    public class SummaryModel
    {
        public int TotalItems { get; set; }
        public int TotalUnits { get; set; }
        public decimal InventoryValue { get; set; }
        public int LowStockCount { get; set; }
        public decimal SalesToday { get; set; }
    }
}