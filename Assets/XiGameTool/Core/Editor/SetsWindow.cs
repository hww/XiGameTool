/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
	public class SetsWindow : EditorWindow {

		[MenuItem("Xi/Window/Sets")]
		public static void ShowWindow ()
		{
			GetWindow<SetsWindow>("XiGameTool: Sets");
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
			if (_lockIcon == null)
				_lockIcon = Resources.Load<Texture>("XiGameTool/Images/ui_lock");
			if (_unlockIcon == null)
				_unlockIcon = Resources.Load<Texture>("XiGameTool/Images/ui_unlock");		
			if (_layerImage == null)
				_layerImage = Resources.Load<Texture>("XiGameTool/Images/ui_layer");
			if (_colorFillIcon == null)
				_colorFillIcon = Resources.Load<Texture>("XiGameTool/Images/ui_color_fill");
			// -- initialize all sets --
			var sets = ArtSets.Sets;
			for (var i = 0; i < sets.Length; i++)
			{
				var artSet = sets[i];
				if (artSet!=null)
					SetViews[i] = new SetView(artSet, "XiGameTool/Icons/ui_set");
			}
			CountObjects();		
		}
	
		private static Texture _arrowDownIcon;
		private static Texture _visibleIcon;
		private static Texture _invisibleIcon;
		private static Texture _lockIcon;
		private static Texture _unlockIcon;
		private static Texture _layerImage;
		private static Texture _colorFillIcon;

		private const float IconWidth = 22;
		private const float IconHeight = 20;
		private readonly GUILayoutOption _iconWidthOption = GUILayout.Width(IconWidth);
		private readonly GUILayoutOption _iconHeightOption = GUILayout.Height(IconHeight);
		private readonly GUILayoutOption _quantityWidthOption = GUILayout.Width(50);
		private readonly GUILayoutOption _colorWidthOption = GUILayout.Width(30);
		private GUIStyle _buttonStyle;
		private Vector2 _scrollPos;
		public static readonly SetView[] SetViews = new SetView[32];
		
		void OnGUI ()
		{
			// -- render tool bar --
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Count Objects"))
			{
				CountObjects(); 
			}
			GUILayout.EndHorizontal();
			DrawSeparator();

			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			// -- render sets --
			for (var i = 0; i < SetViews.Length; i++)
			{
				var layer = SetViews[i];
				if (layer!=null)
					RenderSet(layer);
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
		private void RenderSet(SetView setView)
		{
			var layer = setView.ArtSet;
			GUILayout.BeginHorizontal();
			// -- 1 ---------------------------------------------------
			var isVisible = layer.IsVisible;
			if (GUILayout.Button( isVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
				layer.IsVisible = !isVisible;
			// -- 3 ---------------------------------------------------
			if (GUILayout.Button(_colorFillIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
			{
				SetsTools.AssignArtSet(setView.ArtSet.Tag);
				CountObjects();
			}
			// -- 2 ---------------------------------------------------
			layer.Color = EditorGUILayout.ColorField( layer.Color, _colorWidthOption);
			// -- 3 ---------------------------------------------------
#if XI_GAME_TOOL_ADD_LAYERS_ICON
			GUILayout.Box(setView.Icon, ButtonStyle, IconWidthOption, IconHeightOption);
#endif
			// -- 4 ---------------------------------------------------
			GUILayout.Label(layer.Name, EditorStyles.largeLabel);
			// -- 5 ---------------------------------------------------
			GUILayout.Label(setView.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
		
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Representation for single artSet
		/// </summary>
		public class SetView
		{
			public Texture Icon;
			public ArtSet ArtSet;
			public int Quantity;
			
			public SetView(ArtSet artSet, string iconName)
			{
				ArtSet = artSet;
				Icon = Resources.Load<Texture>(iconName);
			}
		}
		
		public static void CountObjects()
		{
			if (SetViews == null) return;
			var counts = SetsTools.CountInAllSets();
			for (var i = 0; i < SetViews.Length; i++)
			{
				var layer = SetViews[i];
				if (layer != null)
					layer.Quantity = counts[i];
			}
		}
	}
}