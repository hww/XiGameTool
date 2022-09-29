/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    public static class ArtLayers
    {
        public static readonly ArtLayer[] Layers = new ArtLayer[32];

        static ArtLayers()
        {
            // -- initialize all layers --
            var layersValues = Enum.GetValues(typeof(GameLayer));
            foreach (var layer in layersValues)
                Layers[(int) layer] = new ArtLayer((int) layer, ((GameLayer) layer).ToString(), Color.white);
        }

        /// <summary>
        ///     Get art layer by tag
        /// </summary>
        /// <param name="gameLayer"></param>
        /// <returns></returns>
        public static ArtLayer GetLayer(GameLayer gameLayer)
        {
            return Layers[(int) gameLayer];
        }

        /// <summary>
        ///     Get art layer by integer index (same at is in Unity)
        /// </summary>
        /// <param name="gameLayer"></param>
        /// <returns></returns>
        public static ArtLayer GetLayer(int gameLayer)
        {
            return Layers[gameLayer];
        }

        public static Color GetLineColor(int gameLayer)
        {
            return Layers[gameLayer].Color;
        }

        public static Color GetLineColor(GameLayer gameLayer)
        {
            return Layers[(int)gameLayer].Color;
        }

        public static void SetLineColor(int gameLayer, Color color)
        {
            Layers[gameLayer].Color = color;
        }

        public static void SetLineColor(GameLayer gameLayer, Color color)
        {
            Layers[(int)gameLayer].Color = color;
        }
    }
}