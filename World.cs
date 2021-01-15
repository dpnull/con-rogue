using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class World
    {
        // Store created locations
        // TODO: Make locations private
        public List<Location> locations = new List<Location>();

        public void AddLocation(int x, int y, string name, string description)
        {
            Location location = new Location();
            location.X = x;
            location.Y = y;
            location.Name = name;
            location.Description = description;

            // Add newly created location into locations list
            locations.Add(location);
        }

        public Location GetLocation(int x, int y)
        {
            // Loop through locations list to get the specified location using provided x & y
            foreach (Location location in locations)
            {
                if (location.X == x && location.Y == y)
                {
                    return location;
                }
            }

            // No locations match; return null

            return null;
        }


        public void PrintGuiLocations(int x, int y)
        {
            int index = 0;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].Y == 0)
                {
                    y++;
                    Console.SetCursorPosition(x, y);
                    Console.Write("{0}) {1}", index += 1, locations[i].Name);
                }
            }
            /*
            foreach(Location location in locations)
            {
                y++;
                Console.SetCursorPosition(x, y);
                Console.Write(location.Name);                
            }
            */
        }

        public void TravelToLocation(string input, World world)
        {

            for (int i = 0; i < locations.Count; i++)
            {
                if (Int32.Parse(input) == locations[i].X) ///// STUPID SHIT
                {
                    world.GetLocation(locations[i].X, locations[i].Y);
                }
            }
        }

        // Getter for location count
        // Possible improvement ?
        public int GetLocationCount()
        {
            int count = 0;
            for (int i = 0; i < locations.Count; i++)
            {
                count++;
            }
            return count;
        }
    }
}
