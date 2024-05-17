//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Core.Scripts.Localizations.Config
{
    [CreateAssetMenu(fileName = "Localization Profile", menuName = "Localization/LocalizationProfile", order = 1)]
    public class LocalizationProfile : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _localizationFile = "localization.loc";
        private Localization _localization = new();

        #region Propeties

        private string MainPath { get; } = Application.streamingAssetsPath;
        public string LocalizationFile => _localizationFile;
        public LocalizationData[] LocalizationDates => _localization.Localizations;

        #endregion

        #endregion

        /// <summary>
        /// Initialization of the localization system, is the entry point.
        /// </summary>
        public void InitLocalizationSystem()
        {
            Load();
            ReloadLocalizationController();
        }

        /// <summary>
        /// Set localization changes and save.
        /// </summary>
        /// <param name="localizationCode">localization code, is a unique localization identifier.</param>
        /// <param name="languageData">data for localization.</param>
        public void SetLocalization(string localizationCode, LanguageData[] languageData)
        {
            _localization.SetLocalization(localizationCode, new List<LanguageData>(languageData));
            HandleSet();
        }

        public void SetLocalizations(List<LocalizationData> localizationDates)
        {
            _localization.SetLocalization(localizationDates);
            HandleSet();
        }

        /// <summary>
        /// Set language changes and save.
        /// </summary>
        /// <param name="languages">New languages.</param>
        public void SetLanguages(List<Language> languages)
        {
            _localization.SetLocalization(languages.ToArray());
            HandleSet();
        }

        private void HandleSet()
        {
            Save();
            ReloadLocalizationController();
        }

        private void ReloadLocalizationController()
        {
            LocalizationController.InitLocalization(_localization.Languages, _localization.Localizations);
        }

        private void Load()
        {
            var path = Path.Combine(MainPath, _localizationFile);

            if (!File.Exists(path)) return;
            
            var json = File.ReadAllText(path);
            _localization = JsonUtility.FromJson<Localization>(json);
        }

        private void Save()
        {
            var text = JsonUtility.ToJson(_localization, true);
            var path = Path.Combine(MainPath, _localizationFile);
            
            try
            {
                using var fs = File.Create(Path.Combine(path, name));
                
                var info = new UTF8Encoding(true).GetBytes(text);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
