using System;
using System.Collections.Generic;
using System.Linq;

namespace switch_grid
{
    public enum QuestionType { MultipleChoice, TrueFalse }

    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }

    public class QuizManager
    {
        private List<QuizQuestion> question_bank = new List<QuizQuestion>();
        private List<QuizQuestion> session_questions = new List<QuizQuestion>();
        private int current_index = 0;
        private int score = 0;

        public QuizManager()
        {
            build_question_bank();
        }

        private void build_question_bank()
        {
            question_bank.Add(new QuizQuestion
            {
                QuestionText = "What should you do if you receive an email asking for your password?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Reporting phishing emails helps prevent scams and alerts others to the threat."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "A strong password should include a mix of letters, numbers, and symbols.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Mixing character types makes passwords much harder to guess or crack."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "Which of these is the safest way to browse on public Wi-Fi?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Log into your bank account directly", "B) Use a VPN", "C) Disable your firewall", "D) Share your screen" },
                CorrectAnswer = "B",
                Explanation = "A VPN encrypts your traffic, protecting your data from others on the same network."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "It is safe to reuse the same password across multiple accounts.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Reusing passwords means one breach can compromise all your accounts."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "What is 'social engineering' in cybersecurity?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Building secure networks", "B) Manipulating people into revealing information", "C) Writing antivirus software", "D) Encrypting files" },
                CorrectAnswer = "B",
                Explanation = "Social engineering relies on psychological manipulation rather than technical hacking."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "Two-factor authentication (2FA) adds an extra layer of account security.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "2FA requires a second proof of identity, making accounts much harder to break into."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "You get a call from someone claiming to be IT support, asking for your login details. What should you do?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Give them the details immediately", "B) Hang up and verify through official channels", "C) Ask them to email it instead", "D) Give a fake password" },
                CorrectAnswer = "B",
                Explanation = "Legitimate IT staff won't need to ask for your password; always verify independently."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "A firewall can help block unauthorized access to your network.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Firewalls filter incoming and outgoing traffic based on security rules."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "Which of these links is most likely a phishing attempt?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) www.yourbank.com", "B) www.your-bank-secure-login123.com", "C) www.yourbank.com/login", "D) accounts.yourbank.com" },
                CorrectAnswer = "B",
                Explanation = "Odd, lengthy, or misspelled domains are a common sign of phishing links."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "You should update your software and apps regularly.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "True",
                Explanation = "Updates often patch security vulnerabilities that attackers could exploit."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "What does 'malware' refer to?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Malicious software designed to harm systems", "B) A type of firewall", "C) A strong password", "D) A backup tool" },
                CorrectAnswer = "A",
                Explanation = "Malware includes viruses, ransomware, spyware, and other harmful programs."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "Clicking links in unexpected emails from unknown senders is generally safe.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Unexpected links can lead to phishing pages or trigger malware downloads."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "What is the main purpose of a VPN?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Speed up your internet", "B) Encrypt your traffic and protect privacy", "C) Block all cookies", "D) Store passwords" },
                CorrectAnswer = "B",
                Explanation = "A VPN encrypts your connection, hiding your activity from prying eyes on the network."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "Sharing your one-time passcode (OTP) with a caller who says they're from your bank is safe.",
                Type = QuestionType.TrueFalse,
                Options = new List<string> { "True", "False" },
                CorrectAnswer = "False",
                Explanation = "Banks never ask for OTPs by phone; sharing one can let attackers into your account."
            });

            question_bank.Add(new QuizQuestion
            {
                QuestionText = "Which detail is a red flag of a phishing email?",
                Type = QuestionType.MultipleChoice,
                Options = new List<string> { "A) Correct spelling and grammar", "B) Urgent language demanding immediate action", "C) An official company logo", "D) Being addressed by your name" },
                CorrectAnswer = "B",
                Explanation = "Attackers create urgency to pressure victims into acting before they think it through."
            });
        }

        public void StartNewQuiz(int number_of_questions = 10)
        {
            Random random = new Random();
            session_questions = question_bank.OrderBy(q => random.Next()).Take(Math.Min(number_of_questions, question_bank.Count)).ToList();
            current_index = 0;
            score = 0;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (current_index < session_questions.Count)
                return session_questions[current_index];
            return null;
        }

        public bool SubmitAnswer(string chosen_answer)
        {
            QuizQuestion current = GetCurrentQuestion();
            if (current == null) return false;

            bool is_correct = string.Equals(chosen_answer.Trim(), current.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase);
            if (is_correct) score++;
            return is_correct;
        }

        public bool MoveNext()
        {
            current_index++;
            return current_index < session_questions.Count;
        }

        public int GetScore() { return score; }
        public int GetTotalQuestions() { return session_questions.Count; }
        public int GetCurrentQuestionNumber() { return current_index + 1; }

        public string GetFinalFeedback()
        {
            double percentage = (double)score / GetTotalQuestions() * 100;
            if (percentage >= 80) return "Great job! You're a cybersecurity pro!";
            if (percentage >= 50) return "Not bad! A little more practice and you'll be a cybersecurity pro.";
            return "Keep learning to stay safe online!";
        }
    }
}