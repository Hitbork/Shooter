using Intensive.GameObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Intensive.Commands
{
    public class TriggerQuestCommand : QuestCommand, IDependentQuest<TriggerComponent>
    {
        public void CheckComplete(LinkedList<TriggerComponent> triggers)
        {
            if (triggers.Count(t => t.QuestID == QuestID) > 0) return;

            Complete();
        }
    }
}
