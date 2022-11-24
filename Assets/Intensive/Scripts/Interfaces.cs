using System;

using UnityEngine;

namespace Intensive
{
    public abstract class Unit : MonoBehaviour
    {
        /// <summary>
        /// Текущее количество здоровья юнита
        /// </summary>
        public int Health { get; set; }
        /// <summary>
        /// Скорость перемещения юнита
        /// </summary>
        public abstract float MoveSpeed { get; protected set; }
        /// <summary>
        /// Количество наносимого урона юнитом
        /// </summary>
        public abstract ushort Damage { get; protected set; }
    }

    public interface IWorldObject
    {
        string QuestID { get; }
    }

    public interface IActivity
    {
        /// <summary>
        /// Событие диактивации спаунера
        /// </summary>
        event EventHandler OnDiactivatedEvent;

        /// <summary>
        /// Выключение спаунера
        /// </summary>
        void Diactivate();

        /// <summary>
        /// Включение спаунеров
        /// </summary>
        void Activate();
    }

    public interface IPair { }
}