/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
	public class SelectionSetWindow : EditorWindow {

		[MenuItem("Xi/Window/Selection Sets")]
		public static void ShowWindow ()
		{
			GetWindow<SelectionSetWindow>("XiGameTool: Selection Sets");
		}

		void OnEnable()
		{
			EditorApplication.hierarchyChanged -= GameTool.CountAllObjects;
			EditorApplication.hierarchyChanged += GameTool.CountAllObjects;
			if (_buttonStyle == null)
			{
				_buttonStyle = new GUIStyle();
				_buttonStyle.padding = new RectOffset(1,1,1,1);
			}
			_arrowDownIcon = Resources.Load<Texture>("XiGameTool/Images/ui_arrow_down_white");
			_visibleIcon = Resources.Load<Texture>("XiGameTool/Images/ui_visible");
			_invisibleIcon = Resources.Load<Texture>("XiGameTool/Images/ui_invisible");
			_lockIcon = Resources.Load<Texture>("XiGameTool/Images/ui_lock");
			_unlockIcon = Resources.Load<Texture>("XiGameTool/Images/ui_unlock");		
			_layerImage = Resources.Load<Texture>("XiGameTool/Images/ui_layer");
			_colorFillIcon = Resources.Load<Texture>("XiGameTool/Images/ui_color_fill");
			_artSetIcon = Resources.Load<Texture>("XiGameTool/Images/ui_set");

			GameTool.CountAllObjects();		
		}
	
		private static Texture _arrowDownIcon;
		private static Texture _visibleIcon;
		private static Texture _invisibleIcon;
		private static Texture _lockIcon;
		private static Texture _unlockIcon;
		private static Texture _layerImage;
		private static Texture _colorFillIcon;
		private static Texture _artSetIcon;

		private const float IconWidth = 22;
		private const float IconHeight = 20;
		private readonly GUILayoutOption _iconWidthOption = GUILayout.Width(IconWidth);
		private readonly GUILayoutOption _iconHeightOption = GUILayout.Height(IconHeight);
		private readonly GUILayoutOption _quantityWidthOption = GUILayout.Width(50);
		private readonly GUILayoutOption _colorWidthOption = GUILayout.Width(30);
		private GUIStyle _buttonStyle;
		private Vector2 _scrollPos;
		
		void OnGUI ()
		{
			// -- render tool bar --
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Quantity Objects"))
			{
				GameTool.CountAllObjects();
				SceneView.RepaintAll();
			}
			GUILayout.EndHorizontal();
			DrawSeparator();

			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			// -- render sets --

			foreach (var artSet in GameTool.SelectionSets)
			{
				RenderSet(artSet);
			}
			EditorGUILayout.EndScrollView();
		}
		private void DrawSeparator()
		{
			GUILayout.Box(string.Empty, GUILayout.Height(2f), GUILayout.ExpandWidth(true));
		}
		/// <summary>
		/// Render single line
		/// </summary>
		private void RenderSet(SelectionSet aset)
		{
	
			GUILayout.BeginHorizontal();
			// -- 1 ---------------------------------------------------
			var isVisible = aset.IsVisible;
			if (GUILayout.Button(isVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
			{
				aset.IsVisible = !isVisible;
				SceneView.RepaintAll();
			}
			// -- 3 ---------------------------------------------------
			if (GUILayout.Button(_colorFillIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
			{
				SelectionSetTools.AssignArtSet(aset);
				GameTool.CountAllObjects();
				SceneView.RepaintAll();
			}
			// -- 2 ---------------------------------------------------
			aset.Color = EditorGUILayout.ColorField(aset.Color, _colorWidthOption);
			// -- 3 ---------------------------------------------------
#if XI_GAME_TOOL_ADD_LAYERS_ICON
			GUILayout.Box(setView.Icon, ButtonStyle, IconWidthOption, IconHeightOption);
#endif
			// -- 4 ---------------------------------------------------
			GUILayout.Label(aset.Name, EditorStyles.largeLabel);
			// -- 5 ---------------------------------------------------
			GUILayout.Label(aset.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
		
			GUILayout.EndHorizontal();
		}

	}
}