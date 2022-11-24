using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Intensive.UI
{
    public class QuestAssistant : MonoBehaviour
    {
        private List<QuestElement> _quests = new List<QuestElement>();

        [Tooltip("Шаблон квеста"), ReadOnly, SerializeField]
        private QuestElement _questElementPrefab;



        /// <summary>
        /// Добавление нового квеста
        /// </summary>
        /// <param name="text">Текстовое описание квеста</param>
        public void UpdateQuestState(string text, QuestEventType type)
        {
            if (type == QuestEventType.Completed) QuestComplete();
            else if (type == QuestEventType.Start)
            {
                var quest = Instantiate(_questElementPrefab, transform);
                quest.gameObject.SetActive(true);
                quest.Text = text;

                _quests.Add(quest);
            }
        }

        /// <summary>
        /// Выполнение квеста
        /// </summary>
        private void QuestComplete()
        {
            var quest = _quests.FirstOrDefault(t => !t.IsCompleted);

            if (quest == null) Debug.LogError("Отсутствует выполняемый квест");
            else quest.Completed();
		}



        private void Start()
        {
            _questElementPrefab.gameObject.SetActive(false);
        }

        private void OnValidate()
		{
            _questElementPrefab = transform.Find("QuestTemplate").GetComponent<QuestElement>();
		}
	}
}