using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace switch_grid
{
    public class ActivityLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }

    public class ActivityLogManager
    {
        private List<ActivityLogEntry> entries = new List<ActivityLogEntry>();

        public void LogAction(string description)
        {//start of LogAction
            entries.Add(new ActivityLogEntry { Timestamp = DateTime.Now, Description = description });
        }//end of LogAction

        public List<ActivityLogEntry> GetRecentActions(int count = 10)
        {//start of GetRecentActions
            // most recent first when stored, but displayed oldest-to-newest for readability
            return entries.OrderByDescending(e => e.Timestamp).Take(count).Reverse().ToList();
        }//end of GetRecentActions

        public string FormatLog(int count = 10)
        {//start of FormatLog
            List<ActivityLogEntry> recent = GetRecentActions(count);
            if (recent.Count == 0)
                return "No actions logged yet.";

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Here's a summary of recent actions:");
            int number = 1;
            foreach (ActivityLogEntry entry in recent)
            {//start of foreach
                builder.AppendLine(number + ". " + entry.Description + " (" + entry.Timestamp.ToString("MMM d, h:mm tt") + ")");
                number++;
            }//end of foreach
            return builder.ToString().Trim();
        }//end of FormatLog
    }
}