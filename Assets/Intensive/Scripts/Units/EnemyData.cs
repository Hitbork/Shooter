using Intensive.GameObjects;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intensive.Units
{
    [RequireComponent(typeof(Collider))]
    public class EnemyData : Unit, IWorldObject
    {
        private Collider _collider;
        private Animator _animator;
        private EnemyState _state;

        [Tooltip("Очки здоровья")]
        [SerializeField]
        private int _health;

        [Tooltip("Наносимый урон")]
        [SerializeField]
        private ushort _damage;

        [Tooltip("Скорость передвижения")]
        [SerializeField]
        private float _moveSpeed;

        [Tooltip("Время между атаками")]
        [SerializeField]
        private float _attackInterval;

        /// <summary>
        /// Время между атаками
        /// </summary>
        public float AttackCooldown { get; set; }
        /// <summary>
        /// Скорость перемещения юнита
        /// </summary>
		public override float MoveSpeed { get => _moveSpeed; protected set => _moveSpeed = value; }
        /// <summary>
        /// Количество наносимого урона юнитом
        /// </summary>
        public override ushort Damage { get => _damage; protected set => _damage = value; }
        /// <summary>
        /// Идентификатор привязки  к квесту
        /// </summary>
        public string QuestID { get; set; }


        /// <summary>
        /// Состояние бота-противника
        /// </summary>
        public EnemyState State
        {
            get => _state;
            set
            {
                switch (value)
                {
                    case EnemyState.Idle:
                        if (_animator.GetBool("Run"))
                        {
                            _animator.SetBool("Run", false);
                        }
                        break;
                    case EnemyState.Run:
                        if (!_animator.GetBool("Run"))
                        {
                            _animator.SetBool("Run", true);
                        }
                        break;
                    case EnemyState.Attack:
                        if (AttackCooldown <= 0f)
                        {
                            _animator.SetBool("Run", false);
                            _animator.SetTrigger("Attack");
                        }
                        break;
                    case EnemyState.Die:
                        _animator.SetBool("Die", true);
                        break;
                    default:
                        _animator.ResetTrigger("Attack");
                        _animator.SetBool("Run", false);
                        throw new ApplicationException("Enemy animator broke!");
                }
            }
        }

		//Инициализация внутренних данных
		private void Start()
		{
            Health = _health;
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<Collider>();
        }

        /// <summary>
        /// Сбрасывает атаку в перезарядку
        /// </summary>
        public void ResetAttackCooldown()
        {
            AttackCooldown = _attackInterval;
        }

        /// <summary>
        /// Обработка смерти персонажа
        /// </summary>
        public IEnumerator OnDie()
        {
            _collider.enabled = false;

            yield return new WaitForSeconds(5f);
            var timer = 12f;

            while(timer > 0f)
            {
                transform.position -= transform.up * 0.2f * Time.deltaTime;
                yield return null;
                timer -= Time.deltaTime;
			}

            Destroy(gameObject);
		}
	}
}