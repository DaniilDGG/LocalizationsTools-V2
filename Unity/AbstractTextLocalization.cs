using Core.Scripts.Localizations.Unity.Base;
using TMPro;
using UnityEngine;

namespace Core.Scripts.Localizations.Unity
{
    public abstract class AbstractTextLocalization : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text _tmpText;
        
        private LocalizationInfo _localizationInfo;
        
        #endregion
        
        #region MonoBehavior

        private void Awake()
        {
            _localizationInfo = gameObject.GetComponent<LocalizationInfo>();
            _localizationInfo.OnSwitchLanguage += SetText;
            
            SetText(_localizationInfo.GetLocalization());
        }

        private void OnValidate()
        {
            if (_tmpText) return;
            
            _tmpText = gameObject.GetComponent<TMP_Text>();
        }

        #endregion

        private void SetText(string original)
        {
            var text = GetText(original);

            _tmpText.text = text;
        }

        protected abstract string GetText(string original);
    }
}