using Intensive.Units.Player;
using Intensive.ScriptableObjects;
using UnityEngine;
using Zenject;
using AudioConfiguration = Intensive.ScriptableObjects.AudioConfiguration;
using System.Collections.Generic;
using Intensive.Units;
using Intensive.GameObjects;

namespace Intensive.Managers
{
    [RequireComponent(typeof(UnitManager), typeof(AudioManager), typeof(EffectManager))]
    public class GameManager : MonoInstaller
    {
        private UnitManager _unitManager;
        private AudioManager _audioManager;
        private EffectManager _effectManager;
        private QuestManager _questManager;
        private InterfaceManager _interfaceManager;
        private PlayerControllerComponent _player;


        [Tooltip("Аудио данным игры"), SerializeField]
        private AudioConfiguration _audioData;
        [Tooltip("Шаблоны объектов игры"), SerializeField]
        private PrefabConfiguration _prefabData;
        [Tooltip("Шаблоны эффектов"), SerializeField]
        private EffectConfiguration _effectData;
        [Tooltip("Параметры игрока"), SerializeField]
        private PlayerConfiguration _playerData;
        [Tooltip("Параметры оружия"), SerializeField]
        private WeaponConfiguration _weaponData;
        [Tooltip("Квесты"), SerializeField]
        private QuestConfiguration _questData;
        [Tooltip("Контексты диалогов"), SerializeField]
        private DialogConfiguration _contextData;

        public override void InstallBindings()
        {
            //Рассылка конфигурации игры
            Container.BindInstance(_audioData.GetDictionary()).WithId("Configuration").AsSingle();
            Container.BindInstance(_prefabData.GetDictionary()).WithId("Configuration").AsSingle();
            Container.BindInstance(_effectData.GetDictionary()).WithId("Configuration").AsSingle();
            Container.BindInstance(_contextData).WithId("Configuration").AsSingle();
            Container.BindInstance(_questData).WithId("Configuration").AsSingle();
            Container.BindInstance(_playerData).AsSingle();
            Container.BindInstance(_weaponData).AsSingle();

            Container.BindInstance(_player).AsSingle();
            Container.BindInstance(new LinkedList<EnemyData>()).AsSingle();
            Container.BindInstance(new LinkedList<SpawnComponent>()).AsSingle();
            Container.BindInstance(new LinkedList<TriggerComponent>()).AsSingle();
        }

        public void StartGame()
        {
            _audioManager.OnStart();
            _questManager.OnStart();
            _interfaceManager.OnStart();
            _player.OnStart();
        }

		private void OnValidate()
		{
            _player = FindObjectOfType<PlayerControllerComponent>();
            _unitManager = GetComponent<UnitManager>();
            _audioManager = GetComponent<AudioManager>();
            _effectManager = GetComponent<EffectManager>();
            _questManager = GetComponent<QuestManager>();
            _interfaceManager = GetComponent<InterfaceManager>();
        }

		public override void Start()
        {
            base.Start();
            _player.enabled = false;
        }
    }
}
