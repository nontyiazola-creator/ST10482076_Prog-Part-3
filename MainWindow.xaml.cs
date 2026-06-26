using CybersecurityChatbott;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace CybersecurityChatbott
{
    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;
        private TaskManager _taskManager;
        private ActivityLogger _activityLogger;
        private QuizManager _quizManager;
        private string _selectedQuizAnswer;

        public MainWindow()
        {
            InitializeComponent();
            try { AudioPlayer.PlayGreeting(); } catch { }

            _activityLogger = new ActivityLogger();
            _taskManager = new TaskManager(_activityLogger);
            _quizManager = new QuizManager();
            _chatBot = new ChatBot();

            // Link managers to chatbot if needed
            _chatBot.TaskManager = _taskManager;
            _chatBot.ActivityLogger = _activityLogger;
            _chatBot.QuizManager = _quizManager;

            AppendHeaderArt();
            AppendBotMessage(_chatBot.GetGreeting());

            RefreshTaskList();
            InitializeQuizUI();
            RefreshActivityLog();
        }

        // --- Task Management ---
        private void RefreshTaskList()
        {
            try
            {
                lstTasks.Items.Clear();
                var tasks = _taskManager.GetAllTasks();
                foreach (var t in tasks)
                {
                    var status = t.IsComplete ? "[Done]" : "";
                    lstTasks.Items.Add($"{t.Id}: {t.Title} {status} - {t.Description} {(string.IsNullOrEmpty(t.Reminder) ? "" : "(Rem: " + t.Reminder + ")")}");
                }
            }
            catch { }
        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            var title = txtTaskTitle.Text?.Trim() ?? string.Empty;
            var desc = txtTaskDesc.Text?.Trim() ?? string.Empty;
            var rem = txtTaskReminder.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(title))
            {
                AppendBotMessage("Please enter a task title.");
                return;
            }
            var t = _taskManager.AddTask(title, desc, rem);
            AppendBotMessage($"Task added: '{t.Title}'");
            RefreshTaskList();
            RefreshActivityLog();
        }

        private void btnMarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem == null) return;
            var sel = lstTasks.SelectedItem.ToString();
            var idStr = sel.Split(':')[0];
            if (int.TryParse(idStr, out int id))
            {
                _taskManager.MarkAsComplete(id);
                AppendBotMessage($"Marked task {id} complete.");
                RefreshTaskList();
                RefreshActivityLog();
            }
        }

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem == null) return;
            var sel = lstTasks.SelectedItem.ToString();
            var idStr = sel.Split(':')[0];
            if (int.TryParse(idStr, out int id))
            {
                _taskManager.DeleteTask(id);
                AppendBotMessage($"Deleted task {id}.");
                RefreshTaskList();
                RefreshActivityLog();
            }
        }

        // --- Quiz UI ---
        private void InitializeQuizUI()
        {
            try
            {
                _quizManager.ResetQuiz();
                ShowCurrentQuizQuestion();
                UpdateQuizScore();
            }
            catch { }
        }

        private void ShowCurrentQuizQuestion()
        {
            var q = _quizManager.GetCurrentQuestion();
            if (q == null)
            {
                txtQuizQuestion.Text = "No question available.";
                btnOptionA.Visibility = btnOptionB.Visibility = btnOptionC.Visibility = btnOptionD.Visibility = Visibility.Collapsed;
                btnSubmitAnswer.Visibility = Visibility.Collapsed;
                btnNextQuestion.Visibility = Visibility.Collapsed;
                return;
            }
            txtQuizQuestion.Text = q.Question;
            void setBtn(Button b, int idx)
            {
                if (q.Options != null && q.Options.Count > idx)
                {
                    b.Content = q.Options[idx];
                    b.Visibility = Visibility.Visible;
                }
                else
                {
                    b.Visibility = Visibility.Collapsed;
                }
                b.ClearValue(Button.BackgroundProperty);
            }
            setBtn(btnOptionA, 0);
            setBtn(btnOptionB, 1);
            setBtn(btnOptionC, 2);
            setBtn(btnOptionD, 3);
            _selectedQuizAnswer = null;
            btnNextQuestion.Visibility = Visibility.Collapsed;
            btnSubmitAnswer.Visibility = Visibility.Visible;
        }

        private void UpdateQuizScore()
        {
            var result = _quizManager.GetFinalScore();
            txtQuizScore.Text = $"Score: {result.score}/{result.total} ({result.percentage}%)";
        }

        private void SelectQuizButton(Button clicked)
        {
            foreach (Button b in new[] { btnOptionA, btnOptionB, btnOptionC, btnOptionD })
            {
                b.Background = null;
            }
            clicked.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2B6FA5"));
            _selectedQuizAnswer = clicked.Content?.ToString();
        }

        private void btnOption_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                SelectQuizButton(b);
            }
        }

        private void btnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedQuizAnswer))
            {
                AppendBotMessage("Please select an answer before submitting.");
                return;
            }
            string normalized = _selectedQuizAnswer.Trim();
            string answerKey = null;
            if (normalized.Length >= 2 && (normalized[1] == ')' || normalized[1] == '.'))
                answerKey = normalized.Substring(0, 1).ToUpper();
            else if (normalized.Length > 0 && (normalized.StartsWith("A") || normalized.StartsWith("B") || normalized.StartsWith("C") || normalized.StartsWith("D")))
                answerKey = normalized.Substring(0, 1).ToUpper();
            else
                answerKey = normalized;
            bool correct = _quizManager.SubmitAnswer(answerKey);
            var feedback = _quizManager.GetFeedback(correct);
            AppendBotMessage(feedback);
            UpdateQuizScore();
            btnSubmitAnswer.Visibility = Visibility.Collapsed;
            btnNextQuestion.Visibility = Visibility.Visible;
        }

        private void btnNextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_quizManager.IsFinished())
            {
                var result = _quizManager.GetFinalScore();
                AppendBotMessage($"Quiz completed - final score: {result.score} out of {result.total}.");
                AppendBotMessage(_quizManager.GetFinalMessage());
                btnNextQuestion.Visibility = Visibility.Collapsed;
                btnSubmitAnswer.Visibility = Visibility.Collapsed;
                UpdateQuizScore();
                return;
            }
            ShowCurrentQuizQuestion();
            UpdateQuizScore();
        }

        private void btnResetQuiz_Click(object sender, RoutedEventArgs e)
        {
            _quizManager.ResetQuiz();
            InitializeQuizUI();
            AppendBotMessage("Quiz reset. Good luck!");
        }

        // --- Chatbot ---
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void txtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string userInput = txtUserInput.Text;
            if (string.IsNullOrWhiteSpace(userInput))
                return;
            AppendUserMessage(userInput);
            string response = _chatBot.ProcessInput(userInput);
            AppendBotMessage(response);
            AudioPlayer.Speak(response);
            txtUserInput.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            rtbChatDisplay.Document.Blocks.Clear();
            AppendBotMessage(_chatBot.GetGreeting());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string fileName = string.IsNullOrEmpty(_chatBot.UserName) ? "cyber_chat_conversation.txt" : $"{_chatBot.UserName}_conversation.txt";
                string path = System.IO.Path.Combine(docs, fileName);
                var range = new TextRange(rtbChatDisplay.Document.ContentStart, rtbChatDisplay.Document.ContentEnd);
                File.WriteAllText(path, range.Text);
                AppendBotMessage($"Conversation saved to: {path}");
            }
            catch (Exception ex)
            {
                AppendBotMessage("Failed to save conversation: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = System.IO.Path.Combine(docs, "cyber_chat_conversation.txt");
                if (!File.Exists(path))
                {
                    AppendBotMessage("No saved conversation found.");
                    return;
                }
                string text = File.ReadAllText(path);
                rtbChatDisplay.Document.Blocks.Clear();
                var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    if (trimmed.StartsWith("You:"))
                        AppendUserMessage(trimmed.Substring(4).Trim());
                    else if (trimmed.StartsWith("Bot:"))
                        AppendBotMessage(trimmed.Substring(4).Trim());
                    else
                        AppendBotMessage(trimmed);
                }
                AppendBotMessage($"Conversation loaded from: {path}");
            }
            catch (Exception ex)
            {
                AppendBotMessage("Failed to load conversation: " + ex.Message);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    FileName = string.IsNullOrEmpty(_chatBot.UserName) ? "conversation.txt" : $"{_chatBot.UserName}_conversation.txt"
                };
                if (dlg.ShowDialog() == true)
                {
                    var range = new TextRange(rtbChatDisplay.Document.ContentStart, rtbChatDisplay.Document.ContentEnd);
                    File.WriteAllText(dlg.FileName, range.Text);
                    AppendBotMessage($"Conversation exported to: {dlg.FileName}");
                }
            }
            catch (Exception ex)
            {
                AppendBotMessage("Failed to export conversation: " + ex.Message);
            }
        }

        private void AppendUserMessage(string message)
        {
            AppendBubble(message, isUser: true);
        }

        private void AppendBotMessage(string message)
        {
            AppendBubble(message, isUser: false);
        }

        private void AppendBubble(string message, bool isUser)
        {
            var border = new Border
            {
                Background = isUser ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2B6FA5")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1E3A5F")),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                MaxWidth = 480,
                Margin = new Thickness(5)
            };
            var stack = new StackPanel();
            var txt = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.White,
                FontSize = 14
            };
            var time = new TextBlock
            {
                Text = DateTime.Now.ToString("g"),
                FontSize = 10,
                Foreground = Brushes.LightGray,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 6, 0, 0)
            };
            stack.Children.Add(txt);
            stack.Children.Add(time);
            border.Child = stack;
            var container = new BlockUIContainer(border)
            {
                Margin = new Thickness(0)
            };
            if (isUser)
                border.HorizontalAlignment = HorizontalAlignment.Right;
            else
                border.HorizontalAlignment = HorizontalAlignment.Left;
            border.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 10,
                Opacity = 0.28,
                ShadowDepth = 3
            };
            border.Opacity = 0;
            var translate = new TranslateTransform(isUser ? 20 : -20, 0);
            border.RenderTransform = translate;
            rtbChatDisplay.Document.Blocks.Add(container);
            rtbChatDisplay.ScrollToEnd();
            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(320)) { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } };
            var slide = new DoubleAnimation(isUser ? 20 : -20, 0, TimeSpan.FromMilliseconds(320)) { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } };
            border.BeginAnimation(UIElement.OpacityProperty, fade);
            translate.BeginAnimation(TranslateTransform.XProperty, slide);
        }

        private void RefreshActivityLog()
        {
            try
            {
                if (_activityLogger != null)
                {
                    txtActivityLog.Text = _activityLogger.GetFullLog();
                }
                else
                {
                    txtActivityLog.Text = "Activity logger unavailable.";
                }
            }
            catch (Exception ex)
            {
                txtActivityLog.Text = "Unable to load activity log: " + ex.Message;
            }
        }

        private void AppendHeaderArt()
        {
            var headerBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF16324A")),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(8),
                Margin = new Thickness(6),
                MaxWidth = 520,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var headerText = new TextBlock
            {
                Text = "🔐 CYBERSECURITY AWARENESS BOT 🔐\n— Stay safe, stay aware —",
                TextAlignment = TextAlignment.Center,
                Foreground = Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap
            };
            headerBorder.Child = headerText;
            var container = new BlockUIContainer(headerBorder) { Margin = new Thickness(0) };
            var doc = rtbChatDisplay.Document;
            if (doc.Blocks.FirstBlock != null)
                doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, container);
            else
                doc.Blocks.Add(container);
        }
    }
}
