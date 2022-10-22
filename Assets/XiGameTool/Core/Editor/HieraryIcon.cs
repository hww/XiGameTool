using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XiGameTool.Core;

namespace XiGameTool.Core.Editor
{
    /// <summary>Adds applicable icons next to game objects in the hierarchy.</summary>
    [InitializeOnLoad]
    static class IconDisplay
    {
        /// <summary>The font.</summary>
        static Font _font;

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the font.</summary>
        ///
        /// <value>The font.</value>
        ///--------------------------------------------------------------------

        public static Font Font => _font ??= LoadFont("RobotoMonoNerd");


        /// <summary>The label style.</summary>
        static GUIStyle _labelStyle;

        ///--------------------------------------------------------------------
        /// <summary>Gets the label style.</summary>
        ///
        /// <value>The label style.</value>
        ///--------------------------------------------------------------------

        public static GUIStyle labelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle(EditorStyles.label);
                    _labelStyle.alignment = TextAnchor.MiddleRight;
                    _labelStyle.padding.right = 8;
                    _labelStyle.font = Font;
                }

                return _labelStyle;
            }
        }

        /// <summary>Static constructor.</summary>
        static IconDisplay()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HighlightItems;
        }


        ///--------------------------------------------------------------------
        /// <summary>Displays icons beside each game object in the Hierarchy
        /// panel.</summary>
        ///
        /// <param name="instanceID">   Identifier for the instance.</param>
        /// <param name="selectionRect">The selection rectangle.</param>
        ///--------------------------------------------------------------------

        static void HighlightItems(int instanceID, Rect selectionRect)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (target == null)
                return;

            var prim = target.GetComponent<GamePrimitive>();
            if (prim != null)
                GUI.Label(selectionRect, prim.HierarhyIconString, labelStyle);
        }

        ///--------------------------------------------------------------------
        /// <summary>Loads a font from the asset database.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The font.</returns>
        ///--------------------------------------------------------------------

        static Font LoadFont(string name)
        {
            var guid = AssetDatabase.FindAssets(name + " t:font").First();
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<Font>(path);
        }
    }
}
