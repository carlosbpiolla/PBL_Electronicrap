using PBL_Electronicrap.Models;
using PBL_Electronicrap.DAO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PBL_Electronicrap.Controllers
{
    public class UserSenderController : PadraoController<UserSenderViewModel>
    {
        

        public override IActionResult Index()
        {
            return View("Index");
        }

        public override IActionResult Create()
        {
            ViewBag.Operacao = "I";

            UserSenderViewModel userSender = new UserSenderViewModel();

            UserSenderDAO senderDAO = new UserSenderDAO();
            userSender.id = senderDAO.ProximoId();
            userSender.created_date = DateTime.Today;

            return View("Form", userSender);
        }
        public override IActionResult Save(UserSenderViewModel userSender, string Operacao)
        {
            try
            {
                ValidaDados(userSender, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    return View("Form", userSender);
                }
                else
                {
                    UserSenderDAO dao = new UserSenderDAO();
                    if (Operacao == "I")
                    {
                        string username_parametro = userSender.username;
                        dao.Insert(userSender);
                        return RedirectToAction("CriaNovo", "UserSenderLogin", new { username_param = username_parametro });
                    }
                        
                    else
                        dao.Update(userSender);
                    return RedirectToAction("index");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        private new void ValidaDados(UserSenderViewModel userSender, string operacao)
        {
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            UserSenderDAO dao = new UserSenderDAO(); 
            if (operacao == "I" && dao.Consulta(userSender.id) != null)
                ModelState.AddModelError("id", "Código já está em uso.");
            if (operacao == "A" && dao.Consulta(userSender.id) == null)
                ModelState.AddModelError("id", "Usuário não existente.");
            if (userSender.id <= 0)
                ModelState.AddModelError("id", "Id inválido!");
            
            if (userSender.username != null)
            {
                if (operacao == "I" && dao.ConsultaUsername(userSender.username) != null)
                    ModelState.AddModelError("username", "Nome de Usuário já está em uso.");
                if (operacao == "A" && dao.ConsultaUsername(userSender.username) == null)
                    ModelState.AddModelError("username", "Usuário não existente.");
            }
            else
                ModelState.AddModelError("username", "Campo obrigatório!");
            

            ValidaCampoObrigatorio(userSender.full_name, "full_name", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.phone_number, "phone_number", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.postal_code, "postal_code", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.address_street, "address_street", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.address_number, "address_number", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.address_district, "address_district", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.address_city, "address_city", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.address_state, "address_state", "Campo obrigatório!");
            ValidaCampoObrigatorio(userSender.email, "email", "Campo obrigatório!");
            ChecaPreenchimentoCPF(dao, userSender.cpf, operacao);

        }
        private void ValidaCampoObrigatorio(string campo, string nomeCampo, string mensagem)
        {
            if (string.IsNullOrEmpty(campo))
                ModelState.AddModelError(nomeCampo, mensagem);
        }
        private void ChecaPreenchimentoCPF(UserSenderDAO dao, string cpf, string operacao)
        {
            if (cpf == null)
            { ModelState.AddModelError("cpf", "Campo obrigatório!"); return; }
            else if (ValidateCPF(cpf) != true)
            { ModelState.AddModelError("cpf", "CPF preenchido não é válido."); return; }
            else if (operacao == "I" && dao.Consulta_CNPJ_CPF(cpf) != null)
            { ModelState.AddModelError("cpf", "Documento já está em uso."); return; }
            else if (operacao == "A" && dao.Consulta_CNPJ_CPF(cpf) == null)
            { ModelState.AddModelError("cpf", "Documento não encontrado."); return; }

        }

        /// <summary>
        /// VERIFICA SE O CPF INSERIDO É VALIDO
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool ValidateCPF(string cpf)
        {
            // Removendo caracteres não numéricos do CPF
            cpf = cpf.Replace(".", "").Replace("-", "");

            // Verificando se o CPF possui 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verificando se todos os dígitos são iguais (caso contrário, não é um CPF válido)
            bool todosIguais = true;
            for (int i = 1; i < cpf.Length; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    todosIguais = false;
                    break;
                }
            }
            if (todosIguais)
                return false;

            // Calculando o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            // Verificando o primeiro dígito verificador
            if (digitoVerificador1 != int.Parse(cpf[9].ToString()))
                return false;

            // Calculando o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            // Verificando o segundo dígito verificador
            if (digitoVerificador2 != int.Parse(cpf[10].ToString()))
                return false;

            // CPF válido
            return true;
        }

    }
}
