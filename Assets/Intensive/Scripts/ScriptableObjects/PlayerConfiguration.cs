using UnityEngine;

namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPlayerConfiguration", menuName = "Configurations/PlayerConfiguration", order = 1)]
    public class PlayerConfiguration : ScriptableObject
    {
        [Tooltip("Здоровье игрока"), SerializeField]
        private ushort _health = 100;

        [Tooltip("Задержка начала регенерации здоровья"), SerializeField]
        private float _delayRegenerationInSec = 3f;

        [Tooltip("Количество восстанавливаемого здоровья в секунду"), SerializeField]
        private float _hitPointRegenPerSec = 0.5f;

        [Tooltip("Скорость перемещения персонажа"), SerializeField]
        private float _moveSpeed = 5f;

        [Tooltip("Сила прыжка"), SerializeField]
        private float _jumpForce = 5f;

        [Tooltip("Скорость вращения прицела"), SerializeField]
        private float _focusSpeed = 25f;



        /// <summary>
        /// Здоровье игрока
        /// </summary>
        public ushort GetHealth => _health;
        /// <summary>
        /// Задержка начала регенерации здоровья
        /// </summary>
        public float GetDelayRegenerationInSec => _delayRegenerationInSec;
        /// <summary>
        /// Количество восстанавливаемого здоровья в секунду
        /// </summary>
        public float GetHitPointRegenPerSec => _hitPointRegenPerSec;
        /// <summary>
        /// Скорость перемещения персонажа
        /// </summary>
        public float GetMoveSpeed => _moveSpeed;
        /// <summary>
        /// Сила прыжка
        /// </summary>
        public float GetJumpForce => _jumpForce;
        /// <summary>
        /// Скорость вращения прицела
        /// </summary>
        public float GetFocusSpeed => _focusSpeed;
    }
}
