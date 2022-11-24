using System;
using System.Collections;
using UnityEngine;

using static Intensive.Managers.UnitManager;

using Random = UnityEngine.Random;

namespace Intensive
{
    public class SpawnComponent : MonoBehaviour, IWorldObject, IActivity
    {
        [Tooltip("Идентификатор спауна"), SerializeField]
        private string _questID;
        [Tooltip("Тип создаваемого шаблона"), SerializeField]
        private PrefabType _spawnGameObjectType;
        [Tooltip("Задержка перед спамом"), SerializeField]
        private float _delay;
        [Tooltip("Количество создаваемых существ"), SerializeField]
        private int _spawnCount;
        [Tooltip("Время между спаунами"), SerializeField, Range(1f, 100f)]
        private float _spawnCooldown;
        [Tooltip("Разброс времени спауна"), SerializeField, Range(1f, 30f)]
        private float _spawnInterval;
        [Tooltip("Область спауна"), SerializeField, Range(5f, 25f), Space]
        private float _sphereSpawn = 5f;

		/// <summary>
		/// Событие создания нового противника
		/// </summary>
		public event SpawnGameObjectEventHandler OnSpawnEnemyEvent;
        /// <summary>
        /// Событие диактивации спаунера
        /// </summary>
        public event EventHandler OnDiactivatedEvent;


        public string QuestID => _questID;


        /// <summary>
        /// Выключение спаунера
        /// </summary>
        public void Diactivate()
        {
            StopCoroutine(Spawn());
            Destroy(gameObject);
            OnDiactivatedEvent?.Invoke(this, null);
        }

        /// <summary>
        /// Включение спаунеров
        /// </summary>
        public void Activate()
		{
            StartCoroutine(Spawn());
		}

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(_delay);

            while (_spawnCount > 0)
            {
                var time = Random.Range(-_spawnInterval, _spawnInterval) + _spawnCooldown;
                time = Mathf.Clamp(time, 1f, 100f);
                yield return new WaitForSeconds(time);
                _spawnCount--;

                var position = transform.position;
                position.x += Random.Range(-_sphereSpawn, _sphereSpawn);
                position.z += Random.Range(-_sphereSpawn, _sphereSpawn);
                OnSpawnEnemyEvent?.Invoke(position, new Quaternion(), _spawnGameObjectType, this);
			}

            Diactivate();
		}

		private void OnDrawGizmos()
		{
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, _sphereSpawn);
		}
	}
}
