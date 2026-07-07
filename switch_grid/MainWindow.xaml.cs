using System;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Windows;
using System.Drawing;

namespace switch_grid
{
    public partial class MainWindow : Window
    {//start of class

        // generic lists for replies and stopwords
        private List<string> reply = new List<string>();
        private List<string> ignore = new List<string>();

        // memory variables
        private string stored_username = "";
        private string last_keyword = "";
        private string stored_topic = "";

        // sentiment detector instance
        private SentimentDetector sentiment_detector = new SentimentDetector();

        // task manager instance
        private TaskManager task_manager = new TaskManager();

        // quiz manager instance
        private QuizManager quiz_manager = new QuizManager();
        public MainWindow()
        {//start of constructor
            InitializeComponent();

            // load answers and stopwords via legacy adapter
            ArrayList legacyReplyAdapter = new ArrayList();
            ArrayList legacyIgnoreAdapter = new ArrayList();
            respond bot = new respond(legacyReplyAdapter, legacyIgnoreAdapter);
            foreach (var item in legacyReplyAdapter) reply.Add(item.ToString());
            foreach (var item in legacyIgnoreAdapter) ignore.Add(item.ToString());

            // initialize database
            task_manager.InitializeDatabase();

            play_greeting();
            generate_ascii_logo();
        }//end of constructor

        private void generate_ascii_logo()
        {//start of generate_ascii_logo
            try
            {//start of try
                string full_path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.jpg");
                if (!File.Exists(full_path))
                    full_path = System.IO.Path.Combine(Environment.CurrentDirectory, "logo.jpg");
                if (!File.Exists(full_path))
                    full_path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "logo.jpg");

                if (File.Exists(full_path))
                {//start of if
                    using (Bitmap image = new Bitmap(full_path))
                    {//start of using
                        Bitmap resizedImage = new Bitmap(image, new System.Drawing.Size(210, 140));
                        StringBuilder asciiBuilder = new StringBuilder();

                        for (int height = 0; height < resizedImage.Height; height++)
                        {//start of outer loop
                            for (int width = 0; width < resizedImage.Width; width++)
                            {//start of inner loop
                                Color pixelColor = resizedImage.GetPixel(width, height);
                                int colorValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                                char ascii_design = colorValue > 200 ? '.' : colorValue > 150 ? '*' : colorValue > 100 ? 'O' : colorValue > 50 ? '#' : '@';
                                asciiBuilder.Append(ascii_design);
                            }//end of inner loop
                            asciiBuilder.AppendLine();
                        }//end of outer loop

                        ascii_logo_box.Text = asciiBuilder.ToString();
                    }//end of using
                }//end of if
            }//end of try
            catch (Exception ex)
            {//start of catch
                ascii_logo_box.Text = "Error loading logo: " + ex.Message;
            }//end of catch
        }//end of generate_ascii_logo

        private void play_greeting()
        {//start of play_greeting
            try
            {//start of try
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Soundd.wav");
                if (File.Exists(path))
                {//start of if
                    SoundPlayer player = new SoundPlayer(path);
                    player.Play();
                }//end of if
            }//end of try
            catch { }
        }//end of play_greeting

        private void start_ai(object sender, RoutedEventArgs e)
        {//start of start_ai
            logo_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }//end of start_ai

        private void submit_name(object sender, RoutedEventArgs e)
        {//start of submit_name
            string collect_username = user_name.Text.ToString().Trim();

            if (collect_username != "")
            {//start of if
                stored_username = collect_username;
                MessageBox.Show("Welcome back, agent " + stored_username + "!");
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;
                question.Focus();
                chats_list.Items.Add("CyberGuard : Hello " + stored_username + "! Let's secure your perimeter.");
                chats_list.Items.Add("CyberGuard : Ask me about: passwords, phishing, firewall, vpn, fraud, malware.");
                chats_list.Items.Add("CyberGuard : Type 'tasks' to open the task manager.");
                chats_list.Items.Add("──────────────────────────────────────────");
            }//end of if
            else
            {//start of else
                MessageBox.Show("Please enter your name.");
            }//end of else
        }//end of submit_name

