using PixelPawn.Desktop.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PixelPawn.Desktop.Services
{
    public static class ApiClient
    {
        private static readonly HttpClient Http = new()
        {
            BaseAddress = new Uri("https://localhost:7118/")
        };
        private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };

        public static string? UserName { get; private set; }

        // ---------- Auth ----------
        public static async Task<(bool ok, string message)> LoginAsync(string user, string pass)
        {
            var res = await Http.PostAsJsonAsync("api/login/login",
                new LoginRequest { Username = user, Password = pass });
            if (!res.IsSuccessStatusCode) return (false, "Invalid username or password.");

            var body = await res.Content.ReadFromJsonAsync<LoginResponse>(Json);
            if (body == null || string.IsNullOrEmpty(body.IdToken))
                return (false, "Login succeeded but no token was returned.");

            Http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", body.IdToken);
            UserName = user;
            return (true, "");
        }

        public static void Logout()
        {
            Http.DefaultRequestHeaders.Authorization = null;
            UserName = null;
        }

        public static async Task<(bool ok, string message)> RegisterAsync(
            string username, string firstName, string lastName, string password)
        {
            var res = await Http.PostAsJsonAsync("api/login/register",
                new { username, firstName, lastName, password });
            return (res.IsSuccessStatusCode, await ErrorText(res));
        }

        // ---------- Items ----------
        public static async Task<List<ItemModel>> GetItemsAsync()
            => await Http.GetFromJsonAsync<List<ItemModel>>("api/item", Json) ?? new();

        public static async Task<(bool ok, string message)> SaveItemAsync(ItemModel item)
        {
            var res = item.Id == 0
                ? await Http.PostAsJsonAsync("api/item", item)
                : await Http.PutAsJsonAsync($"api/item/{item.Id}", item);
            return (res.IsSuccessStatusCode, await ErrorText(res));
        }

        public static async Task<(bool ok, string message)> DeleteItemAsync(int id)
        {
            var res = await Http.DeleteAsync($"api/item/{id}");
            return (res.IsSuccessStatusCode, await ErrorText(res));
        }

        // ---------- Transactions ----------
        public static async Task<(bool ok, string message)> AddTransactionAsync(TransactionModel t)
        {
            var res = await Http.PostAsJsonAsync("api/transaction", t);
            return (res.IsSuccessStatusCode, await ErrorText(res));
        }

        public static async Task<List<TransactionModel>> GetTransactionsAsync(DateTime? from, DateTime? to)
        {
            var url = "api/transaction";
            if (from != null && to != null)
                url += $"?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";
            return await Http.GetFromJsonAsync<List<TransactionModel>>(url, Json) ?? new();
        }

        // ---------- Reports ----------
        public static async Task<SummaryModel> GetSummaryAsync()
            => await Http.GetFromJsonAsync<SummaryModel>("api/report/summary", Json) ?? new();

        public static async Task<List<ItemModel>> GetLowStockAsync()
            => await Http.GetFromJsonAsync<List<ItemModel>>("api/report/lowstock", Json) ?? new();

        private static async Task<string> ErrorText(HttpResponseMessage res)
            => res.IsSuccessStatusCode ? "" : await res.Content.ReadAsStringAsync();
    }
}