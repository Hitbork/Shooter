using Intensive.Managers;
using Intensive.Units.Player;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Zenject;

namespace Intensive
{
    public class CinematicCameraComponent : MonoBehaviour
    {
        [Inject]
		private GameManager _gameManager;

		[Tooltip("Кнопка пропуска синематика")]
        [SerializeField]
        private InputAction _startGameInputAction;

        [Tooltip("Падающий корабль")]
        [SerializeField]
        private ShipCrashComponent _ship;

        [Tooltip("Черный экран в начале миссии")]
        [SerializeField]
        private Image _blackScreen;

        [Tooltip("Панель пропуска заставки")]
        [SerializeField]
        private GameObject _skipPanel;

        [SerializeField]
        private Intensive.UI.TooltipAssistant Tooltip;

		private async void Start()
		{
            Tooltip.gameObject.SetActive(true);
            StartCoroutine(BlackScreenOff());
        }

		private void OnEnable()
		{
            _startGameInputAction.Enable();
            _startGameInputAction.performed += q => OnSkipCinematic();

        }

        private void OnDisable()
        {
            _startGameInputAction.performed -= q => OnSkipCinematic();
            _startGameInputAction.Disable();
        }

        private IEnumerator BlackScreenOff()
        {
            yield return null;
            Tooltip.gameObject.SetActive(false);
            var color = new Color(0f, 0f, 0f, 1f);
            while (color.a > 0f)
            {
                color.a -= Time.deltaTime * 0.1f;
                _blackScreen.color = color;
                yield return null;
            }
            Destroy(_blackScreen.gameObject);
        }

        //Открытие окна для пропуска синематика
        private void OnSkipCinematic() => _skipPanel.SetActive(true);

        //Подтверждение пропуска ролика
        public void OnSkipCinematic_EditorEvent()
        {
            _skipPanel.SetActive(false);
            StopAllCoroutines();
            Destroy(_blackScreen.gameObject);
            CrushMoment_EditorEvent();
            CinematicEnd_EditorEvent();
        }

        //Отмена пропуска ролика
        public void OnHideSkipPanel_EditorEvent() => _skipPanel.SetActive(false);

        /// <summary>
        /// Момент перезарядки игрока
        /// </summary>
        private void ReloadMoment_EditorEvent()
        {
            FindObjectOfType<PlayerControllerComponent>().ReloadAnimation();
        }

        /// <summary>
        /// Запуск крушения корабля
        /// </summary>
        private void CrushMoment_EditorEvent()
        {
            _ship.CrashStart();
        }

        /// <summary>
        /// Окончание синематика
        /// </summary>
        private void CinematicEnd_EditorEvent()
        {
            Destroy(_ship.GetComponent<Animator>());
            _ship.transform.position = new Vector3(90.89997f, 20.90005f, 336.0001f);
            _ship.transform.eulerAngles = new Vector3(48.226f, 120.359f, -37.78f);
            _gameManager.StartGame();
            Tooltip.Up();
            Destroy(gameObject);
        }
    }
}
