﻿using PBL_Electronicrap.Models;
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

            RecebimentoDAO recebDAO = new RecebimentoDAO();
            List<RecebimentoViewModel> lista = recebDAO.ListagemRecebimentos(receiver.id);
            return View("Index", lista);
        }

        public IActionResult Cria(int receiverId)
        {
            ViewBag.Operacao = "I";

            RecebimentoViewModel recebimento = new RecebimentoViewModel();
            PreparaListaCategoriasParaCombo();
            RecebimentoDAO recebimentoDAO = new RecebimentoDAO();
            recebimento.id = recebimentoDAO.ProximoId();
            recebimento.receiver_id = receiverId;
            return View("Form", recebimento);
        }

        public override IActionResult Save(RecebimentoViewModel userReceiver, string Operacao)
        {
            try
            {
                ValidaDados(userReceiver, Operacao);
                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreparaListaCategoriasParaCombo();
                    return View("Form", userReceiver);
                }
                else
                {
                    RecebimentoDAO dao = new RecebimentoDAO();
                    if (Operacao == "I")
                    {
                        dao.Insert(userReceiver);
                        return RedirectToAction("");
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
    }
}