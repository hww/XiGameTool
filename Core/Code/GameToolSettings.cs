using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using XiGameTool.Core;

namespace XiGameTool
{
    /// <summary>A game tool settings.</summary>
    [CreateAssetMenu(fileName = "GameToolSettings", menuName = "Xi/Settings/GameToolSettings")]
    public class GameToolSettings : ScriptableObject
    {
        private static string _defaultGameTypeName = "ActorSpawners";
        private static string _defaultGameTypeDescription = "The game object wich produce the actor or multiple actors";
        private static Texture _defaultGameTypeIcon;
        private static string _defaultGameSetName = "Default";
        private static string _defaultGameSetDescription = "The game objects not assigned to any set";
        private static Texture _defaultGameSetIcon;
        private static string _defaultGameCategoryName = "Global";
        private static string _defaultGameCategoryDescription = "The game objects not assigned to any category";
        private static Texture _defaultGameCategoryIcon;

        static Texture DefaultGameTypeIcon => _defaultGameTypeIcon ??= Resources.Load<Texture>("XiGameTool/Images/cat_actor");
        static Texture DefaultGameSetIcon => _defaultGameSetIcon ??= Resources.Load<Texture>("XiGameTool/Images/ui_set");
        static Texture DefaultGameCategoryIcon => _defaultGameCategoryIcon ??= Resources.Load<Texture>("XiGameTool/Images/grp_env_ball");

        /// <summary>Called when the object becomes enabled and active.</summary>
        private void OnEnable()
        {
            OnValidate_GameTypes();
            OnValidate_GameSets();
            OnValidate_GameCategories();
        }

        ///--------------------------------------------------------------------
        /// <summary>Called when the script is loaded or a value is changed in
        /// the inspector (Called in the editor only)</summary>
        ///--------------------------------------------------------------------

        private void OnValidate()
        {
            ErrorMessage = string.Empty;
            OnValidate_GameTypes();
            OnValidate_GameSets();
            OnValidate_GameCategories();
        }

        /// <summary>The method will guaranty as minimum one default entry.</summary>
        void Reset()
        {
            Reset_GameTypes();
            Reset_GameSets();
            Reset_GameCategories();
        }

        ///--------------------------------------------------------------------
        /// <summary>Called one before count the objects of scene Then will be
        /// incremented each counter again.</summary>
        ///--------------------------------------------------------------------

        public void ClearCounters()
        {
            foreach (var cat in Categories)
                cat.Quantity = 0;

            foreach (var sub in Subcategories)
                sub.Quantity = 0;

            foreach (var selSet in SelectionSets)
                selSet.Quantity = 0;

            foreach (var lay in GameTool.Layers.AllLayers)
                lay.Quantity = 0;
        }

        #region The Initialization
        /// <summary>Resets to default.</summary>
        [NaughtyAttributes.Button]
        void ResetToDefault()
        {
            var source = GameTool.FindDefaultAssetPath(true);
            if (source != null)
            {
                _gameTypes = source._gameTypes;
                _selectionSetsData = source._selectionSetsData;
                _gameCategoriesData = source._gameCategoriesData;
                return;
            }
            Debug.LogError($"Can't find default asset template  anywere", this);
        }
        #endregion

        #region GameTypes

        /// <summary>List of game types.</summary>
        [ValidateInput("IsUniqueGameTypeName", "Expects have unique names")]
        [SerializeField]
        private List<GameTypeData> _gameTypes = new List<GameTypeData>();

        ///--------------------------------------------------------------------
        /// <summary>Gets a list of game types.</summary>
        ///
        /// <value>A list of types of the games.</value>
        ///--------------------------------------------------------------------

        public IReadOnlyList<GameTypeData> GameTypes => _gameTypes;

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first game type.</summary>
        ///
        /// <param name="name">  The name.</param>
        /// <param name="result">[out] The result.</param>
        ///
        /// <returns>True if it succeeds, false if it fails.</returns>
        ///--------------------------------------------------------------------

        public bool FindGameType(string name, out GameTypeData result)
        {
            foreach (var item in GameTypes)
            {
                if (item.Name == name)
                {
                    result = item;
                    return true;
                }
            }
            result = GameTypes[0];
            return false;
        }

#if UNITY_EDITOR

        ///--------------------------------------------------------------------
        /// <summary>Query if this object is unique game type name.</summary>
        ///
        /// <returns>True if unique game type name, false if not.</returns>
        ///--------------------------------------------------------------------

        private bool IsUniqueGameTypeName()
        {
            return (_gameTypes.Count == _gameTypes.Select(x => x.Name).Distinct().Count());
        }

        /// <summary>Executes the 'validate game types' action.</summary>
        private void OnValidate_GameTypes()
        {
            Reset_GameTypes();
            for (int i = 0; i < _gameTypes.Count; i++)
            {
                var gameType = _gameTypes[i];
                if (gameType.Name == null)
                    gameType.Name = $"Game Type {i}";
                _gameTypes[i] = gameType;
            }
            // Find duplicates
            if (!IsUniqueGameTypeName())
                Debug.LogError($"Duplicate GameType Name in the assed '{name}", this);
        }

