using NaughtyAttributes;
using UnityEngine;

namespace XiGameTool
{
    /// <summary>A game tool settings behaviour.</summary>
    public class GameToolSettingsBehaviour : MonoBehaviour
    {
        /// <summary>Options for controlling the operation.</summary>
        [InfoBox("Reffer to the asset for making settings for this scene", EInfoBoxType.Normal)]
        public GameToolSettings settings;
    }
}
