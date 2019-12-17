using System;
using Telegram.Bot;

namespace testeTelegram
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("1006115671:AAE6exl1YiqfqTC29iesk0Q0sNhhEouRrxc");

        static void Main(string[] args)
        {
            bot.OnMessage += Bot_OnMessage;

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
            
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text) 
            {
                if (e.Message.Text == "oi")
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "olá!");
            }
        }
    }
}
