using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    [RequireComponent(typeof(BoxCollider))]
    public class TestCollider : GamePrimitive
    {
        BoxCollider boxCollider;
        BoxCollider BoxCollider => boxCollider ??= GetComponent<BoxCollider>();

        void OnDrawGizmos()
        {
            if (SelectionSet.IsVisible && Subcategory.IsVisible)
            {
                Gizmos.color = GameTool.Layers.GetColor(gameObject.layer);
                Gizmos.DrawWireCube(transform.position, BoxCollider.size);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
                Gizmos.DrawIcon(transform.position, "../XiGameTool/Gizmos/cat_traversal");
            }
        }
    }
}
