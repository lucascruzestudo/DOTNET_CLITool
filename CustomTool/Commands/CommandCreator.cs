using System;
using System.Collections.Generic;
using System.IO;
using CustomTool.Resources;

namespace CustomTool.Commands
{
    public class CommandCreator
    {
        private readonly ResourceManager _resourceManager;

        public CommandCreator(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public void CreateCommand(string command, string project, string output)
        {
            string outputDir = Path.GetDirectoryName(output);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string commandDir = Path.Combine(outputDir, command);
            if (!Directory.Exists(commandDir))
            {
                Directory.CreateDirectory(commandDir);
            }

            var templates = new Dictionary<string, string>
            {
                { "Templates.Commands.Command.txt", $"{command}Command.cs" },
                { "Templates.Commands.CommandHandler.txt", $"{command}CommandHandler.cs" },
                { "Templates.Commands.CommandRequest.txt", $"{command}CommandRequest.cs" },
                { "Templates.Commands.CommandResponse.txt", $"{command}CommandResponse.cs" },
                { "Templates.Commands.CommandValidator.txt", $"{command}CommandValidator.cs" }
            };

            foreach (var template in templates)
            {
                string templateContent = _resourceManager.GetEmbeddedResource(template.Key);
                if (string.IsNullOrEmpty(templateContent))
                {
                    Console.WriteLine($"Could not read the template {template.Key}.");
                    continue;
                }

                string content = templateContent.Replace("{{command}}", command)
                                                .Replace("{{projectname}}", project);
                string filePath = Path.Combine(commandDir, template.Value);

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
}
