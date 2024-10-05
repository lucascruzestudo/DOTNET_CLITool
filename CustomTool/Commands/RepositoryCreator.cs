using System;
using System.IO;
using CustomTool.Resources;

namespace CustomTool.Commands
{
    public class RepositoryCreator
    {
        private readonly ResourceManager _resourceManager;

        public RepositoryCreator(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public void CreateRepositoryWithInterface(string entity, string project, string output, string interfaceOutput)
        {
            EnsureDirectoryExists(output);
            EnsureDirectoryExists(interfaceOutput);

            string repositoryTemplate = _resourceManager.GetEmbeddedResource("Templates.Repositories.Repository.txt");
            if (!string.IsNullOrEmpty(repositoryTemplate))
            {
                string repositoryContent = repositoryTemplate.Replace("{{entityname}}", entity)
                                                             .Replace("{{projectname}}", project);

                string repositoryFilePath = Path.Combine(output, $"{entity}Repository.cs");
                WriteFile(repositoryFilePath, repositoryContent);
            }

            string interfaceTemplate = _resourceManager.GetEmbeddedResource("Templates.Repositories.RepositoryInterface.txt");
            if (!string.IsNullOrEmpty(interfaceTemplate))
            {
                string interfaceContent = interfaceTemplate.Replace("{{entityname}}", entity)
                                                           .Replace("{{projectname}}", project);

                string interfaceFilePath = Path.Combine(interfaceOutput, $"I{entity}Repository.cs");
                WriteFile(interfaceFilePath, interfaceContent);
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void WriteFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                Console.WriteLine($"File created at {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write file '{filePath}': {ex.Message}");
            }
        }
    }
}
