using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Custom tool for generating commands");

        var newCommand = new Command("new", "Creates a new entity command");
        rootCommand.Add(newCommand);

        var commandCommand = new Command("command", "Creates a new entity command")
        {
            new Option<string>(["-e", "--entity"], "Entity name"),
            new Option<string>(["-p", "--project"], "Project name"),
            new Option<string>(["-o", "--output"], "Output path")
        };

        commandCommand.Handler = CommandHandler.Create<string, string, string>((entity, project, output) =>
        {
            if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(output))
            {
                Console.WriteLine("Entity, project names, and output path are required.");
                return;
            }

            CreateCommand(entity, project, output);
        });

        newCommand.AddCommand(commandCommand);

        return await rootCommand.InvokeAsync(args);
    }

    static void CreateCommand(string entity, string project, string output)
    {
        string template = "Hello {{entity}} from {{projectname}}";
        string content = template.Replace("{{entity}}", entity).Replace("{{projectname}}", project);

        File.WriteAllText(output, content);
        Console.WriteLine($"File created at {output}");
    }
}
