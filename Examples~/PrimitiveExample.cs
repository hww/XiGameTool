using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    public class PrimitiveExample : ArtPrimitive
    {

        void OnDrawGizmos()
        {
            if (ArtSets.GetVisible(ArtSet) && ArtGroups.GetVisible(ArtGroup, ArtCategory))
            {
                Gizmos.color = ArtSets.GetLineColor(ArtSet);
                Gizmos.DrawWireSphere(transform.position, 1f);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
            }        
        }
    }
}
