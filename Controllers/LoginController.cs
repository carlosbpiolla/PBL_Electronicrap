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
        public IActionResult HomeReceiver()
        {
            return View("PagInicialReceiver");
        }
        public IActionResult HomeSender()
        {
            return View("PagInicialSender");
        }
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid == false)  
                {
                    return RedirectToAction("PagInicial");
                }
                else
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
                        return RedirectToAction("PagLogin");
                    }
                    else
                    {
                        if (model.userType == "sender")
                            return View("PagInicialSender");
                        else
                            return View("PagInicialReceiver");
                    }
                        
                }



            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }


    }
}
