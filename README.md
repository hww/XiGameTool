# XiGameTool _The game data tool for Unity 3D_

![](https://img.shields.io/badge/unity-2018.3%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/hww/XiGameTool/actions/workflows/ci.yml/badge.svg)](https://github.com/hww/XiGameTool/actions/workflows/ci.yml)
[![openupm](https://img.shields.io/npm/v/com.hww.xigametool?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.hww.xigametool/)
[![](https://img.shields.io/github/license/hww/XiGameTool.svg)](https://github.com/hww/XiGameTool/blob/master/LICENSE)
[![semantic-release: angular](https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release)](https://github.com/semantic-release/semantic-release)

Simple Unity editor extension for managing visibility of layers and categories of objects, created by [hww](https://github.com/hww)

## Introduction

The scene editor in Unity 3D has enough features to work with small scenes. But when I need to edit scenes with lots of play zones, I sometimes miss the control system for selective categories or layers.  This very simple plugin lets you add two additional panels to the level editor: 

- **Visibility and color management of Unity layers** _used for rendering the colliders and other DebugDraw information related on Unity Layers_.
- **Manage the visibility of object categories** _used for visibility of various data categories: cameras, spawners, splines, etc_.
- **Manage the visibility of object set** _used for visibility of various data categories: cameras, spawners, splines, etc_.

In addition to management, the panels display object statistics and control simple but clear management of layer locking. 

Similarly, you can add a visibility control panel for game zones or Unity tags. 

As a result of the results of the use of this extension has shown high efficiency on large commercial projects.

## Install

The package is available on the openupm registry. You can install it via openupm-cli.

```bash
openupm add com.hww.xigametool
```
You can also install via git url by adding this entry in your manifest.json

```bash
"com.hww.xigametool": "https://github.com/hww/XiGameTool.git#upm"
```

## ArtPrimitive Class

The example of ArtPrimitive class below. This class associate the game object with one of art groups and categories.

```C#
public class ArtPrimitive : MonoBehaviour
{
    public EArtGroup artGroup;        // Select the art group of this object
    public EArtCategory artCategory;  // Select the art category of this object

    public ArtGroup GetArtGroup()
    {
        return ArtGroups.GetGroup(artGroup);
    }
    
    public ArtCategory GetArtCategory()
    {
        return ArtGroups.GetGroup(artGroup).GetCategory(artCategory);
    }
}
``` 

![Art Primitive Component](/Documentation/art-primitive.png)

## Edit Layers Visibility

The pannel alow makes visible or invisible layers, also it can make layer protected or not. Additionaly it allow to change layer's color. And finaly it displays metrics per layer.

![Layers Window](/Documentation/layers_window.png)

## Edit Categories Visibility

The pannel alow makes visible or invisible category (or group of categories). Additionaly it displays metrics per category.

![Categories Window](/Documentation/categories_window.png)

## Change Layer Names

The enum value EGameLayer contains the names for all layers in your game.

## Access to categories settings

For each group static field in the GameGroups class.

```C#
public class ArtGroups
{
      public static Group Camera;
      public static Group Partiles;
      public static Group Sounds;
      public static Group Globals;
      public static Group Rendering;
      public static Group Gameplay;
}
```

Each group has fields per each category.

```C#
public class ArtGroup
{
      public Category ActorsSpawners;
      public Category Regions;
      public Category Splines;
      public Category FeatureOverlays;
      public Category NavShapes;
      public Category Traversal;
}
```

Example of using

```C#
private void OnDrawGizmos()
{
    var category = GetArtCategory();
    if (category.IsVisible)
    {
        var lineColor = category.GetLineColor(gameObject.layer);
        var fillColor = category.GetFillColor(gameObject.layer);
        VarpGizmos.Cylinder3D(transform.position, transform.rotation, 1f, zoneRadius, GizmoDrawAxis.Y, fillColor, lineColor);
        VarpGizmos.Label(transform.position, lineColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, name, 100);
    }
}
```

The line and fill colors will be used from category or from layers panel, depends on checkbox "User Layer Colors"

![Layers Window](/Documentation/layers_window_colors.png)

## Access to layer settings

The ArtLayers class contains settings for layers

```C#
public class ArtGroup
{
      public static readonly ArtLayer[] Layers = new ArtLayer[32];
}
```

In most cases there are no resons access to the layes settings. The layers managed directly by unity UnityEditor. 


