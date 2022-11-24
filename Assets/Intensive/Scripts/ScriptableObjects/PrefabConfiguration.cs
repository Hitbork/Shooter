using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPrefabConfiguration", menuName = "Configurations/PrefabConfiguration", order = 1)]
    public class PrefabConfiguration : PairConfiguration<PrefabPair>
    {
        /// <summary>
        /// Возвращает шаблон объекта по ключу
        /// </summary>
        /// <param name="type">Ключ</param>
        /// <returns>Шаблон объекта</returns>
        public GameObject GetPrefab(PrefabType type)
        {
            return (GameObject)GetValue(type);
        }

        /// <summary>
        /// Преобразует массив структур в словарь
        /// </summary>
        /// <returns>Результирующий словарь</returns>
        public Dictionary<PrefabType, GameObject> GetDictionary()
        {
            var dictinary = new Dictionary<PrefabType, GameObject>();
            foreach (PrefabPair pair in _data) dictinary.Add(pair.Key, pair.Value);
            return dictinary;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PrefabPair))]
    public class PrefabPropertyDrawer : BasePairPropertyDrawer { }
#endif
}