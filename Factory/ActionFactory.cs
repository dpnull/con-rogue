using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue.Factory
{
    public class ActionFactory
    {
        public Action CreateAction()
        {
            Action action = new Action();

            action.AddAction("travel", 't', "Travel", true);
            action.AddAction("exit", 'x', "Exit", false);
            action.AddAction("blacksmith", 'b', "Blacksmith", false);
            action.AddAction("market", 'm', "Market", false);
            action.AddAction("inventory", 'i', "Inventory", true);
            action.AddAction("misc_tab", 'm', "Miscelleanous", false);
            action.AddAction("weapons_tab", 'w', "Weapons", false);
            action.AddAction("hunt", 'h', "Hunt", false);
            action.AddAction("gather", 'g', "Gather", false);
            action.AddAction("attack", 'a', "Attack", false);

            return action;
        }

        public Action CreateBattleAction()
        {
            Action battleAction = new Action();
            battleAction.AddAction("attack", 'a', "Attack", false);
            battleAction.AddAction("defend", 'd', "Defend", false);
            battleAction.AddAction("retreat", 'r', "Retreat", false);
            return battleAction;
        }
        // Can be seen in travel menu?
        // Can be seen in inventory menu?
        // 
    }
}
