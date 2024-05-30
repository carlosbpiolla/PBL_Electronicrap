using Microsoft.AspNetCore.Mvc;
using PBL_Electronicrap.Models;
using PBL_Electronicrap.DAO;
using Microsoft.Data.SqlClient;

namespace PBL_Electronicrap.Controllers
{
    public class CategoriaLixoController : PadraoController<CategoriaLixoViewModel>
    {
        public CategoriaLixoController()
        {
            DAO = new CategoriaLixoDAO();
            GeraProximoId = true;
        }
        protected override void ValidaDados(CategoriaLixoViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);
            if (string.IsNullOrEmpty(model.categoria))
                ModelState.AddModelError("categoria", "Preencha o nome da categoria.");
        }
    }
}
