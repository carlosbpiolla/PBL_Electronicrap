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
    public class UserReceiverController : PadraoController<UserReceiverViewModel>
    {
        public override IActionResult Index()
        {
            return View("Index");
        }

        public override IActionResult Create()
        {
            ViewBag.Operacao = "I";

            UserReceiverViewModel userReceiver = new UserReceiverViewModel();

            UserReceiverDAO receiverDAO = new UserReceiverDAO();
            userReceiver.id = receiverDAO.ProximoId();
            userReceiver.created_date = DateTime.Today;

            return View("Form", userReceiver);
        }
        public override IActionResult Save(UserReceiverViewModel userReceiver, string Operacao)
        {
            try
            {
                ValidaDados(userReceiver, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    return View("Form", userReceiver);
                }
                else
                {
                    UserReceiverDAO dao = new UserReceiverDAO();
                    if (Operacao == "I")
                    {
                        dao.Insert(userReceiver);
                        string username_parametro = userReceiver.username;
                        return RedirectToAction("CriaNovo", "UserReceiverLogin", new { username_param = username_parametro });
                    }
                    else
                        dao.Update(userReceiver);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        private new void ValidaDados(UserReceiverViewModel userReceiver, string operacao)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            UserReceiverDAO dao = new UserReceiverDAO();
            if (operacao == "I" && dao.Consulta(userReceiver.id) != null)
                ModelState.AddModelError("id", "Código já está em uso.");
            if (operacao == "A" && dao.Consulta(userReceiver.id) == null)
                ModelState.AddModelError("id", "Usuário não existente.");
            if (userReceiver.id <= 0)
                ModelState.AddModelError("id", "Id inválido!");

            if (userReceiver.username != null)
            {
                if (operacao == "I" && dao.ConsultaUsername(userReceiver.username) != null)
                    ModelState.AddModelError("username", "Nome de Usuário já está em uso.");
                if (operacao == "A" && dao.ConsultaUsername(userReceiver.username) == null)
                    ModelState.AddModelError("username", "Usuário não existente.");
            }
            else
                ModelState.AddModelError("username", "Campo obrigatório!");

            ValidaCampoObrigatorio(userReceiver.full_name, "full_name", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.fantasy_name, "fantasy_name", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.phone_number, "phone_number", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.postal_code, "postal_code", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.address_street, "address_street", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.address_number, "address_number", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.address_district, "address_district", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.address_city, "address_city", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.address_state, "address_state", "Campo obrigatório!");
            ValidaCampoObrigatorio(userReceiver.email, "email", "Campo obrigatório!");
            ChecaPreenchimentoCNPJ(dao, userReceiver.cnpj , operacao);

            
        }
        private void ValidaCampoObrigatorio(string campo, string nomeCampo, string mensagem)
        {
            if (string.IsNullOrEmpty(campo))
                ModelState.AddModelError(nomeCampo, mensagem);
        }
        private void ChecaPreenchimentoCNPJ(UserReceiverDAO dao, string cnpj, string operacao)
        {
            if (cnpj == null)
            { ModelState.AddModelError("cnpj", "Campo obrigatório!"); return; }
            else if (ValidateCNPJ(cnpj) != true)
            { ModelState.AddModelError("cnpj", "CNPJ preenchido não é válido."); return; }
            else if (operacao == "I" && dao.Consulta_CNPJ_CPF(cnpj) != null)
            { ModelState.AddModelError("cnpj", "Documento já está em uso."); return; }
            else if (operacao == "A" && dao.Consulta_CNPJ_CPF(cnpj) == null)
            { ModelState.AddModelError("cnpj", "Documento não encontrado."); return; }

        }

        public static bool ValidateCNPJ(string cnpj)
        {
            // Removendo caracteres não numéricos do CNPJ
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            // Verificando se o CNPJ possui 14 dígitos
            if (cnpj.Length != 14)
                return false;
            // Calculando o primeiro dígito verificador
            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores1[i];
            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;
            // Verificando o primeiro dígito verificador
            if (digitoVerificador1 != int.Parse(cnpj[12].ToString()))
                return false;
            // Calculando o segundo dígito verificador
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores2[i];
            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;
            // Verificando o segundo dígito verificador
            if (digitoVerificador2 != int.Parse(cnpj[13].ToString()))
                return false;
            // CNPJ válido
            return true;
        }

        
    }
}
