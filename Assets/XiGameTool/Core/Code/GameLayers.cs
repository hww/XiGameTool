/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    public static class GameLayers
    {
        public static readonly GameLayer[] Layers = new GameLayer[32];

        static GameLayers()
        {
            // -- initialize all layers --
            var layersValues = Enum.GetValues(typeof(EGameLayer));
            foreach (var layer in layersValues)
                Layers[(int) layer] = new GameLayer((int) layer, ((EGameLayer) layer).ToString(), Color.white);
        }

        /// <summary>
        ///     Get art layer by tag
        /// </summary>
        /// <param name="gameLayer"></param>
        /// <returns></returns>
        public static GameLayer GetLayer(EGameLayer gameLayer)
        {
            return Layers[(int) gameLayer];
        }

        /// <summary>
        ///     Get art layer by integer index (same at is in Unity)
        /// </summary>
        /// <param name="gameLayer"></param>
        /// <returns></returns>
        public static GameLayer GetLayer(int gameLayer)
        {
            return Layers[gameLayer];
        }

        public static Color GetLineColor(int gameLayer)
        {
            return Layers[gameLayer].Color;
        }

        public static Color GetLineColor(EGameLayer gameLayer)
        {
            return Layers[(int)gameLayer].Color;
        }

        public static void SetLineColor(int gameLayer, Color color)
        {
            Layers[gameLayer].Color = color;
        }

        public static void SetLineColor(EGameLayer gameLayer, Color color)
        {
            Layers[(int)gameLayer].Color = color;
        }
    }
}