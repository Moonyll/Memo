using System.IO;
using System.Web;
using System.Web.Mvc;
using Memo.Models;
using System.Drawing.Imaging;
using System.Drawing;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using System.Collections.Generic;
using System.Linq;

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
        public ActionResult PostTitle(Picture picture)
        {

            var pictureTitle = picture.pictureTitle;
            //try
            //{
            //    if (pictureTitle != null)
            //    {
            //        return Json(new { success = true, message = "It's Okay !" }, JsonRequestBehavior.AllowGet);
            //    }
            //    return Json(new { success = true, message = "It's Okay !" }, JsonRequestBehavior.AllowGet);
            //}
            //catch
            //{
            //    return Json(new { error = "KO" }, JsonRequestBehavior.AllowGet);
            //}
            return View();

        }

        // Get Exifs Informations :
        public pictureExifMetaData GetExifs(string pictureFile)
        {

            pictureExifMetaData pictureExifs = new pictureExifMetaData();

            // Retrieve directories of picture file properties :
            var pictureDirectories = ImageMetadataReader.ReadMetadata(pictureFile).ToList();

            // 1° Read directories metadata files :
    
                // Read "Exif IFD0" directory file :
                var subIfd0Directory = pictureDirectories.OfType<ExifIfd0Directory>().FirstOrDefault();
    
                // Read "Exif SubIFD" directory file :
                var subIfdDirectory = pictureDirectories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
    
                // Read "MetadataDirectory" directory file :
                var subMetadataDirectory = pictureDirectories.OfType<FileMetadataDirectory>().FirstOrDefault();

            
            // 2° Get Exifs data from read file :
            
                // Get the camera make :
                pictureExifs.pictureCameraMake = subIfd0Directory?.GetDescription(ExifDirectoryBase.TagMake);
    
                // Get the camera model :
                pictureExifs.pictureCameraModel = subIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel);
    
                // Get original date time :
                pictureExifs.pictureOriginalDateTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
    
                // Get aperture value :
                pictureExifs.pictureApertureValue = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagAperture);
    
                // Get exposure time :
                pictureExifs.pictureExposureTime = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExposureTime);
    
                // Get iso speed ratings :
                pictureExifs.pictureIsoSpeedRatings = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagIsoEquivalent);

                // Get picture flash :
                pictureExifs.pictureFlash = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFlash);

                // Get focal length :
                pictureExifs.pictureFocalLength = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFocalLength);
    
                // Get picture width :
                pictureExifs.pictureWidth = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageWidth);
    
                // Get picture height :
                pictureExifs.pictureHeight = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageHeight);

                // Get picture file size :
                pictureExifs.pictureFileSize = subMetadataDirectory?.GetDescription(FileMetadataDirectory.TagFileSize);


            return pictureExifs;
        }
}