using System;
using System.Globalization;
using Shae.TaskManager;

namespace Shae.TaskManager.App;

/// <summary>
/// Console UI for the SHAE Task Manager.
///
/// SHAE Field Manual Design System applied to a text UI: monospace-aligned
/// data, ink-on-paper concept, one oxide-red signal for headers/actions,
/// no decorative noise. See DESIGN.md.
///
/// Not compiled locally: the .NET SDK is not installed in this environment.
/// See README.md and scripts/install.sh.
/// </summary>
public static class Program
{
    private static readonly TaskManager Manager;

    static Program()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var storageDir = Path.Combine(appData, ".shae-task-manager");
        var storagePath = Path.Combine(storageDir, "tasks.json");
        Manager = new TaskManager(storagePath);
    }

    public static int Main(string[] args)
    {
        if (args.Length > 0)
        {
            return RunCommand(args);
        }

        PrintHeader();
        PrintHelp();

        bool running = true;
        while (running)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (input == null)
            {
                break;
            }

            running = ProcessInput(input);
            Console.WriteLine();
        }

        Console.WriteLine("Goodbye.");
        return 0;
    }

    private static int RunCommand(string[] args)
    {
        var cmd = args[0].ToLowerInvariant();
        return cmd switch
        {
            "list" => ListTasksArg(args.Length > 1 ? args[1] : null),
            "add" => RunAdd(),
            _ => UnknownCommand(args[0]),
        };
    }

    private static bool ProcessInput(string input)
    {
        var trimmed = input.Trim();
        if (trimmed.Length == 0)
        {
            return true;
        }

        var parts = trimmed.Split(' ', 2);
        var cmd = parts[0].ToLowerInvariant();
        var arg = parts.Length > 1 ? parts[1] : string.Empty;

        switch (cmd)
        {
            case "list":
                ListTasksArg(arg.Length > 0 ? arg : null);
                break;
            case "todo":
                ListTasks("todo");
                break;
            case "done":
                ListTasks("done");
                break;
            case "add":
                RunAdd();
                break;
            case "toggle":
                ToggleTask(arg);
                break;
            case "delete":
                DeleteTask(arg);
                break;
            case "help":
                PrintHelp();
                break;
            case "quit":
            case "exit":
                return false;
            default:
                Console.WriteLine("Unknown command: {0}. Type 'help'.", cmd);
                break;
        }

        return true;
    }

    // ------------------------------------------------------------------
    // Commands
    // ------------------------------------------------------------------

    private static void ListTasksArg(string? filter)
    {
        var done = filter switch
        {
            "todo" => false,
            "done" => true,
            _ => (bool?)null,
        };
        ListTasks(done);
    }

    private static void ListTasks(bool? done)
    {
        var tasks = Manager.ListTasks(done);
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine(" ID  TITLE                          DUE         PRIORITY  STATUS");
        Console.WriteLine(" --  -----                          ---         --------  ------");

        foreach (var task in tasks)
        {
            var status = task.IsDone ? "[done]" : "[todo]";
            var due = task.DueDate == DateTime.MinValue
                ? "-"
                : task.DueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.WriteLine(
                " {0,-2}  {1,-30}  {2,-11}  {3,-9}  {4}",
                task.Id,
                Truncate(task.Title, 30),
                due,
                task.Priority,
                status);
        }
        Console.WriteLine();
    }

    private static void RunAdd()
    {
        Console.Write("Title: ");
        var title = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title is required. Task not added.");
            return;
        }

        Console.Write("Description: ");
        var description = Console.ReadLine() ?? string.Empty;

        Console.Write("Due date (YYYY-MM-DD) [blank for none]: ");
        var dueInput = Console.ReadLine() ?? string.Empty;
        var dueDate = DateTime.MinValue;
        if (!string.IsNullOrWhiteSpace(dueInput))
        {
            if (!DateTime.TryParseExact(
                    dueInput,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dueDate))
            {
                Console.WriteLine("Invalid date format. Task not added.");
                return;
            }
        }

        Console.Write("Priority (low/medium/high) [medium]: ");
        var priorityInput = (Console.ReadLine() ?? "medium").ToLowerInvariant();
        var priority = priorityInput switch
        {
            "low" => Priority.Low,
            "high" => Priority.High,
            _ => Priority.Medium,
        };

        try
        {
            var task = Manager.AddTask(title, description, dueDate, priority);
            Console.WriteLine(
                "Task added (id {0}): {1}",
                task.Id,
                task.Title);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
        }
    }

    private static void ToggleTask(string arg)
    {
        if (!int.TryParse(arg, out var id))
        {
            Console.WriteLine("Usage: toggle <id>");
            return;
        }

        if (Manager.ToggleTask(id))
        {
            Console.WriteLine("Task {0} toggled.", id);
        }
        else
        {
            Console.WriteLine("Task {0} not found.", id);
        }
    }

    private static void DeleteTask(string arg)
    {
        if (!int.TryParse(arg, out var id))
        {
            Console.WriteLine("Usage: delete <id>");
            return;
        }

        if (Manager.DeleteTask(id))
        {
            Console.WriteLine("Task {0} deleted.", id);
        }
        else
        {
            Console.WriteLine("Task {0} not found.", id);
        }
    }

    // ------------------------------------------------------------------
    // UI helpers
    // ------------------------------------------------------------------

    private static void PrintHeader()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("SHAE Task Manager  v0.1.0");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Commands:");
        Console.WriteLine("  list          List all tasks");
        Console.WriteLine("  todo          List incomplete tasks");
        Console.WriteLine("  done          List completed tasks");
        Console.WriteLine("  add           Add a new task (interactive)");
        Console.WriteLine("  toggle <id>   Mark a task complete/incomplete");
        Console.WriteLine("  delete <id>   Delete a task");
        Console.WriteLine("  help          Show this help");
        Console.WriteLine("  quit / exit   Exit the task manager");
        Console.WriteLine();
    }

    private static int UnknownCommand(string cmd)
    {
        Console.WriteLine("Unknown command: {0}. Type 'help'.", cmd);
        return 1;
    }

    private static string Truncate(string s, int maxLength)
    {
        if (s.Length <= maxLength)
        {
            return s;
        }
        return s.Substring(0, maxLength - 1) + "…";
    }
}
