//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using Core.Scripts.Localizations;
using UnityEngine.UIElements;

namespace LocalizationsTools_V2.Editor
{
    public class LanguagesWindow : EditorCustomWindow<LanguagesWindow>
    {
        #region Fields

        public event Action<List<Language>> OnSaveLanguages;
        
        private readonly List<TextField> _codes = new();
        private readonly List<TextField> _names = new();
        
        #endregion
        
        private void CreateGUI()
        {
            Root = new VisualElement();

            var label = new Label("Current Languages:");
            
            rootVisualElement.Add(label);
            rootVisualElement.Add(Root);

            for (var index = 0; index < LocalizationController.Languages.Length; index++)
            {
                _codes.Add(CreateTextInput(LocalizationController.Languages[index].LanguageCode, "language code: "));
                _names.Add(CreateTextInput(LocalizationController.Languages[index].LanguageName, "language name: "));
            }
            
            var buttonAdd = new Button
            {
                text = "Add"
            };
            buttonAdd.clicked += Add;
            rootVisualElement.Add(buttonAdd);
            
            var buttonRemove = new Button
            {
                text = "Remove"
            };
            buttonRemove.clicked += Remove;
            rootVisualElement.Add(buttonRemove);
            
            var button = new Button
            {
                text = "Save"
            };
            button.clicked += Save;
            rootVisualElement.Add(button);
        }

        private void Remove()
        {
            var index = _codes.Count - 1;
            
            Root.Remove(_codes[index]);
            Root.Remove(_names[index]);
            
            _codes.RemoveAt(index);
            _names.RemoveAt(index);
        }
        
        private void Add()
        {
            _codes.Add(CreateTextInput("DefaultCode", "language code: "));
            _names.Add(CreateTextInput("DefaultName", "language name: "));
        }
        
        private void Save()
        {
            var languages = new List<Language>();

            for (var index = 0; index < _codes.Count; index++)
            {
                languages.Add(new Language(_codes[index].text, _names[index].text));
            }
            
            OnSaveLanguages?.Invoke(languages);
        }
    }
}
