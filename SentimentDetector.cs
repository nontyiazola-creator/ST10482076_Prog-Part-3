using System.Collections.Generic;

namespace CybersecurityChatbott
{
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    public class SentimentDetector
    {
        private Dictionary<Sentiment, List<string>>
            _triggers;

        public SentimentDetector()
        {
            _triggers =
            new Dictionary<Sentiment, List<string>>
            {
                {
                    Sentiment.Worried,
                    new List<string>
                    {
                        "worried",
                        "scared",
                        "afraid",
                        "anxious"
                    
                    }
                },

                {
                    Sentiment.Frustrated,
                    new List<string>
                    {
                        "confused",
                        "annoyed",
                        "frustrated"
                    }
                },

                {
                    Sentiment.Happy,
                    new List<string>
                    {
                        "great",
                        "awesome",
                        "love"
                    }
                }
                ,
                {
                    Sentiment.Curious,
                    new List<string>
                    {
                        "curious",
                        "interested",
                        "wondering",
                        "tell me more",
                        "explain",
                        "what is"
                    }
                }
            };
        }

        public Sentiment Detect(string input)
        {
            input = input.ToLower();

            foreach (var sentiment in _triggers)
            {
                foreach (var word in sentiment.Value)
                {
                    if (input.Contains(word))
                    {
                        return sentiment.Key;
                    }
                }
            }

            return Sentiment.Neutral;
        }
    }
}