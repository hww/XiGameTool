/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XiGameTool.Core 
{
    /// <summary>
    ///     Settings for single category
    /// </summary>
    public class ArtType
    {
        const float DefaultFillColorAlpha = 0.1f;

        public readonly EArtType Category;
        public readonly bool IsOptional;

        private readonly string _visiblePreferenceName;
        private bool _isVisible;

        /// <summary>
        ///     Construct new category
        /// </summary>
        /// <param name="groupTag">Parent group</param>
        /// <param name="categoryTag">Category tag</param>
        /// <param name="optional"></param>
        public ArtType(EArtCategory groupTag, EArtType categoryTag, bool optional)
        {
            IsOptional = optional;
            Category = categoryTag;
            var artGroupName = groupTag.ToString();
            var categoryName = categoryTag.ToString();
            _visiblePreferenceName = $"CategoriesWindowVisible{artGroupName}{categoryName}";
            _isVisible = GetVisibleInternal(true);
        }

        /// <summary>
        ///     Is this category visible? Will set to true or false the categories belong
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                SetVisibleInternal(value);
            }
        }

        private bool GetVisibleInternal(bool defaultValue)
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool(_visiblePreferenceName, defaultValue);
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
    }
}
