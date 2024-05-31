using PBL_Electronicrap.Models;
using PBL_Electronicrap.Controllers;
using PBL_Electronicrap.DAO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PBL_Electronicrap.Controllers
{
    public class UserReceiverLoginController : PadraoController<UserReceiverLoginViewModel>
    {
        public override IActionResult Index()
        {
            return View("Index");
        }

        public override IActionResult CriaNovo(string username_param)
        {
            ViewBag.Operacao = "I";

            UserReceiverLoginViewModel receiverLogin = new UserReceiverLoginViewModel();
            
            UserReceiverLoginDAO receiverDAO = new UserReceiverLoginDAO();
            receiverLogin.id = receiverDAO.ProximoId();
            receiverLogin.username= username_param;
            return View("Form", receiverLogin);
        }
        public override IActionResult Save(UserReceiverLoginViewModel receiverLogin, string Operacao)
        {
            try
            {
                ValidaDados(receiverLogin, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    return View("Form", receiverLogin);
                }
                else
                {
                    UserReceiverLoginDAO dao = new UserReceiverLoginDAO();
                    if (Operacao == "I")
                        dao.Insert(receiverLogin);
                    else
                        dao.Update(receiverLogin);
                    return RedirectToAction("Index", "UserReceiver");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private new void ValidaDados(UserReceiverLoginViewModel receiverLogin, string operacao)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            UserReceiverLoginDAO dao = new UserReceiverLoginDAO();
            if (operacao == "I" && dao.Consulta(receiverLogin.id) != null)
                ModelState.AddModelError("id", "Código já está em uso.");
            if (operacao == "A" && dao.Consulta(receiverLogin.id) == null)
                ModelState.AddModelError("id", "Usuário não existente.");
            if (receiverLogin.id <= 0)
                ModelState.AddModelError("id", "Id inválido!");
        }
    }
}
