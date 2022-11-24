using Intensive.Units.Player;
using System;
using System.Collections;
using UnityEngine;

using Zenject;

namespace Intensive
{
    public class ModTriggerComponent : MonoBehaviour
    {
        public int code;
        [Tooltip("Радиус активации триггера"), SerializeField]
        protected float _radius = 10f;
        public GameObject _child;

        private Collider _playerCollider;
        private SphereCollider _selfCollider;


        /// <summary>
        /// Событие взаимодействия объекта с триггером
        /// </summary>
        public event EventHandler<string> OnTriggerEnterEvent;

        [Inject]
        private void Construct(PlayerControllerComponent player)
        {
            _playerCollider = player.GetComponentInChildren<Collider>();
            _selfCollider.radius = _radius;
            _selfCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.Equals(_playerCollider)) return;

            Destroy(_selfCollider);
            OnTriggerEnterEvent?.Invoke(this, code.ToString());
        }

        private void OnValidate()
        {
            _selfCollider = GetComponent<SphereCollider>();
        }

        public void Met()
        {
            switch(code)
            {
                case 1:
                    transform.localPosition = new Vector3(2.24f, 1.48f, 2.31f);
                    transform.localEulerAngles = new Vector3(-58.5f, 445.65f, -29.7f);
                    break;
                case 2:
                    break;
                case 3:
                    StartCoroutine(Cor());
                    break;
			}
		}

        private IEnumerator Cor()
        {
            while (true)
            {
                for (int i = 0; i < 6; i++)
                {
                    _child.SetActive(true);
                    yield return new WaitForSeconds(0.1f);
                }
                _child.SetActive(true);
                yield return new WaitForSeconds(1f);
                _child.SetActive(false);
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
