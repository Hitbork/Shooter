using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEffectConfiguration", menuName = "Configurations/EffectConfiguration", order = 1)]
    public class EffectConfiguration : PairConfiguration<EffectPair>
    {
        /// <summary>
        /// Преобразует массив структур в словарь
        /// </summary>
        /// <returns>Результирующий словарь</returns>
        public Dictionary<EffectType, ParticleSystem> GetDictionary()
        {
            var dictinary = new Dictionary<EffectType, ParticleSystem>();
            foreach (EffectPair pair in _data) dictinary.Add(pair.Key, pair.Value);
            return dictinary;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EffectPair))]
    public class EffectPropertyDrawer : BasePairPropertyDrawer { }

#endif
}
