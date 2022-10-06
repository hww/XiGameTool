using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    public class PrimitiveExample : GamePrimitive
    {

        void OnDrawGizmos()
        {
            if (SelectionSet.IsVisible && Subcategory.IsVisible)
            {
                Gizmos.color = SelectionSet.Color;
                Gizmos.DrawWireSphere(transform.position, 1f);
                UnityEditor.Handles.Label(transform.position, gameObject.name);
            }        
        }
    }
}
