/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    public static class ArtSets
    {
        public static readonly ArtSet[] Sets = new ArtSet[32];

        static ArtSets()
        {
            // -- initialize all layers --
            var setsValues = Enum.GetValues(typeof(ArtSetTag));
            foreach (var setTag in setsValues)
                Sets[(int) setTag] = new ArtSet((ArtSetTag)setTag, ((ArtSetTag) setTag).ToString(), Color.white);
        }

        /// <summary>
        ///     Get art setTag by tag
        /// </summary>
        /// <param name="artSet"></param>
        /// <returns></returns>
        public static ArtSet GetArtSet(ArtSetTag artSet)
        {
            return Sets[(int) artSet];
        }

        /// <summary>
        ///     Get art setTag by integer index (same at is in Unity)
        /// </summary>
        /// <param name="gameLayer"></param>
        /// <returns></returns>
        public static ArtSet GetArtSet(int gameLayer)
        {
            return Sets[gameLayer];
        }


        /// <summary>
        ///     Get color in case if this object in given ArtSet
        /// </summary>
        /// <param name="gameObjectLayer"></param>
        /// <returns></returns>
        public static Color GetLineColor(ArtSetTag artSet)
        {
            return Sets[(int)artSet].Color;
        }


        /// <summary>
        ///     Get fill color if the object is in given ArtSet
        /// </summary>
        /// <param name="gameObjectLayer"></param>
        /// <returns></returns>
        public static Color GetFillColor(ArtSetTag artSet)
        {
            return Sets[(int)artSet].FillColor;
        }
        /// <summary>
        /// Check if set is visible
        /// </summary>
        /// <param name="artSet"></param>
        /// <returns></returns>
        public static bool GetVisible(ArtSetTag artSet)
        {
            return Sets[(int)artSet].IsVisible;
        }


        public static void SetLineColor(ArtSetTag artSet, Color color)
        {
            Sets[(int)artSet].Color = color;
        }
    }
}