using Telegram.Bot;

var botClient = new TelegramBotClient("5068974834:AAHWdjg7uth1MU0c5JYsxBQmErpAvJnOR4U");

var me = await botClient.GetMeAsync();
Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");