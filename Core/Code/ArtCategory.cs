/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using UnityEngine;


namespace XiGameTool.Core
{
    /// <summary>
    /// Settings for single group. Each group contains several categories
    /// </summary>
    public class ArtCategory
    {
        public readonly ArtType[] Categories = new ArtType[(int)EArtType.Count];
        
        public EArtCategory ArtGroupTag;
        public ArtType ActorsSpawners;
        public ArtType Regions;
        public ArtType Splines;
        public ArtType FeatureOverlays;
        public ArtType NavShapes;
        public ArtType Traversal;

        /// <summary>
        ///     Construct new art group
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="color"></param>
        public ArtCategory(EArtCategory groupTag)
        {
            ArtGroupTag = groupTag;
            var isOptional = true;
            FeatureOverlays = CreateCategory(EArtType.FeatureOverlays, isOptional);
            NavShapes = CreateCategory(EArtType.NavShapes, isOptional);
            Traversal = CreateCategory(EArtType.Traversal, isOptional);
            ActorsSpawners = CreateCategory(EArtType.ActorsSpawners, isOptional);
            Regions = CreateCategory(EArtType.Regions, isOptional);
            Splines = CreateCategory(EArtType.Splines, isOptional);
        }
                
        private ArtType CreateCategory(EArtType categoryTag, bool optional = false)
        {
            return Categories[(int)categoryTag] = new ArtType(ArtGroupTag, categoryTag, optional);
        }

        /// <summary>
        ///     Get category in this group
        /// </summary>
        /// <param name="categoryTag"></param>
        /// <returns></returns>
        public ArtType GetCategory(EArtType categoryTag)
        {
            return Categories[(int) categoryTag];
        }
        
        /// <summary>
        ///     Is this group visible or not
        /// </summary>
        public bool IsVisible
        {
            get
            {
                var count = Categories.Length;
                for (var i = 0; i < count; i++)
                {
                    var category = Categories[i];
                    if (category.IsVisible)
                        return true;
                }
                return false;
            }
            set
            {
                var count = Categories.Length;
                for (var i = 0; i < count; i++)
                    Categories[i].IsVisible = value;
            }
        }
    }
}
