using Intensive.Managers;
using Intensive.Units.Player;

using System;

using UnityEngine;

using Zenject;

namespace Intensive.GameObjects
{
    [RequireComponent(typeof(SphereCollider))]
    public class TriggerComponent : MonoBehaviour, IWorldObject, IActivity
    {
        private Collider _playerCollider;
        private SphereCollider _selfCollider;

        [Tooltip("Активен-ли триггер"), SerializeField]
        private bool _isActive;
        [Tooltip("Идентификатор спауна"), SerializeField]
        private string _questID;
        [Tooltip("Радиус активации триггера"), SerializeField]
        protected float _radius = 10f;
        //[Tooltip("Автоматически-ли срабатывает взаимодействие с триггером"), SerializeField]
        //protected bool _isAutoTrigger = false;
        [Tooltip("Связанные спаунеры"), SerializeField]
        private SpawnComponent[] _spawners;

        /// <summary>
        /// Событие диактивации
        /// </summary>
        public event EventHandler OnDiactivatedEvent;
        /// <summary>
        /// Событие взаимодействия объекта с триггером
        /// </summary>
        public event EventHandler<string> OnTriggerEnterEvent;



		public string QuestID => _questID;

        /// <summary>
        /// Выключение
        /// </summary>
        public void Diactivate()
        {
            Destroy(gameObject);
            OnDiactivatedEvent?.Invoke(this, null);
        }

        /// <summary>
        /// Включение
        /// </summary>
        public void Activate()
        {
            _isActive = true;
        }

        //В редакторе отрисовывается область срабатывания триггера
        private void OnDrawGizmos()
        {
            //Установка цвета в зависимости от режима срабатывания триггера
            Gizmos.color = _spawners.Length == 0 ? new Color(1f, .95f, 0f, .25f) : new Color(0.7f, 0f, .29f, .25f);
            //Отрисовка сферы области срабатывания триггера
            Gizmos.DrawSphere(transform.position, _radius);
        }

        [Inject]
        private void Construct(PlayerControllerComponent player)
        {
            _playerCollider = player.GetComponentInChildren<Collider>();
            _selfCollider.radius = _radius;
            _selfCollider.isTrigger = true;
        }

		private void OnTriggerEnter(Collider other)
        {
            if (!_isActive || !other.Equals(_playerCollider)) return;

            //Триггеры спаунеров не оповещают о срабатывании
            if (_spawners.Length == 0) OnTriggerEnterEvent?.Invoke(this, QuestID);
            else foreach (var spawner in _spawners) spawner.Activate();

            Diactivate();
        }

		private void OnValidate()
		{
            _selfCollider = GetComponent<SphereCollider>();
        }
	}
}
