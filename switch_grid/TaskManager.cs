using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

namespace switch_grid
{
    public class Task
    {//start of class
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Username { get; set; }
    }//end of class

    public class TaskManager
    {//start of class

        // connection string to mysql database
        private string connectionString = "Server=localhost;Database=cybersecurity_chatbot;Uid=root;Pwd=root1234;";

        public bool AddTask(string username, string title, string description, DateTime reminderDate)
        {//start of AddTask
            try
            {//start of try
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {//start of using
                    connection.Open();
                    string query = "INSERT INTO tasks (username, title, description, reminder_date, is_completed) VALUES (@username, @title, @description, @reminderDate, 0)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {//start of inner using
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", reminderDate);
                        cmd.ExecuteNonQuery();
                        return true;
                    }//end of inner using
                }//end of using
            }//end of try
            catch (Exception ex)
            {//start of catch
                // show real error message for debugging
                MessageBox.Show("DB Error: " + ex.Message);
                return false;
            }//end of catch
        }//end of AddTask

        public List<Task> GetUserTasks(string username)
        {//start of GetUserTasks
            List<Task> tasks = new List<Task>();

            try
            {//start of try
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {//start of using
                    connection.Open();
                    string query = "SELECT task_id, username, title, description, reminder_date, is_completed FROM tasks WHERE username = @username";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {//start of inner using
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {//start of reader using
                            while (reader.Read())
                            {//start of while
                                Task task = new Task
                                {
                                    TaskId = (int)reader["task_id"],
                                    Username = (string)reader["username"],
                                    Title = (string)reader["title"],
                                    Description = (string)reader["description"],
                                    ReminderDate = (DateTime)reader["reminder_date"],
                                    IsCompleted = (bool)reader["is_completed"]
                                };
                                tasks.Add(task);
                            }//end of while
                        }//end of reader using
                    }//end of inner using
                }//end of using
            }//end of try
            catch (Exception ex)
            {//start of catch
                MessageBox.Show("DB Error: " + ex.Message);
            }//end of catch

            return tasks;
        }//end of GetUserTasks

        public bool CompleteTask(int taskId)
        {//start of CompleteTask
            try
            {//start of try
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {//start of using
                    connection.Open();
                    string query = "UPDATE tasks SET is_completed = 1 WHERE task_id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {//start of inner using
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        cmd.ExecuteNonQuery();
                        return true;
                    }//end of inner using
                }//end of using
            }//end of try
            catch (Exception ex)
            {//start of catch
                MessageBox.Show("DB Error: " + ex.Message);
                return false;
            }//end of catch
        }//end of CompleteTask

        public bool DeleteTask(int taskId)
        {//start of DeleteTask
            try
            {//start of try
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {//start of using
                    connection.Open();
                    string query = "DELETE FROM tasks WHERE task_id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {//start of inner using
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        cmd.ExecuteNonQuery();
                        return true;
                    }//end of inner using
                }//end of using
            }//end of try
            catch (Exception ex)
            {//start of catch
                MessageBox.Show("DB Error: " + ex.Message);
                return false;
            }//end of catch
        }//end of DeleteTask

        public void InitializeDatabase()
        {//start of InitializeDatabase
            try
            {//start of try
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {//start of using
                    connection.Open();
                    string query = @"
                        CREATE TABLE IF NOT EXISTS tasks (
                            task_id INT AUTO_INCREMENT PRIMARY KEY,
                            username VARCHAR(100) NOT NULL,
                            title VARCHAR(255) NOT NULL,
                            description TEXT,
                            reminder_date DATETIME,
                            is_completed BOOLEAN DEFAULT 0,
                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {//start of inner using
                        cmd.ExecuteNonQuery();
                    }//end of inner using
                }//end of using
            }//end of try
            catch (Exception ex)
            {//start of catch
                MessageBox.Show("DB Init Error: " + ex.Message);
            }//end of catch
        }//end of InitializeDatabase

    }//end of class
}//end of namespace