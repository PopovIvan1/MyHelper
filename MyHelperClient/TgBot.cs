using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;

namespace MyHelperClient
{
    public static class TgBot
    {
        private static ITelegramBotClient bot;

        public static void StartBot()
        {
            initializeBot(System.IO.File.ReadAllText("../../../app.txt"));
            Console.WriteLine("Bot " + bot.GetMeAsync().Result.FirstName + " launched.");
            Console.ReadLine();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Hello, how can I help you?");
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "ok");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        private static void initializeBot(string key)
        {
            bot = new TelegramBotClient(key);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
        }
    }
}