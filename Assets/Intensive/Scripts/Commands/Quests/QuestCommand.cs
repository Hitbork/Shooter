using Intensive.Units;
using System;
using System.Linq;
using System.Collections.Generic;
using Intensive.GameObjects;
using UnityEngine;

namespace Intensive.Commands
{
    public class QuestCommand : BaseCommand
    {
        public QuestEventType State { get; private set; }
        public string QuestID { get; set; }

        /// <summary>
        /// Событие обновления состояния квеста
        /// </summary>
        public event EventHandler<QuestEventType> OnQuestStateChanged;

        public void StartQuest()
        {
            State = QuestEventType.Start;
            OnQuestStateChanged?.Invoke(this, State);
		}

        protected void Complete()
        {
            State = QuestEventType.Completed;
            OnQuestStateChanged?.Invoke(this, State);
            Debug.Log("Квест выполнен: " + QuestID);
        }
    }

    public interface IDependentQuest<T1>
    {
        void CheckComplete(LinkedList<T1> a);
    }

    public interface IDependentQuest<T1, T2>
    {
        void CheckComplete(LinkedList<T1> a, LinkedList<T2> b);
    }

    public interface IDependentQuest<T1, T2, T3>
    {
        void CheckComplete(LinkedList<T1> a, LinkedList<T2> b, LinkedList<T3> c);
    }

    public interface IDependentOnEnemies
    {
        void CheckComplete(LinkedList<EnemyData> enemy);
	}

    public interface IDependentOnSpawners
    {
        void CheckComplete(LinkedList<SpawnComponent> spawners);
    }

    public interface IDependendOnTriggers
    {
        void CheckComplete(LinkedList<TriggerComponent> triggers);
    }
}
