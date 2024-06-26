﻿using Microsoft.AspNetCore.Mvc;
using PBL_Electronicrap.Models;
using System.Diagnostics;

namespace PBL_Electronicrap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult PagCriaConta()
        {
            return View("PagCriaConta");
        }
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}