﻿using PBL_Electronicrap.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL_Electronicrap.DAO
{
    public class UserReceiverDAO : PadraoDAO<UserReceiverViewModel>
    {
        /// <summary>
        /// recebe do banco os dados
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        protected override UserReceiverViewModel MontaModel(DataRow registro)
        {
            UserReceiverViewModel receiverModel = new UserReceiverViewModel()
            {
                id = Convert.ToInt32(registro["user_receiver_id"]),
                username = registro["username"].ToString(),
                full_name = registro["full_name"].ToString(),
                fantasy_name = registro["fantasy_name"].ToString(),
                cnpj = registro["cnpj"].ToString(),
                phone_number = registro["phone_number"].ToString(),
                postal_code = registro["postal_code"].ToString(),
                address_street = registro["address_street"].ToString(),
                address_number = registro["address_number"].ToString(),
                address_state = registro["address_state"].ToString(),
                address_city = registro["address_city"].ToString(),
                address_district = registro["address_district"].ToString(),
                email = registro["email"].ToString(),
                created_date = Convert.ToDateTime(registro["created_date"]),
                categoriaId = Convert.ToInt32(registro["categoria_lixo"])
            };
            if (registro["address_complement"] != DBNull.Value)
                receiverModel.address_complement = registro["address_complement"].ToString();
            if (registro.Table.Columns.Contains("Categoria"))
                receiverModel.categoriaDescricao = registro["Categoria"].ToString();

            return receiverModel;
        }

        /// <summary>
        /// MANDA PRO BANCO A PARTIR DO OBJETO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override SqlParameter[] CriaParametros(UserReceiverViewModel model)
        {

            var parametros = new SqlParameter[16];

            parametros[0] = new SqlParameter("user_receiver_id", Convert.ToInt32(model.id));
            parametros[1] = new SqlParameter("username", model.username.ToString().ToLower());
            parametros[2] = new SqlParameter("full_name", model.full_name.ToString());
            parametros[3] = new SqlParameter("fantasy_name", model.fantasy_name.ToString());
            parametros[4] = new SqlParameter("cnpj", model.cnpj.ToString().Replace(".", "").Replace("/", "").Replace("-", ""));
            parametros[5] = new SqlParameter("phone_number", model.phone_number.ToString().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", ""));
            parametros[6] = new SqlParameter("postal_code", model.postal_code.ToString().Replace("-", ""));
            parametros[7] = new SqlParameter("address_street", model.address_street.ToString());
            parametros[8] = new SqlParameter("address_number", model.address_number.ToString());
            
            if (model.address_complement == null)
                parametros[9] = new SqlParameter("address_complement", DBNull.Value);
            else
                parametros[9] = new SqlParameter("address_complement", model.address_complement.ToString());

            parametros[10] = new SqlParameter("address_district", model.address_district.ToString());
            parametros[11] = new SqlParameter("address_city", model.address_city.ToString());
            parametros[12] = new SqlParameter("address_state", model.address_state.ToString());
            parametros[13] = new SqlParameter("email", model.email.ToString());
            parametros[14] = new SqlParameter("created_date", Convert.ToDateTime(model.created_date));
            parametros[15] = new SqlParameter("categoria_lixo", Convert.ToInt32(model.categoriaId));
                
            
            return parametros;
        }


        /// <summary>
        /// INDICA A TABELA DO BANCO
        /// </summary>
        protected override void SetTabela()
        {
            Tabela = "register_receiver_user";
        }
        public override List<UserReceiverViewModel> Listagem()
        {
            List<UserReceiverViewModel> lista = new List<UserReceiverViewModel>();
            string sql = "select r.*, c.categoria_descricao as 'Categoria' from register_receiver_user r left join categorias_lixo_electronicrap c on c.categoria_id = r.categoria_lixo";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }

        public List<UserReceiverViewModel> ListagemFiltrada(string filtraSituacao)
        {
            string sql = "select r.*, c.categoria_descricao as 'Categoria' from register_receiver_user r left join categorias_lixo_electronicrap c on c.categoria_id = r.categoria_lixo where c.categoria_descricao = ";
            List<UserReceiverViewModel> lista = new List<UserReceiverViewModel>();
            if (filtraSituacao == "Todos")
                sql += "'Todos'";
            else if (filtraSituacao == "Equipamentos Refrigeradores")
                sql += "'Equipamentos Refrigeradores'";
            else if (filtraSituacao == "Computadores e Notebooks")
                sql += "'Computadores e Notebooks'";
            else if (filtraSituacao == "Pilhas e Baterias")
                sql += "'Pilhas e Baterias'";
            else if (filtraSituacao == "Televisores e Monitores")
                sql += "'Televisores e Monitores'";
            else
                sql = "select r.*, c.categoria_descricao as 'Categoria' from register_receiver_user r left join categorias_lixo_electronicrap c on c.categoria_id = r.categoria_lixo";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }
    }
}
