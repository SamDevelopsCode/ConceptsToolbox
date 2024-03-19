using Godot;
using Godot.Collections;

namespace PCC.scripts;

[GlobalClass]
public partial class ConceptScenes : Resource
{
    [Export] private PackedScene _dotnetContentScene;
    [Export] private PackedScene _variablesContentScene;
    [Export] private PackedScene _typesContentScene;
    [Export] private PackedScene _stringsContentScene;
    [Export] private PackedScene _arraysContentScene;
    [Export] private PackedScene _switchesContentScene;
    [Export] private PackedScene _loopsContentScene;
    [Export] private PackedScene _accessorsContentScene;
    [Export] private PackedScene _classesContentScene;
    [Export] private PackedScene _propertiesContentScene;
    [Export] private PackedScene _staticContentScene;
    [Export] private PackedScene _inheritanceContentScene;
    [Export] private PackedScene _polymorphismContentScene;
    [Export] private PackedScene _nullReferencesContentScene;
    [Export] private PackedScene _memoryManagementContentScene;
    [Export] private PackedScene _enumerationsContentScene;
    [Export] private PackedScene _tuplesContentScene;
    [Export] private PackedScene _interfacesContentScene;
    [Export] private PackedScene _structsContentScene;
    [Export] private PackedScene _recordsContentScene;
    [Export] private PackedScene _genericsContentScene;

    public Dictionary<string, PackedScene> ContentScenesDictionary = new();

   public void Initialize()
    {
        ContentScenesDictionary["DotNet"] = _dotnetContentScene;
        ContentScenesDictionary["Variables"] = _variablesContentScene;
        ContentScenesDictionary["Types"] = _typesContentScene;
        ContentScenesDictionary["Strings"] = _stringsContentScene;
        ContentScenesDictionary["Arrays"] = _arraysContentScene;
        ContentScenesDictionary["Switches"] = _switchesContentScene;
        ContentScenesDictionary["Loops"] = _loopsContentScene;
        ContentScenesDictionary["Accessors"] = _accessorsContentScene;
        ContentScenesDictionary["Classes"] = _classesContentScene;
        ContentScenesDictionary["Properties"] = _propertiesContentScene;
        ContentScenesDictionary["Static"] = _staticContentScene;
        ContentScenesDictionary["Inheritance"] = _inheritanceContentScene;
        ContentScenesDictionary["Polymorphism"] = _polymorphismContentScene;
        ContentScenesDictionary["Null References"] = _nullReferencesContentScene;
        ContentScenesDictionary["Memory Management"] = _memoryManagementContentScene;
        ContentScenesDictionary["Enumerations"] = _enumerationsContentScene;
        ContentScenesDictionary["Tuples"] = _tuplesContentScene;
        ContentScenesDictionary["Interfaces"] = _interfacesContentScene;
        ContentScenesDictionary["Structs"] = _structsContentScene;
        ContentScenesDictionary["Records"] = _recordsContentScene;
        ContentScenesDictionary["Generics"] = _genericsContentScene;
    } 
}