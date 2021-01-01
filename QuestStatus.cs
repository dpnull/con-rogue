using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class QuestStatus
    {
        // Hold player's quest
        public Quest PlayerQuest { get; set; }
        public bool IsCompleted { get; set; }

        // Initialize every quest's completion as false by default
        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
            IsCompleted = false;
        }
    }
}
