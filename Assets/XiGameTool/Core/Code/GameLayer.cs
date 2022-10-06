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
        const float kDefaultColorA = 0.1f;

        private string _name;
        private int _index;
        private int _mask;
        private int _quantity;
        private Texture _icon;

        private string _colorPreferenceFormat;

        /// <summary>
        ///     Construct new layer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="defaultColor"></param>
        public GameLayer(int index, string name, Color defaultColor, Texture icon = null)
        {
            _name = name;
            _index = index;
            _mask = 1 << index;
            _defaultColor = defaultColor;
            _colorPreferenceFormat = $"GameLayer_{name}_{{0}}";
            _icon = Icon;
            LoadPreferences();
        }
        public int Index =>_index;
        public int Mask =>_mask;
        public string Name => _name;
        public Texture Icon => _icon ?? Texture2D.redTexture;

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
        private Color _defaultColor;

        /// <summary>
        ///     Get color of this layer
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    SavePreferences();
                }
            }
        }

        
        /// <summary>
        ///     Get fill color of this layer. Usually same as color but more transparent
        /// </summary>
        public Color FillColor
        {
            get => new Color(_color.r, _color.g, _color.b, kDefaultColorA);
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        private void SavePreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.SetFloat(string.Format(_colorPreferenceFormat, "R"), _color.r);
            EditorPrefs.SetFloat(string.Format(_colorPreferenceFormat, "G"), _color.g);
            EditorPrefs.SetFloat(string.Format(_colorPreferenceFormat, "B"), _color.b);
#endif
        }

        private void LoadPreferences()
        {
#if UNITY_EDITOR
            var r = EditorPrefs.GetFloat(string.Format(_colorPreferenceFormat, "R"), _defaultColor.r);
            var g = EditorPrefs.GetFloat(string.Format(_colorPreferenceFormat, "G"), _defaultColor.g);
            var b = EditorPrefs.GetFloat(string.Format(_colorPreferenceFormat, "B"), _defaultColor.b);
            _color = new Color(r, g, b);
#endif
        }

        private void ClearPreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.DeleteKey(string.Format(_colorPreferenceFormat, "R"));
            EditorPrefs.DeleteKey(string.Format(_colorPreferenceFormat, "G"));
            EditorPrefs.DeleteKey(string.Format(_colorPreferenceFormat, "B"));
#endif
        }
    }
}