//Copyright 2023 Daniil Glagolev
//Licensed under the Apache License, Version 2.0

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LocalizationsTools_V2.Editor
{
    public class EditorCustomWindow<T> : EditorWindow where T : EditorCustomWindow<T>
    {
        #region Fields

        private static T _editorCustomWindow;

        protected VisualElement Root;

        #endregion
        
        /// <summary>
        /// Show custom editor window.
        /// </summary>
        public static T ShowWindow()
        {
            if (_editorCustomWindow)
            {
                return _editorCustomWindow;
            }
            
            _editorCustomWindow = GetWindow<T>();
            _editorCustomWindow.titleContent = new GUIContent("Editor Custom Window");

            return _editorCustomWindow;
        }

        protected TextField CreateTextInput(string text, string label) => CreateTextInput(text, label, Root);
        
        protected TextField CreateTextInput(string text, string label, VisualElement visualElement, bool multiline = false)
        {
            var textElement = new TextField(label, int.MaxValue, false, false, ' ')
            {
                value = text,
                multiline = multiline,
            };

            if (multiline)
            {
                var scrollView = new ScrollView()
                {
                    style =
                    {
                        flexGrow = 1, 
                        height = 200 
                    }
                };

                scrollView.Add(textElement);
                visualElement.Add(scrollView);
            }
            else
            {
                visualElement.Add(textElement);
            }

            return textElement;
        }
    }
}