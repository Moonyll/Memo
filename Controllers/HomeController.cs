using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using ModellsUp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var pictureExifs = new pictureExifMetaData();

            if (pictureToUpload != null)
            {
                string pic = Path.GetFileName(pictureToUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/"), pic);
                // file is uploaded
                pictureToUpload.SaveAs(path);

                pictureExifs = GetExifs(path);

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
            return View("Index", pictureExifs);
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
            pictureExifs.pictureCameraMake = (string.IsNullOrEmpty(subIfd0Directory?.GetDescription(ExifDirectoryBase.TagMake))) ? "---" : subIfd0Directory?.GetDescription(ExifDirectoryBase.TagMake);

            // Get the camera model :
            pictureExifs.pictureCameraModel = (string.IsNullOrEmpty(subIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel))) ? "---" : subIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel);

            // Get original date time :

            var dt =subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

            Regex rgx1 = new Regex(@"\d{2}-\d{2}-\d{4}");
            Regex rgx2 = new Regex(@"\d{2}:\d{2}:\d{4}");
            Regex rgx3 = new Regex(@"\d{4}-\d{2}-\d{2}");
            Regex rgx4 = new Regex(@"\d{4}:\d{2}:\d{2}");

            Regex rgx5 = new Regex(@" \d{2}:\d{2}:\d{2}");
           
            // Date :
            var mt1 = rgx1.Match(dt).ToString();
            var mt2 = rgx2.Match(dt).ToString();
            var mt3 = rgx3.Match(dt).ToString();
            var mt4 = rgx4.Match(dt).ToString();

            var mtd = mt4.ToString().Replace(":","/");

            // Time :
            //Match mt5 = rgx5.Match(dt)



            var dtNB = string.Join("", dt.ToCharArray().Where(Char.IsDigit));


            //DateTime dateTime = DateTime.Parse(date);

            //var dt = String.Format("{0:d/M/yyyy HH:mm:ss}", date);


            // Get aperture value :
            pictureExifs.pictureApertureValue = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagAperture))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagAperture);

            // Get exposure time :
            pictureExifs.pictureExposureTime = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExposureTime))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExposureTime);

            // Get iso speed ratings :
            pictureExifs.pictureIsoSpeedRatings = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagIsoEquivalent))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagIsoEquivalent);

            // Get picture flash :
            pictureExifs.pictureFlash = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFlash))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFlash);

            // Get focal length :
            pictureExifs.pictureFocalLength = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFocalLength))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagFocalLength);

            // Get picture width :
            pictureExifs.pictureWidth = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageWidth))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageWidth);

            // Get picture height :
            pictureExifs.pictureHeight = (string.IsNullOrEmpty(subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageHeight))) ? "---" : subIfdDirectory?.GetDescription(ExifDirectoryBase.TagExifImageHeight);

            // Get picture file size :
            pictureExifs.pictureFileSize = (string.IsNullOrEmpty(subMetadataDirectory?.GetDescription(FileMetadataDirectory.TagFileSize))) ? "---" : subMetadataDirectory?.GetDescription(FileMetadataDirectory.TagFileSize);


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
