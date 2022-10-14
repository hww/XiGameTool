using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    [RequireComponent(typeof(BoxCollider))]
    public class TestCollider : GamePrimitive
    {
        /// <summary>The box collider.</summary>
        public BoxCollider boxCollider;

        ///--------------------------------------------------------------------
        /// <summary>Called when the script is loaded or a value is changed in
        /// the inspector (Called in the editor only)</summary>
        ///--------------------------------------------------------------------

        private void OnValidate()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        /// <summary>Draws gizmos that are also pickable and always drawn.</summary>
        void OnDrawGizmos()
        {
            if (SelectionSet.IsVisible && Subcategory.IsVisible)
            {
                Gizmos.color = GameTool.Layers.GetColor(gameObject.layer);
                Gizmos.DrawWireCube(transform.position, boxCollider.size);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
                Gizmos.DrawIcon(transform.position, "../XiGameTool/Gizmos/cat_traversal");
            }
        }
    }
}
