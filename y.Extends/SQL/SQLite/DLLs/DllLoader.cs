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
        internal static void Loader()
        {
            var list = LoaderStram();
            foreach (var dll in list)
            {
                try
                {
                    LoadLibrary(dll);
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
            var dir = Path.Combine(Path.GetTempPath(), "y.Extends.DllLoader");
            if (! Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

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
    }
}