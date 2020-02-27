using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string pictureOriginalDateTime { get; set; }

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
}