using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace testeTelegram
{
    public  class PCmd
    {
        public async Task<string> getEmail(string email)
        {
            try
            {
                string cntString = "Host=postgresql-caqui.postgres.database.azure.com;Database=IdentityServer;Port=5432;User ID=identityserver@postgresql-caqui;Password=FBsnGFS38uwLp2DyRvxP;SslMode=Require;";
                NpgsqlConnection sqlCon = new NpgsqlConnection(cntString);
                string query = $"select \"Id\", \"UserName\" from \"AspNetUsers\" where \"Email\" = '{email}'";
                NpgsqlCommand cmd = new NpgsqlCommand(query, sqlCon);
                await sqlCon.OpenAsync();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                await reader.ReadAsync();

                string id = reader["id"].ToString();

                return $"GUID da empresa :\n{id}";
            }
            catch(Exception e) { return e.Message; }
        }
        public async Task<string> confirmEmail(string email)
        {
            try
            {
                string cntString = "Host=postgresql-caqui.postgres.database.azure.com;Database=IdentityServer;Port=5432;User ID=identityserver@postgresql-caqui;Password=FBsnGFS38uwLp2DyRvxP;SslMode=Require;";
                string id = await getEmail(email);

                NpgsqlConnection sqlCon = new NpgsqlConnection(cntString);
                string query = $"update \"AspNetUsers\" set \"EmailConfirmed\" = true where \"Id\" = '{id}'";
                NpgsqlDataAdapter sqlDa = new NpgsqlDataAdapter(query, sqlCon);
                DataTable table = new DataTable();
                await sqlCon.OpenAsync();
                sqlDa.Fill(table);
                
                return "email confirmado.";
            }
            catch(Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
