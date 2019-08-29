using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Memo.Models;
namespace Memo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet] // 1er appel de la méthode - on indique GET
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost] // On soumet le formulaire - On indique POST.
        public ActionResult Index(float nb1, float nb2, string operand)
        {
            Operation calculate = new Operation();
            calculate.x = nb1; // 1er nombre
            calculate.y = nb2; // 2nd nombre
            calculate.op = operand; // Opérateur
            calculate.Calculate(); // Calcul
            ViewBag.result = calculate.result; // Affichage du résultat
            return View();
        }
    }
}