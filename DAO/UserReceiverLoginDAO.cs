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
    public class UserReceiverLoginDAO : PadraoDAO<UserReceiverLoginViewModel>
    {
        /// <summary>
        /// recebe do banco os dados
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        protected override UserReceiverLoginViewModel MontaModel(DataRow registro)
        {
            UserReceiverLoginViewModel receiverLoginModel = new UserReceiverLoginViewModel()
            {
                id = Convert.ToInt32(registro["receiver_login_id"]),
                username = registro["fk_receiver_username"].ToString(),
                password = registro["receiver_password"].ToString()
            };
            return receiverLoginModel;
        }

        /// <summary>
        /// MANDA PRO BANCO A PARTIR DO OBJETO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override SqlParameter[] CriaParametros(UserReceiverLoginViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("receiver_login_id", Convert.ToInt32(model.id)),
                new SqlParameter("fk_receiver_username", model.username.ToString().ToLower()),
                new SqlParameter("receiver_password", model.password.ToString().ToLower())
            };
            return parametros;
        }



        /// <summary>
        /// INDICA A TABELA DO BANCO
        /// </summary>
        protected override void SetTabela()
        {
            Tabela = "register_receiver_logins";
        }
    }
}
