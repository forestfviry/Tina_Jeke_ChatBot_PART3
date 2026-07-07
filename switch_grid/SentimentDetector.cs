using System;
using System.Collections.Generic;

namespace switch_grid
{
    public enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy }

    public class SentimentDetector
    {//start of class

        // dictionary mapping sentiment to trigger words
        private Dictionary<Sentiment, List<string>> sentiment_words = new Dictionary<Sentiment, List<string>>();

        public SentimentDetector()
        {//start of constructor
            sentiment_words[Sentiment.Worried] = new List<string>
            {
                "worried", "scared", "afraid", "anxious", "nervous", "unsafe", "overwhelmed"
            };

            sentiment_words[Sentiment.Curious] = new List<string>
            {
                "curious", "wondering", "interested", "want to know", "how does"
            };

            sentiment_words[Sentiment.Frustrated] = new List<string>
            {
                "frustrated", "annoyed", "confused", "don't understand"
            };

            sentiment_words[Sentiment.Happy] = new List<string>
            {
                "great", "thanks", "helpful", "awesome", "love it", "happy", "excited", "good"
            };
        }//end of constructor

        public Sentiment Detect(string input)
        {//start of Detect
            string lower_input = input.ToLower();

            foreach (KeyValuePair<Sentiment, List<string>> entry in sentiment_words)
            {//start of foreach
                foreach (string trigger in entry.Value)
                {//start of inner foreach
                    if (lower_input.Contains(trigger))
                    {//start of if
                        return entry.Key;
                    }//end of if
                }//end of inner foreach
            }//end of foreach

            return Sentiment.Neutral;
        }//end of Detect

        public string GetSentimentResponse(Sentiment s)
        {//start of GetSentimentResponse
            if (s == Sentiment.Worried)
            {//start of worried if
                return "It's completely understandable to feel that way. Cyber threats can be overwhelming. Let me share some tips to help you stay safe: ";
            }//end of worried if

            if (s == Sentiment.Curious)
            {//start of curious if
                return "Great question! I love the curiosity. Here's what you should know: ";
            }//end of curious if

            if (s == Sentiment.Frustrated)
            {//start of frustrated if
                return "I understand your frustration. Let me break it down clearly for you. ";
            }//end of frustrated if

            if (s == Sentiment.Happy)
            {//start of happy if
                return "Great energy! Staying proactive and positive is key to solid security. Here's a quick tip: ";
            }//end of happy if

            return "";
        }//end of GetSentimentResponse

    }//end of class
}//end of namespace