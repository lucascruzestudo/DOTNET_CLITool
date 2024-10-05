using System;
using System.IO;
using System.Reflection;

namespace CustomTool.Resources
{
    public class ResourceManager
    {
        public string GetEmbeddedResource(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = ConstructResourceName(resourcePath);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Console.WriteLine($"Resource '{resourceName}' not found.");
                    return null;
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string ConstructResourceName(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            return $"{assemblyName}.{resourcePath.Replace('\\', '.').Replace('/', '.')}";
        }
    }
}
