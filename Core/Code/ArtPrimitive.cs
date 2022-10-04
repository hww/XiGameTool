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
        public EArtCategory ArtGroup;
        
        // Select the art group of this object
        [BoxGroup("Art Primitive")]
        public EArtType ArtCategory;
        
        // Select the art set
        [BoxGroup("Art Primitive")]
        public EArtSet ArtSet;

        // Get group of this primitive 
        public ArtCategory GetArtGroup()
        {
            return ArtCategories.GetGroup(ArtGroup);
        }

        // Get category of this primitive
        public ArtType GetArtCategory()
        {
            return ArtCategories.GetGroup(ArtGroup).GetCategory(ArtCategory);
        }

        // Get category of this primitive
        public ArtSet GetArtSet()
        {
            return ArtSets.GetArtSet(ArtSet);
        }
    }
    
}
