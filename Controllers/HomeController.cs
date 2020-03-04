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
            var datetimeString = GetExifData(subIfdDirectory, ExifDirectoryBase.TagDateTimeOriginal);

            var picutreDateTimeValues = (!string.IsNullOrEmpty(datetimeString)) ?
                                            GetDateTimeValues(datetimeString) :
                                            new string[]
                                            {
                                                pictureControls.EmptyValue,
                                                pictureControls.EmptyValue
                                            };

            pictureExifs.pictureOriginalDateTime = pictureControls.DateLabel + picutreDateTimeValues[0] + "   " + pictureControls.TimeLabel + picutreDateTimeValues[1];

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

        public string TestDateTimeString(Regex patternFormat, string datetimeString)

        {
            if (string.IsNullOrEmpty(datetimeString))
            {
             return null;
            }

            var stringFromRegexMatching = patternFormat.Match(datetimeString).ToString();

            return stringFromRegexMatching;
        }

        public string [] GetDateTimeValues(string datetimeString)
        {
            string[] datetimeArrayValues = new string[2] { "date", "time" };

            Dictionary<string, string> datetimeDico = new Dictionary<string, string>()
                    {
                        {"testDateFormatA", TestDateTimeString(pictureControls.PatternOrigDtFA, datetimeString)},
                        {"testDateFormatB", TestDateTimeString(pictureControls.PatternOrigDtFB, datetimeString)},
                        {"testDateFormatC", TestDateTimeString(pictureControls.PatternOrigDtFC, datetimeString)},
                        {"testDateFormatD", TestDateTimeString(pictureControls.PatternOrigDtFD, datetimeString)},
                        {"testTimeFormatA", TestDateTimeString(pictureControls.PatternOrigTmFA, datetimeString)}
                    };

            // Get date & time expressions values from dico :
            var datetimeValues = datetimeDico.Values.Where(value => value.Length > 0).OrderByDescending(x => x.Length).ToList();

            // 1. Manage date value ;
            var dateValue = datetimeValues.ElementAt(0);

            if (dateValue.Contains(":"))
            {
                dateValue = dateValue.Replace(":", "/");
            }
            if (dateValue.Contains("-"))
            {
                dateValue = dateValue.Replace("-", "/");
            }

            DateTime dateValueFormated;

            bool isDateValueBeenFormated = DateTime.TryParse(dateValue, out dateValueFormated);

            var finalDateValue = (isDateValueBeenFormated) ? dateValueFormated.ToString("dd/MM/yyyy") : dateValue.ToString();

            // 2. Manage time value :

            var timeValue = datetimeValues.ElementAt(1);

            DateTime timeValueFormated;

            bool isTimeValueBeenFormated = DateTime.TryParse(timeValue, out timeValueFormated);

            var finalTimeValue = (isTimeValueBeenFormated) ? timeValueFormated.ToString("hh:mm:ss") : timeValue.ToString();

            // Fill the array :
            datetimeArrayValues[0] = finalDateValue;
            datetimeArrayValues[1] = finalTimeValue;

            return datetimeArrayValues;

        }

    }

}
