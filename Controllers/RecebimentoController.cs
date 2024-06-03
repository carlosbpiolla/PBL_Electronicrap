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
    public class RecebimentoController : PadraoController<RecebimentoViewModel>
    {
        public IActionResult Inicio(string username)
        {
            UserReceiverDAO dao = new UserReceiverDAO();
            UserReceiverViewModel receiver = dao.ConsultaUsername(username);

            ViewBag.receiverId = receiver.id;
            ViewBag.receiverUsername = username;
            RecebimentoDAO recebDAO = new RecebimentoDAO();
            List<RecebimentoViewModel> lista = recebDAO.ListagemRecebimentos(receiver.id);
            return View("Index", lista);
        }

        public IActionResult Cria(int receiverId, string username)
        {
            ViewBag.Operacao = "I";
            ViewBag.receiverUsername = username;
            RecebimentoViewModel recebimento = new RecebimentoViewModel();
            PreparaListaCategoriasParaCombo();
            RecebimentoDAO recebimentoDAO = new RecebimentoDAO();
            recebimento.id = recebimentoDAO.ProximoId();
            recebimento.receiver_id = receiverId;
            return View("Form", recebimento);
        }

        public override IActionResult Save(RecebimentoViewModel recebimento, string Operacao)
        {
            try
            {
                //ValidaDados(recebimento, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreparaListaCategoriasParaCombo();
                    return View("Form", recebimento);
                }
                else
                {
                    RecebimentoDAO dao = new RecebimentoDAO();
                    if (Operacao == "I")
                    {
                        dao.Insert(recebimento);
                        return RedirectToAction("Inicio");
                    }
                    else
                        dao.Update(recebimento);
                    return RedirectToAction("Inicio");
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Excluir(string username, int id)
        {
            try
            {
                string name = username;
                RecebimentoDAO dao = new RecebimentoDAO();
                dao.Delete(id);
                return RedirectToAction("Inicio", new {username = name});
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void PreparaListaCategoriasParaCombo()
        {
            CategoriaLixoDAO categoriaDAO = new CategoriaLixoDAO();
            var categorias = categoriaDAO.ListaCategorias();
            List<SelectListItem> listaCategorias = new List<SelectListItem>
            {
                new SelectListItem("Selecione uma categoria...", "0")
            };
            foreach (var categ in categorias)
            {
                SelectListItem item = new SelectListItem(categ.categoria, categ.id.ToString());
                listaCategorias.Add(item);
            }
            ViewBag.Categorias = listaCategorias;
        }

        private new void ValidaDados(RecebimentoViewModel recebimento, string operacao)
        {
            
            ModelState.Clear(); // limpa os erros criados automaticamente pelo Asp.net (que podem estar com msg em inglês)
            UserSenderDAO dao = new UserSenderDAO();
            
            if (recebimento.sender_cpf != null)
            {
                ChecaPreenchimentoCPF(dao, recebimento.sender_cpf, operacao);
                if (dao.Consulta_CNPJ_CPF(recebimento.sender_cpf) == null)
                    ModelState.AddModelError("sender_cpf", "CPF não cadastrado no sistema.");
            }
            else
                ModelState.AddModelError("sender_cpf", "Campo obrigatório!");

            ValidaCampoObrigatorio(recebimento.sender_full_name, "sender_full_name", "Campo obrigatório!");
            ValidaCampoObrigatorio(recebimento.quantidade, "quantidade", "Campo obrigatório!");
            ValidaCampoObrigatorio(recebimento.descricao, "descricao", "Campo obrigatório!");
            ValidaCampoObrigatorio(recebimento.data.ToString(), "data", "Campo obrigatório!");
            
            
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
