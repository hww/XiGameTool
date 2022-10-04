/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using System;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core
{
    public static class ArtCategories
    {
        /// <summary>
        ///     Specialized for camera management
        /// </summary>
        public static ArtCategory Camera;

        /// <summary>
        ///     Specialized for particles and effects management
        /// </summary>
        public static ArtCategory Particles;

        /// <summary>
        ///     Specialized for sounds and listeners management
        /// </summary>
        public static ArtCategory Sounds;

        /// <summary>
        ///     Not fit to any group
        /// </summary>
        public static ArtCategory Globals;

        /// <summary>
        ///     Specialized for GUI, Rendering, Lighting, and PostFX
        /// </summary>
        public static ArtCategory Rendering;

        /// <summary>
        ///     Specialized for game play behaviour
        /// </summary>
        public static ArtCategory Gameplay;

        private static readonly ArtCategory[] Groups = new ArtCategory[System.Enum.GetValues(typeof(EArtCategory)).Length];
        private static bool _isInitialized;
        private static bool _showOptional;

        /// <summary>
        ///     Static constructor
        /// </summary>
        static ArtCategories()
        {
            Initialize();
        }

        /// <summary>
        ///     Initialize all groups
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            Globals = CreateGroup(EArtCategory.Globals);
            Gameplay = CreateGroup(EArtCategory.GamePlay);
            Camera = CreateGroup(EArtCategory.Camera);
            Sounds = CreateGroup(EArtCategory.Sounds);
            Rendering = CreateGroup(EArtCategory.Rendering);
            Particles = CreateGroup(EArtCategory.Particles);
        }

        private static ArtCategory CreateGroup(EArtCategory groupTag)
        {
            var group = new ArtCategory(groupTag);
            Groups[(int) group.ArtGroupTag] = group;
            return group;
        }

        /// <summary>
        /// Get art group
        /// </summary>
        /// <param name="artGroupTag"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ArtCategory GetGroup(EArtCategory artGroupTag)
        {
            switch (artGroupTag)
            {
                case EArtCategory.Globals: return Globals;
                case EArtCategory.Camera: return Camera;
                case EArtCategory.Sounds: return Sounds;
                case EArtCategory.Rendering: return Rendering;
                case EArtCategory.GamePlay: return Gameplay;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static bool ShowOptional
        {
            get
            {
#if UNITY_EDITOR
                return EditorPrefs.GetBool("showOptionalCategories");
#else
                return false;
#endif
            }
            set
            {
#if UNITY_EDITOR
                _showOptional = value;
                EditorPrefs.SetBool("showOptionalCategories", _showOptional);
#else
#endif
            }
        }

        public static bool GetVisible(EArtCategory artGrp)
        {
            return Groups[(int)artGrp].IsVisible;
        }
        public static bool GetVisible(EArtCategory artGrp, EArtType artCat)
        {
            return Groups[(int)artGrp].GetCategory(artCat).IsVisible;
        }
    }
}