using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace warmBot.Managers
{
    internal class RiotManager
    {
        private static HttpClient _client = new HttpClient();

        static Task RunAsync()
        {
            _client.BaseAddress = new Uri("https://na1.api.riotgames.com/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Riot-Token", ConfigManager.Config.RiotToken);

            Console.WriteLine("Client online");

            return Task.CompletedTask;
        }

        static async Task<Uri> GetSummonerAsync(string sid)
        {
            HttpResponseMessage response = await _client.GetAsync($"/lol/summoner/v4/summoners/by-name/{sid}");

        }
    }
}
