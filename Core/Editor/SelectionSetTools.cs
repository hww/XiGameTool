/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
    public static class SelectionSetTools
    {
        // ====================================================================
        // Statistics
        // ====================================================================

        /// <summary>
        ///     Quantity objects on all layers 
        /// </summary>
        /// <returns></returns>
        public static void CountInAllSets()
        {
            foreach (var t in GameTool.SelectionSets)
                t.Quantity = 0;

            var root = Resources.FindObjectsOfTypeAll<GamePrimitive>();
            foreach (var t in root)
                if (t.hideFlags == HideFlags.None)
                    t.SelectionSet.Quantity++;
        }

        // ====================================================================
        // Selecting
        // ====================================================================

        private static void SelectObjectsInSet(SelectionSet artSet)
        {
            var root = Resources.FindObjectsOfTypeAll<GamePrimitive>();
            SelectObjectsInSet(root, artSet);
        }

        private static void SelectObjectsInSet(GamePrimitive[] root, SelectionSet artSet)
        {
            var selected = new List<GameObject>();
            foreach (var t in root)
                if (t.SelectionSet == artSet && t.hideFlags == HideFlags.None)
                    selected.Add(t.gameObject);
            Selection.objects = selected.ToArray();
        }

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


        private static void SelectObjectsByLayerMask(int layerMask)
        {
            var root = Resources.FindObjectsOfTypeAll<GameObject>();
            SelectObjectsByLayerMask(root, layerMask);
        }

        private static void SelectObjectsByLayerMask(GameObject[] root, int layerMask)
        {
            var selected = new List<GameObject>();
            foreach (var t in root)
                if (t.hideFlags == HideFlags.None && ((1 << t.layer) & layerMask) > 0)
                    selected.Add(t);
            Selection.objects = selected.ToArray();
        }
    }
}