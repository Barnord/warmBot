using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warmBot.Managers
{
    internal class EventManager
    {
        private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
        private static CommandService _commandService = ServiceManager.GetService<CommandService>();
        //private static IReadOnlyCollection

        public static Task LoadCommands()
        {
            _client.Log += msg =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t({msg.Source})\t{msg.Message}");
                return Task.CompletedTask;
            };

            _commandService.Log += msg =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t({msg.Source})\t{msg.Message}");
                return Task.CompletedTask;
            };

            _client.Ready += OnReady;

            _client.MessageReceived += OnMessageReceived;
            _client.UserVoiceStateUpdated += OnUserJoinVoice;

            return Task.CompletedTask;
        }

        private static async Task OnMessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, msg);

            if (msg.Author.IsBot || msg.Channel is IDMChannel) return;

            int argPos = 0;

            if (!(msg.HasStringPrefix(ConfigManager.Config.Prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var result = await _commandService.ExecuteAsync(context, argPos, ServiceManager.Provider);

            if (!result.IsSuccess) if (result.Error == CommandError.UnknownCommand) return;
        }

        private static async Task OnUserJoinVoice(SocketUser user, SocketVoiceState left, SocketVoiceState joined)
        {
            if (user.IsBot) return;

            string Puuid = UserManager.GetUserAsync(user);
            
            if (joined.VoiceChannel is null)
            {
                Console.WriteLine($"User:{user}\tLeft Channel:{left}");
            }
            else
            {
                Console.WriteLine($"User:{user}\tJoined Channel:{joined}");
            }
        }

        private static async Task OnReady()
        {
            Console.WriteLine($"[{DateTime.Now}]\t(READY)\tBot is ready");
            await _client.SetStatusAsync(Discord.UserStatus.Online);
            await _client.SetGameAsync($"Prefix {ConfigManager.Config.Prefix}", null, ActivityType.Listening);
        }
    }
}
