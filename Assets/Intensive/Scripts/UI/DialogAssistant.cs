using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Intensive.UI
{
    public class DialogAssistant : MonoBehaviour
    {
		private Coroutine _coroutine;
		private Queue<(string, string, Sprite)> _dialogs = new Queue<(string, string, Sprite)>();

		[ReadOnly, SerializeField]
		private Image _face;
		[ReadOnly, SerializeField]
		private Text _name;
		[ReadOnly, SerializeField]
		private Text _dialog;

		[Tooltip("Задержка на печать одного символа"), SerializeField]
		private float _charDelay = .1f;
		[Tooltip("Задержка на печать конца предложения"), SerializeField]
		private float _endDelay = 1f;
		[Tooltip("Задержка на окончание фразы"), SerializeField]
		private float _finishDelay = 4.25f;

		/// <summary>
		/// Сообщает, активен-ли диалог
		/// </summary>
		public bool IsShow => _coroutine != null;

		private void OnValidate()
		{
			_face = transform.Find("PortraitImage").GetComponent<Image>();
			_name = _face.transform.Find("NameText").GetComponent<Text>();
			_dialog = _face.transform.Find("DialogText").GetComponent<Text>();
		}

		/// <summary>
		/// Добавляет в очередь на отыгровку диалог
		/// </summary>
		/// <param name="name">Имя говорящего</param>
		/// <param name="text">Слова говорящего</param>
		/// <param name="sprite">Портрет говорящего</param>
		public void AddQueue(string name, string text, Sprite sprite = null)
		{
			_dialogs.Enqueue((name, text, sprite));
		}

		/// <summary>
		/// Запускает диалоги
		/// </summary>
		public bool StartDialog()
		{
			if (_dialogs.Count == 0) return false;
			if (_coroutine != null) return true;

			gameObject.SetActive(true);
			_coroutine = StartCoroutine(OnPlay());
			return true;
		}

		/// <summary>
		/// Отыгрывает один диалог
		/// </summary>
		/// <param name="name">Имя говорящего</param>
		/// <param name="text">Слова говорящего</param>
		/// <param name="sprite">Портрет говорящего</param>
		public void ShowDialog(string name, string text, Sprite sprite = null)
		{
			if (_coroutine != null) return;

			gameObject.SetActive(true);
			_dialogs.Enqueue((name, text, sprite));
			_coroutine = StartCoroutine(OnPlay());
		}

		/// <summary>
		/// Останавливает диалоги
		/// </summary>
		public void StopDialog()
		{
			StopCoroutine(_coroutine);
			_dialogs.Clear();
			gameObject.SetActive(false);
		}

		private IEnumerator OnPlay()
		{
			while(_dialogs.Count != 0)
			{
				var dialog = _dialogs.Dequeue();

				//Установка имени и портрета
				_dialog.text = string.Empty;
				_name.text = dialog.Item1;
				if (dialog.Item3 != null)
				{
					_face.sprite = dialog.Item3;
					_face.enabled = true;
				}
				else _face.enabled = false;

				//Пока вся фраза не будет напечатана
				for(int i = 0; i < dialog.Item2.Length; i++)
				{
					_dialog.text += dialog.Item2[i];

					yield return dialog.Item2[i] == '.' || dialog.Item2[i] == '!' || dialog.Item2[i] == '?'
						? new WaitForSeconds(_endDelay)
						: new WaitForSeconds(_charDelay);
				}

				yield return new WaitForSeconds(_finishDelay);
			}

			_coroutine = null;
			gameObject.SetActive(false);
		}
	}
}
