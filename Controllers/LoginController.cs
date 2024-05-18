using Microsoft.AspNetCore.Mvc;
using PBL_Electronicrap.Models;
using PBL_Electronicrap.DAO;
using Microsoft.Data.SqlClient;

namespace PBL_Electronicrap.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult PagLogin()
        {
            return View("PagLogin");
        }
        public IActionResult PagInicial()
        {
            return View("PagInicial");
        }

        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                ValidaLogin(model);


            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel( erro.ToString()));
            }
        }

        private void ValidaLogin(LoginViewModel model)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            var p = new SqlParameter[]
                {
                 new SqlParameter("username", model.username.ToLower()),
                 new SqlParameter("password", model.password.ToLower()),
                 new SqlParameter("userType", model.userType)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_Username", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return RedirectToAction("PagInicial");

        }
    }
}
