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
    public class GameLayer
    {
        const float DefaultColorA = 0.1f;

        public int Index;
        public int Mask;
        public string Name;

        private readonly string _colorPreferenceNameR;
        private readonly string _colorPreferenceNameG;
        private readonly string _colorPreferenceNameB;

        /// <summary>
        ///     Construct new layer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="defaultColor"></param>
        public GameLayer(int index, string name, Color defaultColor)
        {
            Name = name;
            Index = index;
            Mask = 1 << index;
            _colorPreferenceNameR = "LayersWindowColorR_" + name;
            _colorPreferenceNameG = "LayersWindowColorG_" + name;
            _colorPreferenceNameB = "LayersWindowColorB_" + name;
            _color = GetColorInternal(defaultColor);
            _fillColor = _color;
            _fillColor.a = DefaultColorA;
        }

        /// <summary>
        ///     Is this layer locked
        /// </summary>
        public bool IsLocked
        {
#if UNITY_EDITOR
            get
            {
                return (Tools.lockedLayers & Mask) > 0;
            }
            set
            {
                if (value)
                    Tools.lockedLayers |= Mask;
                else
                    Tools.lockedLayers &= ~Mask;
            }
#else
            get;
            set;
#endif
        }

        /// <summary>
        ///     Is this layer visible
        /// </summary>
        public bool IsVisible
        {
#if UNITY_EDITOR
            get
            {
                return (Tools.visibleLayers & Mask) > 0;
            }
            set
            {
                var was = Tools.visibleLayers;

                if (value)
                    Tools.visibleLayers |= Mask;
                else
                    Tools.visibleLayers &= ~Mask;
                if (was != Tools.visibleLayers)
                    SceneView.RepaintAll();
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
    }
}