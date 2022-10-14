using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    /// <summary>A primitive example.</summary>
    public class PrimitiveExample : GamePrimitive
    {
        /// <summary>Draws gizmos that are also pickable and always drawn.</summary>
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