        /// <summary>Resets the game types.</summary>
        private void Reset_GameTypes()
        {
            if (_gameTypes.Count == 0)
            {
                _gameTypes.Add(new GameTypeData() { Name = _defaultGameTypeName, Icon = DefaultGameTypeIcon, Description = _defaultGameTypeDescription });
            }
        }

#endif
        #endregion

        #region GameSets
        /// <summary>Information describing the selection sets.</summary>
        [FormerlySerializedAs("_gameSetsData")]
        [ValidateInput("IsUniqueGameSetName", "Expects to have unique names")]
        [SerializeField]
        private List<GameSetData> _selectionSetsData = new List<GameSetData>();

        /// <summary>Sets the selection belongs to.</summary>
        private List<SelectionSet> _selectionSets = new List<SelectionSet>();

        ///--------------------------------------------------------------------
        /// <summary>Gets the selection sets.</summary>
        ///
        /// <value>The selection sets.</value>
        ///--------------------------------------------------------------------

        public IReadOnlyList<SelectionSet> SelectionSets
        {
            get
            {
                if (_selectionSets == null || _selectionSets.Count == 0)
                    _selectionSets = _selectionSetsData.Select(o => new SelectionSet(o.Name, o.Color, o.Icon, o.Description)).ToList();
                return _selectionSets;
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first selection set.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The found selection set.</returns>
        ///--------------------------------------------------------------------

        public SelectionSet FindSelectionSet(string name)
        {
            if (string.IsNullOrEmpty(name))
                return SelectionSets[0];
            var item = SelectionSets.FirstOrDefault<SelectionSet>(o => o.Name == name);
            if (item == null)
            {
                Debug.LogWarning($"The selection set '{name}' is not found in '{this.name}'", this);
                return SelectionSets[0];
            }
            return item;
        }


#if UNITY_EDITOR

        ///--------------------------------------------------------------------
        /// <summary>Query if this object is unique game set name.</summary>
        ///
        /// <returns>True if unique game set name, false if not.</returns>
        ///--------------------------------------------------------------------

        private bool IsUniqueGameSetName()
        {
            return (_selectionSetsData.Count == _selectionSetsData.Select(o => o.Name).Distinct().Count());
        }

        /// <summary>Executes the 'validate game sets' action.</summary>
        private void OnValidate_GameSets()
        {
            _selectionSets = null; /* Force Reinit */
            Reset_GameSets();
            for (int i = 0; i < _selectionSetsData.Count; i++)
            {
                var gameSet = _selectionSetsData[i];
                if (gameSet.Name == null)
                    gameSet.Name = $"SelectionSet {i}";
                _selectionSetsData[i] = gameSet;
            }
            if (!IsUniqueGameSetName())
                Debug.LogError($"Duplicate GameType Names in the asset '{name}'", this);
        }
        /// <summary>Resets the game sets.</summary>
        private void Reset_GameSets()
        {
            if (_selectionSetsData.Count == 0)
            {
                _selectionSetsData.Add(new GameSetData()
                {
                    Name = _defaultGameSetName,
                    Color = Color.white,
                    Icon = DefaultGameSetIcon,
                    Description = _defaultGameSetDescription
                });
            }
        }
#endif
        #endregion

        #region GameCategories

        /// <summary>Message describing the error.</summary>
        [ValidateInput("IsEmptyMessage", "There is error message below")]
        [TextArea(0,5)]
        public string ErrorMessage;

        /// <summary>Information describing the game categories.</summary>
        [ValidateInput("IsUniqueGameCategoryName", "Expects to have unique names")]
        [SerializeField]
        [AllowNesting]
        private List<GameCategoryData> _gameCategoriesData = new List<GameCategoryData>();

        /// <summary>Categories the game belongs to.</summary>
        private List<Category> _gameCategories = null;
        /// <summary>The game subcategories.</summary>
        private List<Subcategory> _gameSubcategories = null;

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first category.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The found category.</returns>
        ///--------------------------------------------------------------------

        public Category FindCategory(string name)
        {
            var item = Categories.FirstOrDefault<Category>(o=>o.Name == name);
            if (item == null)
            {
                Debug.LogWarning($"The category '{name}' is not found in '{this.name}'", this);
                return Categories[0];
            }
            return item;
        }

        ///--------------------------------------------------------------------
        /// <summary>Searches for the first subcategory.</summary>
        ///
        /// <param name="name">The name.</param>
        ///
        /// <returns>The found subcategory.</returns>
        ///--------------------------------------------------------------------

        public Subcategory FindSubcategory(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Subcategories[0];
            var item = Subcategories.FirstOrDefault<Subcategory>(o => o.FullName == name);
            if (item == null)
            {
                Debug.LogWarning($"The subcategory '{name}' is not found in '{this.name}'", this);
                return Subcategories[0];
            }
            return item;
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets the categories.</summary>
        ///
        /// <value>The categories.</value>
        ///--------------------------------------------------------------------

        public IReadOnlyList<Category> Categories
        {
            get
            {
                if (_gameCategories == null)
                {
                    _gameCategories = _gameCategoriesData.Select(o => new Category(o.Name, o.Icon, o.Description)).ToList();
                }
                return _gameCategories;
            }
        }

        ///--------------------------------------------------------------------
        /// <summary>Gets the subcategories.</summary>
        ///
        /// <value>The subcategories.</value>
        ///--------------------------------------------------------------------

        public IReadOnlyList<Subcategory> Subcategories
        {
            get
            {
                if (_gameSubcategories == null)
                {
                    _gameSubcategories = new List<Subcategory>();
                    var catsData = _gameCategoriesData;
                    foreach (var catData in catsData)
                    {
                        var cat = FindCategory(catData.Name);
                        cat.ClearSubcategories();
                        foreach (var sub in catData.Subcategories)
                        {
                            GameTypeData type;
                            if (!FindGameType(sub, out type))
                                Debug.LogError($"The GameType '{name}' is not found in the asset '{name}'", this);
                            var item = cat.AddSubcategory(sub, cat, type.Icon, type.Description);
                            _gameSubcategories.Add(item);
                        }
                    }
                }
                return _gameSubcategories;
            }
        }

#if UNITY_EDITOR

        ///--------------------------------------------------------------------
        /// <summary>Query if this object is empty message.</summary>
        ///
        /// <returns>True if empty message, false if not.</returns>
        ///--------------------------------------------------------------------

        private bool IsEmptyMessage()
        {
            return string.IsNullOrEmpty(ErrorMessage);
        }

        ///--------------------------------------------------------------------
        /// <summary>Query if this object is unique game category name.</summary>
        ///
        /// <returns>True if unique game category name, false if not.</returns>
        ///--------------------------------------------------------------------

        private bool IsUniqueGameCategoryName()
        {
            return (_gameCategoriesData.Count == _gameCategoriesData.Select(o => o.Name).Distinct().Count());
        }

        /// <summary>Executes the 'validate game categories' action.</summary>
        private void OnValidate_GameCategories()
        {
            _gameCategories = null; /* Force Reinit */
            _gameSubcategories = null; /* Force Reinit */

            Reset_GameCategories();
            for (int i = 0; i < _gameCategoriesData.Count; i++)
            {
                var gameCategory = _gameCategoriesData[i];
                if (gameCategory.Name == null) { 
                    gameCategory.Name = $"Game Category {i}";
                    _gameCategoriesData[i] = gameCategory;
                }
                string msg;
                if (!gameCategory.VerifyValidNames(_gameTypes, out msg))
                {
                    Debug.LogError($"There is error in game category '{gameCategory.Name}' at [{i}] in the asset '{name}' : {ErrorMessage}", this);
                    ErrorMessage += $"{msg}\n";
                }
            }
            if (!IsUniqueGameCategoryName())
                Debug.LogError($"Duplicate Category Names in the asset '{name}'", this);


        }
        /// <summary>Resets the game categories.</summary>
        private void Reset_GameCategories()
        {
            if (_gameCategoriesData.Count == 0)
            {
                _gameCategoriesData.Add(new GameCategoryData() { Name = _defaultGameCategoryName, Icon = DefaultGameCategoryIcon, Description = _defaultGameCategoryDescription });
            }
        }
#endif
        #endregion

    }

    /// <summary>A game type data.</summary>
    [System.Serializable]
    public struct GameTypeData
    {
        public string Name;
        public string Description;
        public Texture Icon;
    }
    /// <summary>A game set data.</summary>
    [System.Serializable]
    public struct GameSetData
    {
        public string Name;
        public string Description;
        public Color Color;
        public Texture Icon;
    }

    /// <summary>A game category data.</summary>
    [System.Serializable]
    public struct GameCategoryData
    {
        public string Name;
        public string Description;
        public Texture Icon;
        public List<string> Subcategories;

#if UNITY_EDITOR

        ///--------------------------------------------------------------------
        /// <summary>Query if this object is unique subcategories name.</summary>
        ///
        /// <returns>True if unique subcategories name, false if not.</returns>
        ///--------------------------------------------------------------------

        public bool IsUniqueSubcategoriesName()
        {
            return (Subcategories.Count == Subcategories.Select(o => o).Distinct().Count());
        }

        ///--------------------------------------------------------------------
        /// <summary>Verify valid names.</summary>
        ///
        /// <param name="types">  The types.</param>
        /// <param name="message">[out] The message.</param>
        ///
        /// <returns>True if it succeeds, false if it fails.</returns>
        ///--------------------------------------------------------------------

        public bool VerifyValidNames(List<GameTypeData> types, out string message)
        {
            if (!IsUniqueSubcategoriesName())
            {
                message = "Expected uniques subcategories";
                return false;
            }
            foreach (var name in Subcategories)
            {
                if (types.Select(o => o.Name).Contains(name))
                    continue;
                message = $"Undefined subcategory name '{name}' in category '{Name}'";
                return false;
            }
            message = null;
            return true;
        }
#endif
    }
}
