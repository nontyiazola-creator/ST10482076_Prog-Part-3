using System;

namespace CybersecurityChatbott
{
    public class CyberTask
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Reminder { get; set; }

        public bool IsComplete { get; set; }

        public string CreatedAt { get; set; }

        // NEW FEATURES

        public string Category { get; set; } = "General";

        public string Priority { get; set; } = "Medium";

        public string CompletedAt { get; set; } = "";

        // Used by the GUI
        public override string ToString()
        {
            string status = IsComplete ? "✅ Completed" : "🟡 Pending";

            return
                $"[{Id}] {Title}\n" +
                $"Category : {Category}\n" +
                $"Priority : {Priority}\n" +
                $"Description : {Description}\n" +
                $"Reminder : {(string.IsNullOrWhiteSpace(Reminder) ? "None" : Reminder)}\n" +
                $"Status : {status}";
        }
    }
}