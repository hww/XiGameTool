/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace XiGameTool.Core
{
    ///------------------------------------------------------------------------
    /// <summary>Settings for single game category. Each category contains
    /// several subcategories.</summary>
    ///------------------------------------------------------------------------

    public class Category
    {
        /// <summary>The name.</summary>
        private string _name;
        /// <summary>The icon.</summary>
        private Texture _icon;
        /// <summary>The description.</summary>
        private string _description;
        /// <summary>The quantity.</summary>
        private int _quantity;
        /// <summary>(Immutable) the subcategories.</summary>
        private readonly List<Subcategory> _subcategories = new List<Subcategory>();

        ///--------------------------------------------------------------------
        /// <summary>Constructor.</summary>
        ///
        /// <param name="name">       Category name.</param>
        /// <param name="icon">       The icon.</param>
        /// <param name="description">The description.</param>
        ///--------------------------------------------------------------------

        public Category(string name, Texture icon, string description)
        {
            _name = name;
            _icon = icon;
            _description = description;
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets the subcategories.</summary>
        ///
        /// <value>The subcategories.</value>
        ///--------------------------------------------------------------------

        public List<Subcategory> Subcategories => _subcategories;

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the name.</summary>
        ///
        /// <value>The name.</value>
        ///--------------------------------------------------------------------

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the description.</summary>
        ///
        /// <value>The description.</value>
        ///--------------------------------------------------------------------

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets the icon.</summary>
        ///
        /// <value>The icon.</value>
        ///--------------------------------------------------------------------

        public Texture Icon => _icon != null ? _icon : Texture2D.redTexture;

        ///--------------------------------------------------------------------
        /// <summary>Is this group visible or not.</summary>
        ///
        /// <value>True if this object is visible, false if not.</value>
        ///--------------------------------------------------------------------

        public bool IsVisible
        {
            get
            {
                foreach (var type in Subcategories)
                {
                    if (type.IsVisible)
                        return true;
                }
                return false;
            }
            set
            {
                foreach (var type in Subcategories)
                    type.IsVisible = value;
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets or sets the quantity.</summary>
        ///
        /// <value>The quantity.</value>
        ///--------------------------------------------------------------------

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>Updates the count.</summary>
        public void UpdateCount()
        {
            var cnt = 0;
            foreach (var sub in Subcategories)
            {
                cnt += sub.Quantity;
            }
            Quantity = cnt;
        }

        ///--------------------------------------------------------------------
        /// <summary>Get subcategory in this category.</summary>
        ///
        /// <param name="name">Subcategory name.</param>
        ///
        /// <returns>The found subcategory.</returns>
        ///--------------------------------------------------------------------

        public Subcategory FindSubcategory(string name)
        {
            return Subcategories.Single(s => s.Name == name);
        }

        ///--------------------------------------------------------------------
        /// <summary>Adds a subcategory.</summary>
        ///
        /// <param name="name">       The name of category.</param>
        /// <param name="category">   The category.</param>
        /// <param name="icon">       The icon.</param>
        /// <param name="description">The description.</param>
        ///
        /// <returns>A Subcategory.</returns>
        ///--------------------------------------------------------------------

        public Subcategory AddSubcategory(string name, Category category, Texture icon, string description)
        {
            var item = new Subcategory(name, category, icon, description);
            _subcategories.Add(item);
            return item; 
        }

        /// <summary>Clears the subcategories.</summary>
        public void ClearSubcategories()
        {
            _subcategories.Clear();
        }
    }
}
