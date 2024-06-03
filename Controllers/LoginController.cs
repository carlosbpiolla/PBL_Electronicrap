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
        public IActionResult HomeReceiver(string username)
        {
            LoginDAO dao = new LoginDAO();
            LoginViewModel model = dao.ConsultaLoginUsername(username, "receiver");

            return View("PagInicialReceiver", model);
        }
        public IActionResult HomeSender(string username_parameter)
        {
            LoginDAO dao = new LoginDAO();
            LoginViewModel model = dao.ConsultaLoginUsername(username_parameter, "sender");

            return View("PagInicialSender", model);
        }
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                ValidaDados(model);
                if (ModelState.IsValid == false)  
                {
                    return View("PagLogin",model);
                }
                else
                {
                    if (model.userType == "sender")
                        return View("PagInicialSender", model);
                    else
                        return View("PagInicialReceiver", model);

                }



            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void ValidaDados(LoginViewModel model)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            var p = new SqlParameter[]
                    {
                         new SqlParameter("username", model.username.ToLower()),
                         new SqlParameter("password", model.password.ToLower()),
                         new SqlParameter("userType", model.userType)
                    };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_Login", p);
            if (tabela.Rows.Count == 0)
            {
                ModelState.AddModelError("username", "Usuário ou senha inválidos");
            }
        }
    }
}
