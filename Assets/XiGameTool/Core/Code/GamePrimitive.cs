/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>
    ///     This class allow to mark object as the one on art category
    /// </summary>
    public class GamePrimitive : MonoBehaviour
    {
        // Select the art group of this object
        [BoxGroup("Art Primitive")]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnSubcategoryChanged))]
        [Dropdown(nameof(SubcategorySelector))]
#endif
        public string subcategoryName;

        // Select the art set
        [BoxGroup("Art Primitive")]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnSelectionSetChanged))]
        [Dropdown(nameof(SelectionSetSelector))]
#endif
        public string selectionSetName;

        private Subcategory _subcategory;
        private SelectionSet _selectionSet;

        // Get the type in this category
        public Subcategory Subcategory
        {
            get => _subcategory ??= GameTool.FindSubcategory(subcategoryName);
            set { _subcategory = value; subcategoryName = value.FullName; }
        }
        // Get selection set for this primitive
        public SelectionSet SelectionSet
        {
            get => _selectionSet ??= GameTool.FindSelectionSet(selectionSetName);
            set { _selectionSet = value; selectionSetName = value.Name; }
        }

        // Get layer of this primitive
        public GameLayer Layer
        {
            get => GameTool.Layers.Find(gameObject.layer);
        }

        public void IncrementCounters()
        {
            _subcategory = null;
            _selectionSet = null;
            Subcategory.Quantity++;
            SelectionSet.Quantity++;
            Layer.Quantity++;
        }

        private void OnValidate()
        {
            _selectionSet = null;
            _subcategory = null;
        }

#if UNITY_EDITOR
        private void OnSelectionSetChanged()
        {
            if (selectionSetName == default)
            {
                _selectionSet = null;
            }
            else
            {
                _selectionSet = GameTool.FindSelectionSet(selectionSetName);
            }
        }

        private void OnSubcategoryChanged()
        {
            if (selectionSetName == default)
            {
                _subcategory = null;
            }
            else
            {
                _subcategory = GameTool.FindSubcategory(subcategoryName);
            }
        }

        private DropdownList<string> SelectionSetSelector()
        {
            var gameSets = GameTool.SelectionSets;
            if (gameSets == null)
            {
                Debug.LogError("The SelectionSets is not exists");
                throw new System.Exception("Exception to prevent data corruption. Look at error above.");
            }
            else
            {
                var list = new NaughtyAttributes.DropdownList<string>();
                if (string.IsNullOrEmpty(selectionSetName))
                {
                    list.Add($"", "");
                }
                else
                {
                    list.Add($"<empty>", "");
                    list.Add($"<{selectionSetName}>", subcategoryName);
                }
                foreach (var entry in gameSets)
                    list.Add(entry.Name, entry.Name);
                return list;
            }
        }

        private DropdownList<string> SubcategorySelector()
        {
            var gameSubcategories = GameTool.Subcategories;

            if (gameSubcategories == null)
            {
                Debug.LogError("The Subcategories is not exists");
                throw new System.Exception("Exception to prevent data corruption. Look at error above.");
            }
            else
            {
                var list = new NaughtyAttributes.DropdownList<string>();
                if (string.IsNullOrEmpty(selectionSetName))
                {
                    list.Add($"", "");
                }
                else
                {
                    list.Add($"<empty>", "");
                    list.Add($"<{subcategoryName}>", subcategoryName);
                }
                foreach (var entry in gameSubcategories)
                    list.Add(entry.FullName, entry.FullName);
                return list;
            }
        }

#endif
    }
}

