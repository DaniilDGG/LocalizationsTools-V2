using Core.Scripts.Localizations.Unity.Base;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts.Localizations.Editor
{
    [CustomEditor(typeof(LocalizationInfo))]
    public class LocalizationInfoGUI : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var info = (LocalizationInfo)target;
            if (GUILayout.Button("Open Localization"))
            {
                LocalizationEditor.OpenLocalizationSetting(info.LocalizationCode);
            }
        }
    }
}
