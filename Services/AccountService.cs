using BunnyMonitor.Models;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using BunnyMonitor.Extensions;

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
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
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

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("user");
        }

        public async Task<bool> Register(User user)
        {
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
            var resp = await _httpClient.PostAsJsonAsync(_baseUrl + "/User/Register", user);
            if (resp.IsSuccessStatusCode)
            {
                User = await resp.Content.ReadFromJsonAsync<User>();
                await _localStorageService.SetItemAsync("user", User);
                return true;
            }
            return false;
        }

        public async Task<List<User>> AllUsers()
        {
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
            var users = await _httpClient.GetFromJsonAsync<List<User>>(_baseUrl + "/User/AllUsers");
            foreach (User user in users)
            {
                var score = await _httpClient.GetFromJsonAsync<double>(_baseUrl + $"/User/{user.Id}/Score");
                user.Score = score;
            }
            return users;
        }

        public async Task<double> Score()
        {
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
            return await _httpClient.GetFromJsonAsync<double>(_baseUrl + $"/User/{User.Id}/Score");
        }

        public async Task<DateTime> LastUpdate()
        {
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
            return await _httpClient.GetFromJsonAsync<DateTime>(_baseUrl + $"/User/LastRating");
        }

        public async Task<User> ClosestUser()
        {
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
            return await _httpClient.GetFromJsonAsync<User>(_baseUrl + $"/Location/ClosestUser");
        }

        public async Task RateUser(int i, int userId)
        {
            await _httpClient.AddAuthHeaderAsync(_localStorageService);
            await _httpClient.GetFromJsonAsync<int>(_baseUrl + $"/User/Rate/{userId}/{i}");
        }


    }
}
