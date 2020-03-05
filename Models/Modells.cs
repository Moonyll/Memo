//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.RegularExpressions;

//namespace ModellsUp.Models
//{
//    public class Modells
//    {
//    }
//    public class pictureExifMetaData
//    {
//        // Constant string values to display :

//        public const string EmptyValue = "---";

//        public const string SpaceTabulation = "\u2003";

//        public const string TabEmpty = "\u2003---";

//        public const string KiloOctets = " Ko";

//        public const string MegaOctets = " Mo";

//        public const string Pixels = " px";

//        public const string ISO = " iso";


//        // Camera make :
//        public string pictureCameraMake { get; set; }

//        // Camera model :
//        public string pictureCameraModel { get; set; }

//        // Original date time :
//        public string pictureOriginalDateTime { get; set; }

//        // F-Number value :
//        public string pictureFnumberValue { get; set; }

//        // Aperture value :
//        public string pictureApertureValue { get; set; }

//        // Exposure time :
//        public string pictureExposureTime { get; set; }

//        // ISO speed ratings :
//        public string pictureIsoSpeedRatings { get; set; }

//        // Picture flash :
//        public string pictureFlash { get; set; }

//        // Focal length :
//        public string pictureFocalLength { get; set; }

//        // Picture width :
//        public string pictureWidth { get; set; }

//        // Picture height :
//        public string pictureHeight { get; set; }

//        // Picture dimensions:
//        public string pictureDimensions { get; set; }

//        // Picture file size :
//        public string pictureFileSize { get; set; }

//    }
//    public static class pictureControls
//    {

//        // Regex & error message for the titles :
//        public const string PatternForPictureTitles = @"^[0-9\-._/A-Za-z\u00C0-\u017F]+$";
//        public const string ErrorForPictureTitles = "Veuillez renseigner un titre valide.";

//        // Regex & error message for the description (blank space authorized) :
//        public const string PatternForPictureDescription = @"^[ 0-9\-._/A-Za-z\u00C0-\u017F]+$";
//        public const string ErrorForPictureDescription = "Veuillez renseigner une description valide.";

//        // Regex & error message for the url :
//        public const string PatternForpictureStandardUrl = @"^[0-9\-._/A-Za-z\u00C0-\u017F]+$";
//        public const string ErrorForpictureStandardUrl = "Veuillez saisir un nom valide";

//        // Upload file picture attributes limitations :

//        // Size
//        public const int pictureFileToUploadMaxSize = 7000000;
//        public const string errorMessageForPictureOutOfSize = "La taille maximale de l'image doit être de 7 Mo.";

//        // Extension :
//        public static string[] pictureFileToUploadExtension = { "image/jpg", "image/jpeg" };
//        public const string errorMessageForPictureOutOfExt = "L'image doit être au format jpg / jpeg";

//        // Diretory :
//        public const string pictureFileDirectory = "~/Content/Images/Pictures/";

//        // Patterns for original date & time regex :
//        public const string OriginalDateFormatA = @"\d{2}-\d{2}-\d{4}";
//        public const string OriginalDateFormatB = @"\d{2}:\d{2}:\d{4}";
//        public const string OriginalDateFormatC = @"\d{4}-\d{2}-\d{2}";
//        public const string OriginalDateFormatD = @"\d{4}:\d{2}:\d{2}";
//        public const string OriginalTimeFormatA = @" \d{2}:\d{2}:\d{2}";

//        // Regex for original date & time :
//        public static Regex PatternOrigDtFA = new Regex(OriginalDateFormatA);
//        public static Regex PatternOrigDtFB = new Regex(OriginalDateFormatB);
//        public static Regex PatternOrigDtFC = new Regex(OriginalDateFormatC);
//        public static Regex PatternOrigDtFD = new Regex(OriginalDateFormatD);
//        public static Regex PatternOrigTmFA = new Regex(OriginalTimeFormatA);

//        public const string EmptyValue = "---";
//        public const string DateLabel = "Date : ";
//        public const string TimeLabel = "Heure : ";
//    }

//}

