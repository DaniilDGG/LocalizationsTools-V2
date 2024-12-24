//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using System;
using Core.Scripts.Localizations.Unity.Base;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LocalizationsTools_V2.Unity.Text
{
    [RequireComponent(typeof(LocalizationInfo))]
    public sealed class TypingTMPTextLocalization : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text _tmp;
        
        [Space(30f)]
        [SerializeField, Range(MinimumCharTypingTime, MaximumCharTypingTime)] private float _charTypingTime = 0.16f;
        
        private LocalizationInfo _info;
        
        private string _typingText;
        private string _currentTypingText;
        private bool _requiredFinish;

        #region Propeties

        public bool IsTyping => _typingText != _tmp.text;

        #endregion

        #region Constants

        public const float MinimumCharTypingTime = 0.01f;
        public const float MaximumCharTypingTime = 0.01f;

        #endregion

        #region Events

        public UnityAction OnStartTyping;
        public UnityAction OnEndTyping;

        #endregion

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

        #region Public Members

        public void SetCharTypingTime(float value, bool isRedraw = false)
        {
            value = value switch
            {
                > MaximumCharTypingTime => MaximumCharTypingTime,
                < MinimumCharTypingTime => MinimumCharTypingTime,
                _ => value
            };

            _charTypingTime = value;

            if (!isRedraw) return;

            if (IsTyping) _currentTypingText = "";
            else Typing(_typingText);
        }
        
        #endregion
        
        private async void Typing(string text)
        {
            _typingText = text;
            _requiredFinish = false;

            if (string.IsNullOrEmpty(text)) return;

            var symbolsCount = 0;
            _currentTypingText = "";
            
            OnStartTyping?.Invoke();
            
            while (_currentTypingText.Length < text.Length && text == _typingText && !_requiredFinish)
            {
                _currentTypingText = text[..symbolsCount];
                _tmp.text = _currentTypingText;

                await UniTask.Delay(TimeSpan.FromSeconds(_charTypingTime));

                symbolsCount++;
            }

            if (_typingText != text) return;
            
            _tmp.text = text;
            _requiredFinish = false;
            
            OnEndTyping?.Invoke();
        }

        public void StopTyping() => _requiredFinish = true;
    }
}
