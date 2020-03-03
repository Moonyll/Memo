using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ModellsUp.Models
{
    public class Modells
    {
    }
    public class pictureExifMetaData
    {

        // Camera make :
        public string pictureCameraMake { get; set; }

        // Camera model :
        public string pictureCameraModel { get; set; }

        // Original date time :
        [DataType(DataType.DateTime)]
        public DateTime pictureOriginalDateTime { get; set; }

        // F-Number value :
        public string pictureFnumberValue { get; set; }

        // Aperture value :
        public string pictureApertureValue { get; set; }

        // Exposure time :
        public string pictureExposureTime { get; set; }

        // ISO speed ratings :
        public string pictureIsoSpeedRatings { get; set; }

        // Picture flash :
        public string pictureFlash { get; set; }

        // Focal length :
        public string pictureFocalLength { get; set; }

        // Picture width :
        public string pictureWidth { get; set; }

        // Picture height :
        public string pictureHeight { get; set; }

        // Picture file size :
        public string pictureFileSize { get; set; }

    }
    public static class pictureControls
    {
        // Patterns for original date & time regex :
        public const string OriginalDateFormat1 = @"\d{2}-\d{2}-\d{4}";
        public const string OriginalDateFormat2 = @"\d{2}:\d{2}:\d{4}";
        public const string OriginalDateFormat3 = @"\d{4}-\d{2}-\d{2}";
        public const string OriginalDateFormat4 = @"\d{4}:\d{2}:\d{2}";
        public const string OriginalTimeFormat = @" \d{2}:\d{2}:\d{2}";

        public static Regex RegOrigDtF1 = new Regex(OriginalDateFormat1);
        public static Regex RegOrigDtF2 = new Regex(OriginalDateFormat2);
        public static Regex RegOrigDtF3 = new Regex(OriginalDateFormat3);
        public static Regex RegOrigDtF4 = new Regex(OriginalDateFormat4);
        public static Regex RegOrigTm = new Regex(OriginalTimeFormat);


    }

}