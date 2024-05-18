//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using System.Linq;
using Core.Scripts.Localizations.Config;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts.Localizations.Editor
{
    public static class LocalizationEditor
    {
        #region Fields

        private static LocalizationProfile _localizationProfile;

        #region Propeties

        public static LocalizationProfile LocalizationProfile => _localizationProfile;

        #endregion
        
        #endregion
        
        [MenuItem("Localization/Language settings")]
        private static void LanguagesSetting()
        {
            Init();
            LanguagesWindow.ShowWindow().OnSaveLanguages += _localizationProfile.SetLanguages;
        }

        [MenuItem("Localization/Localization settings")]
        private static void HandleLocalizationSetting() => LocalizationSetting();
        [MenuItem("Localization/Words Count")]
        private static void GetWordsCount()
        {
            Init();

            var stats = LocalizationController.Languages.Select(t => new StatsInLanguage() { Language = t }).ToList();

            foreach (var data in _localizationProfile.LocalizationDates)
            {
                foreach (var languageData in data.Data)
                {
                    var stat = stats.Find(language => language.Language.LanguageCode == languageData.Language);

                    stat.Chars += languageData.Localization.Length;
                    stat.Words += languageData.Localization.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
                }
            }

            foreach (var stat in stats)
            {
                Debug.Log($"{(string)stat.Language}, chars: {stat.Chars}, words: {stat.Words}");
            }
        }

        private static LocalizationWindow LocalizationSetting()
        {
            Init();
            var localizationWindow = LocalizationWindow.ShowWindow();
            localizationWindow.OnSaveLocalization += _localizationProfile.SetLocalization;
            return localizationWindow;
        }

        public static void OpenLocalizationSetting(string code)
        {
            var localizationWindow = LocalizationSetting();
            localizationWindow.OpenCodeWindow(code);
        }

        public static void Init()
        {
            const string assetPath = "Assets/Resources/LocalizationProfile.asset";

            if (!_localizationProfile)
            {
                _localizationProfile = LocalizationController.GetProfile();
                
                if (!_localizationProfile)
                {
                    _localizationProfile = ScriptableObject.CreateInstance<LocalizationProfile>();
                    
                    var folder = System.IO.Path.GetDirectoryName(assetPath);
                
                    if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);

                    AssetDatabase.CreateAsset(_localizationProfile, assetPath);
                    AssetDatabase.SaveAssets();

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = _localizationProfile;
                }
            }

            _localizationProfile.InitLocalizationSystem();
        }

        private class StatsInLanguage
        {
            public int Chars;
            public int Words;
            public LanguageShort Language;
        }
    }
}
