using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testeTelegram
{
 
    public class Cmd
    {
        
        const string cnx = "Data Source=10.5.0.4\\PRD;Initial Catalog=SINE_PRD;User ID=SINE_app;Password=s1n3pr0d@2014;Connection Timeout=40;MultipleActiveResultSets=true";
        public static string libera(string cpf)
        {
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
                    catch (Exception e) { Console.WriteLine(e.Message); }

                    return resultado;
                }
                catch { return "nenhuma informacao encontrada"; }
            }

        }
        public static string vemail(string email)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                try
                {
                    string query = $@"select pf.nme_pessoa,pf.num_cpf,pf.dta_nascimento, f.num_cnpj from bne_imp.bne.tab_pessoa_fisica pf
                join bne_imp.bne.tab_parametro_pessoa_fisica vp on pf.Idf_Pessoa_Fisica = vp.Idf_Pessoa_Fisica
                join bne_imp.bne.tab_usuario_filial_perfil fp on fp.idf_pessoa_fisica = pf.idf_pessoa_fisica
                join bne_imp.bne.tab_filial f on f.idf_filial = fp.idf_filial where vp.vlr_parametro = '{email}'";

                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();



                    string cpf = reader["num_cpf"].ToString();
                    string cnpj = reader["num_cnpj"].ToString();
                    string nme = reader["nme_pessoa"].ToString();
                    string dta = reader["dta_nascimento"].ToString();

                    string resultado = $"BASE SQL:\n CPF: {cpf}\nEmail: {email}\nData de nascimento: {dta}\nNumero CNPJ: {cnpj}\nNome: {nme}";

                    return resultado;
                }
                catch (Exception e) { return $"nenhuma informacao encontrada : error : {e.Message}"; }
            }
        }
        public static string candidatosTbr(string funcao, string cidade)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                try
                {
                    string queryCv = $@"select top 30 cv.Num_CPF, cv.Nme_Usuario,cv.dta_atualizacao from sine.SIN_Curriculo cv 
                    join plataforma.TAB_Cidade city on cv.idf_Cidade = city.Idf_Cidade
                    join plataforma.TAB_Funcao_Sinonimo fun on cv.Idf_Funcao_Sinonimo = fun.Idf_Funcao_Sinonimo
                    where city.Nme_Cidade = '{cidade}' AND fun.Nme_Sinonimo = '{funcao}'
                    order by cv.Dta_Atualizacao desc";
                    SqlCommand cmd = new SqlCommand(queryCv, sqlCon);
                    cmd.Connection.Open();
                    DataTable table = new DataTable();
                    SqlDataAdapter sqlDa = new SqlDataAdapter(queryCv, sqlCon);
                    sqlDa.Fill(table);
                    string n1 = table.Rows[0].ItemArray[0].ToString();

                    string candidatos = $"CANDIDATOS PARA A FUNCÃO DE {funcao} EM {cidade} (TRABALHA)";

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        candidatos += $"\nCandidato {table.Rows[i].ItemArray[1].ToString()}, \nCPF: {table.Rows[i].ItemArray[0].ToString()}" +
                            $"\nData atualizacao CV: {table.Rows[i].ItemArray[2]}\n";
                    }


                    return candidatos;
                }
                catch (Exception e) { return $"Error ! : {e.Message}"; }
            }
        }
        public static string candidatosBne(string funcao, string cidade)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                try
                {
                    string queryCv = $@"select top 30  pf.nme_pessoa, pf.Num_CPF, cv.dta_atualizacao from bne_imp.bne.tab_pessoa_fisica pf join
                bne_imp.bne.bne_curriculo cv on pf.idf_pessoa_fisica = cv.idf_pessoa_fisica join
                bne_imp.bne.bne_funcao_pretendida fp on cv.idf_curriculo = fp.idf_curriculo join
                bne_imp.plataforma.tab_funcao f on f.idf_funcao = fp.idf_funcao join
                bne_imp.plataforma.tab_cidade c on cv.idf_cidade_pretendida = c.idf_cidade where f.des_funcao = '{funcao}' and c.nme_cidade ='{cidade}'
                order by cv.Dta_Atualizacao desc";
                    SqlCommand cmd = new SqlCommand(queryCv, sqlCon);
                    cmd.Connection.Open();
                    DataTable table = new DataTable();
                    SqlDataAdapter sqlDa = new SqlDataAdapter(queryCv, sqlCon);
                    sqlDa.Fill(table);
                    string n1 = table.Rows[0].ItemArray[0].ToString();

                    string candidatos = $"CANDIDATOS PARA A FUNCÃO DE {funcao} EM {cidade} (BNE)";

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        candidatos += $"\nCandidato {table.Rows[i].ItemArray[1].ToString()}, \nCPF: {table.Rows[i].ItemArray[0].ToString()}" +
                            $"\nData atualizacao CV: {table.Rows[i].ItemArray[2]}\n";
                    }


                    return candidatos;
                }
                catch (Exception e) { return $"Error ! : {e.Message}"; }
            }
        }
        public static string candidatosVaga(string idf_vaga)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                try
                {
                    string query = $@"select top 50 pf.num_cpf, pf.dta_cadastro  from bne_imp.bne.tab_pessoa_fisica pf join
            bne_imp.bne.bne_curriculo cv on pf.idf_pessoa_fisica = cv.idf_pessoa_fisica join
            bne_imp.bne.bne_vaga_candidato ca on cv.idf_curriculo = ca.idf_curriculo where idf_vaga = {idf_vaga} and idf_status_candidatura = 1 order by dta_cadastro desc";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    DataTable table = new DataTable();
                    sqlDa.Fill(table);
                    string resultado = $"Candidatos para a vaga : {idf_vaga}";
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        resultado += $"\n{i + 1} CPF: {table.Rows[i].ItemArray[0].ToString()} Data candidatura: {table.Rows[i].ItemArray[1].ToString()}";
                    }
                    return resultado;
                }
                catch (Exception e) { return $"Erro: {e.Message}"; }
            }

        }
        public static string plano(string cnpj)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(cnx))
                {
                    string query = $@"select top 7 idf_plano_adquirido,dta_inicio_plano, dta_fim_plano, idf_plano_situacao, pl.des_plano from bne_imp.bne.bne_plano_adquirido pa join
                bne_imp.bne.bne_plano pl on pa.idf_plano = pl.idf_plano
                where Idf_Filial = (select idf_filial from bne_imp.bne.tab_filial where num_cnpj = {cnpj}) order by Dta_Fim_Plano desc";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    DataTable table = new DataTable();
                    sqlCon.Open();
                    sqlDa.Fill(table);
                    string resultado = $"\nCNPJ : {cnpj}";
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string idf = table.Rows[i].ItemArray[0].ToString();
                        string inicio = table.Rows[i].ItemArray[1].ToString();
                        string fim = table.Rows[i].ItemArray[2].ToString();
                        string id_situacao = table.Rows[i].ItemArray[3].ToString();
                        string nome_plano = table.Rows[i].ItemArray[4].ToString();
                        string situacao = "";
                        if (id_situacao == "0") situacao = "Aguardando liberacao";
                        else if (id_situacao == "1") situacao = "Liberado";
                        else if (id_situacao == "2") situacao = "encerrado";
                        else if (id_situacao == "3") situacao = "cancelado";
                        else if (id_situacao == "4") situacao = "bloqueado";
                        resultado += $"\n {nome_plano} Id : {idf}\nData Inicio: {inicio} \nData Fim: {fim}\nSituacao: {situacao}\n";
                    }

                    return resultado;
                }
            }
            catch (Exception e) { return e.Message; }
        }
        public static string cancela(string id)
        {
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                try
                {
                    string query = $@"update bne_imp.bne.bne_plano_adquirido set idf_plano_situacao = 2 where idf_plano_adquirido = {id}";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlCon);
                    DataTable table = new DataTable();
                    sqlDa.Fill(table);
                    string resultado = $"Plano {id} encerrado.";
                    return resultado;
                }
                catch (Exception e) { return $"Erro: {e.Message}"; }
            }

        }
        public static async Task<string> vincula(string cpf, string email)
        {
            PCmd pcmd = new PCmd();
            string guid = "";
            try { guid = await pcmd.getEmail(email); }
            catch(Exception e) { return "Postgre error:" + e.Message;  }
            
            using (SqlConnection sqlCon = new SqlConnection(cnx))
            {
                try
                {
                    guid = Regex.Replace(guid, @"GUID da empresa :", "");
                    guid = Regex.Replace(guid, @"\s+", "");
                    await sqlCon.OpenAsync();
                    string query1 = $"select idf_pessoa_fisica from bne_imp.bne.tab_pessoa_fisica where num_cpf = {cpf}";
                    SqlCommand cmd = new SqlCommand(query1, sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();
                    await reader.ReadAsync();
                    string idf_pf = reader["idf_pessoa_fisica"].ToString();

                    string query2 = $@"INSERT INTO BNE_IMP.BNE.TAB_Parametro_Pessoa_Fisica (Idf_Parametro,Idf_Pessoa_Fisica,Dta_Cadastro,Vlr_Parametro, Flg_Inativo, Dta_Alteracao)

                VALUES (615, {idf_pf}, GETDATE(), '{guid}', 0, GETDATE() )

                INSERT INTO BNE_IMP.BNE.TAB_Parametro_Pessoa_Fisica (Idf_Parametro,Idf_Pessoa_Fisica, Dta_Cadastro, Vlr_Parametro,Flg_Inativo,Dta_Alteracao)

                VALUES (616, {idf_pf},  GETDATE(),'{email}', 0, GETDATE() )";
                    SqlDataAdapter sqlDa = new SqlDataAdapter(query2, sqlCon);
                    DataTable table = new DataTable();
                    sqlDa.Fill(table);
                    return $"{cpf} vinculado ao email {email}";
                }
                catch (Exception e) { return $"Erro: {e.Message}"; }
            }

        }

    }


}

