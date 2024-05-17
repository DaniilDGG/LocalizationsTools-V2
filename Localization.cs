//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Localizations
{
    [Serializable]
    public class Localization
    {
        #region Fields

        [SerializeField] private List<LocalizationData> _localizations = new();
        [SerializeField] private Language[] _languages = {new("en", "english")};

        #region Propeties

        public LocalizationData[] Localizations => _localizations.ToArray();
        public Language[] Languages => _languages;

        #endregion

        #endregion

        /// <summary>
        /// Set localization changes and save.
        /// </summary>
        /// <param name="localizationCode">Localization code, is a unique localization identifier.</param>
        /// <param name="languageData">Data for localization.</param>
        public void SetLocalization(string localizationCode, List<LanguageData> languageData)
        {
            var index = _localizations.FindIndex(data => data.LocalizationCode == localizationCode);

            if (index == -1)
            {
                _localizations.Add(new LocalizationData(localizationCode, languageData));
                return;
            }

            _localizations[index] = new LocalizationData(localizationCode, languageData);
        }
        
        /// <summary>
        /// Set localizations changes and save.
        /// </summary>
        /// <param name="localizationDates">New localizations.</param>
        public void SetLocalization(List<LocalizationData> localizationDates)
        {
            _localizations = localizationDates;
            
            FixLocalizations();
        }

        /// <summary>
        /// Set language changes and save.
        /// </summary>
        /// <param name="languages">New languages.</param>
        public void SetLocalization(Language[] languages)
        {
            if (languages.Length == 0)
            {
                Debug.LogError("Languages == 0! Logic aborted...");
                return;
            }
            
            _languages = languages;
            
            FixLocalizations();
        }
        
        private void FixLocalizations()
        {
            for (var index = 0; index < _localizations.Count; index++)
            {
                _localizations[index].SetData(FixLocalization(_localizations[index]));
            }
        }

        private List<LanguageData> FixLocalization(LocalizationData localizationData)
        {
            var languageDates = new List<LanguageData>(localizationData.Data);
            var languages = new List<Language>(_languages);

            for (var index = languageDates.Count - 1; index >= 0; index--)
            {
                if (languages.Find(language => language.LanguageCode == languageDates[index].Language.LanguageCode) == null)
                {
                    languageDates.RemoveAt(index);
                }
            }

            for (int index = 0; index < languages.Count; index++)
            {
                if (languageDates.FindIndex(language => language.Language.LanguageCode == languages[index].LanguageCode) == -1)
                {
                    languageDates.Add(new LanguageData(languages[index], "empty"));
                }
            }

            return languageDates;
        }
    }
}
