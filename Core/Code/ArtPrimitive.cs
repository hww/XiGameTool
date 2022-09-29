/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using NaughtyAttributes;
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>
    ///     This class allow to mark object as the one on art category
    /// </summary>
    public class ArtPrimitive : MonoBehaviour
    {
        // Select the art group of this object
        [BoxGroup("Art Primitive")]
        public ArtGroupTag ArtGroupTag;
        
        // Select the art group of this object
        [BoxGroup("Art Primitive")]
        public ArtCategoryTag ArtCategoryTag;
        
        // Select the art set
        [BoxGroup("Art Primitive")]
        public ArtSetTag ArtSetTag;

        // Get group of this primitive 
        public ArtGroup GetArtGroup()
        {
            return ArtGroups.GetGroup(ArtGroupTag);
        }

        // Get category of this primitive
        public ArtCategory GetArtCategory()
        {
            return ArtGroups.GetGroup(ArtGroupTag).GetCategory(ArtCategoryTag);
        }

        // Get category of this primitive
        public ArtSet GetArtSet()
        {
            return ArtSets.GetArtSet(ArtSetTag);
        }
    }
    
}
