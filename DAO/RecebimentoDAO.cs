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
    public class RecebimentoDAO : PadraoDAO<RecebimentoViewModel>
    {
        protected override RecebimentoViewModel MontaModel(DataRow registro)
        {
            RecebimentoViewModel recebimento = new RecebimentoViewModel()
            {
                id = Convert.ToInt32(registro["recebimento_id"]),
                quantidade = registro["quantidade_recebimento"].ToString(),
                descricao = registro["descricao_recebimento"].ToString(),
                sender_id = Convert.ToInt32(registro["fk_user_sender_id"]),
                receiver_id = Convert.ToInt32(registro["fk_user_receiver_id"]),
                categoria_lixo = Convert.ToInt32(registro["fk_categoria_lixo"]),
                data = Convert.ToDateTime(registro["data_recebimento"])
            };
            if (registro.Table.Columns.Contains("Categoria"))
                recebimento.categoria_lixo_descricao = registro["Categoria"].ToString();
            if (registro.Table.Columns.Contains("CPF Descartador"))
                recebimento.sender_cpf = registro["CPF Descartador"].ToString();
            if (registro.Table.Columns.Contains("Nome Descartador"))
                recebimento.sender_full_name = registro["Nome Descartador"].ToString();
            return recebimento;
        }
        protected override SqlParameter[] CriaParametros(RecebimentoViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("recebimento_id", model.id),
                new SqlParameter("quantidade_recebimento", model.quantidade),
                new SqlParameter("descricao_recebimento", model.descricao),
                new SqlParameter("fk_user_sender_id", model.sender_id),
                new SqlParameter("fk_user_receiver_id", model.receiver_id),
                new SqlParameter("fk_categoria_lixo", model.categoria_lixo),
                new SqlParameter("data_recebimento", model.data)

            };
            return parametros;
        }
        protected override void SetTabela()
        {
            Tabela = "recebimentos_electronicrap";
        }

        public List<RecebimentoViewModel> ListagemRecebimentos(int id)
        {
            List<RecebimentoViewModel> lista = new List<RecebimentoViewModel>();
            string sql = $"select r.*, c.categoria_descricao as 'Categoria', s.cpf as 'CPF Descartador', s.full_name as 'Nome Descartador' from recebimentos_electronicrap r left join categorias_lixo_electronicrap c on c.categoria_id = r.fk_categoria_lixo left join register_sender_user s on s.user_sender_id = r.fk_user_sender_id  where r.fk_user_receiver_id = {id}";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }
    }
}
