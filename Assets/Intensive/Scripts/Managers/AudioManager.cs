using System.Collections;
using System.Collections.Generic;
using Intensive.Units.Player;
using UnityEngine;
using Zenject;

namespace Intensive.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [Inject]
        private PlayerControllerComponent _player;

        [Inject(Id = "Configuration")]
        private Dictionary<AudioType, AudioClip> _dictionary;

        private static AudioManager _self;

        [Tooltip("Отключение саундтрека")]
        [SerializeField]
        private bool _muteSoundtrack;

        [Tooltip("Источник фоновой музыки"), ReadOnly, SerializeField]
        private AudioSource _soundtrackAudioSource;    

        [Tooltip("Основной источник звука"), ReadOnly, SerializeField]
        private AudioSource _mainAudioSource;



        /// <summary>
        /// Одномоментное проигрывание звука
        /// </summary>
        /// <param name="type">Тип звукового ресурса</param>
        public static void PlayOneShot(AudioType type)
        {
            _self._mainAudioSource.PlayOneShot(_self._dictionary[type]);
		}

        /// <summary>
        /// Проигрывание звука в конкретной точке
        /// </summary>
        /// <param name="type">Тип звукового ресурса</param>
        /// <param name="position">Точка в пространстве для проигрывания</param>
        public static void PlayAtPoint(AudioType type, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(_self._dictionary[type], position, _self._mainAudioSource.volume);
        }

        public void OnStart()
        {
            StartCoroutine(SoundtrackStart());
        }

        public IEnumerator SoundtrackStart()
        {
            if(_muteSoundtrack) yield break;//todo написать эдитор на обычный мут

            yield return new WaitForSeconds(5f);
            var timer = 5f;
            _soundtrackAudioSource.Play();
            while (timer > 0f)
            {
                _soundtrackAudioSource.volume = Mathf.Lerp(0f, 0.25f, 1f - timer / 5f);
                timer -= Time.deltaTime;

                yield return null;
			}
		}

        void Awake()
        {
            _self = this;
        }

		private void Start()
		{
            _soundtrackAudioSource = _player.GetComponent<AudioSource>();
            _mainAudioSource = _player.GetComponentInChildren<Camera>(true).GetComponent<AudioSource>();
        }
	}
}