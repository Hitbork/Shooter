using Intensive.Commands;

using System;
using UnityEngine;


namespace Intensive
{
    [Serializable]
    public struct PrefabPair : IPair
    {
        public PrefabType Key;
        public GameObject Value;
    }

    [Serializable]
    public struct AudioPair : IPair
    {
        public AudioType Key;
        public AudioClip Value;
    }

    [Serializable]
    public struct EffectPair : IPair
    {
        public EffectType Key;
        public ParticleSystem Value;
	}

    [Serializable]
    public struct ContextPair : IPair
    {
        public string Key;
        public string Value;
	}

    [Serializable]
    public struct QuestPair : IPair
    {
        public string Key;
        public string Value;
    }

    [Serializable]
    public struct DialogData
    {
        public string Key;
        public ContextPair[] Context; 
    }


    [Serializable]
    public struct WeaponData
    {
        [Tooltip("Название оружия")]
        public string WeaponName;
        [Tooltip("Тип снаряда")]
        public PrefabType ProjectileType;
        [Tooltip("Время между выстрелами")]
        public float CooldownFire;

        [Space, Tooltip("Параметры перегрева оружия")]
        public WeaponOverheatData WeaponOverheat;

        [Space, Tooltip("Параметры снарядов оружия")]
        public ProjectileData Projectile;
    }

    [Serializable]
    public struct ProjectileData
    {
        [Tooltip("Тип взрыва снаряда")]
        public EffectType EffectType;
        [Tooltip("Наносимый урон снарядом")]
        public ushort Damage;
        [Tooltip("Время существования снаряда")]
        public float LifeTime;
        [Tooltip("Скорость полета снаряда")]
        public float Speed;
    }

    [Serializable]
    public struct WeaponOverheatData
    {
        [Tooltip("Допустимая температура после перегрева")]
        public float MediumTemperature;

        [Tooltip("Время начала охлаждения оружия")]
        public float DelayCooling;

        [Tooltip("Время начала охлаждения оружия после перегрева")]
        public float DelayCoolingAfterOverheat;

        [Tooltip("Нагрев за выстрел. 1 равна 100% нагрева")]
        public float WarmingPerShot;

        [Tooltip("Потеря процентов в секунду")]
        public float CoolingInSec;
    }
}