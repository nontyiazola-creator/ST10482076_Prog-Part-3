using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CybersecurityChatbott
{
    public class TaskStorageHelper
    {
        private const string FilePath = "tasks.json";

        public List<CyberTask> LoadTasks()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<CyberTask>();

                var json = File.ReadAllText(FilePath);
                var list = JsonSerializer.Deserialize<List<CyberTask>>(json);
                return list ?? new List<CyberTask>();
            }
            catch
            {
                return new List<CyberTask>();
            }
        }

        public void SaveTasks(List<CyberTask> tasks)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(FilePath, json);
            }
            catch
            {
                // ignore write failures for now
            }
        }

        public CyberTask AddTask(string title, string description, string reminder)
        {
            var tasks = LoadTasks();
            int nextId = 1;
            if (tasks.Count > 0)
                nextId = tasks[tasks.Count - 1].Id + 1;

            var t = new CyberTask
            {
                Id = nextId,
                Title = title,
                Description = description,
                Reminder = reminder ?? string.Empty,
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            tasks.Add(t);
            SaveTasks(tasks);
            return t;
        }

        public void MarkAsComplete(int id)
        {
            var tasks = LoadTasks();
            var found = tasks.Find(x => x.Id == id);
            if (found != null)
            {
                found.IsComplete = true;
                SaveTasks(tasks);
            }
        }

        public void DeleteTask(int id)
        {
            var tasks = LoadTasks();
            tasks.RemoveAll(x => x.Id == id);
            SaveTasks(tasks);
        }

        public void UpdateTaskReminder(int id, string reminder)
        {
            var tasks = LoadTasks();
            var found = tasks.Find(x => x.Id == id);
            if (found != null)
            {
                found.Reminder = reminder ?? string.Empty;
                SaveTasks(tasks);
            }
        }
    }
}
