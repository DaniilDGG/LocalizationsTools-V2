//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using Core.Scripts.Localizations.Unity.Base;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LocalizationsTools_V2.Unity.Text
{
    [RequireComponent(typeof(LocalizationInfo))]
    public sealed class TypingTMPTextLocalization : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text _tmp;
        
        [Space(30f)]
        [SerializeField, Range(0.01f, 1)] private float _charTypingTime = 0.16f;
        
        private LocalizationInfo _info;
        
        private string _typingText;
        private bool _requiredFinish;

        #endregion

        #region MonoBehavior

        private void OnValidate()
        {
            if (!_tmp) _tmp = GetComponent<TMP_Text>();
        }

        private void Awake()
        {
            _info = GetComponent<LocalizationInfo>();
            
            _info.OnSwitchLanguage += Typing;
            Typing(_info.GetLocalization());
        }

        #endregion

        private async void Typing(string text)
        {
            _typingText = text;
            _requiredFinish = false;

            if (string.IsNullOrEmpty(text)) return;

            var symbolsCount = 0;
            var typingText = "";
            
            while (typingText.Length < text.Length && text == _typingText && !_requiredFinish)
            {
                typingText = text[..symbolsCount];
                _tmp.text = typingText;

                await UniTask.Delay(TimeSpan.FromSeconds(_charTypingTime));

                symbolsCount++;
            }

            if (_typingText == text) _tmp.text = text;

            _requiredFinish = false;
        }

        public void StopTyping() => _requiredFinish = true;
    }
}
