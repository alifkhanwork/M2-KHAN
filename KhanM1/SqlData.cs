using ItemDataLibrary.Db;
using ItemDataLibrary.Models;

namespace ItemDataLibrary
{
    public interface ISqlData
    {
        List<ItemModel> ListItems();
        ItemModel? GetItem(int id);
        void AddItem(ItemModel item);
        void UpdateItem(ItemModel item);
        void DeleteItem(int id);
        UserModel? Authenticate(string username, string password);
        void Register(string username, string firstName, string lastName, string password);

        void CreateTransaction(TransactionModel t);
        List<TransactionModel> ListTransactions(DateTime? from, DateTime? to);
        SummaryModel GetSummary();
        List<ItemModel> ListLowStock();
    }

    public class SqlData : ISqlData
    {
        private const string connectionStringName = "SqlDb";
        private readonly ISqlDataAccess _db;
        public SqlData(ISqlDataAccess db) => _db = db;

        public List<ItemModel> ListItems() =>
            _db.LoadData<ItemModel, dynamic>("dbo.spItems_List", new { }, connectionStringName, true);

        public ItemModel? GetItem(int id) =>
            _db.LoadData<ItemModel, dynamic>("dbo.spItems_Details", new { id }, connectionStringName, true).FirstOrDefault();

        public void AddItem(ItemModel i) =>
    _db.SaveData<dynamic>("dbo.spItems_Insert",
        new { i.Name, i.Code, i.Brand, i.UnitPrice, i.Platform, i.Condition, i.Quantity, i.ReorderLevel },
        connectionStringName, true);

        public void UpdateItem(ItemModel i) =>
            _db.SaveData<dynamic>("dbo.spItems_Update",
                new { i.Id, i.Name, i.Code, i.Brand, i.UnitPrice, i.Platform, i.Condition, i.Quantity, i.ReorderLevel },
                connectionStringName, true);

        public void DeleteItem(int id) =>
            _db.SaveData<dynamic>("dbo.spItems_Delete", new { id }, connectionStringName, true);

        public UserModel? Authenticate(string username, string password) =>
    _db.LoadData<UserModel, dynamic>("dbo.spUsers_Authenticate",
        new { username, password }, connectionStringName, true).FirstOrDefault();

        public void Register(string username, string firstName, string lastName, string password) =>
            _db.SaveData<dynamic>("dbo.spUsers_Register",
                new { username, firstName, lastName, password }, connectionStringName, true);

        public void CreateTransaction(TransactionModel t) =>
           _db.SaveData<dynamic>("dbo.spTransactions_Create",
               new { t.ItemId, t.Type, t.Quantity, t.UnitPrice, t.Notes, t.CreatedBy },
               connectionStringName, true);

        public List<TransactionModel> ListTransactions(DateTime? from, DateTime? to) =>
            _db.LoadData<TransactionModel, dynamic>("dbo.spTransactions_List",
                new { From = from, To = to }, connectionStringName, true);

        public SummaryModel GetSummary() =>
            _db.LoadData<SummaryModel, dynamic>("dbo.spReport_Summary",
                new { }, connectionStringName, true).FirstOrDefault() ?? new SummaryModel();

        public List<ItemModel> ListLowStock() =>
            _db.LoadData<ItemModel, dynamic>("dbo.spReport_LowStock",
                new { }, connectionStringName, true);
    }
}