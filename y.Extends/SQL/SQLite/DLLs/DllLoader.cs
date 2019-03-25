using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace y.Extends.SQL.SQLite.DLLs
{
    internal static class DllLoader
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        internal static void Loader()
        {
            var list = LoaderStram();
            foreach (var dll in list)
            {
                try
                {
                    LoadLibrary(dll);
                   //LoadLibraryEx(dll, IntPtr.Zero, LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static List <string> LoaderStram()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var list = assembly.GetManifestResourceNames().Where(i => i.ToLower().Contains("sqlite")).ToList();
            var dir = Path.Combine(Path.GetTempPath(), "C3C16319-BEA2-4A72-A27C-B90829B82D64");
            if (! Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //var dir = Environment.CurrentDirectory;
            var listString = new List <string>();

            foreach (var filePath in list)
            {
                var fileName = filePath.Replace($"{typeof (DllLoader)?.Namespace ?? ""}.", "");

                var stream = assembly.GetManifestResourceStream(filePath);
                if (stream == null)
                {
                    continue;
                }

                var buffer = new byte[ stream.Length ];
                stream.Read(buffer, 0, buffer.Length);

                var file = Path.Combine(dir, fileName);
                if (! File.Exists(file))
                {
                    File.WriteAllBytes(file, buffer);
                }

                listString.Add(file);
            }

            return listString;
        }

        [Flags]
        private enum LoadLibraryFlags : uint
        {
            None = 0,
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
            LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
            LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
            LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }
    }
}