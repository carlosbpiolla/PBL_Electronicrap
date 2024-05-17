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
    public class UserSenderLoginController : PadraoController<UserSenderLoginViewModel>
    {
        public override IActionResult Index()
        {
            return View("Index");
        }

        public override IActionResult CriaNovo(string username_param)
        {
            ViewBag.Operacao = "I";

            UserSenderLoginViewModel senderLogin = new UserSenderLoginViewModel();

            UserSenderLoginDAO senderDAO = new UserSenderLoginDAO();
            senderLogin.id = senderDAO.ProximoId();
            senderLogin.username = username_param;
            return View("Form", senderLogin);
        }
        public override IActionResult Save(UserSenderLoginViewModel senderLogin, string Operacao)
        {
            try
            {
                ValidaDados(senderLogin, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    return View("Form", senderLogin);
                }
                else
                {
                    UserSenderLoginDAO dao = new UserSenderLoginDAO();
                    if (Operacao == "I")
                        dao.Insert(senderLogin);
                    else
                        dao.Update(senderLogin);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private new void ValidaDados(UserSenderLoginViewModel receiverLogin, string operacao)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            UserSenderLoginDAO dao = new UserSenderLoginDAO();
            if (operacao == "I" && dao.Consulta(receiverLogin.id) != null)
                ModelState.AddModelError("id", "Código já está em uso.");
            if (operacao == "A" && dao.Consulta(receiverLogin.id) == null)
                ModelState.AddModelError("id", "Usuário não existente.");
            if (receiverLogin.id <= 0)
                ModelState.AddModelError("id", "Id inválido!");
        }
    }
}
