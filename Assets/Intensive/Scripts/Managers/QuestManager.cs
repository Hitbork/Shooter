using Intensive.Commands;
using Intensive.GameObjects;
using Intensive.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Intensive.Managers
{
    public class QuestManager : MonoBehaviour
    {
        //Рефлексия комманд
        private Dictionary<Type, object> _refCollection;
        private MethodInfo _method;
        private object[] _parameters;

        private LinkedList<QuestCommand> _quests;
        private Transform _triggersPool;

        [Inject]
        private LinkedList<EnemyData> _enemies;
        [Inject]
        private LinkedList<SpawnComponent> _spawners;
        [Inject]
        private LinkedList<TriggerComponent> _triggers;


        /// <summary>
        /// Событие изменения статуса квеста
        /// </summary>
        public event QuestEventHandler OnQuestStatusChangedEvent;

        public delegate void QuestEventHandler(string id, QuestEventType type);

		private void Awake()
		{
            _quests = new LinkedList<QuestCommand>(new QuestCommand[]
            {
                new DefenseQuestCommand { QuestID = "quest_def" },
                new TriggerQuestCommand{ QuestID = "quest_connection" },
                new TriggerQuestCommand{ QuestID = "quest_battery" },
                new TriggerQuestCommand{ QuestID = "quest_fix" },
                new DefenseQuestCommand { QuestID = "quest_attack" },
            });

            //Привязка на изменение состояния квестов
            foreach(var quest in _quests)
            {
                quest.OnQuestStateChanged += OnQuestChanged;
			}
		}

        public void OnStart()
        {
            var quest = _quests.First.Value;
            quest.StartQuest();
		}

        //Обработка изменения состояния квестов
        private void OnQuestChanged(object sender, QuestEventType arg)
        {
            var quest = (QuestCommand)sender;
            switch (arg)
            {
                case QuestEventType.Start:
                    ActivatedGameObjects();
                    break;
                //Удаляем завершенный квест и запускаем следующий
                case QuestEventType.Completed:
                    _quests.Remove(quest);
                    if (_quests.Count == 0) Time.timeScale = 0;//todo
                    else _quests.First.Value.StartQuest();
                    break;
			}

            //Оповещение об изменение статуса квеста
            OnQuestStatusChangedEvent?.Invoke(quest.QuestID, arg);
        }

		private void Start()
		{
            foreach (var spawner in _spawners) spawner.OnDiactivatedEvent += (q, qq) => OnCheckQuest();
            foreach (var trigger in _triggersPool.GetComponentsInChildren<TriggerComponent>(true)) _triggers.AddLast(trigger);

            //Подписка на срабатывание триггеров
            foreach (var trigger in _triggers)
            {
                trigger.OnTriggerEnterEvent += OnTriggerCall;
                trigger.OnDiactivatedEvent += (q, qq) =>
                {
                    _triggers.Remove((TriggerComponent)q);
                    OnCheckQuest();
                };
            }
            //Конфигурация коллекции зависимостей комманд
            _refCollection = new Dictionary<Type, object>
            {
                { typeof(EnemyData), _enemies },
                { typeof(SpawnComponent), _spawners },
                { typeof(TriggerComponent), _triggers },
            };
        }

        private void OnValidate()
        {
            _triggersPool = GetComponentsInChildren<Transform>(true).First(t => t.name == "TriggersPool");
        }

        //Проверка на выполнение квеста
        public void OnCheckQuest()
        {
            _method.Invoke(_quests.First.Value, _parameters);
        }

        //Активация игровых объектов по идентификатору
        private void ActivatedGameObjects()
        {
            var quest = _quests.First.Value;

            var type = quest.GetType();
            var inter = type.GetInterfaces().First(t => t.Name.Contains("IDependentQuest"));
            _method = inter.GetMethod("CheckComplete");
            var args = inter.GetGenericArguments();
            _parameters = new object[args.Length];
            for (int i = 0; i < _parameters.Length; i++)
            {
                _parameters[i] = _refCollection[args[i]];
            }

            foreach (var spawner in _spawners)
            {
                if (spawner.QuestID == quest.QuestID) spawner.Activate();
			}
            foreach(var trigger in _triggers)
            {
                if (trigger.QuestID == quest.QuestID) trigger.Activate();
			}
		}

        //Обработка срабатывания триггера
        private void OnTriggerCall(object sender, string id)
        {
            
		}
    }
}