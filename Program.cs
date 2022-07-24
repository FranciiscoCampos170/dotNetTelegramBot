using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5068974834:AAHWdjg7uth1MU0c5JYsxBQmErpAvJnOR4U");

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Type != UpdateType.Message)
        return;
    // Only process text messages
    if (update.Message!.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    var messageText = update.Message.Text;
    var from = update.Message.From;
    int opt = 0;
    string resposta = string.Empty;

    switch (messageText)
    {
        case "1":
            resposta = "Boa Pergunta";
            break;
        default:
            resposta = "Opção errada amigo";
            break;
    }
    
    

Console.WriteLine($"Received a '{messageText}' message in chat {chatId}, from {from}");
    //Console.WriteLine("")

    // Echo received message text
    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: resposta,
        parseMode: ParseMode.MarkdownV2,
        replyToMessageId: update.Message.MessageId,

        cancellationToken: cancellationToken);

    Message message = await botClient.SendPhotoAsync(
            chatId: chatId,
            photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
            caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);

    Message message1 = await botClient.SendStickerAsync(
            chatId: chatId,
            sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp",
            cancellationToken: cancellationToken);
}



Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
