//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Core.Scripts.Localizations.Editor
{
    public class XlsxImportWindow : EditorCustomWindow<XlsxImportWindow>
    {
        #region Fields

        private Toggle _replaceLocalizationInFile;
        private readonly List<Toggle> _languagesToggles = new();

        public event UnityAction<ParametersImport> OnImport; 

        #endregion

        private void Update() => LanguagesVisible();

        private void CreateGUI()
        {
            Root = new VisualElement();
            
            rootVisualElement.Add(new Label("Import XLSX parameters"));

            _replaceLocalizationInFile = new Toggle("Replace only those localizations that are in the file")
            {
                value = true
            };

            rootVisualElement.Add(_replaceLocalizationInFile);

            rootVisualElement.Add(new Label("Languages import:"));
            rootVisualElement.Add(Root);

            for (var index = 0; index < LocalizationController.Languages.Length; index++)
            {
                var toggle = new Toggle(LocalizationController.Languages[index].LanguageCode);
                
                Root.Add(toggle);
                _languagesToggles.Add(toggle);
            }
            
            var buttonImport = new Button
            {
                text = "Import"
            };
            buttonImport.clicked += Import;
            rootVisualElement.Add(buttonImport);
        }

        private void LanguagesVisible()
        {
            Root.visible = _replaceLocalizationInFile.value;
        }
        
        private void Import()
        {
            var languages = new List<string>();

            for (var index = 0; index < _languagesToggles.Count; index++)
            {
                if (!_languagesToggles[index].value) continue;
                
                languages.Add(LocalizationController.Languages[index]);
            }

            if (languages.Count == 0 && _replaceLocalizationInFile.value) return;

            var parameters = new ParametersImport()
            {
                ReplaceLocalizationInFile = _replaceLocalizationInFile.value,
                Languages = languages
            };
            
            OnImport?.Invoke(parameters);
            
            Close();
        }
    }
    
    public class ParametersImport
    {
        #region Fields

        public bool ReplaceLocalizationInFile;
        public List<string> Languages;

        #endregion
    }
}
