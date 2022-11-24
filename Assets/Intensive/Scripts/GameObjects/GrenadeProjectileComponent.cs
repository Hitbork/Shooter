using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intensive.GameObjects
{
    public class GrenadeProjectileComponent : BaseProjectileComponent
    {
		[Tooltip("Коллайдер снаряда") , SerializeField]
		private CapsuleCollider _projectileCollider;	

		[Tooltip("Коллайдер взрыва"), SerializeField]
		private SphereCollider _explosionCollider;


		protected override void OnTriggerEnter(Collider other)
		{
			if (_projectileCollider.enabled)
			{
				_projectileCollider.enabled = false;
				_explosionCollider.enabled = true;
				Debug.Log("Projectile");
			}
			else
			{
				base.OnTriggerEnter(other);
				Debug.Log("Explosion");
			}
		}
	}
}