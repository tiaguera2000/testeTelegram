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

                else if(e.Message.Text.Contains("/libera"))
                {
                    string cpf = e.Message.Text.Replace("/libera ", "");
                    string verifica = Cmd.libera(cpf);
                    if(verifica == "ok")bot.SendTextMessageAsync(e.Message.Chat.Id, $"cpf {cpf} liberado.");
                }

                else if (e.Message.Text.Contains("/vip"))
                {
                    string cpf = e.Message.Text.Replace("/vip ", "");
                    string verifica = Cmd.vip(cpf);
                    if (verifica == "ok") bot.SendTextMessageAsync(e.Message.Chat.Id, $"cpf {cpf} com vip ativo.");
                }

                else if (e.Message.Text.Contains("/unvip"))
                {
                    string cpf = e.Message.Text.Replace("/unvip ", "");
                    string verifica = Cmd.unvip(cpf);
                    if (verifica == "ok") bot.SendTextMessageAsync(e.Message.Chat.Id, $"cpf {cpf} com vip inativo.");
                }
                else if (e.Message.Text.Contains("/cpf"))
                {
                    string cpf = e.Message.Text.Replace("/cpf ", "");
                    string verifica = Cmd.vcpf(cpf);
                    bot.SendTextMessageAsync(e.Message.Chat.Id, $"{verifica}");
                }
                else if (e.Message.Text.Contains("/email"))
                {
                    string email = e.Message.Text.Replace("/email ", "");
                    string verifica = Cmd.vemail(email);
                    bot.SendTextMessageAsync(e.Message.Chat.Id, $"{verifica}");
                }

                else if (e.Message.Text.Contains("/id"))
                {
                    string email = e.Message.Text.Replace("/id ", "");
                    string verifica = PCmd.getEmail(email);
                    bot.SendTextMessageAsync(e.Message.Chat.Id, $"{verifica}");
                }
            }
        }
    }
}
