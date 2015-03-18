using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyPicturesFolderOrganizer
{
    public static class PicturesOrganizer
    {
        private static Regex r = new Regex(":");

        public static void Organize(string parentFolderPath)
        {
            if (!Directory.Exists(parentFolderPath))
                throw new IOException("Folder selected is not a valid folder.");

            var allFiles = Directory.EnumerateFiles(parentFolderPath, "*.*", SearchOption.AllDirectories);
            var pictures = allFiles.Where(f => f.EndsWith(".JPG"));
            var movies = allFiles.Where(f => f.EndsWith(".MOV"));
            var otherFiles = allFiles.Where(f => !(f.EndsWith(".JPG") || f.EndsWith(".MOV")));

            OrganizePictures(parentFolderPath, pictures);
            OrganizeMovies(parentFolderPath, movies);
            OrganizeOtherFiles(parentFolderPath, parentFolderPath);
            DeleteEmptyFolders();
        }

        private static void DeleteEmptyFolders()
        {
        }

        private static void OrganizeOtherFiles(string parentFolderPath1, string parentFolderPath2)
        {
        }

        private static void OrganizeMovies(string parentFolderPath, IEnumerable<string> movies)
        {
        }

        private static void OrganizePictures(string parentFolderPath, IEnumerable<string> pictures)
        {
            foreach (var picture in pictures)
            {
                var dateTaken = GetDateTakenFromImage(picture);

                var newFolderName = string.Format("{0}{1}", dateTaken.Year, dateTaken.ToString("MMM", CultureInfo.InvariantCulture));

                CreateFolderIfNotExist(parentFolderPath, newFolderName);

                var newFolderPath = parentFolderPath + @"\" + newFolderName;
                MoveToFolder(parentFolderPath, picture, newFolderPath);
            }
        }

        private static void MoveToFolder(string parentFolderPath, string picture, string newFolderPath, int duplicateCounter = 0)
        {
            var fileName = Path.GetFileName(picture);
            if (duplicateCounter > 0)
            {
                fileName = string.Format("{0}_Dup{1}", fileName, duplicateCounter);
            }

            try
            {
                File.Move(picture, newFolderPath + @"\" + fileName);
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("Cannot create a file when that file already exists."))
                {
                    CreateFolderIfNotExist(parentFolderPath, "Duplicates");

                    duplicateCounter = ++duplicateCounter;
                    MoveToFolder(parentFolderPath, picture, parentFolderPath + @"\Duplicates", duplicateCounter);
                }
                else
                    throw;
            }
        }

        private static void CreateFolderIfNotExist(string parentFolder, string folderName)
        {
            Directory.CreateDirectory(parentFolder + @"\" + folderName);
        }

        private static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fs, false, false))
            {
                var propertyId = 36867;
                if (image.PropertyIdList.Any(propId => propId == propertyId))
                {
                    PropertyItem propItem = image.GetPropertyItem(propertyId);

                    var dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);

                    return DateTime.Parse(dateTaken);
                }
                return File.GetCreationTime(path);
            }
        }
    }
}
