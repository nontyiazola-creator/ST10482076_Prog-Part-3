using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CybersecurityChatbott
{
    public class ActivityLogger
    {
        private readonly List<string> _log = new();

        public void Log(string action)
        {
            string entry = $"[{DateTime.Now:HH:mm}] {action}";
            _log.Add(entry);
        }


        public string GetRecentLog(int count = 10)
        {
            if (_log.Count == 0)
                return "No activity recorded yet.";

            StringBuilder sb = new();

            var recent = _log
                .Skip(Math.Max(0, _log.Count - count))
                .ToList();


            for (int i = 0; i < recent.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {recent[i]}");
            }


            if (_log.Count > count)
            {
                sb.AppendLine();
                sb.AppendLine("Type 'show more' to see full history.");
            }


            return sb.ToString();
        }



        public string GetFullLog()
        {
            if (_log.Count == 0)
                return "No activity recorded yet.";


            StringBuilder sb = new();


            for (int i = 0; i < _log.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {_log[i]}");
            }


            return sb.ToString();
        }



        public int GetCount()
        {
            return _log.Count;
        }



        public void Clear()
        {
            _log.Clear();
        }
    }
}