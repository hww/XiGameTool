/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>
    ///     Settings for single art type
    /// </summary>
    public class Subcategory 
    {
        private string _name;

        private string _description;
        private Texture _icon;
        private Category _category;
        private string _visiblePreferenceName;
        private bool _isVisible;
        private int _quantity;


        public Subcategory(string name, Category category, Texture icon, string description)
        {
            _name = name;
            _category = category;
            _icon = icon;
            _description = description;
            _isVisible = true;
            OnUpdateName();
        }

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

        public Texture Icon => _icon ??= Texture2D.redTexture;
        public string Description => _description;
        public Category GameCategory => _category;
        /// <summary>
        ///     To control the visible status 
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

        public string CategoryName => _category.Name; 

        public string Name
        {
            get { return _name; }
            set { _name = value; OnUpdateName(); }
        }

        public string FullName => $"{CategoryName}/{_name}";

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        } 

        private void LoadPreferences()
        {
#if UNITY_EDITOR
            _isVisible = EditorPrefs.GetBool(_visiblePreferenceName, true);
#endif
        }

        private void SavePreferences()
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool(_visiblePreferenceName, _isVisible);
#endif
        }

        private void OnValidate()
        {
            OnUpdateName();
        }

    }
}
