/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>
    ///     Representation for single layer
    /// </summary>
    public class ArtSet
    {
        const float DefaultColorA = 0.1f;

        public ArtSetTag Tag;
        public string Name;
        
        private readonly string _colorPreferenceNameR;
        private readonly string _colorPreferenceNameG;
        private readonly string _colorPreferenceNameB;
        private readonly string _visiblePreferenceName;
        private bool _isVisible;

        /// <summary>
        ///     Construct new layer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="defaultColor"></param>
        public ArtSet(ArtSetTag tag, string name, Color defaultColor)
        {
            Name = name;
            Tag = tag;
            _colorPreferenceNameR = "SetsWindowColorR_" + name;
            _colorPreferenceNameG = "SetsWindowColorG_" + name;
            _colorPreferenceNameB = "SetsWindowColorB_" + name;
            _visiblePreferenceName = "SetsWindowColorVIsible_" + name;
            _color = GetColorInternal(defaultColor);
            _fillColor = _color;
            _fillColor.a = DefaultColorA;
            _isVisible = GetVisibleInternal(true);
        }


        /// <summary>
        ///     Is this layer visible
        /// </summary>
        public bool IsVisible
        {
#if UNITY_EDITOR
            get
            {
                return _isVisible;
            }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    SetVisibleInternal(value);
                    SceneView.RepaintAll();
                }
            }
#else
            get;
            set;
#endif
        }

        private Color _color;

        
        /// <summary>
        ///     Get color of this layer
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value) 
                    SetColorInternal(value);
                _color = value;
                _fillColor = value;
                _fillColor.a = DefaultColorA;
            }
        }

        private Color _fillColor;
        
        /// <summary>
        ///     Get fill color of this layer. Usually same as color but more transparent
        /// </summary>
        public Color FillColor
        {
            get => _fillColor;
            set => _fillColor = value;
        }

        private void SetColorInternal(Color value)
        {
#if UNITY_EDITOR
            EditorPrefs.SetFloat(_colorPreferenceNameR, value.r);
            EditorPrefs.SetFloat(_colorPreferenceNameG, value.g);
            EditorPrefs.SetFloat(_colorPreferenceNameB, value.b);
#endif
        }

        private Color GetColorInternal(Color defaultValue)
        {
#if UNITY_EDITOR
            var r = EditorPrefs.GetFloat(_colorPreferenceNameR, defaultValue.r);
            var g = EditorPrefs.GetFloat(_colorPreferenceNameG, defaultValue.g);
            var b = EditorPrefs.GetFloat(_colorPreferenceNameB, defaultValue.b);
            return new Color(r, g, b);
#else
			return defaultValue;
#endif
        }

        private void SetVisibleInternal(bool value)
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool(_visiblePreferenceName, value);
#endif
        }

        private bool GetVisibleInternal(bool defaultValue)
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool(_visiblePreferenceName, defaultValue);
#else
			return defaultValue;
#endif
        }
    }
}