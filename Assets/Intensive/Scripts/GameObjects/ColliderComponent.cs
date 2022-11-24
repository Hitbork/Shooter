using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intensive
{
    public class ColliderComponent : MonoBehaviour
    {
        public event EventHandler<Collision> OnCollisionEnterEvent;
		public event EventHandler<Collision> OnCollisionExitEvent;

		private void OnCollisionEnter(Collision collision)
		{
			OnCollisionEnterEvent?.Invoke(this, collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			OnCollisionExitEvent?.Invoke(this, collision);
		}
	}
}