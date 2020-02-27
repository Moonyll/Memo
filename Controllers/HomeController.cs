using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using ModellsUp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModellsUp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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

                GetExifs(path);

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
            pictureExifs.pictureCameraMake = (string.IsNullOrEmpty(subIfd0Directory?.GetDescription(ExifDirectoryBase.TagMake))) ? "---" : "data";

            // Get the camera model :
            pictureExifs.pictureCameraModel = (string.IsNullOrEmpty(subIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel))) ? "---" : "data";

            // Get original date time :
            pictureExifs.pictureOriginalDateTime = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal))) ? "---" : "data";

            // Get aperture value :
            pictureExifs.pictureApertureValue = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagAperture))) ? "---" : "data";

            // Get exposure time :
            pictureExifs.pictureExposureTime = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExposureTime))) ? "---" : "data";

            // Get iso speed ratings :
            pictureExifs.pictureIsoSpeedRatings = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagIsoEquivalent))) ? "---" : "data";

            // Get picture flash :
            pictureExifs.pictureFlash = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFlash))) ? "---" : "data";

            // Get focal length :
            pictureExifs.pictureFocalLength = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFocalLength))) ? "---" : "data";

            // Get picture width :
            pictureExifs.pictureWidth = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageWidth))) ? "---" : "data";

            // Get picture height :
            pictureExifs.pictureHeight = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageHeight))) ? "---" : "data";

            // Get picture file size :
            pictureExifs.pictureFileSize = (string.IsNullOrEmpty(subMetadataDirectory?.GetDescription(FileMetadataDirectory.TagFileSize))) ? "---" : "data";


            return pictureExifs;
        }

        // To do recurrent method :
        public string GetExifData(MetadataExtractor.Directory dir, int tag)
        {
            var data = dir?.GetDescription(tag);

            return data;
        }
    }

}
