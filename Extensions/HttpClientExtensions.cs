using Blazored.LocalStorage;
using BunnyMonitor.Models;
using System.Net.Http.Headers;

namespace BunnyMonitor.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task AddAuthHeaderAsync(this HttpClient httpClient, ILocalStorageService localStorageService)
        {
            var user = await localStorageService.GetItemAsync<User>("user");
            if (user is not null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }
        }
    }
}
