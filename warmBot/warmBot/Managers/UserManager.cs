using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var json = JsonConvert.SerializeObject(Users, Formatting.Indented);
                File.WriteAllText(UserPath, json);
            }
            else
            {
                var json = File.ReadAllText(UserPath);
                Users = JsonConvert.DeserializeObject<Dictionary<SocketUser, UserDetails>>(json);
            }
        }

        public static async string GetUserAsync(SocketUser user)
        {
            if (Users[user].Puuid.Length == 78) return Users[user].Puuid;
            else if (!String.IsNullOrWhiteSpace(Users[user].Summoner))
            {
                UserDetails Deets = new UserDetails();
                await RiotManager.GetSummonerAsync(Users[user].Summoner);
                ConfigManager.BotConfig.Channel.SendMessageAsync();
            }
            else
            {
                UserDetails Deets = new UserDetails();
                Deets.DiscordUser = user;
                // Ask user to set summoner
            }
        }

        public static string SetSummoner(SocketUser user, string sid)
        {
            Users.TryGetValue(user, out UserDetails deets);
            deets.Summoner = sid;

            Users.TryGetValue(user, out UserDetails update);

            if (update.Equals(deets)) return "User updated successfully!";
            else return "It didn't work.";
        }

        public struct UserDetails
        {
            public SocketUser DiscordUser { get; set; }
            public string Summoner { get; set; }
            public string Puuid { get; set; }
        }
    }
}
