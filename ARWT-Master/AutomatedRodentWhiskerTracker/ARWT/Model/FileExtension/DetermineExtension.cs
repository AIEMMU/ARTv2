using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARWT.ModelInterface.FileExtension;

namespace ARWT.Model.FileExtension
{
    internal class DetermineExtension : ModelObjectBase, IDetermineExtension
    {
        private readonly string[] ImageExtensions = new string[]{"bmp", "gif", "jpg", "jpeg", "png", "tif", "tiff"};
        private readonly string[] VideoExtensions = new string[] {"avi", "mpg", "mpeg", "mp4", "mov"};

        public bool IsVideo(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            string extension = Path.GetExtension(file);

            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            string extensionLower = extension.ToLower();

            return VideoExtensions.Any(x => x == extensionLower);
        }

        public bool IsImage(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return false;
            }

            string extension = Path.GetExtension(file);

            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            string extensionLower = extension.ToLower();

            return VideoExtensions.Any(x => x == extensionLower);
        }
    }
}
