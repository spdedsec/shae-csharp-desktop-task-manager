using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Shae.TaskManager;

/// <summary>
/// Priority of a task.
/// </summary>
public enum Priority
{
    Low,
    Medium,
    High
}

/// <summary>
/// A single task record.
/// </summary>
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// SHAE Task Manager — in-memory task list persisted to JSON.
///
/// Features:
///   - add task (title, description, due date, priority)
///   - list tasks (filter by status: todo/done/all)
///   - mark task complete/incomplete
///   - delete task
///   - save/load to JSON
///
/// Not compiled locally: the .NET SDK is not installed in this environment.
/// See README.md and scripts/install.sh.
/// </summary>
public class TaskManager
{
    private readonly string _storagePath;
    private readonly List<TaskItem> _tasks;
    private int _nextId;

    /// <summary>
    /// Create a task manager backed by the JSON file at storagePath.
    /// </summary>
    public TaskManager(string storagePath)
    {
        _storagePath = storagePath;
        _tasks = new List<TaskItem>();
        _nextId = 1;
        Load();
    }

    /// <summary>
    /// All tasks, optionally filtered by completion status.
    /// </summary>
    public IReadOnlyList<TaskItem> ListTasks(bool? done = null)
    {
        if (!done.HasValue)
        {
            return _tasks.AsReadOnly();
        }
        var filtered = new List<TaskItem>();
        foreach (var task in _tasks)
        {
            if (task.IsDone == done.Value)
            {
                filtered.Add(task);
            }
        }
        return filtered;
    }

    /// <summary>
    /// Add a new task. Throws ArgumentException if title is empty.
    /// </summary>
    public TaskItem AddTask(string title, string description, DateTime dueDate, Priority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required.", nameof(title));
        }

        var task = new TaskItem
        {
            Id = _nextId++,
            Title = title.Trim(),
            Description = description ?? string.Empty,
            DueDate = dueDate,
            Priority = priority,
            IsDone = false,
            CreatedAt = DateTime.Now,
        };

        _tasks.Add(task);
        Save();
        return task;
    }

    /// <summary>
    /// Toggle a task between complete and incomplete. Returns true if found.
    /// </summary>
    public bool ToggleTask(int id)
    {
        var task = FindTask(id);
        if (task == null)
        {
            return false;
        }
        task.IsDone = !task.IsDone;
        Save();
        return true;
    }

    /// <summary>
    /// Delete a task by id. Returns true if found.
    /// </summary>
    public bool DeleteTask(int id)
    {
        var task = FindTask(id);
        if (task == null)
        {
            return false;
        }
        _tasks.Remove(task);
        Save();
        return true;
    }

    private TaskItem? FindTask(int id)
    {
        foreach (var task in _tasks)
        {
            if (task.Id == id)
            {
                return task;
            }
        }
        return null;
    }

    private void Save()
    {
        var dir = Path.GetDirectoryName(_storagePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        File.WriteAllText(_storagePath, JsonSerializer.Serialize(_tasks, options));
    }

    private void Load()
    {
        if (!File.Exists(_storagePath))
        {
            return;
        }

        try
        {
            var json = File.ReadAllText(_storagePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
            if (tasks != null)
            {
                _tasks.Clear();
                _tasks.AddRange(tasks);
            }

            _nextId = 1;
            foreach (var task in _tasks)
            {
                if (task.Id >= _nextId)
                {
                    _nextId = task.Id + 1;
                }
            }
        }
        catch
        {
            // Corrupt or unreadable storage: start fresh rather than crash.
            _tasks.Clear();
            _nextId = 1;
        }
    }
}
