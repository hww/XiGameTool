/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
	public class GameLayersWindow : EditorWindow {

		[MenuItem("Xi/Window/Unity Layers")]
		public static void ShowWindow ()
		{
			GetWindow<GameLayersWindow>("XiGameTool: Unity Layers");
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

			// -- initialize all layers --
			var layers = GameLayers.Layers;
			for (var i = 0; i < layers.Length; i++)
			{
				var layer = layers[i];
				if (layer!=null)
					LayerViews[i] = new LayerView(layer, "XiGameTool/Images/ui_layer");
			}
			CountObjects();		
		}
	
		private static Texture _arrowDownIcon;
		private static Texture _visibleIcon;
		private static Texture _invisibleIcon;
		private static Texture _lockIcon;
		private static Texture _unlockIcon;
		private static Texture _layerImage;
	 
		private const float IconWidth = 22;
		private const float IconHeight = 20;
		private readonly GUILayoutOption _iconWidthOption = GUILayout.Width(IconWidth);
		private readonly GUILayoutOption _iconHeightOption = GUILayout.Height(IconHeight);
		private readonly GUILayoutOption _quantityWidthOption = GUILayout.Width(50);
		private readonly GUILayoutOption _colorWidthOption = GUILayout.Width(30);
		private GUIStyle _buttonStyle;
		private Vector2 _scrollPos;
		public static readonly LayerView[] LayerViews = new LayerView[32];
		
		void OnGUI ()
		{
			// -- render tool bar --
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Count Objects"))
				CountObjects();
			GUILayout.EndHorizontal();

			EditorGUILayout.HelpBox("Reserved by Unity layers", MessageType.None);
			DrawSeparator();

			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			// -- render layers --
			for (var i = 0; i < LayerViews.Length; i++)
			{
				var layer = LayerViews[i];
				if (layer!=null)
					RenderLayer(layer);
				// draw separator after unity default layers
				if (i == 7)
				{
					GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
					EditorGUILayout.HelpBox("Reserved for Game layers", MessageType.None);
				}
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
		private void RenderLayer(LayerView layerView)
		{
			var layer = layerView.Layer;
			GUILayout.BeginHorizontal();
			// -- 1 ---------------------------------------------------
			var isVisible = layer.IsVisible;
			if (GUILayout.Button( isVisible ? _visibleIcon : _invisibleIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
				layer.IsVisible = !isVisible;
			var isLock = layer.IsLocked;
			if (GUILayout.Button( isLock ? _lockIcon : _unlockIcon, _buttonStyle, _iconWidthOption, _iconHeightOption))
				layer.IsLocked = !isLock;
			// -- 2 ---------------------------------------------------
			layer.Color = EditorGUILayout.ColorField( layer.Color, _colorWidthOption);
			// -- 3 ---------------------------------------------------
#if XI_GAME_TOOL_ADD_LAYERS_ICON
			GUILayout.Box(layerView.Icon, ButtonStyle, IconWidthOption, IconHeightOption);
#endif
			// -- 4 ---------------------------------------------------
			GUILayout.Label(layer.Name, EditorStyles.largeLabel);
			// -- 5 ---------------------------------------------------
			GUILayout.Label(layerView.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
		
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Representation for single layer
		/// </summary>
		public class LayerView
		{
			public Texture Icon;
			public GameLayer Layer;
			public int Quantity;
			
			public LayerView(GameLayer layer, string iconName)
			{
				Layer = layer;
				Icon = Resources.Load<Texture>(iconName);
			}
		}
		
		public static void CountObjects()
		{
			if (LayerViews == null) return;
			var counts = GameLayersTools.CountObjectsInAllLayers();
			for (var i = 0; i < LayerViews.Length; i++)
			{
				var layer = LayerViews[i];
				if (layer != null)
					layer.Quantity = counts[i];
			}
		}
	}
}