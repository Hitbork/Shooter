using Intensive.Units.Player;
using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;

namespace Intensive.Editor
{
    [CustomEditor(typeof(PlayerModelComponent))]
    public class PlayerEditor : UnityEditor.Editor
    {
        private PlayerModelComponent _target;


        private void OnEnable()
        {
            _target = (PlayerModelComponent)target;
        }

        public override void OnInspectorGUI()
        {
            if(_target.showSettings && !Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            //Режим настройки
            if (GUILayout.Button("Show settings", GUILayout.ExpandWidth(true), GUILayout.Height(20), GUILayout.MinWidth(100), GUILayout.MaxWidth(200)))
            {
                _target.showSettings = !_target.showSettings;
            }

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Low", GUILayout.ExpandWidth(true), GUILayout.Height(20), GUILayout.MinWidth(100), GUILayout.MaxWidth(200)))
            {
                if (_target.currentModelIndex == 0) return;
                _target.currentModelIndex = 0;
                _target.UpdateReferences();
            }
            if (GUILayout.Button("Medium", GUILayout.ExpandWidth(true), GUILayout.Height(20), GUILayout.MinWidth(100), GUILayout.MaxWidth(200)))
            {
                if (_target.currentModelIndex == 1) return;
                _target.currentModelIndex = 1;
                _target.UpdateReferences();
            }
            if (GUILayout.Button("High", GUILayout.ExpandWidth(true), GUILayout.Height(20), GUILayout.MinWidth(100), GUILayout.MaxWidth(200)))
            {
                if (_target.currentModelIndex == 2) return;
                _target.currentModelIndex = 2;
                _target.UpdateReferences();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (GUI.changed) SetObjectDirty(_target.gameObject);
        }

        public static void SetObjectDirty(GameObject obj)
        {
            EditorUtility.SetDirty(obj);
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }
    }
}

