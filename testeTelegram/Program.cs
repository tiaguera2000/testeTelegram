using System;
using System.Text.RegularExpressions;
using Telegram.Bot;
using System.Threading;


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

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {
                PCmd pcmd = new PCmd();
                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (e.Message.Text == "oi")
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "olá!");

                    else if (e.Message.Text.Contains("/libera"))
                    {
                        string cpf = e.Message.Text.Replace("/libera ", "");
                        string verifica = Cmd.libera(cpf);
                        if (verifica == "ok") await bot.SendTextMessageAsync(e.Message.Chat.Id, $"cpf {cpf} liberado.");
                    }

                    else if (e.Message.Text.Contains("/vip"))
                    {
                        string cpf = e.Message.Text.Replace("/vip ", "");
                        string verifica = Cmd.vip(cpf);
                        if (verifica == "ok") await bot.SendTextMessageAsync(e.Message.Chat.Id, $"cpf {cpf} com vip ativo.");
                    }

                    else if (e.Message.Text.Contains("/unvip"))
                    {
                        string cpf = e.Message.Text.Replace("/unvip ", "");
                        string verifica = Cmd.unvip(cpf);
                        if (verifica == "ok") await bot.SendTextMessageAsync(e.Message.Chat.Id, $"cpf {cpf} com vip inativo.");
                    }
                    else if (e.Message.Text.Contains("/cpf"))
                    {
                        string cpf = e.Message.Text.Replace("/cpf ", "");
                        string verifica = Cmd.vcpf(cpf);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, $"{verifica}");
                    }
                    else if (e.Message.Text.Contains("/email"))
                    {
                        string email = e.Message.Text.Replace("/email ", "");
                        string verifica = Cmd.vemail(email);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, $"{verifica}");
                    }

                    else if (e.Message.Text.Contains("/id"))
                    {
                        string email = e.Message.Text.Replace("/id ", "");
                        string verifica = await pcmd.getEmail(email);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, $"{verifica}");
                    }
                    else if (e.Message.Text.Contains("cvtbr"))
                    {
                        string msg = e.Message.Text.Replace("/cvtbr ", "");
                        string funcao = Regex.Replace(msg, @"-.*", "");
                        string cidade = Regex.Replace(msg, @".*-", "");
                        string resultado = Cmd.candidatosTbr(funcao, cidade);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                    else if (e.Message.Text.Contains("cvbne"))
                    {
                        string msg = e.Message.Text.Replace("/cvbne ", "");
                        string funcao = Regex.Replace(msg, @"-.*", "");
                        string cidade = Regex.Replace(msg, @".*-", "");
                        string resultado = Cmd.candidatosBne(funcao, cidade);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                    else if (e.Message.Text.Contains("/candidatos"))
                    {
                        string idf = e.Message.Text.Replace("/candidatos ", "");
                        string resultado = Cmd.candidatosVaga(idf);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                    else if (e.Message.Text.Contains("/plano"))
                    {
                        string idf = e.Message.Text.Replace("/plano ", "");
                        string resultado = Cmd.plano(idf);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                    else if (e.Message.Text.Contains("/cancela"))
                    {
                        string id = e.Message.Text.Replace("/cancela ", "");
                        string resultado = Cmd.cancela(id);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                    else if (e.Message.Text.Contains("/vincula"))//\vincula 08929391923 email tiagoreis@a.com
                    {
                        string msg = e.Message.Text.ToLower();
                        string cpf = Regex.Replace(msg, @"\/vincula | email.*", "");
                        string email = Regex.Match(msg, @"(?<=email ).*").ToString();
                        string resultado = await Cmd.vincula(cpf, email);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                    else if (e.Message.Text.Contains("/confirma"))
                    {
                        string email = e.Message.Text.Replace("/confirma ", "");
                        string resultado = await pcmd.confirmEmail(email);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);

                    }
                    else if (e.Message.Text.Contains("/produto"))
                    {
                        string idf_vaga = e.Message.Text.Replace("/produto ", "");
                        string resultado = await Cmd.GetProduct(idf_vaga);
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, resultado);
                    }
                }


            }
            catch(Exception ex) { await bot.SendTextMessageAsync(e.Message.Chat.Id, ex.Message); }

        }
    }
}

