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
        public async Task<string> getId(string email)
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

                return id;
            }
            catch (Exception e) { return e.Message; }
        }
        public async Task<string> confirmEmail(string email)
        {
            try
            {
                string cntString = "Host=postgresql-caqui.postgres.database.azure.com;Database=IdentityServer;Port=5432;User ID=identityserver@postgresql-caqui;Password=FBsnGFS38uwLp2DyRvxP;SslMode=Require;";
                string id = await getId(email);
                if (id == "No row is available") return "email não existente";
                NpgsqlConnection sqlCon = new NpgsqlConnection(cntString);
                string query = $"update \"AspNetUsers\" set \"EmailConfirmed\" = true where \"Id\" = '{id}'";
                await sqlCon.OpenAsync();
                NpgsqlCommand cmd = new NpgsqlCommand(query, sqlCon);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                
                return "email confirmado.";
            }
            catch(Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
