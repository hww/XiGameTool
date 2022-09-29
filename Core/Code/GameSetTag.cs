using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiArtManager.Libs;

namespace XiGameTool.Core
{
    /// <summary>
    /// Edit this enum for your own project
    /// </summary>
    public enum ArtSetTag
    {
        // -- BUILTIN LAYERS --
        Default,
        Scripted,
        Zones,
        JumpTargets
    }

    public static class ArtSetColors 
    {
        public static readonly Color Default = ColorTools.Parse("#939597");
        public static readonly Color Scripted = ColorTools.Parse("#00A170");
        public static readonly Color Zones = ColorTools.Parse("#E0B589");
        public static readonly Color JumpTargets = ColorTools.Parse("#926AA6");
    }
}
