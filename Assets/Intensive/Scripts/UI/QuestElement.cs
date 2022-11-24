using UnityEngine;
using UnityEngine.UI;

namespace Intensive.UI
{
    public class QuestElement : MonoBehaviour
    {
        [ReadOnly, SerializeField, Tooltip("Текст задания")]
        private Text _text;
        [ReadOnly, SerializeField, Tooltip("Окошко для невыполненного задания")]
        private Image _background;
        [ReadOnly, SerializeField, Tooltip("Галочка выполненного задания")]
        private Image _toggle;

        [Tooltip("Цвет задника галочки, после завершения квеста"), SerializeField]
        private Color _completeColorBackground;
        [Tooltip("Цвет текста, после завершения квеста"), SerializeField]
        private Color _completeColorText;



        /// <summary>
        /// Устанавливает и возвращает текст квеста
        /// </summary>
        public string Text { get => _text.text; set => _text.text = value; }
        /// <summary>
        /// Выполнен-ли этот квест
        /// </summary>
        public bool IsCompleted { get; private set; }



		private void OnValidate()
		{
            _text = transform.Find("Label").GetComponent<Text>();
            _background = transform.Find("Background").GetComponent<Image>();
            _toggle = _background.transform.Find("Checkmark").GetComponent<Image>();
        }

        public void Completed()
        {
            IsCompleted = true;
            _text.color = _completeColorText;
            _background.color = _completeColorBackground;
            _toggle.enabled = true;
        }
	}
}