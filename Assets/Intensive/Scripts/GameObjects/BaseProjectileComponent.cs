using System;
using UnityEngine;

namespace Intensive.GameObjects
{
    [RequireComponent(typeof(Collider))]
    public class BaseProjectileComponent : MonoBehaviour
    {
        /// <summary>
        /// Эффект от попадания
        /// </summary>
        public EffectType EffectType;
        /// <summary>
        /// Наносимый урон снарядом
        /// </summary>
        public ushort Damage;
        /// <summary>
        /// Время жизни снаряда
        /// </summary>
        public float LifeTime;
        /// <summary>
        /// Скорость снаряда
        /// </summary>
        public float Speed;


        public event EventHandler<Unit> OnCollisionProjectile;

        /// <summary>
        /// Установка параметров снаряда
        /// </summary>
        /// <param name="data">Данные снаряда</param>
        public void SetParams(ProjectileData data)
        {
            Damage = data.Damage; LifeTime = data.LifeTime; Speed = data.Speed; EffectType = data.EffectType;
        }

        protected virtual void OnTriggerEnter(Collider other)
		{
            var unit = other.GetComponent<Unit>();
            if (unit != null) unit.Health -= Damage;
            OnCollisionProjectile?.Invoke(this, unit);
        }
	}
}