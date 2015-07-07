using System;

namespace SFEditor
{
    public static class PathHelper
    {
        public static string GetRelativePath(string directory, string fileName)
        {
            string dir = directory;
            if (!dir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                dir += System.IO.Path.DirectorySeparatorChar;
            
            var folderUri = new Uri(dir);
            var pathUri = new Uri(fileName);

            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', System.IO.Path.DirectorySeparatorChar));
        }
    }
}
