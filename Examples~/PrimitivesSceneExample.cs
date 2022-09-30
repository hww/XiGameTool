using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiGameTool.Core;

namespace XiArtManager.Examples
{
    public class PrimitivesSceneExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ArtSets.SetLineColor(EArtSet.Default, Color.red);
            ArtSets.SetLineColor(EArtSet.Scripted, Color.yellow);

            ArtLayers.SetLineColor(GameLayer.Default, Color.green);
            ArtLayers.SetLineColor(GameLayer.PostProcessing, Color.cyan);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
