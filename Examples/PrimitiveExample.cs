using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    public class PrimitiveExample : ArtPrimitive
    {

        void OnDrawGizmos()
        {
            if (ArtSets.GetVisible(ArtSetTag) && ArtGroups.GetVisible(ArtGroupTag, ArtCategoryTag))
            {
                Gizmos.color = ArtSets.GetLineColor(ArtSetTag);
                Gizmos.DrawWireSphere(transform.position, 1f);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
            }        
        }
    }
}
