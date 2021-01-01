using System;
using System.Collections.Generic;
using System.Text;

namespace con_rogue
{
    public class EnemyEncounter
    {
        public int EnemyID { get; set; }
        public int EncounterChance { get; set; }

        public EnemyEncounter(int enemyID, int encounterChance)
        {
            EnemyID = enemyID;
            EncounterChance = encounterChance;
        }
    }
}
