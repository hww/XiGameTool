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
        public EArtGroup ArtGroup;
        
        // Select the art group of this object
        [BoxGroup("Art Primitive")]
        public EArtCategory ArtCategory;
        
        // Select the art set
        [BoxGroup("Art Primitive")]
        public EArtSet ArtSet;

        // Get group of this primitive 
        public ArtGroup GetArtGroup()
        {
            return ArtGroups.GetGroup(ArtGroup);
        }

        // Get category of this primitive
        public ArtCategory GetArtCategory()
        {
            return ArtGroups.GetGroup(ArtGroup).GetCategory(ArtCategory);
        }

        // Get category of this primitive
        public ArtSet GetArtSet()
        {
            return ArtSets.GetArtSet(ArtSet);
        }
    }
    
}
