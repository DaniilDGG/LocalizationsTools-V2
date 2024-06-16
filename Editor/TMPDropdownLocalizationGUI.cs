using Core.Scripts.Localizations;
using UnityEditor;
using UnityEngine;

namespace LocalizationsTools_V2.Editor
{
    [CustomPropertyDrawer(typeof(LocalizationString))]
    public class TMPDropdownLocalizationGUI : PropertyDrawer
    {
        private const float ButtonWidth = 120f; 
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var valueProperty = property.FindPropertyRelative("_value");
            if (valueProperty == null)
            {
                EditorGUI.EndProperty();
                return;
            }

            var textFieldRect = new Rect(position.x, position.y, position.width - ButtonWidth - 10, position.height);
            var buttonRect = new Rect(position.x + position.width - ButtonWidth, position.y, ButtonWidth, position.height);

            valueProperty.stringValue = EditorGUI.TextField(textFieldRect, valueProperty.stringValue);

            if (GUI.Button(buttonRect, "Open Localization")) LocalizationEditor.OpenLocalizationSetting(valueProperty.stringValue);

            EditorGUI.EndProperty();
        }
    }
}