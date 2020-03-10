using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileSystem;
using ModellsUp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

                var titi = WebUtility.UrlEncode(path);

                Uri toto = new Uri(path);

                WebRequest wr = WebRequest.Create(toto);


            }
            // after successfully uploading redirect the user
            return View("Index", pictureExifs);
        }

        #region EXIFS Management

        /// <summary>
        /// Get Exifs Informations.
        /// </summary>
        /// <param name="pictureFile"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetExifs(string pictureFile)
        {
            pictureExifMetaData pictureExifs = new pictureExifMetaData();

            // Retrieve picture file path :
            var pathPictureFile = Server.MapPath(pictureFile);

            // Check if file path exists :
            if (System.IO.File.Exists(pathPictureFile))
            {

                // Retrieve directories of picture file properties :
                var pictureDirectories = ImageMetadataReader
                                        .ReadMetadata(pathPictureFile)
                                        .ToList();

                // 1° Read directories metadata files :

                // Read "Exif IFD0" directory file :
                var subIfd0Directory = pictureDirectories
                                       .OfType<ExifIfd0Directory>()
                                       .FirstOrDefault();

                // Read "Exif SubIFD" directory file :
                var subIfdDirectory = pictureDirectories
                                      .OfType<ExifSubIfdDirectory>()
                                      .FirstOrDefault();

                // Read "MetadataDirectory" directory file :
                var subMetadataDirectory = pictureDirectories
                                           .OfType<FileMetadataDirectory>()
                                           .FirstOrDefault();

                // 2° Get Exifs data from read file :

                // Get the camera make :
                pictureExifs.pictureCameraMake = GetExifData
                                                 (
                                                    subIfd0Directory,
                                                    ExifDirectoryBase.TagMake
                                                 );
                // Get the camera model :
                pictureExifs.pictureCameraModel = GetExifData
                                                  (
                                                    subIfd0Directory,
                                                    ExifDirectoryBase.TagModel
                                                  );
                // Get original date time :
                var datetimeString = GetExifData
                                     (
                                        subIfdDirectory,
                                        ExifDirectoryBase.TagDateTimeOriginal
                                     );

                var picutreDateTimeValues = (!string.IsNullOrEmpty(datetimeString)) ?
                                                  GetDateTimeValues(datetimeString) :
                                                  new string[]
                                                  {
                                                    pictureExifMetaData.EmptyValue,
                                                    pictureExifMetaData.EmptyValue
                                                  };

                pictureExifs.pictureOriginalDateTime = pictureExifMetaData.SpaceTabulation +
                                                       picutreDateTimeValues[0] +
                                                       pictureExifMetaData.SpaceTabulation +
                                                       picutreDateTimeValues[1];

                // Get aperture value :
                pictureExifs.pictureApertureValue = GetExifData
                                                    (
                                                        subIfdDirectory,
                                                        ExifDirectoryBase.TagAperture
                                                    );
                // Get exposure time :
                pictureExifs.pictureExposureTime = GetExifData
                                                   (
                                                        subIfdDirectory,
                                                        ExifDirectoryBase.TagExposureTime
                                                   );
                // Get iso speed ratings :

                var isoSpeedValues = GetExifData
                                     (
                                        subIfdDirectory,
                                        ExifDirectoryBase.TagIsoEquivalent
                                     );
                
                pictureExifs.pictureIsoSpeedRatings = (isoSpeedValues != pictureExifMetaData.TabEmpty) ?
                                                              isoSpeedValues + pictureExifMetaData.ISO :
                                                              isoSpeedValues;
                // Get picture flash :
                pictureExifs.pictureFlash = GetExifData
                                            (
                                                subIfdDirectory,
                                                ExifDirectoryBase.TagFlash
                                            );
                // Get focal length :
                pictureExifs.pictureFocalLength = GetExifData
                                                  (
                                                    subIfdDirectory,
                                                    ExifDirectoryBase.TagFocalLength
                                                  );
                // Get picture width :
                pictureExifs.pictureWidth = DisplayPictureDimension
                                            (
                                                GetExifData
                                                (
                                                    subIfdDirectory,
                                                    ExifDirectoryBase.TagExifImageWidth
                                                )
                                            );
                // Get picture height :
                pictureExifs.pictureHeight = DisplayPictureDimension
                                             (
                                                GetExifData
                                                (
                                                    subIfdDirectory,
                                                    ExifDirectoryBase.TagExifImageHeight
                                                )
                                             );
                // Get picture dimensions :

                var pictureWeightAndHeight = pictureExifMetaData.SpaceTabulation +
                                             pictureExifs.pictureWidth +
                                             "   x   " +
                                             pictureExifs.pictureHeight +
                                             pictureExifMetaData.Pixels;

                pictureExifs.pictureDimensions = (pictureExifs.pictureWidth != pictureExifMetaData.TabEmpty 
                                                 && pictureExifs.pictureHeight != pictureExifMetaData.TabEmpty) ?
                                                                                         pictureWeightAndHeight :
                                                                                         pictureExifMetaData.TabEmpty;
                // Get picture file size :
                pictureExifs.pictureFileSize = DisplayPictureSize
                                               (
                                                    GetExifData
                                                    (
                                                        subMetadataDirectory,
                                                        FileMetadataDirectory.TagFileSize
                                                    )
                                               );

                return Json(pictureExifs, JsonRequestBehavior.AllowGet);

            }
                return Json(pictureExifs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Extract exif data from picture directories.
        /// </summary>
        /// <param name="pictureDirectory"></param>
        /// <param name="pictureExiftag"></param>
        /// <returns></returns>
        public string GetExifData(MetadataExtractor.Directory pictureDirectory, int pictureExiftag)
        {
            var exifData = pictureDirectory?.GetDescription(pictureExiftag);

            var pictureExifData = (!string.IsNullOrEmpty(exifData)) ?
                                  (pictureExifMetaData.SpaceTabulation + exifData) :
                                  (pictureExifMetaData.TabEmpty);

            return pictureExifData;
        }

        /// <summary>
        /// Convert bytes to octets for a friendly picture file size display.
        /// </summary>
        /// <param name="exifDataTagFileSize"></param>
        /// <returns></returns>
        public string DisplayPictureSize(string exifDataTagFileSize)
        {
            // Extract bytes numbers of picture file size :
            var extractBytesNumbers = string.Join("", exifDataTagFileSize.ToCharArray().Where(Char.IsDigit));

            // Convert bytes to Ko :
            var convertNumbersToKo = Math.Round(Convert.ToDecimal(extractBytesNumbers) / 1000);

            // Display picture file size in Ko :
            var displayPictureFileSize = pictureExifMetaData.SpaceTabulation +
                                         convertNumbersToKo +
                                         pictureExifMetaData.KiloOctets;

            // Diplay picture file size in Mo : 
            if (convertNumbersToKo >= 1000)
            {
                var convertNumbersToMo = Math.Round(convertNumbersToKo / 1000);

                return displayPictureFileSize = pictureExifMetaData.SpaceTabulation +
                                                convertNumbersToMo +
                                                pictureExifMetaData.MegaOctets;
            }

            return displayPictureFileSize;
        }

        /// <summary>
        ///  Display dimension value of picture width & height.
        /// </summary>
        /// <param name="exifDataTagDimension"></param>
        /// <returns></returns>
        public string DisplayPictureDimension(string exifDataTagDimension)
        {

            if (exifDataTagDimension != pictureExifMetaData.TabEmpty)
            {
                // Extract pixels dimension numbers of picture width or height :
                var displayDimensionNumbers = string.Join("", exifDataTagDimension.ToCharArray().Where(Char.IsDigit));

                return displayDimensionNumbers;
            }

            return exifDataTagDimension;
        }

        public string TestDateTimeString(Regex patternFormat, string datetimeString)

        {
            if (string.IsNullOrEmpty(datetimeString))
            {
                return null;
            }

            var stringFromRegexMatching = patternFormat.Match(datetimeString)
                                                       .ToString();

            return stringFromRegexMatching;
        }

        public string[] GetDateTimeValues(string datetimeString)
        {
            string[] datetimeArrayValues = new string[2] { pictureExifMetaData.EmptyValue, pictureExifMetaData.EmptyValue };

            Dictionary<string, string> datetimeDico = new Dictionary<string, string>()
            {
                {"testDateFormatA", TestDateTimeString(pictureControls.PatternOrigDtFA, datetimeString)},
                {"testDateFormatB", TestDateTimeString(pictureControls.PatternOrigDtFB, datetimeString)},
                {"testDateFormatC", TestDateTimeString(pictureControls.PatternOrigDtFC, datetimeString)},
                {"testDateFormatD", TestDateTimeString(pictureControls.PatternOrigDtFD, datetimeString)},
                {"testTimeFormatA", TestDateTimeString(pictureControls.PatternOrigTmFA, datetimeString)}
            };

            // Get date & time expressions values from dico :
            var datetimeValues = datetimeDico.Values
                                             .Where(value => value.Length > 0)
                                             .OrderByDescending(x => x.Length)
                                             .ToList();
            if (datetimeValues.Count > 0)
            {

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

                var finalDateValue = (isDateValueBeenFormated) ? dateValueFormated.ToString("dd/MM/yyyy")
                                                               : dateValue.ToString();
                // 2. Manage time value :

                var timeValue = datetimeValues.ElementAt(1);

                DateTime timeValueFormated;

                bool isTimeValueBeenFormated = DateTime.TryParse(timeValue, out timeValueFormated);

                var finalTimeValue = (isTimeValueBeenFormated) ? timeValueFormated.ToString("hh:mm:ss")
                                                               : timeValue.ToString();
                // Fill the array :
                datetimeArrayValues[0] = finalDateValue;
                datetimeArrayValues[1] = finalTimeValue;
            }

            return datetimeArrayValues;
        }

        #endregion
    }
}