using System;
using Core.Scripts.Localizations.Config;
using UnityEngine;

namespace Core.Scripts.LocalizationTemplateCode
{
    public class BootLocalization : MonoBehaviour
    {
        [SerializeField] private LocalizationProfile _localizationProfile;
        
        #region MonoBehavior

        private void Awake() => _localizationProfile.InitLocalizationSystem();

        #endregion
    }
}
