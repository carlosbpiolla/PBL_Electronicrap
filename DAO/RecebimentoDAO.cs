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
            RecebimentoViewModel c = new RecebimentoViewModel()
            {
                id = Convert.ToInt32(registro["recebimento_id"]),
                quantidade = registro["quantidade_recebimento"].ToString(),
                descricao = registro["descricao_recebimneto"].ToString(),
                sender_id = Convert.ToInt32(registro["fk_user_sender_id"]),
                receiver_id = Convert.ToInt32(registro["fk_user_receiver_id"]),
                categoria_lixo = Convert.ToInt32(registro["fk_categoria_lixo"])
            };
            return c;
        }
        protected override SqlParameter[] CriaParametros(RecebimentoViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("recebimento_id", model.id),
                new SqlParameter("quantidade_recebimento", model.quantidade),
                new SqlParameter("descricao_recebimneto", model.descricao),
                new SqlParameter("fk_user_sender_id", model.sender_id),
                new SqlParameter("fk_user_receiver_id", model.receiver_id),
                new SqlParameter("fk_categoria_lixo", model.categoria_lixo)

            };
            return parametros;
        }
        protected override void SetTabela()
        {
            Tabela = "recebimentos_electronicrap";
        }
    }
}
