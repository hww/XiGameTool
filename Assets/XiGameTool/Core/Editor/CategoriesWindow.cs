/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
	public class CategoriesWindow : EditorWindow {


	
		private static Texture _arrowDownIcon;
		private static Texture _visibleIcon;
		private static Texture _invisibleIcon;
	
		private const float IconWidth = 24;
		private const float IconHeight = 20;
		private readonly GUILayoutOption _iconWidthOption = GUILayout.Width(IconWidth);
		private readonly GUILayoutOption _iconHeightOption = GUILayout.Height(IconHeight);
		private readonly GUILayoutOption _labelWidhtOption = GUILayout.Width(200);
		private readonly GUILayoutOption _quantityWidthOption = GUILayout.Width(50);
		private readonly GUILayoutOption _colorWidthOption = GUILayout.Width(50);

		private GUIStyle _buttonStyle;
		private readonly GroupView[] _groupViews = new GroupView[System.Enum.GetValues(typeof(EArtGroup)).Length];
		private Vector2 _scrollPos;


		[MenuItem("Xi/Window/Categories")]
		public static void ShowWindow ()
		{
			GetWindow<CategoriesWindow>("XiGameTool: Categories");
		}

		void OnEnable()
		{
			EditorApplication.hierarchyChanged -= CountObjects;
			EditorApplication.hierarchyChanged += CountObjects;
			if (_buttonStyle == null)
			{
				_buttonStyle = new GUIStyle();
				_buttonStyle.padding = new RectOffset(1,1,1,1);
			}
			if (_arrowDownIcon == null)
				_arrowDownIcon = Resources.Load<Texture>("XiGameTool/Images/ui_arrow_down_white");
			if (_visibleIcon == null)
				_visibleIcon = Resources.Load<Texture>("XiGameTool/Images/ui_visible");
			if (_invisibleIcon == null)
				_invisibleIcon = Resources.Load<Texture>("XiGameTool/Images/ui_invisible");
			
			ArtGroups.Initialize();
			
			CreateGroupView(ArtGroups.Globals, "XiGameTool/Images/grp_env_ball");
			CreateGroupView(ArtGroups.Gameplay, "XiGameTool/Images/grp_pacman");
			CreateGroupView(ArtGroups.Camera, "XiGameTool/Images/grp_camera");
			CreateGroupView(ArtGroups.Sounds, "XiGameTool/Images/grp_sound");
			CreateGroupView(ArtGroups.Rendering, "XiGameTool/Images/grp_rendering");
			CreateGroupView(ArtGroups.Particles, "XiGameTool/Images/grp_particle");
			CountObjects();
		}

		private GroupView CreateGroupView(ArtGroup artGroup, string iconName)
		{
			var groupView = new GroupView(artGroup, iconName);	
			_groupViews[(int)artGroup.ArtGroupTag] = groupView;
			return groupView;
		}
		
		private void OnDisable()
		{
			for (var i = 0; i < _groupViews.Length; i++)
				_groupViews[i] = null;
		}



		void OnGUI ()
		{
			// -- render tool bar --
			GUILayout.BeginHorizontal();
			ArtGroups.ShowOptional = GUILayout.Toggle(ArtGroups.ShowOptional, "Display Oprional");
			if (GUILayout.Button("Count"))
				CountObjects();
			GUILayout.EndHorizontal();
			DrawSeparator();
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			for (var i = 0; i < _groupViews.Length; i++)
				RenderGroup(_groupViews[i]);
			EditorGUILayout.EndScrollView();
		}
		private void DrawSeparator()
		{
			GUILayout.Box(string.Empty, GUILayout.Height(2f), GUILayout.ExpandWidth(true));
		}
		private void CountObjects()
		{
			for (var i = 0; i < _groupViews.Length; i++)
				_groupViews[i]?.PreCountArtObjects();
            
			var objects = FindObjectsOfType<ArtPrimitive>();
            
			for (var i = 0; i < objects.Length; i++)
			{
				var obj = objects[i];
				if (obj == null)
					continue;
				CountObject(obj);
			}
			
			// now update counters of all groups
			for (var i = 0; i < _groupViews.Length; i++)
				_groupViews[i].PostCountArtObjects();
		}
        
		private void CountObject(ArtPrimitive obj)
		{
			var group = GetGroup(obj.ArtGroup);
			group.CountArtObject(obj);
		}
		
		private GroupView GetGroup(EArtGroup artGroupTag)
		{
			return _groupViews[(int) artGroupTag];
		}

		/// <summary>
		/// Settings for single group
		/// </summary>
		private void RenderGroup(GroupView groupView)
		{
			var group = groupView.ArtGroup;
			GUILayout.BeginHorizontal();
			// -- 0 ---------------------------------------------------
			GUILayout.Box(groupView.Icon, _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 1 ---------------------------------------------------
			var isVisible = group.IsVisible;
			if (GUILayout.Button( isVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
				group.IsVisible = !isVisible;
			// -- 2 ---------------------------------------------------
			GUILayout.Box("", _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 3 ---------------------------------------------------
			GUILayout.Label(group.ArtGroupTag.ToString(), EditorStyles.boldLabel);
			// -- 4 ---------------------------------------------------
			GUILayout.Label(groupView.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
			GUILayout.EndHorizontal();

			var categories = groupView.Categories;
			var count = categories.Length;
			for (var i = 0; i < count; i++)
			{
				var category = categories[i];
				
				if (!ArtGroups.ShowOptional && category.IsOptional && category.Quantity == 0)
					continue;
				RenderCategory(category);
			}
		
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		}
	

		/// <summary>
		/// Render single category
		/// </summary>
		private void RenderCategory(CategoryView categoryView)
		{
			var category = categoryView.Category;

			GUILayout.BeginHorizontal();
			// -- 0 ---------------------------------------------------
			GUILayout.Box("", _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 1 ---------------------------------------------------
			if (GUILayout.Button(category.IsVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
				category.IsVisible = !category.IsVisible;
			// -- 2 ---------------------------------------------------
			GUILayout.Box(categoryView.Icon, _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 3 ---------------------------------------------------
			GUILayout.Label(category.Category.ToString(), EditorStyles.largeLabel);
			// -- 4 ---------------------------------------------------
			GUILayout.Label(categoryView.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
		
			GUILayout.EndHorizontal();
		}
		
	}

	/// <summary>
	/// Interface to the GroupSettings
	/// </summary>
	public class GroupView
	{
		public readonly Texture Icon;
		public readonly CategoryView[] Categories = new CategoryView[(int)EArtCategory.Count];
		public readonly ArtGroup ArtGroup;
		public int Quantity;


		
		private readonly CategoryView _featureOverlays;
		private readonly CategoryView _navShapes;
		private readonly CategoryView _traversal;
		private readonly CategoryView _actorsSpawners;
		private readonly CategoryView _regions;
		private readonly CategoryView _splines;
		
		public GroupView(ArtGroup artGroup, string iconName)
		{
			Debug.Assert(artGroup != null);
			this.ArtGroup = artGroup;
			Icon = Resources.Load<Texture>(iconName);
			
			_featureOverlays = CreateCategoryView(artGroup.FeatureOverlays, "XiGameTool/Images/cat_overlay");
			_navShapes = CreateCategoryView(artGroup.NavShapes, "XiGameTool/Images/cat_navigation");
			_traversal = CreateCategoryView(artGroup.Traversal, "XiGameTool/Images/cat_traversal");
			_actorsSpawners = CreateCategoryView(artGroup.ActorsSpawners, "XiGameTool/Images/cat_actor");
			_regions = CreateCategoryView(artGroup.Regions, "XiGameTool/Images/cat_region");
			_splines = CreateCategoryView(artGroup.Splines, "XiGameTool/Images/cat_spline");
		}

		private CategoryView CreateCategoryView(ArtCategory artCategory, string iconName)
		{
			if (artCategory == null)
				return null;
			var categoryView = new CategoryView(artCategory, iconName);
			Categories[(int)categoryView.Category.Category] = categoryView;
			return categoryView;
		}
		
		        
		public void PreCountArtObjects()
		{
			for (var i = 0; i < Categories.Length; i++)
				Categories[i].Quantity = 0;
		}
		
		public void CountArtObject(ArtPrimitive obj)
		{
			switch (obj.ArtCategory)
			{
				case EArtCategory.ActorsSpawners:
					_actorsSpawners.Quantity++;
					break;
				case EArtCategory.NavShapes:
					_navShapes.Quantity++;
					break;
				case EArtCategory.Splines:
					_splines.Quantity++;
					break;
				case EArtCategory.Regions:
					_regions.Quantity++;
					break;
				case EArtCategory.Traversal:
					_traversal.Quantity++;
					break;
				case EArtCategory.FeatureOverlays:
					_featureOverlays.Quantity++;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void PostCountArtObjects()
		{
			Quantity = 0;
			var count = Categories.Length;
			for (var i = 0; i < count; i++)
			{
				var category = Categories[i];
				if (category != null)
					Quantity += category.Quantity;
			}
		}
	}

	/// <summary>
	/// Interface to the CategorySettings
	/// </summary>
	public  class CategoryView
	{
		public readonly Texture Icon;
		public readonly ArtCategory Category;
		public readonly bool IsOptional;
		public int Quantity;
		
		public CategoryView(ArtCategory category, string iconName)
		{
			this.Category = category;
			IsOptional = category.IsOptional;
			Icon = Resources.Load<Texture>(iconName);
		}
	}
}
