using Intensive.ScriptableObjects;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Intensive.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [Inject(Id = "Configuration")]
        private Dictionary<EffectType, ParticleSystem> _dictionary;

        private static EffectManager _self;
        private Transform _effectsPool;


        void Awake()
        {
            _self = this;
        }

		private void Start()
		{
            //Находим игровой объект, вмещающий в себя все создаваемые эффекты
            _effectsPool = GetComponentsInChildren<Transform>(true).First(t => t.name == "EffectsPool");
        }


		/// <summary>
		/// Одномоментное проигрывание эффекта
		/// </summary>
		/// <param name="type">Тип эффекта</param>
		public static void PlayOneShot(EffectType type, Vector3 position, Quaternion rotation)
        {
            var effect = Instantiate(_self._dictionary[type], _self._effectsPool);//todo добавить пул эффектов
            effect.transform.position = position;
            effect.transform.rotation = rotation;
        }
    }
}