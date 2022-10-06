/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace XiGameTool.Core
{
    /// <summary>
    ///     Settings for single game category. Each category contains several subcategories
    /// </summary>

    public class Category
    {
        private string _name;
        private Texture _icon;
        private string _description;
        private int _quantity;
        private readonly List<Subcategory> _subcategories = new List<Subcategory>();

        public Category(string name, Texture icon, string description)
        {
            _name = name;
            _icon = icon;
            _description = description;
        }

        public List<Subcategory> Subcategories => _subcategories;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Texture Icon => _icon ?? Texture2D.redTexture;


        /// <summary>
        ///     Is this group visible or not
        /// </summary>
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

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public void UpdateCount()
        {
            var cnt = 0;
            foreach (var sub in Subcategories)
            {
                cnt += sub.Quantity;
            }
            Quantity = cnt;
        }


        /// <summary>
        ///     Get sub in this group
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Subcategory FindSubcategory(string name)
        {
            return Subcategories.Single(s => s.Name == name);
        }

        public Subcategory AddSubcategory(string name, Category category, Texture icon, string description)
        {
            var item = new Subcategory(name, category, icon, description);
            _subcategories.Add(item);
            return item; 
        }

        public void ClearSubcategories()
        {
            _subcategories.Clear();
        }
    }
}
