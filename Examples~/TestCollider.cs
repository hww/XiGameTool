using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    [RequireComponent(typeof(BoxCollider))]
    public class TestCollider : ArtPrimitive
    {
        BoxCollider boxCollider;

        // Update is called once per frame
        void OnDrawGizmos()
        {
            if (ArtSets.GetVisible(ArtSet) && ArtCategories.GetVisible(ArtGroup, ArtCategory))
            {
                Gizmos.color = GameLayers.GetLineColor(gameObject.layer);
                boxCollider = (boxCollider == null) ? GetComponent<BoxCollider>() : boxCollider;
                Gizmos.DrawWireCube(transform.position, boxCollider.size);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
                Gizmos.DrawIcon(transform.position, "../XiGameTool/Gizmos/cat_traversal");
            }
        }
    }
}
