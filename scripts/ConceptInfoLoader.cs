using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ConceptInfoLoader : VBoxContainer
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
	
	[ExportCategory("Main Content Container")]
	[Export] private VBoxContainer _content;


	public override void _Ready()
	{
		foreach (Button child in GetChildren())
		{
			// Godot won't allow '.' character in Node names so manually setting it in editor
			if (child.Name == "DotNet")
			{
				child.Text = ".NET";
				child.TooltipText = ".NET";
				child.Pressed += () => OnButtonPressed(child);
				continue;
			}
			child.Text = child.Name;
			child.TooltipText = child.Name;
			child.Pressed += () => OnButtonPressed(child);
		}
	}

	
	private async void OnButtonPressed(Button button)
	{
		if (_content.GetChildCount() > 0)
		{
			var currentConceptContentInstance = _content.GetChild(0);
			if (currentConceptContentInstance.Name == button.Name)
			{
				return;
			}
			
			await TweenConceptContentInstanceExit(currentConceptContentInstance);
		}

		AddConceptContentPage(button.Name);
	}

	
	private void AddConceptContentPage(string buttonName)
	{
		switch (buttonName)
		{ 
			case "DotNet":
				CreateConceptContentInstance(_dotnetContentScene, buttonName);
				break;
			case "Variables":
				CreateConceptContentInstance(_variablesContentScene, buttonName);
				break;
			case "Types":
				CreateConceptContentInstance(_typesContentScene, buttonName);
				break;
			case "Strings":
				CreateConceptContentInstance(_stringsContentScene, buttonName);
				break;
			case "Arrays":
				CreateConceptContentInstance(_arraysContentScene, buttonName);
				break;
			case "Switches":
				CreateConceptContentInstance(_switchesContentScene, buttonName);
				break;
			case "Loops":
				CreateConceptContentInstance(_loopsContentScene, buttonName);
				break;
			case "Accessors":
				CreateConceptContentInstance(_accessorsContentScene, buttonName);
				break;
			case "Classes":
				CreateConceptContentInstance(_classesContentScene, buttonName);
				break;
			case "Properties":
				CreateConceptContentInstance(_propertiesContentScene, buttonName);
				break;
			case "Static":
				CreateConceptContentInstance(_staticContentScene, buttonName);
				break;
			case "Inheritance":
				CreateConceptContentInstance(_inheritanceContentScene, buttonName);
				break;
			case "Polymorphism":
				CreateConceptContentInstance(_polymorphismContentScene, buttonName);
				break;
			case "Null References":
				CreateConceptContentInstance(_nullReferencesContentScene, buttonName);
				break;
			case "Memory Management":
				CreateConceptContentInstance(_memoryManagementContentScene, buttonName);
				break;
			case "Enumerations":
				CreateConceptContentInstance(_enumerationsContentScene, buttonName);
				break;
			case "Tuples":
				CreateConceptContentInstance(_tuplesContentScene, buttonName);
				break;
			case "Interfaces":
				CreateConceptContentInstance(_interfacesContentScene, buttonName);
				break;
			case "Structs":
				CreateConceptContentInstance(_structsContentScene, buttonName);
				break;
			case "Records":
				CreateConceptContentInstance(_recordsContentScene, buttonName);
				break;
			case "Generics":
				CreateConceptContentInstance(_genericsContentScene, buttonName);
				break;
		};
	}

	
	private async void CreateConceptContentInstance(PackedScene packedScene, string buttonName)
	{
		Control newConceptContentInstance = packedScene.Instantiate<Control>();
		_content.AddChild(newConceptContentInstance);
		
		//using modulation in method instead of hiding/showing because of weird flickering effect
		newConceptContentInstance.Modulate = new Color(0, 0, 0, 0);
		// setting the Title of the content page to the name of button node 
		// except for .NET as editor won't allow '.' in Node names.
		if (buttonName != "DotNet")
		{
			newConceptContentInstance.GetNode<Label>("%Title").Text = buttonName;
		}
		
		// docs suggest to await a process frame before setting scale of a control that is a child of a container
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		newConceptContentInstance.Modulate = Colors.White;
		newConceptContentInstance.Scale = new Vector2(.01f, .01f);
		newConceptContentInstance.PivotOffset = newConceptContentInstance.Size / 2;
		
		TweenConceptContentInstanceEntry(newConceptContentInstance);
	}


	private void TweenConceptContentInstanceEntry(Control conceptContentInstance)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(conceptContentInstance, "scale:x", 1.0f, .3f);
		tween.TweenProperty(conceptContentInstance, "scale:y", 1.0f, .3f);
	}
	
	
	private async Task TweenConceptContentInstanceExit(Node currentConceptContentInstance)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(currentConceptContentInstance, "scale:x", 0.01f, .3f);
		tween.TweenProperty(currentConceptContentInstance, "scale:y", 0.01f, .3f);
		await ToSignal(tween, Tween.SignalName.Finished);
		currentConceptContentInstance.QueueFree();
		await ToSignal(currentConceptContentInstance, Node.SignalName.TreeExited);
	}
}
