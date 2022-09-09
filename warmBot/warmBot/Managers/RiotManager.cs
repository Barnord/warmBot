using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using warmBot.DTOs;

namespace warmBot.Managers
{
    internal class RiotManager
    {
        private static HttpClient _client = new HttpClient();

        public static Task RunAsync()
        {
            _client.BaseAddress = new Uri("https://na1.api.riotgames.com/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("X-Riot-Token", ConfigManager.Config.RiotToken);

            Console.WriteLine("Client online");

            return Task.CompletedTask;
        }

        public static async Task<string?> GetSummonerAsync(string sid)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"/lol/summoner/v4/summoners/by-name/{sid}");
                Task<string> result = response.Content.ReadAsStringAsync();
                SummonerDTO? summoner = JsonConvert.DeserializeObject<SummonerDTO>(result.Result);
                return summoner.puuid;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
