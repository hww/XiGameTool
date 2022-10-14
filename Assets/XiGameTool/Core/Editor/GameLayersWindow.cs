/* Copyright(c) 2021 Valeriya Pudova(hww.github.io) read more at the license file  */


using UnityEditor;
using UnityEngine;

namespace XiGameTool.Core.Editor
{
    /// <summary>Form for viewing the game layers.</summary>
	public class GameLayersWindow : EditorWindow {

		[MenuItem("Xi/Window/Unity Layers")]
		public static void ShowWindow ()
		{
			GetWindow<GameLayersWindow>("XiGameTool: Unity Layers");
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
			_lockIcon = Resources.Load<Texture>("XiGameTool/Images/ui_lock");
			_unlockIcon = Resources.Load<Texture>("XiGameTool/Images/ui_unlock");		
			_layerImage = Resources.Load<Texture>("XiGameTool/Images/ui_layer");

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
		
        /// <summary>Called for rendering and handling GUI events.</summary>
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

			EditorGUILayout.HelpBox("Reserved by Unity layers", MessageType.None);
			DrawSeparator();

			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
			// -- render layers --
			var layers = GameTool.Layers.AllLayers;
			for (var i = 0; i < layers.Length; i++)
			{
				var layer = layers[i];
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
        /// <summary>Draw a space separator.</summary>
		private void DrawSeparator()
		{
			GUILayout.Box(string.Empty, GUILayout.Height(2f), GUILayout.ExpandWidth(true));
		}

        ///--------------------------------------------------------------------
        /// <summary>Render single line.</summary>
        ///
        /// <param name="layer">The layer.</param>
        ///--------------------------------------------------------------------

		private void RenderLayer(GameLayer layer)
		{
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
			GUILayout.Label(layer.Quantity.ToString(), EditorStyles.boldLabel, _quantityWidthOption);
		
			GUILayout.EndHorizontal();
		}

		
        /// <summary>Count objects.</summary>
		public static void CountObjects()
		{
			if (GameTool.Layers.AllLayers == null) return;
			var counts = GameLayersTools.CountObjectsInAllLayers();
			SceneView.RepaintAll();
		}
	}
}