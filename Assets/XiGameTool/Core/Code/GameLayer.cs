/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

namespace XiGameTool.Core
{
    /// <summary>
    /// Edit this enum for your own project
    /// </summary>
    public enum GameLayer
    {
        // -- BUILTIN LAYERS --
        Default,
        TransparentFx,
        IgnoreRayCast,
        BuiltinLayer3,
        Water,
        Ui,
        BuiltinLayer6,
        BuiltinLayer7,
        
        // -- GAME LAYERS (Define here layers of your game) --
        PostProcessing,
        Target,
        TargetFill,
        NotCollidable,
        ClawModel,
        ClawCollideWithTargets
    }
}