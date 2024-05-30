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
    public class CategoriaLixoDAO : PadraoDAO<CategoriaLixoViewModel>
    {
        public List<CategoriaLixoViewModel> ListaCategorias()
        {
            List<CategoriaLixoViewModel> lista = new List<CategoriaLixoViewModel>();
            DataTable tabela = HelperDAO.ExecutaProcSelect("spListagemCategoriasLixo", null);
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));
            return lista;
        }
        protected override CategoriaLixoViewModel MontaModel(DataRow registro)
        {
            CategoriaLixoViewModel c = new CategoriaLixoViewModel()
            {
                id = Convert.ToInt32(registro["categoria_id"]),
                categoria = registro["categoria_descricao"].ToString()
            };
            return c;
        }
        protected override SqlParameter[] CriaParametros(CategoriaLixoViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("categoria_id", model.id),
                new SqlParameter("categoria_descricao", model.categoria)
            };
            return parametros;
        }
        protected override void SetTabela()
        {
            Tabela = "categorias_lixo_electronicrap";
        }
    }
}
