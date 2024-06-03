using PBL_Electronicrap.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL_Electronicrap.DAO
{
    public class LoginDAO : PadraoDAO<LoginViewModel>
    {
        protected override LoginViewModel MontaModel(DataRow registro)
        {
            LoginViewModel c = new LoginViewModel();
            if (registro.Table.Columns.Contains("receiver_login_id"))
            {
                c.id = Convert.ToInt32(registro["receiver_login_id"]);
                c.username = registro["fk_receiver_username"].ToString();
                c.password = registro["receiver_password"].ToString();
                c.userType = "receiver";
            }
            else if (registro.Table.Columns.Contains("receiver_login_id"))
            {
                c.id = Convert.ToInt32(registro["sender_login_id"]);
                c.username = registro["fk_sender_username"].ToString();
                c.password = registro["sender_password"].ToString();
                c.userType = "sender";
            }

            return c;
        }
        protected override SqlParameter[] CriaParametros(LoginViewModel model)
        {
            SqlParameter[] parametros =
            {
            };
            return parametros;
        }
        protected override void SetTabela()
        {
            Tabela = "";
        }
    }
}
