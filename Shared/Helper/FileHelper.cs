using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Shared.Helper
{
    public static class FileHelper
    {
        public static bool EnsureFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("La ruta no puede estar vacía.");

            string pathDefault = path;
            if (SettingsEnv.ProcessEnv == SettingsEnv.PROD)
                pathDefault = Path.Combine(Directory.GetCurrentDirectory(), path);
            if (!Directory.Exists(pathDefault))
            {
                var d = Directory.CreateDirectory(pathDefault);
                return d.Exists;
            }

            return true;
        }

        public static void SaveXmlDocument(this XDocument xmlDocument, string fileName)
        {
            var name = Path.GetFileName(fileName);
            string pathPartial = Constants.PATHSENDSIESAPLANE_DEV;
            if (SettingsEnv.ProcessEnv == SettingsEnv.PROD)
                pathPartial = Constants.PATHSENDSIESAPLANE_PROD;
            var path = Path.Combine(pathPartial, fileName.Replace(name, ""));
            if (name.Contains('-'))
            {
                var collection = name.Split('-', (char)StringSplitOptions.RemoveEmptyEntries)[0];
                path += collection;
            }

            if (EnsureFolder(path))
            {
                if (!name.EndsWith(".xml"))
                    name += ".xml";
                xmlDocument.Save(Path.Combine(Directory.GetCurrentDirectory(), path, name));
            }
        }
    }
}
