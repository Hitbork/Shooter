using Intensive.Commands;
using Intensive.GameObjects;
using Intensive.ScriptableObjects;
using Intensive.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Intensive.Managers
{
    public class InterfaceManager : MonoBehaviour
    {
		private QuestAssistant _questAssistant;
		private TooltipAssistant _tooltipAssistant;
		private DialogAssistant _dialogAssistant;

		[Inject]
        private QuestManager _questManager;
		[Inject(Id = "Configuration")]
		private QuestConfiguration _questContext;
		[Inject(Id = "Configuration")]
		private DialogConfiguration _dialogContext;

		private void Start()
		{
			_questManager.OnQuestStatusChangedEvent += (id, type) => _questAssistant.UpdateQuestState(_questContext.GetValue(id), type);
			_questManager.OnQuestStatusChangedEvent += OnDialogs;
		}

		public void OnStart()
		{
			_questAssistant.gameObject.SetActive(true);
			_tooltipAssistant.Construct("/Intensive/Configurations/");
		}

		private void OnValidate()
		{
			var canvas = FindObjectsOfType<Canvas>().First(t=> t.name == "Canvas");
			_questAssistant = canvas.GetComponentInChildren<QuestAssistant>(true);
			_tooltipAssistant = canvas.GetComponentInChildren<TooltipAssistant>(true);
			_dialogAssistant = canvas.GetComponentInChildren<DialogAssistant>(true);
		}

		private void OnDialogs(string key, QuestEventType args)//todo Можно сделать проверку состояния квеста и настраивать момент отыгрыша
		{
			if(args != QuestEventType.Start) return;
			foreach (var data in _dialogContext.GetData.First(t=> t.Key == key).Context)
				_dialogAssistant.AddQueue(data.Key, data.Value);

			_dialogAssistant.StartDialog();
		}
	}
}
