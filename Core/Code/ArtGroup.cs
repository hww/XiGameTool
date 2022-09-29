/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using UnityEngine;


namespace XiGameTool.Core
{
    /// <summary>
    /// Settings for single group. Each group contains several categories
    /// </summary>
    public class ArtGroup
    {
        public readonly ArtCategory[] Categories = new ArtCategory[(int)ArtCategoryTag.Count];
        
        public ArtGroupTag ArtGroupTag;
        public ArtCategory ActorsSpawners;
        public ArtCategory Regions;
        public ArtCategory Splines;
        public ArtCategory FeatureOverlays;
        public ArtCategory NavShapes;
        public ArtCategory Traversal;

        /// <summary>
        ///     Construct new art group
        /// </summary>
        /// <param name="groupTag"></param>
        /// <param name="color"></param>
        public ArtGroup(ArtGroupTag groupTag)
        {
            ArtGroupTag = groupTag;
            var isOptional = true;
            FeatureOverlays = CreateCategory(ArtCategoryTag.FeatureOverlays, isOptional);
            NavShapes = CreateCategory(ArtCategoryTag.NavShapes, isOptional);
            Traversal = CreateCategory(ArtCategoryTag.Traversal, isOptional);
            ActorsSpawners = CreateCategory(ArtCategoryTag.ActorsSpawners, isOptional);
            Regions = CreateCategory(ArtCategoryTag.Regions, isOptional);
            Splines = CreateCategory(ArtCategoryTag.Splines, isOptional);
        }
                
        private ArtCategory CreateCategory(ArtCategoryTag categoryTag, bool optional = false)
        {
            return Categories[(int)categoryTag] = new ArtCategory(ArtGroupTag, categoryTag, optional);
        }

        /// <summary>
        ///     Get category in this group
        /// </summary>
        /// <param name="categoryTag"></param>
        /// <returns></returns>
        public ArtCategory GetCategory(ArtCategoryTag categoryTag)
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
