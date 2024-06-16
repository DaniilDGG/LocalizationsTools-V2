//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using Core.Scripts.Localizations;
using UnityEngine.UIElements;

namespace LocalizationsTools_V2.Editor
{
    public class LocalizationWindow : EditorCustomWindow<LocalizationWindow>
    {
        #region Fields

        public event Action<string, LanguageData[]> OnSaveLocalization;

        private VisualElement _visualElementCode;
        private VisualElement _visualElementLocalizations;

        private TextField _codeField;
        private List<TextField> _localizationFields = new();

        private string _code;

        #endregion
        
        private void CreateGUI()
        {
            Root = new VisualElement();

            var label = new Label("Localization");
            
            rootVisualElement.Add(label);
            rootVisualElement.Add(Root);

            _visualElementCode = new VisualElement();
            _visualElementLocalizations = new VisualElement();
            
            var save = new Button
            {
                text = "Save"
            };
            save.clicked += Save;
            _visualElementLocalizations.Add(save);
            
            var returnButton = new Button
            {
                text = "return"
            };
            returnButton.clicked += Return;
            _visualElementLocalizations.Add(returnButton);

            var continueButton = new Button
            {
                text = "continue"
            };
            continueButton.clicked += Continue;
            _visualElementCode.Add(continueButton);

            _codeField = CreateTextInput("localizationCode", "code: " ,  _visualElementCode);

            for (var index = 0; index < LocalizationController.Languages.Length; index++)
            {
                _localizationFields.Add(CreateTextInput("", LocalizationController.Languages[index].LanguageCode, _visualElementLocalizations, true));
            }
            
            Root.Add(_visualElementCode);
        }

        private void Save()
        {
            var languages = new LanguageData[_localizationFields.Count];

            for (var index = 0; index < languages.Length; index++)
            {
                languages[index] = new LanguageData(LocalizationController.GetLanguageByCode(_localizationFields[index].label), _localizationFields[index].text);
            }

            OnSaveLocalization?.Invoke(_code, languages);
        }

        private void Return()
        {
            _code = "";
            _codeField.value = _code;
            
            Root.Add(_visualElementCode);
            Root.Remove(_visualElementLocalizations);
        }
        
        private void Continue()
        {
            _code = _codeField.value;
            OpenCodeWindow();
        }

        private void OpenCodeWindow()
        {
            Root.Remove(_visualElementCode);
            Root.Add(_visualElementLocalizations);

            var localizationData = LocalizationController.GetLocalization(_code);

            if (localizationData == null)
            {
                for (var index = 0; index < _localizationFields.Count; index++)
                {
                    _localizationFields[index].value = "empty";
                }

                return;
            }
            
            for (var index = 0; index < localizationData.Data.Count; index++)
            {
                _localizationFields[index].value = localizationData.Data[index].Localization;
                _localizationFields[index].label = localizationData.Data[index].Language.LanguageCode;
            }
        }

        public void OpenCodeWindow(string code)
        {
            _code = code;
            OpenCodeWindow();
        }
    }
}
