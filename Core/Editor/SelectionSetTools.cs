/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
    /// <summary>A selection set tools.</summary>
    public static class SelectionSetTools
    {
        ///--------------------------------------------------------------------
        /// <summary>Quantity objects on all layers.</summary>
        ///
        /// ### <returns>.</returns>
        ///--------------------------------------------------------------------

        public static void CountInAllSets()
        {
            foreach (var t in GameTool.SelectionSets)
                t.Quantity = 0;

            var root = Resources.FindObjectsOfTypeAll<GamePrimitive>();
            foreach (var t in root)
                if (t.hideFlags == HideFlags.None)
                    t.SelectionSet.Quantity++;
        }

        ///--------------------------------------------------------------------
        /// <summary>Select objects in set.</summary>
        ///
        /// <param name="artSet">Set the art belongs to.</param>
        ///--------------------------------------------------------------------

        private static void SelectObjectsInSet(SelectionSet artSet)
        {
            var root = Resources.FindObjectsOfTypeAll<GamePrimitive>();
            SelectObjectsInSet(root, artSet);
        }

        ///--------------------------------------------------------------------
        /// <summary>Select objects in set.</summary>
        ///
        /// <param name="root">  The root.</param>
        /// <param name="artSet">Set the art belongs to.</param>
        ///--------------------------------------------------------------------

        private static void SelectObjectsInSet(GamePrimitive[] root, SelectionSet artSet)
        {
            var selected = new List<GameObject>();
            foreach (var t in root)
                if (t.SelectionSet == artSet && t.hideFlags == HideFlags.None)
                    selected.Add(t.gameObject);
            Selection.objects = selected.ToArray();
        }

        ///--------------------------------------------------------------------
        /// <summary>Assign art set.</summary>
        ///
        /// <param name="artSet">Set the art belongs to.</param>
        ///--------------------------------------------------------------------

        public static void AssignArtSet(SelectionSet artSet)
        {
            foreach (var o in Selection.objects)
            {
                if (o is GameObject)
                {
                    var oo = (o as GameObject).GetComponents<GamePrimitive>();
                    foreach (var ooo in oo)
                        ooo.SelectionSet = artSet;
                }
            }
        }

    }
}