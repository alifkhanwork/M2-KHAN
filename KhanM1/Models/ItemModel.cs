namespace ItemDataLibrary.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Brand { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public string Platform { get; set; }
        public string Condition { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
    }

}