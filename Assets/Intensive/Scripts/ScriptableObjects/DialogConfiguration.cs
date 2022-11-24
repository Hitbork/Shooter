using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Intensive.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewDialogConfiguration", menuName = "Configurations/DialogConfiguration", order = 1)]
    public class DialogConfiguration : ScriptableObject
    {
        [Tooltip("Данные диалогов"), SerializeField]
        private DialogData[] Data;

        public DialogData[] GetData => Data;
    }
}
