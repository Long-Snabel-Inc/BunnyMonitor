using BunnyMonitor.Models;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace BunnyMonitor.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private ILocalStorageService _localStorageService;

        public AccountService(HttpClient httpClient, IConfiguration configuration, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration.GetValue<string>("ApiEndpoint");
            _localStorageService = localStorageService;
        }

        public User User { get; set; }

        public bool IsLoggedIn => User != null;

        public async Task Initialize()
        {
            User = await _localStorageService.GetItemAsync<User>("user");
        }

        public async Task<bool> Login(Login user)
        {
            var resp = await _httpClient.PostAsJsonAsync(_baseUrl + "/User/Authenticate", user);
            Console.WriteLine(await resp.Content.ReadAsStringAsync());
            if (resp.IsSuccessStatusCode)
            {
                User = await resp.Content.ReadFromJsonAsync<User>();
                await _localStorageService.SetItemAsync("user", User);
                return true;
            }
            return false;
        }

        public async Task<bool> Register(User user)
        {
            var resp = await _httpClient.PostAsJsonAsync(_baseUrl + "/User/Register", user);
            if (resp.IsSuccessStatusCode)
            {
                User = await resp.Content.ReadFromJsonAsync<User>();
                await _localStorageService.SetItemAsync("user", User);
                return true;
            }
            return false;
        }
    }
}
