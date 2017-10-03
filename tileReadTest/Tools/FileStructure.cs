using System.IO;
using System.Reflection;

namespace tileRead.Tools
{
    /// <summary>
    /// Static filestructure class I created to use within any project that would need a place for intermediate storage
    /// </summary>
    public static class FileStructure
    {
        //Path of the cache directory
        public static readonly string cacheDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\cache\";
        public static readonly string parseDirectory = cacheDirectory + "\\parsed_files\\";

        /// <summary>
        /// Checks if the cache directory exists, and creates it if not
        /// </summary>
        private static void createCache()
        {
            Directory.CreateDirectory(cacheDirectory);
            instantiateParseDirectory();
        }

        /// <summary>
        /// Pass in the filename !WITHOUT THE PATH, OR EXTENSION! to create a directory
        /// within the cache. Returns the path to the created directory
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>The path of the created directory</returns>
        private static string instantiateFileDirectory(string filename)
        {
            DirectoryInfo info = Directory.CreateDirectory(cacheDirectory + filename + "\\");
            return info.FullName;
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public static void clearCache()
        {
            Directory.Delete(cacheDirectory, true);
            createCache();
        }

        /// <summary>
        /// Pass in the filename WITHOUT THE PATH OR EXTENSION
        /// </summary>
        /// <param name="filename"></param>
        public static void removeFileFromCache(string filename)
        {
            if (Directory.Exists(cacheDirectory + filename + "\\"))
                Directory.Delete(cacheDirectory + filename + "\\", true);
        }

        /// <summary>
        /// Give a full path, and a field directory will be created within it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static void instantiateParseDirectory()
        {
            DirectoryInfo info = Directory.CreateDirectory(parseDirectory);
        }

        /// <summary>
        /// Pass in a file, and the sub-directory for that file will be created
        /// </summary>
        /// <param name="filename"></param>
        public static void addFileToDirectory(string filename)
        {
            createCache();
            string name = Path.GetFileNameWithoutExtension(filename);
            string dir = instantiateFileDirectory(name);
            
        }


        public static string[] getAllCurrentNames()
        {
            if (Directory.Exists(cacheDirectory))
            {
                string[] directories = Directory.GetDirectories(cacheDirectory);
                for (int i = 0; i < directories.Length; i++)
                    directories[i] = directories[i].Replace(cacheDirectory, "");
                return directories;
            }

            else
                return null;
        }

        public static bool CheckIfFileParsed(string fileName)
        {      
            foreach(string file in Directory.GetFiles(cacheDirectory + "\\parsed_files\\"))
            {
                string Name = Path.GetFileNameWithoutExtension(file);
                if(Name == fileName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

