using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace con_rogue.Factory
{
    public static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            List<ItemBundle> itemsToComplete = new List<ItemBundle>();
            List<ItemBundle> rewardItems = new List<ItemBundle>();

            itemsToComplete.Add(new ItemBundle(1001, 1));
            rewardItems.Add(new ItemBundle(2003, 1));

            // Create the quest
            _quests.Add(new Quest(1, "Spider Trouble", "Obtain 1 spider silk", itemsToComplete, 20, 10, rewardItems));
        }

        // Get the first matching quest by passed ID
        public static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
