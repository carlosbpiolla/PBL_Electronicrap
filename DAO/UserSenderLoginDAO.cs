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
    public class UserSenderLoginDAO : PadraoDAO<UserSenderLoginViewModel>
    {

        protected override UserSenderLoginViewModel MontaModel(DataRow registro)
        {
            UserSenderLoginViewModel senderLoginModel = new UserSenderLoginViewModel()
            {
                id = Convert.ToInt32(registro["sender_login_id"]),
                username = registro["fk_sender_username"].ToString(),
                password = registro["sender_password"].ToString()
            };
            return senderLoginModel;
        }

        /// <summary>
        /// MANDA PRO BANCO A PARTIR DO OBJETO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override SqlParameter[] CriaParametros(UserSenderLoginViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("sender_login_id", Convert.ToInt32(model.id)),
                new SqlParameter("fk_sender_username", model.username.ToString()),
                new SqlParameter("sender_password", model.password.ToString())
            };
            return parametros;
        }



        /// <summary>
        /// INDICA A TABELA DO BANCO
        /// </summary>
        protected override void SetTabela()
        {
            Tabela = "register_sender_logins";
        }
    }
}
