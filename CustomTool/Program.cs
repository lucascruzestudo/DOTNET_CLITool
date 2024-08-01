using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Custom tool for generating commands");

        var newCommand = new Command("new", "Creates a new command");
        rootCommand.Add(newCommand);

        var commandCommand = new Command("command", "Creates a new command")
        {
            new Option<string>(new[] { "-c", "--command" }, "Command name"),
            new Option<string>(new[] { "-p", "--project" }, "Project name"),
            new Option<string>(new[] { "-o", "--output" }, "Output directory")
        };

        commandCommand.Handler = CommandHandler.Create<string, string, string>((command, project, output) =>
        {
            if (string.IsNullOrEmpty(command) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Command, project names, and output directory are required.");
                return Task.FromResult(1);
            }

            CreateCommand(command, project, output);
            return Task.FromResult(0);
        });

        newCommand.AddCommand(commandCommand);

        return await rootCommand.InvokeAsync(args);
    }

    static void CreateCommand(string command, string project, string output)
{
    // Ensure the base output directory exists
    string outputDir = Path.GetDirectoryName(output);
    if (!Directory.Exists(outputDir))
    {
        Directory.CreateDirectory(outputDir);
    }

    // Create a subdirectory named after the command
    string commandDir = Path.Combine(outputDir, command);
    if (!Directory.Exists(commandDir))
    {
        Directory.CreateDirectory(commandDir);
    }

    // List of templates and their output names
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
        string templateFile = template.Key;
        string fileName = template.Value;

        string templateContent = GetEmbeddedResource(templateFile);
        if (string.IsNullOrEmpty(templateContent))
        {
            Console.WriteLine($"Could not read the template {templateFile}.");
            continue;
        }

        string content = templateContent.Replace("{{command}}", command)
                                         .Replace("{{projectname}}", project);

        string filePath = Path.Combine(commandDir, fileName);

        // Write the content to the file
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

    static string GetEmbeddedResource(string resourcePath)
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

static string ConstructResourceName(string resourcePath)
{
    var assembly = Assembly.GetExecutingAssembly();
    var assemblyName = assembly.GetName().Name;

    return $"{assemblyName}.{resourcePath.Replace('\\', '.').Replace('/', '.')}";
}
}
