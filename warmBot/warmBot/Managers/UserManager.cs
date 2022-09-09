using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warmBot.Core;

namespace warmBot.Managers
{
    internal class UserManager
    {
        private static string UserFolder = "Resources";
        private static string UserFile = "users.json";
        private static string UserPath = $"{UserFolder}/{UserFile}";

        // TODO: Make this a dictionary.
        //public static List<UserDetails> Users { get; private set; }
        public static Dictionary<SocketUser, UserDetails> Users { get; private set; }

        static UserManager()
        {
            if (!Directory.Exists(UserFolder)) Directory.CreateDirectory(UserFolder);

            if (!File.Exists(UserPath))
            {
                Users = new Dictionary<SocketUser, UserDetails>();
                SerializeUsers();
            }
            else
            {
                string json = File.ReadAllText(UserPath);
                Users = JsonConvert.DeserializeObject<Dictionary<SocketUser, UserDetails>>(json);
            }
        }

        public static async Task<string?> GetUserAsync(SocketUser user)
        {
            if (Users[user].Puuid.Length == 78) return Users[user].Puuid;
            else if (!String.IsNullOrWhiteSpace(Users[user].Summoner))
            {
                UserDetails Deets = new UserDetails();
                string? puuid = await RiotManager.GetSummonerAsync(Users[user].Summoner);
                if (puuid != null && puuid.Length == 78)
                {
                    Deets.Puuid = puuid;
                    SerializeUsers();
                    return puuid;
                }
            }
            else
            {
                UserDetails Deets = new UserDetails();
                Deets.DiscordUser = user;
            }

            return null;
        }

        public static string SetSummoner(SocketUser user, string sid)
        {
            Users.TryGetValue(user, out UserDetails deets);
            deets.Summoner = sid;

            Users.TryGetValue(user, out UserDetails update);

            if (update.Equals(deets)) return "User updated successfully!";
            else return "It didn't work.";
        }

        public static async Task AddUser(SocketUser user)
        {
            if (!Users.TryGetValue(user, out UserDetails deets))
            {
                deets = new UserDetails();
                deets.DiscordUser = user;

                await Bot.channel.SendMessageAsync($"Please use the {ConfigManager.Config.Prefix}user command, followed by your summoner name.");
            }
        }

        private static void SerializeUsers()
        {
            string json = JsonConvert.SerializeObject(Users, Formatting.Indented);
            File.WriteAllText(UserPath, json);
        }

        public struct UserDetails
        {
            public SocketUser DiscordUser { get; set; }
            public string Summoner { get; set; }
            public string Puuid { get; set; }
        }
    }
}
