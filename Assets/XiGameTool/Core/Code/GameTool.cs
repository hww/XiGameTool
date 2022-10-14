/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>A game tool.</summary>
    public static partial class GameTool
    {


        #region Load Settings

        /// <summary>The GameToolSettings.</summary>
        private static GameToolSettings _settings;

        ///--------------------------------------------------------------------
        /// <summary>Gets options for controlling the operation.</summary>
        ///
        /// <value>The settings.</value>
        ///--------------------------------------------------------------------

        public static GameToolSettings Settings => _settings;

        /// <summary>Static constructor.</summary>
        static GameTool()
        {
            _settings = null;
            var setsBehaviour = GameObject.FindObjectOfType<GameToolSettingsBehaviour>();
            if (setsBehaviour != null)
            {
                if (setsBehaviour.settings == null)
                {
                    Debug.LogError($"The scene contains {nameof(GameToolSettingsBehaviour)} with undefined settings reference");
                }
                else
                {
                    _settings = setsBehaviour.settings;
                }
            }
            if (_settings == null)
                _settings = FindDefaultAssetPath(false);
            _displayUnused = true;
            LoadPreferences();
        }

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first default asset path.</summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error
        ///                                 condition occurs.</exception>
        ///
        /// <param name="packagePriority">True to package priority.</param>
        ///
        /// <returns>The found default asset path.</returns>
        ///--------------------------------------------------------------------

        public static GameToolSettings FindDefaultAssetPath(bool packagePriority)
        {
            const string defaultAssetPath1 = "Assets/XiGameTool/Resources/XiGameTool/GameToolSettings.asset";
            const string defaultAssetPath2 = "Packages/com.hww.xigametool/Resources/XiGameTool/GameToolSettings.asset";
            const string defaultAssetPath3 = "/Resources/XiGameTool/GameToolSettings.asset";
            const string defaultAssetPath4 = "GameToolSettings.asset";

            var idList = AssetDatabase.FindAssets($"t: {nameof(GameToolSettings)}");
            if (idList.Length == 0)
                throw new System.Exception($"There must be a single {nameof(Settings)}");
            var pathList = idList.Select(id => UnityEditor.AssetDatabase.GUIDToAssetPath(id)).ToList();

            if (packagePriority)
            {
                return TryReadDefaultAsset(defaultAssetPath2, pathList) ??
                    TryReadDefaultAsset(defaultAssetPath1, pathList) ??
                    TryReadDefaultAsset(defaultAssetPath3, pathList) ??
                    TryReadDefaultAsset(defaultAssetPath4, pathList);
            }
            else
            {
                return TryReadDefaultAsset(defaultAssetPath1, pathList) ??
                    TryReadDefaultAsset(defaultAssetPath2, pathList) ??
                    TryReadDefaultAsset(defaultAssetPath3, pathList) ??
                    TryReadDefaultAsset(defaultAssetPath4, pathList);
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>Read the asset at the path if it is exists or return null
        /// TODO Probably could be done by single Unity method (no time to
        /// find)</summary>
        ///
        /// <param name="loadPath">         Full pathname of the load file.</param>
        /// <param name="existinPathList">  List of existin paths.</param>
        ///
        /// <returns>The GameToolSettings.</returns>
        ///--------------------------------------------------------------------

        private static GameToolSettings TryReadDefaultAsset(string loadPath, List<string> existinPathList)
        { 
            foreach (var path in existinPathList)
            {
                if (path.Contains(loadPath))
                    return UnityEditor.AssetDatabase.LoadAssetAtPath<GameToolSettings>(path);
            }
            return null;
        }
        #endregion


        /// <summary>Show optional lines.</summary>

        private static bool _displayUnused;

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets a value indicating whether the display
        /// unused.</summary>
        ///
        /// <value>True if display unused, false if not.</value>
        ///--------------------------------------------------------------------

        public static bool DisplayUnused
        {
            get
            {
                return _displayUnused;
            }
            set
            {
#if UNITY_EDITOR
                if (_displayUnused != value)
                {
                    _displayUnused = value;
                    SavePreferences();
                }
#else
#endif
            }
        }

        /// <summary>Saves the preferences.</summary>
        private static void SavePreferences()
        {
            EditorPrefs.SetBool("GameToolDisplayUnused", _displayUnused);
        }

        /// <summary>Loads the preferences.</summary>
        private static void LoadPreferences()
        {
            _displayUnused = EditorPrefs.GetBool("GameToolDisplayUnused", true);
        }

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first category.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The found category.</returns>
        ///--------------------------------------------------------------------

        public static Category FindCategory(string name)
        {
            return _settings.FindCategory(name);
        }

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first subcategory.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The found subcategory.</returns>
        ///--------------------------------------------------------------------

        public static Subcategory FindSubcategory(string name)
        {
            return _settings.FindSubcategory(name);
        }

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first selection set.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The found selection set.</returns>
        ///--------------------------------------------------------------------

        public static SelectionSet FindSelectionSet(string name)
        {
            return _settings.FindSelectionSet(name);
        }

        /// <summary>(Immutable) the categories.</summary>
        private static readonly List<Category> categories;

        ///--------------------------------------------------------------------
        /// <summary>Gets the categories.</summary>
        ///
        /// <value>The categories.</value>
        ///--------------------------------------------------------------------

        public static IReadOnlyList<Category> Categories => _settings.Categories;

        ///--------------------------------------------------------------------
        /// <summary>Gets the subcategories.</summary>
        ///
        /// <value>The subcategories.</value>
        ///--------------------------------------------------------------------

        public static IReadOnlyList<Subcategory> Subcategories => _settings.Subcategories;

        ///--------------------------------------------------------------------
        /// <summary>Gets the sets the selection belongs to.</summary>
        ///
        /// <value>The selection sets.</value>
        ///--------------------------------------------------------------------

        public static IReadOnlyList<SelectionSet> SelectionSets => _settings.SelectionSets;


        /// <summary>Count all objects.</summary>
        public static void CountAllObjects()
        {
            ClearCounters();

            var objects = GameObject.FindObjectsOfType<GamePrimitive>();

            foreach (var obj in objects)
            {
                obj.IncrementCounters();
            }

            // now update counters of all groups
            var cats = Categories;
            foreach (var artCategory in cats)
                artCategory.UpdateCount();
        }

        /// <summary>Clears the counters.</summary>
        private static void ClearCounters()
        {
            _settings.ClearCounters();
        }
    }
}
