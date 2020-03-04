using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace testeTelegram
{
    public static class putDot
    {
        public static string put(string cpf)
        {
            if (cpf.Length == 10) cpf = "0" + cpf;
            string newCpf = "";
            byte c = 0;
            string g4 = Regex.Match(cpf, @"(?<=\d{9}).*").ToString();
            MatchCollection col = Regex.Matches(cpf, @"\d{3}");
            foreach(Match m in col)
            {
                if (c == 0 || c == 1) newCpf += $"{m.ToString()}.";
                else if (c == 2) newCpf += $"{m.ToString()}-{g4}";
                c++;
            }
            return newCpf;
        }
    }
}
