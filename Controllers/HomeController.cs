using System.IO;
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

        public ActionResult Index(Operation calculate)
        {
            var x = calculate.x; // 1er nombre
            var y = calculate.y; // 2nd nombre
            var op = calculate.op; // Opérateur
            calculate.Calculate(); // Calcul
            ViewBag.result = calculate.result; // Affichage du résultat
            return View();
        }
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Test(Number nb)
        {
            var nb1 = nb.a;
            ViewBag.number = nb1;
            return View(nb);
        }

        public ActionResult FileUpload(HttpPostedFileBase pictureToUpload)
        {
            if (pictureToUpload != null)
            {
                string pic = Path.GetFileName(pictureToUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/"), pic);
                // file is uploaded
                pictureToUpload.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureToUpload.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return RedirectToAction("Test", "Home");
        }

    }
}