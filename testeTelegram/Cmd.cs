using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace testeTelegram
{
    public static class Cmd
    {
        const string cnx = "Data Source=10.5.0.4\\PRD;Initial Catalog=SINE_PRD;User ID=SINE_app;Password=s1n3pr0d@2014;Connection Timeout=40;MultipleActiveResultSets=true";
        public static string libera(string cpf){
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                string query = "update BNE_IMP.BNE.BNE_CURRICULO set flg_inativo = 0, idf_situacao_curriculo = 10 where idf_pessoa_fisica = " +
                    $"(select idf_pessoa_fisica from BNE_IMP.BNE.TAB_PESSOA_FISICA where num_cpf = {cpf}) " +
                    $"update bne_imp.bne.tab_usuario_filial_perfil set flg_inativo = 0 where idf_pessoa_fisica = " +
                    $"(select idf_pessoa_fisica from BNE_IMP.BNE.TAB_PESSOA_FISICA where num_cpf = {cpf})";
                
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return "ok";
            }
            
        }
        public static string vip(string cpf)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                string query = "update BNE_IMP.BNE.BNE_CURRICULO set flg_vip = 1 where idf_pessoa_fisica = " +
                    $"(select idf_pessoa_fisica from BNE_IMP.BNE.TAB_PESSOA_FISICA where num_cpf = {cpf}) " +
                    $"update bne_imp.bne.tab_usuario_filial_perfil set idf_perfil = 2 where idf_pessoa_fisica = " +
                    $"(select idf_pessoa_fisica from BNE_IMP.BNE.TAB_PESSOA_FISICA where num_cpf = {cpf})";

                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return "ok";
            }

        }

        public static string unvip(string cpf)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                string query = "update BNE_IMP.BNE.BNE_CURRICULO set flg_vip = 0 where idf_pessoa_fisica = " +
                    $"(select idf_pessoa_fisica from BNE_IMP.BNE.TAB_PESSOA_FISICA where num_cpf = {cpf}) " +
                    $"update bne_imp.bne.tab_usuario_filial_perfil set idf_perfil = 3 where idf_pessoa_fisica = " +
                    $"(select idf_pessoa_fisica from BNE_IMP.BNE.TAB_PESSOA_FISICA where num_cpf = {cpf})";

                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return "ok";
            }

        }

        public static string vcpf(string cpf)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                string query = $@"select pf.nme_pessoa,vp.vlr_parametro,pf.dta_nascimento, f.num_cnpj from bne_imp.bne.tab_pessoa_fisica pf
                join bne_imp.bne.tab_parametro_pessoa_fisica vp on pf.Idf_Pessoa_Fisica = vp.Idf_Pessoa_Fisica
                join bne_imp.bne.tab_usuario_filial_perfil fp on fp.idf_pessoa_fisica = pf.idf_pessoa_fisica
                join bne_imp.bne.tab_filial f on f.idf_filial = fp.idf_filial where pf.num_cpf = {cpf} and vp.Idf_Parametro = 616";

                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                try
                {
                    string cnpj = reader["num_cnpj"].ToString();
                    string nme = reader["nme_pessoa"].ToString();
                    string dta = reader["dta_nascimento"].ToString();
                    string email = reader["vlr_parametro"].ToString();

                    string resultado = $"BASE SQL:\n CPF: {cpf}\nEmail: {email}\nData de nascimento: {dta}\nNumero CNPJ: {cnpj}\nNome: {nme}";
                    try
                    {
                        string pcpf = putDot.put(cpf);
                        string cntString = "Host=postgresql-caqui.postgres.database.azure.com;Database=IdentityServer;Port=5432;User ID=identityserver@postgresql-caqui;Password=FBsnGFS38uwLp2DyRvxP;SslMode=Require;";
                        using (NpgsqlConnection sqlPCon = new NpgsqlConnection(cntString))
                        {
                            string Pquery = $"select \"ClaimValue\" from \"AspNetUserClaims\" where \"ClaimType\" = 'name' and \"UserId\" = (select \"UserId\" from \"AspNetUserClaims\" where \"ClaimType\" = 'CPF' and \"ClaimValue\" = '{pcpf}')";
                            NpgsqlCommand pcmd = new NpgsqlCommand(Pquery, sqlPCon);
                            sqlPCon.Open();
                            NpgsqlDataReader preader = pcmd.ExecuteReader();
                            preader.Read();
                            string pname = preader["claimvalue"].ToString();

                            resultado += $"\nBASE POSTGRE\nNome:{pname}";
                        }
                    }
                    catch(Exception e) { Console.WriteLine(e.Message); }

                    return resultado;
                }
                catch { return "nenhuma informacao encontrada"; }
            }

        }
        public static string vemail(string email)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                string query = $@"select pf.nme_pessoa,pf.num_cpf,pf.dta_nascimento, f.num_cnpj from bne_imp.bne.tab_pessoa_fisica pf
                join bne_imp.bne.tab_parametro_pessoa_fisica vp on pf.Idf_Pessoa_Fisica = vp.Idf_Pessoa_Fisica
                join bne_imp.bne.tab_usuario_filial_perfil fp on fp.idf_pessoa_fisica = pf.idf_pessoa_fisica
                join bne_imp.bne.tab_filial f on f.idf_filial = fp.idf_filial where vp.vlr_parametro = '{email}'";

                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                try
                {
                    string cpf = reader["num_cpf"].ToString();
                    string cnpj = reader["num_cnpj"].ToString();
                    string nme = reader["nme_pessoa"].ToString();
                    string dta = reader["dta_nascimento"].ToString();

                    string resultado = $"BASE SQL:\n CPF: {cpf}\nEmail: {email}\nData de nascimento: {dta}\nNumero CNPJ: {cnpj}\nNome: {nme}";

                    return resultado;
                }
                catch { return "nenhuma informacao encontrada"; }
            }
        }

       

    }
}
