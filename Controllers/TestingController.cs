using Memo.Models;
using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Memo.Controllers
{
    public class TestingController : Controller
    {
        // GET: Testing
        public ActionResult Details(int? Ids)
        {
            Picture pict = new Picture();

            pict.pictureId = 1;
            pict.pictureTitle = "lake";
            pict.pictureDescription = "a lake";
            pict.pictureLocationUrl = "/lac.jpg";
            pict.pictureRatingValue = 5;
            pict.pictureViewsNumber = 10;

            //Image pic = new Bitmap("C:\\Users\\s.pouwels\\Desktop\\Memento\\Memo\\Memo\\lac.jpg");

            //PropertyItem[] propItems = pic.PropertyItems;

            //var dir = ImageMetadataReader.ReadMetadata("C:\\Users\\s.pouwels\\Desktop\\Memento\\Memo\\Memo\\lac.jpg");

            Ids = pict.pictureId;

            return View(pict);
        }

        //public ActionResult Connect()
        //{
        //    Identify userConnect = new Identify { loginValue = "toto", passValue = "toto" };

        //    return View(userConnect);

        //}
        [HttpGet] // 1er appel de la méthode - On indique GET.
        public ActionResult Connect()
        {
            return View();
        }
        [HttpPost] // On soumet le formulaire - On indique POST.
        public ActionResult Connect(Identify userConnect)
        {
            if(userConnect.testConnectionOk())
            {
                return View("Connexion");
            }
                return View("Connect");
        }
        public ActionResult Connexion()
        {
            return View();
        }
    }
}