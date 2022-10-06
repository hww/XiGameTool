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
    public class SelectionSet
    {
        private string _name;
        private Color _defaultColor;
        private Texture _icon;


        private bool _isVisible;
        private Color _color;
        private string _description;
        private string _preferencesFormat;
        private int _quantity;

        public SelectionSet(string name, Color color, Texture icon, string descriptin)
        {
            _name = name;
            _isVisible = true;
            _defaultColor = color;
            _color = color;
            _icon = icon;
            _description = descriptin;
            _quantity = 0;
            OnChangeName();
        }

        public Texture Icon => _icon ?? Texture2D.redTexture;

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }
        public string Description => _description;

        public string Name
        {
            get { return _name; }
            set { 
                if (_name != value)
                {
                    _name = value;
                    OnChangeName();
                }
            }
        }
        private void OnChangeName()
        {
            if (_preferencesFormat != null)
                ClearPreferences();
            _preferencesFormat = $"GameTool.SelectionSet.WindowColor_{_name}_{{0}}";
            SavePreferences();
        }

        /// <summary>
        ///     Is this layer visible
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    SavePreferences();
                }
            }
        }



        
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
        public Color FillColor => new Color(_color.r, _color.g, _color.b, 0.1f);

        private void SavePreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool(string.Format(_preferencesFormat, "isVisible"), _isVisible);
            EditorPrefs.SetFloat(string.Format(_preferencesFormat, "R"), _color.r);
            EditorPrefs.SetFloat(string.Format(_preferencesFormat, "G"), _color.g);
            EditorPrefs.SetFloat(string.Format(_preferencesFormat, "B"), _color.b);
#endif
        }

        private void LoadPreferences()
        {
#if UNITY_EDITOR
            _isVisible = EditorPrefs.GetBool(string.Format(_preferencesFormat, "isVisible"), true);
            var r = EditorPrefs.GetFloat(string.Format(_preferencesFormat, "R"), _defaultColor.r);
            var g = EditorPrefs.GetFloat(string.Format(_preferencesFormat, "G"), _defaultColor.g);
            var b = EditorPrefs.GetFloat(string.Format(_preferencesFormat, "B"), _defaultColor.b);
            _color = new Color(r, g, b);
#else
#endif
        }
        private void ClearPreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.DeleteKey(string.Format(_preferencesFormat, "isVisible"));
            EditorPrefs.DeleteKey(string.Format(_preferencesFormat, "R"));
            EditorPrefs.DeleteKey(string.Format(_preferencesFormat, "G"));
            EditorPrefs.DeleteKey(string.Format(_preferencesFormat, "B"));
#else

#endif
        }
    }
}