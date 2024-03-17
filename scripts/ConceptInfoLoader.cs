using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ConceptInfoLoader : VBoxContainer
{
	[Export] private PackedScene _dotnetInfoScene;
	[Export] private PackedScene _variablesInfoScene;
	[Export] private PackedScene _typesInfoScene;
	
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
		// if there is already a info panel showing
		if (_content.GetChildCount() > 0)
		{
			var currentInfoContainer = _content.GetChild(0);
			if (currentInfoContainer.Name == button.Name)
			{
				return;
			}
			
			await TweenInfoContainerExit(currentInfoContainer);
		}

		await AddInfoContainer(button);
	}

	
	private async Task AddInfoContainer(Button button)
	{
		Control infoContainerInstance;
		switch (button.Name)
		{ 
			case "DotNet":
				infoContainerInstance = _dotnetInfoScene.Instantiate<Control>();
				_content.AddChild(infoContainerInstance);
				infoContainerInstance.GetNode<Label>("%Title").Text = button.Name;
				await ToSignal(GetTree(), "process_frame");
				infoContainerInstance.Scale = new Vector2(.01f, .01f);
				infoContainerInstance.PivotOffset = infoContainerInstance.Size / 2;
				TweenInfoContainerEntry(infoContainerInstance);
				break;
			case "Variables":
				infoContainerInstance = _variablesInfoScene.Instantiate<Control>();
				_content.AddChild(infoContainerInstance);
				infoContainerInstance.GetNode<Label>("%Title").Text = button.Name;
				await ToSignal(GetTree(), "process_frame");
				infoContainerInstance.Scale = new Vector2(.01f, .01f);
				infoContainerInstance.PivotOffset = infoContainerInstance.Size / 2;
				TweenInfoContainerEntry(infoContainerInstance);
				break;
			case "Types":
				infoContainerInstance = _typesInfoScene.Instantiate<Control>();
				_content.AddChild(infoContainerInstance);
				infoContainerInstance.GetNode<Label>("%Title").Text = button.Name;
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				infoContainerInstance.Scale = new Vector2(.01f, .01f);
				infoContainerInstance.PivotOffset = infoContainerInstance.Size / 2;
				TweenInfoContainerEntry(infoContainerInstance);
				break;
		};
	}


	private void TweenInfoContainerEntry(Control infoContainerInstance)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(infoContainerInstance, "scale:x", 1.0f, .3f);
		tween.TweenProperty(infoContainerInstance, "scale:y", 1.0f, .3f);
	}
	
	
	private async Task TweenInfoContainerExit(Node currentInfoContainer)
	{
		Tween tween = GetTree().CreateTween();
		tween.SetParallel();
		tween.TweenProperty(currentInfoContainer, "scale:x", 0.01f, .3f);
		tween.TweenProperty(currentInfoContainer, "scale:y", 0.01f, .3f);
		await ToSignal(tween, Tween.SignalName.Finished);
		currentInfoContainer.QueueFree();
		await ToSignal(currentInfoContainer, Node.SignalName.TreeExited);
	}
}
