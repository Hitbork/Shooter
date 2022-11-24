using TMPro;

using UnityEngine;


namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewWeaponConfiguration", menuName = "Configurations/WeaponConfiguration", order = 1)]
    public class WeaponConfiguration : ScriptableObject
    {
        [Tooltip("Данные оружия")]
        [SerializeField]
        private WeaponData[] _data;



        public WeaponData[] GetWeaponData => _data;
    }
}