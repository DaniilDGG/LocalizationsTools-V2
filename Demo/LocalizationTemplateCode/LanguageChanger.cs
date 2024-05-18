using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Localizations.Demo.LocalizationTemplateCode
{
    public class LanguageChanger : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Dropdown _dropdown;

        #endregion

        #region MonoBehavior

        private void Awake()
        {
            _dropdown.ClearOptions();

            List<string> options = new();

            for (var index = 0; index < LocalizationController.Languages.Length; index++)
            {
                options.Add(LocalizationController.Languages[index].LanguageName);
            }
            
            _dropdown.AddOptions(options);
            _dropdown.onValueChanged.AddListener(delegate { EditLanguage(); });
        }

        #endregion

        private void EditLanguage() => LocalizationController.SwitchLanguage(_dropdown.value);
    }
}
