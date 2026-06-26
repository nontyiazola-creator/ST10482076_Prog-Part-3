using System;
using System.IO;

namespace CybersecurityChatbott
{
    public class ChatBot
    {
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;

        private string _lastTopic = null;
        private bool _awaitingName = true;

        private int? _pendingTaskId = null;
        private bool _awaitingReminder = false;


        private string DefaultMemoryPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "cyber_memory.json"
            );


        public TaskManager? TaskManager { get; set; }
        public ActivityLogger? ActivityLogger { get; set; }
        public QuizManager? QuizManager { get; set; }


        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();


            try
            {
                if (_memory.LoadFromFile(DefaultMemoryPath))
                {
                    _awaitingName = string.IsNullOrEmpty(_memory.UserName);
                }
            }
            catch
            {
                _awaitingName = true;
            }
        }



        public string UserName
        {
            get
            {
                return _memory.UserName ?? "";
            }
        }



        public string GetGreeting()
        {
            if (!string.IsNullOrEmpty(UserName) && !_awaitingName)
            {
                return $"Welcome back {UserName}! Ask me anything about cybersecurity.";
            }


            return "Hello! Welcome to the Cybersecurity Awareness Bot. What is your name?";
        }



        public string GetSystemStatus()
        {
            return
                "CyberBot System Status\n\n" +

                "User: " +
                (string.IsNullOrEmpty(UserName) ? "Unknown" : UserName)

                + "\n\nTask Manager: " +
                (TaskManager != null ? "Connected" : "Unavailable")

                + "\nActivity Logger: " +
                (ActivityLogger != null ? "Connected" : "Unavailable")

                + "\nQuiz Manager: " +
                (QuizManager != null ? "Connected" : "Unavailable");
        }




        public string ProcessInput(string input)
        {

            if (string.IsNullOrWhiteSpace(input))
                return "";


            input = input.Trim();

            string lower = input.ToLower();



            // Commands
            if (input.StartsWith("/"))
            {
                return HandleCommand(input);
            }



            // System test
            if (lower.Contains("status"))
            {
                return GetSystemStatus();
            }



            // Name collection
            if (_awaitingName)
            {
                _memory.UserName = input;
                _awaitingName = false;


                try
                {
                    _memory.SaveToFile(DefaultMemoryPath);
                }
                catch
                {

                }


                return $"Nice to meet you {UserName}. How can I help with cybersecurity?";
            }




            // Task creation
            if (lower.Contains("add task") ||
                lower.Contains("remind me") ||
                lower.Contains("create task"))
            {

                if (TaskManager == null)
                {
                    return "Task manager is unavailable.";
                }


                string title = input;


                CyberTask task =
                    TaskManager.AddTask(
                        title,
                        "Cybersecurity reminder",
                        ""
                    );


                _pendingTaskId = task.Id;
                _awaitingReminder = true;


                return
                    $"Task created:\n{task.Title}\n\nWould you like a reminder?";
            }



            // Activity log
            if (lower.Contains("show log") ||
               lower.Contains("activity"))
            {
                if (ActivityLogger != null)
                {
                    return ActivityLogger.GetRecentLog();
                }

                return "Activity log unavailable.";
            }



            // Quiz
            if (lower.Contains("quiz"))
            {
                return "Please open the Quiz tab to start the cybersecurity quiz.";
            }




            // Help
            if (lower.Contains("help"))
            {
                return
                    "I can help with:\n\n" +
                    "- Cybersecurity questions\n" +
                    "- Creating tasks\n" +
                    "- Viewing activity logs\n" +
                    "- Taking quizzes\n" +
                    "- Saving memory";
            }




            // Cybersecurity responses
            string matched;

            string response =
                _keywords.GetResponseWithKey(input, out matched);


            if (response != null)
            {
                _lastTopic = matched;
                return response;
            }



            return
                "I am not sure I understand. Try asking about passwords, phishing, malware, or privacy.";
        }





        private string HandleCommand(string command)
        {

            string cmd = command.ToLower().Trim();



            if (cmd == "/help")
            {
                return
                "Commands:\n" +
                "/help\n" +
                "/save\n" +
                "/load\n" +
                "/reset\n" +
                "/whoami";
            }



            if (cmd == "/save")
            {
                try
                {
                    _memory.SaveToFile(DefaultMemoryPath);
                    return "Memory saved.";
                }
                catch (Exception ex)
                {
                    return "Save failed: " + ex.Message;
                }
            }



            if (cmd == "/load")
            {
                try
                {
                    if (_memory.LoadFromFile(DefaultMemoryPath))
                    {
                        _awaitingName =
                            string.IsNullOrEmpty(_memory.UserName);

                        return "Memory loaded.";
                    }

                    return "No memory found.";
                }
                catch (Exception ex)
                {
                    return "Load failed: " + ex.Message;
                }
            }




            if (cmd == "/reset")
            {
                _memory = new MemoryStore();
                _awaitingName = true;


                try
                {
                    File.Delete(DefaultMemoryPath);
                }
                catch
                {

                }


                return "Memory reset. What is your name?";
            }





            if (cmd == "/whoami")
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    return "You are " + UserName;
                }


                return "I do not know your name yet.";
            }




            return "Unknown command.";
        }
    }
}