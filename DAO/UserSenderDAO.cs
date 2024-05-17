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
    public class UserSenderDAO : PadraoDAO<UserSenderViewModel>
    {
        /// <summary>
        /// RECEBE AS INFOS DO BANCO E MONTA UM OBJETO
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        protected override UserSenderViewModel MontaModel(DataRow registro)
        {
            UserSenderViewModel senderModel = new UserSenderViewModel()
            {
                id = Convert.ToInt32(registro["user_sender_id"]),
                username = registro["username"].ToString(),
                full_name = registro["full_name"].ToString(),
                cpf = registro["cpf"].ToString(),
                phone_number = registro["phone_number"].ToString(),
                postal_code = registro["postal_code"].ToString(),
                address_street = registro["address_street"].ToString(),
                address_number = registro["address_number"].ToString(),
                address_state = registro["address_state"].ToString(),
                address_city = registro["address_city"].ToString(),
                address_district = registro["address_district"].ToString(),
                email = registro["email"].ToString(),
                created_date = Convert.ToDateTime(registro["created_date"])
            };
            if (registro["address_complement"] != DBNull.Value)
                senderModel.address_complement = registro["address_complement"].ToString();

            return senderModel;
        }

        /// <summary>
        /// MANDA PRO BANCO A PARTIR DO OBJETO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override SqlParameter[] CriaParametros(UserSenderViewModel model)
        {
            var parametros = new SqlParameter[14];

            parametros[0] = new SqlParameter("user_sender_id", Convert.ToInt32(model.id));
            parametros[1] = new SqlParameter("username", model.username.ToString());
            parametros[2] = new SqlParameter("full_name", model.full_name.ToString());
            parametros[3] = new SqlParameter("cpf", model.cpf.ToString().Replace(".", "").Replace("-", ""));
            parametros[4] = new SqlParameter("phone_number", model.phone_number.ToString().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
            parametros[5] = new SqlParameter("postal_code", model.postal_code.ToString().Replace("-", ""));
            parametros[6] = new SqlParameter("address_street", model.address_street.ToString());
            parametros[7] = new SqlParameter("address_number", model.address_number.ToString());
            
            if (model.address_complement == null)
                parametros[8] = new SqlParameter("address_complement", DBNull.Value);
            else
                parametros[8] = new SqlParameter("address_complement", model.address_complement.ToString());

            parametros[9] = new SqlParameter("address_district", model.address_district.ToString());
            parametros[10] = new SqlParameter("address_city", model.address_city.ToString());
            parametros[11] = new SqlParameter("address_state", model.address_state.ToString());
            parametros[12] = new SqlParameter("email", model.email.ToString());
            parametros[13] = new SqlParameter("created_date", Convert.ToDateTime(model.created_date));

            return parametros;
        }


        /// <summary>
        /// INDICA A TABELA DO BANCO
        /// </summary>
        protected override void SetTabela()
        {
            Tabela = "register_sender_user";
        }

    }
}