        private void send_question(object sender, RoutedEventArgs e)
        {//start of send_question
            string user_input = question.Text.ToString().Trim();

            if (user_input != "")
            {//start of if
                chats_list.Items.Add(stored_username + " : " + user_input);
                string response = get_response(user_input);
                chats_list.Items.Add("CyberGuard : " + response);
                chats_list.Items.Add("──────────────────────────────────────────");
                question.Clear();
                chats_list.ScrollIntoView(chats_list.Items[chats_list.Items.Count - 1]);
            }//end of if
        }//end of send_question

        private string get_response(string user_input)
        {//start of get_response
            string lower_input = user_input.ToLower();

            // open task manager
            if (lower_input.Contains("tasks") || lower_input.Contains("task manager") || lower_input.Contains("add task") || lower_input.Contains("my tasks"))
            {//start of tasks if
                chats_grid.Visibility = Visibility.Hidden;
                tasks_grid.Visibility = Visibility.Visible;
                load_tasks();
                return "Opening task manager...";
            }//end of tasks if

            // memory: store favourite topic
            if (lower_input.Contains("i'm interested in") || lower_input.Contains("my favourite topic is"))
            {//start of memory if
                string topic = lower_input.Contains("i'm interested in") ?
                    lower_input.Replace("i'm interested in", "").Trim() :
                    lower_input.Replace("my favourite topic is", "").Trim();
                stored_topic = topic;
                return "Great! I'll remember that you're interested in " + stored_topic + ". It's a crucial part of staying safe online.";
            }//end of memory if

            // memory recall
            if (lower_input.Contains("what do you know about me") || lower_input.Contains("what do you remember"))
            {//start of recall if
                string recall = "Your name is " + stored_username + ".";
                if (stored_topic != "") recall += " You told me you're interested in " + stored_topic + ".";
                if (last_keyword != "") recall += " Last topic discussed: " + last_keyword + ".";
                return recall;
            }//end of recall if

            // follow up
            if (lower_input.Contains("tell me more") || lower_input.Contains("give me another tip") || lower_input.Contains("explain more"))
            {//start of follow up if
                if (last_keyword != "")
                    return get_answer_by_keyword(last_keyword);
                return "What topic would you like more info on?";
            }//end of follow up if

            // sentiment detection
            Sentiment sentiment = sentiment_detector.Detect(lower_input);
            if (sentiment != Sentiment.Neutral)
            {//start of sentiment if
                string sentiment_response = sentiment_detector.GetSentimentResponse(sentiment);
                last_keyword = sentiment.ToString().ToLower();
                string auto_tip = get_auto_tip(lower_input);
                if (auto_tip != "")
                    return sentiment_response + auto_tip;
                return sentiment_response;
            }//end of sentiment if

            // keyword matching
            string[] input_words = lower_input.Split(' ');
            foreach (string word in input_words)
            {//start of foreach
                string cleaned_word = word.Trim().ToLower();
                if (ignore.Contains(cleaned_word)) continue;

                foreach (string answer in reply)
                {//start of inner foreach
                    string keyword = answer.Split(' ')[0];
                    if (cleaned_word.Contains(keyword) || keyword.Contains(cleaned_word))
                    {//start of match if
                        last_keyword = keyword;
                        return get_answer_by_keyword(keyword);
                    }//end of match if
                }//end of inner foreach
            }//end of foreach

            return "Query unrecognised. Try: passwords, phishing, firewall, vpn, fraud, malware.";
        }//end of get_response

        private string get_answer_by_keyword(string keyword)
        {//start of get_answer_by_keyword
            List<string> matching = new List<string>();
            foreach (string answer in reply)
            {//start of foreach
                string answer_keyword = answer.Split(' ')[0];
                if (answer_keyword == keyword)
                    matching.Add(answer.Substring(keyword.Length).Trim());
            }//end of foreach

            if (matching.Count > 0)
            {//start of if
                Random random = new Random();
                return matching[random.Next(matching.Count)];
            }//end of if

            return "No matching records found.";
        }//end of get_answer_by_keyword

