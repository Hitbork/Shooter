using Intensive.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Intensive.Commands
{
    public class DefenseQuestCommand : QuestCommand, IDependentQuest<EnemyData, SpawnComponent>
    {
        public void CheckComplete(LinkedList<EnemyData> enemies, LinkedList<SpawnComponent> spawns)
        {
            if (spawns.Count(t => t.QuestID == QuestID) > 0) return;
            if (enemies.Count(t => t.QuestID == QuestID) > 0) return;

            Complete();
        }
    }
}
