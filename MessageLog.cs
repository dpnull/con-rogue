using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    // Represents a queue of messages that can be added to and drawn to a RLConsole
    public class MessageLog
    {
        // Define the maximum number of lines to store
        private static readonly int _maxLines = 6;

        // Use a Queue to keep track of the lines of text
        // The first line added to the log will also be the first removed
        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        // Add a line to the MessageLog queue
        public void Add(string message)
        {
            _lines.Enqueue(message);

            // When exceeding the maximum number of lines remove the oldest one.
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        public void AddEncounter(string str)
        {
            _lines.Enqueue($"You have encountered {str}!");

            // When exceeding the maximum number of lines remove the oldest one.
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        // Draw each line of the MessageLog queue to the console
        public void Draw(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            string[] lines = _lines.ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(lines[i]);
                y++;
            }
        }
    }
}
