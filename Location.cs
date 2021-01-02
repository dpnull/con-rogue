using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using con_rogue.Factory;

namespace con_rogue
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // Initializing removes need for constructor
        public List<Quest> QuestsAvailable { get; set; } = new List<Quest>();
        public List<EnemyEncounter> EnemiesHere { get; set; } = new List<EnemyEncounter>();

        public Location()
        {
            X = 0;
            Y = 0;
            Name = "Home";
            Description = "This is your house.";
        }

        // Dont get this too well
        public void AddEnemy(int enemyID, int encounterChance)
        {
            // Get the first matching ID of enemy
            if (EnemiesHere.Exists(e => e.EnemyID == enemyID))
            {
                // If enemy of this ID exists, override the encounter chance
                EnemiesHere.First(e => e.EnemyID == enemyID).EncounterChance = encounterChance;
            }
            else
            {
                // If enemy of this ID does not exist, add it to the property
                EnemiesHere.Add(new EnemyEncounter(enemyID, encounterChance));
            }
        }

        public Enemy GetEnemy()
        {
            // If the list is empty return null
            if (!EnemiesHere.Any())
            {
                return null;
            }

            // Add up encounter chance of every enemy in the locations
            int totalChance = EnemiesHere.Sum(e => e.EncounterChance);

            // Select random number from 1 to total encounter chance based off all enemies in location
            int random = RNG.Generator(1, totalChance);

            int runningTotal = 0;

            // Get to understand this more...
            foreach (EnemyEncounter enemyEncounter in EnemiesHere)
            {
                runningTotal += enemyEncounter.EncounterChance;
                if (random <= runningTotal)
                {
                    return EnemyFactory.CreateEnemy(enemyEncounter.EnemyID);
                }
            }

            // If ID couldn't be found, return the last correct ID
            return EnemyFactory.CreateEnemy(EnemiesHere.Last().EnemyID);
        }

    }
}
