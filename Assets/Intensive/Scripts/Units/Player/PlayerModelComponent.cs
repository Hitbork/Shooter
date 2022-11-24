using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intensive.Units.Player
{
    //Hello, world
    [RequireComponent(typeof(PlayerControllerComponent))]
    public class PlayerModelComponent : MonoBehaviour
    {
        public bool showSettings;
        public byte currentModelIndex;

        [Tooltip("Текущая используемая модель")]
        [SerializeField]
        private GameObject _currentModel;

        [Tooltip("Модели персонажа")]
        [SerializeField]
        private GameObject[] _models;

		private void Start()
		{
            CheckAndUpdateReferences();
        }

		public void UpdateReferences()
        {
            DestroyImmediate(_currentModel);

            _currentModel = Instantiate(_models[currentModelIndex], transform);
            _currentModel.transform.localPosition = Vector3.zero;
            _currentModel.transform.localEulerAngles = Vector3.zero;
            _currentModel.transform.gameObject.name = "Model";
        }

        private void CheckAndUpdateReferences()
        {
            var controller = GetComponent<PlayerControllerComponent>();
            //controller.SetReferences(_currentModel.GetComponent<Animator>());
        }
    }
}