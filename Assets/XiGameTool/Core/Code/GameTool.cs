/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core
{
    public static partial class GameTool
    {
        /** The GameToolSettings */

        #region Load Settings

        private static GameToolSettings _settings;
        public static GameToolSettings Settings => _settings;

        /// <summary>
        ///     Static constructor
        /// </summary>
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

        // Read the asset at the path if it is exists or return null
        // TODO Probably could be done by single Unity method (no time to find)
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


        /** Show optional lines */

        private static bool _displayUnused;

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

        private static void SavePreferences()
        {
            EditorPrefs.SetBool("GameToolDisplayUnused", _displayUnused);
        }

        private static void LoadPreferences()
        {
            _displayUnused = EditorPrefs.GetBool("GameToolDisplayUnused", true);
        }


        public static Category FindCategory(string name)
        {
            return _settings.FindCategory(name);
        }

        public static Subcategory FindSubcategory(string name)
        {
            return _settings.FindSubcategory(name);
        }

        public static SelectionSet FindSelectionSet(string name)
        {
            return _settings.FindSelectionSet(name);
        }

        private static readonly List<Category> categories;

        public static IReadOnlyList<Category> Categories => _settings.Categories;
        public static IReadOnlyList<Subcategory> Subcategories => _settings.Subcategories;
        public static IReadOnlyList<SelectionSet> SelectionSets => _settings.SelectionSets;


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
        private static void ClearCounters()
        {
            _settings.ClearCounters();
        }
    }
}