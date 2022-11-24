using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewQuestConfiguration", menuName = "Configurations/QuestConfiguration", order = 1)]
    public class QuestConfiguration : PairConfiguration<ContextPair>
    {
        /// <summary>
        /// Преобразует массив структур в словарь
        /// </summary>
        /// <returns>Результирующий словарь</returns>
        public Dictionary<string, string> GetDictionary()
        {
            var dictinary = new Dictionary<string, string>();
            foreach (ContextPair pair in _data) dictinary.Add(pair.Key, pair.Value);
            return dictinary;
        }

        public string GetValue(string key)
        {
            return (string)base.GetValue(key);
        }
    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ContextPair))]
    public class DialogPropertyDrawer : BasePairPropertyDrawer { }

#endif

}
