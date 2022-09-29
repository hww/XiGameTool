/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using System;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core
{
    public static class ArtGroups
    {
        /// <summary>
        ///     Specialized for camera management
        /// </summary>
        public static ArtGroup Camera;

        /// <summary>
        ///     Specialized for particles and effects management
        /// </summary>
        public static ArtGroup Particles;

        /// <summary>
        ///     Specialized for sounds and listeners management
        /// </summary>
        public static ArtGroup Sounds;

        /// <summary>
        ///     Not fit to any group
        /// </summary>
        public static ArtGroup Globals;

        /// <summary>
        ///     Specialized for GUI, Rendering, Lighting, and PostFX
        /// </summary>
        public static ArtGroup Rendering;

        /// <summary>
        ///     Specialized for game play behaviour
        /// </summary>
        public static ArtGroup Gameplay;

        private static readonly ArtGroup[] Groups = new ArtGroup[System.Enum.GetValues(typeof(ArtGroupTag)).Length];
        private static bool _isInitialized;
        private static bool _showOptional;

        /// <summary>
        ///     Static constructor
        /// </summary>
        static ArtGroups()
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

            Globals = CreateGroup(ArtGroupTag.Globals);
            Gameplay = CreateGroup(ArtGroupTag.GamePlay);
            Camera = CreateGroup(ArtGroupTag.Camera);
            Sounds = CreateGroup(ArtGroupTag.Sounds);
            Rendering = CreateGroup(ArtGroupTag.Rendering);
            Particles = CreateGroup(ArtGroupTag.Particles);
        }

        private static ArtGroup CreateGroup(ArtGroupTag groupTag)
        {
            var group = new ArtGroup(groupTag);
            Groups[(int) group.ArtGroupTag] = group;
            return group;
        }

        /// <summary>
        /// Get art group
        /// </summary>
        /// <param name="artGroupTag"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ArtGroup GetGroup(ArtGroupTag artGroupTag)
        {
            switch (artGroupTag)
            {
                case ArtGroupTag.Globals: return Globals;
                case ArtGroupTag.Camera: return Camera;
                case ArtGroupTag.Sounds: return Sounds;
                case ArtGroupTag.Rendering: return Rendering;
                case ArtGroupTag.GamePlay: return Gameplay;
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

        public static bool GetVisible(ArtGroupTag artGrp)
        {
            return Groups[(int)artGrp].IsVisible;
        }
        public static bool GetVisible(ArtGroupTag artGrp, ArtCategoryTag artCat)
        {
            return Groups[(int)artGrp].GetCategory(artCat).IsVisible;
        }
    }
}