using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class Action
    {
        public List<Action> actions = new List<Action>();
        public string SID { get; set; }
        public char Keybind { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }


        public void AddAction(string sid, char keybind, string name, bool isVisible)
        {
            Action action = new Action();
            action.SID = sid;
            action.Keybind = keybind;
            action.Name = name;
            action.IsVisible = isVisible;

            actions.Add(action);
        }

        public void HideActions()
        {
            foreach (Action action in actions)
            {
                action.IsVisible = false;
            }
        }

        public char GetKeybind(string sid)
        {
            foreach (Action action in actions)
            {
                if (action.SID == sid)
                {
                    return action.Keybind;
                }
            }

            return '\0';
        }

        public void PrintActions(int x, int y)
        {
            foreach (Action action in actions)
            {
                if (action.IsVisible == true)
                {
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.Write("{0}) {1}", action.Keybind.ToString(), action.Name);
                }
            }
        }

        // Currently does not work
        public void AutoVisibility(string sid, bool condition)
        {
            foreach (Action action in actions)
            {
                if (action.SID == sid)
                {
                    if(condition == true)
                    {
                        MakeVisible(sid);
                    }
                    if(condition == false)
                    {
                        MakeHidden(sid);
                    }
                }
                
            }

        }

        public void MakeVisible(string sid)
        {
            foreach (Action action in actions)
            {
                if (action.SID == sid)
                {
                    action.IsVisible = true;
                }
            }
        }

        public void MakeHidden(string sid)
        {
            foreach (Action action in actions)
            {
                if (action.SID == sid)
                {
                    action.IsVisible = false;
                }
            }
        }

        public bool GetActionState(string sid)
        {
            foreach (Action action in actions)
            {
                if (action.SID == sid)
                {
                    return action.IsVisible;
                }
            }

            return false;
        }
    }

}
