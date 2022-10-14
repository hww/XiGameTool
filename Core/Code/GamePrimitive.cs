/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace XiGameTool.Core
{
    /// <summary>This class allow to mark object as the one on art category.</summary>
    public class GamePrimitive : MonoBehaviour
    {
        [BoxGroup("Art Primitive")]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnSubcategoryChanged))]
        [Dropdown(nameof(SubcategorySelector))]
#endif
        /// <summary>Name of the subcategory.</summary>
        public string subcategoryName;

        [BoxGroup("Art Primitive")]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnSelectionSetChanged))]
        [Dropdown(nameof(SelectionSetSelector))]
#endif
        /// <summary>Name of the selection set.</summary>
        public string selectionSetName;
        /// <summary>The subcategory.</summary>
        private Subcategory _subcategory;
        /// <summary>Set the selection belongs to.</summary>
        private SelectionSet _selectionSet;

        ///--------------------------------------------------------------------
        /// <summary>Get the type in this category.</summary>
        ///
        /// <value>The subcategory.</value>
        ///--------------------------------------------------------------------

        public Subcategory Subcategory
        {
            get => _subcategory ??= GameTool.FindSubcategory(subcategoryName);
            set { _subcategory = value; subcategoryName = value.FullName; }
        }

        ///--------------------------------------------------------------------
        /// <summary>Get selection set for this primitive.</summary>
        ///
        /// <value>The selection set.</value>
        ///--------------------------------------------------------------------

        public SelectionSet SelectionSet
        {
            get => _selectionSet ??= GameTool.FindSelectionSet(selectionSetName);
            set { _selectionSet = value; selectionSetName = value.Name; }
        }

        ///--------------------------------------------------------------------
        /// <summary>Get layer of this primitive.</summary>
        ///
        /// <value>The layer.</value>
        ///--------------------------------------------------------------------

        public GameLayer Layer
        {
            get => GameTool.Layers.Find(gameObject.layer);
        }

        /// <summary>Increment counters.</summary>
        public void IncrementCounters()
        {
            _subcategory = null;
            _selectionSet = null;
            Subcategory.Quantity++;
            SelectionSet.Quantity++;
            Layer.Quantity++;
        }

        ///--------------------------------------------------------------------
        /// <summary>Called when the script is loaded or a value is changed in
        /// the inspector (Called in the editor only)</summary>
        ///--------------------------------------------------------------------

        private void OnValidate()
        {
            _selectionSet = null;
            _subcategory = null;
        }

#if UNITY_EDITOR
        /// <summary>Executes the 'selection set changed' action.</summary>
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

        /// <summary>Executes the 'subcategory changed' action.</summary>
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

        ///--------------------------------------------------------------------
        /// <summary>Selection set selector.</summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error
        ///                                 condition occurs.</exception>
        ///
        /// <returns>A list of.</returns>
        ///--------------------------------------------------------------------

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

        ///--------------------------------------------------------------------
        /// <summary>Subcategory selector.</summary>
        ///
        /// <exception cref="Exception">    Thrown when an exception error
        ///                                 condition occurs.</exception>
        ///
        /// <returns>A list of.</returns>
        ///--------------------------------------------------------------------

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

