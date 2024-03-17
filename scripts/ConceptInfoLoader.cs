using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ConceptInfoLoader : VBoxContainer
{
	[Export] private PackedScene _dotnetContentScene;
	[Export] private PackedScene _variablesContentScene;
	[Export] private PackedScene _typesContentScene;
	
	[ExportCategory("Main Content Container")]
	[Export] private VBoxContainer _content;


	public override void _Ready()
	{
		foreach (Button child in GetChildren())
		{
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
		};
	}

	
	private async void CreateConceptContentInstance(PackedScene packedScene, string buttonName)
	{
		Control newConceptContentInstance = packedScene.Instantiate<Control>();
		_content.AddChild(newConceptContentInstance);
		
		//using modulation in method instead of hiding/showing because of weird flickering effect
		newConceptContentInstance.Modulate = new Color(0, 0, 0, 0);
		// setting the Title of the content page to the name of button node (Why type it when no need?)
		newConceptContentInstance.GetNode<Label>("%Title").Text = buttonName;
		
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
