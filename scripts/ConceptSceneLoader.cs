using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PCC.scripts;

public partial class ConceptSceneLoader : VBoxContainer
{
	[Export] private ConceptScenes _conceptScenes;
	
	[ExportCategory("Main Content Container")]
	[Export] private VBoxContainer _contentContainer;

	private bool _transitioningContentScenes = false;

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
		
		_conceptScenes.Initialize();
	}

	
	private async void OnButtonPressed(Button button)
	{
		if (_transitioningContentScenes) return;
		
		if (_contentContainer.GetChildCount() > 0)
		{
			var currentConceptContentInstance = _contentContainer.GetChild(0);

			if (currentConceptContentInstance.Name == button.Name)
			{
				return;
			}
			
			await TweenConceptContentInstanceExit(currentConceptContentInstance);
		}

		AddConceptContentPage(_conceptScenes.ContentScenesDictionary[button.Name], button.Name);
	}

	
	private async void AddConceptContentPage(PackedScene packedScene, string buttonName)
	{
		Control newConceptContentInstance = packedScene.Instantiate<Control>();
		_contentContainer.AddChild(newConceptContentInstance);
		
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
		_transitioningContentScenes = true;
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(currentConceptContentInstance, "scale:x", 0.01f, .3f);
		tween.TweenProperty(currentConceptContentInstance, "scale:y", 0.01f, .3f);
		await ToSignal(tween, Tween.SignalName.Finished);
		
		currentConceptContentInstance.QueueFree();
		
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		_transitioningContentScenes = false;
	}
}