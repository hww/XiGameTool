/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;

namespace XiGameTool.Core
{
    public static partial class GameTool
    {
        public static class Layers
        {

            private static string[] s_LayerNames;
            private static GameLayer[] s_AllLayers;
            public static string[] GetLayerNames()
            {

                if (s_LayerNames == null)
                {
#if UNITY_EDITOR
                    s_LayerNames = Enumerable.Range(0, 32).Select(index => LayerMask.LayerToName(index)).ToArray();
#else
                    s_LayerNames = new string[32];
#endif
                    OnSetLayerNames();
                }
                return s_LayerNames;
            }
            public static void SetLayerNames(string[] names)
            {
                UnityEngine.Debug.Assert(names.Length == 32);
                s_LayerNames = names;
                OnSetLayerNames();
                for (var i = 0; i < names.Length; i++)
                    s_AllLayers[i] = new GameLayer(i, names[i], Color.white);
            }

            private static void OnSetLayerNames()
            {
                // Make a name for not named
                for (var i = 0; i < s_LayerNames.Length; i++)
                {
                    if (string.IsNullOrEmpty(s_LayerNames[i]))
                        s_LayerNames[i] = $"Layer {i}";
                }
            }
     

            public static GameLayer[] AllLayers
            {
                get
                {
                    if (s_AllLayers == null)
                    {
                        s_AllLayers = new GameLayer[32];
                        var names = GetLayerNames();
                        for (var i = 0; i < 32; i++)
                        {
                            s_AllLayers[i] = new GameLayer(i, names[i], Color.white);
                        }
                    }
                    return s_AllLayers;
                }
            }
            // <summary>
            //     Get art layer by tag
            // </summary>
            // <param name="layerNum"></param>
            // <returns></returns>
            public static GameLayer Find(string name)
            {
                var names = GetLayerNames();
                for (var i = 0; i < names.Length; i++)
                {
                    if (names[i] == name)
                        return s_AllLayers[i];
                }
                Debug.LogError($"Layer {name} was not found");
                return null;
            }

            // <summary>
            //     Get art layer by tag
            // </summary>
            // <param name="layerNum"></param>
            // <returns></returns>
            public static GameLayer Find(int id)
            {
                return AllLayers[id];
            }

            /// <summary>
            ///     Get art layer by integer index (same at is in Unity)
            /// </summary>
            /// <param name="layerNum"></param>
            /// <returns></returns>
            public static GameLayer GetLayer(int layerNum)
            {
                return AllLayers[layerNum];
            }

            public static Color GetColor(int layerNum)
            {
                return AllLayers[layerNum].Color;
            }

            public static void SetColor(int layerNum, Color color)
            {
                AllLayers[layerNum].Color = color;
            }
        }
    }
}