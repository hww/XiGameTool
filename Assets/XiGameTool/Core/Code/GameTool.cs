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
                    return;
                }
            }

            var sets = AssetDatabase.FindAssets($"t: {nameof(GameToolSettings)}").SingleOrDefault();
            if (sets == default)
                throw new System.Exception($"There must be a single {nameof(Settings)}");
            _settings = AssetDatabase.LoadAssetAtPath<GameToolSettings>(AssetDatabase.GUIDToAssetPath(sets));
            _displayUnised = true;
            LoadPreferences();
        }

        #endregion


        /** Show optional lines */

        private static bool _displayUnised;

        public static bool DisplayUnused
        {
            get
            {
                return _displayUnised;
            }
            set
            {
#if UNITY_EDITOR
                if (_displayUnised != value)
                {
                    SavePreferences();
                    _displayUnised = value;
                }

#else
#endif
            }
        }

        private static void SavePreferences()
        {
            EditorPrefs.SetBool("GameToolDisplayUnused", _displayUnised);
        }

        private static void LoadPreferences()
        {
            _displayUnised = EditorPrefs.GetBool("GameToolDisplayUnused", true);
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