        private string get_auto_tip(string input)
        {//start of get_auto_tip
            if (input.Contains("scam") || input.Contains("fraud"))
            { last_keyword = "fraud"; return get_answer_by_keyword("fraud"); }
            if (input.Contains("password"))
            { last_keyword = "password"; return get_answer_by_keyword("password"); }
            if (input.Contains("phishing"))
            { last_keyword = "phishing"; return get_answer_by_keyword("phishing"); }
            if (input.Contains("hack"))
            { last_keyword = "hacked"; return get_answer_by_keyword("hacked"); }
            if (input.Contains("malware") || input.Contains("virus"))
            { last_keyword = "malicious"; return get_answer_by_keyword("malicious"); }
            if (input.Contains("vpn"))
            { last_keyword = "vpn"; return get_answer_by_keyword("vpn"); }
            return "";
        }//end of get_auto_tip

        // ── TASK MANAGER METHODS ──────────────────────────────────────────

        private void load_tasks()
        {//start of load_tasks
            tasks_list.Items.Clear();
            List<Task> tasks = task_manager.GetUserTasks(stored_username);

            if (tasks.Count == 0)
            {//start of if
                tasks_list.Items.Add("No tasks found. Add one below!");
            }//end of if
            else
            {//start of else
                foreach (Task task in tasks)
                {//start of foreach
                    string status = task.IsCompleted ? "✓" : "○";
                    tasks_list.Items.Add(status + " [" + task.TaskId + "] " + task.Title + " — " + task.Description + " | Reminder: " + task.ReminderDate.ToShortDateString());
                }//end of foreach
            }//end of else
        }//end of load_tasks

        private void add_task(object sender, RoutedEventArgs e)
        {//start of add_task
            string title = task_title.Text.ToString().Trim();
            string description = task_description.Text.ToString().Trim();

            if (title != "" && description != "")
            {//start of if
                DateTime reminder = DateTime.Now.AddDays(7);
                bool success = task_manager.AddTask(stored_username, title, description, reminder);

                if (success)
                {//start of success if
                    MessageBox.Show("Task added! Reminder set for: " + reminder.ToShortDateString());
                    task_title.Clear();
                    task_description.Clear();
                    load_tasks();
                }//end of success if
                else
                {//start of else
                    MessageBox.Show("Error adding task. Check database connection.");
                }//end of else
            }//end of if
            else
            {//start of outer else
                MessageBox.Show("Please enter a title and description.");
            }//end of outer else
        }//end of add_task

        private void complete_task(object sender, RoutedEventArgs e)
        {//start of complete_task
            if (tasks_list.SelectedItem != null)
            {//start of if
                string selected = tasks_list.SelectedItem.ToString();
                // extract task id from between brackets
                int start = selected.IndexOf('[') + 1;
                int end = selected.IndexOf(']');
                if (start > 0 && end > start)
                {//start of inner if
                    int task_id = int.Parse(selected.Substring(start, end - start));
                    task_manager.CompleteTask(task_id);
                    load_tasks();
                }//end of inner if
            }//end of if
            else
            {//start of else
                MessageBox.Show("Please select a task first.");
            }//end of else
        }//end of complete_task

        private void delete_task(object sender, RoutedEventArgs e)
        {//start of delete_task
            if (tasks_list.SelectedItem != null)
            {//start of if
                string selected = tasks_list.SelectedItem.ToString();
                int start = selected.IndexOf('[') + 1;
                int end = selected.IndexOf(']');
                if (start > 0 && end > start)
                {//start of inner if
                    int task_id = int.Parse(selected.Substring(start, end - start));
                    task_manager.DeleteTask(task_id);
                    load_tasks();
                }//end of inner if
            }//end of if
            else
            {//start of else
                MessageBox.Show("Please select a task first.");
            }//end of else
        }//end of delete_task

        private void back_to_chat(object sender, RoutedEventArgs e)
        {//start of back_to_chat
            tasks_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;
        }//end of back_to_chat

    }//end of class
}//end of namespace