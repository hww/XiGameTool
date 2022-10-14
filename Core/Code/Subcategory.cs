/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>Settings for single art type.</summary>
    public class Subcategory 
    {
        private string _name;

        private string _description;
        private Texture _icon;
        private Category _category;
        private string _visiblePreferenceName;
        private bool _isVisible;
        private int _quantity;

        ///--------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        ///
        /// <param name="name">       The name.</param>
        /// <param name="category">   The category.</param>
        /// <param name="icon">       The icon.</param>
        /// <param name="description">The description.</param>
        ///--------------------------------------------------------------------

        public Subcategory(string name, Category category, Texture icon, string description)
        {
            _name = name;
            _category = category;
            _icon = icon;
            _description = description;
            _isVisible = true;
            OnUpdateName();
        }

        /// <summary>Updates the user interface for the name action.</summary>
        private void OnUpdateName()
        {
            var pName = $"Subcategory{CategoryName}_{_name}_isVisible";
            if (pName != _visiblePreferenceName)
            {
                EditorPrefs.DeleteKey(_visiblePreferenceName);
                _visiblePreferenceName = pName;
                SavePreferences(); 
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets the icon.</summary>
        ///
        /// <value>The icon.</value>
        ///--------------------------------------------------------------------

        public Texture Icon => _icon = _icon != null ? _icon : Texture2D.redTexture;

        ///--------------------------------------------------------------------
        /// <summary>Gets the description.</summary>
        ///
        /// <value>The description.</value>
        ///--------------------------------------------------------------------

        public string Description => _description;

        ///--------------------------------------------------------------------
        /// <summary>Gets the category the game belongs to.</summary>
        ///
        /// <value>The game category.</value>
        ///--------------------------------------------------------------------

        public Category GameCategory => _category;

        ///--------------------------------------------------------------------
        /// <summary>To control the visible status.</summary>
        ///
        /// <value>True if this object is visible, false if not.</value>
        ///--------------------------------------------------------------------

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

        ///--------------------------------------------------------------------
        /// <summary>Gets the name of the category.</summary>
        ///
        /// <value>The name of the category.</value>
        ///--------------------------------------------------------------------

        public string CategoryName => _category.Name; 

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the name.</summary>
        ///
        /// <value>The name.</value>
        ///--------------------------------------------------------------------

        public string Name
        {
            get { return _name; }
            set { _name = value; OnUpdateName(); }
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets the name of the full.</summary>
        ///
        /// <value>The name of the full.</value>
        ///--------------------------------------------------------------------

        public string FullName => $"{CategoryName}/{_name}";

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the quantity.</summary>
        ///
        /// <value>The quantity.</value>
        ///--------------------------------------------------------------------

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        } 

        /// <summary>Loads the preferences.</summary>
        private void LoadPreferences()
        {
#if UNITY_EDITOR
            _isVisible = EditorPrefs.GetBool(_visiblePreferenceName, true);
#endif
        }

        /// <summary>Saves the preferences.</summary>
        private void SavePreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool(_visiblePreferenceName, _isVisible);
#endif
        }

        ///--------------------------------------------------------------------
        /// <summary>Called when the script is loaded or a value is changed in the inspector (Called in
        /// the editor only)</summary>
        ///--------------------------------------------------------------------

        private void OnValidate()
        {
            OnUpdateName();
        }

    }
}
