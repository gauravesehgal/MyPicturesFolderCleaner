using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Linq;

namespace MyPicturesFolderOrganizer
{
    public static class PicturesOrganizer
    {
        private static Regex r = new Regex(":");

        public static void Organize(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new IOException("Folder selected is not a valid folder.");

            var pictures = Directory.GetFiles(folderPath, "*.JPG", SearchOption.AllDirectories);

            for (int pictureIndex = 0; pictureIndex < pictures.Length; pictureIndex++)
            {
                var dateTaken = GetDateTakenFromImage(pictures[pictureIndex]);
            }
        }

        private static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fs, false, false))
            {
                if (image.PropertyIdList.Any(propId => propId == 36867))
                {
                    PropertyItem propItem = image.GetPropertyItem(36867);

                    var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);

                    return DateTime.Parse(dateTaken);
                }
                return File.GetCreationTime(path);
            }
        }
    }
}
