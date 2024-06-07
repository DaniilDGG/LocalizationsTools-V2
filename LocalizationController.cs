//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using Core.Scripts.Localizations.Config;
using UnityEditor;
using UnityEngine;

namespace Core.Scripts.Localizations
{
    [Serializable]
    public static class LocalizationController
    {
        #region Fields

        private static Language[] _languages = {new("en", "english")};
        private static LocalizationData[] _localizations;

        private static Language _currentLanguage;
        
        #region Propeties

        public static Language[] Languages => _languages;

        #endregion

        public static event Action<Language> OnLanguageSwitch;
        
        #endregion
        
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void EditorOnLoad() => RuntimeOnLoad();
#endif
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeOnLoad()
        {
            var profile = GetProfile();

            if (!profile)
            {
                Debug.LogError("Profile is null!");
                return;
            }
            
            profile.InitLocalizationSystem();
        }
        
        internal static void InitLocalization(Language[] languages, LocalizationData[] localizations)
        {
            _languages = languages;
            _localizations = localizations;

            if (_languages.Length == 0)
            {
                Debug.LogError("languages == 0! Logic aborted...");
                return;
            }
            
            SwitchLanguage(0);
        }

        /// <summary>
        /// Obtain localization when a localization code is available.
        /// </summary>
        /// <param name="localizationCode">localization code, is a unique localization identifier.</param>
        /// <returns>data for localization.</returns>
        public static LocalizationData GetLocalization(string localizationCode)
        {
            for (var index = 0; index < _localizations.Length; index++)
            {
                if (_localizations[index].LocalizationCode == localizationCode)
                {
                    return _localizations[index];
                }
            }

            return null;
        }
        
        /// <summary>
        /// Obtain localization when a localization code is available.
        /// </summary>
        /// <param name="localizationCode">localization code, is a unique localization identifier.</param>
        /// <returns>data for localization.</returns>
        public static LanguageData GetCurrentLocalization(string localizationCode)
        {
            var localization = GetLocalization(localizationCode);

            return localization == null ? new LanguageData() : localization.Data.Find(data => GetCurrentLanguage().LanguageCode == data.Language);
        }

        public static void SwitchLanguage(int index)
        {
            _currentLanguage = _languages[index];
            OnLanguageSwitch?.Invoke(_currentLanguage);
        }

        public static Language GetCurrentLanguage() => _currentLanguage;

        public static int GetCurrentLanguageIndex()
        {
            for (var index = 0; index < _languages.Length; index++)
            {
                if (_languages[index] == _currentLanguage) return index;
            }
            
            return -1;
        }
        public static Language GetLanguageByCode(string code)
        {
            for (var index = 0; index < _languages.Length; index++)
            {
                if (_languages[index].LanguageCode == code)
                {
                    return _languages[index];
                }
            }

            return null;
        }

        public static LocalizationProfile GetProfile()
        {
            var profiles = Resources.LoadAll<LocalizationProfile>("");
                
            return profiles.Length > 0 ? profiles[0] : null;
        }
    }
}
