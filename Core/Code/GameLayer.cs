/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>Representation for single layer.</summary>
    public class GameLayer
    {
        /// <summary>(Immutable) the default fill color a.</summary>
        const float kDefaultColorA = 0.1f;

        /// <summary>The name.</summary>
        private string _name;
        /// <summary>Zero-based index of the.</summary>
        private int _index;
        /// <summary>The layer mask.</summary>
        private int _mask;
        /// <summary>The objects quantity.</summary>
        private int _quantity;
        /// <summary>The icon.</summary>
        private Texture _icon;
        /// <summary>The color preference format.</summary>
        private string _colorPreferenceFormat;

        ///--------------------------------------------------------------------
        /// <summary>Construct new layer.</summary>
        ///
        /// <param name="index">       The layer index.</param>
        /// <param name="name">        The layer na,e.</param>
        /// <param name="defaultColor">The layer color.</param>
        /// <param name="icon">        (Optional) The layer icon.</param>
        ///--------------------------------------------------------------------

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

        ///--------------------------------------------------------------------
        /// <summary>Gets the zero-based index of this object.</summary>
        ///
        /// <value>The index.</value>
        ///--------------------------------------------------------------------

        public int Index =>_index;

        ///--------------------------------------------------------------------
        /// <summary>Gets the mask.</summary>
        ///
        /// <value>The mask.</value>
        ///--------------------------------------------------------------------

        public int Mask =>_mask;

        ///--------------------------------------------------------------------
        /// <summary>Gets the name.</summary>
        ///
        /// <value>The name.</value>
        ///--------------------------------------------------------------------

        public string Name => _name;

        ///--------------------------------------------------------------------
        /// <summary>Gets the icon.</summary>
        ///
        /// <value>The icon.</value>
        ///--------------------------------------------------------------------

        public Texture Icon => _icon != null ? _icon : Texture2D.redTexture;

        ///--------------------------------------------------------------------
        /// <summary>Is this layer locked.</summary>
        ///
        /// <value>True if this object is locked, false if not.</value>
        ///--------------------------------------------------------------------

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

        ///--------------------------------------------------------------------
        /// <summary>Is this layer visible.</summary>
        ///
        /// <value>True if this object is visible, false if not.</value>
        ///--------------------------------------------------------------------

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

        ///--------------------------------------------------------------------
        /// <summary>Get color of this layer.</summary>
        ///
        /// <value>The color.</value>
        ///--------------------------------------------------------------------

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

        ///--------------------------------------------------------------------
        /// <summary>Get fill color of this layer. Usually same as color but more transparent.</summary>
        ///
        /// <value>The color of the fill.</value>
        ///--------------------------------------------------------------------

        public Color FillColor
        {
            get => new Color(_color.r, _color.g, _color.b, kDefaultColorA);
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the quantity.</summary>
        ///
        /// <value>The quantity.</value>
        ///--------------------------------------------------------------------

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        /// <summary>Saves the preferences.</summary>
        private void SavePreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.SetFloat(string.Format(_colorPreferenceFormat, "R"), _color.r);
            EditorPrefs.SetFloat(string.Format(_colorPreferenceFormat, "G"), _color.g);
            EditorPrefs.SetFloat(string.Format(_colorPreferenceFormat, "B"), _color.b);
#endif
        }

        /// <summary>Loads the preferences.</summary>
        private void LoadPreferences()
        {
#if UNITY_EDITOR
            var r = EditorPrefs.GetFloat(string.Format(_colorPreferenceFormat, "R"), _defaultColor.r);
            var g = EditorPrefs.GetFloat(string.Format(_colorPreferenceFormat, "G"), _defaultColor.g);
            var b = EditorPrefs.GetFloat(string.Format(_colorPreferenceFormat, "B"), _defaultColor.b);
            _color = new Color(r, g, b);
#endif
        }

        /// <summary>Clears the preferences.</summary>
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