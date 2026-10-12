
namespace PixelPawn.Desktop.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public string Type { get; set; } = "";   
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}