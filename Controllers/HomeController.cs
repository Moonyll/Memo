using System.IO;
using System.Web;
using System.Web.Mvc;
using Memo.Models;
using System.Drawing.Imaging;
using System.Drawing;
using MetadataExtractor;

namespace Memo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet] // 1er appel de la méthode - On indique GET.
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

        [HttpGet]
        public JsonResult ShowPicture()
        {
            Picture pict = new Picture();

            pict.pictureId = 1;
            pict.pictureTitle = "lake";
            pict.pictureDescription = "a lake";
            pict.pictureLocationUrl = "/lac.jpg";
            pict.pictureRatingValue = 5;
            pict.pictureViewsNumber = 10;

            Image pic = new Bitmap("/lac.jpg");

            PropertyItem[] propItems = pic.PropertyItems;

            var dir = ImageMetadataReader.ReadMetadata("/lac.jpg");

            return Json(pict, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Test()
        //{
        //    Picture pict = new Picture();

        //    pict.pictureId = 1;
        //    pict.pictureTitle = "lake";
        //    pict.pictureDescritption = "a lake";
        //    pict.pictureLocationUrl = "/lac.jpg";
        //    pict.pictureRatingValue = 5;
        //    pict.pictureViewsNumber = 10;

        //    Image pic = new Bitmap("C:\\Users\\s.pouwels\\Desktop\\Memento\\Memo\\Memo\\lac.jpg");

        //    PropertyItem[] propItems = pic.PropertyItems;

        //    var dir = ImageMetadataReader.ReadMetadata("C:\\Users\\s.pouwels\\Desktop\\Memento\\Memo\\Memo\\lac.jpg");

        //    return View(pict);
        //}
        public ActionResult Index2(string id)
        {
            id = "10";
            return Content(id);
        }

        public ActionResult Form()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetData(string test)
        {
            return Json("Successfully get method executed.", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PostData(string test)
        {
            return Json("Successfully post method executed.");
        }

        [HttpPost]
        public ActionResult PostTitle(string pictureTitle)
        {
            try
            {
                if (pictureTitle != null)
                {
                    return Json(new { success = true, message = "It's Okay !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, message = "It's Okay !" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { error = "KO" }, JsonRequestBehavior.AllowGet);
            }  

        }
    }
}