using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewAudioConfiguration", menuName = "Configurations/AudioConfiguration", order = 1)]
    public class AudioConfiguration : PairConfiguration<AudioPair>
    {
        /// <summary>
        /// Возвращает аудиоданные по ключу
        /// </summary>
        /// <param name="type">Ключ</param>
        /// <returns>Аудио клип</returns>
        public AudioClip GetAudio(AudioType type)
        {
            return (AudioClip)GetValue(type);
        }

        /// <summary>
        /// Преобразует массив структур в словарь
        /// </summary>
        /// <returns>Результирующий словарь</returns>
        public Dictionary<AudioType, AudioClip> GetDictionary()
        {
            var dictinary = new Dictionary<AudioType, AudioClip>();
            foreach (AudioPair pair in _data) dictinary.Add(pair.Key, pair.Value);
            return dictinary;
        }
    }


#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AudioPair))]
    public class AudioPropertyDrawer : BasePairPropertyDrawer { }

#endif

}