using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PicturesVideosOrganizer
{
    public static class Organizer
    {
        private static readonly Regex R = new Regex(":");

        public static void Organize(string parentFolderPath)
        {
            if (!Directory.Exists(parentFolderPath))
                throw new IOException("Folder selected is not a valid folder.");

            var allFiles = Directory.EnumerateFiles(parentFolderPath, "*.*", SearchOption.AllDirectories);
            var pictures = allFiles.Where(f => f.EndsWith(".JPG", StringComparison.OrdinalIgnoreCase)  );
            var movies = allFiles.Where(f => f.EndsWith(".MOV", StringComparison.OrdinalIgnoreCase));
            var otherFiles = allFiles.Where(f => !(f.EndsWith(".JPG", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".MOV", StringComparison.OrdinalIgnoreCase)));

            OrganizePictures(parentFolderPath, pictures);
            OrganizeMovies(parentFolderPath, movies);
            OrganizeOtherFiles(parentFolderPath, otherFiles);
            DeleteEmptyFolders(parentFolderPath);
        }

        private static void DeleteEmptyFolders(string parentFolderPath)
        {
            foreach (var folder in Directory.GetDirectories(parentFolderPath))
            {
                DeleteEmptyFolders(folder);
                if (Directory.GetFiles(folder).Length == 0 &&
                    Directory.GetDirectories(folder).Length == 0)
                {
                    Directory.Delete(folder, false);
                }
            }
        }

        private static void OrganizeOtherFiles(string parentFolderPath, IEnumerable<string> otherFiles)
        {
            CreateFolderIfNotExist(parentFolderPath, "OtherFiles");
            var otherFilesFolder = parentFolderPath + @"\OtherFiles";
            foreach (var otherFile in otherFiles)
            { 
                MoveToFolder(otherFilesFolder, otherFile, otherFilesFolder);
            }
        }

        private static void OrganizeMovies(string parentFolderPath, IEnumerable<string> movies)
        {
            CreateFolderIfNotExist(parentFolderPath, "Movies");
            var moviesFolderPath = parentFolderPath + @"\Movies";
            foreach (var movie in movies)
            {
                MoveToFolder(moviesFolderPath, movie, moviesFolderPath);
            }
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

        private static void MoveToFolder(string parentFolderPath, string filePath, string newFolderPath, int duplicateCounter = 0)
        {
            var fileName = Path.GetFileName(filePath);
            
            if (duplicateCounter > 0)
            {
                fileName = string.Format("{0}_Dup{1}{2}", Path.GetFileNameWithoutExtension(filePath), duplicateCounter, Path.GetExtension(filePath));
            }

            try
            {
                File.Move(filePath, newFolderPath + @"\" + fileName);
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("Cannot create a file when that file already exists."))
                {
                    CreateFolderIfNotExist(parentFolderPath, "Duplicates");

                    duplicateCounter = ++duplicateCounter;
                    MoveToFolder(parentFolderPath, filePath, parentFolderPath + @"\Duplicates", duplicateCounter);
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
            using (Image image = Image.FromStream(fs, false, false))
            {
                var propertyId = 36867;
                if (image.PropertyIdList.Any(propId => propId == propertyId))
                {
                    PropertyItem propItem = image.GetPropertyItem(propertyId);

                    var dateTaken = R.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);

                    return DateTime.Parse(dateTaken);
                }
                return File.GetCreationTime(path);
            }
        }
    }
}
