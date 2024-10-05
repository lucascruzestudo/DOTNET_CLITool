using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using CustomTool.Commands;
using CustomTool.Resources;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Custom tool for generating commands");

        var newCommand = new Command("new", "Creates a new item");
        rootCommand.Add(newCommand);

        // Command for generating application commands
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
                Console.WriteLine("Command, project names, and output directory are required. Use -c, -p, -o parameters.");
                return Task.FromResult(1);
            }

            var resourceManager = new ResourceManager();
            var commandCreator = new CommandCreator(resourceManager);
            commandCreator.CreateCommand(command, project, output);

            return Task.FromResult(0);
        });
        newCommand.AddCommand(commandCommand);

        // Command for generating repository and interface
        var repositoryCommand = new Command("repository", "Creates a new repository and interface")
        {
            new Option<string>(new[] { "-e", "--entity" }, "Entity name"),
            new Option<string>(new[] { "-p", "--project" }, "Project name"),
            new Option<string>(new[] { "-o", "--output" }, "Repository class output directory"),
            new Option<string>(new[] { "-io", "--interfaceoutput" }, "Interface output directory")
        };

        repositoryCommand.Handler = CommandHandler.Create<string, string, string, string>((entity, project, output, interfaceoutput) =>
        {
            if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(output) || string.IsNullOrEmpty(interfaceoutput))
            {
                Console.WriteLine("Entity name, project name, repository output, and interface output directory are required. Use -e, -p, -o, -io parameters.");
                return Task.FromResult(1);
            }

            var resourceManager = new ResourceManager();
            var repositoryCreator = new RepositoryCreator(resourceManager);
            repositoryCreator.CreateRepositoryWithInterface(entity, project, output, interfaceoutput);

            return Task.FromResult(0);
        });
        newCommand.AddCommand(repositoryCommand);

        return await rootCommand.InvokeAsync(args);
    }
}
