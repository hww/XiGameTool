/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace XiGameTool.Core.Editor
{
    /// <summary>Form for viewing the categories.</summary>
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
		private Vector2 _scrollPos;


        /// <summary>Shows the window.</summary>
		[MenuItem("Xi/Window/Game Categories")]
		public static void ShowWindow ()
		{
			GetWindow<CategoriesWindow>("XiGameTool: Categories");
		}

        /// <summary>Called when the object becomes enabled and active.</summary>
		void OnEnable()
		{
			EditorApplication.hierarchyChanged -= GameTool.CountAllObjects;
			EditorApplication.hierarchyChanged += GameTool.CountAllObjects;
			_buttonStyle = new GUIStyle();
			_buttonStyle.padding = new RectOffset(1,1,1,1);
			_arrowDownIcon = Resources.Load<Texture>("XiGameTool/Images/ui_arrow_down_white");
			_visibleIcon = Resources.Load<Texture>("XiGameTool/Images/ui_visible");
			_invisibleIcon = Resources.Load<Texture>("XiGameTool/Images/ui_invisible");
			GameTool.CountAllObjects();
		}

        ///--------------------------------------------------------------------
        /// <summary>Called when the behaviour becomes disabled or
        /// inactive.</summary>
        ///--------------------------------------------------------------------

		private void OnDisable()
		{

		}


        /// <summary>Called for rendering and handling GUI events.</summary>
		void OnGUI ()
		{
			// -- render tool bar --
			GUILayout.BeginHorizontal();
			GameTool.DisplayUnused = GUILayout.Toggle(GameTool.DisplayUnused, "Display Unused");
			if (GUILayout.Button("Quantity"))
			{
				GameTool.CountAllObjects();
				SceneView.RepaintAll();
			}
			GUILayout.EndHorizontal();
			DrawSeparator();
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			foreach (var category in GameTool.Categories)
				RenderCategory(category);
			EditorGUILayout.EndScrollView();
		}

        /// <summary>Draw a horizontal space-separator.</summary>
		private void DrawSeparator()
		{
			GUILayout.Box(string.Empty, GUILayout.Height(2f), GUILayout.ExpandWidth(true));
		}

        ///--------------------------------------------------------------------
        /// <summary>Settings for single group.</summary>
        ///
        /// <param name="category">The category.</param>
        ///--------------------------------------------------------------------

		private void RenderCategory(Category category)
		{

			GUILayout.BeginHorizontal();
			// -- 0 ---------------------------------------------------
			GUILayout.Box(category.Icon, _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 1 ---------------------------------------------------
			var isVisible = category.IsVisible;
			if (GUILayout.Button(isVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
			{
				category.IsVisible = !isVisible;
				SceneView.RepaintAll();
			}
			// -- 2 ---------------------------------------------------
			GUILayout.Box("", _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 3 ---------------------------------------------------
			GUILayout.Label(category.Name, EditorStyles.boldLabel);
			// -- 4 ---------------------------------------------------
			GUILayout.Label(category.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
			GUILayout.EndHorizontal();


			foreach (var subcategory in category.Subcategories)
			{
				if (subcategory.Quantity == 0 && !GameTool.DisplayUnused)
					continue;
				RenderType(subcategory);
			}
		
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		}

        ///--------------------------------------------------------------------
        /// <summary>Render single category.</summary>
        ///
        /// <param name="type">The type.</param>
        ///--------------------------------------------------------------------

		private void RenderType(Subcategory type)
		{
			GUILayout.BeginHorizontal();
			// -- 0 ---------------------------------------------------
			GUILayout.Box("", _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 1 ---------------------------------------------------
			if (GUILayout.Button(type.IsVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
				type.IsVisible = !type.IsVisible;
			// -- 2 ---------------------------------------------------
			GUILayout.Box(type.Icon, _buttonStyle, _iconWidthOption, _iconHeightOption);
			// -- 3 ---------------------------------------------------
			GUILayout.Label(type.Name, EditorStyles.largeLabel);
			// -- 4 ---------------------------------------------------
			GUILayout.Label(type.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
		
			GUILayout.EndHorizontal();
		}
		
	}
}
