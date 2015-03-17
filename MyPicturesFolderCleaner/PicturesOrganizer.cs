using System.IO;

namespace MyPicturesFolderOrganizer
{
    public static class PicturesOrganizer
    {
        public static void Organize(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new IOException("Folder selected is not a valid folder.");
        }
    }
}
