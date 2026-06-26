using System;
using System.Collections.Generic;

namespace CybersecurityChatbott
{
    public class QuizManager
    {
        private readonly List<QuizQuestion> _questions;

        private int _currentIndex = 0;
        private int _score = 0;


        public QuizManager()
        {
            _questions = new List<QuizQuestion>();

            LoadQuestions();
        }



        private void LoadQuestions()
        {
            _questions.Clear();


            _questions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",

                Options = new List<string>
                {
                    "A) Reply with your password",
                    "B) Delete the email",
                    "C) Report the email as phishing",
                    "D) Ignore it"
                },

                CorrectAnswer = "C",

                Explanation =
                "Correct! Reporting phishing emails helps prevent scams."
            });



            _questions.Add(new QuizQuestion
            {
                Question =
                "True or False: You should reuse the same password on multiple websites.",

                Options = new List<string>
                {
                    "True",
                    "False"
                },

                CorrectAnswer = "False",

                Explanation =
                "Each account should have its own unique password."
            });



            _questions.Add(new QuizQuestion
            {
                Question =
                "Which password is strongest?",

                Options = new List<string>
                {
                    "A) password123",
                    "B) John2004",
                    "C) Summer",
                    "D) T!9#vQ8@Lp$21"
                },

                CorrectAnswer = "D",

                Explanation =
                "Long random passwords are harder to crack."
            });



            _questions.Add(new QuizQuestion
            {
                Question =
                "What is phishing?",

                Options = new List<string>
                {
                    "A) Catching fish",
                    "B) Fake messages used to steal information",
                    "C) Updating software",
                    "D) Installing Windows"
                },

                CorrectAnswer = "B",

                Explanation =
                "Phishing tricks users into revealing information."
            });



            _questions.Add(new QuizQuestion
            {
                Question =
                "What does 2FA mean?",

                Options = new List<string>
                {
                    "A) Two File Access",
                    "B) Two Factor Authentication",
                    "C) Two Firewall Access",
                    "D) Double Login"
                },

                CorrectAnswer = "B",

                Explanation =
                "2FA adds an extra layer of account security."
            });



            _questions.Add(new QuizQuestion
            {
                Question =
                "True or False: Software updates fix security vulnerabilities.",

                Options = new List<string>
                {
                    "True",
                    "False"
                },

                CorrectAnswer = "True",

                Explanation =
                "Updates often patch security weaknesses."
            });


        }





        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex >= _questions.Count)
                return null;


            return _questions[_currentIndex];
        }





        public bool SubmitAnswer(string answer)
        {
            QuizQuestion current = GetCurrentQuestion();


            if (current == null)
                return false;


            bool correct =
                answer.Trim()
                .Equals(
                    current.CorrectAnswer,
                    StringComparison.OrdinalIgnoreCase
                );


            if (correct)
                _score++;


            _currentIndex++;


            return correct;
        }






        public string GetFeedback(bool correct)
        {
            int index = _currentIndex - 1;


            if (index < 0)
                index = 0;


            return
                (correct ? "✅ Correct! " : "❌ Incorrect. ")
                +
                _questions[index].Explanation;
        }





        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }





        public (int score, int total, int percentage) GetFinalScore()
        {
            int percentage = 0;


            if (_questions.Count > 0)
            {
                percentage =
                    (_score * 100) /
                    _questions.Count;
            }


            return
            (
                _score,
                _questions.Count,
                percentage
            );
        }





        public string GetFinalMessage()
        {
            var result = GetFinalScore();


            if (result.percentage >= 80)
            {
                return
                "🏆 Excellent work!\n" +
                "You have strong cybersecurity awareness.";
            }


            if (result.percentage >= 50)
            {
                return
                "👍 Good job!\n" +
                "Keep improving your cybersecurity knowledge.";
            }


            return
            "⚠️ Keep practising.\n" +
            "Cybersecurity awareness improves with learning.";
        }





        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }

    }
}