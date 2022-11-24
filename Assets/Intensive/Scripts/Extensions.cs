using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Данный файл расширяет интерфейс Unity и не участвует в самой игре
/// </summary>
namespace Intensive
{
    [CreateAssetMenu(fileName = "NewAudioData", menuName = "Configurations/AudioData", order = 1)]
    public abstract class PairConfiguration<T> : ScriptableObject where T : struct, IPair
    {
        private Func<T, object> _getterKey;
        private Func<T, object> _getterValue;

        [Tooltip("Массив данных")]
        [SerializeField]
        protected T[] _data;

        public PairConfiguration()
        {
            _getterKey = (target) => typeof(T).GetField("Key").GetValue(target);//todo добавить экспресию
            _getterValue = (target) => typeof(T).GetField("Value").GetValue(target);
        }

        /// <summary>
        /// Возвращает аудиоданные по ключу
        /// </summary>
        /// <param name="type">Ключ</param>
        /// <returns>Аудио клип</returns>
        protected object GetValue(object type)
        {
            foreach(var el in _data)
                if (_getterKey.Invoke(el).Equals(type)) return _getterValue.Invoke(el);

            throw new ApplicationException("В конфигурации " + GetType() + " отсутствует запрашиваемые данные. Ключ: " + type);
        }
    }


#if UNITY_EDITOR
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        protected string[] _properties;

        private const float space = 5;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var firstLineRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            DrawMainProperties(firstLineRect, property);

            EditorGUI.indentLevel = indent;
        }

        private void DrawMainProperties(Rect rect, SerializedProperty property)
        {
            rect.width = (rect.width - 2 * space) / _properties.Length;
            foreach (var prop in _properties)
            {
                DrawProperty(rect, property.FindPropertyRelative(prop));
                rect.x += rect.width + space;
            }
        }

        private void DrawProperty(Rect rect, SerializedProperty property)
        {
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        }
    }

    public abstract class BasePairPropertyDrawer : BasePropertyDrawer
    {
        public BasePairPropertyDrawer()
        {
            _properties = new string[] { "Key", "Value" };
        }
    }

    public class ReadOnlyAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}