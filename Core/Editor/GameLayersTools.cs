/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
    public static class GameLayersTools
    {
        // ====================================================================
        // Statistics
        // ====================================================================

        /// <summary>
        ///     Quantity objects on all layers 
        /// </summary>
        /// <returns></returns>
        public static int[] CountObjectsInAllLayers()
        {
            var counts = new int[32];
            var root = Resources.FindObjectsOfTypeAll<GameObject>();
            CountObjectsInAllLayers(root, counts);
            return counts;
        }

        /// <summary>
        ///     Quantity objects on all layers, but start this given roots
        /// </summary>
        /// <param name="root"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        private static int CountObjectsInAllLayers(GameObject[] root, int[] counts)
        {
            var count = 0;
            foreach (var t in root)
                if (t.hideFlags == HideFlags.None)
                    counts[t.layer]++;
            return count;
        }

        // ====================================================================
        // Selecting
        // ====================================================================

        private static void SelectObjectsInLayer(int layerIndex)
        {
            var root = Resources.FindObjectsOfTypeAll<GameObject>();
            SelectObjectsInLayer(root, layerIndex);
        }

        private static void SelectObjectsInLayer(GameObject[] root, int layerIndex)
        {
            var selected = new List<GameObject>();
            foreach (var t in root)
                if (t.layer == layerIndex && t.hideFlags == HideFlags.None)
                    selected.Add(t);
            Selection.objects = selected.ToArray();
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