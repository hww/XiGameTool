/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
    public static class SetsTools
    {
        // ====================================================================
        // Statistics
        // ====================================================================

        /// <summary>
        ///     Count objects on all layers 
        /// </summary>
        /// <returns></returns>
        public static int[] CountInAllSets()
        {
            var counts = new int[32];
            var root = Resources.FindObjectsOfTypeAll<ArtPrimitive>();
            CountInAllSets(root, counts);
            return counts;
        }

        /// <summary>
        ///     Count objects on all layers, but start this given roots
        /// </summary>
        /// <param name="root"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        private static int CountInAllSets(ArtPrimitive[] root, int[] counts)
        {
            var count = 0;
            foreach (var t in root)
                if (t.hideFlags == HideFlags.None)
                    counts[(int)t.ArtSetTag]++;
            return count;
        }

        // ====================================================================
        // Selecting
        // ====================================================================

        private static void SelectObjectsInSet(ArtSetTag artSet)
        {
            var root = Resources.FindObjectsOfTypeAll<ArtPrimitive>();
            SelectObjectsInSet(root, artSet);
        }

        private static void SelectObjectsInSet(ArtPrimitive[] root, ArtSetTag artSet)
        {
            var selected = new List<GameObject>();
            foreach (var t in root)
                if (t.ArtSetTag == artSet && t.hideFlags == HideFlags.None)
                    selected.Add(t.gameObject);
            Selection.objects = selected.ToArray();
        }

        public static void AssignArtSet(ArtSetTag artSet)
        {
            foreach (var o in Selection.objects)
            {
                if (o is GameObject)
                {
                    var oo = (o as GameObject).GetComponents<ArtPrimitive>();
                    foreach (var ooo in oo)
                        ooo.ArtSetTag = artSet;
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