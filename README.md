# XiGameTool _The game data tool for Unity 3D_

![](https://img.shields.io/badge/unity-2018.3%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/hww/XiGameTool/actions/workflows/ci.yml/badge.svg)](https://github.com/hww/XiGameTool/actions/workflows/ci.yml)
[![openupm](https://img.shields.io/npm/v/com.hww.xigametool?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.hww.xigametool/)
[![](https://img.shields.io/github/license/hww/XiGameTool.svg)](https://github.com/hww/XiGameTool/blob/master/LICENSE)
[![semantic-release: angular](https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release)](https://github.com/semantic-release/semantic-release)

![Title Image](Documentation/title_image.png)

Simple Unity editor extension for managing visibility of layers and categories of objects, created by [hww](https://github.com/hww)

## Introduction

The scene editor in Unity 3D has enough features to work with small scenes. But when I need to edit scenes with lots of play zones, I sometimes miss the control system for selective categories, layers or sets. This very simple plugin lets you add three additional panels to the level editor. The tool controls the visibility and color for any debug information rendering such as _gizmos_, _splines_, _colliders_ etc.  The layers managing tool has simple for layer locking

- **Visibility and color management of Unity layers** _For managing the primitives related on Unity Layers_.
- **Manage the visibility of object categories** _For managing scene data by gtoups (_camera_, _gameplay_, _battle_ etc) and by categories (_spawner_, _traversal_, _splines_ etc)_.
- **Manage the visibility of object set** _For managing the objects sets such as _scrpable, _target pointes_, etc_.

It is easy to modify tool to have your custom lists of _categories_, _subcategories_ and _selection-sets_.

In addition to management, the panels display the object's statistics. As a result of the results of the use of this extension has shown high efficiency on large commercial projects.

The diagram of classes below.

![XiGameToolDiagram](/Documentation/XiGameToolDiagram.drawio.png)

## Install

The package is available on the openupm registry. You can install it via openupm-cli.

```bash
openupm add com.hww.xigametool
```
You can also install via git url by adding this entry in your manifest.json

```bash
"com.hww.xigametool": "https://github.com/hww/XiGameTool.git#upm"
```
## TODO

- [x] Basic functionality
- [x] Configurabe and safe (no enum) the objects tagging
- [x] Update documentation
- [ ] The performance optimization
- [ ] Better redraw screen for Unity

## GamePrimitive Class

The example of primitive class below. This class associate the game object with one of selection sets and categories.

```C#
public class GamePrimitive : MonoBehaviour
{
    public string subcategoryName;            // Select the art group of this object
    public string selectionSetName;           // Select the art category of this object
    
    public Subcategory Subcategory => ...     // Get the game-type in this category
    public SelectionSet SelectionSet => ...   // Get selection-set for this primitive
    public GameLayer Layer => ...             // Get the layer of this primitive
}
``` 

The strings in the field will be associated (the references cached) with first access to a property. The editor will generate the drop down selection menu for each field. Or values could be set as thext with the inspector's developing mode.  
  
![Art Primitive Component](/Documentation/art-primitive.png)

In case when the configuration will be changed, the string will keep previous config value. The drop down menu will indicate it. It is easy to make the validation or migrations tools. So the solution is very safe for large project and team.

## Game Categories Window

To control the visibility and displaying statistics for categories and subcategories of the objects.

![Layers Window](/Documentation/categories_window.png)

## Unity Layers Window

The pannel alow makes visible or invisible the Unity layers, also it can set a layer protected or not. Additionaly it allow to change layer's color. And finaly it displays a statistics per layer.

![Layers Window](/Documentation/layers_window_colors.png)

## Selection Sets Window

The pannel alow makes visible or invisible the objet set. Additionaly it displays metrics per category.

![Categories Window](/Documentation/object_sets.png)

## Example of using

The example of drawing gizmos for your game object below.

```C#
void OnDrawGizmos()
{
    if (SelectionSet.IsVisible && Subcategory.IsVisible)
    {
        Gizmos.color = SelectionSet.Color;
        Gizmos.DrawWireSphere(transform.position, 1f);
        UnityEditor.Handles.Label(transform.position, gameObject.name);
    }
}
```

For a physical colliders there is different way in the game tool.

```C#
BoxCollider _boxCollider;
BoxCollider BoxCollider => _boxCollider ??= GetComponent<BoxCollider>();

void OnDrawGizmos()
{
    if (SelectionSet.IsVisible && Subcategory.IsVisible)
    {
        Gizmos.color = GameTool.Layers.GetColor(gameObject.layer);
        Gizmos.DrawWireCube(transform.position, BoxCollider.size);
        UnityEditor.Handles.Label(transform.position, gameObject.name);
        Gizmos.DrawIcon(transform.position, "your gizmo icon");
    }
}
```

## Per Project Settings

There is GameToolSettings asset with configuation of the tool (see below).

![Setting Form](/Documentation/tool_settings.png)

With this tool is possible to configure the next options:

- Declare the list names and icons for your game types 
- Declare the list of categories. Each will have name, icon and list of game types
- Declare the list of selection sets with name and icor for each

## Per Scene Settings

Alternatively, it is possible to place `GameToolSettingsBehaviour` on the Scene and point to one of other `GameToolSettings` assets. This is the way to have configuration per scene.
