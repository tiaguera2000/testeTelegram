using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace testeTelegram
{
    public static class PCmd
    {
        public static string getEmail(string email)
        {
            try
            {
                string cntString = "Host=postgresql-caqui.postgres.database.azure.com;Database=IdentityServer;Port=5432;User ID=identityserver@postgresql-caqui;Password=FBsnGFS38uwLp2DyRvxP;SslMode=Require;";
                NpgsqlConnection sqlCon = new NpgsqlConnection(cntString);
                string query = $"select \"Id\", \"UserName\" from \"AspNetUsers\" where \"Email\" = '{email}'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, sqlCon);
                sqlCon.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                string id = reader["id"].ToString();

                return $"GUID da empresa :\n {id}";
            }
            catch { return "not found on POSTGRE!"; }
        }
    }
}
