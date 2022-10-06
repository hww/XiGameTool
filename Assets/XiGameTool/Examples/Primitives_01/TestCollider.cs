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

        // Update is called once per frame
        void OnDrawGizmos()
        {
            if (SelectionSet.IsVisible && Subcategory.IsVisible)
            {
                Gizmos.color = GameTool.Layers.GetColor(gameObject.layer);
                boxCollider = (boxCollider == null) ? GetComponent<BoxCollider>() : boxCollider;
                Gizmos.DrawWireCube(transform.position, boxCollider.size);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
                Gizmos.DrawIcon(transform.position, "../XiGameTool/Gizmos/cat_traversal");
            }
        }
    }
}
