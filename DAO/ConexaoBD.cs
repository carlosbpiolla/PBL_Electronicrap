using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PBL_Electronicrap.DAO
{
    public class ConexaoBD
    {
        /// <summary>
        /// Método Estático que retorna um conexao aberta com o BD
        /// </summary>
        /// <returns>Conexão aberta</returns>
        public static SqlConnection GetConexao()
        {
            string strCon = "Server=tcp:server-electronicrap.database.windows.net,1433;Initial Catalog=DB_Electronicrap;Persist Security Info=False;User ID=082200013@faculdade.cefsa.edu.br;Password=Cbc141427#6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=" + "Active Directory Password;";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }

    }
}
