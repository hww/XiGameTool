using NaughtyAttributes;
using UnityEngine;

namespace XiGameTool
{
    public class GameToolSettingsBehaviour : MonoBehaviour
    {
        [InfoBox("Reffer to the asset for making settings for this scene", EInfoBoxType.Normal)]
        public GameToolSettings settings;
    }
}
